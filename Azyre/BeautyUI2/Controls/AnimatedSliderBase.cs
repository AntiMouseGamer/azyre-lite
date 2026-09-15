using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BeautyUI2.Controls
{
	// Token: 0x02000018 RID: 24
	internal abstract class AnimatedSliderBase : Control
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000155 RID: 341 RVA: 0x00007D74 File Offset: 0x00005F74
		// (remove) Token: 0x06000156 RID: 342 RVA: 0x00007DAC File Offset: 0x00005FAC
		public event EventHandler ValueChanged;

		// Token: 0x06000157 RID: 343 RVA: 0x00007DE4 File Offset: 0x00005FE4
		protected AnimatedSliderBase()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this._animHover = new Timer();
			this._animHover.Interval = 15;
			this._animHover.Tick += this.AnimHover_Tick;
			this.UpdatePreferredSize();
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00007E90 File Offset: 0x00006090
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00007E98 File Offset: 0x00006098
		public int TrackWidth
		{
			get
			{
				return this._trackWidth;
			}
			set
			{
				this._trackWidth = Math.Max(10, value);
				this.UpdatePreferredSize();
				base.Invalidate();
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007EB4 File Offset: 0x000060B4
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00007EBC File Offset: 0x000060BC
		public int CornerRadius
		{
			get
			{
				return this._cornerRadius;
			}
			set
			{
				this._cornerRadius = Math.Max(0, value);
				base.Invalidate();
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007ED1 File Offset: 0x000060D1
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00007ED9 File Offset: 0x000060D9
		[Category("Thumb")]
		[DefaultValue(14f)]
		public float ThumbWidth
		{
			get
			{
				return this._thumbWidth;
			}
			set
			{
				this._thumbWidth = Math.Max(2f, value);
				this.UpdatePreferredSize();
				base.Invalidate();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007EF8 File Offset: 0x000060F8
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00007F00 File Offset: 0x00006100
		[Category("Thumb")]
		[DefaultValue(14f)]
		public float ThumbHeight
		{
			get
			{
				return this._thumbHeight;
			}
			set
			{
				this._thumbHeight = Math.Max(2f, value);
				this.UpdatePreferredSize();
				base.Invalidate();
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007F1F File Offset: 0x0000611F
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007F27 File Offset: 0x00006127
		[Category("Thumb")]
		[DefaultValue(7)]
		public int ThumbCornerRadius
		{
			get
			{
				return this._thumbCornerRadius;
			}
			set
			{
				this._thumbCornerRadius = Math.Max(0, value);
				base.Invalidate();
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007F3C File Offset: 0x0000613C
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007F44 File Offset: 0x00006144
		[Category("Thumb")]
		public Color ThumbColor
		{
			get
			{
				return this._thumbColor;
			}
			set
			{
				this._thumbColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007F53 File Offset: 0x00006153
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00007F5B File Offset: 0x0000615B
		[Category("Thumb")]
		public Color HoverThumbColor
		{
			get
			{
				return this._hoverThumbColor;
			}
			set
			{
				this._hoverThumbColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007F6A File Offset: 0x0000616A
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007F72 File Offset: 0x00006172
		[Category("Thumb")]
		public Color PressedThumbColor
		{
			get
			{
				return this._pressedThumbColor;
			}
			set
			{
				this._pressedThumbColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007F81 File Offset: 0x00006181
		public int GetPreferredWidth()
		{
			return Math.Max(this._trackWidth, (int)Math.Ceiling((double)this._thumbWidth)) + 6;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007F9D File Offset: 0x0000619D
		private void UpdatePreferredSize()
		{
			base.Width = this.GetPreferredWidth();
			this._thumbRadius = Math.Max(this._thumbWidth, this._thumbHeight) / 2f;
			this.SyncThumb();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007FD0 File Offset: 0x000061D0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._animHover != null)
			{
				this._animHover.Stop();
				this._animHover.Tick -= this.AnimHover_Tick;
				this._animHover.Dispose();
				this._animHover = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600016B RID: 363
		protected abstract void DrawTrack(Graphics g, Rectangle trackRect, GraphicsPath clipPath);

		// Token: 0x0600016C RID: 364
		protected abstract void SetValueFromRatio(double t, bool fireEvent);

		// Token: 0x0600016D RID: 365
		protected abstract double GetRatio();

		// Token: 0x0600016E RID: 366 RVA: 0x00008024 File Offset: 0x00006224
		protected void RaiseValueChanged()
		{
			EventHandler valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008047 File Offset: 0x00006247
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.SyncThumb();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008058 File Offset: 0x00006258
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle trackRect = this.GetTrackRect();
			int radius = (this._cornerRadius <= 0) ? (trackRect.Width / 2) : this._cornerRadius;
			using (GraphicsPath graphicsPath = GraphicsUtil.RoundedRect(trackRect, radius))
			{
				using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(22, 22, 22)))
				{
					graphics.FillPath(solidBrush, graphicsPath);
				}
				Region clip = graphics.Clip;
				graphics.SetClip(graphicsPath);
				this.DrawTrack(graphics, trackRect, graphicsPath);
				graphics.Clip = clip;
				using (Pen pen = new Pen(Color.FromArgb(55, 55, 55), 1f))
				{
					graphics.DrawPath(pen, graphicsPath);
				}
			}
			float num = (float)trackRect.Left + (float)trackRect.Width / 2f;
			float thumbY = this._thumbY;
			Color color = AnimatedSliderBase.Blend3(this._thumbColor, this._hoverThumbColor, this._pressedThumbColor, this._hoverStep, this._pressedStep);
			RectangleF rect = new RectangleF(num - this._thumbWidth / 2f, thumbY - this._thumbHeight / 2f, this._thumbWidth, this._thumbHeight);
			using (SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(110, 0, 0, 0)))
			{
				graphics.FillEllipse(solidBrush2, rect.X, rect.Y + 1f, rect.Width, rect.Height);
			}
			using (SolidBrush solidBrush3 = new SolidBrush(color))
			{
				using (GraphicsPath graphicsPath2 = AnimatedSliderBase.CreateRoundedRectangle(rect, Math.Min((float)this.ThumbCornerRadius, Math.Min(rect.Width / 2f, rect.Height / 2f))))
				{
					graphics.FillPath(solidBrush3, graphicsPath2);
				}
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008298 File Offset: 0x00006498
		protected Rectangle GetTrackRect()
		{
			int num = (int)Math.Ceiling((double)this._thumbRadius);
			int num2 = Math.Min(this._trackWidth, Math.Max(10, base.Width - 4));
			return new Rectangle((base.Width - num2) / 2, num, num2, Math.Max(1, base.Height - num * 2));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000082F0 File Offset: 0x000064F0
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			this._dragging = true;
			base.Capture = true;
			this._isPressed = true;
			if (!this._animHover.Enabled)
			{
				this._animHover.Start();
			}
			this.UpdateFromMouse(e.Y, true);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000834C File Offset: 0x0000654C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!this._dragging)
			{
				return;
			}
			this.UpdateFromMouse(e.Y, true);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000836B File Offset: 0x0000656B
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this._dragging = false;
			base.Capture = false;
			this._isPressed = false;
			if (!this._animHover.Enabled)
			{
				this._animHover.Start();
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000083A1 File Offset: 0x000065A1
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this._isHover = true;
			if (!this._animHover.Enabled)
			{
				this._animHover.Start();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000083C9 File Offset: 0x000065C9
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this._isHover = false;
			if (!this._animHover.Enabled)
			{
				this._animHover.Start();
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000083F4 File Offset: 0x000065F4
		private void UpdateFromMouse(int mouseY, bool fireEvent)
		{
			Rectangle trackRect = this.GetTrackRect();
			double num = (double)(mouseY - trackRect.Top) / (double)Math.Max(1, trackRect.Height);
			num = ColorUtil.Clamp(num, 0.0, 1.0);
			this.SetValueFromRatio(num, fireEvent);
			this.SyncThumb();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000844C File Offset: 0x0000664C
		protected void SyncThumb()
		{
			Rectangle trackRect = this.GetTrackRect();
			double ratio = this.GetRatio();
			this._thumbY = (float)((double)trackRect.Top + ratio * (double)trackRect.Height);
			base.Invalidate();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008488 File Offset: 0x00006688
		private void AnimHover_Tick(object sender, EventArgs e)
		{
			float num = 0.016f;
			this._hoverStep = AnimatedSliderBase.Lerp(this._hoverStep, this._isHover ? 1f : 0f, num * 12f);
			this._pressedStep = AnimatedSliderBase.Lerp(this._pressedStep, this._isPressed ? 1f : 0f, num * 12f);
			if (Math.Abs(this._hoverStep - (this._isHover ? 1f : 0f)) < 0.01f && Math.Abs(this._pressedStep - (this._isPressed ? 1f : 0f)) < 0.01f && !this._isHover && !this._isPressed)
			{
				this._animHover.Stop();
			}
			base.Invalidate();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008562 File Offset: 0x00006762
		private static float Clamp01(float v)
		{
			if (v < 0f)
			{
				return 0f;
			}
			if (v <= 1f)
			{
				return v;
			}
			return 1f;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008581 File Offset: 0x00006781
		private static float Lerp(float a, float b, float t)
		{
			t = AnimatedSliderBase.Clamp01(t);
			return a + (b - a) * t;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008592 File Offset: 0x00006792
		private static Color Blend3(Color baseC, Color hoverC, Color pressedC, float hoverT, float pressedT)
		{
			return AnimatedSliderBase.InterpolateColor(AnimatedSliderBase.InterpolateColor(baseC, hoverC, AnimatedSliderBase.Clamp01(hoverT)), pressedC, AnimatedSliderBase.Clamp01(pressedT));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000085B0 File Offset: 0x000067B0
		private static Color InterpolateColor(Color from, Color to, float t)
		{
			t = AnimatedSliderBase.Clamp01(t);
			return Color.FromArgb((int)((float)from.A + (float)(to.A - from.A) * t), (int)((float)from.R + (float)(to.R - from.R) * t), (int)((float)from.G + (float)(to.G - from.G) * t), (int)((float)from.B + (float)(to.B - from.B) * t));
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000863C File Offset: 0x0000683C
		private static GraphicsPath CreateRoundedRectangle(RectangleF rect, float radius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (rect.Width <= 0.5f || rect.Height <= 0.5f || float.IsNaN(rect.Width) || float.IsNaN(rect.Height) || float.IsInfinity(rect.Width) || float.IsInfinity(rect.Height))
			{
				return graphicsPath;
			}
			if (radius <= 0f)
			{
				graphicsPath.AddRectangle(rect);
				graphicsPath.CloseFigure();
				return graphicsPath;
			}
			float num = Math.Min(radius, Math.Min(rect.Width / 2f, rect.Height / 2f)) * 2f;
			RectangleF rect2 = new RectangleF(rect.X, rect.Y, num, num);
			graphicsPath.AddArc(rect2, 180f, 90f);
			rect2.X = rect.Right - num;
			graphicsPath.AddArc(rect2, 270f, 90f);
			rect2.Y = rect.Bottom - num;
			graphicsPath.AddArc(rect2, 0f, 90f);
			rect2.X = rect.Left;
			graphicsPath.AddArc(rect2, 90f, 90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		// Token: 0x040000CE RID: 206
		protected Timer _animHover;

		// Token: 0x040000CF RID: 207
		protected float _thumbY;

		// Token: 0x040000D0 RID: 208
		protected bool _dragging;

		// Token: 0x040000D1 RID: 209
		protected bool _isHover;

		// Token: 0x040000D2 RID: 210
		protected bool _isPressed;

		// Token: 0x040000D3 RID: 211
		protected float _hoverStep;

		// Token: 0x040000D4 RID: 212
		protected float _pressedStep;

		// Token: 0x040000D5 RID: 213
		protected float _thumbWidth = 14f;

		// Token: 0x040000D6 RID: 214
		protected float _thumbHeight = 14f;

		// Token: 0x040000D7 RID: 215
		protected int _thumbCornerRadius = 7;

		// Token: 0x040000D8 RID: 216
		protected Color _thumbColor = Color.White;

		// Token: 0x040000D9 RID: 217
		protected Color _hoverThumbColor = Color.White;

		// Token: 0x040000DA RID: 218
		protected Color _pressedThumbColor = Color.White;

		// Token: 0x040000DB RID: 219
		private int _trackWidth = 16;

		// Token: 0x040000DC RID: 220
		private int _cornerRadius = 8;

		// Token: 0x040000DD RID: 221
		protected float _thumbRadius = 7f;

		// Token: 0x040000DF RID: 223
		private const int ExtraPadding = 6;
	}
}
