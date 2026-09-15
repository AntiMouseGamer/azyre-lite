using System;
using System.Numerics;

namespace Cryptographic
{
	// Token: 0x02000011 RID: 17
	internal static class BigIntegerHelpers
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00005618 File Offset: 0x00003818
		public static BigInteger Mod(this BigInteger num, BigInteger modulo)
		{
			BigInteger bigInteger = num % modulo;
			if (!(bigInteger < 0L))
			{
				return bigInteger;
			}
			return bigInteger + modulo;
		}
	}
}
