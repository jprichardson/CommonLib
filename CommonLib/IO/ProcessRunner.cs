using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommonLib.IO
{
	public class ProcessRunner{

		protected Process _proc = new Process();

		public ProcessRunner(string programFile){
			_proc.StartInfo.UseShellExecute = false;
			 
			_proc.StartInfo.FileName = programFile;

			this.RedirectOutput = true;
			this.WaitForExit = true;
			this.HideWindow = true;
		}

		public virtual string Arguments { get { return _proc.StartInfo.Arguments; } set { _proc.StartInfo.Arguments = value; } }

		public virtual bool HideWindow { get; set; }
		
		public virtual string Program { get { return _proc.StartInfo.FileName; } }

		public virtual bool RedirectOutput { get { return _proc.StartInfo.RedirectStandardOutput; } set { _proc.StartInfo.RedirectStandardOutput = value; } }

		public virtual bool WaitForExit { get; set; }

		public virtual string Run() {
			return Run(null);
		}

		public virtual string Run(string arguments) {
			if (arguments != null && arguments != "")
				_proc.StartInfo.Arguments += arguments;

			//_proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			if (this.HideWindow)
				_proc.StartInfo.CreateNoWindow = true;

			_proc.Start();

			string output = "";
			if (this.RedirectOutput)
				output = _proc.StandardOutput.ReadToEnd();
			
			if (this.WaitForExit)
				_proc.WaitForExit();
			
			return output;
		}
	}
}
