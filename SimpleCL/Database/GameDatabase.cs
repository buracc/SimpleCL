using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using SimpleCL.Enums;
using SimpleCL.Enums.Server;
using SimpleCL.Util;

namespace SimpleCL.Database
{
    public class GameDatabase
    {
        private static GameDatabase _instance;

        public static GameDatabase Get => _instance ?? (_instance = new GameDatabase());
        public SilkroadServer SelectedServer { get; set; }

        private GameDatabase()
        {
        }

        private List<NameValueCollection> GetData(string sql)
        {
            if (SelectedServer == null)
            {
                throw new SystemException("Current server wasn't set");
            }

            string dbFile = DirectoryUtils.GetDbFile(SelectedServer.Name);

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
            return ulong.Parse(GetData("SELECT * FROM leveldata WHERE level = " + level)[0]["player"]);
        }
        
        public ulong GetJobNextLevelExp(byte level)
        {
            return ulong.Parse(GetData("SELECT * FROM leveldata WHERE level = " + level)[0]["job"]);
        }

        public ulong GetFellowNextLevelExp(byte level)
        {
            return ulong.Parse(GetData("SELECT * FROM leveldata WHERE level = " + level)[0]["fellow"]);
        }

        public NameValueCollection GetItemData(uint id)
        {
            var result = GetData("SELECT * FROM items WHERE id = " + id);
            if (result.Count == 0)
            {
                throw new SystemException("Item " + id + " not found");
            }
            
            return result[0];
        }

        public NameValueCollection GetMagicOption(uint id)
        {
            var result = GetData("SELECT * FROM magicoption WHERE id = " + id);
            if (result.Count == 0)
            {
                throw new SystemException("Magic option " + id + " not found");
            }
            
            return result[0];
        }

        public NameValueCollection GetSkill(uint id)
        {
            var result = GetData("SELECT * FROM skills WHERE id = " + id);
            if (result.Count == 0)
            {
                throw new SystemException("Skill " + id + " not found");
            }
            
            return result[0];
        }

        public uint GetGameVersion()
        {
            var result = GetData("SELECT * FROM data WHERE k = 'version'");
            if (result.Count == 0)
            {
                throw new SystemException("Server data not found");
            }

            return uint.Parse(result[0]["v"]);
        }
    }
}