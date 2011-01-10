using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace CommonLib.Data.Sql
{
	public class SQLiteBulkInsert
	{
		private SQLiteConnection _dbCon;
		private SQLiteCommand _cmd;
		private SQLiteTransaction _trans;

		private Dictionary<string, SQLiteParameter> _parameters = new Dictionary<string, SQLiteParameter>();

		private uint _counter = 0;

		private string _beginInsertText;

		public SQLiteBulkInsert(SQLiteConnection dbConnection, string tableName) {
			_dbCon = dbConnection;
			_tableName = tableName;

			var query = new StringBuilder(255);
			query.Append("INSERT INTO ["); query.Append(tableName); query.Append("] (");
			_beginInsertText = query.ToString();
		}

		private bool _allowBulkInsert = true;
		public bool AllowBulkInsert { get { return _allowBulkInsert; } set { _allowBulkInsert = value; } }

		public string CommandText {
			get {
				if (_parameters.Count < 1)
					throw new SQLiteException("You must add at least one parameter.");

				var sb = new StringBuilder(255);
				sb.Append(_beginInsertText);

				foreach (string param in _parameters.Keys) {
					sb.Append('[');
					sb.Append(param);
					sb.Append(']');
					sb.Append(", ");
				}
				sb.Remove(sb.Length - 2, 2);

				sb.Append(") VALUES (");

				foreach (string param in _parameters.Keys) {
					sb.Append(_paramDelim);
					sb.Append(param);
					sb.Append(", ");
				}
				sb.Remove(sb.Length - 2, 2);

				sb.Append(")");

				return sb.ToString();
			}
		}

		private int _commitMax = 10000;
		public int CommitMax { get { return _commitMax; } set { _commitMax = value; } }

		private string _tableName;
		public string TableName { get { return _tableName; } }

		private string _paramDelim = ":";
		public string ParamDelimiter { get { return _paramDelim; } }

		public void AddParameter(string name, DbType dbType) {
			SQLiteParameter param = new SQLiteParameter(_paramDelim + name, dbType);
			_parameters.Add(name, param);
		}

		public void Flush() {
			try {
				if (_trans != null)
					_trans.Commit();
			}
			catch (Exception ex) { throw new Exception("Could not commit transaction. See InnerException for more details", ex); }
			finally {
				if (_trans != null)
					_trans.Dispose();

				_trans = null;
				_counter = 0;
			}
		}

		public void Insert(params object[] paramValues) {
			if (paramValues.Length != _parameters.Count)
				throw new Exception("The values array count must be equal to the count of the number of parameters.");

			_counter++;

			if (_counter == 1) {
				if (_allowBulkInsert)
					_trans = _dbCon.BeginTransaction();

				_cmd = _dbCon.CreateCommand();
				foreach (SQLiteParameter par in _parameters.Values)
					_cmd.Parameters.Add(par);

				_cmd.CommandText = this.CommandText;
			}

			int i = 0;
			foreach (SQLiteParameter par in _parameters.Values) {
				par.Value = paramValues[i];
				i++;
			}

			_cmd.ExecuteNonQuery();

			if (_counter == _commitMax) {
				try {
					if (_trans != null)
						_trans.Commit();
				}
				catch  { }
				finally {
					if (_trans != null) {
						_trans.Dispose();
						_trans = null;
					}

					_counter = 0;
				}
			}
		}
	}
}
