using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace GuardPrototype
{
    public class TuyaPlug
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _region; // e.g., "eu", "us", "cn"
        private readonly HttpClient _httpClient;

        public TuyaPlug(string clientId, string clientSecret, string region)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _region = region;
            _httpClient = new HttpClient();
        }

        // Call this to turn a plug ON or OFF
        public async Task<bool> TogglePlugAsync(string accessToken, string deviceId, bool turnOn)
        {
            string url = $"https://openapi.tuya{_region}.com/v1.0/devices/{deviceId}/commands";
            string bodyJson = JsonSerializer.Serialize(new
            {
                commands = new[]
                {
                new { code = "switch_1", value = turnOn }
            }
            });

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string nonce = Guid.NewGuid().ToString();

            string bodyHash = Sha256Hex(bodyJson);
            string stringToSign = $"POST\n{bodyHash}\n\n/v1.0/devices/{deviceId}/commands";
            string sign = HmacSha256Hex(_clientSecret, _clientId + timestamp + nonce + stringToSign).ToUpper();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            request.Headers.Add("client_id", _clientId);
            request.Headers.Add("access_token", accessToken);
            request.Headers.Add("t", timestamp.ToString());
            request.Headers.Add("nonce", nonce);
            request.Headers.Add("sign", sign);
            request.Headers.Add("sign_method", "HMAC-SHA256");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static string Sha256Hex(string input)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static string HmacSha256Hex(string key, string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

