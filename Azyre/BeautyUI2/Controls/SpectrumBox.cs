using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeautyUI2.Controls
{
	// Token: 0x02000014 RID: 20
	internal class SpectrumBox : Control
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000132 RID: 306 RVA: 0x000071E0 File Offset: 0x000053E0
		// (remove) Token: 0x06000133 RID: 307 RVA: 0x00007218 File Offset: 0x00005418
		public event EventHandler<SVChangedEventArgs> SVChanged;

		// Token: 0x06000134 RID: 308 RVA: 0x00007250 File Offset: 0x00005450
		public SpectrumBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this._regenTimer = new Timer();
			this._regenTimer.Interval = 30;
			this._regenTimer.Tick += this.RegenTimer_Tick;
			base.Height = 170;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000072DE File Offset: 0x000054DE
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000072E6 File Offset: 0x000054E6
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000072FB File Offset: 0x000054FB
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00007304 File Offset: 0x00005504
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
				this._pendingHue = this._hue;
				this._regenPending = true;
				if (!this._regenTimer.Enabled)
				{
					this._regenTimer.Start();
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000737C File Offset: 0x0000557C
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00007384 File Offset: 0x00005584
		public double Sat
		{
			get
			{
				return this._sat;
			}
			set
			{
				this._sat = ColorUtil.Clamp01(value);
				this.UpdateTargetFromSV();
				base.Invalidate();
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000739E File Offset: 0x0000559E
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000073A6 File Offset: 0x000055A6
		public double Val
		{
			get
			{
				return this._val;
			}
			set
			{
				this._val = ColorUtil.Clamp01(value);
				this.UpdateTargetFromSV();
				base.Invalidate();
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000073C0 File Offset: 0x000055C0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._regenTimer != null)
				{
					this._regenTimer.Stop();
					this._regenTimer.Tick -= this.RegenTimer_Tick;
					this._regenTimer.Dispose();
					this._regenTimer = null;
				}
				if (this._bmp != null)
				{
					this._bmp.Dispose();
					this._bmp = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000742D File Offset: 0x0000562D
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.InvalidateBitmap();
			this.UpdateTargetFromSV();
			this._pendingHue = this._hue;
			this._regenPending = true;
			if (!this._regenTimer.Enabled)
			{
				this._regenTimer.Start();
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000746D File Offset: 0x0000566D
		private void InvalidateBitmap()
		{
			this._bmpHue = -1.0;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007480 File Offset: 0x00005680
		private void RegenTimer_Tick(object sender, EventArgs e)
		{
			if (!this._regenPending)
			{
				this._regenTimer.Stop();
				return;
			}
			this._regenPending = false;
			if (Math.Abs(this._bmpHue - this._pendingHue) < 0.01)
			{
				if (!this._regenPending)
				{
					this._regenTimer.Stop();
				}
				return;
			}
			this.GenerateBitmapAsync(this._pendingHue);
			if (!this._regenPending)
			{
				this._regenTimer.Stop();
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000074FC File Offset: 0x000056FC
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle r = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
			this.EnsureBitmap();
			using (GraphicsPath graphicsPath = GraphicsUtil.RoundedRect(r, this._cornerRadius))
			{
				using (Region region = new Region(graphicsPath))
				{
					graphics.Clip = region;
					if (this._bmp != null)
					{
						InterpolationMode interpolationMode = graphics.InterpolationMode;
						graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
						graphics.DrawImage(this._bmp, new Rectangle(0, 0, base.Width, base.Height));
						graphics.InterpolationMode = interpolationMode;
					}
					graphics.ResetClip();
				}
			}
			using (Pen pen = new Pen(Color.FromArgb(55, 55, 55), 1f))
			{
				graphics.DrawPath(pen, GraphicsUtil.RoundedRect(r, this._cornerRadius));
			}
			this.DrawMarker(graphics);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000761C File Offset: 0x0000581C
		private void DrawMarker(Graphics g)
		{
			float cx = this._cx;
			float cy = this._cy;
			float num = 6f;
			using (Pen pen = new Pen(Color.FromArgb(100, 0, 0, 0), 2f))
			{
				g.DrawEllipse(pen, cx - num, cy - num, num * 2f, num * 2f);
			}
			using (Pen pen2 = new Pen(Color.White, 2f))
			{
				g.DrawEllipse(pen2, cx - num, cy - num, num * 2f, num * 2f);
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000076D0 File Offset: 0x000058D0
		private void EnsureBitmap()
		{
			if (this._bmp == null)
			{
				this._pendingHue = this._hue;
				this._regenPending = true;
				if (!this._regenTimer.Enabled)
				{
					this._regenTimer.Start();
				}
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007708 File Offset: 0x00005908
		private async Task GenerateBitmapAsync(double hue)
		{
			try
			{
				int token = ++this._genToken;
				int w = Math.Max(1, base.Width);
				int h = Math.Max(1, base.Height);
				int bw = Math.Min(w, 220);
				int bh = Math.Min(h, 220);
				if (bw <= 0 || bh <= 0)
				{
					return;
				}
				Bitmap bmp = await Task.Run(() => SpectrumBox.BuildSpectrumBitmap(bw, bh, hue));
				if (token != this._genToken)
				{
					bmp.Dispose();
					return;
				}
				try
				{
					Bitmap old = this._bmp;
					this._bmp = bmp;
					this._bmpW = bw;
					this._bmpH = bh;
					this._bmpHue = hue;
					this._regenPending = false;
					if (old != null)
					{
						old.Dispose();
					}
					base.Invalidate();
				}
				catch
				{
					bmp.Dispose();
				}
			}
			catch { }
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007754 File Offset: 0x00005954
		private static Bitmap BuildSpectrumBitmap(int bw, int bh, double hue)
		{
			Bitmap bitmap = new Bitmap(bw, bh, PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bw, bh), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			try
			{
				int stride = bitmapData.Stride;
				int num = Math.Abs(stride) * bh;
				byte[] array = new byte[num];
				for (int i = 0; i < bh; i++)
				{
					double value = 1.0 - (double)i / ((double)bh - 1.0);
					for (int j = 0; j < bw; j++)
					{
						double saturation = (double)j / ((double)bw - 1.0);
						Color color = ColorUtil.ColorFromHSV(hue, saturation, value, 255);
						int num2 = i * stride + j * 4;
						array[num2] = color.B;
						array[num2 + 1] = color.G;
						array[num2 + 2] = color.R;
						array[num2 + 3] = byte.MaxValue;
					}
				}
				Marshal.Copy(array, 0, bitmapData.Scan0, num);
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000786C File Offset: 0x00005A6C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			base.Capture = true;
			this.SetFromPoint(e.Location);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007896 File Offset: 0x00005A96
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Capture)
			{
				return;
			}
			this.SetFromPoint(e.Location);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000078B4 File Offset: 0x00005AB4
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			base.Capture = false;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000078C4 File Offset: 0x00005AC4
		private void SetFromPoint(Point p)
		{
			int num = Math.Max(1, base.Width - 1);
			int num2 = Math.Max(1, base.Height - 1);
			double sat = ColorUtil.Clamp((double)p.X / (double)num, 0.0, 1.0);
			double val = ColorUtil.Clamp(1.0 - (double)p.Y / (double)num2, 0.0, 1.0);
			this._sat = sat;
			this._val = val;
			this.UpdateTargetFromSV();
			EventHandler<SVChangedEventArgs> svchanged = this.SVChanged;
			if (svchanged != null)
			{
				svchanged(this, new SVChangedEventArgs(this._sat, this._val));
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007978 File Offset: 0x00005B78
		private void UpdateTargetFromSV()
		{
			int num = Math.Max(1, base.Width - 1);
			int num2 = Math.Max(1, base.Height - 1);
			this._cx = (float)(this._sat * (double)num);
			this._cy = (float)((1.0 - this._val) * (double)num2);
			base.Invalidate();
		}

		// Token: 0x040000AF RID: 175
		private int _genToken;

		// Token: 0x040000B0 RID: 176
		private double _hue;

		// Token: 0x040000B1 RID: 177
		private double _sat = 1.0;

		// Token: 0x040000B2 RID: 178
		private double _val = 1.0;

		// Token: 0x040000B3 RID: 179
		private Bitmap _bmp;

		// Token: 0x040000B4 RID: 180
		private int _bmpW;

		// Token: 0x040000B5 RID: 181
		private int _bmpH;

		// Token: 0x040000B6 RID: 182
		private double _bmpHue = -1.0;

		// Token: 0x040000B7 RID: 183
		private const int MaxBitmapSize = 220;

		// Token: 0x040000B8 RID: 184
		private const int RegenIntervalMs = 30;

		// Token: 0x040000B9 RID: 185
		private Timer _regenTimer;

		// Token: 0x040000BA RID: 186
		private double _pendingHue;

		// Token: 0x040000BB RID: 187
		private bool _regenPending;

		// Token: 0x040000BC RID: 188
		private float _cx;

		// Token: 0x040000BD RID: 189
		private float _cy;

		// Token: 0x040000BE RID: 190
		private int _cornerRadius = 12;
	}
}
