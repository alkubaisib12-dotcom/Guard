using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardPrototype
{
    public class RoomSetupManager
    {
        public static List<RoomConfig> Rooms { get; } = new();

        public static void AddRoom(RoomConfig config)
        {
            Rooms.Add(config);
        }
    }

    // ✅ Corrected RoomConfig: Tuya removed, fields aligned with API DTOs
    public class RoomConfig
    {
        // Room Info
        public string RoomName { get; set; } = "";
        public int TotalDevices { get; set; }
        public int TotalSmartPlugs { get; set; }

        // Camera Setup
        public string CameraUsername { get; set; } = "";
        public string CameraPassword { get; set; } = "";
        public string CameraIpAddress { get; set; } = "";

        // Air Sensor
        public string AirSensorApiKey { get; set; } = "";

        // Shelly Devices
        public string ShellyPMMiniIp { get; set; } = "";
        public string ShellyFloodIp { get; set; } = "";

        // IR Blaster
        public int TotalDevicesBlaster { get; set; }
        public List<string> Devices { get; set; } = new();
        public string LocationInRoom { get; set; } = "";

        // Smart Plug
        public string SmartPlugDeviceId { get; set; } = "";
        public string SmartPlugRegion { get; set; } = "";
    }

    public class RoomService
    {
        public string RoomName { get; private set; }
        private readonly int _roomId;
        private readonly int _userId;
        private readonly string _openAiEndpoint;
        private readonly string _openAiKey;

        private AirQualitySensor? _airSensor;
        private PMSensor? _pmSensor;
        private WaterLeakSensor? _leakSensor;

        private AirQualityBackgroundService? _airService;
        private PMBackgroundService? _pmService;
        private WaterLeakBackgroundService? _leakService;

        private HikvisionFrameFetcher? _cameraFetcher;
        private OpenAICameraModel? _cameraModel;
        private OpenAIActionsModel? _actionsModel;
        private OpenAIRecommendations? _recommendationsModel;
        private DeviceFetcher? _deviceFetcher;
        private DeviceExecutor? _deviceExecutor;
        private CameraMonitorService? _cameraMonitor;

        public RoomService(RoomConfig config, int userId, int roomId, string openAiEndpoint, string openAiKey)
        {
            RoomName = config.RoomName;
            _userId = userId;
            _roomId = roomId;
            _openAiEndpoint = openAiEndpoint;
            _openAiKey = openAiKey;

            Initialize(config);
        }

        private void Initialize(RoomConfig config)
        {
            // Camera
            if (!string.IsNullOrWhiteSpace(config.CameraIpAddress))
            {
                string snapshotUrl = $"http://{config.CameraIpAddress}/ISAPI/Streaming/channels/101/picture";
                _cameraFetcher = new HikvisionFrameFetcher(snapshotUrl, config.CameraUsername, config.CameraPassword);
                _cameraModel = new OpenAICameraModel(_openAiEndpoint, _openAiKey);
                _actionsModel = new OpenAIActionsModel(_openAiEndpoint, _openAiKey);
                _recommendationsModel = new OpenAIRecommendations(_openAiEndpoint, _openAiKey);
                _deviceFetcher = new DeviceFetcher("https://localhost:7169");
                _deviceExecutor = new DeviceExecutor();
                _cameraMonitor = new CameraMonitorService(_cameraFetcher, _cameraModel);
            }

            // Air Sensor
            if (!string.IsNullOrWhiteSpace(config.AirSensorApiKey))
            {
                _airSensor = new AirQualitySensor(config.RoomName, config.AirSensorApiKey, "auto");
                _airService = new AirQualityBackgroundService(
                    _airSensor,
                    _cameraMonitor!,
                    _actionsModel!,
                    _recommendationsModel!,
                    _deviceFetcher!,
                    _deviceExecutor!,
                    "Filesets/air_model_standalone.onnx",
                    _userId,
                    _roomId,
                    config.RoomName
                );
            }

            // Shelly PM Mini
            if (!string.IsNullOrWhiteSpace(config.ShellyPMMiniIp))
            {
                _pmSensor = new PMSensor(config.RoomName, $"http://{config.ShellyPMMiniIp}/status");
                _pmService = new PMBackgroundService(
                    _pmSensor,
                    _cameraMonitor!,
                    _actionsModel!,
                    _recommendationsModel!,
                    _deviceFetcher!,
                    _deviceExecutor!,
                    "Filesets/pm_mini_model_real.onnx",
                    _userId,
                    _roomId,
                    config.RoomName
                );
            }

            // Shelly Flood
            if (!string.IsNullOrWhiteSpace(config.ShellyFloodIp))
            {
                _leakSensor = new WaterLeakSensor(config.RoomName, config.ShellyFloodIp);
                _leakService = new WaterLeakBackgroundService(
                    _leakSensor,
                    _cameraMonitor!,
                    _actionsModel!,
                    _recommendationsModel!,
                    _deviceFetcher!,
                    _deviceExecutor!,
                    "Filesets/water_leak_model.onnx",
                    _userId,
                    _roomId,
                    config.RoomName
                );
            }
        }

        public async Task<RoomConfig?> FetchRoomConfigAsync()
        {
            try
            {
                using var client = new HttpClient();
                string url = $"https://localhost:7169/api/setup/{_roomId}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RoomConfig>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching room config: {ex.Message}");
                return null;
            }
        }

        public async void Start()
        {
            var latestConfig = await FetchRoomConfigAsync();
            if (latestConfig != null)
            {
                RoomName = latestConfig.RoomName;
                Initialize(latestConfig);
            }

            _cameraMonitor?.Start();
            _airService?.Start();
            _pmService?.Start();
            _leakService?.Start();
        }

        public void Stop()
        {
            _airService?.Stop();
            _pmService?.Stop();
            _leakService?.Stop();
            _cameraMonitor?.Stop();
        }

        public async Task MonitorCameraAsync()
        {
            if (_cameraFetcher == null || _cameraModel == null || _actionsModel == null || _recommendationsModel == null) return;

            var frame = await _cameraFetcher.GetLatestFrameAsync();
            var hazard = await _cameraModel.PredictHazardAsync(frame);

            var devices = await _deviceFetcher!.GetDevicesAsync(_roomId);
            var action = await _actionsModel.DecideActionAsync(hazard, devices);
            var recommendation = await _recommendationsModel.GetRecommendationAsync(hazard, action);

            await LoggerService.LogHazardAsync(_userId, _roomId, hazard);
            await LoggerService.LogRecommendationAsync(_userId, _roomId, recommendation);
        }
    }
}
