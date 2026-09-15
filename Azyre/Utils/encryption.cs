using System;
using System.Runtime.InteropServices;

namespace Azyre.Utils
{
	// Token: 0x02000036 RID: 54
	public static class encryption
	{
		// Token: 0x06000244 RID: 580
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

		// Token: 0x06000245 RID: 581
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x06000246 RID: 582 RVA: 0x0000E224 File Offset: 0x0000C424
		public static byte[] str_to_byte_arr(string hex)
		{
			byte[] result;
			try
			{
				int length = hex.Length;
				byte[] array = new byte[length / 2];
				for (int i = 0; i < length; i += 2)
				{
					array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				}
				result = array;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
