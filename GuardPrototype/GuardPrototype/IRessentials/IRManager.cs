using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardPrototype.IRessentials
{
    internal class IRManager
    {

        public class RM3Device
        {
            public string name { get; set; }   // IP address
            public string mac { get; set; }    // MAC address
            public int type { get; set; }      // RM3 type
        }

        public class RM3Manager
        {
            public List<RM3Device> Devices { get; private set; } = new();

            public void DiscoverDevices()
            {
                string scriptPath = Path.Combine(AppContext.BaseDirectory, "discover_rm3.py");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"\"{scriptPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Devices = JsonSerializer.Deserialize<List<RM3Device>>(output);
            }
        }





    }
}
