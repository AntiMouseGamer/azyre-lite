using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BeautyUI2.Controls
{
	// Token: 0x0200001E RID: 30
	[ToolboxItem(true)]
	[Description("Animated loading dots with stages.")]
	public class BeautyDotsLoader : Control
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000905C File Offset: 0x0000725C
		public BeautyDotsLoader()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			base.Size = new Size(160, 50);
			this._animationTimer = new Timer();
			this._animationTimer.Interval = 16;
			this._animationTimer.Tick += this.AnimationTick;
			this._animationTimer.Start();
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000912F File Offset: 0x0000732F
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00009137 File Offset: 0x00007337
		[Category("Appearance")]
		public int DotCount
		{
			get
			{
				return this._dotCount;
			}
			set
			{
				this._dotCount = Math.Max(1, value);
				base.Invalidate();
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000914C File Offset: 0x0000734C
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00009154 File Offset: 0x00007354
		[Category("Appearance")]
		public float DotSize
		{
			get
			{
				return this._dotSize;
			}
			set
			{
				this._dotSize = Math.Max(2f, value);
				base.Invalidate();
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000916D File Offset: 0x0000736D
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00009175 File Offset: 0x00007375
		[Category("Appearance")]
		public float DotSpacing
		{
			get
			{
				return this._dotSpacing;
			}
			set
			{
				this._dotSpacing = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00009184 File Offset: 0x00007384
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000918C File Offset: 0x0000738C
		[Category("Colors")]
		public Color DotColor
		{
			get
			{
				return this._dotColor;
			}
			set
			{
				this._dotColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000919B File Offset: 0x0000739B
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000091A3 File Offset: 0x000073A3
		[Category("Colors")]
		public Color InactiveDotColor
		{
			get
			{
				return this._inactiveDotColor;
			}
			set
			{
				this._inactiveDotColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000091B2 File Offset: 0x000073B2
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000091BA File Offset: 0x000073BA
		[Category("Stages")]
		public int StageCount
		{
			get
			{
				return this._stageCount;
			}
			set
			{
				this._stageCount = Math.Max(1, value);
				base.Invalidate();
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000091CF File Offset: 0x000073CF
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000091D7 File Offset: 0x000073D7
		[Category("Stages")]
		public int CurrentStage
		{
			get
			{
				return this._currentStage;
			}
			set
			{
				this._currentStage = Math.Max(0, Math.Min(this._stageCount, value));
				base.Invalidate();
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000091F7 File Offset: 0x000073F7
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000091FF File Offset: 0x000073FF
		[Category("Stages")]
		public string StageText
		{
			get
			{
				return this._stageText;
			}
			set
			{
				this._stageText = value;
				base.Invalidate();
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000920E File Offset: 0x0000740E
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00009216 File Offset: 0x00007416
		[Category("Stages")]
		public bool ShowStageText
		{
			get
			{
				return this._showStageText;
			}
			set
			{
				this._showStageText = value;
				base.Invalidate();
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009225 File Offset: 0x00007425
		private void AnimationTick(object sender, EventArgs e)
		{
			this._time += 0.12f;
			base.Invalidate();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009240 File Offset: 0x00007440
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			float num = (float)this._dotCount * this._dotSize + (float)(this._dotCount - 1) * this._dotSpacing;
			float num2 = ((float)base.Width - num) / 2f;
			float num3 = (float)base.Height / 2f - 6f;
			for (int i = 0; i < this._dotCount; i++)
			{
				float num4 = (float)i * 0.5f;
				float num5 = (float)Math.Sin((double)(this._time + num4)) * 6f;
				float x = num2 + (float)i * (this._dotSize + this._dotSpacing);
				float y = num3 - this._dotSize / 2f - num5;
				Color color;
				if (i < this._currentStage)
				{
					color = this._dotColor;
				}
				else
				{
					color = this._inactiveDotColor;
				}
				using (SolidBrush solidBrush = new SolidBrush(color))
				{
					graphics.FillEllipse(solidBrush, x, y, this._dotSize, this._dotSize);
				}
			}
			this.DrawStageText(graphics);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009370 File Offset: 0x00007570
		private void DrawStageText(Graphics g)
		{
			if (!this._showStageText)
			{
				return;
			}
			using (SolidBrush solidBrush = new SolidBrush(this.ForeColor))
			{
				string text = string.Format("{0} ({1}/{2})", this._stageText, this._currentStage, this._stageCount);
				SizeF sizeF = g.MeasureString(text, this.Font);
				float x = ((float)base.Width - sizeF.Width) / 2f;
				float y = (float)base.Height - sizeF.Height - 2f;
				g.DrawString(text, this.Font, solidBrush, x, y);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009424 File Offset: 0x00007624
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Timer animationTimer = this._animationTimer;
				if (animationTimer != null)
				{
					animationTimer.Stop();
				}
				Timer animationTimer2 = this._animationTimer;
				if (animationTimer2 != null)
				{
					animationTimer2.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040000E4 RID: 228
		private Timer _animationTimer;

		// Token: 0x040000E5 RID: 229
		private int _dotCount = 5;

		// Token: 0x040000E6 RID: 230
		private float _dotSize = 8f;

		// Token: 0x040000E7 RID: 231
		private float _dotSpacing = 8f;

		// Token: 0x040000E8 RID: 232
		private float _time;

		// Token: 0x040000E9 RID: 233
		private Color _dotColor = Color.FromArgb(135, 135, 255);

		// Token: 0x040000EA RID: 234
		private Color _inactiveDotColor = Color.FromArgb(60, 60, 60);

		// Token: 0x040000EB RID: 235
		private int _stageCount = 5;

		// Token: 0x040000EC RID: 236
		private int _currentStage;

		// Token: 0x040000ED RID: 237
		private string _stageText = "Loading...";

		// Token: 0x040000EE RID: 238
		private bool _showStageText = true;

		// Token: 0x040000EF RID: 239
		private IContainer components;
	}
}
