using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Pk2Extractor.Database;

namespace Pk2Extractor.Api
{
    public class Pk2Extractor
    {
        private readonly string _dbPath;
        private readonly Pk2Reader _pk2Reader;

        private readonly Dictionary<string, string> _nameReferences = new();
        private readonly Dictionary<string, string> _textReferences = new();

        private readonly Dictionary<string, string[]> _teleportData = new();
        private readonly Dictionary<string, string[]> _teleportBuildings = new();

        private const string EnabledLine = "1\t";
        private static readonly char[] DataSeparator = {'\t'};

        private readonly Blowfish _blowfish = new();
        private readonly int _languageIndex;

        public Pk2Extractor(string pk2Path, string dbPath, bool forceUpdate = false)
        {
            _dbPath = dbPath;
            _pk2Reader = new Pk2Reader(pk2Path);
            _languageIndex = ExtractLanguageIdx();

            if (!Outdated() && !forceUpdate)
            {
                throw new SystemException("Files are already up to date");
            }

            StoreGameVersion();
            LoadNameReferences();
            LoadTeleportData();
            LoadTextReferences();
        }

        #region Data

        private bool Outdated()
        {
            var current = GetGameVersion();
            var actual = ExtractGameVersion();
            Console.WriteLine($"Our version: {current}. Actual version: {actual}");
            return current < actual;
        }

        private int ExtractLanguageIdx()
        {
            var lines = _pk2Reader.GetFileText("type.txt")
                .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Contains("Language"))
                {
                    var type = line.Split('=')[1].Replace("\"", "").Trim();
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

        private int ExtractGameVersion()
        {
            var bytes = _pk2Reader.GetFileBytes("SV.T");
            using var data = new BinaryReader(new MemoryStream(bytes));
            var length = data.ReadUInt32();
            var version = data.ReadBytes((int) length);

            _blowfish.Initialize(new byte[] {0x53, 0x49, 0x4C, 0x4B, 0x52, 0x4F, 0x41, 0x44});
            var decoded = _blowfish.Decode(version);
            var versionString = Encoding.ASCII.GetString(decoded);
            return int.Parse(versionString);
        }

        private int GetGameVersion()
        {
            var query = new QueryBuilder(_dbPath);
            try
            {
                return int.Parse(query.Query("SELECT * FROM data WHERE k = 'version'").ExecuteSelect()[0]["v"]);
            }
            catch
            {
                return -1;
            }
        }

        private void StoreGameVersion()
        {
            try
            {
                var query = new QueryBuilder(_dbPath, true);
                query.Query("DROP TABLE IF EXISTS data")
                    .ExecuteUpdate(false);
                query.Query("CREATE TABLE data (id INTEGER NOT NULL,k VARCHAR(512),v VARCHAR(1024),PRIMARY KEY(id))")
                    .ExecuteUpdate(false);
                query.Query("INSERT INTO data (id,k,v) VALUES (0, 'version', ?)")
                    .Bind("version", ExtractGameVersion())
                    .ExecuteUpdate(false);
                query.Finish();
            }
            catch
            {
                Console.WriteLine("Failed to get extract client version");
                throw;
            }
        }

        #endregion

        #region Levels

        public void StoreLevelData()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS leveldata")
                .ExecuteUpdate();

            var sql =
                "CREATE TABLE leveldata (level INTEGER NOT NULL, player INTEGER,sp INTEGER, job INTEGER,fellow INTEGER, PRIMARY KEY(level))";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/leveldata.txt")))
            {
                string line;
                string[] data;

                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    data = line.Split(DataSeparator, StringSplitOptions.None);

                    query.Query("INSERT INTO leveldata (level, player, sp, fellow) VALUES (?, ?, ?, ?)")
                        .Bind("level", data[0])
                        .Bind("player", data[1])
                        .Bind("sp", data[2])
                        .Bind("fellow", data[9])
                        .ExecuteUpdate(false);
                }
            }

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/tradeconflict_joblevel.txt")))
            {
                string line;
                string[] data;

                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    data = line.Split(DataSeparator, StringSplitOptions.None);

                    query.Query("UPDATE leveldata SET job = " + data[1] + " WHERE level = " + data[0])
                        .ExecuteUpdate(false);
                }

                query.Finish();
            }
        }

        #endregion

        #region Text

        public void StoreTextReferences()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS textuisystem")
                .ExecuteUpdate();

            var sql = "CREATE TABLE textuisystem ("
                      + "id INTEGER PRIMARY KEY,"
                      + "servername VARCHAR(64) UNIQUE,"
                      + "text VARCHAR(256)"
                      + ")";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);
            var j = 0;
            foreach (var key in _textReferences.Keys)
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

                using (var reader =
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

                                _textReferences[data[2]] = text;
                            }
                        }
                    }
                }
            }
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

                using (var reader =
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

        #endregion

        #region Models

        public void StoreModels()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS models")
                .ExecuteUpdate();

            var sql = "CREATE TABLE models (id INTEGER PRIMARY KEY, servername VARCHAR(64), "
                      + "name VARCHAR(64),"
                      + "tid2 INTEGER,"
                      + "tid3 INTEGER,"
                      + "tid4 INTEGER,"
                      + "hp INTEGER,"
                      + "level INTEGER,"
                      + "skills VARCHAR(1024),"
                      + "rarity INTEGER"
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

                using (var reader =
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

                            var skills = "";
                            for (var skillIndex = 83; skillIndex < 93; skillIndex++)
                            {
                                if (data[skillIndex] != "0")
                                {
                                    skills += data[skillIndex] + ",";
                                }
                            }

                            skills = skills == ""
                                ? null
                                : skills.Remove(skills.LastIndexOf(",", StringComparison.Ordinal));

                            query
                                .Query(
                                    "INSERT INTO models (id,servername,name,tid2,tid3,tid4,hp,level,skills,rarity) VALUES (?,?,?,?,?,?,?,?,?,?)")
                                .Bind("id", data[1])
                                .Bind("servername", data[2])
                                .Bind("name", name)
                                .Bind("tid2", data[10])
                                .Bind("tid3", data[11])
                                .Bind("tid4", data[12])
                                .Bind("hp", data[59])
                                .Bind("level", data[57])
                                .Bind("skills", skills)
                                .Bind("rarity", data[15])
                                .ExecuteUpdate(false);
                        }
                    }
                }
            }

            query.Finish();
        }

        #endregion

        #region Items

        public void StoreItems()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS items")
                .ExecuteUpdate();

            var sql = "CREATE TABLE items ("
                      + "id INTEGER PRIMARY KEY,"
                      + "servername VARCHAR(64),"
                      + "cashitem BOOLEAN,"
                      + "name VARCHAR(64),"
                      + "stack INTEGER,"
                      + "tid2 INTEGER,"
                      + "tid3 INTEGER,"
                      + "tid4 INTEGER,"
                      + "level INTEGER,"
                      + "icon VARCHAR(64),"
                      + "skills VARCHAR(1024),"
                      + "price INTEGER"
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

                using (var reader =
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

                        if (!line.StartsWith("1\t"))
                        {
                            continue;
                        }

                        data = line.Split(new[] {'\t'}, StringSplitOptions.None);
                        name = "";

                        if (data[5] != "xxx")
                        {
                            name = GetNameReference(data[5]);
                        }

                        if (name == "")
                        {
                            name = data[2];
                        }

                        var skills1 = data[119];
                        var skills2 = data[121];

                        query.Query(
                                "INSERT INTO items (id,servername,cashitem,name,stack,tid2,tid3,tid4,level,icon,skills,price) VALUES (?,?,?,?,?,?,?,?,?,?,?,?)")
                            .Bind("id", data[1])
                            .Bind("servername", data[2])
                            .Bind("cashitem", data[7])
                            .Bind("name", name)
                            .Bind("stack", data[57])
                            .Bind("tid2", data[10])
                            .Bind("tid3", data[11])
                            .Bind("tid4", data[12])
                            .Bind("level", data[33])
                            .Bind("icon", (data.Length > 150 ? data[54] : data[50]).ToLower())
                            .Bind("skills",
                                skills1.Contains("SKILL") ? skills1 : skills2.Contains("SKILL") ? skills2 : null)
                            .Bind("price", data[26])
                            .ExecuteUpdate(false);
                    }
                }
            }

            query.Finish();
        }

        #endregion

        #region Teleports

        private void LoadTeleportData()
        {
            string line;
            string[] data;

            using (var reader =
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

            using (var reader =
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

        public void StoreTeleportBuildings()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS teleportbuildings")
                .ExecuteUpdate();

            var sql = "CREATE TABLE teleportbuildings ("
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

                using (var reader =
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

            var sql = "CREATE TABLE teleportlinks ("
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

                using (var reader =
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

                        if (!line.StartsWith(EnabledLine))
                        {
                            continue;
                        }

                        data = line.Split(DataSeparator, StringSplitOptions.None);

                        var model = query.Query("SELECT * FROM models WHERE id = " +
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
                        destination = destinationTeleport.Count > 0
                            ? destinationTeleport[0]["name"]
                            : GetNameReference(_teleportData[data[2]][4]);

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

        #endregion

        #region Icons

        public void ExtractItemIcons()
        {
            var sql = "SELECT icon FROM items GROUP BY icon";

            if (!Directory.Exists("Icon"))
            {
                Directory.CreateDirectory("Icon");
            }

            var path = Directory.GetCurrentDirectory() + "/Icon/";
            var rows = new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteSelect();

            var defaultIcons = new NameValueCollection {{"icon", "icon_default.ddj"}};
            rows.Add(defaultIcons);
            defaultIcons = new NameValueCollection {{"icon", "action\\icon_cha_auto_attack.ddj"}};
            rows.Add(defaultIcons);

            foreach (var column in rows)
            {
                var iconPath = "icon\\" + column["icon"];
                Console.WriteLine("Extracting: " + iconPath);

                var ddjFile = _pk2Reader.GetFile(iconPath);
                if (ddjFile == null)
                {
                    continue;
                }

                var saveFilePath = Path.ChangeExtension(Path.GetFullPath(path + iconPath), "png");
                if (File.Exists(saveFilePath))
                {
                    continue;
                }

                var saveFolderPath = Path.GetDirectoryName(saveFilePath);
                if (!Directory.Exists(saveFolderPath) && saveFolderPath != null)
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                var img = DdsReader.FromDdj(_pk2Reader.GetFileBytes(ddjFile));

                img.Save(saveFilePath, ImageFormat.Png);
            }
        }

        public void ExtractSkillIcons()
        {
            var sql = "SELECT icon FROM skills GROUP BY icon";

            if (!Directory.Exists("Icon"))
            {
                Directory.CreateDirectory("Icon");
            }

            var path = Directory.GetCurrentDirectory() + "/Icon/";
            var rows = new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteSelect();

            foreach (var column in rows)
            {
                var iconPath = "icon\\" + column["icon"];
                Console.WriteLine("Extracting: " + iconPath);

                var ddjFile = _pk2Reader.GetFile(iconPath);
                if (ddjFile == null)
                {
                    continue;
                }

                var saveFilePath = Path.ChangeExtension(Path.GetFullPath(path + iconPath), "png");
                if (File.Exists(saveFilePath))
                {
                    continue;
                }

                var saveFolderPath = Path.GetDirectoryName(saveFilePath);
                if (!Directory.Exists(saveFolderPath) && saveFolderPath != null)
                {
                    Directory.CreateDirectory(saveFolderPath);
                }

                var img = DdsReader.FromDdj(_pk2Reader.GetFileBytes(ddjFile));

                img.Save(saveFilePath, ImageFormat.Png);
            }
        }

        #endregion

        #region Minimap

        public void AddMinimap()
        {
            string folderPath;

            folderPath = "Minimap\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var minimap = _pk2Reader.GetFolder("minimap");
            if (minimap != null)
            {
                ExtractAllImages(minimap, folderPath, ImageFormat.Jpeg);
            }

            folderPath = "Minimap\\d\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            minimap = _pk2Reader.GetFolder("minimap_d");
            if (minimap != null)
            {
                ExtractAllImages(minimap, folderPath, ImageFormat.Jpeg);
            }
        }

        public void ExtractAllImages(Pk2Folder folder, string outPutPath, ImageFormat format)
        {
            var ext = Equals(format, ImageFormat.Jpeg) ? "jpg" : format.ToString().ToLower();
            foreach (var f in folder.Files)
            {
                // Check path if the file already exists
                var saveFilePath = Path.ChangeExtension(Path.GetFullPath(outPutPath + f.Name), ext);
                if (File.Exists(saveFilePath))
                {
                    continue;
                }

                Console.WriteLine(f.Name);

                // Convert DDJ to DDS to Bitmap
                var img = DdsReader.FromDdj(_pk2Reader.GetFileBytes(f));
                img?.Save(saveFilePath, format);
                img?.Dispose();
            }

            foreach (var f in folder.SubFolders)
            {
                ExtractAllImages(f, outPutPath, format);
            }
        }

        #endregion

        #region Masteries

        public void StoreMasteries()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS mastery")
                .ExecuteUpdate();

            var sql = "CREATE TABLE mastery ("
                      + "id INTEGER,"
                      + "group_index INTEGER,"
                      + "name VARCHAR(64),"
                      + "description VARCHAR(256),"
                      + "type VARCHAR(64),"
                      + "weapons VARCHAR(12),"
                      + "icon VARCHAR(64)"
                      + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);

            string name, desc, type;

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/skillmasterydata.txt")))
            {
                string line;
                string[] data;

                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (line.StartsWith("//"))
                    {
                        continue;
                    }

                    data = line.Split(DataSeparator, StringSplitOptions.None);
                    if (data.Length != 14 || data[3] == "xxx")
                    {
                        continue;
                    }

                    name = GetTextReference(data[3]);
                    if (name == "")
                    {
                        name = GetNameReference(data[3]);
                    }

                    if (name == "")
                    {
                        name = data[3];
                    }

                    desc = GetTextReference(data[5]);
                    if (desc == "")
                    {
                        desc = GetNameReference(data[5]);
                    }

                    if (desc == "")
                    {
                        desc = data[5];
                    }

                    type = GetTextReference(data[6]);
                    if (desc == "")
                    {
                        type = GetNameReference(data[6]);
                    }

                    if (type == "")
                    {
                        type = data[6];
                    }

                    query.Query(
                            "INSERT INTO mastery (id,group_index,name,description,type,weapons,icon) VALUES (?,?,?,?,?,?,?)")
                        .Bind("id", int.Parse(data[0]))
                        .Bind("group_index", data[1])
                        .Bind("name", name)
                        .Bind("description", desc)
                        .Bind("type", type)
                        .Bind("weapons", data[8] + "," + data[9] + "," + data[10])
                        .Bind("icon", data[11])
                        .ExecuteUpdate(false);
                }
            }

            query.Finish();
        }

        #endregion

        #region Skills

        public void StoreSkills()
        {
            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS skills")
                .ExecuteUpdate();

            var sql = "CREATE TABLE skills ("
                      + "id INTEGER PRIMARY KEY,"
                      + "group_id INTEGER,"
                      + "servername VARCHAR(64),"
                      + "name VARCHAR(64),"
                      + "casttime INTEGER,"
                      + "cooldown INTEGER,"
                      + "duration INTEGER,"
                      + "mastery INTEGER,"
                      + "skillgroup INTEGER,"
                      + "skillgroup_index INTEGER,"
                      + "sp INTEGER,"
                      + "mp INTEGER,"
                      + "level INTEGER,"
                      + "icon VARCHAR(64),"
                      + "description VARCHAR(1024),"
                      + "attributes VARCHAR(256),"
                      + "requiredlearn_1 INTEGER,"
                      + "requiredlearn_2 INTEGER,"
                      + "requiredlearn_3 INTEGER,"
                      + "requiredlearnlevel_1 INTEGER,"
                      + "requiredlearnlevel_2 INTEGER,"
                      + "requiredlearnlevel_3 INTEGER,"
                      + "weapon_1 INTEGER,"
                      + "weapon_2 INTEGER,"
                      + "target_required INTEGER,"
                      + "overlap INTEGER,"
                      + "range INTEGER"
                      + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("skilldata_")))
            {
                Console.WriteLine("Extracting: " + pk2File.Name);

                var tempFile = pk2File.Name + ".tmp";
                File.WriteAllBytes(tempFile, DecryptSkillData(_pk2Reader.GetFileStream(pk2File)));

                using (var reader =
                    new StreamReader(tempFile))
                {
                    string line;
                    string[] data;

                    while (!reader.EndOfStream)
                    {
                        if ((line = reader.ReadLine()) == null)
                        {
                            continue;
                        }

                        if (!line.StartsWith(EnabledLine))
                        {
                            continue;
                        }

                        data = line.Split(DataSeparator, StringSplitOptions.None);

                        if (data[2] == "0")
                        {
                            continue;
                        }

                        var name = "";
                        var desc = "";

                        if (data[62] != "xxx")
                        {
                            name = GetNameReference(data[62]);
                        }

                        if (data[64] != "xxx")
                        {
                            desc = GetNameReference(data[64]);
                        }

                        var duration = "0";
                        var skillParams = "";
                        var skillDatas = new List<SkillParam>();

                        for (var paramIndex = 68; paramIndex < 118; paramIndex++)
                        {
                            try
                            {
                                var param = int.Parse(data[paramIndex]);
                                if (param < 25000)
                                {
                                    continue;
                                }

                                var skilldata = (SkillParam) param;
                                skillDatas.Add(skilldata);
                                if (skilldata == SkillParam.Timed)
                                {
                                    duration = data[paramIndex + 1];
                                }

                                // Is EU class passive skill
                                if (skillDatas.Contains(SkillParam.EuPassiveSkill) &&
                                    _euClassPassiveSkills.Contains(skilldata))
                                {
                                    paramIndex += 3;
                                }
                                else if (skillDatas.Contains(SkillParam.EuPassiveSkill) &&
                                         _etcPassiveSkills.Contains(skilldata))
                                {
                                    paramIndex += 2;
                                }
                                else
                                {
                                    paramIndex += GetParamAmount(skilldata);
                                }

                                skillParams += param + ",";
                            }
                            catch
                            {
                                Console.WriteLine("failed to parse param");
                                Console.WriteLine(data[paramIndex]);
                            }
                        }

                        skillParams = skillParams == ""
                            ? null
                            : skillParams.Remove(skillParams.LastIndexOf(",", StringComparison.Ordinal));

                        var insert = "INSERT INTO skills ("
                                     + "id,"
                                     + "group_id,"
                                     + "servername,"
                                     + "name,"
                                     + "casttime,"
                                     + "cooldown,"
                                     + "duration,"
                                     + "mastery,"
                                     + "skillgroup,"
                                     + "skillgroup_index,"
                                     + "sp,"
                                     + "mp,"
                                     + "level,"
                                     + "icon,"
                                     + "description,"
                                     + "attributes,"
                                     + "requiredlearn_1,"
                                     + "requiredlearn_2,"
                                     + "requiredlearn_3,"
                                     + "requiredlearnlevel_1,"
                                     + "requiredlearnlevel_2,"
                                     + "requiredlearnlevel_3,"
                                     + "weapon_1,"
                                     + "weapon_2,"
                                     + "target_required,"
                                     + "overlap,"
                                     + "range"
                                     + ") VALUES ("
                                     + "?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?"
                                     + ")";

                        query.Query(insert)
                            .Bind("id", data[1])
                            .Bind("group_id", data[2])
                            .Bind("servername", data[5])
                            .Bind("name", name)
                            .Bind("casttime", data[13])
                            .Bind("cooldown", data[14])
                            .Bind("duration", duration)
                            .Bind("mastery", data[34])
                            .Bind("skillgroup", byte.Parse(data[59]))
                            .Bind("skillgroup_index", "-1")
                            .Bind("sp", data[46])
                            .Bind("mp", data[53])
                            .Bind("level", data[36])
                            .Bind("icon", data[61])
                            .Bind("description", desc)
                            .Bind("attributes", skillParams)
                            .Bind("requiredlearn_1", data[40])
                            .Bind("requiredlearn_2", data[41])
                            .Bind("requiredlearn_3", data[42])
                            .Bind("requiredlearnlevel_1", data[43])
                            .Bind("requiredlearnlevel_2", data[44])
                            .Bind("requiredlearnlevel_3", data[45])
                            .Bind("target_required", data[22])
                            .Bind("weapon_1", data[50])
                            .Bind("weapon_2", data[51])
                            .Bind("overlap", data[18])
                            .Bind("range", data[21])
                            .ExecuteUpdate(false);
                    }
                }

                File.Delete(tempFile);
            }

            query.Finish();
        }


        public byte[] DecryptSkillData(Stream skillDataEnc)
        {
            if (skillDataEnc.Length == 0)
            {
                return new byte[0];
            }

            byte[] hashTable1 =
            {
                0x07, 0x83, 0xBC, 0xEE, 0x4B, 0x79, 0x19, 0xB6, 0x2A, 0x53, 0x4F, 0x3A, 0xCF, 0x71, 0xE5, 0x3C,
                0x2D, 0x18, 0x14, 0xCB, 0xB6, 0xBC, 0xAA, 0x9A, 0x31, 0x42, 0x3A, 0x13, 0x42, 0xC9, 0x63, 0xFC,
                0x54, 0x1D, 0xF2, 0xC1, 0x8A, 0xDD, 0x1C, 0xB3, 0x52, 0xEA, 0x9B, 0xD7, 0xC4, 0xBA, 0xF8, 0x12,
                0x74, 0x92, 0x30, 0xC9, 0xD6, 0x56, 0x15, 0x52, 0x53, 0x60, 0x11, 0x33, 0xC5, 0x9D, 0x30, 0x9A,
                0xE5, 0xD2, 0x93, 0x99, 0xEB, 0xCF, 0xAA, 0x79, 0xE3, 0x78, 0x6A, 0xB9, 0x02, 0xE0, 0xCE, 0x8E,
                0xF3, 0x63, 0x5A, 0x73, 0x74, 0xF3, 0x72, 0xAA, 0x2C, 0x9F, 0xBB, 0x33, 0x91, 0xDE, 0x5F, 0x91,
                0x66, 0x48, 0xD1, 0x7A, 0xFD, 0x3F, 0x91, 0x3E, 0x5D, 0x22, 0xEC, 0xEF, 0x7C, 0xA5, 0x43, 0xC0,
                0x1D, 0x4F, 0x60, 0x7F, 0x0B, 0x4A, 0x4B, 0x2A, 0x43, 0x06, 0x46, 0x14, 0x45, 0xD0, 0xC5, 0x83,
                0x92, 0xE4, 0x16, 0xD0, 0xA3, 0xA1, 0x13, 0xDA, 0xD1, 0x51, 0x07, 0xEB, 0x7D, 0xCE, 0xA5, 0xDB,
                0x78, 0xE0, 0xC1, 0x0B, 0xE5, 0x8E, 0x1C, 0x7C, 0xB4, 0xDF, 0xED, 0xB8, 0x53, 0xBA, 0x2C, 0xB5,
                0xBB, 0x56, 0xFB, 0x68, 0x95, 0x6E, 0x65, 0x00, 0x60, 0xBA, 0xE3, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x9C, 0xB5, 0xD5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2E, 0x3F, 0x41, 0x56,
                0x43, 0x45, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x64, 0xBB, 0xE3, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            byte[] hashTable2 =
            {
                0x0D, 0x05, 0x90, 0x41, 0xF9, 0xD0, 0x65, 0xBF, 0xF9, 0x0B, 0x15, 0x93, 0x80, 0xFB, 0x01, 0x02,
                0xB6, 0x08, 0xC4, 0x3C, 0xC1, 0x49, 0x94, 0x4D, 0xCE, 0x1D, 0xFD, 0x69, 0xEA, 0x19, 0xC9, 0x57,
                0x9C, 0x4D, 0x84, 0x62, 0xE3, 0x67, 0xF9, 0x87, 0xF4, 0xF9, 0x93, 0xDA, 0xE5, 0x15, 0xF1, 0x4C,
                0xA4, 0xEC, 0xBC, 0xCF, 0xDD, 0xB3, 0x6F, 0x04, 0x3D, 0x70, 0x1C, 0x74, 0x21, 0x6B, 0x00, 0x71,
                0x31, 0x7F, 0x54, 0xB3, 0x72, 0x6C, 0xAA, 0x42, 0xC1, 0x78, 0x61, 0x3E, 0xD5, 0xF2, 0xE1, 0x27,
                0x36, 0x71, 0x3A, 0x25, 0x36, 0x57, 0xD1, 0xF8, 0x70, 0x86, 0xBD, 0x0E, 0x58, 0xB3, 0x76, 0x6D,
                0xC3, 0x50, 0xF6, 0x6C, 0xA0, 0x10, 0x06, 0x64, 0xA2, 0xD6, 0x2C, 0xD4, 0x27, 0x30, 0xA5, 0x36,
                0x1C, 0x1E, 0x3E, 0x58, 0x9D, 0x59, 0x76, 0x9D, 0xA7, 0x42, 0x5A, 0xF0, 0x00, 0xBC, 0x69, 0x31,
                0x40, 0x1E, 0xFA, 0x09, 0x1D, 0xE7, 0xEE, 0xE4, 0x54, 0x89, 0x36, 0x7C, 0x67, 0xC8, 0x65, 0x22,
                0x7E, 0xA3, 0x60, 0x44, 0x1E, 0xBC, 0x68, 0x6F, 0x15, 0x2A, 0xFD, 0x9D, 0x3F, 0x36, 0x6B, 0x28,
                0x06, 0x67, 0xFE, 0xC6, 0x49, 0x6B, 0x9B, 0x3F, 0x80, 0x2A, 0xD2, 0xD4, 0xD3, 0x20, 0x1B, 0x96,
                0xF4, 0xD2, 0xCA, 0x8C, 0x74, 0xEE, 0x0B, 0x6A, 0xE1, 0xE9, 0xC6, 0xD2, 0x6E, 0x33, 0x63, 0xC0,
                0xE9, 0xD0, 0x37, 0xA9, 0x3C, 0xF7, 0x18, 0xF2, 0x4A, 0x74, 0xEC, 0x41, 0x61, 0x7A, 0x19, 0x47,
                0x8F, 0xA0, 0xBB, 0x94, 0x8F, 0x3D, 0x11, 0x11, 0x26, 0xCF, 0x69, 0x18, 0x1B, 0x2C, 0x87, 0x6D,
                0xB3, 0x22, 0x6C, 0x78, 0x41, 0xCC, 0xC2, 0x84, 0xC5, 0xCB, 0x01, 0x6A, 0x37, 0x00, 0x01, 0x65,
                0x4F, 0xA7, 0x85, 0x85, 0x15, 0x59, 0x05, 0x67, 0xF2, 0x4F, 0xAB, 0xB7, 0x88, 0xFA, 0x69, 0x24,
                0x9E, 0xC6, 0x7B, 0x3F, 0xD5, 0x0E, 0x4D, 0x7B, 0xFB, 0xB1, 0x21, 0x3C, 0xB0, 0xC0, 0xCB, 0x2C,
                0xAA, 0x26, 0x8D, 0xCC, 0xDD, 0xDA, 0xC1, 0xF8, 0xCA, 0x7F, 0x6A, 0x3F, 0x2A, 0x61, 0xE7, 0x60,
                0x5C, 0xCE, 0xD3, 0x4C, 0xAC, 0x45, 0x40, 0x62, 0xEA, 0x51, 0xF1, 0x66, 0x5D, 0x2C, 0x45, 0xD6,
                0x8B, 0x7D, 0xCE, 0x9C, 0xF5, 0xBB, 0xF7, 0x52, 0x24, 0x1A, 0x13, 0x02, 0x2B, 0x00, 0xBB, 0xA1,
                0x8F, 0x6E, 0x7A, 0x33, 0xAD, 0x5F, 0xF4, 0x4A, 0x82, 0x76, 0xAB, 0xDE, 0x80, 0x98, 0x8B, 0x26,
                0x4F, 0x33, 0xD8, 0x68, 0x1E, 0xD9, 0xAE, 0x06, 0x6B, 0x7E, 0xA9, 0x95, 0x67, 0x60, 0xEB, 0xE8,
                0xD0, 0x7D, 0x07, 0x4B, 0xF1, 0xAA, 0x9A, 0xC5, 0x29, 0x93, 0x9D, 0x5C, 0x92, 0x3F, 0x15, 0xDE,
                0x48, 0xF1, 0xCA, 0xEA, 0xC9, 0x78, 0x3C, 0x28, 0x7E, 0xB0, 0x46, 0xD3, 0x71, 0x6C, 0xD7, 0xBD,
                0x2C, 0xF7, 0x25, 0x2F, 0xC7, 0xDD, 0xB4, 0x6D, 0x35, 0xBB, 0xA7, 0xDA, 0x3E, 0x3D, 0xA7, 0xCA,
                0xBD, 0x87, 0xDD, 0x9F, 0x22, 0x3D, 0x50, 0xD2, 0x30, 0xD5, 0x14, 0x5B, 0x8F, 0xF4, 0xAF, 0xAA,
                0xA0, 0xFC, 0x17, 0x3D, 0x33, 0x10, 0x99, 0xDC, 0x76, 0xA9, 0x40, 0x1B, 0x64, 0x14, 0xDF, 0x35,
                0x68, 0x66, 0x5B, 0x49, 0x05, 0x33, 0x68, 0x26, 0xC8, 0xBA, 0xD1, 0x8D, 0x39, 0x2B, 0xFB, 0x3E,
                0x24, 0x52, 0x2F, 0x9A, 0x69, 0xBC, 0xF2, 0xB2, 0xAC, 0xB8, 0xEF, 0xA1, 0x17, 0x29, 0x2D, 0xEE,
                0xF5, 0x23, 0x21, 0xEC, 0x81, 0xC7, 0x5B, 0xC0, 0x82, 0xCC, 0xD2, 0x91, 0x9D, 0x29, 0x93, 0x0C,
                0x9D, 0x5D, 0x57, 0xAD, 0xD4, 0xC6, 0x40, 0x93, 0x8D, 0xE9, 0xD3, 0x35, 0x9D, 0xC6, 0xD3, 0x00
            };
            uint key = 0x8C1F;
            var encrypted = false;

            var buffer = new byte[skillDataEnc.Length + 1];
            // Data decoded
            skillDataEnc.Read(buffer, 0, 2);
            skillDataEnc.Seek(0, SeekOrigin.Begin);


            // Check if the data is truly encoded
            if (buffer[0] == 0xE2 && buffer[1] == 0xB0)
            {
                encrypted = true;
            }

            if (encrypted)
            {
                for (var i = 0; i <= skillDataEnc.Length; i++)
                {
                    var buff = (byte) (hashTable1[key % 0xA7] - hashTable2[key % 0x1Ef]);
                    key++;
                    buffer[i] = (byte) (skillDataEnc.ReadByte() + buff);
                }
            }
            else
            {
                skillDataEnc.Read(buffer, 0, (int) skillDataEnc.Length);
            }

            return buffer;
        }

        #endregion

        // npc id, tab, slot, item id
        public void StoreShops()
        {
            var shops = new List<Shop>();

            string line;

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/refshopgroup.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);

                    var shop = new Shop
                    {
                        StoreGroupName = data[3].StartsWith("GROUP_MALL_") ? data[3].Substring(6) : data[3],
                        NpcName = data[4]
                    };

                    if (shop.StoreGroupName.Equals("xxx") || shop.NpcName.Equals("xxx"))
                    {
                        continue;
                    }

                    shops.Add(shop);
                }
            }

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/refmappingshopgroup.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);

                    foreach (var shop in shops)
                    {
                        if (shop.StoreGroupName.StartsWith("MALL"))
                        {
                            if (shop.StoreGroupName != data[3])
                            {
                                continue;
                            }

                            shop.StoreName = data[3];
                            shop.StoreGroupName = data[2];
                        }
                        else if (shop.StoreGroupName == data[2])
                        {
                            shop.StoreName = data[3];
                        }
                    }
                }
            }

            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/refmappingshopwithtab.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);

                    foreach (var shop in shops)
                    {
                        if (shop.StoreName != data[2])
                        {
                            continue;
                        }

                        var group = new Shop.Group {Name = data[3]};
                        shop.Groups.Add(group);
                    }
                }
            }

            var refShopTab = new List<string[]>();
            using (var reader =
                new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/refshoptab.txt")))
            {
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);

                    refShopTab.Add(new[] {data[3], data[4], data[5]});
                }
            }

            foreach (var shop in shops)
            {
                foreach (var group in shop.Groups)
                {
                    foreach (var t in refShopTab)
                    {
                        if (group.Name != t[1])
                        {
                            continue;
                        }

                        var tab = new Shop.Group.Tab {Name = t[0], Title = GetTextReference(t[2])};
                        group.Tabs.Add(tab);
                    }
                }
            }

            var refShopGoods = new List<string[]>();

            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("refshopgoods_")))
            {
                using var reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name));
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);
                    refShopGoods.Add(new[] {data[2], data[3], data[4]});
                }
            }

            var refScrapOfPackageItems = new Dictionary<string, string[]>();
            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("refscrapofpackageitem_")))
            {
                using var reader =
                    new StreamReader(_pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name));
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);
                    var magicOptions = new string[byte.Parse(data[7])];
                    for (byte j = 0; j < magicOptions.Length; j++)
                    {
                        magicOptions[j] = data[j + 8];
                    }

                    refScrapOfPackageItems[data[2]] = new[]
                        {data[3], data[4], data[6], string.Join(",", magicOptions)};
                }
            }

            var prices = new Dictionary<string, List<Shop.Group.Tab.Item.Price>>();
            foreach (var pk2File in _pk2Reader.GetFolder("server_dep/silkroad/textdata").Files.Where(file =>
                file.Name.Contains("refpricepolicyofitem_")))
            {
                using var reader =
                    new StreamReader(
                        _pk2Reader.GetFileStream("server_dep/silkroad/textdata/" + pk2File.Name));
                while (!reader.EndOfStream)
                {
                    if ((line = reader.ReadLine()) == null)
                    {
                        continue;
                    }

                    if (!line.StartsWith(EnabledLine))
                    {
                        continue;
                    }

                    var data = line.Split(DataSeparator, StringSplitOptions.None);

                    var packageName = data[2];
                    var price = new Shop.Group.Tab.Item.Price
                    {
                        Currency = uint.Parse(data[3]), Value = ulong.Parse(data[5])
                    };

                    var value = prices.TryGetValue(packageName, out var itemPrices);
                    if (!value)
                    {
                        prices[packageName] = new List<Shop.Group.Tab.Item.Price> {price};
                    }
                    else
                    {
                        itemPrices.Add(price);
                    }
                }
            }

            foreach (var shop in shops)
            {
                foreach (var group in shop.Groups)
                {
                    foreach (var tab in group.Tabs)
                    {
                        foreach (var goodsData in refShopGoods)
                        {
                            if (tab.Name != goodsData[0])
                            {
                                continue;
                            }

                            if (!refScrapOfPackageItems.TryGetValue(goodsData[1], out var scrapOfPackageItemData))
                            {
                                continue;
                            }

                            var item = new Shop.Group.Tab.Item
                            {
                                PackageName = goodsData[1],
                                Name = scrapOfPackageItemData[0],
                                Slot = goodsData[2],
                                Plus = scrapOfPackageItemData[1],
                                Durability = scrapOfPackageItemData[2],
                                MagicParams = scrapOfPackageItemData[3]
                            };

                            var value = prices.TryGetValue(item.PackageName, out var itemPrices);
                            if (!value)
                            {
                                continue;
                            }

                            item.Prices = itemPrices;
                            tab.Items.Add(item);
                        }
                    }
                }
            }

            new QueryBuilder(_dbPath)
                .Query("DROP TABLE IF EXISTS npcgoods")
                .ExecuteUpdate();

            var sql = "CREATE TABLE npcgoods ("
                      + "model INTEGER,"
                      + "model_servername VARCHAR(64),"
                      + "tab INTEGER,"
                      + "slot INTEGER,"
                      + "item INTEGER,"
                      + "item_servername VARCHAR(64),"
                      + "plus INTEGER,"
                      + "durability INTEGER,"
                      + "magic_params VARCHAR(256),"
                      + "prices VARCHAR(1024),"
                      + "PRIMARY KEY (model_servername,tab,slot)"
                      + ");";

            new QueryBuilder(_dbPath)
                .Query(sql)
                .ExecuteUpdate();

            var query = new QueryBuilder(_dbPath, true);

            Console.WriteLine("Storing shopdata (this will take a while)");
            foreach (var shop in shops)
            {
                var tabCount = 0;
                foreach (var t1 in shop.Groups)
                {
                    foreach (var t2 in t1.Tabs)
                    {
                        for (var i = 0; i < t2.Items.Count; i++)
                        {
                            var item = t2.Items[i];
                            var itemIds = query
                                .Query("SELECT * FROM items WHERE servername='" + item.Name + "'")
                                .ExecuteSelect(false);
                            if (itemIds.Count > 0)
                            {
                                item.Id = uint.Parse(itemIds[0]["id"]);
                            }
                            else
                            {
                                continue;
                            }

                            var npcIds = query
                                .Query("SELECT * FROM models WHERE servername='" + shop.NpcName + "'")
                                .ExecuteSelect(false);
                            if (npcIds.Count > 0)
                            {
                                shop.Id = uint.Parse(npcIds[0]["id"]);
                            }
                            else
                            {
                                continue;
                            }

                            var results = query
                                .Query("SELECT * FROM npcgoods WHERE model_servername='" + shop.NpcName +
                                       "' AND tab=" + tabCount + " AND slot=" + i)
                                .ExecuteSelect(false);
                            if (results.Count == 0)
                            {
                                // New
                                query.Query(
                                        "INSERT INTO npcgoods (model,model_servername,tab,slot,item,item_servername,plus,durability,magic_params,prices) VALUES (?,?,?,?,?,?,?,?,?,?)")
                                    .Bind("model", shop.Id)
                                    .Bind("model_servername", shop.NpcName)
                                    .Bind("tab", tabCount)
                                    .Bind("slot", i);
                            }
                            else
                            {
                                // Override
                                query.Query(
                                    "UPDATE npcgoods SET item=?,item_servername=?,plus=?,durability=?,magic_params=?,prices=? WHERE model_servername='" +
                                    shop.NpcName + "' AND tab=" + tabCount + " AND slot=" + i);
                            }

                            query
                                .Bind("item", item.Id)
                                .Bind("item_servername", item.Name)
                                .Bind("plus", item.Plus)
                                .Bind("durability", item.Durability)
                                .Bind("magic_params", item.MagicParams)
                                .Bind("prices", string.Join(",", item.Prices.Select(x => $"{x.Currency}={x.Value}")))
                                .ExecuteUpdate(false);
                        }

                        tabCount++;
                    }
                }
            }

            query.Finish();
        }

        private class Shop
        {
            public uint Id { get; set; }
            public string StoreGroupName { get; set; }
            public string StoreName { get; set; }
            public string NpcName { get; set; }
            public List<Group> Groups { get; } = new();

            public Group.Tab.Item GetItem(string packageName)
            {
                return (from grp in Groups from tab in grp.Tabs from item in tab.Items select item).FirstOrDefault(
                    item =>
                        string.Equals(item.PackageName, packageName, StringComparison.Ordinal));
            }

            internal class Group
            {
                public string Name { get; set; }
                public List<Tab> Tabs { get; }

                public Group()
                {
                    Tabs = new List<Tab>();
                }

                internal class Tab
                {
                    public string Name { get; set; }
                    public string Title { get; set; }
                    public List<Item> Items { get; }

                    public Tab()
                    {
                        Items = new List<Item>();
                    }

                    internal class Item
                    {
                        public uint Id { get; set; }
                        public string PackageName { get; set; }
                        public string Name { get; set; }
                        public string Slot { get; set; }
                        public string Plus { get; set; }
                        public string RentType { get; set; }
                        public string Durability { get; set; }
                        public string MagicParams { get; set; }
                        public List<Price> Prices { get; set; }

                        internal class Price
                        {
                            public uint Currency { get; set; }
                            public ulong Value { get; set; }

                            public override string ToString()
                            {
                                return $"{Currency}={Value}";
                            }
                        }
                    }
                }
            }
        }

        private string GetNameReference(string serverName)
        {
            return _nameReferences.ContainsKey(serverName) ? _nameReferences[serverName] : "";
        }

        private string GetTextReference(string textName)
        {
            return _textReferences.ContainsKey(textName) ? _textReferences[textName] : "";
        }

        public enum SkillParam : uint
        {
            Attack = 6386804,
            HpMpRecovery = 1751474540,
            Resurrection = 1919251317,
            DamageAbsorb = 1868849522,
            ActiveMpConsumed = 1869506150,
            RequiredItem = 1919250793,
            AreaEffect = 6645362,
            OverTime = 1886743667,
            HealWeaponReflect = 1836542056,
            Timed = 1685418593,
            AutoTransferEffect = 1701213281,
            LinkedSkill = 1819175795,
            DamageDivide = 1818977380,
            MultiHit = 28003,
            PhyMagDefenseIncrease = 1684366960,
            BlockingRatio = 25202,
            KnockDown = 27503,
            DownAttack = 25697,
            Stun = 29556,
            KnockBack = 27490,
            HpIncrease = 6844521,
            CritIncrease = 25458,
            HitRatio = 26738,
            HelperSummon = 1937075565,
            ShotRangeIncrease = 29301,
            Freeze = 26234,
            Frostbite = 26210,
            ProtectionWall = 28791,
            ElectricShock = 25971,
            DamagePercentIncrease = 6582901,
            MovementSpeed = 1752396901,
            BlinkDash = 1952803941,
            WizardTeleport = 1952803890,
            ParryRatio = 25970,
            Burn = 25205,
            CurseReduce = 1650946657,
            CurseProbabilityReduce = 1919246700,
            CurseRemove = 1668641396, // Socket stones do not have this one
            CurseRemove2 = 1668641388, // Socket stones also have this one
            BadStatusRemoveAmount = 1919120754,
            PreventAttack = 1886350433,
            ReincarnationIncrease = 1769105251,
            ConditionalCast = 1919250787,
            GetVariable = 1734702198,
            ConsumeItem = 1668182893,
            Debuff = 1952542324,
            Unk1 = 1296122196,
            Unk2 = 1650619750,
            Unk3 = 1851946342,
            ChParryDebuff = 1952805476,
            ChHitRatioDebuff = 1953002084,
            IncreaseMp = 7172201,
            NonPlayerBuff = 1667396966, // Items, Npcs, Events
            AlchemyRatioIncrease = 1634493301,
            StrIncrease = 1937011305,
            IntIncrease = 1768846441,
            MonsterCapture = 1902474100,
            Trap = 1953653104,
            GoldDropRatioIncrease = 6775922,
            Detect = 6583412,
            LuckIncrease = 1819632491,
            SkinChange = 1836278632,
            WarriorOneHand = 1160860481,
            WarriorTwoHand = 1160926017,
            WarriorAxe = 1160921416,
            WarriorAxe2 = 1160921409,
            AggroIncrease = 1953395762,
            WarriorWeaponPhyReflect = 1886876788,
            ReduceSelfPhyMagDmg = 1886217319,
            AoeDetect = 1685353584,
            TransferDamageAbsorb = 1818977394,
            AggroAbsorb = 1818976615,
            EuPassiveSkill = 1936028790,
            EuPassiveSkill2 = 1886613351,
            CritEvasion = 1684238953,
            Dull = 29548,
            DebuffedDamageIncrease = 1635017569,
            DamageReturn = 1684891506,
            IncreaseAttack = 1634754933,
            CurseSeriesReduce = 1919246708,
            Bleed = 25196,
            CrossbowDamage = 1128415572,
            CrossbowRangeIncrease = 1128419905,
            DaggerDamage = 1145520468,
            DaggerHitRate = 1145522258,
            DaggerStealthDamage = 1145520449,
            RogueStealth = 1398031445,
            Disorient = 1751741549,
            Invisible = 1751737445,
            Stealth = 1398035280,
            Cancelable = 7564131,
            TagPoint = 1752069232,
            Poison = 28787,
            RoguePoison = 1380992085,
            RoguePoison2 = 1380996181,
            PoisonImbue = 1380991573,
            MonsterMask = 1835229552,
            ReduceSelfMaxHp = 1886218352,
            ReduceSelfPhyMagDef = 1886217328,
            WizardManaAttack = 1464421700,
            WizardRangedAttack = 1464422997,
            WizardEarthDamage = 1161904468,
            WizardColdDamage = 1129267540,
            WizardFireDamage = 1179205972,
            WizardLightDamage = 1279869268,
            Root = 29300,
            Overlap = 1870031922,
            Combustion = 1668509040,
            Fear = 26213,
            WarlockDotDamage = 1146372436,
            WarlockZerkIncrease = 1146373202,
            ZerkObtainIncrease = 1752656242,
            Connection = 1818981170,
            WarlockManaUsageDecrease = 1380535620,
            Decay = 1668509796,
            Weaken = 1668509028,
            Impotent = 1668510578,
            Division = 1668508020,
            WarlockNukeDamage = 1112293716,
            WarlockTrapDamageIncrease = 1414676801,
            Hidden = 29794,
            WarlockTrap = 84545280,
            ShortSight = 28025,
            WarlockVampIncrease = 1112754256,
            WarlockTrueDamage = 1396785473,
            WarlockVamp = 1818653556,
            WarlockVampWeaponReflect = 1836542067,
            Disease = 25715,
            TrueDamage = 1885629799,
            ScreamMask = 1633840738,
            Sleep = 29541,
            Panic = 1668507760,
            MobAggroReduce = 1685352052,
            AggroWeaponReflect = 1836541044,
            Phantasma = 25325,
            Phantasma2 = 29982,
            BardDamage = 1297432916,
            BardRange = 1297433938,
            BardManaUsageDecrease = 1111772484,
            BardResistanceIncrease = 1297433426,
            BardMusic = 1935895667,
            Confusion = 25441,
            StealMp = 1684891508,
            ManaRecovery = 1836543336,
            DanceRangeIncrease = 1146307922,
            DanceResistanceIncrease = 1146307410,
            BardDance = 1919250540,
            HealingDance = 1919447669,
            ManaDance = 1684237680,
            ClericDamage = 1212957012,
            ClericHealIncrease = 1212961365,
            ClericManaUsageReduce = 1212960068,
            ClericPassive = 1919250798,
            AllyHeal = 1702062192,
            BlackResBuffApplication = 1919776116,
            BlackResHpMpReduce = 1667785586,
            BlackResBuffHpMpReduce = 1668113266,
            ClericStrIncrease = 1212958291,
            ClericIntIncrease = 1212960073,
            PhyDefenseIncrease = 1212957264,
            MagDefenseIncrease = 1212961613,
            TargetDash = 1952803891,
            Darkness = 25710,
            DamageMpAbsorption = 1684499824,
            DaggerSpeed = 1752396850,
            VipHonorBuffs = 1986164070,
            MovementIncrease = 1752396851,
            FortressRepair = 1919970164,
            StructureAoe = 1885629746,
            PetExpPotion = 1702391913,
            ManaSwitch = 1818977384,
            BicheonSkill = 1936745569,
            IgnoreDefense = 1836280164,
            SocketStoneSp = 7565417,
            SocketStoneReflect = 1651270770,
            SocketStoneStunRemove = 7236966,
            SocketStoneHeal = 7236968,
            SocketStoneFreeMp = 7172978,
            ZerkDurationIncrease = 1752656244,
            ExpSpIncrease = 1702391925,
            RamadanExpSpIncrease = 1886219632,
            JobSkill = 1785684595,
            JobPipe = 1668511085,
            TraderPouch = 1650683508,
            TransportHpIncrease = 1667788905,
            TransportSpeedIncrease = 1667789684,
            DemonBless = 1685287020,
            FellowSkill = 1885697074,
            FellowAttack = 6384748,
            FellowIncreaseAttack = 7365222,
            FellowZerk = 1752654190,
            FellowBuff = 7168614,
            FellowIncreaseSelfAttack = 1634759285,
            RedBlueFellowAttack = 1835295334,
            RedBlueFellowBuff = 7366768,
            FellowMpRecovery = 7171440,
            PartyZerkIncrease = 1752654965,
            AwesomeWorld = 2036556899,
            DanceClone = 1919968115, // Some type of dance skill clone, links to original dance skill id
            FlowerSkill = 7630179
        }

        // These have 3 params
        private readonly ImmutableList<SkillParam> _euClassPassiveSkills = ImmutableList.Create(
            SkillParam.CrossbowDamage,
            SkillParam.CrossbowRangeIncrease,
            SkillParam.DaggerDamage,
            SkillParam.DaggerHitRate,
            SkillParam.DaggerStealthDamage,
            SkillParam.RogueStealth,
            SkillParam.WizardEarthDamage,
            SkillParam.WizardColdDamage,
            SkillParam.WizardFireDamage,
            SkillParam.WizardLightDamage,
            SkillParam.WarlockDotDamage,
            SkillParam.WarlockNukeDamage,
            SkillParam.WarlockTrueDamage,
            SkillParam.BardDamage,
            SkillParam.BardRange,
            SkillParam.ClericDamage
        );

        // These have 2 params
        private readonly ImmutableList<SkillParam> _etcPassiveSkills = ImmutableList.Create(
            SkillParam.RoguePoison,
            SkillParam.RoguePoison2,
            SkillParam.PoisonImbue,
            SkillParam.WizardManaAttack,
            SkillParam.WizardRangedAttack,
            SkillParam.WarlockZerkIncrease,
            SkillParam.WarlockManaUsageDecrease,
            SkillParam.WarlockTrapDamageIncrease,
            SkillParam.WarlockVampIncrease,
            SkillParam.BardManaUsageDecrease,
            SkillParam.BardResistanceIncrease,
            SkillParam.ClericHealIncrease,
            SkillParam.ClericManaUsageReduce,
            SkillParam.ClericStrIncrease,
            SkillParam.ClericIntIncrease,
            SkillParam.PhyDefenseIncrease,
            SkillParam.MagDefenseIncrease
        );

        private int GetParamAmount(SkillParam param)
        {
            switch (param)
            {
                case SkillParam.Timed: // Duration
                case SkillParam.HelperSummon: // Duration
                case SkillParam.ChParryDebuff:
                case SkillParam.ChHitRatioDebuff:
                case SkillParam.AlchemyRatioIncrease: // % increase
                case SkillParam.GoldDropRatioIncrease: // Amount increase %
                case SkillParam.LuckIncrease: // % increase
                case SkillParam.WarriorWeaponPhyReflect: // Amount %
                case SkillParam.CritEvasion: // Amount
                case SkillParam.Overlap: // No clue, idk if this is even the correct name
                case SkillParam.ZerkObtainIncrease: // Increase %
                case SkillParam.WarlockVamp: // Amount
                case SkillParam.WarlockVampWeaponReflect: // Amount %
                case SkillParam.TrueDamage: // Damage
                case SkillParam.ScreamMask: // Distance
                case SkillParam.AggroWeaponReflect: // Amount %
                case SkillParam.Phantasma: // Unk
                case SkillParam.Phantasma2: // Unk
                case SkillParam.BardMusic: // Unk
                case SkillParam.ManaRecovery: // Weapon mag reflect %
                case SkillParam.DanceRangeIncrease: // Distance
                case SkillParam.DanceResistanceIncrease: // Amount
                case SkillParam.BardDance: // Always 1
                case SkillParam.BlackResBuffApplication: // Buff Id
                case SkillParam.BlackResHpMpReduce: // Amount %
                case SkillParam.DamageMpAbsorption: // Amount %
                case SkillParam.DaggerSpeed: // Movement speed %
                case SkillParam.MovementIncrease: // Movement speed %
                case SkillParam.StructureAoe: // Radius
                case SkillParam.IgnoreDefense: // Ignored %
                case SkillParam.SocketStoneSp: // Extra SP
                case SkillParam.ZerkDurationIncrease: // Increase %
                case SkillParam.JobSkill: // Level required
                case SkillParam.JobPipe: // Pipe level
                case SkillParam.TraderPouch: // Slots
                case SkillParam.TransportHpIncrease: // Increase %
                case SkillParam.TransportSpeedIncrease: // Increase %
                case SkillParam.DemonBless: // Unk (must be some percentage value)
                case SkillParam.FellowIncreaseSelfAttack: // Increase %
                case SkillParam.PartyZerkIncrease: // Increase %
                case SkillParam.DanceClone: // Original dance skill id
                case SkillParam.FlowerSkill: // 1
                case SkillParam.OverTime: // Time
                case SkillParam.HealWeaponReflect: // Reflect %
                    return 1;
                case SkillParam.StrIncrease: // Amount, Limit on current Str %
                case SkillParam.IntIncrease: // Amount, Limit on current Int %
                case SkillParam.IncreaseMp: // Amount, Amount %
                case SkillParam.Detect: // Type?, Level
                case SkillParam.SkinChange: // Type, Max level
                case SkillParam.AggroIncrease: // Taunt amount, Aggro amount
                case SkillParam.AoeDetect: // Unk1, Unk2
                case SkillParam.AggroAbsorb: // Aggro absorb %, absorb %
                case SkillParam.DebuffedDamageIncrease: // Unk, % increase
                case SkillParam.IncreaseAttack: // Phy increase %, Mag increase %
                case SkillParam.CurseSeriesReduce: // Type?, Effect
                case SkillParam.MonsterMask: // Monster level, Item Id
                case SkillParam.WizardTeleport: // Time, Distance
                case SkillParam.MobAggroReduce: // Reduction amount, Reduction %
                case SkillParam.StealMp: // Unk, Amount
                case SkillParam.HealingDance: // Recovery %, Unk
                case SkillParam.ManaDance: // Recovery %, Unk
                case SkillParam.TargetDash: // Unk, Unk
                case SkillParam.PetExpPotion: // Unk, Increase %
                case SkillParam.ManaSwitch: // Unk, Damage MP conversion %
                case SkillParam.BicheonSkill: // Shield def reduction %, phy increase amount
                case SkillParam.SocketStoneReflect: // Phy ratio %, Mag ratio %
                case SkillParam.SocketStoneFreeMp: // Mp refunded
                case SkillParam.ExpSpIncrease: // Exp increase %, Sp increase %
                case SkillParam.Resurrection: // Max lvl, recovered exp
                case SkillParam.DamageAbsorb: // Phy %, Mag %
                case SkillParam.ActiveMpConsumed: // Mp consumed / 1000, Base mp consumed
                case SkillParam.RequiredItem: // Tid3, tid4
                    return 2;
                case SkillParam.CurseProbabilityReduce: // Unk, Probability %, Level 
                case SkillParam.CurseRemove2:
                case SkillParam.TransferDamageAbsorb: // Type, absorb %, Unk
                case SkillParam.Invisible: // Type, Level, Movement speed reduction %
                case SkillParam.Cancelable: // Unk1, Unk2 (2 = when attacked), Unk3
                case SkillParam.Poison: // Effect, Probability %, Unk
                case SkillParam.Root: // Duration, Probability, Unk
                case SkillParam.Fear: // Duration, Probability %, Level
                case SkillParam.WarlockTrap: // Unk, Unk, Unk
                case SkillParam.Confusion: // Duration, Probability %, Level
                case SkillParam.FellowAttack:
                case SkillParam.FellowIncreaseAttack:
                case SkillParam.FellowZerk:
                case SkillParam.FellowBuff:
                case SkillParam.RedBlueFellowBuff:
                case SkillParam.FellowMpRecovery:
                    return 3;
                case SkillParam.MonsterCapture: // Type, MobId1, mobId2, mobId3
                case SkillParam.ReduceSelfPhyMagDmg:
                case SkillParam.DamageReturn: // Probability %, Phy return %, Mag return %, Radius
                case SkillParam.LinkedSkill: // Type, Distance, Number of connections, Link owner (bool)
                case SkillParam.ReduceSelfMaxHp: // Duration?, Unk, Amount %, Unk2
                case SkillParam.ReduceSelfPhyMagDef: // Duration?, Phy %, Mag %, Unk2
                case SkillParam.Decay: // Duration, Probability %, Level, Effect
                case SkillParam.Weaken: // Duration, Probability %, Level, Effect
                case SkillParam.Division: // Duration, Probability %, Level, Effect
                case SkillParam.Impotent: // Duration, Probability %, Level, Decrease %
                case SkillParam.Hidden: // Damage, Probability, Level, Unk
                case SkillParam.ShortSight: // Duration, Probability %, Level, Effect
                case SkillParam.Disease: // Duration, Probability %, Level, Effect
                case SkillParam.Darkness: // Duration, Probability %, Level, Effect
                case SkillParam.RamadanExpSpIncrease: // Duration, Unk, Increase %, Unk
                case SkillParam.HpMpRecovery: // Hp amount, Hp %, Mp amount, Mp %
                    return 4;
                case SkillParam.Sleep: // Duration, Probability %, Level, Effect, Times
                case SkillParam.Dull: // Duration, Probability %, Level, Effect, Times
                case SkillParam.Bleed:
                case SkillParam.Attack: // Dmg type, dmg %, min dmg, max dmg, dmg % players
                    return 5;
                case SkillParam.Combustion: // Duration, Probability, Level, Reduce MaxMP %, MP Regen %, Current MP %
                case SkillParam.Panic: // Duration, Probability, Level, Reduce MaxHP %, HP Regen %, Current HP %
                case SkillParam.AreaEffect: // Base range, Range type, Distance, Amount affected, Reduce per target, Unk
                    return 6;
                default:
                    return 0;
            }
        }
    }
}