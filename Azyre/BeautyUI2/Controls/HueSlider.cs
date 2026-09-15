using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BeautyUI2.Controls
{
	// Token: 0x0200001C RID: 28
	internal class HueSlider : AnimatedSliderBase
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008B7F File Offset: 0x00006D7F
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00008B88 File Offset: 0x00006D88
		public double Hue
		{
			get
			{
				return this._hue;
			}
			set
			{
				value = ColorUtil.Clamp(value, 0.0, 360.0);
				if (Math.Abs(this._hue - value) < 0.0001)
				{
					return;
				}
				this._hue = value;
				base.SyncThumb();
				base.Invalidate();
				base.RaiseValueChanged();
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008BE4 File Offset: 0x00006DE4
		protected override void DrawTrack(Graphics g, Rectangle trackRect, GraphicsPath clipPath)
		{
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(trackRect, Color.Red, Color.Red, 90f))
			{
				linearGradientBrush.InterpolationColors = new ColorBlend
				{
					Positions = new float[]
					{
						0f,
						0.17f,
						0.33f,
						0.5f,
						0.67f,
						0.83f,
						1f
					},
					Colors = new Color[]
					{
						ColorUtil.ColorFromHSV(0.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(60.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(120.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(180.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(240.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(300.0, 1.0, 1.0, 255),
						ColorUtil.ColorFromHSV(360.0, 1.0, 1.0, 255)
					}
				};
				g.FillRectangle(linearGradientBrush, trackRect);
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008D9C File Offset: 0x00006F9C
		protected override void SetValueFromRatio(double t, bool fireEvent)
		{
			double num = t * 360.0;
			if (Math.Abs(this._hue - num) < 0.0001)
			{
				return;
			}
			this._hue = num;
			if (fireEvent)
			{
				base.RaiseValueChanged();
			}
			base.Invalidate();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008DE4 File Offset: 0x00006FE4
		protected override double GetRatio()
		{
			return ColorUtil.Clamp(this._hue / 360.0, 0.0, 1.0);
		}

		// Token: 0x040000E1 RID: 225
		private double _hue;
	}
}
