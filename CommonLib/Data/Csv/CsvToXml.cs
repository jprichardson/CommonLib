using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using CommonLib.Data.Xml;
using CommonLib.IO;
using System.Globalization;
using CommonLib.Utility;
using CommonLib.Collections;

namespace CommonLib.Data.Csv
{
	//the whole purpose of this class is to be able to use LINQ on CSV files
	public class CsvToXml
	{
		public CsvToXml():this(null) { }

		public CsvToXml(string csvFile) : this(csvFile, false) { }

		public CsvToXml(string csvFile, bool hasColumnNames) { 
			this.CsvFile = csvFile;
			this.RecordDelimiter = ',';
			this.TextQualifier = '"';
			this.HasColumnNames = hasColumnNames;
		}

		private Dictionary<string, Type> _columnTypes = new Dictionary<string, Type>();
		public Dictionary<string, Type> ColumnTypes { get { return _columnTypes; } }

		private string[] _columnNames = null;
		public string[] ColumnNames { get { return _columnNames; } }

		public string CsvFile { get; set; }

		public bool HasColumnNames { get; set; }

		public char RecordDelimiter { get; set; }

		public IEnumerable<XElement> Records {
			get {
				if (this.XmlData != null) {
					return this.XmlData.Descendants("Record");
				}
				else
					return null;
			}
		}

		public IEnumerable<dynamic> DynamicRecords {
			get {
				foreach (var r in this.Records) {
					DynamicXElement dx = new DynamicXElement(r);
					foreach (var columnName in _columnTypes.Keys)
						dx.ElementTypes.Add(columnName, _columnTypes[columnName]);
					yield return dx;
				}
			}
		}

		public char? TextQualifier { get; set; }

		public XElement XmlData { get; protected set; }

		public string XmlString {
			get {
				if (this.XmlData == null)
					return "";
				else
					return StringUtil.XElementToString(this.XmlData);
			}
		}

		public void Convert() {
			var tempFileLines = new List<string>();
			var sr = new StreamReader(this.CsvFile);
			var sb = new StringBuilder(255);
			while (!sr.EndOfStream) {
				sb.Append(sr.ReadLine());
				if (sb[sb.Length - 1] == this.RecordDelimiter)
					sb.Remove(sb.Length - 1, 1);
				tempFileLines.Add(sb.ToString());
				sb.Clear();
			}
			sr.Close();

			var tempLines = tempFileLines.ToArray();
			string[] lines = null;
			_columnNames = null;

			if (this.HasColumnNames) {
				_columnNames = Csv.RecordSplit(tempLines[0], this.RecordDelimiter, this.TextQualifier);

				lines = new string[tempLines.Length - 1];
				Array.Copy(tempLines, 1, lines, 0, lines.Length);
			} else {
				var columnCount = Csv.RecordSplit(tempLines[0], this.RecordDelimiter, this.TextQualifier).Length;
				_columnNames = new string[columnCount];
				for (int x = 0; x < _columnNames.Length; ++x)
					_columnNames[x] = "Column" + (x+1);

				lines = tempLines;
			}

			this.XmlData = new XElement("Records",
				from line in lines
				let fields = Csv.RecordSplit(line, this.RecordDelimiter, this.TextQualifier)
				select new XElement("Record",
					from fieldData in fields
					let i = fields.ToList().FindIndex(f => f == fieldData)
					select new XElement(_columnNames[i], fieldData)
				)
			);
		}
	}
}
