using System;
using System.Diagnostics;
using System.IO;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Exceptions;

namespace SimpleCL.Client
{
    public class SilkroadClient
    {
        private readonly Process _clientProcess;

        public SilkroadClient(string path, Locale srLocale)
        {
            _clientProcess = new Process
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
            if (!File.Exists(_clientProcess.StartInfo.FileName))
            {
                throw new SroClientNotFoundException("SRO_Client.exe file not found");
            }

            if (!_clientProcess.Start())
            {
                throw new SroClientLaunchException("Failed to launch Silkroad client");
            }

            return _clientProcess;
        }

        public bool Kill()
        {
            if (_clientProcess == null || _clientProcess.HasExited)
            {
                return true;
            }

            try
            {
                _clientProcess.Kill();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}