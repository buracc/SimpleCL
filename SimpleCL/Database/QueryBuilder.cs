using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;

namespace SimpleCL.Database
{
    public class QueryBuilder
    {
        private readonly SQLiteConnection _db;
        private readonly SQLiteCommand _command;
        private readonly bool _fast;

        public QueryBuilder(string dbPath, bool fast = false)
        {
            _db = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
            _fast = fast;
            _command = new SQLiteCommand(_db) {CommandTimeout = 1000};

            if (fast)
            {
                _db.Open();
                _command.CommandText = "BEGIN";
                _command.ExecuteNonQuery();
            }
            else
            {
                _command = new SQLiteCommand(_db);
            }
        }

        public QueryBuilder Bind(string column, object value)
        {
            _command.Parameters.Add(new SQLiteParameter(column, value));
            return this;
        }

        public QueryBuilder Query(string query)
        {
            _command.CommandText = query;
            return this;
        }

        public void ExecuteUpdate(bool close = true)
        {
            if (!_fast)
            {
                _db.Open();
            }
            
            _command.ExecuteNonQuery();
            
            if (close)
            {
                Close();
            }
        }

        public List<NameValueCollection> ExecuteSelect(bool close = true)
        {
            var result = new List<NameValueCollection>();

            if (!_fast)
            {
                _db.Open();
            }

            var reader = _command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetValues());
            }

            reader.Close();
            
            if (close)
            {
                Close();
            }
            
            return result;
        }

        public void Finish()
        {
            if (_fast)
            {
                _command.CommandText = "END";
                _command.ExecuteNonQuery();
                Close();
            }
        }

        private void Close()
        {
            _command.Dispose();
            _db.Close();
        }
    }
}