using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GuardPrototype;

namespace GuardPrototype
{
    public class HikvisionFrameFetcher
    {
        private readonly string _snapshotUrl;
        private readonly string _username;
        private readonly string _password;

        public HikvisionFrameFetcher(string snapshotUrl, string username, string password)
        {
            _snapshotUrl = snapshotUrl;
            _username = username;
            _password = password;
        }

        public async Task<byte[]> GetLatestFrameAsync()
        {
            try
            {
                var client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var bytes = await client.GetByteArrayAsync(_snapshotUrl);
                if (bytes == null || bytes.Length == 0)
                    throw new Exception("Camera returned empty frame");
                return bytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Camera] Error fetching frame: {ex.Message}");
                return Array.Empty<byte>();
            }
        }
    }

    public class OpenAICameraModel
    {
        private readonly string _openAiEndpoint;
        private readonly string _apiKey;

        public OpenAICameraModel(string openAiEndpoint, string apiKey)
        {
            _openAiEndpoint = openAiEndpoint;
            _apiKey = apiKey;
        }

        public async Task<string> PredictHazardAsync(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return "Invalid image";

            var base64Image = Convert.ToBase64String(imageBytes);
            var payload = new
            {
                model = "gpt-4-vision-preview",
                messages = new[]
                {
                    new {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = "Are there any hazards about to occur?" },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                        }
                    }
                },
                max_tokens = 300
            };

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                var response = await client.PostAsync(_openAiEndpoint,
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"OpenAI error: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0 &&
                    choices[0].TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var content))
                {
                    return content.GetString();
                }

                return "No hazard detected";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenAI Vision] Error: {ex.Message}");
                return "Error analyzing image";
            }
        }
    }

    public class OpenAIActionsModel
    {
        private readonly string _openAiEndpoint;
        private readonly string _apiKey;

        public OpenAIActionsModel(string openAiEndpoint, string apiKey)
        {
            _openAiEndpoint = openAiEndpoint;
            _apiKey = apiKey;
        }

        public async Task<string> DecideActionAsync(string hazard, string[] devices)
        {
            var prompt = $"Hazard detected: {hazard}\nAvailable devices: {string.Join(", ", devices)}\nWhat action should be taken?";
            var payload = new
            {
                model = "gpt-5",
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 200
            };

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                var response = await client.PostAsync(_openAiEndpoint,
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"OpenAI error: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0 &&
                    choices[0].TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var content))
                {
                    return content.GetString();
                }

                return "No action suggested";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenAI Actions] Error: {ex.Message}");
                return "Error deciding action";
            }
        }
    }

    public class OpenAIRecommendations
    {
        private readonly string _openAiEndpoint;
        private readonly string _apiKey;

        public OpenAIRecommendations(string openAiEndpoint, string apiKey)
        {
            _openAiEndpoint = openAiEndpoint;
            _apiKey = apiKey;
        }

        public async Task<string> GetRecommendationAsync(string hazard, string action)
        {
            var prompt = $"Hazard: {hazard}\nAction taken: {action}\nWhat recommendation should be logged for future prevention?";
            var payload = new
            {
                model = "gpt-5",
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 150
            };

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                var response = await client.PostAsync(_openAiEndpoint,
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"OpenAI error: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0 &&
                    choices[0].TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var content))
                {
                    return content.GetString();
                }

                return "No recommendation available";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenAI Recommendations] Error: {ex.Message}");
                return "Error generating recommendation";
            }
        }
    }

    public class DeviceFetcher
    {
        private readonly string _baseUrl;

        public DeviceFetcher(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<string[]> GetDevicesAsync(int roomId)
        {
            var client = new HttpClient();
            try
            {
                var irResponse = await client.GetStringAsync($"{_baseUrl}/api/irblasters/{roomId}/devices");
                var plugResponse = await client.GetStringAsync($"{_baseUrl}/api/smartplugs/{roomId}");

                var irDevices = JsonSerializer.Deserialize<string[]>(irResponse) ?? Array.Empty<string>();
                var plugs = JsonSerializer.Deserialize<string[]>(plugResponse) ?? Array.Empty<string>();

                return irDevices.Concat(plugs).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeviceFetcher] Error: {ex.Message}");
                return Array.Empty<string>();
            }
        }
    }

    public class DeviceExecutor
    {
        public void ExecuteIR(string ip, string hex)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"discover_rm3.py {ip} {hex}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var process = Process.Start(psi);
                if (process == null)
                    throw new Exception("Failed to start Python process");

                process.WaitForExit(5000); // timeout
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                    Console.WriteLine($"[IR] Python error: {error}");
                else
                    Console.WriteLine($"[IR] Python output: {output}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IR] Execution failed: {ex.Message}");
            }
        }

        public async Task<bool> ExecuteTuyaAsync(string accessToken, string deviceId, bool turnOn, string clientId, string clientSecret, string region)
        {
            try
            {
                var url = $"https://openapi.tuya{region}.com/v1.0/devices/{deviceId}/commands";
                var bodyJson = JsonSerializer.Serialize(new
                {
                    commands = new[] { new { code = "switch_1", value = turnOn } }
                });

                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                string nonce = Guid.NewGuid().ToString();
                string bodyHash = Sha256Hex(bodyJson);
                string stringToSign = $"POST\n{bodyHash}\n\n/v1.0/devices/{deviceId}/commands";
                string sign = HmacSha256Hex(clientSecret, clientId + timestamp + nonce + stringToSign).ToUpper();

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
                };

                request.Headers.Add("client_id", clientId);
                request.Headers.Add("access_token", accessToken);
                request.Headers.Add("t", timestamp.ToString());
                request.Headers.Add("nonce", nonce);
                request.Headers.Add("sign", sign);
                request.Headers.Add("sign_method", "HMAC-SHA256");

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Tuya] API call failed: {response.StatusCode} - {error}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Tuya] Execution error: {ex.Message}");
                return false;
            }
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
namespace GuardPrototype
{
    public static class LoggerService
    {
        private static readonly string _baseUrl = "https://localhost:7169";

        public static async Task LogHazardAsync(int userId, int roomId, string hazard)
        {
            var dto = new { UserId = userId, RoomId = roomId, Description = hazard };
            var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{_baseUrl}/api/hazards",
                    new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Logger] Hazard log failed: {response.StatusCode} - {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logger] Hazard logging error: {ex.Message}");
            }
        }

        public static async Task LogRecommendationAsync(int userId, int roomId, string recommendation)
        {
            var dto = new { UserId = userId, RoomId = roomId, Text = recommendation };
            var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{_baseUrl}/api/recommendations",
                    new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[Logger] Recommendation log failed: {response.StatusCode} - {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logger] Recommendation logging error: {ex.Message}");
            }
        }
    }
}
