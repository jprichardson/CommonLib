using CommonLib.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System;

namespace TestYourProject
{
	///
	///This is a test class for SQLiteBulkInsertTest and is intended
	///to contain all SQLiteBulkInsertTest Unit Tests
	///
	[TestClass()]
	public class SQLiteBulkInsertTest
	{
		private static string m_testDir;
		private static string m_testFile;
		private static string m_testTableName = "test_table";
		private static string m_connectionString;

		private static string m_deleteAllQuery = "DELETE FROM [{0}]";
		private static string m_countAllQuery = "SELECT COUNT(id) FROM [{0}]";
		private static string m_selectAllQuery = "SELECT * FROM [{0}]";

		private static SQLiteConnection m_dbCon;
		private static SQLiteCommand m_deleteAllCmd;
		private static SQLiteCommand m_countAllCmd;
		private static SQLiteCommand m_selectAllCmd;

		private TestContext testContextInstance;

		///
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext) {
			Random rand = new Random(Environment.TickCount);
			int rn = rand.Next(0, int.MaxValue);
			m_testDir = @"C:\SqliteBulkInsertTest-" + rn + @"\";
			m_testFile = m_testDir + "db.sqlite";

			if (!Directory.Exists(m_testDir))
				Directory.CreateDirectory(m_testDir);

			if (!File.Exists(m_testFile)) {
				FileStream fs = File.Create(m_testFile);
				fs.Close();
			}

			m_connectionString = string.Format(@"data source={0};datetimeformat=Ticks", m_testFile);
			m_dbCon = new SQLiteConnection(m_connectionString);
			m_dbCon.Open();

			SQLiteCommand cmd = m_dbCon.CreateCommand();
			string query = "CREATE TABLE IF NOT EXISTS [{0}] (id INTEGER PRIMARY KEY AUTOINCREMENT, somestring VARCHAR(16), somereal REAL, someint INTEGER(4), somedt DATETIME)";
			query = string.Format(query, m_testTableName);
			cmd.CommandText = query;
			cmd.ExecuteNonQuery();
		}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		[ClassCleanup()]
		public static void MyClassCleanup() {
			m_dbCon.Close();

			File.Delete(m_testFile);
			Directory.Delete(m_testDir);
		}
		#endregion

		private void AddParameters(SQLiteBulkInsert target) {
			target.AddParameter("somestring", DbType.String);
			target.AddParameter("somereal", DbType.String);
			target.AddParameter("someint", DbType.Int32);
			target.AddParameter("somedt", DbType.DateTime);
		}

		private long CountRecords() {
			m_countAllCmd = m_dbCon.CreateCommand();
			m_countAllCmd.CommandText = string.Format(m_countAllQuery, m_testTableName);

			long ret = (long)m_countAllCmd.ExecuteScalar();
			m_countAllCmd.Dispose();

			return ret;
		}

		private void DeleteRecords() {
			m_deleteAllCmd = m_dbCon.CreateCommand();
			m_deleteAllCmd.CommandText = string.Format(m_deleteAllQuery, m_testTableName);

			m_deleteAllCmd.ExecuteNonQuery();
			m_deleteAllCmd.Dispose();
		}

		private SQLiteDataReader SelectAllRecords() {
			m_selectAllCmd = m_dbCon.CreateCommand();
			m_selectAllCmd.CommandText = string.Format(m_selectAllQuery, m_testTableName);
			return m_selectAllCmd.ExecuteReader();
		}

		[TestMethod()]
		public void AddParameterTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			AddParameters(target);

			string pd = target.ParamDelimiter;
			string expectedStmnt = "INSERT INTO [{0}] ([somestring], [somereal], [someint], [somedt]) VALUES ({1}somestring, {2}somereal, {3}someint, {4}somedt)";
			expectedStmnt = string.Format(expectedStmnt, m_testTableName, pd, pd, pd, pd);
			Assert.AreEqual(expectedStmnt, target.CommandText);
		}

		[TestMethod()]
		public void SQLiteBulkInsertConstructorTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			Assert.AreEqual(m_testTableName, target.TableName);

			bool wasException = false;
			try {
				string a = target.CommandText;
			}
			catch (SQLiteException ex) { wasException = true; }

			Assert.IsTrue(wasException);
		}

		[TestMethod()]
		public void CommandTextTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			AddParameters(target);

			string pd = target.ParamDelimiter;
			string expectedStmnt = "INSERT INTO [{0}] ([somestring], [somereal], [someint], [somedt]) VALUES ({1}somestring, {2}somereal, {3}someint, {4}somedt)";
			expectedStmnt = string.Format(expectedStmnt, m_testTableName, pd, pd, pd, pd);
			Assert.AreEqual(expectedStmnt, target.CommandText);
		}

		[TestMethod()]
		public void TableNameTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			Assert.AreEqual(m_testTableName, target.TableName);
		}

		[TestMethod()]
		public void InsertTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);

			bool didThrow = false;
			try {
				target.Insert("hello"); //object.length must equal the number of parameters added
			}
			catch (Exception ex) { didThrow = true; }
			Assert.IsTrue(didThrow);

			AddParameters(target);

			target.CommitMax = 4;
			DateTime dt1 = DateTime.Now; DateTime dt2 = DateTime.Now; DateTime dt3 = DateTime.Now; DateTime dt4 = DateTime.Now;
			target.Insert("john", 3.45f, 10, dt1);
			target.Insert("paul", -0.34f, 100, dt2 );
			target.Insert("ringo", 1000.98f, 1000, dt3 );
			target.Insert("george", 5.0f, 10000, dt4 );

			long count = CountRecords();
			Assert.AreEqual(4, count);

			SQLiteDataReader reader = SelectAllRecords();

			Assert.IsTrue(reader.Read());
			Assert.AreEqual("john", reader.GetString(1)); Assert.AreEqual(3.45f, reader.GetFloat(2));
			Assert.AreEqual(10, reader.GetInt32(3)); Assert.AreEqual(dt1, reader.GetDateTime(4));

			Assert.IsTrue(reader.Read());
			Assert.AreEqual("paul", reader.GetString(1)); Assert.AreEqual(-0.34f, reader.GetFloat(2));
			Assert.AreEqual(100, reader.GetInt32(3)); Assert.AreEqual(dt2, reader.GetDateTime(4));

			Assert.IsTrue(reader.Read());
			Assert.AreEqual("ringo", reader.GetString(1)); Assert.AreEqual(1000.98f, reader.GetFloat(2));
			Assert.AreEqual(1000, reader.GetInt32(3)); Assert.AreEqual(dt3, reader.GetDateTime(4));

			Assert.IsTrue(reader.Read());
			Assert.AreEqual("george", reader.GetString(1)); Assert.AreEqual(5.0f, reader.GetFloat(2));
			Assert.AreEqual(10000, reader.GetInt32(3)); Assert.AreEqual(dt4, reader.GetDateTime(4));

			Assert.IsFalse(reader.Read());

			DeleteRecords();

			count = CountRecords();
			Assert.AreEqual(0, count);
		}

		[TestMethod()]
		public void FlushTest() {
			string[] names = new string[] { "metalica", "beatles", "coldplay", "tiesto", "t-pain", "blink 182", "plain white ts", "staind", "pink floyd" };
			Random rand = new Random(Environment.TickCount);

			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			AddParameters(target);

			target.CommitMax = 1000;

			//Insert less records than commitmax
			for (int x = 0; x < 50; x++)
				target.Insert(names[rand.Next(names.Length)], (float)rand.NextDouble(), rand.Next(50), DateTime.Now);

			//Close connect to verify records were not inserted
			m_dbCon.Close();

			m_dbCon = new SQLiteConnection(m_connectionString);
			m_dbCon.Open();

			long count = CountRecords();
			Assert.AreEqual(0, count);

			//Now actually verify flush worked
			target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			AddParameters(target);

			target.CommitMax = 1000;

			//Insert less records than commitmax
			for (int x = 0; x < 50; x++)
				target.Insert(names[rand.Next(names.Length)], (float)rand.NextDouble(), rand.Next(50), DateTime.Now);

			target.Flush();

			count = CountRecords();
			Assert.AreEqual(50, count);

			//Close connect to verify flush worked
			m_dbCon.Close();

			m_dbCon = new SQLiteConnection(m_connectionString);
			m_dbCon.Open();

			count = CountRecords();
			Assert.AreEqual(50, count);

			DeleteRecords();
			count = CountRecords();
			Assert.AreEqual(0, count);
		}

		[TestMethod()]
		public void CommitMaxTest() {
			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);

			target.CommitMax = 4;
			Assert.AreEqual(4, target.CommitMax);

			target.CommitMax = 1000;
			Assert.AreEqual(1000, target.CommitMax);
		}

		//SPEED TEST
		[TestMethod()]
		public void AllowBulkInsertTest() {
			string[] names = new string[] { "metalica", "beatles", "coldplay", "tiesto", "t-pain", "blink 182", "plain white ts", "staind", "pink floyd"};
			Random rand = new Random(Environment.TickCount);

			SQLiteBulkInsert target = new SQLiteBulkInsert(m_dbCon, m_testTableName);
			AddParameters(target);

			const int COUNT = 100;

			target.CommitMax = COUNT;

			DateTime start1 = DateTime.Now;
			for (int x = 0; x < COUNT; x++)
				target.Insert(names[rand.Next(names.Length)], (float)rand.NextDouble(), rand.Next(COUNT), DateTime.Now);

			DateTime end1 = DateTime.Now;
			TimeSpan delta1 = end1 - start1;

			DeleteRecords();

			target.AllowBulkInsert = false;
			DateTime start2 = DateTime.Now;
			for (int x = 0; x < COUNT; x++)
				target.Insert(names[rand.Next(names.Length)], (float)rand.NextDouble(), rand.Next(COUNT), DateTime.Now);

			DateTime end2 = DateTime.Now;
			TimeSpan delta2 = end2 - start2;

			//THIS MAY FAIL DEPENDING UPON THE MACHINE THE TEST IS RUNNING ON.
			Assert.IsTrue(delta1.TotalSeconds < 0.1); //approx true for 100 recs 			Assert.IsTrue(delta2.TotalSeconds &gt; 1.0); //approx true for 100 recs;

			//UNCOMMENT THIS TO MAKE IT FAIL AND SEE ACTUAL NUMBERS IN FAILED REPORT
			//Assert.AreEqual(delta1.TotalSeconds, delta2.TotalSeconds);

			DeleteRecords();
		}
	}
}

