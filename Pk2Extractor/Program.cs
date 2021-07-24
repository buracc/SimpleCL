using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Pk2Extractor.Api;
using Pk2Extractor.Database;

namespace Pk2Extractor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pk2Path = @"C:\Program Files (x86)\SilkroadTR\Media.pk2";
            var dbPath = "TRSRO_DB.db3";
            if (!File.Exists(dbPath))
            {
                Console.WriteLine("db doesnt exist");
                SQLiteConnection.CreateFile(dbPath);
            }
            
            var pk2Extractor = new Api.Pk2Extractor(pk2Path, dbPath);
            pk2Extractor.StoreTextReferences();
            pk2Extractor.StoreModels();
            pk2Extractor.StoreTeleportBuildings();
            pk2Extractor.StoreTeleportLinks();
            pk2Extractor.StoreItems();
            pk2Extractor.StoreMasteries();
            pk2Extractor.StoreSkills();
            
            pk2Extractor.ExtractItemIcons();
            pk2Extractor.ExtractSkillIcons();
            
            pk2Extractor.AddMinimap();

            // FindUnparsedParams(dbPath);
        }

        public static void FindUnparsedParams(string dbPath)
        {
            var attrs = new Dictionary<string, NameValueCollection>();

            var data = new QueryBuilder(dbPath)
                .Query("SELECT * FROM skills")
                .ExecuteSelect();

            foreach (var nvc in data)
            {
                var attributes = nvc["attributes"].Split(',');
                foreach (var attribute in attributes)
                {
                    if (attrs.ContainsKey(attribute))
                    {
                        continue;
                    }

                    attrs.Add(attribute, nvc);
                }
            }

            var index = 1;
            foreach (var kvp in attrs)
            {
                try
                {
                    var param = (Api.Pk2Extractor.SkillParam) uint.Parse(kvp.Key);
                    if (Enum.IsDefined(typeof(Api.Pk2Extractor.SkillParam), param))
                    {
                        continue;
                    }

                    Console.WriteLine($"{index++}. {param}");
                    Console.WriteLine(string.Join(",", kvp.Value.AllKeys.Select(key => kvp.Value[key])));
                }
                catch
                {
                    Console.WriteLine($"invalid: {kvp.Key}");
                }
            }
        }
    }
}