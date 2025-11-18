using CommunityToolkit.Maui.Alerts;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuardPrototype;

namespace GuardPrototype
{
    public class WaterLeakBackgroundService
    {
        private readonly WaterLeakSensor _sensor;
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

        public WaterLeakBackgroundService(
            WaterLeakSensor sensor,
            CameraMonitorService cameraMonitor,
            OpenAIActionsModel actionsModel,
            OpenAIRecommendations recommendationsModel,
            DeviceFetcher deviceFetcher,
            DeviceExecutor deviceExecutor,
            string onnxAssetPath,
            int userId,
            int roomId,
            string roomName,
            int intervalSeconds = 30)
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
            var data = await _sensor.GetCurrentStatusAsync();
            if (data == null) return;

            var inputTensor = new DenseTensor<float>(new[] { 1, 1 });
            inputTensor[0, 0] = data.Flood ? 1f : 0f;

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", inputTensor)
            };

            using var results = _onnxSession.Run(inputs);
            var outputTensor = results.First().AsTensor<float>();
            int predictedIndex = Array.IndexOf(outputTensor.ToArray(), outputTensor.Max());

            string hazard = predictedIndex switch
            {
                0 => "SAFE",
                1 => "WATER_LEAK_DETECTED",
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
- Flood: {data.Flood}
- Battery: {data.Bat?.Value ?? 0}
- Temperature: {data.Tmp?.Value ?? 0f}

Camera observation:
{cause}

Available devices:
{string.Join(", ", devices)}

What action should be taken?
";

            var action = await _actionsModel.DecideActionAsync(prompt, devices);

            // ⚙️ Execute matching devices
            foreach (var device in devices)
            {
                if (action.Contains(device, StringComparison.OrdinalIgnoreCase))
                {
                    if (device.StartsWith("tuya:"))
                    {
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

            // 🔔 Notify user
            var toast = Toast.Make($"{_roomName}: {hazard} → {action}", CommunityToolkit.Maui.Core.ToastDuration.Long);
            await toast.Show();

            // 📝 Log everything
            await LoggerService.LogHazardAsync(_userId, _roomId, hazard);
            var recommendation = await _recommendationsModel.GetRecommendationAsync(hazard, action);
            await LoggerService.LogRecommendationAsync(_userId, _roomId, recommendation);

            Console.WriteLine($"{DateTime.Now} - {_roomName}: {hazard} → {action}");
        }
    }
}
