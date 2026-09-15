using System;
using System.Drawing;

namespace BeautyUI2.Controls
{
	// Token: 0x02000019 RID: 25
	internal class ColorChangedEventArgs : EventArgs
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00008778 File Offset: 0x00006978
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00008780 File Offset: 0x00006980
		public Color Color { get; private set; }

		// Token: 0x06000181 RID: 385 RVA: 0x00008789 File Offset: 0x00006989
		public ColorChangedEventArgs(Color c)
		{
			this.Color = c;
		}
	}
}
