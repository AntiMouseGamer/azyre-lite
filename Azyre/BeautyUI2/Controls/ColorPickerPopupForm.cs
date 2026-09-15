using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BeautyUI2.Controls
{
	// Token: 0x02000013 RID: 19
	internal partial class ColorPickerPopupForm : Form
	{
		// Token: 0x060000F2 RID: 242
		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000F3 RID: 243 RVA: 0x00006424 File Offset: 0x00004624
		// (remove) Token: 0x060000F4 RID: 244 RVA: 0x0000645C File Offset: 0x0000465C
		public event EventHandler<ColorChangedEventArgs> ColorChanged;

		// Token: 0x060000F5 RID: 245 RVA: 0x00006494 File Offset: 0x00004694
		public ColorPickerPopupForm()
		{
			base.FormBorderStyle = FormBorderStyle.None;
			base.ShowInTaskbar = false;
			base.TopMost = true;
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(18, 18, 18);
			this._spectrum = new SpectrumBox();
			this._hue = new HueSlider();
			this._alpha = new AlphaSlider();
			this._spectrum.SVChanged += this.Spectrum_SVChanged;
			this._hue.ValueChanged += this.Hue_ValueChanged;
			this._alpha.ValueChanged += this.Alpha_ValueChanged;
			base.Controls.Add(this._spectrum);
			base.Controls.Add(this._hue);
			base.Controls.Add(this._alpha);
			base.Opacity = 0.0;
			this._fadeTimer = new Timer();
			this._fadeTimer.Interval = 15;
			this._fadeTimer.Tick += this.FadeTimer_Tick;
			base.Shown += delegate(object s, EventArgs e)
			{
				this.StartFadeIn();
			};
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000066A7 File Offset: 0x000048A7
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000066AF File Offset: 0x000048AF
		public Color PopupBackColor
		{
			get
			{
				return this.BackColor;
			}
			set
			{
				this.BackColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000066BE File Offset: 0x000048BE
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 128;
				return createParams;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000066D8 File Offset: 0x000048D8
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			try
			{
				int attr = 33;
				int num = 2;
				ColorPickerPopupForm.DwmSetWindowAttribute(base.Handle, attr, ref num, 4);
			}
			catch
			{
			}
			this.UpdateDwmBorder();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000671C File Offset: 0x0000491C
		private void UpdateDwmBorder()
		{
			if (base.IsHandleCreated)
			{
				try
				{
					int attr = 34;
					int num = ColorTranslator.ToWin32(this._popupBorderColor);
					ColorPickerPopupForm.DwmSetWindowAttribute(base.Handle, attr, ref num, 4);
				}
				catch
				{
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006768 File Offset: 0x00004968
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			this.StartFadeOutAndClose();
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000067CB File Offset: 0x000049CB
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000067D3 File Offset: 0x000049D3
		public bool ShowPopupTitle
		{
			get
			{
				return this._showPopupTitle;
			}
			set
			{
				this._showPopupTitle = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000067E2 File Offset: 0x000049E2
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000067EA File Offset: 0x000049EA
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				this._titleText = (value ?? "");
				base.Invalidate();
				base.PerformLayout();
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006808 File Offset: 0x00004A08
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00006810 File Offset: 0x00004A10
		public Image TitleImage
		{
			get
			{
				return this._titleImage;
			}
			set
			{
				this._titleImage = value;
				base.Invalidate();
				base.PerformLayout();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006825 File Offset: 0x00004A25
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000682D File Offset: 0x00004A2D
		public Padding ContentPadding
		{
			get
			{
				return this._contentPadding;
			}
			set
			{
				this._contentPadding = value;
				base.PerformLayout();
				base.Invalidate();
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006842 File Offset: 0x00004A42
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000684A File Offset: 0x00004A4A
		public int Spacing
		{
			get
			{
				return this._spacing;
			}
			set
			{
				this._spacing = Math.Max(0, value);
				base.PerformLayout();
				base.Invalidate();
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00006865 File Offset: 0x00004A65
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000686D File Offset: 0x00004A6D
		public int HeaderHeight
		{
			get
			{
				return this._headerHeight;
			}
			set
			{
				this._headerHeight = Math.Max(0, value);
				base.PerformLayout();
				base.Invalidate();
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00006888 File Offset: 0x00004A88
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00006890 File Offset: 0x00004A90
		public Point HeaderTextOffset
		{
			get
			{
				return this._headerTextOffset;
			}
			set
			{
				this._headerTextOffset = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000689F File Offset: 0x00004A9F
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000068A7 File Offset: 0x00004AA7
		public Point HeaderIconOffset
		{
			get
			{
				return this._headerIconOffset;
			}
			set
			{
				this._headerIconOffset = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000068B6 File Offset: 0x00004AB6
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000068BE File Offset: 0x00004ABE
		public int TitleIconSize
		{
			get
			{
				return this._titleIconSize;
			}
			set
			{
				this._titleIconSize = Math.Max(6, value);
				base.PerformLayout();
				base.Invalidate();
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000068D9 File Offset: 0x00004AD9
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000068E1 File Offset: 0x00004AE1
		public Color TitleIconColor
		{
			get
			{
				return this._titleIconColor;
			}
			set
			{
				this._titleIconColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000068F0 File Offset: 0x00004AF0
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000068F8 File Offset: 0x00004AF8
		public Font TitleFont
		{
			get
			{
				return this._titleFont;
			}
			set
			{
				this._titleFont = (value ?? new Font("Segoe UI", 9f, FontStyle.Bold));
				base.Invalidate();
				base.PerformLayout();
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00006921 File Offset: 0x00004B21
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00006929 File Offset: 0x00004B29
		public Color TitleForeColor
		{
			get
			{
				return this._titleForeColor;
			}
			set
			{
				this._titleForeColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006938 File Offset: 0x00004B38
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00006940 File Offset: 0x00004B40
		public Color HeaderBackColor
		{
			get
			{
				return this._headerBackColor;
			}
			set
			{
				this._headerBackColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000694F File Offset: 0x00004B4F
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00006957 File Offset: 0x00004B57
		public Color PopupBorderColor
		{
			get
			{
				return this._popupBorderColor;
			}
			set
			{
				this._popupBorderColor = value;
				this.UpdateDwmBorder();
				base.Invalidate();
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000696C File Offset: 0x00004B6C
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00006974 File Offset: 0x00004B74
		public int SliderWidth
		{
			get
			{
				return this._sliderWidth;
			}
			set
			{
				this._sliderWidth = Math.Max(10, value);
				this._hue.TrackWidth = this._sliderWidth;
				this._alpha.TrackWidth = this._sliderWidth;
				base.PerformLayout();
				base.Invalidate();
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000069B2 File Offset: 0x00004BB2
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000069BA File Offset: 0x00004BBA
		public int SliderCornerRadius
		{
			get
			{
				return this._sliderCornerRadius;
			}
			set
			{
				this._sliderCornerRadius = Math.Max(0, value);
				this._hue.CornerRadius = this._sliderCornerRadius;
				this._alpha.CornerRadius = this._sliderCornerRadius;
				base.Invalidate();
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000069F1 File Offset: 0x00004BF1
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000069F9 File Offset: 0x00004BF9
		public int SpectrumCornerRadius
		{
			get
			{
				return this._spectrumCornerRadius;
			}
			set
			{
				this._spectrumCornerRadius = Math.Max(0, value);
				this._spectrum.CornerRadius = this._spectrumCornerRadius;
				base.Invalidate();
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006A20 File Offset: 0x00004C20
		public void SetFromColor(Color c, bool raiseEvent = true)
		{
			this._suppress = true;
			this._A = (int)c.A;
			ColorUtil.ColorToHSV(c, out this._H, out this._S, out this._V);
			this._hue.Hue = this._H;
			this._alpha.Alpha = this._A;
			this._spectrum.Hue = this._H;
			this._spectrum.Sat = this._S;
			this._spectrum.Val = this._V;
			this._alpha.BaseColor = ColorUtil.ColorFromHSV(this._H, this._S, this._V, 255);
			this._suppress = false;
			if (raiseEvent)
			{
				this.RaiseColorChanged();
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006AE8 File Offset: 0x00004CE8
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			this._hue.TrackWidth = this._sliderWidth;
			this._alpha.TrackWidth = this._sliderWidth;
			this._hue.CornerRadius = this._sliderCornerRadius;
			this._alpha.CornerRadius = this._sliderCornerRadius;
			this._spectrum.CornerRadius = this._spectrumCornerRadius;
			int num = (!string.IsNullOrEmpty(this._titleText) || this._titleImage != null) ? Math.Max(0, this._headerHeight) : 0;
			this._headerRect = new Rectangle(0, 0, base.ClientSize.Width, num);
			int left = this._contentPadding.Left;
			int y = num + this._contentPadding.Top;
			int num2 = base.ClientSize.Width - this._contentPadding.Left - this._contentPadding.Right;
			int height = base.ClientSize.Height - num - this._contentPadding.Top - this._contentPadding.Bottom;
			int preferredWidth = this._hue.GetPreferredWidth();
			int num3 = this._spacing * 2;
			int num4 = Math.Max(120, num2 - (preferredWidth * 2 + num3));
			this._spectrum.SetBounds(left, y, num4, height);
			int num5 = left + num4 + this._spacing;
			this._hue.SetBounds(num5, y, preferredWidth, height);
			num5 += preferredWidth + this._spacing;
			this._alpha.SetBounds(num5, y, preferredWidth, height);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006C80 File Offset: 0x00004E80
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(this.BackColor);
			if (this._headerRect.Height > 0)
			{
				using (SolidBrush solidBrush = new SolidBrush(this._headerBackColor))
				{
					e.Graphics.FillRectangle(solidBrush, this._headerRect);
				}
				this.DrawHeader(e.Graphics, this._headerRect);
			}
			using (Pen pen = new Pen(this._popupBorderColor, 1f))
			{
				Rectangle rect = new Rectangle(0, 0, base.ClientSize.Width - 1, base.ClientSize.Height - 1);
				e.Graphics.DrawRectangle(pen, rect);
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006D60 File Offset: 0x00004F60
		private void DrawHeader(Graphics g, Rectangle r)
		{
			int num = r.Left + 6;
			int titleIconSize = this._titleIconSize;
			if (this._titleImage != null && titleIconSize > 0)
			{
				int num2 = r.Top + (r.Height - titleIconSize) / 2;
				Rectangle dest = new Rectangle(num + this._headerIconOffset.X, num2 + this._headerIconOffset.Y, titleIconSize, titleIconSize);
				ColorPickerPopupForm.DrawTintedImage(g, this._titleImage, dest, this._titleIconColor);
				num += titleIconSize + 8;
			}
			if (this._showPopupTitle && !string.IsNullOrEmpty(this._titleText))
			{
				Rectangle bounds = new Rectangle(num + this._headerTextOffset.X, r.Top + this._headerTextOffset.Y, r.Right - num - 6, r.Height);
				TextRenderer.DrawText(g, this._titleText, this._titleFont, bounds, this._titleForeColor, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006E48 File Offset: 0x00005048
		private static void DrawTintedImage(Graphics g, Image img, Rectangle dest, Color tint)
		{
			float num = (float)tint.R / 255f;
			float num2 = (float)tint.G / 255f;
			float num3 = (float)tint.B / 255f;
			float[][] array = new float[5][];
			int num4 = 0;
			float[] array2 = new float[5];
			array2[0] = num;
			array[num4] = array2;
			int num5 = 1;
			float[] array3 = new float[5];
			array3[1] = num2;
			array[num5] = array3;
			int num6 = 2;
			float[] array4 = new float[5];
			array4[2] = num3;
			array[num6] = array4;
			int num7 = 3;
			float[] array5 = new float[5];
			array5[3] = 1f;
			array[num7] = array5;
			array[4] = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			};
			ColorMatrix newColorMatrix = new ColorMatrix(array);
			using (ImageAttributes imageAttributes = new ImageAttributes())
			{
				imageAttributes.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
				InterpolationMode interpolationMode = g.InterpolationMode;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(img, dest, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
				g.InterpolationMode = interpolationMode;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006F38 File Offset: 0x00005138
		private void Spectrum_SVChanged(object sender, SVChangedEventArgs e)
		{
			if (this._suppress)
			{
				return;
			}
			this._S = e.S;
			this._V = e.V;
			this._alpha.BaseColor = ColorUtil.ColorFromHSV(this._H, this._S, this._V, 255);
			this.RaiseColorChanged();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006F94 File Offset: 0x00005194
		private void Hue_ValueChanged(object sender, EventArgs e)
		{
			if (this._suppress)
			{
				return;
			}
			this._H = this._hue.Hue;
			this._spectrum.Hue = this._H;
			this._alpha.BaseColor = ColorUtil.ColorFromHSV(this._H, this._S, this._V, 255);
			this.RaiseColorChanged();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006FF9 File Offset: 0x000051F9
		private void Alpha_ValueChanged(object sender, EventArgs e)
		{
			if (this._suppress)
			{
				return;
			}
			this._A = this._alpha.Alpha;
			this.RaiseColorChanged();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000701C File Offset: 0x0000521C
		private void RaiseColorChanged()
		{
			Color c = ColorUtil.ColorFromHSV(this._H, this._S, this._V, this._A);
			EventHandler<ColorChangedEventArgs> colorChanged = this.ColorChanged;
			if (colorChanged != null)
			{
				colorChanged(this, new ColorChangedEventArgs(c));
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000705E File Offset: 0x0000525E
		public void SetSliderThumbWidth(float width)
		{
			this._hue.ThumbWidth = width;
			this._alpha.ThumbWidth = width;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007078 File Offset: 0x00005278
		public void SetSliderThumbHeight(float height)
		{
			this._hue.ThumbHeight = height;
			this._alpha.ThumbHeight = height;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007092 File Offset: 0x00005292
		public void SetSliderThumbCornerRadius(int radius)
		{
			this._hue.ThumbCornerRadius = radius;
			this._alpha.ThumbCornerRadius = radius;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000070AC File Offset: 0x000052AC
		public void SetSliderThumbColor(Color color)
		{
			this._hue.ThumbColor = color;
			this._alpha.ThumbColor = color;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000070C6 File Offset: 0x000052C6
		public void SetSliderHoverThumbColor(Color color)
		{
			this._hue.HoverThumbColor = color;
			this._alpha.HoverThumbColor = color;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000070E0 File Offset: 0x000052E0
		public void SetSliderPressedThumbColor(Color color)
		{
			this._hue.PressedThumbColor = color;
			this._alpha.PressedThumbColor = color;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000070FA File Offset: 0x000052FA
		private void StartFadeIn()
		{
			this._fadingOut = false;
			base.Opacity = 0.0;
			this._fadeTimer.Start();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000711D File Offset: 0x0000531D
		private void StartFadeOutAndClose()
		{
			if (this._fadingOut)
			{
				return;
			}
			this._fadingOut = true;
			this._fadeTimer.Start();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000713C File Offset: 0x0000533C
		private void FadeTimer_Tick(object sender, EventArgs e)
		{
			if (!this._fadingOut)
			{
				double num = base.Opacity + 0.1;
				if (num >= 1.0)
				{
					base.Opacity = 1.0;
					this._fadeTimer.Stop();
					return;
				}
				base.Opacity = num;
				return;
			}
			else
			{
				double num2 = base.Opacity - 0.12;
				if (num2 <= 0.0)
				{
					base.Opacity = 0.0;
					this._fadeTimer.Stop();
					base.Close();
					return;
				}
				base.Opacity = num2;
				return;
			}
		}

		// Token: 0x04000093 RID: 147
		private bool _fadingOut;

		// Token: 0x04000094 RID: 148
		private bool _showPopupTitle = true;

		// Token: 0x04000095 RID: 149
		private string _titleText = "";

		// Token: 0x04000096 RID: 150
		private Image _titleImage;

		// Token: 0x04000097 RID: 151
		private Padding _contentPadding = new Padding(14);

		// Token: 0x04000098 RID: 152
		private int _spacing = 10;

		// Token: 0x04000099 RID: 153
		private int _headerHeight = 36;

		// Token: 0x0400009A RID: 154
		private Point _headerTextOffset = Point.Empty;

		// Token: 0x0400009B RID: 155
		private Point _headerIconOffset = Point.Empty;

		// Token: 0x0400009C RID: 156
		private int _titleIconSize = 18;

		// Token: 0x0400009D RID: 157
		private Color _titleIconColor = Color.White;

		// Token: 0x0400009E RID: 158
		private Font _titleFont = new Font("Segoe UI", 9f, FontStyle.Bold);

		// Token: 0x0400009F RID: 159
		private Color _titleForeColor = Color.FromArgb(235, 235, 235);

		// Token: 0x040000A0 RID: 160
		private Color _headerBackColor = Color.FromArgb(28, 28, 28);

		// Token: 0x040000A1 RID: 161
		private Color _popupBorderColor = Color.FromArgb(50, 50, 55);

		// Token: 0x040000A2 RID: 162
		private int _sliderWidth = 16;

		// Token: 0x040000A3 RID: 163
		private int _sliderCornerRadius = 8;

		// Token: 0x040000A4 RID: 164
		private int _spectrumCornerRadius = 12;

		// Token: 0x040000A5 RID: 165
		private Rectangle _headerRect;

		// Token: 0x040000A6 RID: 166
		private SpectrumBox _spectrum;

		// Token: 0x040000A7 RID: 167
		private HueSlider _hue;

		// Token: 0x040000A8 RID: 168
		private AlphaSlider _alpha;

		// Token: 0x040000A9 RID: 169
		private double _H;

		// Token: 0x040000AA RID: 170
		private double _S = 1.0;

		// Token: 0x040000AB RID: 171
		private double _V = 1.0;

		// Token: 0x040000AC RID: 172
		private int _A = 255;

		// Token: 0x040000AD RID: 173
		private bool _suppress;
	}
}
