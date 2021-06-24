using System;
using System.Collections.Generic;
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

        private Dictionary<string, string> _nameReferences = new Dictionary<string, string>();
        private Dictionary<string, string> _textReferences = new Dictionary<string, string>();

        private const string EnabledLine = "1\t";
        private static readonly char[] DataSeparator = {'\t'};

        private const int LanguageIndex = 13;

        public Pk2Extractor(string pk2Path, string dbPath)
        {
            _dbPath = dbPath;
            _pk2Reader = new Pk2Reader(pk2Path);
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

        public void LoadNameReferences()
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

                            if (data.Length > LanguageIndex && data[LanguageIndex] != "0")
                            {
                                _nameReferences[data[2]] = data[LanguageIndex];
                            }
                        }
                    }
                }
            }
        }

        public void LoadTextReferences()
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

                            if (data.Length > LanguageIndex && data[LanguageIndex] != "0")
                            {
                                text = data[LanguageIndex];
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