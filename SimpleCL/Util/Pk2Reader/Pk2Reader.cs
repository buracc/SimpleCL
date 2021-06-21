using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SilkroadSecurityApi;
using xBot.PK2Extractor.PK2ReaderAPI;

namespace SimpleCL.Util.Pk2Reader
{
    public class Pk2Reader : IDisposable
    {
        private readonly Blowfish _blowfish = new Blowfish();

        public long Size { get; private set; }
        public byte[] Key { get; private set; }
        public string AsciiKey { get; private set; }
        public string FullPath { get; }

        public SPk2Header Header { get; private set; }
        private Dictionary<string, Pk2File> _files = new Dictionary<string, Pk2File>();

        public List<Pk2File> Files
        {
            get { return new List<Pk2File>(_files.Values); }
        }

        private Dictionary<string, Pk2Folder> _folders = new Dictionary<string, Pk2Folder>();

        public List<Pk2Folder> Folders
        {
            get { return new List<Pk2Folder>(_folders.Values); }
        }

        private Pk2Folder _currentFolder;
        private Pk2Folder _mainFolder;
        private FileStream _fileStream;

        public Pk2Reader(string filePath, string blowfishKey)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File not found");
            }

            FullPath = Path.GetFullPath(filePath);

            if (blowfishKey == "")
            {
                AsciiKey = "169841";
            }
            else
            {
                AsciiKey = blowfishKey;
            }

            Key = GenerateFinalBlowfishKey(AsciiKey);

            _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Size = _fileStream.Length;

            _blowfish.Initialize(Key);
            BinaryReader reader = new BinaryReader(_fileStream);
            Header = (SPk2Header) BufferToStruct(reader.ReadBytes(256), typeof(SPk2Header));

            _currentFolder = new Pk2Folder();
            _currentFolder.Name = filePath;
            _currentFolder.Files = new List<Pk2File>();
            _currentFolder.SubFolders = new List<Pk2Folder>();

            _mainFolder = _currentFolder;
            Read(reader.BaseStream.Position, "");
        }

        private void Read(long position, string rootPath)
        {
            BinaryReader reader = new BinaryReader(_fileStream);
            reader.BaseStream.Position = position;
            List<Pk2Folder> folders = new List<Pk2Folder>();
            SPk2EntryBlock entryBlock = (SPk2EntryBlock) BufferToStruct(
                _blowfish.Decode(reader.ReadBytes(Marshal.SizeOf(typeof(SPk2EntryBlock)))), typeof(SPk2EntryBlock));

            for (int i = 0; i < 20; i++)
            {
                SPk2Entry entry = entryBlock.Entries[i];
                switch (entry.Type)
                {
                    case 0:

                        break;
                    case 1:
                        if (entry.Name != "." && entry.Name != "..")
                        {
                            Pk2Folder folder = new Pk2Folder();
                            folder.Name = entry.Name;
                            folder.Position = BitConverter.ToInt64(entry.g_Position, 0);
                            folders.Add(folder);
                            _folders[(rootPath + entry.Name).ToUpper()] = folder;
                            _currentFolder.SubFolders.Add(folder);
                        }

                        break;

                    case 2:
                        Pk2File file = new Pk2File();
                        file.Position = entry.Position;
                        file.Name = entry.Name;
                        file.Size = entry.Size;
                        file.ParentFolder = _currentFolder;
                        _files[(rootPath + entry.Name).ToUpper()] = file;
                        _currentFolder.Files.Add(file);
                        break;
                }
            }

            if (entryBlock.Entries[19].NextChain != 0)
            {
                Read(entryBlock.Entries[19].NextChain, rootPath);
            }

            foreach (Pk2Folder folder in folders)
            {
                _currentFolder = folder;
                if (folder.Files == null)
                {
                    folder.Files = new List<Pk2File>();
                }

                if (folder.SubFolders == null)
                {
                    folder.SubFolders = new List<Pk2Folder>();
                }

                Read(folder.Position, rootPath + folder.Name + "\\");
            }
        }

        public void Close()
        {
            _fileStream.Close();
        }

        public List<Pk2File> GetRootFiles()
        {
            return _mainFolder.Files;
        }

        public List<Pk2Folder> GetRootFolders()
        {
            return _mainFolder.SubFolders;
        }

        public Pk2Folder GetFolder(string folderPath)
        {
            if (folderPath == "")
            {
                return null;
            }

            folderPath = folderPath.ToUpper();
            folderPath = folderPath.Replace("/", "\\");
            if (folderPath.EndsWith("\\"))
                folderPath = folderPath.Substring(0, folderPath.Length - 1);

            Pk2Folder folder;
            _folders.TryGetValue(folderPath, out folder);
            return folder;
        }

        public Pk2File GetFile(string filePath)
        {
            if (filePath == "")
                return null;

            // Normalize to the same dictionary key path format
            filePath = filePath.ToUpper();
            filePath = filePath.Replace("/", "\\");

            Pk2File file = null;
            _files.TryGetValue(filePath, out file);
            return file;
        }

        public byte[] GetFileBytes(Pk2File file)
        {
            BinaryReader reader = new BinaryReader(_fileStream);
            reader.BaseStream.Position = file.Position;
            return reader.ReadBytes((int) file.Size);
        }

        public byte[] GetFileBytes(string path)
        {
            BinaryReader reader = new BinaryReader(_fileStream);
            Pk2File file = GetFile(path);
            reader.BaseStream.Position = file.Position;
            return reader.ReadBytes((int) file.Size);
        }

        public Stream GetFileStream(Pk2File file)
        {
            return new MemoryStream(GetFileBytes(file));
        }

        public Stream GetFileStream(string path)
        {
            return new MemoryStream(GetFileBytes(path));
        }

        public string GetFileText(Pk2File file)
        {
            byte[] tempBuffer = GetFileBytes(file);
            if (tempBuffer != null)
            {
                TextReader txtReader = new StreamReader(new MemoryStream(tempBuffer));
                return txtReader.ReadToEnd();
            }

            return null;
        }

        public string GetFileText(string path)
        {
            byte[] tempBuffer = GetFileBytes(path);
            if (tempBuffer != null)
            {
                TextReader txtReader = new StreamReader(new MemoryStream(tempBuffer));
                return txtReader.ReadToEnd();
            }

            return null;
        }

        public void ExtractFile(Pk2File file, string outputPath)
        {
            byte[] data = GetFileBytes(file);
            FileStream stream = new FileStream(outputPath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(data);
            stream.Close();
        }

        
        public string GetFileExtension(Pk2File file)
        {
            int offset = file.Name.LastIndexOf('.');
            return file.Name.Substring(offset);
        }

        
        public List<Pk2File> GetFiles(string path)
        {
            List<Pk2File> files = new List<Pk2File>();

            Pk2Folder folder;
            _folders.TryGetValue(path, out folder);

            if (folder != null)
                files.AddRange(folder.Files);
            return files;
        }
        
        public List<Pk2Folder> GetSubFolders(string path)
        {
            List<Pk2Folder> folders = new List<Pk2Folder>();

            _folders.TryGetValue(path, out var folder);

            if (folder != null)
            {
                folders.AddRange(folder.SubFolders);
            }
                
            return folders;
        }

        
        public bool FileExists(string fileName, string path)
        {
            _folders.TryGetValue(path, out var folder);

            if (folder != null)
            {
                return folder.Files.Exists(file => file.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
            }
            
            return false;
        }

        private static byte[] GenerateFinalBlowfishKey(string asciiKey)
        {
            return GenerateFinalBlowfishKey(asciiKey,
                new byte[] {0x03, 0xF8, 0xE4, 0x44, 0x88, 0x99, 0x3F, 0x64, 0xFE, 0x35});
        }

        private static byte[] GenerateFinalBlowfishKey(string asciiKey, byte[] baseKey)
        {
            byte asciiKeyLength = (byte) asciiKey.Length;

            // Max count of 56 key bytes
            if (asciiKeyLength > 56)
            {
                asciiKeyLength = 56;
            }

            // Get bytes from ascii
            byte[] aKey = Encoding.ASCII.GetBytes(asciiKey);

            // This is the Silkroad bas key used in all versions
            byte[] bKey = new byte[56];

            // Copy key to array to keep the b_key at 56 bytes. b_key has to be bigger than a_key
            // to be able to xor every index of a_key.
            Array.ConstrainedCopy(baseKey, 0, bKey, 0, baseKey.Length);

            // Their key modification algorithm for the final blowfish key
            byte[] bfKey = new byte[asciiKeyLength];
            for (byte x = 0; x < asciiKeyLength; ++x)
            {
                bfKey[x] = (byte) (aKey[x] ^ bKey[x]);
            }

            return bfKey;
        }

        object BufferToStruct(byte[] buffer, Type returnStruct)
        {
            IntPtr pointer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, pointer, buffer.Length);
            return Marshal.PtrToStructure(pointer, returnStruct);
        }

        [StructLayout(LayoutKind.Sequential, Size = 256)]
        public struct SPk2Header
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string Name;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Version;

            [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
            public byte Encryption;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] Verify;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 205)]
            public byte[] Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Size = 128)]
        public struct SPk2Entry
        {
            [MarshalAs(UnmanagedType.I1)] public byte Type; //files are 2, folger are 1, null entries re 0

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
            public string Name;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] AccessTime;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] CreateTime;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] ModifyTime;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] g_Position;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            private byte[] m_Size;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            private byte[] m_NextChain;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Padding;

            public long NextChain => BitConverter.ToInt64(m_NextChain, 0);
            public long Position => BitConverter.ToInt64(g_Position, 0);
            public uint Size => BitConverter.ToUInt32(m_Size, 0);
        }

        [StructLayout(LayoutKind.Sequential, Size = 2560)]
        public struct SPk2EntryBlock
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public readonly SPk2Entry[] Entries;
        }

        public void Dispose()
        {
            _currentFolder = null;
            _files = null;
            _fileStream = null;
            _folders = null;
            Key = null;
            AsciiKey = null;
            _mainFolder = null;
            Size = 0;
        }
    }
}