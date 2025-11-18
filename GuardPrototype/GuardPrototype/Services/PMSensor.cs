using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardPrototype
{
    // ------------------- Shelly PM Mini JSON mapping -------------------
    public class ShellyResponse
    {
        public Meter[] meters { get; set; } = Array.Empty<Meter>();
    }

    public class Meter
    {
        public float power { get; set; }
        public float energy { get; set; }
        public float voltage { get; set; }
        public float current { get; set; }
        public bool overpower { get; set; }
        public float temperature { get; set; }
    }

    // ------------------- PMData for ONNX input -------------------
    public class PMData
    {
        public float Current { get; set; }
        public float Voltage { get; set; }
        public float Power { get; set; }
        public float Energy { get; set; }
        public float Frequency { get; set; } = 50f; // placeholder
        public bool Overpower { get; set; }
        public bool Overtemperature { get; set; }
    }

    // ------------------- Sensor API wrapper -------------------
    public class PMSensor
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public string Name { get; }

        public PMSensor(string name, string apiUrl)
        {
            Name = name;
            _apiUrl = apiUrl;
            _client = new HttpClient();
        }

        public async Task<PMData?> GetCurrentMeasurementsAsync()
        {
            try
            {
                var response = await _client.GetAsync(_apiUrl);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var shelly = JsonSerializer.Deserialize<ShellyResponse>(json);

                if (shelly?.meters.Length > 0)
                {
                    var m = shelly.meters[0];
                    return new PMData
                    {
                        Current = m.current,
                        Voltage = m.voltage,
                        Power = m.power,
                        Energy = m.energy,
                        Frequency = 50f, // PM Mini doesn’t return frequency
                        Overpower = m.overpower,
                        Overtemperature = m.temperature > 80 // example threshold
                    };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
