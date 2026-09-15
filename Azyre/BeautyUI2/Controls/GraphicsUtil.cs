using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BeautyUI2.Controls
{
	// Token: 0x0200001A RID: 26
	internal static class GraphicsUtil
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00008798 File Offset: 0x00006998
		public static GraphicsPath RoundedRect(Rectangle r, int radius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (radius <= 0)
			{
				graphicsPath.AddRectangle(r);
				graphicsPath.CloseFigure();
				return graphicsPath;
			}
			int num = radius * 2;
			Rectangle rect = new Rectangle(r.X, r.Y, num, num);
			graphicsPath.AddArc(rect, 180f, 90f);
			rect.X = r.Right - num;
			graphicsPath.AddArc(rect, 270f, 90f);
			rect.Y = r.Bottom - num;
			graphicsPath.AddArc(rect, 0f, 90f);
			rect.X = r.X;
			graphicsPath.AddArc(rect, 90f, 90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
	}
}
