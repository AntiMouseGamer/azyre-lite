using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Azyre.Categories;
using BeautyUI;

namespace Azyre
{
	// Token: 0x0200001F RID: 31
	public class Main : UserControl
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00009454 File Offset: 0x00007654
		public Main()
		{
			this.InitializeComponent();
			Main.combatControl = new Combat();
			Main.movementControl = new Movement();
			Main.visualsControl = new Visuals();
			Main.utilitiesControl = new Utilities();
			Main.destructControl = new Destruct();
			Main.combatControl.Top = 5;
			Main.combatControl.Left = 5;
			this.DefaultPanel.Controls.Add(Main.combatControl);
			Main.selectedControl = Main.combatControl;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000094D4 File Offset: 0x000076D4
		private void ChangePage(object sender, EventArgs e)
		{
			try
			{
				UserControl next = null;
				string title = null;
				if (sender == this.CombatButton)
				{
					next = Main.combatControl;
					title = "I Combat";
				}
				else if (sender == this.MovementButton)
				{
					next = Main.movementControl;
					title = "I Movement";
				}
				else if (sender == this.VisualsButton)
				{
					next = Main.visualsControl;
					title = "I Visuals";
				}
				else if (sender == this.UtilitiesButton)
				{
					next = Main.utilitiesControl;
					title = "I Utilities";
				}
				else if (sender == this.SettingsButton)
				{
					next = Main.destructControl;
					title = "I Settings";
				}
				if (next == null || next == Main.selectedControl)
				{
					return;
				}
				if (Main.selectedControl != null && this.DefaultPanel.Controls.Contains(Main.selectedControl))
				{
					this.DefaultPanel.Controls.Remove(Main.selectedControl);
				}
				next.Top = 5;
				next.Left = 5;
				next.Dock = DockStyle.Fill;
				this.DefaultPanel.Controls.Add(next);
				Main.selectedControl = next;
				if (title != null)
				{
					this.TitleLabel.Text = title;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009513 File Offset: 0x00007713
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009534 File Offset: 0x00007734
		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Main));
			this.BackPanel = new BeautyPanel();
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel5 = new BeautyPanel();
			this.TitleLabel = new BeautyLabel();
			this.beautyLabel3 = new BeautyLabel();
			this.TabPanel = new BeautyPanel();
			this.VisualsButton = new BeautyButton();
			this.MovementButton = new BeautyButton();
			this.SettingsButton = new BeautyButton();
			this.UtilitiesButton = new BeautyButton();
			this.CombatButton = new BeautyButton();
			this.pictureBox1 = new PictureBox();
			this.beautyDragControl2 = new BeautyDragControl(this.components);
			this.Topbar = new BeautyPanel();
			this.BackPanel.SuspendLayout();
			this.beautyPanel5.SuspendLayout();
			this.TabPanel.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			this.Topbar.SuspendLayout();
			base.SuspendLayout();
			this.BackPanel.BackColor = Color.FromArgb(16, 18, 18);
			this.BackPanel.BorderColor = Color.FromArgb(12, 14, 14);
			this.BackPanel.BorderSizeBottom = 0f;
			this.BackPanel.BorderSizeLeft = 0f;
			this.BackPanel.BorderSizeRight = 0f;
			this.BackPanel.BorderSizeTop = 0f;
			this.BackPanel.Controls.Add(this.DefaultPanel);
			this.BackPanel.Controls.Add(this.beautyPanel5);
			this.BackPanel.FillColor = Color.FromArgb(12, 14, 14);
			this.BackPanel.FullHeight = 0;
			this.BackPanel.Location = new Point(100, 60);
			this.BackPanel.Name = "BackPanel";
			this.BackPanel.RadiusBottomLeft = 0f;
			this.BackPanel.RadiusBottomRight = 0f;
			this.BackPanel.RadiusTopLeft = 12f;
			this.BackPanel.RadiusTopRight = 0f;
			this.BackPanel.ScrollbarColor = Color.FromArgb(100, 100, 100);
			this.BackPanel.ScrollbarPadding = new Point(20, 2);
			this.BackPanel.Size = new Size(620, 540);
			this.BackPanel.TabIndex = 822;
			this.DefaultPanel.BackColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.BorderColor = Color.FromArgb(20, 22, 22);
			this.DefaultPanel.BorderSizeBottom = 1f;
			this.DefaultPanel.BorderSizeLeft = 1f;
			this.DefaultPanel.BorderSizeRight = 1f;
			this.DefaultPanel.BorderSizeTop = 1f;
			this.DefaultPanel.FillColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.FullHeight = 0;
			this.DefaultPanel.Location = new Point(20, 80);
			this.DefaultPanel.Name = "DefaultPanel";
			this.DefaultPanel.RadiusBottomLeft = 6f;
			this.DefaultPanel.RadiusBottomRight = 6f;
			this.DefaultPanel.RadiusTopLeft = 6f;
			this.DefaultPanel.RadiusTopRight = 6f;
			this.DefaultPanel.ScrollbarColor = Color.Firebrick;
			this.DefaultPanel.ScrollbarPadding = new Point(10, 10);
			this.DefaultPanel.ScrollbarWidth = 4;
			this.DefaultPanel.Size = new Size(580, 420);
			this.DefaultPanel.TabIndex = 907;
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(26, 28, 28);
			this.beautyPanel5.BorderSizeBottom = 0f;
			this.beautyPanel5.BorderSizeLeft = 0f;
			this.beautyPanel5.BorderSizeRight = 0f;
			this.beautyPanel5.BorderSizeTop = 0f;
			this.beautyPanel5.Controls.Add(this.TitleLabel);
			this.beautyPanel5.Controls.Add(this.beautyLabel3);
			this.beautyPanel5.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel5.FullHeight = 0;
			this.beautyPanel5.Location = new Point(20, 20);
			this.beautyPanel5.Name = "beautyPanel5";
			this.beautyPanel5.RadiusBottomLeft = 6f;
			this.beautyPanel5.RadiusBottomRight = 6f;
			this.beautyPanel5.RadiusTopLeft = 6f;
			this.beautyPanel5.RadiusTopRight = 6f;
			this.beautyPanel5.ScrollbarColor = Color.FromArgb(100, 100, 100);
			this.beautyPanel5.ScrollbarPadding = new Point(2, 2);
			this.beautyPanel5.Size = new Size(580, 50);
			this.beautyPanel5.TabIndex = 843;
			this.TitleLabel.BackColor = Color.FromArgb(16, 18, 18);
			this.TitleLabel.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.TitleLabel.ForeColor = Color.FromArgb(70, 70, 80);
			this.TitleLabel.Location = new Point(84, 17);
			this.TitleLabel.Name = "TitleLabel";
			this.TitleLabel.Size = new Size(67, 18);
			this.TitleLabel.TabIndex = 2;
			this.TitleLabel.Text = "I Combat";
			this.TitleLabel.TextPadding = new Padding(0);
			this.beautyLabel3.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.Firebrick;
			this.beautyLabel3.Location = new Point(15, 17);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(69, 18);
			this.beautyLabel3.TabIndex = 1;
			this.beautyLabel3.Text = "A Z Y R E";
			this.beautyLabel3.TextPadding = new Padding(0);
			this.TabPanel.BackColor = Color.FromArgb(16, 18, 18);
			this.TabPanel.BorderColor = Color.FromArgb(26, 28, 28);
			this.TabPanel.BorderSizeBottom = 0f;
			this.TabPanel.BorderSizeLeft = 0f;
			this.TabPanel.BorderSizeRight = 0f;
			this.TabPanel.BorderSizeTop = 0f;
			this.TabPanel.Controls.Add(this.VisualsButton);
			this.TabPanel.Controls.Add(this.MovementButton);
			this.TabPanel.Controls.Add(this.SettingsButton);
			this.TabPanel.Controls.Add(this.UtilitiesButton);
			this.TabPanel.Controls.Add(this.CombatButton);
			this.TabPanel.Dock = DockStyle.Left;
			this.TabPanel.FillColor = Color.FromArgb(16, 18, 18);
			this.TabPanel.FullHeight = 0;
			this.TabPanel.Location = new Point(0, 60);
			this.TabPanel.Name = "TabPanel";
			this.TabPanel.RadiusBottomLeft = 0f;
			this.TabPanel.RadiusBottomRight = 0f;
			this.TabPanel.RadiusTopLeft = 0f;
			this.TabPanel.RadiusTopRight = 0f;
			this.TabPanel.ScrollbarColor = Color.FromArgb(100, 100, 100);
			this.TabPanel.ScrollbarPadding = new Point(2, 2);
			this.TabPanel.Size = new Size(100, 540);
			this.TabPanel.TabIndex = 820;
			this.VisualsButton.AnimationSpeed = 0.6f;
			this.VisualsButton.BackColor = Color.FromArgb(16, 18, 18);
			this.VisualsButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.VisualsButton.BorderSize = 0f;
			this.VisualsButton.ButtonImage = (Image)componentResourceManager.GetObject("VisualsButton.ButtonImage");
			this.VisualsButton.ButtonType = (ButtonType)1;
			this.VisualsButton.CheckedBorderColor = Color.FromArgb(48, 20, 20);
			this.VisualsButton.CheckedFillColor = Color.FromArgb(48, 20, 20);
			this.VisualsButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.VisualsButton.CheckedImageColor = Color.Firebrick;
			this.VisualsButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.VisualsButton.FillColor = Color.FromArgb(16, 18, 18);
			this.VisualsButton.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.VisualsButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.VisualsButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.VisualsButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.VisualsButton.HoverImageColor = Color.FromArgb(70, 70, 80);
			this.VisualsButton.ImageAlignment = (ImageAlignment)2;
			this.VisualsButton.ImageColor = Color.FromArgb(40, 40, 50);
			this.VisualsButton.ImageOffset = new Point(0, 0);
			this.VisualsButton.ImageSize = new Size(25, 25);
			this.VisualsButton.Location = new Point(25, 225);
			this.VisualsButton.Name = "VisualsButton";
			this.VisualsButton.NewAnimation = false;
			this.VisualsButton.RadioFrequency = 69;
			this.VisualsButton.RadiusBottomLeft = 4f;
			this.VisualsButton.RadiusBottomRight = 4f;
			this.VisualsButton.RadiusTopLeft = 4f;
			this.VisualsButton.RadiusTopRight = 4f;
			this.VisualsButton.Size = new Size(50, 50);
			this.VisualsButton.TabIndex = 863;
			this.VisualsButton.TextAlignment = (TextAlignment)0;
			this.VisualsButton.TextOffset = new Point(0, 0);
			this.VisualsButton.TextPadding = new Padding(0);
			this.VisualsButton.YOffSet = 0;
			this.VisualsButton.Click += this.ChangePage;
			this.MovementButton.AnimationSpeed = 0.6f;
			this.MovementButton.BackColor = Color.FromArgb(16, 18, 18);
			this.MovementButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.MovementButton.BorderSize = 0f;
			this.MovementButton.ButtonImage = (Image)componentResourceManager.GetObject("MovementButton.ButtonImage");
			this.MovementButton.ButtonType = (ButtonType)1;
			this.MovementButton.CheckedBorderColor = Color.FromArgb(48, 20, 20);
			this.MovementButton.CheckedFillColor = Color.FromArgb(48, 20, 20);
			this.MovementButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.MovementButton.CheckedImageColor = Color.Firebrick;
			this.MovementButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.MovementButton.FillColor = Color.FromArgb(16, 18, 18);
			this.MovementButton.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.MovementButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.MovementButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.MovementButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.MovementButton.HoverImageColor = Color.FromArgb(70, 70, 80);
			this.MovementButton.ImageAlignment = (ImageAlignment)2;
			this.MovementButton.ImageColor = Color.FromArgb(40, 40, 50);
			this.MovementButton.ImageOffset = new Point(0, 0);
			this.MovementButton.ImageSize = new Size(25, 25);
			this.MovementButton.Location = new Point(25, 169);
			this.MovementButton.Name = "MovementButton";
			this.MovementButton.NewAnimation = false;
			this.MovementButton.RadioFrequency = 69;
			this.MovementButton.RadiusBottomLeft = 4f;
			this.MovementButton.RadiusBottomRight = 4f;
			this.MovementButton.RadiusTopLeft = 4f;
			this.MovementButton.RadiusTopRight = 4f;
			this.MovementButton.Size = new Size(50, 50);
			this.MovementButton.TabIndex = 862;
			this.MovementButton.TextAlignment = (TextAlignment)0;
			this.MovementButton.TextOffset = new Point(0, 0);
			this.MovementButton.TextPadding = new Padding(0);
			this.MovementButton.YOffSet = 0;
			this.MovementButton.Click += this.ChangePage;
			this.SettingsButton.AnimationSpeed = 0.6f;
			this.SettingsButton.BackColor = Color.FromArgb(16, 18, 18);
			this.SettingsButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.SettingsButton.BorderSize = 0f;
			this.SettingsButton.ButtonImage = (Image)componentResourceManager.GetObject("SettingsButton.ButtonImage");
			this.SettingsButton.ButtonType = (ButtonType)1;
			this.SettingsButton.CheckedBorderColor = Color.FromArgb(48, 20, 20);
			this.SettingsButton.CheckedFillColor = Color.FromArgb(48, 20, 20);
			this.SettingsButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.SettingsButton.CheckedImageColor = Color.Firebrick;
			this.SettingsButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.SettingsButton.FillColor = Color.FromArgb(16, 18, 18);
			this.SettingsButton.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.SettingsButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.SettingsButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SettingsButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.SettingsButton.HoverImageColor = Color.FromArgb(70, 70, 80);
			this.SettingsButton.ImageAlignment = (ImageAlignment)2;
			this.SettingsButton.ImageColor = Color.FromArgb(40, 40, 50);
			this.SettingsButton.ImageOffset = new Point(0, 0);
			this.SettingsButton.ImageSize = new Size(25, 25);
			this.SettingsButton.Location = new Point(25, 337);
			this.SettingsButton.Name = "SettingsButton";
			this.SettingsButton.NewAnimation = false;
			this.SettingsButton.RadioFrequency = 69;
			this.SettingsButton.RadiusBottomLeft = 4f;
			this.SettingsButton.RadiusBottomRight = 4f;
			this.SettingsButton.RadiusTopLeft = 4f;
			this.SettingsButton.RadiusTopRight = 4f;
			this.SettingsButton.Size = new Size(50, 50);
			this.SettingsButton.TabIndex = 860;
			this.SettingsButton.TextAlignment = (TextAlignment)0;
			this.SettingsButton.TextOffset = new Point(0, 0);
			this.SettingsButton.TextPadding = new Padding(0);
			this.SettingsButton.YOffSet = 0;
			this.SettingsButton.Click += this.ChangePage;
			this.UtilitiesButton.AnimationSpeed = 0.6f;
			this.UtilitiesButton.BackColor = Color.FromArgb(16, 18, 18);
			this.UtilitiesButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.UtilitiesButton.BorderSize = 0f;
			this.UtilitiesButton.ButtonImage = (Image)componentResourceManager.GetObject("UtilitiesButton.ButtonImage");
			this.UtilitiesButton.ButtonType = (ButtonType)1;
			this.UtilitiesButton.CheckedBorderColor = Color.FromArgb(48, 20, 20);
			this.UtilitiesButton.CheckedFillColor = Color.FromArgb(48, 20, 20);
			this.UtilitiesButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.UtilitiesButton.CheckedImageColor = Color.Firebrick;
			this.UtilitiesButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.UtilitiesButton.FillColor = Color.FromArgb(16, 18, 18);
			this.UtilitiesButton.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.UtilitiesButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.UtilitiesButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.UtilitiesButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.UtilitiesButton.HoverImageColor = Color.FromArgb(70, 70, 80);
			this.UtilitiesButton.ImageAlignment = (ImageAlignment)2;
			this.UtilitiesButton.ImageColor = Color.FromArgb(40, 40, 50);
			this.UtilitiesButton.ImageOffset = new Point(0, 0);
			this.UtilitiesButton.ImageSize = new Size(25, 25);
			this.UtilitiesButton.Location = new Point(25, 281);
			this.UtilitiesButton.Name = "UtilitiesButton";
			this.UtilitiesButton.NewAnimation = false;
			this.UtilitiesButton.RadioFrequency = 69;
			this.UtilitiesButton.RadiusBottomLeft = 4f;
			this.UtilitiesButton.RadiusBottomRight = 4f;
			this.UtilitiesButton.RadiusTopLeft = 4f;
			this.UtilitiesButton.RadiusTopRight = 4f;
			this.UtilitiesButton.Size = new Size(50, 50);
			this.UtilitiesButton.TabIndex = 859;
			this.UtilitiesButton.TextAlignment = (TextAlignment)0;
			this.UtilitiesButton.TextOffset = new Point(0, 0);
			this.UtilitiesButton.TextPadding = new Padding(0);
			this.UtilitiesButton.YOffSet = 0;
			this.UtilitiesButton.Click += this.ChangePage;
			this.CombatButton.AnimationSpeed = 0.6f;
			this.CombatButton.AnimationStep = 0.9999998f;
			this.CombatButton.BackColor = Color.FromArgb(16, 18, 18);
			this.CombatButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.CombatButton.BorderSize = 0f;
			this.CombatButton.ButtonImage = (Image)componentResourceManager.GetObject("CombatButton.ButtonImage");
			this.CombatButton.ButtonType = (ButtonType)1;
			this.CombatButton.Checked = true;
			this.CombatButton.CheckedBorderColor = Color.FromArgb(48, 20, 20);
			this.CombatButton.CheckedFillColor = Color.FromArgb(48, 20, 20);
			this.CombatButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.CombatButton.CheckedImageColor = Color.Firebrick;
			this.CombatButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.CombatButton.FillColor = Color.FromArgb(16, 18, 18);
			this.CombatButton.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.CombatButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.CombatButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.CombatButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.CombatButton.HoverImageColor = Color.FromArgb(70, 70, 80);
			this.CombatButton.ImageAlignment = (ImageAlignment)2;
			this.CombatButton.ImageColor = Color.FromArgb(40, 40, 50);
			this.CombatButton.ImageOffset = new Point(0, 0);
			this.CombatButton.ImageSize = new Size(25, 25);
			this.CombatButton.Location = new Point(25, 113);
			this.CombatButton.Name = "CombatButton";
			this.CombatButton.NewAnimation = false;
			this.CombatButton.RadioFrequency = 69;
			this.CombatButton.RadiusBottomLeft = 4f;
			this.CombatButton.RadiusBottomRight = 4f;
			this.CombatButton.RadiusTopLeft = 4f;
			this.CombatButton.RadiusTopRight = 4f;
			this.CombatButton.Size = new Size(50, 50);
			this.CombatButton.TabIndex = 812;
			this.CombatButton.TextAlignment = (TextAlignment)0;
			this.CombatButton.TextOffset = new Point(0, 0);
			this.CombatButton.TextPadding = new Padding(0);
			this.CombatButton.YOffSet = 0;
			this.CombatButton.Click += this.ChangePage;
			this.pictureBox1.BackColor = Color.FromArgb(16, 18, 18);
			this.pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
			this.pictureBox1.Location = new Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(100, 60);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 818;
			this.pictureBox1.TabStop = false;
			this.beautyDragControl2.TargetControl = this.Topbar;
			this.Topbar.BackColor = Color.FromArgb(16, 18, 18);
			this.Topbar.BorderColor = Color.FromArgb(12, 14, 14);
			this.Topbar.BorderSizeBottom = 0f;
			this.Topbar.BorderSizeLeft = 0f;
			this.Topbar.BorderSizeRight = 0f;
			this.Topbar.BorderSizeTop = 0f;
			this.Topbar.Controls.Add(this.pictureBox1);
			this.Topbar.Dock = DockStyle.Top;
			this.Topbar.FillColor = Color.FromArgb(16, 18, 18);
			this.Topbar.FullHeight = 0;
			this.Topbar.Location = new Point(0, 0);
			this.Topbar.Name = "Topbar";
			this.Topbar.RadiusBottomLeft = 0f;
			this.Topbar.RadiusBottomRight = 0f;
			this.Topbar.RadiusTopLeft = 0f;
			this.Topbar.RadiusTopRight = 0f;
			this.Topbar.ScrollbarColor = Color.FromArgb(100, 100, 100);
			this.Topbar.ScrollbarPadding = new Point(2, 2);
			this.Topbar.Size = new Size(720, 60);
			this.Topbar.TabIndex = 821;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.BackPanel);
			base.Controls.Add(this.TabPanel);
			base.Controls.Add(this.Topbar);
			base.Name = "Main";
			base.Size = new Size(720, 600);
			this.BackPanel.ResumeLayout(false);
			this.beautyPanel5.ResumeLayout(false);
			this.TabPanel.ResumeLayout(false);
			((ISupportInitialize)this.pictureBox1).EndInit();
			this.Topbar.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000F0 RID: 240
		public static UserControl selectedControl;

		// Token: 0x040000F1 RID: 241
		public static UserControl combatControl;

		// Token: 0x040000F2 RID: 242
		public static UserControl movementControl;

		// Token: 0x040000F3 RID: 243
		public static UserControl visualsControl;

		// Token: 0x040000F4 RID: 244
		public static UserControl utilitiesControl;

		// Token: 0x040000F5 RID: 245
		public static UserControl destructControl;

		// Token: 0x040000F6 RID: 246
		private IContainer components;

		// Token: 0x040000F7 RID: 247
		private BeautyPanel BackPanel;

		// Token: 0x040000F8 RID: 248
		private BeautyPanel DefaultPanel;

		// Token: 0x040000F9 RID: 249
		private BeautyPanel beautyPanel5;

		// Token: 0x040000FA RID: 250
		private BeautyLabel TitleLabel;

		// Token: 0x040000FB RID: 251
		private BeautyLabel beautyLabel3;

		// Token: 0x040000FC RID: 252
		private BeautyPanel TabPanel;

		// Token: 0x040000FD RID: 253
		public BeautyButton VisualsButton;

		// Token: 0x040000FE RID: 254
		public BeautyButton MovementButton;

		// Token: 0x040000FF RID: 255
		public BeautyButton SettingsButton;

		// Token: 0x04000100 RID: 256
		public BeautyButton UtilitiesButton;

		// Token: 0x04000101 RID: 257
		public BeautyButton CombatButton;

		// Token: 0x04000102 RID: 258
		private PictureBox pictureBox1;

		// Token: 0x04000103 RID: 259
		private BeautyDragControl beautyDragControl2;

		// Token: 0x04000104 RID: 260
		private BeautyPanel Topbar;
	}
}
