using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using SimpleCL.Enums;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Coordinates;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Database
{
    public class GameDatabase
    {
        private static GameDatabase _instance;

        public static GameDatabase Get => _instance ??= new GameDatabase();

        private SilkroadServer _selectedServer;

        public SilkroadServer SelectedServer
        {
            get => _selectedServer;
            set
            {
                _selectedServer = value;
                // LoadSpawns();
            }
        }

        private readonly Dictionary<uint, NameValueCollection> _itemCache;
        private readonly Dictionary<uint, NameValueCollection> _modelCache;
        private readonly Dictionary<uint, NameValueCollection> _skillCache;
        private readonly Dictionary<uint, NameValueCollection> _masteryCache;
        private readonly Dictionary<uint, List<NameValueCollection>> _teleportCache;
        private readonly Dictionary<uint, List<NameValueCollection>> _shopCache;
        public readonly Dictionary<uint, List<SpawnPoint>> SpawnPoints = new();

        private GameDatabase()
        {
            _itemCache = LoadFromCache("items");
            _modelCache = LoadFromCache("models");
            _skillCache = LoadFromCache("skills");
            _masteryCache = LoadFromCache("masteries");
            _teleportCache = LoadListFromCache("teleports");
            _shopCache = LoadListFromCache("shops");
            
            _selectedServer = SilkroadServer.Trsro;
        }

        #region Retrieve data

        private List<NameValueCollection> GetData(string sql, string dbNameExtra = "_DB")
        {
            if (SelectedServer == null)
            {
                throw new SystemException("Current server wasn't set");
            }

            var dbFile = SelectedServer.Name + dbNameExtra + ".db3";

            if (dbFile == "")
            {
                throw new SystemException("DB file not found");
            }

            var data = new List<NameValueCollection>();
            using var conn = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;");
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandTimeout = 1000;
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            var reader = comm.ExecuteReader();
            while (reader.Read())
            {
                data.Add(reader.GetValues());
            }

            return data;
        }

        #endregion

        #region Game data

        public uint GetGameVersion()
        {
            var result = GetData("SELECT * FROM data WHERE k = 'version'");
            return result.IsEmpty() ? 0 : uint.Parse(result[0]["v"]);
        }

        #endregion

        #region Exp

        public ulong GetNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            return data.IsEmpty() ? 0 : ulong.Parse(data[0]["player"]);
        }

        public ulong GetJobNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            return data.IsEmpty() ? 0 : ulong.Parse(data[0]["job"]);
        }

        public ulong GetFellowNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            return data.IsEmpty() ? 0 : ulong.Parse(data[0]["fellow"]);
        }

        #endregion

        #region Items

        public NameValueCollection GetItemData(uint id, QueryBuilder queryBuilder = null)
        {
            if (_itemCache.ContainsKey(id))
            {
                return _itemCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM items WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM items WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _itemCache[id] = null;
            }

            return _itemCache[id] = result[0];
        }

        #endregion

        #region Shops

        public List<NameValueCollection> GetShop(uint npcId, QueryBuilder queryBuilder = null)
        {
            if (_shopCache.ContainsKey(npcId))
            {
                return _shopCache[npcId];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM npcgoods WHERE model = " + npcId)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM npcgoods WHERE model = " + npcId);
            }

            if (result.IsEmpty())
            {
                return _shopCache[npcId] = new List<NameValueCollection>();
            }

            return _shopCache[npcId] = result;
        }

        #endregion

        #region Magic options

        public NameValueCollection GetMagicOption(uint id, QueryBuilder queryBuilder = null)
        {
            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM magicoption WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM magicoption WHERE id = " + id);
            }

            return result.IsEmpty() ? null : result[0];
        }

        #endregion

        #region Skills

        public NameValueCollection GetSkill(uint id, QueryBuilder queryBuilder = null)
        {
            if (_skillCache.ContainsKey(id))
            {
                return _skillCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM skills WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM skills WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _skillCache[id] = null;
            }

            return _skillCache[id] = result[0];
        }

        public NameValueCollection GetMastery(uint id, QueryBuilder queryBuilder = null)
        {
            if (_masteryCache.ContainsKey(id))
            {
                return _masteryCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM mastery WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM mastery WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _masteryCache[id] = null;
            }

            return _masteryCache[id] = result[0];
        }

        #endregion

        #region Models

        public NameValueCollection GetModel(uint id, QueryBuilder queryBuilder = null)
        {
            if (_modelCache.ContainsKey(id))
            {
                return _modelCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM models WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM models WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _modelCache[id] = null;
            }

            return _modelCache[id] = result[0];
        }

        #endregion

        #region Teleporters

        public List<NameValueCollection> GetTeleportLinks(uint id, QueryBuilder queryBuilder = null)
        {
            if (_teleportCache.ContainsKey(id))
            {
                return _teleportCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM teleportlinks WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM teleportlinks WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _teleportCache[id] = new List<NameValueCollection>();
            }

            return _teleportCache[id] = result;
        }

        #endregion

        #region Spawns

        public void LoadSpawns()
        {
            foreach (var entry in GetData(
                "SELECT n.*, m.name " +
                "FROM npcpos n " +
                "INNER JOIN monsters m " +
                "ON m.id = n.id"))
            {
                var id = uint.Parse(entry["id"]);
                var region = short.Parse(entry["region"]);
                var x = float.Parse(entry["x"], CultureInfo.InvariantCulture);
                var y = float.Parse(entry["y"], CultureInfo.InvariantCulture);
                var z = float.Parse(entry["z"], CultureInfo.InvariantCulture);
                var name = entry[Constants.Strings.Name];

                if (SpawnPoints.ContainsKey(id))
                {
                    SpawnPoints[id].Add(new SpawnPoint(new LocalPoint((ushort) region, x, z, y), id, name));
                }
                else
                {
                    SpawnPoints[id] = new List<SpawnPoint>
                        {new(new LocalPoint((ushort) region, x, z, y), id, name)};
                }
            }
        }

        #endregion

        #region Cache writers

        public void CacheData()
        {
            if (!Directory.Exists("Cache"))
            {
                Directory.CreateDirectory("Cache");
            }

            CacheToFile(_itemCache, "items");
            CacheToFile(_modelCache, "models");
            CacheToFile(_skillCache, "skills");
            CacheToFile(_masteryCache, "masteries");
            
            CacheListToFile(_teleportCache, "teleports");
            CacheListToFile(_shopCache, "shops");
        }

        public void CacheToFile(Dictionary<uint, NameValueCollection> cache, string fileName)
        {
            var values = new Dictionary<uint, Dictionary<string, string>>();
            foreach (var valueCollection in cache)
            {
                if (valueCollection.Value == null)
                {
                    values[valueCollection.Key] = null;
                }
                else
                {
                    values[valueCollection.Key] = valueCollection.Value.ToDictionary();
                }
            }

            using var file = File.CreateText("Cache/" + fileName + ".json");
            new JsonSerializer().Serialize(file, values);
        }
        
        public void CacheListToFile(Dictionary<uint, List<NameValueCollection>> cache, string fileName)
        {
            var values = new Dictionary<uint, List<Dictionary<string, string>>>();
            foreach (var entry in cache)
            {
                if (entry.Value == null)
                {
                    values[entry.Key] = new List<Dictionary<string, string>>();
                }
                else
                {
                    values[entry.Key] = new List<Dictionary<string, string>>();
                    foreach (var link in entry.Value)
                    {
                        values[entry.Key].Add(link.ToDictionary());
                    }
                }
            }

            using var file = File.CreateText("Cache/" + fileName + ".json");
            new JsonSerializer().Serialize(file, values);
        }

        #endregion

        #region Cache loaders

        public Dictionary<uint, NameValueCollection> LoadFromCache(string fileName)
        {
            if (!Directory.Exists("Cache"))
            {
                return new Dictionary<uint, NameValueCollection>();
            }

            if (!File.Exists("Cache/" + fileName + ".json"))
            {
                return new Dictionary<uint, NameValueCollection>();
            }

            try
            {
                var jsonString = File.ReadAllText("Cache/" + fileName + ".json");

                var json =
                    JsonConvert.DeserializeObject<Dictionary<uint, Dictionary<string, string>>>(jsonString);
                if (json != null)
                {
                    var output = new Dictionary<uint, NameValueCollection>();
                    foreach (var entry in json)
                    {
                        if (entry.Key > ushort.MaxValue)
                        {
                            Console.WriteLine("Found unusual entity with id: " + entry.Key +
                                              " in cache, removing it.");
                            continue;
                        }

                        if (entry.Value == null)
                        {
                            output[entry.Key] = null;
                        }
                        else
                        {
                            var nvc = new NameValueCollection();
                            foreach (var entry2 in entry.Value)
                            {
                                nvc.Add(entry2.Key, entry2.Value);
                            }

                            output[entry.Key] = nvc;
                        }
                    }

                    return output;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(fileName);
                Console.WriteLine(e);
            }

            return new Dictionary<uint, NameValueCollection>();
        }

        public Dictionary<uint, List<NameValueCollection>> LoadListFromCache(string fileName)
        {
            if (!Directory.Exists("Cache"))
            {
                return new Dictionary<uint, List<NameValueCollection>>();
            }

            if (!File.Exists("Cache/" + fileName + ".json"))
            {
                return new Dictionary<uint, List<NameValueCollection>>();
            }

            try
            {
                var jsonString = File.ReadAllText("Cache/" + fileName + ".json");

                var json =
                    JsonConvert.DeserializeObject<Dictionary<uint, List<Dictionary<string, string>>>>(jsonString);
                if (json != null)
                {
                    var output = new Dictionary<uint, List<NameValueCollection>>();
                    foreach (var entry in json)
                    {
                        var id = entry.Key;

                        var links = entry.Value;
                        if (links == null)
                        {
                            output[id] = new List<NameValueCollection>();
                        }
                        else
                        {
                            output[id] = new List<NameValueCollection>();

                            foreach (var link in links)
                            {
                                var nvc = new NameValueCollection();
                                foreach (var entry2 in link)
                                {
                                    nvc.Add(entry2.Key, entry2.Value);
                                }

                                output[id].Add(nvc);
                            }
                        }
                    }

                    return output;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(fileName);
                Console.WriteLine(e);
            }

            return new Dictionary<uint, List<NameValueCollection>>();
        }

        #endregion
    }
}