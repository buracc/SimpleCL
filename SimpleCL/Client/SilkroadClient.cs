using System;
using System.Diagnostics;
using System.IO;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Exceptions;

namespace SimpleCL.Client
{
    public class SilkroadClient
    {
        public readonly Process ClientProcess;

        public SilkroadClient(string path, Locale srLocale)
        {
            ClientProcess = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = path,
                    FileName = $"{path}/sro_client.exe",
                    Arguments = $"/{(byte) srLocale} 0 0"
                }
            };
        }

        public Process Launch()
        {
            if (!File.Exists(ClientProcess.StartInfo.FileName))
            {
                throw new SroClientNotFoundException("SRO_Client.exe file not found");
            }

            if (!ClientProcess.Start())
            {
                throw new SroClientLaunchException("Failed to launch Silkroad client");
            }

            return ClientProcess;
        }

        public bool Kill()
        {
            if (ClientProcess == null || ClientProcess.HasExited)
            {
                return true;
            }

            try
            {
                ClientProcess.Kill();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}