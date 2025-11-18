using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuardPrototype;

namespace GuardPrototype
{
    public class AirQualityBackgroundService
    {
        private readonly AirQualitySensor _sensor;
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

        private readonly Queue<float> _aqiHistory = new();
        private const int HistoryLength = 10;

        public AirQualityBackgroundService(
            AirQualitySensor sensor,
            CameraMonitorService cameraMonitor,
            OpenAIActionsModel actionsModel,
            OpenAIRecommendations recommendationsModel,
            DeviceFetcher deviceFetcher,
            DeviceExecutor deviceExecutor,
            string onnxAssetPath,
            int userId,
            int roomId,
            string roomName,
            int intervalSeconds = 60)
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

            Device.StartTimer(TimeSpan.FromSeconds(_intervalSeconds), () =>
            {
                _ = MonitorAsync();
                return _isRunning;
            });
        }

        public void Stop() => _isRunning = false;

        private async Task MonitorAsync()
        {
            var data = await _sensor.GetCurrentMeasurementsAsync();
            if (data == null) return;

            UpdateHistory(data.AQI);
            float trend = ComputeTrend();

            var inputTensor = new DenseTensor<float>(new[] { 1, 4 });
            inputTensor[0, 0] = data.AQI;
            inputTensor[0, 1] = trend;
            inputTensor[0, 2] = data.Temperature;
            inputTensor[0, 3] = data.Humidity;

            var inputs = new List<NamedOnnxValue>
    {
        NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
    };

            using var results = _onnxSession.Run(inputs);
            var outputTensor = results.First().AsTensor<float>();
            int predictedIndex = Array.IndexOf(outputTensor.ToArray(), outputTensor.Max());

            string hazard = predictedIndex switch
            {
                0 => "NONE",
                1 => "AIR_QUALITY_ALERT",
                2 => "AIR_QUALITY_CRITICAL",
                _ => "NONE"
            };

            if (hazard == "NONE") return;

            // 🔥 Trigger camera for real-time cause
            var cause = await _cameraMonitor.GetFreshObservationAsync();




            // 🔌 Get available devices
            var devices = await _deviceFetcher.GetDevicesAsync(_roomId);

            // 🧠 Build full prompt
            var prompt = $@"
Hazard: {hazard}
Sensor readings:
- AQI: {data.AQI}
- Temperature: {data.Temperature}
- Humidity: {data.Humidity}
- AQI Trend: {trend}

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
                        // Example format: "tuya:deviceId:accessToken:clientId:clientSecret:region"
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
                        // Example format: "ir:ip:hex"
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


            // ⚙️ Execute if device is mentioned (optional)
            bool handled = devices.Any(d => action.Contains(d, StringComparison.OrdinalIgnoreCase));

            if (!handled || action.Contains("user", StringComparison.OrdinalIgnoreCase))
            {
                var toast = Toast.Make($"{_roomName}: {hazard} → {action}", CommunityToolkit.Maui.Core.ToastDuration.Long);
                await toast.Show();
            }

            // 📝 Log everything
            await LoggerService.LogHazardAsync(_userId, _roomId, hazard);
            var recommendation = await _recommendationsModel.GetRecommendationAsync(hazard, action);
            await LoggerService.LogRecommendationAsync(_userId, _roomId, recommendation);

            Console.WriteLine($"{DateTime.Now} - {_roomName}: {hazard} → {action}");
        }


        private void UpdateHistory(float aqi)
        {
            _aqiHistory.Enqueue(aqi);
            if (_aqiHistory.Count > HistoryLength) _aqiHistory.Dequeue();
        }

        private float ComputeTrend()
        {
            if (_aqiHistory.Count < 2) return 0f;
            return (_aqiHistory.Last() - _aqiHistory.First()) / _aqiHistory.Count;
        }
    }
}
