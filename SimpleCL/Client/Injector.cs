using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleCL.Client
{
    public class Injector
    {
        private readonly Process _srProcess;
        private readonly string _dllPath;

        public Injector(Process srProcess, string dllPath)
        {
            _srProcess = srProcess;
            _dllPath = dllPath;
        }

        public bool Inject()
        {
            var handle = OpenProcess(0x2 | 0x8 | 0x10 | 0x20 | 0x400, 1, (uint) _srProcess.Id);
            
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            var loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (loadLibraryAddr == IntPtr.Zero)
            {
                return false;
            }
            
            CreateMutex(IntPtr.Zero, false, "Silkroad Online Launcher");
            CreateMutex(IntPtr.Zero, false, "Ready");
            
            if (!File.Exists(_dllPath))
            {
                return false;
                // throw new DllNotFoundException("Dll not found");
            }

            var allocMemAddress = VirtualAllocEx(handle, (IntPtr) null, (IntPtr) _dllPath.Length,
                (uint) VaeEnums.AllocationType.MemCommit |
                (uint) VaeEnums.AllocationType.MemReserve,
                (uint) VaeEnums.ProtectionConstants.PageExecuteReadwrite);

            if (allocMemAddress == IntPtr.Zero)
            {
                return false;
            }

            var bytes = Encoding.Default.GetBytes(_dllPath);
            WriteProcessMemory(handle, allocMemAddress, bytes, (uint) bytes.Length, out var bytesWritten);

            var ipThread = CreateRemoteThread(handle, (IntPtr) null, IntPtr.Zero, loadLibraryAddr, allocMemAddress, 0,
                (IntPtr) null);

            if (ipThread == IntPtr.Zero)
            {
                return false;
            }

            var result = WaitForSingleObject(ipThread, 10000);
            if (result == 0x00000080L || result == 0x00000102L || result == 0xFFFFFFFF)
            {
                if (handle != IntPtr.Zero)
                {
                    CloseHandle(handle);
                }

                return false;
            }

            if (handle != IntPtr.Zero)
            {
                CloseHandle(handle);
            }

            return true;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, uint dwAddress, ref byte[] lpBuffer, int nSize,
            out int lpBytesRead);

        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize,
            out IntPtr iBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheiritHandle, IntPtr dwProcessId);

        public static uint Rights = 0x0010 | 0x0020 | 0x0008;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            UInt32 dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 CloseHandle(
            IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            IntPtr dwSize,
            uint flAllocationType,
            uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] buffer,
            uint size,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttribute,
            IntPtr dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        public static class VaeEnums
        {
            public enum AllocationType
            {
                MemCommit = 0x1000,
                MemReserve = 0x2000,
                MemReset = 0x80000,
            }

            public enum ProtectionConstants
            {
                PageExecute = 0X10,
                PageExecuteRead = 0X20,
                PageExecuteReadwrite = 0X40,
                PageExecuteWritecopy = 0X80,
                PageNoaccess = 0X01
            }
        }
    }
}