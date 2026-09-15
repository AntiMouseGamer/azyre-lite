using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Azyre.Prot
{
	public static class Helpers
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		public static string GetHwid()
		{
			return "UNKNOWN";
		}

		public static void ShowMessage(string message, bool CMD, bool SelfDelete)
		{
			if (CMD)
			{
				Process.Start(new ProcessStartInfo("cmd.exe", "/c start cmd /C \"color 03 && echo " + message + " && echo. && pause && pause && pause && pause")
				{
					CreateNoWindow = true,
					UseShellExecute = false
				});
				if (SelfDelete)
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = "cmd.exe",
						Arguments = "/C timeout 1 & del \"" + Process.GetCurrentProcess().MainModule.FileName + "\"",
						CreateNoWindow = true,
						UseShellExecute = false
					});
				}
				Helpers.TerminateProcess(Helpers.GetCurrentProcess(), 0U);
				Application.Exit();
				Environment.Exit(0);
				return;
			}
			MessageBox.Show(message, "Elite Private Softwares", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}
}
