using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Cryptographic
{
	// Token: 0x0200000E RID: 14
	public class Ed25519
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00004CFC File Offset: 0x00002EFC
		private static byte[] ComputeHash(byte[] m)
		{
			byte[] result;
			using (SHA512 sha = SHA512.Create())
			{
				result = sha.ComputeHash(m);
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004D34 File Offset: 0x00002F34
		private static BigInteger ExpMod(BigInteger number, BigInteger exponent, BigInteger modulo)
		{
			BigInteger bigInteger = BigInteger.One;
			BigInteger bigInteger2 = number.Mod(modulo);
			while (exponent > 0L)
			{
				if (!exponent.IsEven)
				{
					bigInteger = (bigInteger * bigInteger2).Mod(modulo);
				}
				bigInteger2 = (bigInteger2 * bigInteger2).Mod(modulo);
				exponent /= 2;
			}
			return bigInteger;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004D8F File Offset: 0x00002F8F
		private static BigInteger Inv(BigInteger x)
		{
			if (!Ed25519.InverseCache.ContainsKey(x))
			{
				Ed25519.InverseCache[x] = Ed25519.ExpMod(x, Ed25519.Qm2, Ed25519.Q);
			}
			return Ed25519.InverseCache[x];
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004DC4 File Offset: 0x00002FC4
		private static BigInteger RecoverX(BigInteger y)
		{
			BigInteger bigInteger = y * y;
			BigInteger bigInteger2 = (bigInteger - 1) * Ed25519.Inv(Ed25519.D * bigInteger + 1);
			BigInteger bigInteger3 = Ed25519.ExpMod(bigInteger2, Ed25519.Qp3 / Ed25519.Eight, Ed25519.Q);
			if (!(bigInteger3 * bigInteger3 - bigInteger2).Mod(Ed25519.Q).Equals(BigInteger.Zero))
			{
				bigInteger3 = (bigInteger3 * Ed25519.I).Mod(Ed25519.Q);
			}
			if (!bigInteger3.IsEven)
			{
				bigInteger3 = Ed25519.Q - bigInteger3;
			}
			return bigInteger3;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004E74 File Offset: 0x00003074
		private static Tuple<BigInteger, BigInteger> Edwards(BigInteger px, BigInteger py, BigInteger qx, BigInteger qy)
		{
			BigInteger right = px * qx;
			BigInteger right2 = py * qy;
			BigInteger right3 = Ed25519.D * right * right2;
			BigInteger num = (px * qy + qx * py) * Ed25519.Inv(1 + right3);
			BigInteger num2 = (py * qy + right) * Ed25519.Inv(1 - right3);
			return new Tuple<BigInteger, BigInteger>(num.Mod(Ed25519.Q), num2.Mod(Ed25519.Q));
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004F0C File Offset: 0x0000310C
		private static Tuple<BigInteger, BigInteger> EdwardsSquare(BigInteger x, BigInteger y)
		{
			BigInteger right = x * x;
			BigInteger bigInteger = y * y;
			BigInteger right2 = Ed25519.D * right * bigInteger;
			BigInteger num = 2 * x * y * Ed25519.Inv(1 + right2);
			BigInteger num2 = (bigInteger + right) * Ed25519.Inv(1 - right2);
			return new Tuple<BigInteger, BigInteger>(num.Mod(Ed25519.Q), num2.Mod(Ed25519.Q));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004F9C File Offset: 0x0000319C
		private static Tuple<BigInteger, BigInteger> ScalarMul(Tuple<BigInteger, BigInteger> point, BigInteger scalar)
		{
			Tuple<BigInteger, BigInteger> tuple = new Tuple<BigInteger, BigInteger>(BigInteger.Zero, BigInteger.One);
			Tuple<BigInteger, BigInteger> tuple2 = point;
			while (scalar > 0L)
			{
				if (!scalar.IsEven)
				{
					tuple = Ed25519.Edwards(tuple.Item1, tuple.Item2, tuple2.Item1, tuple2.Item2);
				}
				tuple2 = Ed25519.EdwardsSquare(tuple2.Item1, tuple2.Item2);
				scalar >>= 1;
			}
			return tuple;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000500C File Offset: 0x0000320C
		public static byte[] EncodeInt(BigInteger y)
		{
			byte[] array = y.ToByteArray();
			byte[] array2 = new byte[Math.Max(array.Length, 32)];
			Array.Copy(array, array2, array.Length);
			return array2;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000503C File Offset: 0x0000323C
		public static byte[] EncodePoint(BigInteger x, BigInteger y)
		{
			byte[] array = Ed25519.EncodeInt(y);
			int num = array.Length - 1;
			array[num] |= (byte)(x.IsEven ? 0 : 128);
			return array;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005065 File Offset: 0x00003265
		private static int GetBit(byte[] h, int i)
		{
			return h[i / 8] >> i % 8 & 1;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005078 File Offset: 0x00003278
		public static byte[] PublicKey(byte[] signingKey)
		{
			byte[] h = Ed25519.ComputeHash(signingKey);
			BigInteger bigInteger = Ed25519.TwoPowBitLengthMinusTwo;
			for (int i = 3; i < 254; i++)
			{
				if (Ed25519.GetBit(h, i) != 0)
				{
					bigInteger += Ed25519.TwoPowCache[i];
				}
			}
			Tuple<BigInteger, BigInteger> tuple = Ed25519.ScalarMul(Ed25519.B, bigInteger);
			return Ed25519.EncodePoint(tuple.Item1, tuple.Item2);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000050DC File Offset: 0x000032DC
		private static BigInteger HashInt(byte[] m)
		{
			byte[] h = Ed25519.ComputeHash(m);
			BigInteger bigInteger = BigInteger.Zero;
			for (int i = 0; i < 512; i++)
			{
				if (Ed25519.GetBit(h, i) != 0)
				{
					bigInteger += Ed25519.TwoPowCache[i];
				}
			}
			return bigInteger;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005124 File Offset: 0x00003324
		public static byte[] Signature(byte[] message, byte[] signingKey, byte[] publicKey)
		{
			byte[] array = Ed25519.ComputeHash(signingKey);
			BigInteger bigInteger = Ed25519.TwoPowBitLengthMinusTwo;
			for (int i = 3; i < 254; i++)
			{
				if (Ed25519.GetBit(array, i) != 0)
				{
					bigInteger += Ed25519.TwoPowCache[i];
				}
			}
			BigInteger bigInteger2;
			using (MemoryStream memoryStream = new MemoryStream(32 + message.Length))
			{
				memoryStream.Write(array, 32, 32);
				memoryStream.Write(message, 0, message.Length);
				bigInteger2 = Ed25519.HashInt(memoryStream.ToArray());
			}
			Tuple<BigInteger, BigInteger> tuple = Ed25519.ScalarMul(Ed25519.B, bigInteger2);
			byte[] array2 = Ed25519.EncodePoint(tuple.Item1, tuple.Item2);
			BigInteger y;
			using (MemoryStream memoryStream2 = new MemoryStream(32 + publicKey.Length + message.Length))
			{
				memoryStream2.Write(array2, 0, array2.Length);
				memoryStream2.Write(publicKey, 0, publicKey.Length);
				memoryStream2.Write(message, 0, message.Length);
				y = (bigInteger2 + Ed25519.HashInt(memoryStream2.ToArray()) * bigInteger).Mod(Ed25519.L);
			}
			byte[] result;
			using (MemoryStream memoryStream3 = new MemoryStream(64))
			{
				memoryStream3.Write(array2, 0, array2.Length);
				byte[] array3 = Ed25519.EncodeInt(y);
				memoryStream3.Write(array3, 0, array3.Length);
				result = memoryStream3.ToArray();
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000052A8 File Offset: 0x000034A8
		private static bool IsOnCurve(BigInteger x, BigInteger y)
		{
			BigInteger right = x * x;
			BigInteger bigInteger = y * y;
			BigInteger right2 = Ed25519.D * bigInteger * right;
			return (bigInteger - right - right2 - 1).Mod(Ed25519.Q).Equals(BigInteger.Zero);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005306 File Offset: 0x00003506
		private static BigInteger DecodeInt(byte[] s)
		{
			return new BigInteger(s) & Ed25519.Un;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005318 File Offset: 0x00003518
		private static Tuple<BigInteger, BigInteger> DecodePoint(byte[] pointBytes)
		{
			BigInteger bigInteger = new BigInteger(pointBytes) & Ed25519.Un;
			BigInteger bigInteger2 = Ed25519.RecoverX(bigInteger);
			if (((!bigInteger2.IsEven) ? 1 : 0) != Ed25519.GetBit(pointBytes, 255))
			{
				bigInteger2 = Ed25519.Q - bigInteger2;
			}
			Tuple<BigInteger, BigInteger> result = new Tuple<BigInteger, BigInteger>(bigInteger2, bigInteger);
			if (!Ed25519.IsOnCurve(bigInteger2, bigInteger))
			{
				throw new ArgumentException("Decoding point that is not on curve");
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000537C File Offset: 0x0000357C
		public static bool CheckValid(byte[] signature, byte[] message, byte[] publicKey)
		{
			if (signature.Length != 64)
			{
				throw new ArgumentException("Signature length is wrong");
			}
			if (publicKey.Length != 32)
			{
				throw new ArgumentException("Public key length is wrong");
			}
			Tuple<BigInteger, BigInteger> tuple = Ed25519.DecodePoint(Arrays.CopyOfRange(signature, 0, 32));
			Tuple<BigInteger, BigInteger> point = Ed25519.DecodePoint(publicKey);
			BigInteger scalar = Ed25519.DecodeInt(Arrays.CopyOfRange(signature, 32, 64));
			BigInteger scalar2;
			using (MemoryStream memoryStream = new MemoryStream(32 + publicKey.Length + message.Length))
			{
				byte[] array = Ed25519.EncodePoint(tuple.Item1, tuple.Item2);
				memoryStream.Write(array, 0, array.Length);
				memoryStream.Write(publicKey, 0, publicKey.Length);
				memoryStream.Write(message, 0, message.Length);
				scalar2 = Ed25519.HashInt(memoryStream.ToArray());
			}
			Tuple<BigInteger, BigInteger> tuple2 = Ed25519.ScalarMul(Ed25519.B, scalar);
			Tuple<BigInteger, BigInteger> tuple3 = Ed25519.ScalarMul(point, scalar2);
			Tuple<BigInteger, BigInteger> tuple4 = Ed25519.Edwards(tuple.Item1, tuple.Item2, tuple3.Item1, tuple3.Item2);
			return tuple2.Item1.Equals(tuple4.Item1) && tuple2.Item2.Equals(tuple4.Item2);
		}

		// Token: 0x04000063 RID: 99
		private static readonly Dictionary<BigInteger, BigInteger> InverseCache = new Dictionary<BigInteger, BigInteger>();

		// Token: 0x04000064 RID: 100
		private const int BitLength = 256;

		// Token: 0x04000065 RID: 101
		private static readonly BigInteger TwoPowBitLengthMinusTwo = BigInteger.Pow(2, 254);

		// Token: 0x04000066 RID: 102
		private static readonly BigInteger[] TwoPowCache = (from i in Enumerable.Range(0, 512)
		select BigInteger.Pow(2, i)).ToArray<BigInteger>();

		// Token: 0x04000067 RID: 103
		private static readonly BigInteger Q = BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819949");

		// Token: 0x04000068 RID: 104
		private static readonly BigInteger Qm2 = BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819947");

		// Token: 0x04000069 RID: 105
		private static readonly BigInteger Qp3 = BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819952");

		// Token: 0x0400006A RID: 106
		private static readonly BigInteger L = BigInteger.Parse("7237005577332262213973186563042994240857116359379907606001950938285454250989");

		// Token: 0x0400006B RID: 107
		private static readonly BigInteger D = BigInteger.Parse("-4513249062541557337682894930092624173785641285191125241628941591882900924598840740");

		// Token: 0x0400006C RID: 108
		private static readonly BigInteger I = BigInteger.Parse("19681161376707505956807079304988542015446066515923890162744021073123829784752");

		// Token: 0x0400006D RID: 109
		private static readonly BigInteger By = BigInteger.Parse("46316835694926478169428394003475163141307993866256225615783033603165251855960");

		// Token: 0x0400006E RID: 110
		private static readonly BigInteger Bx = BigInteger.Parse("15112221349535400772501151409588531511454012693041857206046113283949847762202");

		// Token: 0x0400006F RID: 111
		private static readonly Tuple<BigInteger, BigInteger> B = new Tuple<BigInteger, BigInteger>(Ed25519.Bx.Mod(Ed25519.Q), Ed25519.By.Mod(Ed25519.Q));

		// Token: 0x04000070 RID: 112
		private static readonly BigInteger Un = BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564819967");

		// Token: 0x04000071 RID: 113
		private static readonly BigInteger Two = new BigInteger(2);

		// Token: 0x04000072 RID: 114
		private static readonly BigInteger Eight = new BigInteger(8);
	}
}
