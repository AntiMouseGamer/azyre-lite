using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Azyre.MH;
using Azyre.Utils;
using BeautyUI;
using BeautyUI.Controls;

namespace Azyre.Categories
{
	// Token: 0x02000065 RID: 101
	public class Utilities : UserControl
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00027DF6 File Offset: 0x00025FF6
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00027DFD File Offset: 0x00025FFD
		public static Utilities Static { get; set; }

		// Token: 0x0600033C RID: 828 RVA: 0x00027E08 File Offset: 0x00026008
		public Utilities()
		{
			this.InitializeComponent();
			Utilities.Static = this;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00027E88 File Offset: 0x00026088
		private async void BindButtons_MouseDown(object sender, MouseEventArgs e)
		{
			BeautyAutoButton btn = sender as BeautyAutoButton;
			if (btn == null)
			{
				return;
			}
			try
			{
				btn.Text = "...";
				Keys key = await Binds.getBind();
				Action act;
				Keys oldKey;
				if (this.currentBinds.TryGetValue(btn, out oldKey) && this.currentActions.TryGetValue(btn, out act))
				{
					List<Action> list;
					if (Binds.keybinds.TryGetValue(oldKey, out list))
					{
						list.Remove(act);
						if (list.Count == 0)
						{
							Binds.keybinds.Remove(oldKey);
							Binds.keysToCheck.Remove(oldKey);
							Binds.keyStates.Remove(oldKey);
						}
					}
					if (!Binds.keysToCheck.Contains(key))
					{
						Binds.keysToCheck.Add(key);
					}
					if (!Binds.keyStates.ContainsKey(key))
					{
						Binds.keyStates[key] = false;
					}
					if (!Binds.keybinds.ContainsKey(key))
					{
						Binds.keybinds[key] = new List<Action>();
					}
					Binds.keybinds[key].Add(act);
					this.currentBinds[btn] = key;
					btn.Text = key.ToString();
				}
				else
				{
					btn.Text = key.ToString();
				}
				dllconnect.EnviarConfiguracoes();
			}
			catch
			{
				try
				{
					btn.Text = "None";
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00027EC8 File Offset: 0x000260C8
		public void RegisterBind(BeautyAutoButton bindButton, BeautyToggleSwitch toggleSwitch, Keys bindKey, Action onToggle)
		{
			if (this.currentBinds.ContainsKey(bindButton) && this.currentActions.ContainsKey(bindButton))
			{
				Keys keys = this.currentBinds[bindButton];
				Action item = this.currentActions[bindButton];
				if (Binds.keybinds.ContainsKey(keys))
				{
					Binds.keybinds[keys].Remove(item);
					if (Binds.keybinds[keys].Count == 0)
					{
						Binds.keybinds.Remove(keys);
						Binds.keysToCheck.Remove(keys);
						Binds.keyStates.Remove(keys);
					}
				}
			}
			if (bindKey == Keys.None)
			{
				bindButton.Text = "None";
				this.currentBinds.Remove(bindButton);
				this.currentActions.Remove(bindButton);
				return;
			}
			bindButton.Text = bindKey.ToString();
			if (!Binds.keysToCheck.Contains(bindKey))
			{
				Binds.keysToCheck.Add(bindKey);
			}
			if (!Binds.keyStates.ContainsKey(bindKey))
			{
				Binds.keyStates[bindKey] = false;
			}
			if (!Binds.keybinds.ContainsKey(bindKey))
			{
				Binds.keybinds[bindKey] = new List<Action>();
			}
			Binds.keybinds[bindKey].Add(onToggle);
			this.currentBinds[bindButton] = bindKey;
			this.currentActions[bindButton] = onToggle;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbHitDelay_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00028018 File Offset: 0x00026218
		private void cbEnabled_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00028048 File Offset: 0x00026248
		private void SliderRight_Scroll(object sender, ScrollEventArgs e)
		{
			double num = (double)this.SliderRight.Value / 10.0;
			this.lbvalue.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbBlock_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbAntibot_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbTeams_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0002808D File Offset: 0x0002628D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000280AC File Offset: 0x000262AC
		private void InitializeComponent()
		{
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel7 = new BeautyPanel();
			this.beautyPanel8 = new BeautyPanel();
			this.beautyLabel8 = new BeautyLabel();
			this.cbTeams = new BeautyToggleSwitch();
			this.beautyLabel9 = new BeautyLabel();
			this.beautyPanel5 = new BeautyPanel();
			this.beautyPanel6 = new BeautyPanel();
			this.beautyLabel4 = new BeautyLabel();
			this.cbAntibot = new BeautyToggleSwitch();
			this.beautyLabel7 = new BeautyLabel();
			this.beautyPanel3 = new BeautyPanel();
			this.BindRight = new BeautyAutoButton();
			this.cbBlock = new BeautyCheckBox();
			this.beautyLabel5 = new BeautyLabel();
			this.beautyPanel4 = new BeautyPanel();
			this.beautyLabel11 = new BeautyLabel();
			this.cbEnabled = new BeautyToggleSwitch();
			this.beautyLabel1 = new BeautyLabel();
			this.SliderRight = new BeautyFlatSlider();
			this.lbvalue = new BeautyLabel();
			this.beautyLabel6 = new BeautyLabel();
			this.beautyPanel1 = new BeautyPanel();
			this.beautyPanel2 = new BeautyPanel();
			this.beautyLabel2 = new BeautyLabel();
			this.cbHitDelay = new BeautyToggleSwitch();
			this.beautyLabel3 = new BeautyLabel();
			this.DefaultPanel.SuspendLayout();
			this.beautyPanel7.SuspendLayout();
			this.beautyPanel8.SuspendLayout();
			this.beautyPanel5.SuspendLayout();
			this.beautyPanel6.SuspendLayout();
			this.beautyPanel3.SuspendLayout();
			this.beautyPanel4.SuspendLayout();
			this.beautyPanel1.SuspendLayout();
			this.beautyPanel2.SuspendLayout();
			base.SuspendLayout();
			this.DefaultPanel.AutoScroll = true;
			this.DefaultPanel.AutoScrollMinSize = new Size(0, 700);
			this.DefaultPanel.BackColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.BorderColor = Color.FromArgb(20, 22, 22);
			this.DefaultPanel.BorderSizeBottom = 0f;
			this.DefaultPanel.BorderSizeLeft = 0f;
			this.DefaultPanel.BorderSizeRight = 0f;
			this.DefaultPanel.BorderSizeTop = 0f;
			this.DefaultPanel.Controls.Add(this.beautyPanel7);
			this.DefaultPanel.Controls.Add(this.beautyPanel5);
			this.DefaultPanel.Controls.Add(this.beautyPanel3);
			this.DefaultPanel.Controls.Add(this.beautyPanel1);
			this.DefaultPanel.Dock = DockStyle.Fill;
			this.DefaultPanel.FillColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.FullHeight = 700;
			this.DefaultPanel.Location = new Point(0, 0);
			this.DefaultPanel.Name = "DefaultPanel";
			this.DefaultPanel.RadiusBottomLeft = 10f;
			this.DefaultPanel.RadiusBottomRight = 10f;
			this.DefaultPanel.RadiusTopLeft = 10f;
			this.DefaultPanel.RadiusTopRight = 10f;
			this.DefaultPanel.Scrollable = true;
			this.DefaultPanel.ScrollbarColor = Color.Firebrick;
			this.DefaultPanel.ScrollbarPadding = new Point(10, 10);
			this.DefaultPanel.ScrollbarWidth = 4;
			this.DefaultPanel.Size = new Size(570, 410);
			this.DefaultPanel.TabIndex = 846;
			this.beautyPanel7.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel7.BorderSizeBottom = 1f;
			this.beautyPanel7.BorderSizeLeft = 1f;
			this.beautyPanel7.BorderSizeRight = 1f;
			this.beautyPanel7.BorderSizeTop = 1f;
			this.beautyPanel7.Controls.Add(this.beautyPanel8);
			this.beautyPanel7.Controls.Add(this.cbTeams);
			this.beautyPanel7.Controls.Add(this.beautyLabel9);
			this.beautyPanel7.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.FullHeight = 350;
			this.beautyPanel7.Location = new Point(11, 197);
			this.beautyPanel7.Name = "beautyPanel7";
			this.beautyPanel7.RadiusBottomLeft = 6f;
			this.beautyPanel7.RadiusBottomRight = 6f;
			this.beautyPanel7.RadiusTopLeft = 6f;
			this.beautyPanel7.RadiusTopRight = 6f;
			this.beautyPanel7.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel7.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel7.ScrollbarWidth = 4;
			this.beautyPanel7.Size = new Size(260, 92);
			this.beautyPanel7.TabIndex = 914;
			this.beautyPanel8.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel8.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel8.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel8.BorderSizeBottom = 1f;
			this.beautyPanel8.BorderSizeLeft = 1f;
			this.beautyPanel8.BorderSizeRight = 1f;
			this.beautyPanel8.BorderSizeTop = 1f;
			this.beautyPanel8.Controls.Add(this.beautyLabel8);
			this.beautyPanel8.Dock = DockStyle.Top;
			this.beautyPanel8.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel8.FullHeight = 350;
			this.beautyPanel8.Location = new Point(0, 0);
			this.beautyPanel8.Name = "beautyPanel8";
			this.beautyPanel8.RadiusBottomLeft = 0f;
			this.beautyPanel8.RadiusBottomRight = 0f;
			this.beautyPanel8.RadiusTopLeft = 6f;
			this.beautyPanel8.RadiusTopRight = 6f;
			this.beautyPanel8.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel8.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel8.ScrollbarWidth = 4;
			this.beautyPanel8.Size = new Size(260, 40);
			this.beautyPanel8.TabIndex = 904;
			this.beautyLabel8.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel8.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel8.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel8.Location = new Point(10, 11);
			this.beautyLabel8.Name = "beautyLabel8";
			this.beautyLabel8.Size = new Size(51, 18);
			this.beautyLabel8.TabIndex = 905;
			this.beautyLabel8.Text = "Teams";
			this.beautyLabel8.TextPadding = new Padding(0);
			this.cbTeams.AutoRoundCorners = true;
			this.cbTeams.BackColor = Color.FromArgb(12, 14, 14);
			this.cbTeams.Checked = false;
			this.cbTeams.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbTeams.CheckedState.BorderRadius = 4;
			this.cbTeams.CheckedState.BorderThickness = 1;
			this.cbTeams.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbTeams.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbTeams.CheckedState.InnerBorderRadius = 4;
			this.cbTeams.CheckedState.InnerBorderThickness = 0;
			this.cbTeams.CheckedState.InnerColor = Color.Firebrick;
			this.cbTeams.CheckedState.InnerOffset = 2;
			this.cbTeams.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbTeams.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbTeams.LinkedLabel = this.beautyLabel9;
			this.cbTeams.Location = new Point(10, 50);
			this.cbTeams.Name = "cbTeams";
			this.cbTeams.Size = new Size(44, 22);
			this.cbTeams.TabIndex = 894;
			this.cbTeams.Text = "beautyToggleSwitch2";
			this.cbTeams.ThumbSize = 12;
			this.cbTeams.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbTeams.UncheckedState.BorderRadius = 4;
			this.cbTeams.UncheckedState.BorderThickness = 1;
			this.cbTeams.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbTeams.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbTeams.UncheckedState.InnerBorderRadius = 4;
			this.cbTeams.UncheckedState.InnerBorderThickness = 0;
			this.cbTeams.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbTeams.UncheckedState.InnerOffset = 30;
			this.cbTeams.CheckedChanged += this.cbTeams_CheckedChanged;
			this.beautyLabel9.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel9.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel9.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel9.Location = new Point(60, 53);
			this.beautyLabel9.Name = "beautyLabel9";
			this.beautyLabel9.Size = new Size(53, 18);
			this.beautyLabel9.TabIndex = 842;
			this.beautyLabel9.Text = "Enable";
			this.beautyLabel9.TextPadding = new Padding(0);
			this.beautyPanel5.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel5.BorderSizeBottom = 1f;
			this.beautyPanel5.BorderSizeLeft = 1f;
			this.beautyPanel5.BorderSizeRight = 1f;
			this.beautyPanel5.BorderSizeTop = 1f;
			this.beautyPanel5.Controls.Add(this.beautyPanel6);
			this.beautyPanel5.Controls.Add(this.cbAntibot);
			this.beautyPanel5.Controls.Add(this.beautyLabel7);
			this.beautyPanel5.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.FullHeight = 350;
			this.beautyPanel5.Location = new Point(281, 119);
			this.beautyPanel5.Name = "beautyPanel5";
			this.beautyPanel5.RadiusBottomLeft = 6f;
			this.beautyPanel5.RadiusBottomRight = 6f;
			this.beautyPanel5.RadiusTopLeft = 6f;
			this.beautyPanel5.RadiusTopRight = 6f;
			this.beautyPanel5.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel5.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel5.ScrollbarWidth = 4;
			this.beautyPanel5.Size = new Size(260, 92);
			this.beautyPanel5.TabIndex = 913;
			this.beautyPanel6.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel6.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel6.BorderSizeBottom = 1f;
			this.beautyPanel6.BorderSizeLeft = 1f;
			this.beautyPanel6.BorderSizeRight = 1f;
			this.beautyPanel6.BorderSizeTop = 1f;
			this.beautyPanel6.Controls.Add(this.beautyLabel4);
			this.beautyPanel6.Dock = DockStyle.Top;
			this.beautyPanel6.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel6.FullHeight = 350;
			this.beautyPanel6.Location = new Point(0, 0);
			this.beautyPanel6.Name = "beautyPanel6";
			this.beautyPanel6.RadiusBottomLeft = 0f;
			this.beautyPanel6.RadiusBottomRight = 0f;
			this.beautyPanel6.RadiusTopLeft = 6f;
			this.beautyPanel6.RadiusTopRight = 6f;
			this.beautyPanel6.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel6.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel6.ScrollbarWidth = 4;
			this.beautyPanel6.Size = new Size(260, 40);
			this.beautyPanel6.TabIndex = 904;
			this.beautyLabel4.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel4.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel4.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel4.Location = new Point(10, 11);
			this.beautyLabel4.Name = "beautyLabel4";
			this.beautyLabel4.Size = new Size(62, 18);
			this.beautyLabel4.TabIndex = 905;
			this.beautyLabel4.Text = "Anti Bot";
			this.beautyLabel4.TextPadding = new Padding(0);
			this.cbAntibot.AutoRoundCorners = true;
			this.cbAntibot.BackColor = Color.FromArgb(12, 14, 14);
			this.cbAntibot.Checked = false;
			this.cbAntibot.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbAntibot.CheckedState.BorderRadius = 4;
			this.cbAntibot.CheckedState.BorderThickness = 1;
			this.cbAntibot.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbAntibot.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbAntibot.CheckedState.InnerBorderRadius = 4;
			this.cbAntibot.CheckedState.InnerBorderThickness = 0;
			this.cbAntibot.CheckedState.InnerColor = Color.Firebrick;
			this.cbAntibot.CheckedState.InnerOffset = 2;
			this.cbAntibot.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbAntibot.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbAntibot.LinkedLabel = this.beautyLabel7;
			this.cbAntibot.Location = new Point(10, 50);
			this.cbAntibot.Name = "cbAntibot";
			this.cbAntibot.Size = new Size(44, 22);
			this.cbAntibot.TabIndex = 894;
			this.cbAntibot.Text = "beautyToggleSwitch2";
			this.cbAntibot.ThumbSize = 12;
			this.cbAntibot.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbAntibot.UncheckedState.BorderRadius = 4;
			this.cbAntibot.UncheckedState.BorderThickness = 1;
			this.cbAntibot.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbAntibot.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbAntibot.UncheckedState.InnerBorderRadius = 4;
			this.cbAntibot.UncheckedState.InnerBorderThickness = 0;
			this.cbAntibot.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbAntibot.UncheckedState.InnerOffset = 30;
			this.cbAntibot.CheckedChanged += this.cbAntibot_CheckedChanged;
			this.beautyLabel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel7.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel7.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel7.Location = new Point(60, 53);
			this.beautyLabel7.Name = "beautyLabel7";
			this.beautyLabel7.Size = new Size(53, 18);
			this.beautyLabel7.TabIndex = 842;
			this.beautyLabel7.Text = "Enable";
			this.beautyLabel7.TextPadding = new Padding(0);
			this.beautyPanel3.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel3.BorderSizeBottom = 1f;
			this.beautyPanel3.BorderSizeLeft = 1f;
			this.beautyPanel3.BorderSizeRight = 1f;
			this.beautyPanel3.BorderSizeTop = 1f;
			this.beautyPanel3.Controls.Add(this.BindRight);
			this.beautyPanel3.Controls.Add(this.cbBlock);
			this.beautyPanel3.Controls.Add(this.beautyLabel5);
			this.beautyPanel3.Controls.Add(this.beautyPanel4);
			this.beautyPanel3.Controls.Add(this.cbEnabled);
			this.beautyPanel3.Controls.Add(this.SliderRight);
			this.beautyPanel3.Controls.Add(this.beautyLabel6);
			this.beautyPanel3.Controls.Add(this.lbvalue);
			this.beautyPanel3.Controls.Add(this.beautyLabel1);
			this.beautyPanel3.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.FullHeight = 350;
			this.beautyPanel3.Location = new Point(11, 21);
			this.beautyPanel3.Name = "beautyPanel3";
			this.beautyPanel3.RadiusBottomLeft = 6f;
			this.beautyPanel3.RadiusBottomRight = 6f;
			this.beautyPanel3.RadiusTopLeft = 6f;
			this.beautyPanel3.RadiusTopRight = 6f;
			this.beautyPanel3.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel3.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel3.ScrollbarWidth = 4;
			this.beautyPanel3.Size = new Size(260, 170);
			this.beautyPanel3.TabIndex = 912;
			this.BindRight.AnimationSpeed = 0.6f;
			this.BindRight.BorderColor = Color.FromArgb(16, 18, 18);
			this.BindRight.BorderRadius = 4f;
			this.BindRight.BorderSize = 1f;
			this.BindRight.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.BindRight.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.BindRight.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.BindRight.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.BindRight.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.BindRight.FillColor = Color.FromArgb(16, 18, 18);
			this.BindRight.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.BindRight.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.BindRight.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.BindRight.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.BindRight.ImageOffset = new Point(0, 0);
			this.BindRight.Location = new Point(173, 50);
			this.BindRight.MinimumSize = new Size(20, 22);
			this.BindRight.MinimumTextWidth = 20;
			this.BindRight.Name = "BindRight";
			this.BindRight.Size = new Size(77, 22);
			this.BindRight.TabIndex = 909;
			this.BindRight.Text = "None";
			this.BindRight.TextOffset = new Point(0, 0);
			this.BindRight.TextPadding = new Padding(0);
			this.BindRight.YOffSet = 0;
			this.BindRight.MouseDown += this.BindButtons_MouseDown;
			this.cbBlock.AnimationSpeed = 0.6f;
			this.cbBlock.AnimationStep = 0.9999998f;
			this.cbBlock.BackColor = Color.FromArgb(12, 14, 14);
			this.cbBlock.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbBlock.BorderRadius = 2f;
			this.cbBlock.BorderSize = 1f;
			this.cbBlock.Checked = true;
			this.cbBlock.CheckedBorderColor = Color.Firebrick;
			this.cbBlock.CheckedFillColor = Color.Firebrick;
			this.cbBlock.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbBlock.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbBlock.CheckMarkScale = 0.6f;
			this.cbBlock.FillColor = Color.FromArgb(16, 18, 18);
			this.cbBlock.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbBlock.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbBlock.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbBlock.Location = new Point(228, 128);
			this.cbBlock.Name = "cbBlock";
			this.cbBlock.Size = new Size(22, 22);
			this.cbBlock.TabIndex = 907;
			this.cbBlock.TargetLabel = this.beautyLabel5;
			this.cbBlock.Text = "beautyCheckBox3";
			this.cbBlock.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbBlock.CheckedChanged += this.cbBlock_CheckedChanged;
			this.beautyLabel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel5.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel5.ForeColor = Color.FromArgb(119, 119, 129);
			this.beautyLabel5.Location = new Point(10, 130);
			this.beautyLabel5.Name = "beautyLabel5";
			this.beautyLabel5.Size = new Size(84, 18);
			this.beautyLabel5.TabIndex = 908;
			this.beautyLabel5.Text = "Blocks only";
			this.beautyLabel5.TextPadding = new Padding(0);
			this.beautyPanel4.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel4.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel4.BorderSizeBottom = 1f;
			this.beautyPanel4.BorderSizeLeft = 1f;
			this.beautyPanel4.BorderSizeRight = 1f;
			this.beautyPanel4.BorderSizeTop = 1f;
			this.beautyPanel4.Controls.Add(this.beautyLabel11);
			this.beautyPanel4.Dock = DockStyle.Top;
			this.beautyPanel4.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel4.FullHeight = 350;
			this.beautyPanel4.Location = new Point(0, 0);
			this.beautyPanel4.Name = "beautyPanel4";
			this.beautyPanel4.RadiusBottomLeft = 0f;
			this.beautyPanel4.RadiusBottomRight = 0f;
			this.beautyPanel4.RadiusTopLeft = 6f;
			this.beautyPanel4.RadiusTopRight = 6f;
			this.beautyPanel4.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel4.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel4.ScrollbarWidth = 4;
			this.beautyPanel4.Size = new Size(260, 40);
			this.beautyPanel4.TabIndex = 904;
			this.beautyLabel11.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel11.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel11.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel11.Location = new Point(10, 11);
			this.beautyLabel11.Name = "beautyLabel11";
			this.beautyLabel11.Size = new Size(93, 18);
			this.beautyLabel11.TabIndex = 905;
			this.beautyLabel11.Text = "Right Clicker";
			this.beautyLabel11.TextPadding = new Padding(0);
			this.cbEnabled.AutoRoundCorners = true;
			this.cbEnabled.BackColor = Color.FromArgb(12, 14, 14);
			this.cbEnabled.Checked = false;
			this.cbEnabled.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbEnabled.CheckedState.BorderRadius = 4;
			this.cbEnabled.CheckedState.BorderThickness = 1;
			this.cbEnabled.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbEnabled.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbEnabled.CheckedState.InnerBorderRadius = 4;
			this.cbEnabled.CheckedState.InnerBorderThickness = 0;
			this.cbEnabled.CheckedState.InnerColor = Color.Firebrick;
			this.cbEnabled.CheckedState.InnerOffset = 2;
			this.cbEnabled.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbEnabled.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbEnabled.LinkedLabel = this.beautyLabel1;
			this.cbEnabled.Location = new Point(10, 50);
			this.cbEnabled.Name = "cbEnabled";
			this.cbEnabled.Size = new Size(44, 22);
			this.cbEnabled.TabIndex = 894;
			this.cbEnabled.Text = "beautyToggleSwitch1";
			this.cbEnabled.ThumbSize = 12;
			this.cbEnabled.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbEnabled.UncheckedState.BorderRadius = 4;
			this.cbEnabled.UncheckedState.BorderThickness = 1;
			this.cbEnabled.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbEnabled.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbEnabled.UncheckedState.InnerBorderRadius = 4;
			this.cbEnabled.UncheckedState.InnerBorderThickness = 0;
			this.cbEnabled.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbEnabled.UncheckedState.InnerOffset = 30;
			this.cbEnabled.CheckedChanged += this.cbEnabled_CheckedChanged;
			this.beautyLabel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel1.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel1.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel1.Location = new Point(60, 53);
			this.beautyLabel1.Name = "beautyLabel1";
			this.beautyLabel1.Size = new Size(53, 18);
			this.beautyLabel1.TabIndex = 842;
			this.beautyLabel1.Text = "Enable";
			this.beautyLabel1.TextPadding = new Padding(0);
			this.SliderRight.AnimationTrigger = 0;
			this.SliderRight.BackColor = Color.FromArgb(12, 14, 14);
			this.SliderRight.BarColor = Color.Firebrick;
			this.SliderRight.BorderColor = Color.FromArgb(20, 22, 22);
			this.SliderRight.BorderRadius = 2f;
			this.SliderRight.BorderSize = 1;
			this.SliderRight.FillColor = Color.FromArgb(16, 18, 18);
			this.SliderRight.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.SliderRight.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.SliderRight.HoverBarColor = Color.Firebrick;
			this.SliderRight.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.SliderRight.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SliderRight.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.SliderRight.Location = new Point(10, 102);
			this.SliderRight.Maximum = 350;
			this.SliderRight.Minimum = 50;
			this.SliderRight.Name = "SliderRight";
			this.SliderRight.Offset = 1f;
			this.SliderRight.ShowText = false;
			this.SliderRight.ShowValue = true;
			this.SliderRight.Size = new Size(240, 20);
			this.SliderRight.TabIndex = 887;
			this.SliderRight.TargetLabel = this.lbvalue;
			this.SliderRight.Text = "beautyFlatSlider1";
			this.SliderRight.Value = 50;
			this.SliderRight.WriteInLabel = true;
			this.SliderRight.Scroll += this.SliderRight_Scroll;
			this.lbvalue.AutoResize = false;
			this.lbvalue.BackColor = Color.FromArgb(12, 14, 14);
			this.lbvalue.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbvalue.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbvalue.Location = new Point(178, 78);
			this.lbvalue.Name = "lbvalue";
			this.lbvalue.Size = new Size(72, 18);
			this.lbvalue.TabIndex = 889;
			this.lbvalue.Text = "5";
			this.lbvalue.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbvalue.TextPadding = new Padding(0);
			this.beautyLabel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel6.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel6.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel6.Location = new Point(10, 78);
			this.beautyLabel6.Name = "beautyLabel6";
			this.beautyLabel6.Size = new Size(38, 18);
			this.beautyLabel6.TabIndex = 888;
			this.beautyLabel6.Text = "CPS:";
			this.beautyLabel6.TextPadding = new Padding(0);
			this.beautyPanel1.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel1.BorderSizeBottom = 1f;
			this.beautyPanel1.BorderSizeLeft = 1f;
			this.beautyPanel1.BorderSizeRight = 1f;
			this.beautyPanel1.BorderSizeTop = 1f;
			this.beautyPanel1.Controls.Add(this.beautyPanel2);
			this.beautyPanel1.Controls.Add(this.cbHitDelay);
			this.beautyPanel1.Controls.Add(this.beautyLabel3);
			this.beautyPanel1.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.FullHeight = 350;
			this.beautyPanel1.Location = new Point(281, 21);
			this.beautyPanel1.Name = "beautyPanel1";
			this.beautyPanel1.RadiusBottomLeft = 6f;
			this.beautyPanel1.RadiusBottomRight = 6f;
			this.beautyPanel1.RadiusTopLeft = 6f;
			this.beautyPanel1.RadiusTopRight = 6f;
			this.beautyPanel1.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel1.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel1.ScrollbarWidth = 4;
			this.beautyPanel1.Size = new Size(260, 92);
			this.beautyPanel1.TabIndex = 911;
			this.beautyPanel2.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel2.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel2.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel2.BorderSizeBottom = 1f;
			this.beautyPanel2.BorderSizeLeft = 1f;
			this.beautyPanel2.BorderSizeRight = 1f;
			this.beautyPanel2.BorderSizeTop = 1f;
			this.beautyPanel2.Controls.Add(this.beautyLabel2);
			this.beautyPanel2.Dock = DockStyle.Top;
			this.beautyPanel2.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel2.FullHeight = 350;
			this.beautyPanel2.Location = new Point(0, 0);
			this.beautyPanel2.Name = "beautyPanel2";
			this.beautyPanel2.RadiusBottomLeft = 0f;
			this.beautyPanel2.RadiusBottomRight = 0f;
			this.beautyPanel2.RadiusTopLeft = 6f;
			this.beautyPanel2.RadiusTopRight = 6f;
			this.beautyPanel2.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel2.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel2.ScrollbarWidth = 4;
			this.beautyPanel2.Size = new Size(260, 40);
			this.beautyPanel2.TabIndex = 904;
			this.beautyLabel2.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel2.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel2.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel2.Location = new Point(10, 11);
			this.beautyLabel2.Name = "beautyLabel2";
			this.beautyLabel2.Size = new Size(91, 18);
			this.beautyLabel2.TabIndex = 905;
			this.beautyLabel2.Text = "No Hit Delay";
			this.beautyLabel2.TextPadding = new Padding(0);
			this.cbHitDelay.AutoRoundCorners = true;
			this.cbHitDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.cbHitDelay.Checked = false;
			this.cbHitDelay.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbHitDelay.CheckedState.BorderRadius = 4;
			this.cbHitDelay.CheckedState.BorderThickness = 1;
			this.cbHitDelay.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbHitDelay.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbHitDelay.CheckedState.InnerBorderRadius = 4;
			this.cbHitDelay.CheckedState.InnerBorderThickness = 0;
			this.cbHitDelay.CheckedState.InnerColor = Color.Firebrick;
			this.cbHitDelay.CheckedState.InnerOffset = 2;
			this.cbHitDelay.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbHitDelay.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbHitDelay.LinkedLabel = this.beautyLabel3;
			this.cbHitDelay.Location = new Point(10, 50);
			this.cbHitDelay.Name = "cbHitDelay";
			this.cbHitDelay.Size = new Size(44, 22);
			this.cbHitDelay.TabIndex = 894;
			this.cbHitDelay.Text = "beautyToggleSwitch2";
			this.cbHitDelay.ThumbSize = 12;
			this.cbHitDelay.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbHitDelay.UncheckedState.BorderRadius = 4;
			this.cbHitDelay.UncheckedState.BorderThickness = 1;
			this.cbHitDelay.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbHitDelay.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbHitDelay.UncheckedState.InnerBorderRadius = 4;
			this.cbHitDelay.UncheckedState.InnerBorderThickness = 0;
			this.cbHitDelay.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbHitDelay.UncheckedState.InnerOffset = 30;
			this.cbHitDelay.CheckedChanged += this.cbHitDelay_CheckedChanged;
			this.beautyLabel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel3.Location = new Point(60, 53);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(53, 18);
			this.beautyLabel3.TabIndex = 842;
			this.beautyLabel3.Text = "Enable";
			this.beautyLabel3.TextPadding = new Padding(0);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(12, 14, 14);
			base.Controls.Add(this.DefaultPanel);
			base.Name = "Utilities";
			base.Size = new Size(570, 410);
			this.DefaultPanel.ResumeLayout(false);
			this.beautyPanel7.ResumeLayout(false);
			this.beautyPanel8.ResumeLayout(false);
			this.beautyPanel5.ResumeLayout(false);
			this.beautyPanel6.ResumeLayout(false);
			this.beautyPanel3.ResumeLayout(false);
			this.beautyPanel3.PerformLayout();
			this.beautyPanel4.ResumeLayout(false);
			this.beautyPanel1.ResumeLayout(false);
			this.beautyPanel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040002F9 RID: 761
		public Keys bindRightClick;

		// Token: 0x040002FA RID: 762
		public static int RightClickBindInt;

		// Token: 0x040002FB RID: 763
		private Dictionary<BeautyAutoButton, Keys> currentBinds = new Dictionary<BeautyAutoButton, Keys>();

		// Token: 0x040002FC RID: 764
		private Dictionary<BeautyAutoButton, Action> currentActions = new Dictionary<BeautyAutoButton, Action>();

		// Token: 0x040002FD RID: 765
		private IContainer components;

		// Token: 0x040002FE RID: 766
		private BeautyPanel DefaultPanel;

		// Token: 0x040002FF RID: 767
		private BeautyPanel beautyPanel1;

		// Token: 0x04000300 RID: 768
		private BeautyPanel beautyPanel2;

		// Token: 0x04000301 RID: 769
		private BeautyLabel beautyLabel2;

		// Token: 0x04000302 RID: 770
		public BeautyToggleSwitch cbHitDelay;

		// Token: 0x04000303 RID: 771
		private BeautyLabel beautyLabel3;

		// Token: 0x04000304 RID: 772
		private BeautyPanel beautyPanel3;

		// Token: 0x04000305 RID: 773
		public BeautyCheckBox cbBlock;

		// Token: 0x04000306 RID: 774
		private BeautyLabel beautyLabel5;

		// Token: 0x04000307 RID: 775
		private BeautyPanel beautyPanel4;

		// Token: 0x04000308 RID: 776
		private BeautyLabel beautyLabel11;

		// Token: 0x04000309 RID: 777
		public BeautyToggleSwitch cbEnabled;

		// Token: 0x0400030A RID: 778
		private BeautyLabel beautyLabel1;

		// Token: 0x0400030B RID: 779
		public BeautyFlatSlider SliderRight;

		// Token: 0x0400030C RID: 780
		private BeautyLabel beautyLabel6;

		// Token: 0x0400030D RID: 781
		private BeautyPanel beautyPanel7;

		// Token: 0x0400030E RID: 782
		private BeautyPanel beautyPanel8;

		// Token: 0x0400030F RID: 783
		private BeautyLabel beautyLabel8;

		// Token: 0x04000310 RID: 784
		public BeautyToggleSwitch cbTeams;

		// Token: 0x04000311 RID: 785
		private BeautyLabel beautyLabel9;

		// Token: 0x04000312 RID: 786
		private BeautyPanel beautyPanel5;

		// Token: 0x04000313 RID: 787
		private BeautyPanel beautyPanel6;

		// Token: 0x04000314 RID: 788
		private BeautyLabel beautyLabel4;

		// Token: 0x04000315 RID: 789
		public BeautyToggleSwitch cbAntibot;

		// Token: 0x04000316 RID: 790
		private BeautyLabel beautyLabel7;

		// Token: 0x04000317 RID: 791
		public BeautyAutoButton BindRight;

		// Token: 0x04000318 RID: 792
		public BeautyLabel lbvalue;
	}
}
