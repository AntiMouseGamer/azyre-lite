using System;

namespace BeautyUI2.Controls
{
	// Token: 0x02000017 RID: 23
	internal class SVChangedEventArgs : EventArgs
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007D3A File Offset: 0x00005F3A
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00007D42 File Offset: 0x00005F42
		public double S { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007D4B File Offset: 0x00005F4B
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00007D53 File Offset: 0x00005F53
		public double V { get; private set; }

		// Token: 0x06000154 RID: 340 RVA: 0x00007D5C File Offset: 0x00005F5C
		public SVChangedEventArgs(double s, double v)
		{
			this.S = s;
			this.V = v;
		}
	}
}
