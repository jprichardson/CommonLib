using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLib.IO
{
	//see: http://devproj20.blogspot.com/2008/02/writing-xml-with-utf-8-encoding-using.html
	public class StringWriterUTF8 : StringWriter
	{
		public StringWriterUTF8() : base() {	}
		public StringWriterUTF8(IFormatProvider formatProvider) : base(formatProvider) { } 

		public override Encoding Encoding { get { return Encoding.UTF8; } }
	}
}
