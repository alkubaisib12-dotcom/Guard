using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GuardPrototype.IRessentials
{
    internal class AddDevices
    {

        public class Appliance
        {
            public string Name { get; set; }      // e.g., "TV"
            public string Type { get; set; }      // e.g., "TV", "AC"
            public string RM3Ip { get; set; }     // IP from RM3 discovery
            public string CommandHex { get; set; } // Learned IR code
        }

        public class LocalServer
        {
            private readonly HttpListener _listener = new HttpListener();
            private readonly List<Appliance> _userAppliances = new List<Appliance>();

            public LocalServer(int port = 5000)
            {
                _listener.Prefixes.Add($"http://localhost:{port}/");
            }

            public void RegisterAppliance(Appliance appliance)
            {
                _userAppliances.Add(appliance);
            }

            public async Task StartAsync()



            {
                _listener.Start();
                Console.WriteLine("Local server started on https://localhost:7169");

                while (true)
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
            }

            private void HandleRequest(HttpListenerContext context)
            {
                var request = context.Request;
                var response = context.Response;

                if (request.Url.AbsolutePath.StartsWith("/sendCommand"))
                {
                    string applianceName = request.QueryString["device"];
                    string commandHex = request.QueryString["command"];

                    var appliance = _userAppliances.FirstOrDefault(a => a.Name == applianceName);
                    if (appliance != null)
                    {
                        try
                        {
                            // Call Python script to send command
                            var scriptPath = Path.Combine(AppContext.BaseDirectory, "discover_rm3.py");

                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = "python",
                                Arguments = $"\"{scriptPath}\" {appliance.RM3Ip} {commandHex}",
                                RedirectStandardOutput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };



                            using Process process = Process.Start(psi);
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit();

                            string responseString = $"Sent command '{commandHex}' to '{applianceName}'";
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        catch (Exception ex)
                        {
                            response.StatusCode = 500;
                            byte[] buffer = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        response.StatusCode = 404;
                        byte[] buffer = Encoding.UTF8.GetBytes($"Appliance '{applianceName}' not found");
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }

                    response.OutputStream.Close();
                }
                else
                {
                    response.StatusCode = 404;
                    response.OutputStream.Close();
                }
            }
        }








    }
}
