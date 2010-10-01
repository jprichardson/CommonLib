using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Data.Csv
{
	public static class Csv
	{
		public static string[] RecordSplit(string line, char recordDelimiter, char? textQualifier) {
			if (textQualifier != null) {
				if (line[0] != textQualifier)
					throw new Exception("First character is not a record delimiter.");

				if (line[line.Length - 1] != textQualifier)
					throw new Exception("Last character is not a record delimiter.");

				char[] splitter = new char[3];
				splitter[0] = textQualifier.Value; splitter[1] = recordDelimiter; splitter[2] = textQualifier.Value;
				
				var dat = line.Split(new string[] { new string(splitter) }, StringSplitOptions.None);
				
				//trim off textQualifier of beginning of the line and end of the line
				dat[0] = dat[0].Substring(1, dat[0].Length - 1);
				dat[dat.Length - 1] = dat[dat.Length - 1].Substring(0, dat[dat.Length - 1].Length - 1);
				
				return dat;
			}
			else
				return line.Split(recordDelimiter);
		}
	}
}
