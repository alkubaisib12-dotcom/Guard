using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardPrototype
{
    public class WaterLeakSensor
    {
        private readonly string _deviceIp;
        private readonly HttpClient _client;

        public string RoomName { get; }

        public WaterLeakSensor(string roomName, string deviceIp)
        {
            RoomName = roomName;
            _deviceIp = deviceIp;
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public async Task<WaterLeakData?> GetCurrentStatusAsync()
        {
            try
            {
                string url = $"http://{_deviceIp}/status";
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"WaterLeakSensor: Failed to fetch status from {url}. Status code: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("WaterLeakSensor: Empty response.");
                    return null;
                }

                var data = JsonSerializer.Deserialize<WaterLeakData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WaterLeakSensor error: {ex.Message}");
                return null;
            }
        }
    }

    public class WaterLeakData
    {
        public bool Flood { get; set; }
        public BatteryData Bat { get; set; } = new BatteryData();
        public TempData Tmp { get; set; } = new TempData();
    }

    public class BatteryData
    {
        public int Value { get; set; }
    }

    public class TempData
    {
        public float Value { get; set; }
    }
}
