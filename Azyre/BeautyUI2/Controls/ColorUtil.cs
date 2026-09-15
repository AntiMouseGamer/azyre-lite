using System;
using System.Drawing;

namespace BeautyUI2.Controls
{
	// Token: 0x0200001B RID: 27
	internal static class ColorUtil
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00008852 File Offset: 0x00006A52
		public static double Clamp(double v, double min, double max)
		{
			if (v < min)
			{
				return min;
			}
			if (v > max)
			{
				return max;
			}
			return v;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008861 File Offset: 0x00006A61
		public static double Clamp01(double v)
		{
			return ColorUtil.Clamp(v, 0.0, 1.0);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000887C File Offset: 0x00006A7C
		public static Color ColorFromHSV(double hue, double saturation, double value, int alpha)
		{
			hue = ColorUtil.Clamp(hue, 0.0, 360.0);
			saturation = ColorUtil.Clamp01(saturation);
			value = ColorUtil.Clamp01(value);
			alpha = (int)ColorUtil.Clamp((double)alpha, 0.0, 255.0);
			int num = (int)Math.Floor(hue / 60.0) % 6;
			double num2 = hue / 60.0 - Math.Floor(hue / 60.0);
			value *= 255.0;
			int num3 = (int)Math.Round(value);
			int num4 = (int)Math.Round(value * (1.0 - saturation));
			int num5 = (int)Math.Round(value * (1.0 - num2 * saturation));
			int num6 = (int)Math.Round(value * (1.0 - (1.0 - num2) * saturation));
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			switch (num)
			{
			case 0:
				num7 = num3;
				num8 = num6;
				num9 = num4;
				break;
			case 1:
				num7 = num5;
				num8 = num3;
				num9 = num4;
				break;
			case 2:
				num7 = num4;
				num8 = num3;
				num9 = num6;
				break;
			case 3:
				num7 = num4;
				num8 = num5;
				num9 = num3;
				break;
			case 4:
				num7 = num6;
				num8 = num4;
				num9 = num3;
				break;
			case 5:
				num7 = num3;
				num8 = num4;
				num9 = num5;
				break;
			}
			num7 = (int)ColorUtil.Clamp((double)num7, 0.0, 255.0);
			num8 = (int)ColorUtil.Clamp((double)num8, 0.0, 255.0);
			num9 = (int)ColorUtil.Clamp((double)num9, 0.0, 255.0);
			return Color.FromArgb(alpha, num7, num8, num9);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008A34 File Offset: 0x00006C34
		public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
		{
			double num = (double)color.R / 255.0;
			double num2 = (double)color.G / 255.0;
			double num3 = (double)color.B / 255.0;
			double num4 = Math.Max(num, Math.Max(num2, num3));
			double num5 = Math.Min(num, Math.Min(num2, num3));
			double num6 = num4 - num5;
			if (num6 < 1E-05)
			{
				hue = 0.0;
			}
			else if (num4 == num)
			{
				hue = 60.0 * ((num2 - num3) / num6 % 6.0);
			}
			else if (num4 == num2)
			{
				hue = 60.0 * ((num3 - num) / num6 + 2.0);
			}
			else
			{
				hue = 60.0 * ((num - num2) / num6 + 4.0);
			}
			if (hue < 0.0)
			{
				hue += 360.0;
			}
			saturation = ((num4 <= 0.0) ? 0.0 : (num6 / num4));
			value = num4;
			hue = ColorUtil.Clamp(hue, 0.0, 360.0);
			saturation = ColorUtil.Clamp01(saturation);
			value = ColorUtil.Clamp01(value);
		}
	}
}
