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
	// Token: 0x02000060 RID: 96
	public class Movement : UserControl
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00023F56 File Offset: 0x00022156
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00023F5D File Offset: 0x0002215D
		public static Movement Static { get; set; }

		// Token: 0x06000321 RID: 801 RVA: 0x00023F68 File Offset: 0x00022168
		public Movement()
		{
			this.InitializeComponent();
			Movement.Static = this;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00023FE8 File Offset: 0x000221E8
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

		// Token: 0x06000323 RID: 803 RVA: 0x00024028 File Offset: 0x00022228
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

		// Token: 0x06000324 RID: 804 RVA: 0x00024178 File Offset: 0x00022378
		private void cbJumpDelay_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbBridge_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbRightBA_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000241A8 File Offset: 0x000223A8
		private void edgeOffset_Scroll(object sender, ScrollEventArgs e)
		{
			float value = (float)this.edgeOffset.Value / 100f * 0.3f;
			this.edglb.Text = value.ToString("0.00", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
			Console.WriteLine(value);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000241F8 File Offset: 0x000223F8
		private void unsneakDelay_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.unsneakDelay.Value;
			this.delaylb.Text = value.ToString() + "ms";
			dllconnect.EnviarConfiguracoes();
			Console.WriteLine(value);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbRandomize_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbSneakKeyPressed_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbHoldingBlocks_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbLookingDown_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbSprint_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbAutoSwap_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00024238 File Offset: 0x00022438
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00024258 File Offset: 0x00022458
		private void InitializeComponent()
		{
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel7 = new BeautyPanel();
			this.btSprint = new BeautyAutoButton();
			this.beautyPanel8 = new BeautyPanel();
			this.beautyLabel9 = new BeautyLabel();
			this.cbSprint = new BeautyToggleSwitch();
			this.beautyLabel10 = new BeautyLabel();
			this.beautyPanel5 = new BeautyPanel();
			this.cbLookingDown = new BeautyCheckBox();
			this.beautyLabel19 = new BeautyLabel();
			this.cbHoldingBlocks = new BeautyCheckBox();
			this.beautyLabel18 = new BeautyLabel();
			this.cbSneakKeyPressed = new BeautyCheckBox();
			this.beautyLabel17 = new BeautyLabel();
			this.cbRandomize = new BeautyCheckBox();
			this.beautyLabel16 = new BeautyLabel();
			this.unsneakDelay = new BeautyFlatSlider();
			this.delaylb = new BeautyLabel();
			this.beautyLabel15 = new BeautyLabel();
			this.edgeOffset = new BeautyFlatSlider();
			this.edglb = new BeautyLabel();
			this.beautyLabel13 = new BeautyLabel();
			this.bindBridge = new BeautyAutoButton();
			this.cbSneakOnJump = new BeautyCheckBox();
			this.beautyLabel4 = new BeautyLabel();
			this.beautyPanel6 = new BeautyPanel();
			this.beautyLabel7 = new BeautyLabel();
			this.cbBridge = new BeautyToggleSwitch();
			this.beautyLabel8 = new BeautyLabel();
			this.beautyPanel1 = new BeautyPanel();
			this.btJump = new BeautyAutoButton();
			this.beautyPanel2 = new BeautyPanel();
			this.beautyLabel2 = new BeautyLabel();
			this.cbJumpDelay = new BeautyToggleSwitch();
			this.beautyLabel3 = new BeautyLabel();
			this.cbAutoSwap = new BeautyCheckBox();
			this.beautyLabel1 = new BeautyLabel();
			this.DefaultPanel.SuspendLayout();
			this.beautyPanel7.SuspendLayout();
			this.beautyPanel8.SuspendLayout();
			this.beautyPanel5.SuspendLayout();
			this.beautyPanel6.SuspendLayout();
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
			this.beautyPanel7.Controls.Add(this.btSprint);
			this.beautyPanel7.Controls.Add(this.beautyPanel8);
			this.beautyPanel7.Controls.Add(this.cbSprint);
			this.beautyPanel7.Controls.Add(this.beautyLabel10);
			this.beautyPanel7.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.FullHeight = 350;
			this.beautyPanel7.Location = new Point(281, 119);
			this.beautyPanel7.Name = "beautyPanel7";
			this.beautyPanel7.RadiusBottomLeft = 6f;
			this.beautyPanel7.RadiusBottomRight = 6f;
			this.beautyPanel7.RadiusTopLeft = 6f;
			this.beautyPanel7.RadiusTopRight = 6f;
			this.beautyPanel7.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel7.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel7.ScrollbarWidth = 4;
			this.beautyPanel7.Size = new Size(260, 92);
			this.beautyPanel7.TabIndex = 913;
			this.btSprint.AnimationSpeed = 0.6f;
			this.btSprint.BorderColor = Color.FromArgb(16, 18, 18);
			this.btSprint.BorderRadius = 4f;
			this.btSprint.BorderSize = 1f;
			this.btSprint.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.btSprint.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.btSprint.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.btSprint.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.btSprint.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.btSprint.FillColor = Color.FromArgb(16, 18, 18);
			this.btSprint.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.btSprint.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.btSprint.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.btSprint.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.btSprint.ImageOffset = new Point(0, 0);
			this.btSprint.Location = new Point(173, 50);
			this.btSprint.MinimumSize = new Size(20, 22);
			this.btSprint.MinimumTextWidth = 20;
			this.btSprint.Name = "btSprint";
			this.btSprint.Size = new Size(77, 22);
			this.btSprint.TabIndex = 909;
			this.btSprint.Text = "None";
			this.btSprint.TextOffset = new Point(0, 0);
			this.btSprint.TextPadding = new Padding(0);
			this.btSprint.YOffSet = 0;
			this.btSprint.MouseDown += this.BindButtons_MouseDown;
			this.beautyPanel8.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel8.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel8.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel8.BorderSizeBottom = 1f;
			this.beautyPanel8.BorderSizeLeft = 1f;
			this.beautyPanel8.BorderSizeRight = 1f;
			this.beautyPanel8.BorderSizeTop = 1f;
			this.beautyPanel8.Controls.Add(this.beautyLabel9);
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
			this.beautyLabel9.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel9.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel9.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel9.Location = new Point(10, 11);
			this.beautyLabel9.Name = "beautyLabel9";
			this.beautyLabel9.Size = new Size(48, 18);
			this.beautyLabel9.TabIndex = 905;
			this.beautyLabel9.Text = "Sprint";
			this.beautyLabel9.TextPadding = new Padding(0);
			this.cbSprint.AutoRoundCorners = true;
			this.cbSprint.BackColor = Color.FromArgb(12, 14, 14);
			this.cbSprint.Checked = false;
			this.cbSprint.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbSprint.CheckedState.BorderRadius = 4;
			this.cbSprint.CheckedState.BorderThickness = 1;
			this.cbSprint.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbSprint.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbSprint.CheckedState.InnerBorderRadius = 4;
			this.cbSprint.CheckedState.InnerBorderThickness = 0;
			this.cbSprint.CheckedState.InnerColor = Color.Firebrick;
			this.cbSprint.CheckedState.InnerOffset = 2;
			this.cbSprint.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbSprint.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbSprint.LinkedLabel = this.beautyLabel10;
			this.cbSprint.Location = new Point(10, 50);
			this.cbSprint.Name = "cbSprint";
			this.cbSprint.Size = new Size(44, 22);
			this.cbSprint.TabIndex = 894;
			this.cbSprint.Text = "beautyToggleSwitch2";
			this.cbSprint.ThumbSize = 12;
			this.cbSprint.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbSprint.UncheckedState.BorderRadius = 4;
			this.cbSprint.UncheckedState.BorderThickness = 1;
			this.cbSprint.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbSprint.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbSprint.UncheckedState.InnerBorderRadius = 4;
			this.cbSprint.UncheckedState.InnerBorderThickness = 0;
			this.cbSprint.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbSprint.UncheckedState.InnerOffset = 30;
			this.cbSprint.CheckedChanged += this.cbSprint_CheckedChanged;
			this.beautyLabel10.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel10.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel10.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel10.Location = new Point(60, 53);
			this.beautyLabel10.Name = "beautyLabel10";
			this.beautyLabel10.Size = new Size(53, 18);
			this.beautyLabel10.TabIndex = 842;
			this.beautyLabel10.Text = "Enable";
			this.beautyLabel10.TextPadding = new Padding(0);
			this.beautyPanel5.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel5.BorderSizeBottom = 1f;
			this.beautyPanel5.BorderSizeLeft = 1f;
			this.beautyPanel5.BorderSizeRight = 1f;
			this.beautyPanel5.BorderSizeTop = 1f;
			this.beautyPanel5.Controls.Add(this.cbAutoSwap);
			this.beautyPanel5.Controls.Add(this.beautyLabel1);
			this.beautyPanel5.Controls.Add(this.cbLookingDown);
			this.beautyPanel5.Controls.Add(this.beautyLabel19);
			this.beautyPanel5.Controls.Add(this.cbHoldingBlocks);
			this.beautyPanel5.Controls.Add(this.beautyLabel18);
			this.beautyPanel5.Controls.Add(this.cbSneakKeyPressed);
			this.beautyPanel5.Controls.Add(this.beautyLabel17);
			this.beautyPanel5.Controls.Add(this.cbRandomize);
			this.beautyPanel5.Controls.Add(this.beautyLabel16);
			this.beautyPanel5.Controls.Add(this.unsneakDelay);
			this.beautyPanel5.Controls.Add(this.beautyLabel15);
			this.beautyPanel5.Controls.Add(this.delaylb);
			this.beautyPanel5.Controls.Add(this.edgeOffset);
			this.beautyPanel5.Controls.Add(this.beautyLabel13);
			this.beautyPanel5.Controls.Add(this.edglb);
			this.beautyPanel5.Controls.Add(this.bindBridge);
			this.beautyPanel5.Controls.Add(this.cbSneakOnJump);
			this.beautyPanel5.Controls.Add(this.beautyLabel4);
			this.beautyPanel5.Controls.Add(this.beautyPanel6);
			this.beautyPanel5.Controls.Add(this.cbBridge);
			this.beautyPanel5.Controls.Add(this.beautyLabel8);
			this.beautyPanel5.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.FullHeight = 350;
			this.beautyPanel5.Location = new Point(11, 21);
			this.beautyPanel5.Name = "beautyPanel5";
			this.beautyPanel5.RadiusBottomLeft = 6f;
			this.beautyPanel5.RadiusBottomRight = 6f;
			this.beautyPanel5.RadiusTopLeft = 6f;
			this.beautyPanel5.RadiusTopRight = 6f;
			this.beautyPanel5.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel5.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel5.ScrollbarWidth = 4;
			this.beautyPanel5.Size = new Size(260, 399);
			this.beautyPanel5.TabIndex = 912;
			this.cbLookingDown.AnimationSpeed = 0.6f;
			this.cbLookingDown.BackColor = Color.FromArgb(12, 14, 14);
			this.cbLookingDown.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbLookingDown.BorderRadius = 2f;
			this.cbLookingDown.BorderSize = 1f;
			this.cbLookingDown.CheckedBorderColor = Color.Firebrick;
			this.cbLookingDown.CheckedFillColor = Color.Firebrick;
			this.cbLookingDown.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbLookingDown.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbLookingDown.CheckMarkScale = 0.6f;
			this.cbLookingDown.FillColor = Color.FromArgb(16, 18, 18);
			this.cbLookingDown.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbLookingDown.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbLookingDown.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbLookingDown.Location = new Point(228, 314);
			this.cbLookingDown.Name = "cbLookingDown";
			this.cbLookingDown.Size = new Size(22, 22);
			this.cbLookingDown.TabIndex = 922;
			this.cbLookingDown.TargetLabel = this.beautyLabel19;
			this.cbLookingDown.Text = "beautyCheckBox4";
			this.cbLookingDown.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbLookingDown.CheckedChanged += this.cbLookingDown_CheckedChanged;
			this.beautyLabel19.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel19.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel19.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel19.Location = new Point(10, 318);
			this.beautyLabel19.Name = "beautyLabel19";
			this.beautyLabel19.Size = new Size(101, 18);
			this.beautyLabel19.TabIndex = 923;
			this.beautyLabel19.Text = "Looking down";
			this.beautyLabel19.TextPadding = new Padding(0);
			this.cbHoldingBlocks.AnimationSpeed = 0.6f;
			this.cbHoldingBlocks.AnimationStep = 0.9999998f;
			this.cbHoldingBlocks.BackColor = Color.FromArgb(12, 14, 14);
			this.cbHoldingBlocks.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbHoldingBlocks.BorderRadius = 2f;
			this.cbHoldingBlocks.BorderSize = 1f;
			this.cbHoldingBlocks.Checked = true;
			this.cbHoldingBlocks.CheckedBorderColor = Color.Firebrick;
			this.cbHoldingBlocks.CheckedFillColor = Color.Firebrick;
			this.cbHoldingBlocks.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbHoldingBlocks.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbHoldingBlocks.CheckMarkScale = 0.6f;
			this.cbHoldingBlocks.FillColor = Color.FromArgb(16, 18, 18);
			this.cbHoldingBlocks.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbHoldingBlocks.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbHoldingBlocks.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbHoldingBlocks.Location = new Point(228, 286);
			this.cbHoldingBlocks.Name = "cbHoldingBlocks";
			this.cbHoldingBlocks.Size = new Size(22, 22);
			this.cbHoldingBlocks.TabIndex = 920;
			this.cbHoldingBlocks.TargetLabel = this.beautyLabel18;
			this.cbHoldingBlocks.Text = "beautyCheckBox3";
			this.cbHoldingBlocks.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbHoldingBlocks.CheckedChanged += this.cbHoldingBlocks_CheckedChanged;
			this.beautyLabel18.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel18.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel18.ForeColor = Color.FromArgb(119, 119, 129);
			this.beautyLabel18.Location = new Point(10, 289);
			this.beautyLabel18.Name = "beautyLabel18";
			this.beautyLabel18.Size = new Size(105, 18);
			this.beautyLabel18.TabIndex = 921;
			this.beautyLabel18.Text = "Holding blocks";
			this.beautyLabel18.TextPadding = new Padding(0);
			this.cbSneakKeyPressed.AnimationSpeed = 0.6f;
			this.cbSneakKeyPressed.BackColor = Color.FromArgb(12, 14, 14);
			this.cbSneakKeyPressed.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbSneakKeyPressed.BorderRadius = 2f;
			this.cbSneakKeyPressed.BorderSize = 1f;
			this.cbSneakKeyPressed.CheckedBorderColor = Color.Firebrick;
			this.cbSneakKeyPressed.CheckedFillColor = Color.Firebrick;
			this.cbSneakKeyPressed.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbSneakKeyPressed.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbSneakKeyPressed.CheckMarkScale = 0.6f;
			this.cbSneakKeyPressed.FillColor = Color.FromArgb(16, 18, 18);
			this.cbSneakKeyPressed.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbSneakKeyPressed.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbSneakKeyPressed.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbSneakKeyPressed.Location = new Point(228, 258);
			this.cbSneakKeyPressed.Name = "cbSneakKeyPressed";
			this.cbSneakKeyPressed.Size = new Size(22, 22);
			this.cbSneakKeyPressed.TabIndex = 918;
			this.cbSneakKeyPressed.TargetLabel = this.beautyLabel17;
			this.cbSneakKeyPressed.Text = "beautyCheckBox3";
			this.cbSneakKeyPressed.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbSneakKeyPressed.CheckedChanged += this.cbSneakKeyPressed_CheckedChanged;
			this.beautyLabel17.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel17.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel17.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel17.Location = new Point(10, 258);
			this.beautyLabel17.Name = "beautyLabel17";
			this.beautyLabel17.Size = new Size(134, 18);
			this.beautyLabel17.TabIndex = 919;
			this.beautyLabel17.Text = "Sneak key pressed";
			this.beautyLabel17.TextPadding = new Padding(0);
			this.cbRandomize.AnimationSpeed = 0.6f;
			this.cbRandomize.BackColor = Color.FromArgb(12, 14, 14);
			this.cbRandomize.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbRandomize.BorderRadius = 2f;
			this.cbRandomize.BorderSize = 1f;
			this.cbRandomize.CheckedBorderColor = Color.Firebrick;
			this.cbRandomize.CheckedFillColor = Color.Firebrick;
			this.cbRandomize.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbRandomize.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbRandomize.CheckMarkScale = 0.6f;
			this.cbRandomize.FillColor = Color.FromArgb(16, 18, 18);
			this.cbRandomize.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbRandomize.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbRandomize.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbRandomize.Location = new Point(228, 203);
			this.cbRandomize.Name = "cbRandomize";
			this.cbRandomize.Size = new Size(22, 22);
			this.cbRandomize.TabIndex = 916;
			this.cbRandomize.TargetLabel = this.beautyLabel16;
			this.cbRandomize.Text = "beautyCheckBox3";
			this.cbRandomize.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbRandomize.CheckedChanged += this.cbRandomize_CheckedChanged;
			this.beautyLabel16.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel16.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel16.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel16.Location = new Point(10, 203);
			this.beautyLabel16.Name = "beautyLabel16";
			this.beautyLabel16.Size = new Size(82, 18);
			this.beautyLabel16.TabIndex = 917;
			this.beautyLabel16.Text = "Randomize";
			this.beautyLabel16.TextPadding = new Padding(0);
			this.unsneakDelay.AnimationTrigger = 0;
			this.unsneakDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.unsneakDelay.BarColor = Color.Firebrick;
			this.unsneakDelay.BorderColor = Color.FromArgb(20, 22, 22);
			this.unsneakDelay.BorderRadius = 2f;
			this.unsneakDelay.BorderSize = 1;
			this.unsneakDelay.FillColor = Color.FromArgb(16, 18, 18);
			this.unsneakDelay.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.unsneakDelay.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.unsneakDelay.HoverBarColor = Color.Firebrick;
			this.unsneakDelay.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.unsneakDelay.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.unsneakDelay.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.unsneakDelay.Location = new Point(10, 167);
			this.unsneakDelay.Maximum = 300;
			this.unsneakDelay.Minimum = 0;
			this.unsneakDelay.Name = "unsneakDelay";
			this.unsneakDelay.Offset = 1f;
			this.unsneakDelay.ShowText = false;
			this.unsneakDelay.ShowValue = true;
			this.unsneakDelay.Size = new Size(240, 20);
			this.unsneakDelay.TabIndex = 913;
			this.unsneakDelay.TargetLabel = this.delaylb;
			this.unsneakDelay.Text = "beautyFlatSlider2";
			this.unsneakDelay.Value = 90;
			this.unsneakDelay.WriteInLabel = true;
			this.unsneakDelay.Scroll += this.unsneakDelay_Scroll;
			this.delaylb.AutoResize = false;
			this.delaylb.BackColor = Color.FromArgb(12, 14, 14);
			this.delaylb.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.delaylb.ForeColor = Color.FromArgb(70, 70, 80);
			this.delaylb.Location = new Point(178, 143);
			this.delaylb.Name = "delaylb";
			this.delaylb.Size = new Size(72, 18);
			this.delaylb.TabIndex = 915;
			this.delaylb.Text = "90ms";
			this.delaylb.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.delaylb.TextPadding = new Padding(0);
			this.beautyLabel15.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel15.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel15.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel15.Location = new Point(10, 143);
			this.beautyLabel15.Name = "beautyLabel15";
			this.beautyLabel15.Size = new Size(105, 18);
			this.beautyLabel15.TabIndex = 914;
			this.beautyLabel15.Text = "Unsneak delay";
			this.beautyLabel15.TextPadding = new Padding(0);
			this.edgeOffset.AnimationTrigger = 0;
			this.edgeOffset.BackColor = Color.FromArgb(12, 14, 14);
			this.edgeOffset.BarColor = Color.Firebrick;
			this.edgeOffset.BorderColor = Color.FromArgb(20, 22, 22);
			this.edgeOffset.BorderRadius = 2f;
			this.edgeOffset.BorderSize = 1;
			this.edgeOffset.FillColor = Color.FromArgb(16, 18, 18);
			this.edgeOffset.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.edgeOffset.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.edgeOffset.HoverBarColor = Color.Firebrick;
			this.edgeOffset.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.edgeOffset.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.edgeOffset.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.edgeOffset.Location = new Point(10, 114);
			this.edgeOffset.Maximum = 100;
			this.edgeOffset.Minimum = 0;
			this.edgeOffset.Name = "edgeOffset";
			this.edgeOffset.Offset = 1f;
			this.edgeOffset.ShowText = false;
			this.edgeOffset.ShowValue = true;
			this.edgeOffset.Size = new Size(240, 20);
			this.edgeOffset.TabIndex = 910;
			this.edgeOffset.TargetLabel = this.edglb;
			this.edgeOffset.Text = "beautyFlatSlider1";
			this.edgeOffset.Value = 60;
			this.edgeOffset.WriteInLabel = true;
			this.edgeOffset.Scroll += this.edgeOffset_Scroll;
			this.edglb.AutoResize = false;
			this.edglb.BackColor = Color.FromArgb(12, 14, 14);
			this.edglb.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.edglb.ForeColor = Color.FromArgb(70, 70, 80);
			this.edglb.Location = new Point(178, 90);
			this.edglb.Name = "edglb";
			this.edglb.Size = new Size(72, 18);
			this.edglb.TabIndex = 912;
			this.edglb.Text = "0.20";
			this.edglb.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.edglb.TextPadding = new Padding(0);
			this.beautyLabel13.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel13.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel13.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel13.Location = new Point(10, 90);
			this.beautyLabel13.Name = "beautyLabel13";
			this.beautyLabel13.Size = new Size(87, 18);
			this.beautyLabel13.TabIndex = 911;
			this.beautyLabel13.Text = "Edge offset:";
			this.beautyLabel13.TextPadding = new Padding(0);
			this.bindBridge.AnimationSpeed = 0.6f;
			this.bindBridge.BorderColor = Color.FromArgb(16, 18, 18);
			this.bindBridge.BorderRadius = 4f;
			this.bindBridge.BorderSize = 1f;
			this.bindBridge.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.bindBridge.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.bindBridge.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.bindBridge.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.bindBridge.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.bindBridge.FillColor = Color.FromArgb(16, 18, 18);
			this.bindBridge.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.bindBridge.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.bindBridge.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.bindBridge.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.bindBridge.ImageOffset = new Point(0, 0);
			this.bindBridge.Location = new Point(173, 50);
			this.bindBridge.MinimumSize = new Size(20, 22);
			this.bindBridge.MinimumTextWidth = 20;
			this.bindBridge.Name = "bindBridge";
			this.bindBridge.Size = new Size(77, 22);
			this.bindBridge.TabIndex = 909;
			this.bindBridge.Text = "None";
			this.bindBridge.TextOffset = new Point(0, 0);
			this.bindBridge.TextPadding = new Padding(0);
			this.bindBridge.YOffSet = 0;
			this.bindBridge.MouseDown += this.BindButtons_MouseDown;
			this.cbSneakOnJump.AnimationSpeed = 0.6f;
			this.cbSneakOnJump.BackColor = Color.FromArgb(12, 14, 14);
			this.cbSneakOnJump.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbSneakOnJump.BorderRadius = 2f;
			this.cbSneakOnJump.BorderSize = 1f;
			this.cbSneakOnJump.CheckedBorderColor = Color.Firebrick;
			this.cbSneakOnJump.CheckedFillColor = Color.Firebrick;
			this.cbSneakOnJump.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbSneakOnJump.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbSneakOnJump.CheckMarkScale = 0.6f;
			this.cbSneakOnJump.FillColor = Color.FromArgb(16, 18, 18);
			this.cbSneakOnJump.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbSneakOnJump.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbSneakOnJump.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbSneakOnJump.Location = new Point(228, 230);
			this.cbSneakOnJump.Name = "cbSneakOnJump";
			this.cbSneakOnJump.Size = new Size(22, 22);
			this.cbSneakOnJump.TabIndex = 907;
			this.cbSneakOnJump.TargetLabel = this.beautyLabel4;
			this.cbSneakOnJump.Text = "beautyCheckBox3";
			this.cbSneakOnJump.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbSneakOnJump.CheckedChanged += this.cbRightBA_CheckedChanged;
			this.beautyLabel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel4.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel4.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel4.Location = new Point(10, 230);
			this.beautyLabel4.Name = "beautyLabel4";
			this.beautyLabel4.Size = new Size(106, 18);
			this.beautyLabel4.TabIndex = 908;
			this.beautyLabel4.Text = "Sneak on jump";
			this.beautyLabel4.TextPadding = new Padding(0);
			this.beautyPanel6.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel6.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel6.BorderSizeBottom = 1f;
			this.beautyPanel6.BorderSizeLeft = 1f;
			this.beautyPanel6.BorderSizeRight = 1f;
			this.beautyPanel6.BorderSizeTop = 1f;
			this.beautyPanel6.Controls.Add(this.beautyLabel7);
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
			this.beautyLabel7.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel7.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel7.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel7.Location = new Point(10, 11);
			this.beautyLabel7.Name = "beautyLabel7";
			this.beautyLabel7.Size = new Size(99, 18);
			this.beautyLabel7.TabIndex = 905;
			this.beautyLabel7.Text = "Bridge Assist";
			this.beautyLabel7.TextPadding = new Padding(0);
			this.cbBridge.AutoRoundCorners = true;
			this.cbBridge.BackColor = Color.FromArgb(12, 14, 14);
			this.cbBridge.Checked = false;
			this.cbBridge.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbBridge.CheckedState.BorderRadius = 4;
			this.cbBridge.CheckedState.BorderThickness = 1;
			this.cbBridge.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbBridge.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbBridge.CheckedState.InnerBorderRadius = 4;
			this.cbBridge.CheckedState.InnerBorderThickness = 0;
			this.cbBridge.CheckedState.InnerColor = Color.Firebrick;
			this.cbBridge.CheckedState.InnerOffset = 2;
			this.cbBridge.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbBridge.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbBridge.LinkedLabel = this.beautyLabel8;
			this.cbBridge.Location = new Point(10, 50);
			this.cbBridge.Name = "cbBridge";
			this.cbBridge.Size = new Size(44, 22);
			this.cbBridge.TabIndex = 894;
			this.cbBridge.Text = "beautyToggleSwitch1";
			this.cbBridge.ThumbSize = 12;
			this.cbBridge.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbBridge.UncheckedState.BorderRadius = 4;
			this.cbBridge.UncheckedState.BorderThickness = 1;
			this.cbBridge.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbBridge.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbBridge.UncheckedState.InnerBorderRadius = 4;
			this.cbBridge.UncheckedState.InnerBorderThickness = 0;
			this.cbBridge.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbBridge.UncheckedState.InnerOffset = 30;
			this.cbBridge.CheckedChanged += this.cbBridge_CheckedChanged;
			this.beautyLabel8.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel8.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel8.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel8.Location = new Point(60, 53);
			this.beautyLabel8.Name = "beautyLabel8";
			this.beautyLabel8.Size = new Size(53, 18);
			this.beautyLabel8.TabIndex = 842;
			this.beautyLabel8.Text = "Enable";
			this.beautyLabel8.TextPadding = new Padding(0);
			this.beautyPanel1.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel1.BorderSizeBottom = 1f;
			this.beautyPanel1.BorderSizeLeft = 1f;
			this.beautyPanel1.BorderSizeRight = 1f;
			this.beautyPanel1.BorderSizeTop = 1f;
			this.beautyPanel1.Controls.Add(this.btJump);
			this.beautyPanel1.Controls.Add(this.beautyPanel2);
			this.beautyPanel1.Controls.Add(this.cbJumpDelay);
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
			this.beautyPanel1.TabIndex = 910;
			this.btJump.AnimationSpeed = 0.6f;
			this.btJump.BorderColor = Color.FromArgb(16, 18, 18);
			this.btJump.BorderRadius = 4f;
			this.btJump.BorderSize = 1f;
			this.btJump.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.btJump.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.btJump.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.btJump.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.btJump.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.btJump.FillColor = Color.FromArgb(16, 18, 18);
			this.btJump.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.btJump.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.btJump.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.btJump.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.btJump.ImageOffset = new Point(0, 0);
			this.btJump.Location = new Point(173, 50);
			this.btJump.MinimumSize = new Size(20, 22);
			this.btJump.MinimumTextWidth = 20;
			this.btJump.Name = "btJump";
			this.btJump.Size = new Size(77, 22);
			this.btJump.TabIndex = 909;
			this.btJump.Text = "None";
			this.btJump.TextOffset = new Point(0, 0);
			this.btJump.TextPadding = new Padding(0);
			this.btJump.YOffSet = 0;
			this.btJump.MouseDown += this.BindButtons_MouseDown;
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
			this.beautyLabel2.Size = new Size(109, 18);
			this.beautyLabel2.TabIndex = 905;
			this.beautyLabel2.Text = "No Jump Delay";
			this.beautyLabel2.TextPadding = new Padding(0);
			this.cbJumpDelay.AutoRoundCorners = true;
			this.cbJumpDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.cbJumpDelay.Checked = false;
			this.cbJumpDelay.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbJumpDelay.CheckedState.BorderRadius = 4;
			this.cbJumpDelay.CheckedState.BorderThickness = 1;
			this.cbJumpDelay.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbJumpDelay.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbJumpDelay.CheckedState.InnerBorderRadius = 4;
			this.cbJumpDelay.CheckedState.InnerBorderThickness = 0;
			this.cbJumpDelay.CheckedState.InnerColor = Color.Firebrick;
			this.cbJumpDelay.CheckedState.InnerOffset = 2;
			this.cbJumpDelay.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbJumpDelay.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbJumpDelay.LinkedLabel = this.beautyLabel3;
			this.cbJumpDelay.Location = new Point(10, 50);
			this.cbJumpDelay.Name = "cbJumpDelay";
			this.cbJumpDelay.Size = new Size(44, 22);
			this.cbJumpDelay.TabIndex = 894;
			this.cbJumpDelay.Text = "beautyToggleSwitch2";
			this.cbJumpDelay.ThumbSize = 12;
			this.cbJumpDelay.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbJumpDelay.UncheckedState.BorderRadius = 4;
			this.cbJumpDelay.UncheckedState.BorderThickness = 1;
			this.cbJumpDelay.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbJumpDelay.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbJumpDelay.UncheckedState.InnerBorderRadius = 4;
			this.cbJumpDelay.UncheckedState.InnerBorderThickness = 0;
			this.cbJumpDelay.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbJumpDelay.UncheckedState.InnerOffset = 30;
			this.cbJumpDelay.CheckedChanged += this.cbJumpDelay_CheckedChanged;
			this.beautyLabel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel3.Location = new Point(60, 53);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(53, 18);
			this.beautyLabel3.TabIndex = 842;
			this.beautyLabel3.Text = "Enable";
			this.beautyLabel3.TextPadding = new Padding(0);
			this.cbAutoSwap.AnimationSpeed = 0.6f;
			this.cbAutoSwap.BackColor = Color.FromArgb(12, 14, 14);
			this.cbAutoSwap.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbAutoSwap.BorderRadius = 2f;
			this.cbAutoSwap.BorderSize = 1f;
			this.cbAutoSwap.CheckedBorderColor = Color.Firebrick;
			this.cbAutoSwap.CheckedFillColor = Color.Firebrick;
			this.cbAutoSwap.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbAutoSwap.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbAutoSwap.CheckMarkScale = 0.6f;
			this.cbAutoSwap.FillColor = Color.FromArgb(16, 18, 18);
			this.cbAutoSwap.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbAutoSwap.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbAutoSwap.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbAutoSwap.Location = new Point(228, 342);
			this.cbAutoSwap.Name = "cbAutoSwap";
			this.cbAutoSwap.Size = new Size(22, 22);
			this.cbAutoSwap.TabIndex = 924;
			this.cbAutoSwap.TargetLabel = this.beautyLabel1;
			this.cbAutoSwap.Text = "beautyCheckBox4";
			this.cbAutoSwap.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbAutoSwap.CheckedChanged += this.cbAutoSwap_CheckedChanged;
			this.beautyLabel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel1.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel1.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel1.Location = new Point(10, 346);
			this.beautyLabel1.Name = "beautyLabel1";
			this.beautyLabel1.Size = new Size(80, 18);
			this.beautyLabel1.TabIndex = 925;
			this.beautyLabel1.Text = "Auto Swap";
			this.beautyLabel1.TextPadding = new Padding(0);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(12, 14, 14);
			base.Controls.Add(this.DefaultPanel);
			base.Name = "Movement";
			base.Size = new Size(570, 410);
			this.DefaultPanel.ResumeLayout(false);
			this.beautyPanel7.ResumeLayout(false);
			this.beautyPanel7.PerformLayout();
			this.beautyPanel8.ResumeLayout(false);
			this.beautyPanel5.ResumeLayout(false);
			this.beautyPanel5.PerformLayout();
			this.beautyPanel6.ResumeLayout(false);
			this.beautyPanel1.ResumeLayout(false);
			this.beautyPanel1.PerformLayout();
			this.beautyPanel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040002BB RID: 699
		public Keys bindJump;

		// Token: 0x040002BC RID: 700
		public Keys bindBridgeKey;

		// Token: 0x040002BD RID: 701
		public Keys bindSprintKey;

		// Token: 0x040002BE RID: 702
		public static int JumpBindInt;

		// Token: 0x040002BF RID: 703
		public static int BridgeBindInt;

		// Token: 0x040002C0 RID: 704
		public static int SprintBindInt;

		// Token: 0x040002C1 RID: 705
		private Dictionary<BeautyAutoButton, Keys> currentBinds = new Dictionary<BeautyAutoButton, Keys>();

		// Token: 0x040002C2 RID: 706
		private Dictionary<BeautyAutoButton, Action> currentActions = new Dictionary<BeautyAutoButton, Action>();

		// Token: 0x040002C3 RID: 707
		private IContainer components;

		// Token: 0x040002C4 RID: 708
		private BeautyPanel DefaultPanel;

		// Token: 0x040002C5 RID: 709
		private BeautyPanel beautyPanel1;

		// Token: 0x040002C6 RID: 710
		private BeautyPanel beautyPanel2;

		// Token: 0x040002C7 RID: 711
		private BeautyLabel beautyLabel2;

		// Token: 0x040002C8 RID: 712
		public BeautyToggleSwitch cbJumpDelay;

		// Token: 0x040002C9 RID: 713
		private BeautyLabel beautyLabel3;

		// Token: 0x040002CA RID: 714
		private BeautyPanel beautyPanel5;

		// Token: 0x040002CB RID: 715
		public BeautyCheckBox cbSneakOnJump;

		// Token: 0x040002CC RID: 716
		private BeautyLabel beautyLabel4;

		// Token: 0x040002CD RID: 717
		private BeautyPanel beautyPanel6;

		// Token: 0x040002CE RID: 718
		private BeautyLabel beautyLabel7;

		// Token: 0x040002CF RID: 719
		public BeautyToggleSwitch cbBridge;

		// Token: 0x040002D0 RID: 720
		private BeautyLabel beautyLabel8;

		// Token: 0x040002D1 RID: 721
		private BeautyPanel beautyPanel7;

		// Token: 0x040002D2 RID: 722
		private BeautyPanel beautyPanel8;

		// Token: 0x040002D3 RID: 723
		private BeautyLabel beautyLabel9;

		// Token: 0x040002D4 RID: 724
		public BeautyToggleSwitch cbSprint;

		// Token: 0x040002D5 RID: 725
		private BeautyLabel beautyLabel10;

		// Token: 0x040002D6 RID: 726
		public BeautyCheckBox cbHoldingBlocks;

		// Token: 0x040002D7 RID: 727
		private BeautyLabel beautyLabel18;

		// Token: 0x040002D8 RID: 728
		public BeautyCheckBox cbSneakKeyPressed;

		// Token: 0x040002D9 RID: 729
		private BeautyLabel beautyLabel17;

		// Token: 0x040002DA RID: 730
		public BeautyCheckBox cbRandomize;

		// Token: 0x040002DB RID: 731
		private BeautyLabel beautyLabel16;

		// Token: 0x040002DC RID: 732
		public BeautyFlatSlider unsneakDelay;

		// Token: 0x040002DD RID: 733
		private BeautyLabel beautyLabel15;

		// Token: 0x040002DE RID: 734
		public BeautyFlatSlider edgeOffset;

		// Token: 0x040002DF RID: 735
		private BeautyLabel beautyLabel13;

		// Token: 0x040002E0 RID: 736
		public BeautyCheckBox cbLookingDown;

		// Token: 0x040002E1 RID: 737
		private BeautyLabel beautyLabel19;

		// Token: 0x040002E2 RID: 738
		public BeautyAutoButton bindBridge;

		// Token: 0x040002E3 RID: 739
		public BeautyAutoButton btJump;

		// Token: 0x040002E4 RID: 740
		public BeautyAutoButton btSprint;

		// Token: 0x040002E5 RID: 741
		public BeautyLabel delaylb;

		// Token: 0x040002E6 RID: 742
		public BeautyLabel edglb;

		// Token: 0x040002E7 RID: 743
		public BeautyCheckBox cbAutoSwap;

		// Token: 0x040002E8 RID: 744
		private BeautyLabel beautyLabel1;
	}
}
