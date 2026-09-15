using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BeautyUI2.Controls
{
	// Token: 0x02000012 RID: 18
	[DefaultEvent("SelectedColorChanged")]
	public class BeautyColorPicker : Control
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00005640 File Offset: 0x00003840
		public BeautyColorPicker()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.Cursor = Cursors.Hand;
			base.Size = new Size(46, 22);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000057BD File Offset: 0x000039BD
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000057C5 File Offset: 0x000039C5
		[Category("Popup")]
		public Color PopupBackColor
		{
			get
			{
				return this._popupBackColor;
			}
			set
			{
				this._popupBackColor = value;
				if (this._popup != null)
				{
					this._popup.PopupBackColor = value;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000057E2 File Offset: 0x000039E2
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000057EA File Offset: 0x000039EA
		[Category("Popup")]
		public Color PopupBorderColor
		{
			get
			{
				return this._popupBorderColor;
			}
			set
			{
				this._popupBorderColor = value;
				if (this._popup != null)
				{
					this._popup.PopupBorderColor = value;
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005807 File Offset: 0x00003A07
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00005814 File Offset: 0x00003A14
		[Category("Behavior")]
		[DefaultValue(255)]
		public int Alpha
		{
			get
			{
				return (int)this._selectedColor.A;
			}
			set
			{
				int num = Math.Max(0, Math.Min(255, value));
				if ((int)this._selectedColor.A == num)
				{
					return;
				}
				this.SelectedColor = Color.FromArgb(num, this._selectedColor);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005854 File Offset: 0x00003A54
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000585C File Offset: 0x00003A5C
		[Category("Behavior")]
		public Color SelectedColor
		{
			get
			{
				return this._selectedColor;
			}
			set
			{
				if (this._selectedColor.ToArgb() == value.ToArgb())
				{
					return;
				}
				this._selectedColor = value;
				if (this._popup != null && !this._popup.IsDisposed && !this._internalPopupUpdate)
				{
					this._popup.SetFromColor(this._selectedColor, false);
				}
				base.Invalidate();
				this.OnSelectedColorChanged();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000058C0 File Offset: 0x00003AC0
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000058C8 File Offset: 0x00003AC8
		[Category("Appearance")]
		[DefaultValue(10)]
		public int Radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = Math.Max(0, value);
				base.Invalidate();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000058DD File Offset: 0x00003ADD
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000058E5 File Offset: 0x00003AE5
		[Category("Popup")]
		public Padding PopupPadding
		{
			get
			{
				return this._popupPadding;
			}
			set
			{
				this._popupPadding = value;
				if (this._popup != null)
				{
					this._popup.ContentPadding = value;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005902 File Offset: 0x00003B02
		// (set) Token: 0x060000BA RID: 186 RVA: 0x0000590A File Offset: 0x00003B0A
		[Category("Popup")]
		[DefaultValue(10)]
		public int PopupSpacing
		{
			get
			{
				return this._popupSpacing;
			}
			set
			{
				this._popupSpacing = Math.Max(0, value);
				if (this._popup != null)
				{
					this._popup.Spacing = this._popupSpacing;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005932 File Offset: 0x00003B32
		// (set) Token: 0x060000BC RID: 188 RVA: 0x0000593A File Offset: 0x00003B3A
		[Category("Popup")]
		public Size PopupSize
		{
			get
			{
				return this._popupSize;
			}
			set
			{
				this._popupSize = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005943 File Offset: 0x00003B43
		// (set) Token: 0x060000BE RID: 190 RVA: 0x0000594B File Offset: 0x00003B4B
		[Category("Popup Title")]
		[DefaultValue(true)]
		public bool ShowPopupTitle
		{
			get
			{
				return this._showPopupTitle;
			}
			set
			{
				this._showPopupTitle = value;
				if (this._popup != null)
				{
					this._popup.ShowPopupTitle = value;
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005968 File Offset: 0x00003B68
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005970 File Offset: 0x00003B70
		[Category("Popup Title")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return this._titleText;
			}
			set
			{
				this._titleText = (value ?? string.Empty);
				if (this._popup != null)
				{
					this._popup.TitleText = this._titleText;
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000599B File Offset: 0x00003B9B
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000059A3 File Offset: 0x00003BA3
		[Category("Popup Title")]
		[Browsable(true)]
		public Image Image
		{
			get
			{
				return this._titleImage;
			}
			set
			{
				this._titleImage = value;
				if (this._popup != null)
				{
					this._popup.TitleImage = this._titleImage;
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000059C5 File Offset: 0x00003BC5
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000059CD File Offset: 0x00003BCD
		[Category("Popup Header")]
		[DefaultValue(36)]
		public int PopupHeaderHeight
		{
			get
			{
				return this._popupHeaderHeight;
			}
			set
			{
				this._popupHeaderHeight = Math.Max(0, value);
				if (this._popup != null)
				{
					this._popup.HeaderHeight = this._popupHeaderHeight;
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000059F5 File Offset: 0x00003BF5
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000059FD File Offset: 0x00003BFD
		[Category("Popup Header")]
		public Point PopupHeaderTextOffset
		{
			get
			{
				return this._popupHeaderTextOffset;
			}
			set
			{
				this._popupHeaderTextOffset = value;
				if (this._popup != null)
				{
					this._popup.HeaderTextOffset = value;
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005A1A File Offset: 0x00003C1A
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005A22 File Offset: 0x00003C22
		[Category("Popup Header")]
		public Point PopupHeaderIconOffset
		{
			get
			{
				return this._popupHeaderIconOffset;
			}
			set
			{
				this._popupHeaderIconOffset = value;
				if (this._popup != null)
				{
					this._popup.HeaderIconOffset = value;
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005A3F File Offset: 0x00003C3F
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00005A47 File Offset: 0x00003C47
		[Category("Popup Header")]
		[DefaultValue(18)]
		public int PopupTitleIconSize
		{
			get
			{
				return this._popupTitleIconSize;
			}
			set
			{
				this._popupTitleIconSize = Math.Max(6, value);
				if (this._popup != null)
				{
					this._popup.TitleIconSize = this._popupTitleIconSize;
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005A6F File Offset: 0x00003C6F
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00005A77 File Offset: 0x00003C77
		[Category("Popup Header")]
		public Color PopupTitleIconColor
		{
			get
			{
				return this._popupTitleIconColor;
			}
			set
			{
				this._popupTitleIconColor = value;
				if (this._popup != null)
				{
					this._popup.TitleIconColor = value;
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005A94 File Offset: 0x00003C94
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00005A9C File Offset: 0x00003C9C
		[Category("Popup Header")]
		public Font PopupTitleFont
		{
			get
			{
				return this._popupTitleFont;
			}
			set
			{
				this._popupTitleFont = (value ?? new Font("Segoe UI", 9f, FontStyle.Bold));
				if (this._popup != null)
				{
					this._popup.TitleFont = this._popupTitleFont;
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005AD2 File Offset: 0x00003CD2
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00005ADA File Offset: 0x00003CDA
		[Category("Popup Header")]
		public Color PopupTitleForeColor
		{
			get
			{
				return this._popupTitleForeColor;
			}
			set
			{
				this._popupTitleForeColor = value;
				if (this._popup != null)
				{
					this._popup.TitleForeColor = value;
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005AF7 File Offset: 0x00003CF7
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00005AFF File Offset: 0x00003CFF
		[Category("Popup Header")]
		public Color PopupHeaderBackColor
		{
			get
			{
				return this._popupHeaderBackColor;
			}
			set
			{
				this._popupHeaderBackColor = value;
				if (this._popup != null)
				{
					this._popup.HeaderBackColor = value;
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005B1C File Offset: 0x00003D1C
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00005B24 File Offset: 0x00003D24
		[Category("Popup Bars")]
		[DefaultValue(16)]
		public int PopupSliderWidth
		{
			get
			{
				return this._popupSliderWidth;
			}
			set
			{
				this._popupSliderWidth = Math.Max(10, value);
				if (this._popup != null)
				{
					this._popup.SliderWidth = this._popupSliderWidth;
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005B4D File Offset: 0x00003D4D
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005B55 File Offset: 0x00003D55
		[Category("Popup Bars")]
		[DefaultValue(8)]
		public int PopupSliderCornerRadius
		{
			get
			{
				return this._popupSliderCornerRadius;
			}
			set
			{
				this._popupSliderCornerRadius = Math.Max(0, value);
				if (this._popup != null)
				{
					this._popup.SliderCornerRadius = this._popupSliderCornerRadius;
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005B7D File Offset: 0x00003D7D
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00005B85 File Offset: 0x00003D85
		[Category("Popup Spectrum")]
		[DefaultValue(12)]
		public int PopupSpectrumCornerRadius
		{
			get
			{
				return this._popupSpectrumCornerRadius;
			}
			set
			{
				this._popupSpectrumCornerRadius = Math.Max(0, value);
				if (this._popup != null)
				{
					this._popup.SpectrumCornerRadius = this._popupSpectrumCornerRadius;
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005BAD File Offset: 0x00003DAD
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00005BB5 File Offset: 0x00003DB5
		[Category("Slider Thumb")]
		[DefaultValue(14f)]
		public float SliderThumbWidth
		{
			get
			{
				return this._sliderThumbWidth;
			}
			set
			{
				this._sliderThumbWidth = Math.Max(2f, value);
				if (this._popup != null)
				{
					this._popup.SetSliderThumbWidth(this._sliderThumbWidth);
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005BE1 File Offset: 0x00003DE1
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00005BE9 File Offset: 0x00003DE9
		[Category("Slider Thumb")]
		[DefaultValue(14f)]
		public float SliderThumbHeight
		{
			get
			{
				return this._sliderThumbHeight;
			}
			set
			{
				this._sliderThumbHeight = Math.Max(2f, value);
				if (this._popup != null)
				{
					this._popup.SetSliderThumbHeight(this._sliderThumbHeight);
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00005C15 File Offset: 0x00003E15
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00005C1D File Offset: 0x00003E1D
		[Category("Slider Thumb")]
		[DefaultValue(7)]
		public int SliderThumbCornerRadius
		{
			get
			{
				return this._sliderThumbCornerRadius;
			}
			set
			{
				this._sliderThumbCornerRadius = Math.Max(0, value);
				if (this._popup != null)
				{
					this._popup.SetSliderThumbCornerRadius(this._sliderThumbCornerRadius);
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00005C45 File Offset: 0x00003E45
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00005C4D File Offset: 0x00003E4D
		[Category("Slider Thumb")]
		public Color SliderThumbColor
		{
			get
			{
				return this._sliderThumbColor;
			}
			set
			{
				this._sliderThumbColor = value;
				if (this._popup != null)
				{
					this._popup.SetSliderThumbColor(this._sliderThumbColor);
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005C6F File Offset: 0x00003E6F
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00005C77 File Offset: 0x00003E77
		[Category("Slider Thumb")]
		public Color SliderHoverThumbColor
		{
			get
			{
				return this._sliderHoverThumbColor;
			}
			set
			{
				this._sliderHoverThumbColor = value;
				if (this._popup != null)
				{
					this._popup.SetSliderHoverThumbColor(this._sliderHoverThumbColor);
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005C99 File Offset: 0x00003E99
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00005CA1 File Offset: 0x00003EA1
		[Category("Slider Thumb")]
		public Color SliderPressedThumbColor
		{
			get
			{
				return this._sliderPressedThumbColor;
			}
			set
			{
				this._sliderPressedThumbColor = value;
				if (this._popup != null)
				{
					this._popup.SetSliderPressedThumbColor(this._sliderPressedThumbColor);
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000E5 RID: 229 RVA: 0x00005CC4 File Offset: 0x00003EC4
		// (remove) Token: 0x060000E6 RID: 230 RVA: 0x00005CFC File Offset: 0x00003EFC
		public event EventHandler SelectedColorChanged;

		// Token: 0x060000E7 RID: 231 RVA: 0x00005D34 File Offset: 0x00003F34
		protected virtual void OnSelectedColorChanged()
		{
			EventHandler selectedColorChanged = this.SelectedColorChanged;
			if (selectedColorChanged != null)
			{
				selectedColorChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005D58 File Offset: 0x00003F58
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			RectangleF rect = new RectangleF(0f, 0f, (float)base.Width, (float)base.Height);
			using (GraphicsPath graphicsPath = BeautyColorPicker.CreateRoundedRectanglePath(rect, this._radius))
			{
				using (Region region = new Region(graphicsPath))
				{
					graphics.Clip = region;
					BeautyColorPicker.DrawCheckerboard(graphics, rect, 8);
					using (SolidBrush solidBrush = new SolidBrush(this._selectedColor))
					{
						graphics.FillRectangle(solidBrush, rect);
					}
					graphics.ResetClip();
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005E24 File Offset: 0x00004024
		private static void DrawCheckerboard(Graphics g, RectangleF rect, int cell)
		{
			Color color = Color.FromArgb(45, 45, 45);
			Color color2 = Color.FromArgb(65, 65, 65);
			for (float num = rect.Top; num < rect.Bottom; num += (float)cell)
			{
				for (float num2 = rect.Left; num2 < rect.Right; num2 += (float)cell)
				{
					using (SolidBrush solidBrush = new SolidBrush((((int)(num2 / (float)cell) + (int)(num / (float)cell)) % 2 == 1) ? color2 : color))
					{
						g.FillRectangle(solidBrush, num2, num, (float)cell, (float)cell);
					}
				}
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005EC4 File Offset: 0x000040C4
		private static GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int radius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (radius <= 0)
			{
				graphicsPath.AddRectangle(rect);
				graphicsPath.CloseFigure();
				return graphicsPath;
			}
			float num = (float)(radius * 2);
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

		// Token: 0x060000EB RID: 235 RVA: 0x00005F7F File Offset: 0x0000417F
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			this.ShowPopup();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005F9C File Offset: 0x0000419C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._popup != null)
			{
				try
				{
					this._popup.Close();
				}
				catch
				{
				}
				this._popup = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005FE4 File Offset: 0x000041E4
		private void ApplyAllPopupSettings(ColorPickerPopupForm p)
		{
			p.ShowPopupTitle = this._showPopupTitle;
			p.TitleText = this._titleText;
			p.TitleImage = this._titleImage;
			p.ContentPadding = this._popupPadding;
			p.Spacing = this._popupSpacing;
			p.HeaderHeight = this._popupHeaderHeight;
			p.HeaderTextOffset = this._popupHeaderTextOffset;
			p.HeaderIconOffset = this._popupHeaderIconOffset;
			p.TitleIconSize = this._popupTitleIconSize;
			p.TitleIconColor = this._popupTitleIconColor;
			p.TitleFont = this._popupTitleFont;
			p.TitleForeColor = this._popupTitleForeColor;
			p.SliderWidth = this._popupSliderWidth;
			p.SliderCornerRadius = this._popupSliderCornerRadius;
			p.SpectrumCornerRadius = this._popupSpectrumCornerRadius;
			p.PopupBorderColor = this._popupBorderColor;
			p.HeaderBackColor = this._popupHeaderBackColor;
			p.PopupBackColor = this._popupBackColor;
			p.SetSliderThumbWidth(this._sliderThumbWidth);
			p.SetSliderThumbHeight(this._sliderThumbHeight);
			p.SetSliderThumbCornerRadius(this._sliderThumbCornerRadius);
			p.SetSliderThumbColor(this._sliderThumbColor);
			p.SetSliderHoverThumbColor(this._sliderHoverThumbColor);
			p.SetSliderPressedThumbColor(this._sliderPressedThumbColor);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006114 File Offset: 0x00004314
		private void ShowPopup()
		{
			if (this._popup != null && !this._popup.IsDisposed)
			{
				this.ApplyAllPopupSettings(this._popup);
				this.PositionPopup(this._popup);
				this._popup.BringToFront();
				this._popup.Activate();
				return;
			}
			Form form = base.FindForm();
			this._popup = new ColorPickerPopupForm();
			this.ApplyAllPopupSettings(this._popup);
			this._popup.SetFromColor(this._selectedColor, true);
			this._popup.ColorChanged += this.Popup_ColorChanged;
			this._popup.FormClosed += delegate(object s, FormClosedEventArgs e)
			{
				try
				{
					this._popup.ColorChanged -= this.Popup_ColorChanged;
				}
				catch
				{
				}
				this._popup = null;
			};
			this._popup.Size = this._popupSize;
			this._popup.StartPosition = FormStartPosition.Manual;
			this.PositionPopup(this._popup);
			if (form != null)
			{
				this._popup.Show(form);
				return;
			}
			this._popup.Show();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006208 File Offset: 0x00004408
		private void PositionPopup(Form popup)
		{
			Form form = base.FindForm();
			if (form != null)
			{
				int num = form.Left + (form.Width - popup.Width) / 2;
				int num2 = form.Top + (form.Height - popup.Height) / 2;
				Rectangle workingArea = Screen.FromControl(form).WorkingArea;
				if (num < workingArea.Left)
				{
					num = workingArea.Left + 6;
				}
				if (num2 < workingArea.Top)
				{
					num2 = workingArea.Top + 6;
				}
				if (num + popup.Width > workingArea.Right)
				{
					num = workingArea.Right - popup.Width - 6;
				}
				if (num2 + popup.Height > workingArea.Bottom)
				{
					num2 = workingArea.Bottom - popup.Height - 6;
				}
				popup.Location = new Point(num, num2);
				return;
			}
			Point point = base.PointToScreen(new Point(base.Width / 2, base.Height / 2));
			Rectangle workingArea2 = Screen.FromPoint(point).WorkingArea;
			int num3 = point.X - popup.Width / 2;
			int num4 = point.Y - popup.Height / 2;
			if (num3 < workingArea2.Left)
			{
				num3 = workingArea2.Left + 6;
			}
			if (num4 < workingArea2.Top)
			{
				num4 = workingArea2.Top + 6;
			}
			if (num3 + popup.Width > workingArea2.Right)
			{
				num3 = workingArea2.Right - popup.Width - 6;
			}
			if (num4 + popup.Height > workingArea2.Bottom)
			{
				num4 = workingArea2.Bottom - popup.Height - 6;
			}
			popup.Location = new Point(num3, num4);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000063A8 File Offset: 0x000045A8
		private void Popup_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			this._internalPopupUpdate = true;
			try
			{
				this.SelectedColor = e.Color;
			}
			finally
			{
				this._internalPopupUpdate = false;
			}
		}

		// Token: 0x04000074 RID: 116
		private Color _selectedColor = Color.FromArgb(255, 255, 0, 0);

		// Token: 0x04000075 RID: 117
		private int _radius = 10;

		// Token: 0x04000076 RID: 118
		private Padding _popupPadding = new Padding(14);

		// Token: 0x04000077 RID: 119
		private int _popupSpacing = 10;

		// Token: 0x04000078 RID: 120
		private Size _popupSize = new Size(320, 280);

		// Token: 0x04000079 RID: 121
		private bool _showPopupTitle = true;

		// Token: 0x0400007A RID: 122
		private string _titleText = "Menu accent";

		// Token: 0x0400007B RID: 123
		private Image _titleImage;

		// Token: 0x0400007C RID: 124
		private int _popupHeaderHeight = 36;

		// Token: 0x0400007D RID: 125
		private Point _popupHeaderTextOffset = Point.Empty;

		// Token: 0x0400007E RID: 126
		private Point _popupHeaderIconOffset = Point.Empty;

		// Token: 0x0400007F RID: 127
		private int _popupTitleIconSize = 18;

		// Token: 0x04000080 RID: 128
		private Color _popupTitleIconColor = Color.White;

		// Token: 0x04000081 RID: 129
		private Font _popupTitleFont = new Font("Segoe UI", 9f, FontStyle.Bold);

		// Token: 0x04000082 RID: 130
		private Color _popupTitleForeColor = Color.FromArgb(235, 235, 235);

		// Token: 0x04000083 RID: 131
		private Color _popupHeaderBackColor = Color.FromArgb(28, 28, 28);

		// Token: 0x04000084 RID: 132
		private int _popupSliderWidth = 16;

		// Token: 0x04000085 RID: 133
		private int _popupSliderCornerRadius = 8;

		// Token: 0x04000086 RID: 134
		private int _popupSpectrumCornerRadius = 12;

		// Token: 0x04000087 RID: 135
		private float _sliderThumbWidth = 14f;

		// Token: 0x04000088 RID: 136
		private float _sliderThumbHeight = 14f;

		// Token: 0x04000089 RID: 137
		private int _sliderThumbCornerRadius = 7;

		// Token: 0x0400008A RID: 138
		private Color _sliderThumbColor = Color.White;

		// Token: 0x0400008B RID: 139
		private Color _sliderHoverThumbColor = Color.White;

		// Token: 0x0400008C RID: 140
		private Color _sliderPressedThumbColor = Color.White;

		// Token: 0x0400008D RID: 141
		private Color _popupBackColor = Color.FromArgb(18, 18, 18);

		// Token: 0x0400008E RID: 142
		private Color _popupBorderColor = Color.FromArgb(50, 50, 55);

		// Token: 0x0400008F RID: 143
		private bool _internalPopupUpdate;

		// Token: 0x04000090 RID: 144
		private ColorPickerPopupForm _popup;
	}
}
