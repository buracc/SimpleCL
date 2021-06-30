using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pk2Extractor.Database;

namespace Pk2Extractor.Api
{
    public class Pk2Extractor
    {
        private readonly string _dbPath;
        private readonly Pk2Reader _pk2Reader;

        private readonly Dictionary<string, string> _nameReferences = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _textReferences = new Dictionary<string, string>();

        private readonly Dictionary<string, string[]> _teleportData = new Dictionary<string, string[]>();
        private readonly Dictionary<string, string[]> _teleportBuildings = new Dictionary<string, string[]>();

        private const string EnabledLine = "1\t";
        private static readonly char[] DataSeparator = {'\t'};

        private readonly int _languageIndex; // TRSRO language idx, change later

        public Pk2Extractor(string pk2Path, string dbPath)
        {
            _dbPath = dbPath;
            _pk2Reader = new Pk2Reader(pk2Path);
            _languageIndex = ExtractLanguageIdx();

            Console.WriteLine(_languageIndex);

            LoadNameReferences();
            LoadTeleportData();
            LoadTextReferences();
        }

        private int ExtractLanguageIdx()
        {
            string[] lines = _pk2Reader.GetFileText("type.txt")
                .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Contains("Language"))
                {
                    string type = line.Split('=')[1].Replace("\"", "").Trim();
                    switch (type)
                    {
                        case "English":
                            return 9;
                        case "Russia":
                            return 10;
                        case "Turkey":
                            return 13;
                        case "Arabic":
                            return 15;
                        case "German":
                            return 16;
                    }
                }
            }

            return 8;
        }

        public void StoreModels()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS models")
                .ExecuteUpdate();

            string sql = "CREATE TABLE models (id INTEGER PRIMARY KEY, servername VARCHAR(64), "
                         + "name VARCHAR(64),"
                         + "tid2 INTEGER,"
                         + "tid3 INTEGER,"
                         + "tid4 INTEGER,"
                         + "hp INTEGER,"
                         + "level INTEGER"
                         + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            string name;

            var query = new QueryBuilder(_dbPath, true);

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("characterdata_")))
            {
                Console.WriteLine("Extracting: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    string line;
                    string[] data;

                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        // Data is enabled in game
                        if (line.StartsWith(EnabledLine))
                        {
                            data = line.Split(DataSeparator, StringSplitOptions.None);
                            // Extract name if has one
                            name = "";

                            if (data[5] != "xxx")
                            {
                                name = GetNameReference(data[5]);
                            }

                            if (name == "")
                            {
                                name = data[2];
                            }

                            query
                                .Query(
                                    "INSERT INTO models (id,servername,name,tid2,tid3,tid4,hp,level) VALUES (?,?,?,?,?,?,?,?)")
                                .Bind("id", data[1])
                                .Bind("servername", data[2])
                                .Bind("name", name)
                                .Bind("tid2", data[10])
                                .Bind("tid3", data[11])
                                .Bind("tid4", data[12])
                                .Bind("hp", data[59])
                                .Bind("level", data[57])
                                .ExecuteUpdate(false);
                        }
                    }
                }
            }

            query.Finish();
        }

        public void StoreItems()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS items")
                .ExecuteUpdate();

            string sql = "CREATE TABLE items ("
                         + "id INTEGER PRIMARY KEY,"
                         + "servername VARCHAR(64),"
                         + "name VARCHAR(64),"
                         + "stack INTEGER,"
                         + "tid2 INTEGER,"
                         + "tid3 INTEGER,"
                         + "tid4 INTEGER,"
                         + "level INTEGER,"
                         + "icon VARCHAR(64)"
                         + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            string name;

            var query = new QueryBuilder(_dbPath, true);

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("itemdata_")))
            {
                Console.WriteLine("Extracting: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    string line;
                    string[] data;

                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        // Data is enabled in game
                        if (line.StartsWith("1\t"))
                        {
                            data = line.Split(new[] {'\t'}, StringSplitOptions.None);
                            // Extract name if has one
                            name = "";

                            if (data[5] != "xxx")
                            {
                                name = GetNameReference(data[5]);
                            }

                            if (name == "")
                            {
                                name = data[2];
                            }

                            query.Query(
                                    "INSERT INTO items (id,servername,name,stack,tid2,tid3,tid4,level,icon) VALUES (?,?,?,?,?,?,?,?,?)")
                                .Bind("id", data[1])
                                .Bind("servername", data[2])
                                .Bind("name", name)
                                .Bind("stack", data[57])
                                .Bind("tid2", data[10])
                                .Bind("tid3", data[11])
                                .Bind("tid4", data[12])
                                .Bind("level", data[33])
                                .Bind("icon", (data.Length > 150 ? data[54] : data[50]).ToLower())
                                .ExecuteUpdate(false);
                        }
                    }
                }
            }

            query.Finish();
        }

        public void StoreTeleportBuildings()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS teleportbuildings")
                .ExecuteUpdate();

            string sql = "CREATE TABLE teleportbuildings ("
                         + "id INTEGER PRIMARY KEY,"
                         + "servername VARCHAR(64), "
                         + "name VARCHAR(64),"
                         + "tid1 INTEGER,"
                         + "tid2 INTEGER,"
                         + "tid3 INTEGER,"
                         + "tid4 INTEGER"
                         + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            string name;

            var query = new QueryBuilder(_dbPath, true);

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("teleportbuilding")))
            {
                Console.WriteLine("Extracting: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    string line;
                    string[] data;

                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        // Data is enabled in game
                        if (line.StartsWith(EnabledLine))
                        {
                            data = line.Split(DataSeparator, StringSplitOptions.None);
                            // Extract name if has one
                            name = "";

                            if (data[5] != "xxx")
                            {
                                name = GetNameReference(data[5]);
                            }

                            if (name == "")
                            {
                                name = data[2];
                            }

                            query
                                .Query(
                                    "INSERT INTO teleportbuildings (id,servername,name,tid1,tid2,tid3,tid4) VALUES(?,?,?,?,?,?,?)")
                                .Bind("id", data[1])
                                .Bind("servername", data[2])
                                .Bind("name", name)
                                .Bind("tid1", data[9])
                                .Bind("tid2", data[10])
                                .Bind("tid3", data[11])
                                .Bind("tid4", data[12])
                                .ExecuteUpdate(false);
                        }
                    }
                }
            }

            query.Finish();
        }

        public void StoreTeleportLinks()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS teleportlinks")
                .ExecuteUpdate();

            string sql = "CREATE TABLE teleportlinks ("
                         + "sourceid INTEGER,"
                         + "destinationid INTEGER,"
                         + "id INTEGER,"
                         + "servername VARCHAR(64),"
                         + "name VARCHAR(64),"
                         + "destination VARCHAR(64),"
                         + "tid1 INTEGER,"
                         + "tid2 INTEGER,"
                         + "tid3 INTEGER,"
                         + "tid4 INTEGER,"
                         + "gold INTEGER,"
                         + "level INTEGER,"
                         + "spawn_region INTEGER,"
                         + "spawn_x INTEGER,"
                         + "spawn_y INTEGER,"
                         + "spawn_z INTEGER,"
                         + "pos_region INTEGER,"
                         + "pos_x INTEGER,"
                         + "pos_y INTEGER,"
                         + "pos_z INTEGER,"
                         + "PRIMARY KEY (sourceid, destinationid)"
                         + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            string name, destination, tid1, tid2, tid3, tid4;

            var query = new QueryBuilder(_dbPath, true);

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("teleportlink")))
            {
                Console.WriteLine("Extracting: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    string line;
                    string[] data;

                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        if (line.StartsWith(EnabledLine))
                        {
                            data = line.Split(DataSeparator, StringSplitOptions.None);

                            var model = query.Query("SELECT name,tid2,tid3,tid4 FROM models WHERE id= " +
                                                    _teleportData[data[1]][3])
                                .ExecuteSelect(false);

                            if (model.Count > 0)
                            {
                                name = model[0]["name"];
                                tid1 = "1";
                                tid2 = model[0]["tid2"];
                                tid3 = model[0]["tid3"];
                                tid4 = model[0]["tid4"];
                            }
                            else
                            {
                                name = GetNameReference(_teleportData[data[1]][4]);
                                tid1 = "4";
                                tid2 = tid3 = tid4 = "0";
                            }

                            if (name == "")
                            {
                                name = _teleportData[data[1]][2];
                            }

                            var destinationTeleport = query
                                .Query("SELECT name FROM models WHERE id= " + _teleportData[data[2]][3])
                                .ExecuteSelect(false);
                            if (destinationTeleport.Count > 0)
                            {
                                destination = destinationTeleport[0]["name"];
                            }
                            else
                            {
                                destination = GetNameReference(_teleportData[data[2]][4]);
                            }

                            if (destination == "")
                            {
                                destination = _teleportData[data[2]][2];
                            }

                            query
                                .Query(
                                    "INSERT INTO teleportlinks (sourceid,destinationid,id,servername,name,destination,tid1,tid2,tid3,tid4,gold,level,spawn_region,spawn_x,spawn_y,spawn_z,pos_region,pos_x,pos_y,pos_z) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)")
                                .Bind("sourceid", data[1])
                                .Bind("destinationid", data[2])
                                .Bind("id", _teleportData[data[1]][3])
                                .Bind("servername", _teleportData[data[1]][2])
                                .Bind("name", name)
                                .Bind("destination", destination)
                                .Bind("tid1", tid1)
                                .Bind("tid2", tid2)
                                .Bind("tid3", tid3)
                                .Bind("tid4", tid4)
                                .Bind("gold", data[3])
                                .Bind("level", data[7])
                                .Bind("spawn_region", (ushort) short.Parse(_teleportData[data[1]][5]))
                                .Bind("spawn_x", int.Parse(_teleportData[data[1]][6]))
                                .Bind("spawn_y", int.Parse(_teleportData[data[1]][8]))
                                .Bind("spawn_z", int.Parse(_teleportData[data[1]][7]))
                                .Bind("pos_region", (ushort) short.Parse(_teleportData[data[2]][5]))
                                .Bind("pos_x", int.Parse(_teleportData[data[2]][6]))
                                .Bind("pos_y", int.Parse(_teleportData[data[2]][8]))
                                .Bind("pos_z", int.Parse(_teleportData[data[2]][7]))
                                .ExecuteUpdate(false);
                        }
                    }
                }
            }

            query.Query(
                "INSERT INTO teleportlinks (sourceid,destinationid,id,servername,name,destination,tid1,tid2,tid3,tid4,gold,level,spawn_region,spawn_x,spawn_y,spawn_z,pos_region,pos_x,pos_y,pos_z) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)")
                .Bind("sourceid", 0)
                .Bind("destinationid", 0)
                .Bind("id", 19076)
                .Bind("servername", "INS_QUEST_TELEPORT")
                .Bind("name", "Gap of Dimensions")
                .Bind("destination", 0)
                .Bind("tid1", 4)
                .Bind("tid2", 1)
                .Bind("tid3", 2)
                .Bind("tid4", 0)
                .Bind("gold", 0)
                .Bind("level", 0)
                .Bind("spawn_region", 0)
                .Bind("spawn_x", 0)
                .Bind("spawn_y", 0)
                .Bind("spawn_z", 0)
                .Bind("pos_region", 0)
                .Bind("pos_x", 0)
                .Bind("pos_y", 0)
                .Bind("pos_z", 0)
                .ExecuteUpdate(false);


            query.Finish();
        }

        public void StoreTextReferences()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS textuisystem")
                .ExecuteUpdate();

            string sql = "CREATE TABLE textuisystem ("
                         + "id INTEGER PRIMARY KEY,"
                         + "servername VARCHAR(64) UNIQUE,"
                         + "text VARCHAR(256)"
                         + ")";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);
            int j = 0;
            foreach (string key in _textReferences.Keys)
            {
                // INSERT
                query.Query("INSERT INTO textuisystem (id, servername, text) VALUES (?, ?, ?)")
                    .Bind("id", j++)
                    .Bind("servername", key)
                    .Bind("text", _textReferences[key])
                    .ExecuteUpdate(false);
            }

            query.Finish();
        }

        private void LoadNameReferences()
        {
            var regex = new Regex(".*\\d.*");

            string line;
            string[] data;

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata")
                .Files
                .Where(x => x.Name.Contains("textdata_") && x.Name.Contains(".txt") && regex.IsMatch(x.Name))
            )
            {
                Console.WriteLine("Extracting file: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        if (line.StartsWith(EnabledLine))
                        {
                            data = line.Split(DataSeparator, StringSplitOptions.None);

                            if (data.Length > _languageIndex && data[_languageIndex] != "0")
                            {
                                _nameReferences[data[2]] = data[_languageIndex];
                            }
                        }
                    }
                }
            }
        }

        public void AddMinimap()
        {
            string folderPath;

            // Check directory
            folderPath = "Minimap\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Get files
            Pk2Folder minimap = _pk2Reader.GetFolder("minimap");
            if (minimap != null)
            {
                ExtractAllImages(minimap, folderPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            // Check directory
            folderPath = "Minimap\\d\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Get files
            minimap = _pk2Reader.GetFolder("minimap_d");
            if (minimap != null)
            {
                ExtractAllImages(minimap, folderPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public void ExtractAllImages(Pk2Folder folder, string outPutPath, System.Drawing.Imaging.ImageFormat format)
        {
            string ext = Equals(format, System.Drawing.Imaging.ImageFormat.Jpeg) ? "jpg" : format.ToString().ToLower();
            foreach (Pk2File f in folder.Files)
            {
                // Check path if the file already exists
                string saveFilePath = Path.ChangeExtension(Path.GetFullPath(outPutPath + f.Name), ext);
                if (File.Exists(saveFilePath))
                {
                    continue;
                }

                Console.WriteLine(f.Name);

                // Convert DDJ to DDS to Bitmap
                Bitmap img = DdsReader.FromDdj(_pk2Reader.GetFileBytes(f));
                img?.Save(saveFilePath, format);
            }

            foreach (Pk2Folder f in folder.SubFolders)
            {
                ExtractAllImages(f, outPutPath, format);
            }
        }

        private void LoadTextReferences()
        {
            var regex = new Regex(".*\\d.*");

            string line;
            string text;
            string[] data;

            string[] cFormats = {"%d", "%s", "%ld", "%u", "%08x", "%I64d", "%l64d"};

            int formatCount, formatIndex;

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata")
                .Files
                .Where(x => x.Name.Contains("textuisystem_") && x.Name.Contains(".txt") && regex.IsMatch(x.Name))
            )
            {
                Console.WriteLine("Extracting file: " + pk2File.Name);

                using (StreamReader reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name)))
                {
                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        if (line.StartsWith(EnabledLine))
                        {
                            data = line.Split(DataSeparator, StringSplitOptions.None);

                            if (data.Length > _languageIndex && data[_languageIndex] != "0")
                            {
                                text = data[_languageIndex];
                                formatCount = 0;

                                for (byte i = 0; i < cFormats.Length; i++)
                                {
                                    while ((formatIndex = text.IndexOf(cFormats[i])) != -1)
                                    {
                                        text = text.Remove(formatIndex) + "{" + formatCount + "}" +
                                               text.Substring(formatIndex + cFormats[i].Length);
                                        formatCount++;
                                    }
                                }

                                _textReferences[data[1]] = text;
                            }
                        }
                    }
                }
            }
        }

        private void LoadTeleportData()
        {
            string line;
            string[] data;

            using (StreamReader reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/teleportdata.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (line.StartsWith(EnabledLine))
                    {
                        data = line.Split(DataSeparator, StringSplitOptions.None);
                        _teleportData[data[1]] = data;
                    }
                }
            }

            using (StreamReader reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/teleportbuilding.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (line.StartsWith(EnabledLine))
                    {
                        data = line.Split(DataSeparator, StringSplitOptions.None);
                        _teleportBuildings[data[1]] = data;
                    }
                }
            }
        }

        private string GetNameReference(string serverName)
        {
            if (_nameReferences.ContainsKey(serverName))
            {
                return _nameReferences[serverName];
            }

            return "";
        }

        private string GetTextReference(string textName)
        {
            if (_textReferences.ContainsKey(textName))
            {
                return _textReferences[textName];
            }

            return "";
        }
    }
}