namespace Azyre
{
	// Token: 0x02000021 RID: 33
	public partial class MainForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000B19D File Offset: 0x0000939D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B1BC File Offset: 0x000093BC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Azyre.MainForm));
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.beautyDragControl2 = new global::BeautyUI.BeautyDragControl(this.components);
			this.BackPanel = new global::BeautyUI.BeautyPanel();
			this.beautyDotsLoader1 = new global::BeautyUI2.Controls.BeautyDotsLoader();
			this.cuiFormRounder1 = new global::BeautyUI.Components.cuiFormRounder();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			this.BackPanel.SuspendLayout();
			base.SuspendLayout();
			this.pictureBox1.BackColor = global::System.Drawing.Color.FromArgb(12, 14, 14);
			this.pictureBox1.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("pictureBox1.Image");
			this.pictureBox1.Location = new global::System.Drawing.Point(228, 89);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(255, 218);
			this.pictureBox1.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 818;
			this.pictureBox1.TabStop = false;
			this.beautyDragControl2.TargetControl = this;
			this.BackPanel.BackColor = global::System.Drawing.Color.FromArgb(16, 18, 18);
			this.BackPanel.BorderColor = global::System.Drawing.Color.FromArgb(12, 14, 14);
			this.BackPanel.BorderSizeBottom = 0f;
			this.BackPanel.BorderSizeLeft = 0f;
			this.BackPanel.BorderSizeRight = 0f;
			this.BackPanel.BorderSizeTop = 0f;
			this.BackPanel.Controls.Add(this.beautyDotsLoader1);
			this.BackPanel.Controls.Add(this.pictureBox1);
			this.BackPanel.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BackPanel.FillColor = global::System.Drawing.Color.FromArgb(12, 14, 14);
			this.BackPanel.FullHeight = 0;
			this.BackPanel.Location = new global::System.Drawing.Point(0, 0);
			this.BackPanel.Name = "BackPanel";
			this.BackPanel.RadiusBottomLeft = 0f;
			this.BackPanel.RadiusBottomRight = 0f;
			this.BackPanel.RadiusTopLeft = 12f;
			this.BackPanel.RadiusTopRight = 0f;
			this.BackPanel.ScrollbarColor = global::System.Drawing.Color.FromArgb(100, 100, 100);
			this.BackPanel.ScrollbarPadding = new global::System.Drawing.Point(20, 2);
			this.BackPanel.Size = new global::System.Drawing.Size(720, 600);
			this.BackPanel.TabIndex = 819;
			this.beautyDotsLoader1.BackColor = global::System.Drawing.Color.FromArgb(12, 14, 14);
			this.beautyDotsLoader1.CurrentStage = 0;
			this.beautyDotsLoader1.DotColor = global::System.Drawing.Color.Firebrick;
			this.beautyDotsLoader1.DotCount = 5;
			this.beautyDotsLoader1.DotSize = 8f;
			this.beautyDotsLoader1.DotSpacing = 8f;
			this.beautyDotsLoader1.Font = new global::System.Drawing.Font("Bahnschrift", 11.25f, global::System.Drawing.FontStyle.Bold);
			this.beautyDotsLoader1.ForeColor = global::System.Drawing.Color.FromArgb(60, 60, 60);
			this.beautyDotsLoader1.InactiveDotColor = global::System.Drawing.Color.FromArgb(60, 60, 60);
			this.beautyDotsLoader1.Location = new global::System.Drawing.Point(201, 353);
			this.beautyDotsLoader1.Name = "beautyDotsLoader1";
			this.beautyDotsLoader1.ShowStageText = true;
			this.beautyDotsLoader1.Size = new global::System.Drawing.Size(317, 188);
			this.beautyDotsLoader1.StageCount = 5;
			this.beautyDotsLoader1.StageText = "Loading...";
			this.beautyDotsLoader1.TabIndex = 819;
			this.beautyDotsLoader1.Text = "beautyDotsLoader1";
			this.cuiFormRounder1.OutlineColor = global::System.Drawing.Color.FromArgb(30, 255, 255, 255);
			this.cuiFormRounder1.Rounding = 8;
			this.cuiFormRounder1.TargetForm = this;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(16, 18, 18);
			base.ClientSize = new global::System.Drawing.Size(720, 600);
			base.Controls.Add(this.BackPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MainForm";
			base.ShowIcon = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			base.Load += new global::System.EventHandler(this.MainForm_Load);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			this.BackPanel.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400010F RID: 271
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000110 RID: 272
		private global::BeautyUI.BeautyDragControl beautyDragControl2;

		// Token: 0x04000111 RID: 273
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000112 RID: 274
		private global::BeautyUI.BeautyPanel BackPanel;

		// Token: 0x04000113 RID: 275
		private global::BeautyUI2.Controls.BeautyDotsLoader beautyDotsLoader1;

		// Token: 0x04000114 RID: 276
		private global::BeautyUI.Components.cuiFormRounder cuiFormRounder1;
	}
}
