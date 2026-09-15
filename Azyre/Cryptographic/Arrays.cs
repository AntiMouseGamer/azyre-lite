using System;

namespace Cryptographic
{
	// Token: 0x02000010 RID: 16
	internal static class Arrays
	{
		// Token: 0x060000AA RID: 170 RVA: 0x000055F4 File Offset: 0x000037F4
		public static byte[] CopyOfRange(byte[] original, int from, int to)
		{
			int num = to - from;
			byte[] array = new byte[num];
			Array.Copy(original, from, array, 0, num);
			return array;
		}
	}
}
