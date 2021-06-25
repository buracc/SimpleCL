using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using SimpleCL.Enums.Server;
using SimpleCL.Util;

namespace SimpleCL.Database
{
    public class GameDatabase
    {
        private static GameDatabase _instance;

        public static GameDatabase Get => _instance ?? (_instance = new GameDatabase());
        public SilkroadServer SelectedServer { get; set; }

        private Dictionary<uint, NameValueCollection> _itemCache = new Dictionary<uint, NameValueCollection>();
        private Dictionary<uint, NameValueCollection> _modelCache = new Dictionary<uint, NameValueCollection>();
        private Dictionary<uint, NameValueCollection> _skillCache = new Dictionary<uint, NameValueCollection>();
        private Dictionary<uint, NameValueCollection> _teleBuildingCache = new Dictionary<uint, NameValueCollection>();
        private Dictionary<uint, NameValueCollection> _teleLinkCache = new Dictionary<uint, NameValueCollection>();

        private GameDatabase()
        {
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

        public NameValueCollection GetItemData(uint id)
        {
            if (_itemCache.ContainsKey(id))
            {
                return _itemCache[id];
            }
            
            var result = GetData("SELECT * FROM items WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return _itemCache[id] = result[0];
        }

        public NameValueCollection GetMagicOption(uint id)
        {
            var result = GetData("SELECT * FROM magicoption WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return result[0];
        }

        public NameValueCollection GetSkill(uint id)
        {
            if (_skillCache.ContainsKey(id))
            {
                return _skillCache[id];
            }
            
            var result = GetData("SELECT * FROM skills WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return _skillCache[id] = result[0];
        }
        
        public NameValueCollection GetModel(uint id)
        {
            if (_modelCache.ContainsKey(id))
            {
                return _modelCache[id];
            }
            
            var result = GetData("SELECT * FROM models WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return _modelCache[id] = result[0];
        }
        
        public NameValueCollection GetTeleportBuilding(uint id)
        {
            if (_teleBuildingCache.ContainsKey(id))
            {
                return _teleBuildingCache[id];
            }
            
            var result = GetData("SELECT * FROM teleportbuildings WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return _teleBuildingCache[id] = result[0];
        }
        
        public NameValueCollection GetTeleportLink(uint id)
        {
            if (_teleLinkCache.ContainsKey(id))
            {
                return _teleLinkCache[id];
            }
            
            var result = GetData("SELECT * FROM teleportlinks WHERE id = " + id);
            if (result.Count == 0)
            {
                return null;
            }
            
            return _teleLinkCache[id] = result[0];
        }

        public uint GetGameVersion()
        {
            var result = GetData("SELECT * FROM data WHERE k = 'version'");
            if (result.Count == 0)
            {
                return 0;
            }

            return uint.Parse(result[0]["v"]);
        }
    }
}