using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BeautyUI2.Controls
{
	// Token: 0x0200001D RID: 29
	internal class AlphaSlider : AnimatedSliderBase
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008E15 File Offset: 0x00007015
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008E20 File Offset: 0x00007020
		public int Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				value = (int)ColorUtil.Clamp((double)value, 0.0, 255.0);
				if (this._alpha == value)
				{
					return;
				}
				this._alpha = value;
				base.SyncThumb();
				base.Invalidate();
				base.RaiseValueChanged();
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008E6C File Offset: 0x0000706C
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008E74 File Offset: 0x00007074
		public Color BaseColor
		{
			get
			{
				return this._baseColor;
			}
			set
			{
				this._baseColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008E84 File Offset: 0x00007084
		protected override void DrawTrack(Graphics g, Rectangle trackRect, GraphicsPath clipPath)
		{
			AlphaSlider.DrawCheckerboard(g, trackRect, 8);
			Color color = Color.FromArgb(0, (int)this._baseColor.R, (int)this._baseColor.G, (int)this._baseColor.B);
			Color color2 = Color.FromArgb(255, (int)this._baseColor.R, (int)this._baseColor.G, (int)this._baseColor.B);
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(trackRect, color, color2, 90f))
			{
				g.FillRectangle(linearGradientBrush, trackRect);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008F20 File Offset: 0x00007120
		private static void DrawCheckerboard(Graphics g, Rectangle rect, int cell)
		{
			Color color = Color.FromArgb(45, 45, 45);
			Color color2 = Color.FromArgb(65, 65, 65);
			for (int i = rect.Top; i < rect.Bottom; i += cell)
			{
				for (int j = rect.Left; j < rect.Right; j += cell)
				{
					using (SolidBrush solidBrush = new SolidBrush(((j / cell + i / cell) % 2 == 1) ? color2 : color))
					{
						g.FillRectangle(solidBrush, j, i, cell, cell);
					}
				}
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008FB8 File Offset: 0x000071B8
		protected override void SetValueFromRatio(double t, bool fireEvent)
		{
			int num = (int)Math.Round(t * 255.0);
			num = (int)ColorUtil.Clamp((double)num, 0.0, 255.0);
			if (this._alpha == num)
			{
				return;
			}
			this._alpha = num;
			if (fireEvent)
			{
				base.RaiseValueChanged();
			}
			base.Invalidate();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009012 File Offset: 0x00007212
		protected override double GetRatio()
		{
			return ColorUtil.Clamp((double)this._alpha / 255.0, 0.0, 1.0);
		}

		// Token: 0x040000E2 RID: 226
		private int _alpha = 255;

		// Token: 0x040000E3 RID: 227
		private Color _baseColor = Color.Red;
	}
}
