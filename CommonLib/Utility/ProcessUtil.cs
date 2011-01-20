using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommonLib.Utility
{
	public static class ProcessUtil
	{
		public static bool IsRunning(string processName) {
			if (String.IsNullOrWhiteSpace(processName))
				return false;

			processName = processName.ToLower();
			if (processName.EndsWith(".exe"))
				processName = processName.Substring(0, processName.Length - 4);

			foreach (var p in Process.GetProcesses())
				if (p.ProcessName.ToLower().Equals(processName))
					return true;

			return false;
		}
	}
}
