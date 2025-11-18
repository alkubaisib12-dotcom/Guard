using CommunityToolkit.Maui.Alerts;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuardPrototype
{
    public class PMBackgroundService
    {
        private readonly PMSensor _sensor;
        private readonly CameraMonitorService _cameraMonitor;
        private readonly OpenAIActionsModel _actionsModel;
        private readonly OpenAIRecommendations _recommendationsModel;
        private readonly DeviceFetcher _deviceFetcher;
        private readonly DeviceExecutor _deviceExecutor;
        private readonly InferenceSession _onnxSession;

        private readonly int _userId;
        private readonly int _roomId;
        private readonly string _roomName;
        private readonly int _intervalSeconds;
        private bool _isRunning;

        private readonly Queue<float> _currentHistory = new();
        private readonly Queue<float> _voltageHistory = new();
        private readonly Queue<float> _powerHistory = new();
        private const int HistoryLength = 5;

        public PMBackgroundService(
            PMSensor sensor,
            CameraMonitorService cameraMonitor,
            OpenAIActionsModel actionsModel,
            OpenAIRecommendations recommendationsModel,
            DeviceFetcher deviceFetcher,
            DeviceExecutor deviceExecutor,
            string onnxAssetPath,
            int userId,
            int roomId,
            string roomName,
            int intervalSeconds = 10)
        {
            _sensor = sensor;
            _cameraMonitor = cameraMonitor;
            _actionsModel = actionsModel;
            _recommendationsModel = recommendationsModel;
            _deviceFetcher = deviceFetcher;
            _deviceExecutor = deviceExecutor;
            _userId = userId;
            _roomId = roomId;
            _roomName = roomName;
            _intervalSeconds = intervalSeconds;

            string tempPath = Path.Combine(FileSystem.CacheDirectory, Path.GetFileName(onnxAssetPath));
            Task.Run(async () =>
            {
                using var assetStream = await FileSystem.OpenAppPackageFileAsync(onnxAssetPath);
                using var fileStream = File.Create(tempPath);
                await assetStream.CopyToAsync(fileStream);
            }).Wait();

            _onnxSession = new InferenceSession(tempPath);
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            Device.StartTimer(TimeSpan.FromSeconds(_intervalSeconds), () => { _ = MonitorAsync(); return _isRunning; });
        }

        public void Stop() => _isRunning = false;

        private async Task MonitorAsync()
        {
            var data = await _sensor.GetCurrentMeasurementsAsync();
            if (data == null) return;

            UpdateHistory(_currentHistory, data.Current);
            UpdateHistory(_voltageHistory, data.Voltage);
            UpdateHistory(_powerHistory, data.Power);

            float currentTrend = ComputeTrend(_currentHistory);
            float voltageTrend = ComputeTrend(_voltageHistory);
            float powerTrend = ComputeTrend(_powerHistory);

            var inputTensor = new DenseTensor<float>(new[] { 1, 11 });
            inputTensor[0, 0] = data.Current;
            inputTensor[0, 1] = data.Voltage;
            inputTensor[0, 2] = data.Power;
            inputTensor[0, 3] = data.Energy;
            inputTensor[0, 4] = data.Frequency;
            inputTensor[0, 5] = data.Overpower ? 1f : 0f;
            inputTensor[0, 6] = data.Overtemperature ? 1f : 0f;
            inputTensor[0, 7] = currentTrend;
            inputTensor[0, 8] = voltageTrend;
            inputTensor[0, 9] = powerTrend;
            inputTensor[0, 10] = 0f;

            var inputs = new List<NamedOnnxValue>
    {
        NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
    };

            using var results = _onnxSession.Run(inputs);
            var outputTensor = results.First().AsTensor<float>();
            int predictedIndex = Array.IndexOf(outputTensor.ToArray(), outputTensor.Max());

            string hazard = predictedIndex switch
            {
                0 => "SAFE",
                1 => "POWER_ALERT",
                2 => "DEVICE_SHUTDOWN_REQUIRED",
                _ => "SAFE"
            };

            if (hazard == "SAFE") return;

            // 🔥 Trigger camera for real-time cause
            var cause = await _cameraMonitor.GetFreshObservationAsync();

            // 🔌 Get available devices
            var devices = await _deviceFetcher.GetDevicesAsync(_roomId);

            // 🧠 Build full prompt
            var prompt = $@"
Hazard: {hazard}
Sensor readings:
- Current: {data.Current}
- Voltage: {data.Voltage}
- Power: {data.Power}
- Energy: {data.Energy}
- Frequency: {data.Frequency}
- Overpower: {data.Overpower}
- Overtemperature: {data.Overtemperature}
- Current Trend: {currentTrend}
- Voltage Trend: {voltageTrend}
- Power Trend: {powerTrend}

Camera observation:
{cause}

Available devices:
{string.Join(", ", devices)}

What action should be taken?
";

            var action = await _actionsModel.DecideActionAsync(prompt, devices);

            foreach (var device in devices)
            {
                if (action.Contains(device, StringComparison.OrdinalIgnoreCase))
                {
                    if (device.StartsWith("tuya:"))
                    {
                        // Format: "tuya:deviceId:accessToken:clientId:clientSecret:region"
                        var parts = device.Split(':');
                        if (parts.Length == 6)
                        {
                            string deviceId = parts[1];
                            string accessToken = parts[2];
                            string clientId = parts[3];
                            string clientSecret = parts[4];
                            string region = parts[5];

                            await _deviceExecutor.ExecuteTuyaAsync(accessToken, deviceId, true, clientId, clientSecret, region);
                        }
                    }
                    else if (device.StartsWith("ir:"))
                    {
                        // Format: "ir:ip:hex"
                        var parts = device.Split(':');
                        if (parts.Length == 3)
                        {
                            string ip = parts[1];
                            string hex = parts[2];

                            _deviceExecutor.ExecuteIR(ip, hex);
                        }
                    }
                }
            }


            bool handled = devices.Any(d => action.Contains(d, StringComparison.OrdinalIgnoreCase));
            if (!handled || action.Contains("user", StringComparison.OrdinalIgnoreCase))
            {
                var toast = Toast.Make($"{_roomName}: {hazard} → {action}", CommunityToolkit.Maui.Core.ToastDuration.Long);
                await toast.Show();
            }

            await LoggerService.LogHazardAsync(_userId, _roomId, hazard);
            var recommendation = await _recommendationsModel.GetRecommendationAsync(hazard, action);
            await LoggerService.LogRecommendationAsync(_userId, _roomId, recommendation);

            Console.WriteLine($"{DateTime.Now} - {_roomName}: {hazard} → {action}");
        }




        private void UpdateHistory(Queue<float> q, float value)
        {
            q.Enqueue(value);
            if (q.Count > HistoryLength) q.Dequeue();
        }

        private float ComputeTrend(Queue<float> q)
        {
            if (q.Count < 2) return 0f;
            return (q.Last() - q.First()) / q.Count;
        }
    }
}
