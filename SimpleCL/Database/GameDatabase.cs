﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Coordinates;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Database
{
    public class GameDatabase
    {
        private static GameDatabase _instance;

        public static GameDatabase Get => _instance ?? (_instance = new GameDatabase());

        private SilkroadServer _selectedServer;

        public SilkroadServer SelectedServer
        {
            get => _selectedServer;
            set
            {
                _selectedServer = value;
                LoadSpawns();
            }
        }

        private readonly Dictionary<uint, NameValueCollection> _itemCache;
        private readonly Dictionary<uint, NameValueCollection> _modelCache;
        private readonly Dictionary<uint, NameValueCollection> _skillCache;
        private readonly Dictionary<uint, NameValueCollection> _teleBuildingCache;
        private readonly Dictionary<uint, NameValueCollection> _teleLinkCache;
        public readonly Dictionary<uint, List<SpawnPoint>> SpawnPoints = new Dictionary<uint, List<SpawnPoint>>();

        private GameDatabase()
        {
            _itemCache = LoadFromCache("items");
            _modelCache = LoadFromCache("models");
            _skillCache = LoadFromCache("skills");
            _teleBuildingCache = LoadFromCache("teleportbuildings");
            _teleLinkCache = LoadFromCache("teleportlinks");
        }

        private List<NameValueCollection> GetData(string sql, string dbNameExtra = "_DB")
        {
            if (SelectedServer == null)
            {
                throw new SystemException("Current server wasn't set");
            }

            string dbFile = DirectoryUtils.GetDbFile(SelectedServer.Name + dbNameExtra);

            if (dbFile == "")
            {
                throw new SystemException("DB file not found");
            }

            List<NameValueCollection> data = new List<NameValueCollection>();
            using (var conn = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand comm = conn.CreateCommand();
                comm.CommandTimeout = 1000;
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
                SQLiteDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader.GetValues());
                }

                return data;
            }
        }

        public ulong GetNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            if (data.Count == 0)
            {
                return 0;
            }

            return ulong.Parse(data[0]["player"]);
        }

        public ulong GetJobNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            if (data.Count == 0)
            {
                return 0;
            }

            return ulong.Parse(data[0]["job"]);
        }

        public ulong GetFellowNextLevelExp(byte level)
        {
            var data = GetData("SELECT * FROM leveldata WHERE level = " + level);
            if (data.Count == 0)
            {
                return 0;
            }

            return ulong.Parse(data[0]["fellow"]);
        }

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

            if (result.Count == 0)
            {
                return _itemCache[id] = null;
            }

            return _itemCache[id] = result[0];
        }

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

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }

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

            if (result.Count == 0)
            {
                return _skillCache[id] = null;
            }

            return _skillCache[id] = result[0];
        }

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

            if (result.Count == 0)
            {
                return _modelCache[id] = null;
            }

            return _modelCache[id] = result[0];
        }

        public NameValueCollection GetTeleportBuilding(uint id, QueryBuilder queryBuilder = null)
        {
            if (_teleBuildingCache.ContainsKey(id))
            {
                return _teleBuildingCache[id];
            }

            List<NameValueCollection> result;
            if (queryBuilder != null)
            {
                result = queryBuilder.Query("SELECT * FROM teleportbuildings WHERE id = " + id)
                    .ExecuteSelect(false);
            }
            else
            {
                result = GetData("SELECT * FROM teleportbuildings WHERE id = " + id);
            }

            if (result.IsEmpty())
            {
                return _teleBuildingCache[id] = null;
            }

            return _teleBuildingCache[id] = result[0];
        }

        public NameValueCollection GetTeleportLink(uint id, QueryBuilder queryBuilder = null)
        {
            if (_teleLinkCache.ContainsKey(id))
            {
                return _teleLinkCache[id];
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
                return _teleLinkCache[id] = null;
            }

            return _teleLinkCache[id] = result[0];
        }

        public uint GetGameVersion()
        {
            var result = GetData("SELECT * FROM data WHERE k = 'version'");
            if (result.IsEmpty())
            {
                return 0;
            }

            return uint.Parse(result[0]["v"]);
        }

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
                var name = entry["name"];

                if (SpawnPoints.ContainsKey(id))
                {
                    SpawnPoints[id].Add(new SpawnPoint(new LocalPoint((ushort) region, x, z, y), id, name));
                }
                else
                {
                    SpawnPoints[id] = new List<SpawnPoint>
                        {new SpawnPoint(new LocalPoint((ushort) region, x, z, y), id, name)};
                }
            }
        }

        public void CacheData()
        {
            if (!Directory.Exists("Cache"))
            {
                Directory.CreateDirectory("Cache");
            }

            CacheToFile(_itemCache, "items");
            CacheToFile(_modelCache, "models");
            CacheToFile(_skillCache, "skills");
            CacheToFile(_teleBuildingCache, "teleportbuildings");
            CacheToFile(_teleLinkCache, "teleportlinks");
        }

        public void CacheToFile(Dictionary<uint, NameValueCollection> cache, string fileName)
        {
            var values = new Dictionary<uint, Dictionary<string, string>>();
            foreach (KeyValuePair<uint, NameValueCollection> valueCollection in cache)
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

            using (StreamWriter file = File.CreateText("Cache/" + fileName + ".json"))
            {
                new JsonSerializer().Serialize(file, values);
            }
        }

        public Dictionary<uint, NameValueCollection> LoadFromCache(string fileName)
        {
            if (Directory.Exists("Cache"))
            {
                if (File.Exists("Cache/" + fileName + ".json"))
                {
                    try
                    {
                        string jsonString = File.ReadAllText("Cache/" + fileName + ".json");

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
                                    NameValueCollection nvc = new NameValueCollection();
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
                }
            }

            return new Dictionary<uint, NameValueCollection>();
        }
    }
}