using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Azyre
{
	internal static class Program
	{
		[DllImport("kernel32.dll")]
		public static extern void ExitProcess(uint uExitCode);

		[STAThread]
		private static void Main()
		{
			Costura.AssemblyLoader.Attach(true);
			Control.CheckForIllegalCrossThreadCalls = false;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
