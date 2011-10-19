using CommonLib.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using CommonLib.Data.Xml;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;

namespace TestCommonLib
{

	[TestClass()]
	public class CsvToXmlTest
	{

		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		[TestInitialize()]
		public void MyTestInitialize()
		{
			TestEnvironment.Bootstrap();
		}
		
		[TestCleanup()]
		public void MyTestCleanup()
		{
			TestEnvironment.Destroy();
		}

		[TestMethod()]
		public void ConstructorTest() {
			string file = "DOESNT_EXIST.CSV";
			var csv = new CsvToXml(file);
			Assert.AreEqual(file, csv.CsvFile);
			Assert.AreEqual(',', csv.RecordDelimiter);
			Assert.AreEqual('"', csv.TextQualifier);
			Assert.AreEqual(false, csv.HasColumnNames);
		}

		[TestMethod()]
		public void ConvertTest() {
			var columnNames = "'Product','Price','DateStocked'";
			var data = "'Pepsi','4.50','2010-05-04'\n'Coke','3.00','2010-09-22'\n'Cheetos','7.25','2009-01-13'";

			var csvWithColumnNamesFile = TestEnvironment.DataPath + "csv-columnNames.csv";
			var csvFile = TestEnvironment.DataPath + "csv.csv";

			File.WriteAllText(csvWithColumnNamesFile, columnNames + "\n" + data);
			File.WriteAllText(csvFile, data);

			string xmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";

			string xmlWithColumnNames = xmlHeader +
@"<Records>
	<Record>
		<Product>Pepsi</Product>
		<Price>4.50</Price>
		<DateStocked>2010-05-04</DateStocked>
	</Record>
	<Record>
		<Product>Coke</Product>
		<Price>3.00</Price>
		<DateStocked>2010-09-22</DateStocked>
	</Record>
	<Record>
		<Product>Cheetos</Product>
		<Price>7.25</Price>
		<DateStocked>2009-01-13</DateStocked>
	</Record>
</Records>";

			var csvWithColumnNames = new CsvToXml(csvWithColumnNamesFile);
			csvWithColumnNames.RecordDelimiter = ','; csvWithColumnNames.TextQualifier = '\'';
			csvWithColumnNames.HasColumnNames = true;
			csvWithColumnNames.Convert();

			string actualXmlWithColumnNames = csvWithColumnNames.XmlString;

			Assert.AreEqual(xmlWithColumnNames, actualXmlWithColumnNames);

			string xml = xmlHeader +
@"<Records>
	<Record>
		<Column1>Pepsi</Column1>
		<Column2>4.50</Column2>
		<Column3>2010-05-04</Column3>
	</Record>
	<Record>
		<Column1>Coke</Column1>
		<Column2>3.00</Column2>
		<Column3>2010-09-22</Column3>
	</Record>
	<Record>
		<Column1>Cheetos</Column1>
		<Column2>7.25</Column2>
		<Column3>2009-01-13</Column3>
	</Record>
</Records>";

			var csv = new CsvToXml(csvFile);
			csv.RecordDelimiter = ','; csv.TextQualifier = '\'';
			csv.HasColumnNames = false;
			csv.Convert();

			var actualXml = csv.XmlString;

			Assert.AreEqual(xml, actualXml);
		}

		[TestMethod()]
		public void Convert2Test() {

			var columnNames = "Product;Price;DateStocked;";
			var data = "Pepsi;4.50;2010-05-04;\nCoke;3.00;2010-09-22;\nCheetos;7.25;2009-01-13;";

			var csvWithColumnNamesFile = TestEnvironment.DataPath + "csv-columnNames.csv";
			var csvFile = TestEnvironment.DataPath + "csv.csv";

			File.WriteAllText(csvWithColumnNamesFile, columnNames + "\n" + data);
			File.WriteAllText(csvFile, data);

			string xmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";

			string xmlWithColumnNames = xmlHeader +
@"<Records>
	<Record>
		<Product>Pepsi</Product>
		<Price>4.50</Price>
		<DateStocked>2010-05-04</DateStocked>
	</Record>
	<Record>
		<Product>Coke</Product>
		<Price>3.00</Price>
		<DateStocked>2010-09-22</DateStocked>
	</Record>
	<Record>
		<Product>Cheetos</Product>
		<Price>7.25</Price>
		<DateStocked>2009-01-13</DateStocked>
	</Record>
</Records>";

			var csvWithColumnNames = new CsvToXml(csvWithColumnNamesFile);
			csvWithColumnNames.RecordDelimiter = ';'; csvWithColumnNames.TextQualifier = null;
			csvWithColumnNames.HasColumnNames = true;
			csvWithColumnNames.Convert();

			string actualXmlWithColumnNames = csvWithColumnNames.XmlString;

			Assert.AreEqual(xmlWithColumnNames, actualXmlWithColumnNames);

		}

		[TestMethod()]
		public void RecordsTest() {
			var columnNames = "'Product','Price','DateStocked'";
			var data = "'Pepsi','4.50','2010-05-04'\n'Coke','3.00','2010-09-22'\n'Cheetos','7.25','2009-01-13'";

			var csv2xmlFile = TestEnvironment.DataPath + "csv2xml.csv";
			File.WriteAllText(csv2xmlFile, columnNames + "\n" + data);

			var csv2xml = new CsvToXml(csv2xmlFile, true);
			csv2xml.TextQualifier = '\'';
			csv2xml.Convert();

			Assert.AreEqual(3, csv2xml.Records.Count());

			var records = from rec in csv2xml.Records
						  where (decimal)rec.Element("Price") > 3.5m
						  orderby (string)rec.Element("Product")
						  select rec;

			Assert.AreEqual(2, records.Count());
			Assert.AreEqual("Cheetos", (string)records.ElementAt(0).Element("Product"));
			Assert.AreEqual("Pepsi", (string)records.ElementAt(1).Element("Product"));

			records = from rec in csv2xml.Records
					  where Convert.ToDateTime((string)rec.Element("DateStocked")) > new DateTime(2010, 9, 1)
					  orderby (string)rec.Element("Product")
					  select rec;

			Assert.AreEqual(1, records.Count());
			Assert.AreEqual("Coke", (string)records.ElementAt(0).Element("Product"));
		}

		[TestMethod()]
		public void DynamicRecordsTest() {
			var columnNames = "'Product','Price','DateStocked'";
			var data = "'Pepsi','4.50','2010-05-04'\n'Coke','3.00','2010-09-22'\n'Cheetos','7.25','2009-01-13'";

			var csv2xmlFile = TestEnvironment.DataPath + "csv2xml.csv";
			File.WriteAllText(csv2xmlFile, columnNames + "\n" + data);

			//WITHOUT Configuring Column Types
			var csv2xml = new CsvToXml(csv2xmlFile, true);
			csv2xml.TextQualifier = '\'';
			csv2xml.Convert();

			Assert.AreEqual(3, csv2xml.DynamicRecords.Count());

			var records = from rec in csv2xml.DynamicRecords
						  where (decimal)rec.Price > 3.5m
						  orderby (string)rec.Product
						  select rec;

			Assert.AreEqual(2, records.Count());
			Assert.AreEqual("Cheetos", (string)records.ElementAt(0).Product);
			Assert.AreEqual("Pepsi", (string)records.ElementAt(1).Product);

			records = from rec in csv2xml.DynamicRecords
					  where Convert.ToDateTime((string)rec.DateStocked) > new DateTime(2010, 9, 1)
					  orderby (string)rec.Product
					  select rec;

			Assert.AreEqual(1, records.Count());
			Assert.AreEqual("Coke", (string)records.ElementAt(0).Product);

			//With Configuring Column Types
			csv2xml = new CsvToXml(csv2xmlFile, true);
			csv2xml.TextQualifier = '\'';

			csv2xml.ColumnTypes.Add("Product", typeof(string));
			csv2xml.ColumnTypes.Add("Price", typeof(decimal));
			csv2xml.ColumnTypes.Add("DateStocked", typeof(DateTime));
			
			csv2xml.Convert();
			
			Assert.AreEqual(3, csv2xml.DynamicRecords.Count());

			records = from rec in csv2xml.DynamicRecords
						where rec.Price > 3.5m
						orderby rec.Product
						select rec;

			Assert.AreEqual(2, records.Count());
			Assert.AreEqual("Cheetos", records.ElementAt(0).Product);
			Assert.AreEqual("Pepsi", records.ElementAt(1).Product);

			records = from rec in csv2xml.DynamicRecords
						where rec.DateStocked > new DateTime(2010, 9, 1)
						orderby rec.Product
						select rec;

			Assert.AreEqual(1, records.Count());
			Assert.AreEqual("Coke", records.ElementAt(0).Product);
		}		
	}
}
