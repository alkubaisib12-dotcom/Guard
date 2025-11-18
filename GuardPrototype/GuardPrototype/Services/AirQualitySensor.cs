using CommunityToolkit.Maui.Alerts;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.IO;

namespace GuardPrototype
{
    // ------------------- AirQualitySensor -------------------
    public class AirQualitySensor
    {
        private readonly string _apiKey;
        private readonly string _deviceId;
        private readonly HttpClient _client;

        public string RoomName { get; }

        public AirQualitySensor(string roomName, string apiKey, string deviceId)
        {
            RoomName = roomName;
            _apiKey = apiKey;
            _deviceId = deviceId;
            _client = new HttpClient();
        }

        public async Task<AirData?> GetCurrentMeasurementsAsync()
        {
            try
            {
                var response = await _client.GetAsync($"https://api.airvisual.com/v2/device/{_deviceId}?key={_apiKey}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AirData>(json);
                return data;
            }
            catch
            {
                return null;
            }
        }
    }

    public class AirData
    {
        public CurrentData Current { get; set; } = new CurrentData();
        public float AQI => Current.Pollution.Aqius;
        public float Temperature => Current.Weather.Tp;
        public float Humidity => Current.Weather.Hu;
    }

    public class CurrentData
    {
        public PollutionData Pollution { get; set; } = new PollutionData();
        public WeatherData Weather { get; set; } = new WeatherData();
    }

    public class PollutionData
    {
        public int Aqius { get; set; }
        public string Mainus { get; set; } = "";
    }

    public class WeatherData
    {
        public float Tp { get; set; }
        public int Hu { get; set; }
    }
}




