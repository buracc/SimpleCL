using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Linq;
using SimpleCL.Network.Enums;

namespace SimpleCL.Database
{
    public class GameDatabase
    {
        private static GameDatabase _instance;

        private GameDatabase()
        {
        }

        public static GameDatabase GetInstance()
        {
            return _instance ?? (_instance = new GameDatabase());
        }

        private List<NameValueCollection> GetData(string sql)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Programs\phBot Testing\Data\TRSRO";
            List<NameValueCollection> data = new List<NameValueCollection>();
            using (var conn = new SQLiteConnection("Data Source=" + path + ".db3;Version=3;"))
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
            return ulong.Parse(GetData("SELECT player FROM leveldata WHERE level = " + level)[0]["player"]);
        }

        public ulong GetFellowNextLevelExp(byte level)
        {
            return ulong.Parse(GetData("SELECT player FROM leveldata WHERE level = " + level)[0]["fellow"]);
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
    }
}