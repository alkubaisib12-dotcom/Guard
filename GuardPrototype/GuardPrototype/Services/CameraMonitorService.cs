using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using GuardPrototype;

namespace GuardPrototype
{
    public class CameraMonitorService
    {
        private readonly HikvisionFrameFetcher _frameFetcher;
        private readonly OpenAICameraModel _cameraModel;
        private readonly int _intervalSeconds;

        private string _latestObservation = "No observation yet.";
        private bool _isRunning;

        public CameraMonitorService(HikvisionFrameFetcher frameFetcher, OpenAICameraModel cameraModel, int intervalSeconds = 30)
        {
            _frameFetcher = frameFetcher;
            _cameraModel = cameraModel;
            _intervalSeconds = intervalSeconds;
        }

        // Optional continuous scan (can be disabled if not needed)
        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;

            Device.StartTimer(TimeSpan.FromSeconds(_intervalSeconds), () =>
            {
                _ = ScanAsync();
                return _isRunning;
            });
        }

        public void Stop() => _isRunning = false;

        // Returns last cached observation (used by fallback logic)
        public async Task<string> GetLatestObservationAsync()
        {
            return _latestObservation;
        }

        // 🔥 NEW: Trigger fresh scan on demand (used by sensor services)
        public async Task<string> GetFreshObservationAsync()
        {
            try
            {
                var frame = await _frameFetcher.GetLatestFrameAsync();
                var observation = await _cameraModel.PredictHazardAsync(frame);
                _latestObservation = observation;
                Console.WriteLine($"{DateTime.Now} - Fresh Camera Observation: {observation}");
                return observation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fresh camera scan failed: {ex.Message}");
                return "Camera error";
            }
        }

        // ✅ NEW: Snapshot method for dashboard image display
        public async Task<byte[]> GetSnapshotAsync()
        {
            try
            {
                return await _frameFetcher.GetLatestFrameAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Snapshot fetch failed: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        // Background loop (optional)
        private async Task ScanAsync()
        {
            try
            {
                var frame = await _frameFetcher.GetLatestFrameAsync();
                var observation = await _cameraModel.PredictHazardAsync(frame);
                _latestObservation = observation;
                Console.WriteLine($"{DateTime.Now} - Camera Observation: {observation}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Camera scan failed: {ex.Message}");
            }
        }
    }
}
