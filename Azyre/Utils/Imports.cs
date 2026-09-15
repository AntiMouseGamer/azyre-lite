using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BeautyUI.Controls;

namespace Azyre.Utils
{
	// Token: 0x0200003C RID: 60
	public class Imports
	{
		// Token: 0x06000257 RID: 599
		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr intptr_0, out uint uint_2);

		// Token: 0x06000258 RID: 600
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000259 RID: 601 RVA: 0x0000E718 File Offset: 0x0000C918
		public static void Checkar(UserControl formulario, BeautyToggleSwitch c)
		{
			if (c == null) return;
			try
			{
				if (formulario != null && formulario.InvokeRequired)
					formulario.Invoke(new Action(() => { c.Checked = !c.Checked; }));
				else
					c.Checked = !c.Checked;
			}
			catch { }
		}
	}
}
