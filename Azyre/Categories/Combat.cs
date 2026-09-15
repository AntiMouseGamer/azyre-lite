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
	// Token: 0x0200004A RID: 74
	public class Combat : UserControl
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000288 RID: 648 RVA: 0x000112B1 File Offset: 0x0000F4B1
		// (set) Token: 0x06000289 RID: 649 RVA: 0x000112B8 File Offset: 0x0000F4B8
		public static Combat Static { get; set; }

		// Token: 0x0600028A RID: 650 RVA: 0x000112C0 File Offset: 0x0000F4C0
		public Combat()
		{
			this.InitializeComponent();
			Combat.Static = this;
			this.ModeTypeSprint.BringToFront();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0001134C File Offset: 0x0000F54C
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

		// Token: 0x0600028C RID: 652 RVA: 0x0001138C File Offset: 0x0000F58C
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

		// Token: 0x0600028D RID: 653 RVA: 0x000114DC File Offset: 0x0000F6DC
		private void CPS_Slider_Scroll(object sender, ScrollEventArgs e)
		{
			double num = (double)this.CPS_Slider.Value / 10.0;
			this.labelCPS.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbInventory_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00011528 File Offset: 0x0000F728
		private void ClickerEnable_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00011521 File Offset: 0x0000F721
		private void Inventory_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbBreak_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00011521 File Offset: 0x0000F721
		private void Weapon_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00011521 File Offset: 0x0000F721
		private void AimEnable_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbMouseMove_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00011558 File Offset: 0x0000F758
		private void slideDistance_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.slideDistance.Value / 100f;
			this.aimlabeldist.Text = num.ToString("0.00", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001159C File Offset: 0x0000F79C
		private void AimAssistFovSlider_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.AimAssistFovSlider.Value;
			this.lbfov.Text = value.ToString();
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00011521 File Offset: 0x0000F721
		private void AimAssistOnlyWeapon_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00011521 File Offset: 0x0000F721
		private void AimAssistClickingOnly_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00011521 File Offset: 0x0000F721
		private void AimAssistThroughWall_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbLockTarget_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000115CC File Offset: 0x0000F7CC
		private void slidehorizontalaim_Scroll(object sender, ScrollEventArgs e)
		{
			float value = (float)this.slidehorizontalaim.Value / 10f;
			this.labelHorizontal.Text = value.ToString("0.0", CultureInfo.InvariantCulture);
			Console.WriteLine(value);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00011614 File Offset: 0x0000F814
		private void slideverticalaim_Scroll(object sender, ScrollEventArgs e)
		{
			float value = (float)this.slideverticalaim.Value / 10f;
			this.labelVertical.Text = value.ToString("0.0", CultureInfo.InvariantCulture);
			Console.WriteLine(value);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbHitboxClosest_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbVertical_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00011521 File Offset: 0x0000F721
		private void ReachEnable_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0001165C File Offset: 0x0000F85C
		private void DistanceSlider_Scroll(object sender, ScrollEventArgs e)
		{
			float value = (float)this.DistanceSlider.Value / 100f;
			this.labelReach.Text = value.ToString("0.00", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
			Console.WriteLine(value);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000116A4 File Offset: 0x0000F8A4
		private void HitBoxSl_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.HitBoxSl.Value / 100f;
			this.labelHitbox.Text = num.ToString("0.00", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbReachWeapon_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbWallCheck_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00011521 File Offset: 0x0000F721
		private void VelocityEnable_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00011521 File Offset: 0x0000F721
		private void VelocityMovingOnly_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbTargeting_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbAttacking_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000116E8 File Offset: 0x0000F8E8
		private void ticksvl_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.ticksvl.Value;
			this.lbticks.Text = value.ToString() + " ms";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00011724 File Offset: 0x0000F924
		private void VelocityHrz_Scroll(object sender, ScrollEventArgs e)
		{
			double num = (double)this.VelocityHrz.Value / 10.0;
			this.labelVelH.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001176C File Offset: 0x0000F96C
		private void VelocityVrt_Scroll(object sender, ScrollEventArgs e)
		{
			double num = (double)this.VelocityVrt.Value / 10.0;
			this.labelVelV.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000117B4 File Offset: 0x0000F9B4
		private void ChanceSlider_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.ChanceSlider.Value;
			this.lbchancevl.Text = value.ToString() + "%";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbSuperKnockback_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000117F0 File Offset: 0x0000F9F0
		private void sliderSuperKnockbackChance_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numSprintResetChance.Value;
			this.lbSprintResetChance.Text = value.ToString() + "%";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0001182C File Offset: 0x0000FA2C
		private void numSuperKnockbackDelay_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numSprintResetMinRePress.Value;
			this.lbSprintResetDelay.Text = value.ToString() + " ms";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00011868 File Offset: 0x0000FA68
		private void numSuperKnockbackMaxHurt_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numSprintResetMaxRePress.Value;
			this.lbSprintResetStop.Text = value.ToString() + " ms";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00011521 File Offset: 0x0000F721
		private void ModeSprint_IndexChanged(object sender, int e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00011521 File Offset: 0x0000F721
		private void ModeSprint_IndexChanged_1(object sender, int e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbJumpReset_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000118A4 File Offset: 0x0000FAA4
		private void numJumpResetChance_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numJumpResetChance.Value;
			this.lbJumpResetChance.Text = value.ToString() + "%";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000118E0 File Offset: 0x0000FAE0
		private void numJumpResetDelay_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numJumpResetDelay.Value;
			this.lbJumpResetDelay.Text = value.ToString() + " ms";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0001191C File Offset: 0x0000FB1C
		private void numJumpResetDuration_Scroll(object sender, ScrollEventArgs e)
		{
			int value = this.numJumpResetDuration.Value;
			this.lbJumpResetDuration.Text = value.ToString() + " ms";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbAimBreakBlocks_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00011956 File Offset: 0x0000FB56
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00011978 File Offset: 0x0000FB78
		private void InitializeComponent()
		{
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel11 = new BeautyPanel();
			this.numJumpResetDuration = new BeautyFlatSlider();
			this.lbJumpResetDuration = new BeautyLabel();
			this.beautyLabel45 = new BeautyLabel();
			this.numJumpResetDelay = new BeautyFlatSlider();
			this.lbJumpResetDelay = new BeautyLabel();
			this.beautyLabel51 = new BeautyLabel();
			this.JumpResetButton = new BeautyAutoButton();
			this.numJumpResetChance = new BeautyFlatSlider();
			this.lbJumpResetChance = new BeautyLabel();
			this.beautyLabel53 = new BeautyLabel();
			this.beautyPanel12 = new BeautyPanel();
			this.beautyLabel54 = new BeautyLabel();
			this.cbJumpReset = new BeautyToggleSwitch();
			this.beautyLabel55 = new BeautyLabel();
			this.beautyPanel1 = new BeautyPanel();
			this.beautyLabel41 = new BeautyLabel();
			this.beautyLabel36 = new BeautyLabel();
			this.ModeSprint = new BeautyComboBox();
			this.ModeTypeSprint = new BeautyComboBox();
			this.numSprintResetMaxRePress = new BeautyFlatSlider();
			this.lbSprintResetStop = new BeautyLabel();
			this.beautyLabel40 = new BeautyLabel();
			this.numSprintResetMinRePress = new BeautyFlatSlider();
			this.lbSprintResetDelay = new BeautyLabel();
			this.beautyLabel38 = new BeautyLabel();
			this.SprintResetButton = new BeautyAutoButton();
			this.numSprintResetChance = new BeautyFlatSlider();
			this.lbSprintResetChance = new BeautyLabel();
			this.beautyLabel43 = new BeautyLabel();
			this.beautyPanel5 = new BeautyPanel();
			this.beautyLabel47 = new BeautyLabel();
			this.cbSprintReset = new BeautyToggleSwitch();
			this.beautyLabel48 = new BeautyLabel();
			this.beautyPanel2 = new BeautyPanel();
			this.cbAimBreakBlocks = new BeautyCheckBox();
			this.beautyLabel37 = new BeautyLabel();
			this.aimlabeldist = new BeautyLabel();
			this.cbLockTarget = new BeautyCheckBox();
			this.beautyLabel35 = new BeautyLabel();
			this.cbMouseMove = new BeautyCheckBox();
			this.beautyLabel15 = new BeautyLabel();
			this.cbVertical = new BeautyCheckBox();
			this.beautyLabel4 = new BeautyLabel();
			this.slideverticalaim = new BeautyFlatSlider();
			this.labelVertical = new BeautyLabel();
			this.slidehorizontalaim = new BeautyFlatSlider();
			this.labelHorizontal = new BeautyLabel();
			this.beautyLabel18 = new BeautyLabel();
			this.beautyLabel25 = new BeautyLabel();
			this.cbHitboxClosest = new BeautyCheckBox();
			this.beautyLabel3 = new BeautyLabel();
			this.AimAssistOnlyWeapon = new BeautyCheckBox();
			this.beautyLabel8 = new BeautyLabel();
			this.AimBindButton = new BeautyAutoButton();
			this.slideDistance = new BeautyFlatSlider();
			this.AimAssistFovSlider = new BeautyFlatSlider();
			this.lbfov = new BeautyLabel();
			this.beautyLabel14 = new BeautyLabel();
			this.beautyLabel17 = new BeautyLabel();
			this.AimAssistClickingOnly = new BeautyCheckBox();
			this.beautyLabel10 = new BeautyLabel();
			this.beautyPanel6 = new BeautyPanel();
			this.beautyLabel12 = new BeautyLabel();
			this.AimEnable = new BeautyToggleSwitch();
			this.beautyLabel13 = new BeautyLabel();
			this.AimAssistThroughWall = new BeautyCheckBox();
			this.beautyLabel16 = new BeautyLabel();
			this.beautyPanel9 = new BeautyPanel();
			this.cbAttacking = new BeautyCheckBox();
			this.beautyLabel33 = new BeautyLabel();
			this.cbTargeting = new BeautyCheckBox();
			this.beautyLabel32 = new BeautyLabel();
			this.ticksvl = new BeautyFlatSlider();
			this.lbticks = new BeautyLabel();
			this.beautyLabel19 = new BeautyLabel();
			this.VelocityVrt = new BeautyFlatSlider();
			this.labelVelV = new BeautyLabel();
			this.beautyLabel9 = new BeautyLabel();
			this.VelocityBindButton = new BeautyAutoButton();
			this.VelocityHrz = new BeautyFlatSlider();
			this.labelVelH = new BeautyLabel();
			this.beautyLabel26 = new BeautyLabel();
			this.ChanceSlider = new BeautyFlatSlider();
			this.lbchancevl = new BeautyLabel();
			this.beautyLabel28 = new BeautyLabel();
			this.VelocityMovingOnly = new BeautyCheckBox();
			this.beautyLabel29 = new BeautyLabel();
			this.beautyPanel10 = new BeautyPanel();
			this.beautyLabel30 = new BeautyLabel();
			this.VelocityEnable = new BeautyToggleSwitch();
			this.beautyLabel31 = new BeautyLabel();
			this.beautyPanel7 = new BeautyPanel();
			this.cbWallCheck = new BeautyCheckBox();
			this.beautyLabel27 = new BeautyLabel();
			this.ReachBindButton = new BeautyAutoButton();
			this.DistanceSlider = new BeautyFlatSlider();
			this.labelReach = new BeautyLabel();
			this.beautyLabel22 = new BeautyLabel();
			this.HitBoxSl = new BeautyFlatSlider();
			this.labelHitbox = new BeautyLabel();
			this.beautyLabel23 = new BeautyLabel();
			this.beautyPanel8 = new BeautyPanel();
			this.beautyLabel20 = new BeautyLabel();
			this.ReachEnable = new BeautyToggleSwitch();
			this.beautyLabel21 = new BeautyLabel();
			this.cbReachWeapon = new BeautyCheckBox();
			this.beautyLabel24 = new BeautyLabel();
			this.beautyPanel3 = new BeautyPanel();
			this.cbInventory = new BeautyCheckBox();
			this.beautyLabel34 = new BeautyLabel();
			this.ClickerBindButton = new BeautyAutoButton();
			this.Randomize = new BeautyCheckBox();
			this.beautyLabel5 = new BeautyLabel();
			this.Weapon = new BeautyCheckBox();
			this.beautyLabel2 = new BeautyLabel();
			this.beautyPanel4 = new BeautyPanel();
			this.beautyLabel11 = new BeautyLabel();
			this.ClickerEnable = new BeautyToggleSwitch();
			this.beautyLabel1 = new BeautyLabel();
			this.CPS_Slider = new BeautyFlatSlider();
			this.labelCPS = new BeautyLabel();
			this.beautyLabel6 = new BeautyLabel();
			this.cbBreak = new BeautyCheckBox();
			this.beautyLabel7 = new BeautyLabel();
			this.DefaultPanel.SuspendLayout();
			this.beautyPanel11.SuspendLayout();
			this.beautyPanel12.SuspendLayout();
			this.beautyPanel1.SuspendLayout();
			this.beautyPanel5.SuspendLayout();
			this.beautyPanel2.SuspendLayout();
			this.beautyPanel6.SuspendLayout();
			this.beautyPanel9.SuspendLayout();
			this.beautyPanel10.SuspendLayout();
			this.beautyPanel7.SuspendLayout();
			this.beautyPanel8.SuspendLayout();
			this.beautyPanel3.SuspendLayout();
			this.beautyPanel4.SuspendLayout();
			base.SuspendLayout();
			this.DefaultPanel.AutoScroll = true;
			this.DefaultPanel.AutoScrollMinSize = new Size(0, 1500);
			this.DefaultPanel.BackColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.BorderColor = Color.FromArgb(20, 22, 22);
			this.DefaultPanel.BorderSizeBottom = 0f;
			this.DefaultPanel.BorderSizeLeft = 0f;
			this.DefaultPanel.BorderSizeRight = 0f;
			this.DefaultPanel.BorderSizeTop = 0f;
			this.DefaultPanel.Controls.Add(this.beautyPanel11);
			this.DefaultPanel.Controls.Add(this.beautyPanel1);
			this.DefaultPanel.Controls.Add(this.beautyPanel2);
			this.DefaultPanel.Controls.Add(this.beautyPanel9);
			this.DefaultPanel.Controls.Add(this.beautyPanel7);
			this.DefaultPanel.Controls.Add(this.beautyPanel3);
			this.DefaultPanel.Dock = DockStyle.Fill;
			this.DefaultPanel.FillColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.FullHeight = 1500;
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
			this.DefaultPanel.TabIndex = 845;
			this.beautyPanel11.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel11.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel11.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel11.BorderSizeBottom = 1f;
			this.beautyPanel11.BorderSizeLeft = 1f;
			this.beautyPanel11.BorderSizeRight = 1f;
			this.beautyPanel11.BorderSizeTop = 1f;
			this.beautyPanel11.Controls.Add(this.numJumpResetDuration);
			this.beautyPanel11.Controls.Add(this.beautyLabel45);
			this.beautyPanel11.Controls.Add(this.lbJumpResetDuration);
			this.beautyPanel11.Controls.Add(this.numJumpResetDelay);
			this.beautyPanel11.Controls.Add(this.beautyLabel51);
			this.beautyPanel11.Controls.Add(this.lbJumpResetDelay);
			this.beautyPanel11.Controls.Add(this.JumpResetButton);
			this.beautyPanel11.Controls.Add(this.numJumpResetChance);
			this.beautyPanel11.Controls.Add(this.beautyLabel53);
			this.beautyPanel11.Controls.Add(this.lbJumpResetChance);
			this.beautyPanel11.Controls.Add(this.beautyPanel12);
			this.beautyPanel11.Controls.Add(this.cbJumpReset);
			this.beautyPanel11.Controls.Add(this.beautyLabel55);
			this.beautyPanel11.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel11.FullHeight = 350;
			this.beautyPanel11.Location = new Point(281, 925);
			this.beautyPanel11.Name = "beautyPanel11";
			this.beautyPanel11.RadiusBottomLeft = 6f;
			this.beautyPanel11.RadiusBottomRight = 6f;
			this.beautyPanel11.RadiusTopLeft = 6f;
			this.beautyPanel11.RadiusTopRight = 6f;
			this.beautyPanel11.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel11.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel11.ScrollbarWidth = 4;
			this.beautyPanel11.Size = new Size(260, 242);
			this.beautyPanel11.TabIndex = 911;
			this.numJumpResetDuration.AnimationTrigger = 0;
			this.numJumpResetDuration.BackColor = Color.FromArgb(12, 14, 14);
			this.numJumpResetDuration.BarColor = Color.Firebrick;
			this.numJumpResetDuration.BorderColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetDuration.BorderRadius = 2f;
			this.numJumpResetDuration.BorderSize = 1;
			this.numJumpResetDuration.FillColor = Color.FromArgb(16, 18, 18);
			this.numJumpResetDuration.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numJumpResetDuration.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numJumpResetDuration.HoverBarColor = Color.Firebrick;
			this.numJumpResetDuration.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numJumpResetDuration.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetDuration.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numJumpResetDuration.Location = new Point(11, 205);
			this.numJumpResetDuration.Maximum = 300;
			this.numJumpResetDuration.Minimum = 30;
			this.numJumpResetDuration.Name = "numJumpResetDuration";
			this.numJumpResetDuration.Offset = 1f;
			this.numJumpResetDuration.ShowText = false;
			this.numJumpResetDuration.ShowValue = false;
			this.numJumpResetDuration.Size = new Size(240, 20);
			this.numJumpResetDuration.TabIndex = 916;
			this.numJumpResetDuration.TargetLabel = this.lbJumpResetDuration;
			this.numJumpResetDuration.Text = "beautyFlatSlider1";
			this.numJumpResetDuration.Value = 30;
			this.numJumpResetDuration.WriteInLabel = true;
			this.numJumpResetDuration.Scroll += this.numJumpResetDuration_Scroll;
			this.lbJumpResetDuration.AutoResize = false;
			this.lbJumpResetDuration.BackColor = Color.FromArgb(12, 14, 14);
			this.lbJumpResetDuration.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbJumpResetDuration.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbJumpResetDuration.Location = new Point(179, 180);
			this.lbJumpResetDuration.Name = "lbJumpResetDuration";
			this.lbJumpResetDuration.Size = new Size(72, 18);
			this.lbJumpResetDuration.TabIndex = 918;
			this.lbJumpResetDuration.Text = "30 ms";
			this.lbJumpResetDuration.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbJumpResetDuration.TextPadding = new Padding(0);
			this.beautyLabel45.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel45.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel45.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel45.Location = new Point(11, 181);
			this.beautyLabel45.Name = "beautyLabel45";
			this.beautyLabel45.Size = new Size(68, 18);
			this.beautyLabel45.TabIndex = 917;
			this.beautyLabel45.Text = "Duration:";
			this.beautyLabel45.TextPadding = new Padding(0);
			this.numJumpResetDelay.AnimationTrigger = 0;
			this.numJumpResetDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.numJumpResetDelay.BarColor = Color.Firebrick;
			this.numJumpResetDelay.BorderColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetDelay.BorderRadius = 2f;
			this.numJumpResetDelay.BorderSize = 1;
			this.numJumpResetDelay.FillColor = Color.FromArgb(16, 18, 18);
			this.numJumpResetDelay.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numJumpResetDelay.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numJumpResetDelay.HoverBarColor = Color.Firebrick;
			this.numJumpResetDelay.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numJumpResetDelay.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetDelay.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numJumpResetDelay.Location = new Point(10, 155);
			this.numJumpResetDelay.Maximum = 100;
			this.numJumpResetDelay.Minimum = 0;
			this.numJumpResetDelay.Name = "numJumpResetDelay";
			this.numJumpResetDelay.Offset = 1f;
			this.numJumpResetDelay.ShowText = false;
			this.numJumpResetDelay.ShowValue = false;
			this.numJumpResetDelay.Size = new Size(240, 20);
			this.numJumpResetDelay.TabIndex = 913;
			this.numJumpResetDelay.TargetLabel = this.lbJumpResetDelay;
			this.numJumpResetDelay.Text = "beautyFlatSlider1";
			this.numJumpResetDelay.Value = 80;
			this.numJumpResetDelay.WriteInLabel = true;
			this.numJumpResetDelay.Scroll += this.numJumpResetDelay_Scroll;
			this.lbJumpResetDelay.AutoResize = false;
			this.lbJumpResetDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.lbJumpResetDelay.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbJumpResetDelay.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbJumpResetDelay.Location = new Point(178, 130);
			this.lbJumpResetDelay.Name = "lbJumpResetDelay";
			this.lbJumpResetDelay.Size = new Size(72, 18);
			this.lbJumpResetDelay.TabIndex = 915;
			this.lbJumpResetDelay.Text = "80 ms";
			this.lbJumpResetDelay.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbJumpResetDelay.TextPadding = new Padding(0);
			this.beautyLabel51.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel51.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel51.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel51.Location = new Point(10, 131);
			this.beautyLabel51.Name = "beautyLabel51";
			this.beautyLabel51.Size = new Size(47, 18);
			this.beautyLabel51.TabIndex = 914;
			this.beautyLabel51.Text = "Delay:";
			this.beautyLabel51.TextPadding = new Padding(0);
			this.JumpResetButton.AnimationSpeed = 0.6f;
			this.JumpResetButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.JumpResetButton.BorderRadius = 4f;
			this.JumpResetButton.BorderSize = 1f;
			this.JumpResetButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.JumpResetButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.JumpResetButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.JumpResetButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.JumpResetButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.JumpResetButton.FillColor = Color.FromArgb(16, 18, 18);
			this.JumpResetButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.JumpResetButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.JumpResetButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.JumpResetButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.JumpResetButton.ImageOffset = new Point(0, 0);
			this.JumpResetButton.Location = new Point(173, 50);
			this.JumpResetButton.MinimumSize = new Size(20, 22);
			this.JumpResetButton.MinimumTextWidth = 20;
			this.JumpResetButton.Name = "JumpResetButton";
			this.JumpResetButton.Size = new Size(77, 22);
			this.JumpResetButton.TabIndex = 913;
			this.JumpResetButton.Text = "None";
			this.JumpResetButton.TextOffset = new Point(0, 0);
			this.JumpResetButton.TextPadding = new Padding(0);
			this.JumpResetButton.YOffSet = 0;
			this.JumpResetButton.MouseDown += this.BindButtons_MouseDown;
			this.numJumpResetChance.AnimationTrigger = 0;
			this.numJumpResetChance.BackColor = Color.FromArgb(12, 14, 14);
			this.numJumpResetChance.BarColor = Color.Firebrick;
			this.numJumpResetChance.BorderColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetChance.BorderRadius = 2f;
			this.numJumpResetChance.BorderSize = 1;
			this.numJumpResetChance.FillColor = Color.FromArgb(16, 18, 18);
			this.numJumpResetChance.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numJumpResetChance.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numJumpResetChance.HoverBarColor = Color.Firebrick;
			this.numJumpResetChance.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numJumpResetChance.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numJumpResetChance.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numJumpResetChance.Location = new Point(10, 104);
			this.numJumpResetChance.Maximum = 100;
			this.numJumpResetChance.Minimum = 0;
			this.numJumpResetChance.Name = "numJumpResetChance";
			this.numJumpResetChance.Offset = 1f;
			this.numJumpResetChance.ShowText = false;
			this.numJumpResetChance.ShowValue = false;
			this.numJumpResetChance.Size = new Size(240, 20);
			this.numJumpResetChance.TabIndex = 910;
			this.numJumpResetChance.TargetLabel = this.lbJumpResetChance;
			this.numJumpResetChance.Text = "beautyFlatSlider1";
			this.numJumpResetChance.Value = 100;
			this.numJumpResetChance.WriteInLabel = true;
			this.numJumpResetChance.Scroll += this.numJumpResetChance_Scroll;
			this.lbJumpResetChance.AutoResize = false;
			this.lbJumpResetChance.BackColor = Color.FromArgb(12, 14, 14);
			this.lbJumpResetChance.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbJumpResetChance.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbJumpResetChance.Location = new Point(178, 80);
			this.lbJumpResetChance.Name = "lbJumpResetChance";
			this.lbJumpResetChance.Size = new Size(72, 18);
			this.lbJumpResetChance.TabIndex = 912;
			this.lbJumpResetChance.Text = "100%";
			this.lbJumpResetChance.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbJumpResetChance.TextPadding = new Padding(0);
			this.beautyLabel53.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel53.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel53.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel53.Location = new Point(10, 80);
			this.beautyLabel53.Name = "beautyLabel53";
			this.beautyLabel53.Size = new Size(59, 18);
			this.beautyLabel53.TabIndex = 911;
			this.beautyLabel53.Text = "Chance:";
			this.beautyLabel53.TextPadding = new Padding(0);
			this.beautyPanel12.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel12.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel12.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel12.BorderSizeBottom = 1f;
			this.beautyPanel12.BorderSizeLeft = 1f;
			this.beautyPanel12.BorderSizeRight = 1f;
			this.beautyPanel12.BorderSizeTop = 1f;
			this.beautyPanel12.Controls.Add(this.beautyLabel54);
			this.beautyPanel12.Dock = DockStyle.Top;
			this.beautyPanel12.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel12.FullHeight = 350;
			this.beautyPanel12.Location = new Point(0, 0);
			this.beautyPanel12.Name = "beautyPanel12";
			this.beautyPanel12.RadiusBottomLeft = 0f;
			this.beautyPanel12.RadiusBottomRight = 0f;
			this.beautyPanel12.RadiusTopLeft = 6f;
			this.beautyPanel12.RadiusTopRight = 6f;
			this.beautyPanel12.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel12.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel12.ScrollbarWidth = 4;
			this.beautyPanel12.Size = new Size(260, 40);
			this.beautyPanel12.TabIndex = 904;
			this.beautyLabel54.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel54.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel54.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel54.Location = new Point(10, 11);
			this.beautyLabel54.Name = "beautyLabel54";
			this.beautyLabel54.Size = new Size(88, 18);
			this.beautyLabel54.TabIndex = 905;
			this.beautyLabel54.Text = "Jump Reset";
			this.beautyLabel54.TextPadding = new Padding(0);
			this.cbJumpReset.AutoRoundCorners = true;
			this.cbJumpReset.BackColor = Color.FromArgb(12, 14, 14);
			this.cbJumpReset.Checked = false;
			this.cbJumpReset.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbJumpReset.CheckedState.BorderRadius = 4;
			this.cbJumpReset.CheckedState.BorderThickness = 1;
			this.cbJumpReset.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbJumpReset.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbJumpReset.CheckedState.InnerBorderRadius = 4;
			this.cbJumpReset.CheckedState.InnerBorderThickness = 0;
			this.cbJumpReset.CheckedState.InnerColor = Color.Firebrick;
			this.cbJumpReset.CheckedState.InnerOffset = 2;
			this.cbJumpReset.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbJumpReset.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbJumpReset.LinkedLabel = this.beautyLabel55;
			this.cbJumpReset.Location = new Point(10, 50);
			this.cbJumpReset.Name = "cbJumpReset";
			this.cbJumpReset.Size = new Size(44, 22);
			this.cbJumpReset.TabIndex = 894;
			this.cbJumpReset.Text = "beautyToggleSwitch4";
			this.cbJumpReset.ThumbSize = 12;
			this.cbJumpReset.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbJumpReset.UncheckedState.BorderRadius = 4;
			this.cbJumpReset.UncheckedState.BorderThickness = 1;
			this.cbJumpReset.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbJumpReset.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbJumpReset.UncheckedState.InnerBorderRadius = 4;
			this.cbJumpReset.UncheckedState.InnerBorderThickness = 0;
			this.cbJumpReset.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbJumpReset.UncheckedState.InnerOffset = 30;
			this.cbJumpReset.CheckedChanged += this.cbJumpReset_CheckedChanged;
			this.beautyLabel55.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel55.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel55.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel55.Location = new Point(60, 53);
			this.beautyLabel55.Name = "beautyLabel55";
			this.beautyLabel55.Size = new Size(53, 18);
			this.beautyLabel55.TabIndex = 842;
			this.beautyLabel55.Text = "Enable";
			this.beautyLabel55.TextPadding = new Padding(0);
			this.beautyPanel1.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel1.BorderSizeBottom = 1f;
			this.beautyPanel1.BorderSizeLeft = 1f;
			this.beautyPanel1.BorderSizeRight = 1f;
			this.beautyPanel1.BorderSizeTop = 1f;
			this.beautyPanel1.Controls.Add(this.beautyLabel41);
			this.beautyPanel1.Controls.Add(this.beautyLabel36);
			this.beautyPanel1.Controls.Add(this.ModeSprint);
			this.beautyPanel1.Controls.Add(this.ModeTypeSprint);
			this.beautyPanel1.Controls.Add(this.numSprintResetMaxRePress);
			this.beautyPanel1.Controls.Add(this.beautyLabel40);
			this.beautyPanel1.Controls.Add(this.lbSprintResetStop);
			this.beautyPanel1.Controls.Add(this.numSprintResetMinRePress);
			this.beautyPanel1.Controls.Add(this.beautyLabel38);
			this.beautyPanel1.Controls.Add(this.lbSprintResetDelay);
			this.beautyPanel1.Controls.Add(this.SprintResetButton);
			this.beautyPanel1.Controls.Add(this.numSprintResetChance);
			this.beautyPanel1.Controls.Add(this.beautyLabel43);
			this.beautyPanel1.Controls.Add(this.lbSprintResetChance);
			this.beautyPanel1.Controls.Add(this.beautyPanel5);
			this.beautyPanel1.Controls.Add(this.cbSprintReset);
			this.beautyPanel1.Controls.Add(this.beautyLabel48);
			this.beautyPanel1.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.FullHeight = 350;
			this.beautyPanel1.Location = new Point(11, 530);
			this.beautyPanel1.Name = "beautyPanel1";
			this.beautyPanel1.RadiusBottomLeft = 6f;
			this.beautyPanel1.RadiusBottomRight = 6f;
			this.beautyPanel1.RadiusTopLeft = 6f;
			this.beautyPanel1.RadiusTopRight = 6f;
			this.beautyPanel1.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel1.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel1.ScrollbarWidth = 4;
			this.beautyPanel1.Size = new Size(260, 424);
			this.beautyPanel1.TabIndex = 910;
			this.beautyLabel41.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel41.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel41.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel41.Location = new Point(10, 270);
			this.beautyLabel41.Name = "beautyLabel41";
			this.beautyLabel41.Size = new Size(47, 18);
			this.beautyLabel41.TabIndex = 940;
			this.beautyLabel41.Text = "Mode:";
			this.beautyLabel41.TextPadding = new Padding(0);
			this.beautyLabel36.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel36.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel36.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel36.Location = new Point(9, 237);
			this.beautyLabel36.Name = "beautyLabel36";
			this.beautyLabel36.Size = new Size(80, 18);
			this.beautyLabel36.TabIndex = 939;
			this.beautyLabel36.Text = "Mode Type:";
			this.beautyLabel36.TextPadding = new Padding(0);
			this.ModeSprint.BorderColor = Color.FromArgb(20, 22, 22);
			this.ModeSprint.BorderRadius = 2f;
			this.ModeSprint.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ModeSprint.FillColor = Color.FromArgb(16, 18, 18);
			this.ModeSprint.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.ModeSprint.ForeColor = Color.FromArgb(120, 120, 130);
			this.ModeSprint.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.ModeSprint.ForeText = "Attack";
			this.ModeSprint.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ModeSprint.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ModeSprint.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.ModeSprint.ItemHeight = 30;
			this.ModeSprint.Items = new string[]
			{
				"Attack",
				"Damage"
			};
			this.ModeSprint.Location = new Point(157, 270);
			this.ModeSprint.Name = "ModeSprint";
			this.ModeSprint.Size = new Size(93, 24);
			this.ModeSprint.TabIndex = 938;
			this.ModeSprint.Text = "Attack";
			this.ModeSprint.IndexChanged += this.ModeSprint_IndexChanged_1;
			this.ModeTypeSprint.BorderColor = Color.FromArgb(20, 22, 22);
			this.ModeTypeSprint.BorderRadius = 2f;
			this.ModeTypeSprint.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ModeTypeSprint.FillColor = Color.FromArgb(16, 18, 18);
			this.ModeTypeSprint.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.ModeTypeSprint.ForeColor = Color.FromArgb(120, 120, 130);
			this.ModeTypeSprint.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.ModeTypeSprint.ForeText = "W-tap";
			this.ModeTypeSprint.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ModeTypeSprint.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ModeTypeSprint.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.ModeTypeSprint.ItemHeight = 30;
			this.ModeTypeSprint.Items = new string[]
			{
				"W-tap",
				"S-tap",
				"Shift tap",
				"Blatant",
				"NoStop"
			};
			this.ModeTypeSprint.Location = new Point(157, 235);
			this.ModeTypeSprint.Name = "ModeTypeSprint";
			this.ModeTypeSprint.Size = new Size(93, 24);
			this.ModeTypeSprint.TabIndex = 937;
			this.ModeTypeSprint.Text = "W-tap";
			this.ModeTypeSprint.IndexChanged += this.ModeSprint_IndexChanged;
			this.numSprintResetMaxRePress.AnimationTrigger = 0;
			this.numSprintResetMaxRePress.BackColor = Color.FromArgb(12, 14, 14);
			this.numSprintResetMaxRePress.BarColor = Color.Firebrick;
			this.numSprintResetMaxRePress.BorderColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetMaxRePress.BorderRadius = 2f;
			this.numSprintResetMaxRePress.BorderSize = 1;
			this.numSprintResetMaxRePress.FillColor = Color.FromArgb(16, 18, 18);
			this.numSprintResetMaxRePress.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numSprintResetMaxRePress.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numSprintResetMaxRePress.HoverBarColor = Color.Firebrick;
			this.numSprintResetMaxRePress.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numSprintResetMaxRePress.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetMaxRePress.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numSprintResetMaxRePress.Location = new Point(10, 205);
			this.numSprintResetMaxRePress.Maximum = 300;
			this.numSprintResetMaxRePress.Minimum = 30;
			this.numSprintResetMaxRePress.Name = "numSprintResetMaxRePress";
			this.numSprintResetMaxRePress.Offset = 1f;
			this.numSprintResetMaxRePress.ShowText = false;
			this.numSprintResetMaxRePress.ShowValue = false;
			this.numSprintResetMaxRePress.Size = new Size(240, 20);
			this.numSprintResetMaxRePress.TabIndex = 928;
			this.numSprintResetMaxRePress.TargetLabel = this.lbSprintResetStop;
			this.numSprintResetMaxRePress.Text = "beautyFlatSlider2";
			this.numSprintResetMaxRePress.Value = 50;
			this.numSprintResetMaxRePress.WriteInLabel = true;
			this.numSprintResetMaxRePress.Scroll += this.numSuperKnockbackMaxHurt_Scroll;
			this.lbSprintResetStop.AutoResize = false;
			this.lbSprintResetStop.BackColor = Color.FromArgb(12, 14, 14);
			this.lbSprintResetStop.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbSprintResetStop.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbSprintResetStop.Location = new Point(178, 181);
			this.lbSprintResetStop.Name = "lbSprintResetStop";
			this.lbSprintResetStop.Size = new Size(72, 18);
			this.lbSprintResetStop.TabIndex = 930;
			this.lbSprintResetStop.Text = "50 ms";
			this.lbSprintResetStop.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbSprintResetStop.TextPadding = new Padding(0);
			this.beautyLabel40.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel40.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel40.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel40.Location = new Point(10, 181);
			this.beautyLabel40.Name = "beautyLabel40";
			this.beautyLabel40.Size = new Size(98, 18);
			this.beautyLabel40.TabIndex = 929;
			this.beautyLabel40.Text = "StopDuration:";
			this.beautyLabel40.TextPadding = new Padding(0);
			this.numSprintResetMinRePress.AnimationTrigger = 0;
			this.numSprintResetMinRePress.BackColor = Color.FromArgb(12, 14, 14);
			this.numSprintResetMinRePress.BarColor = Color.Firebrick;
			this.numSprintResetMinRePress.BorderColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetMinRePress.BorderRadius = 2f;
			this.numSprintResetMinRePress.BorderSize = 1;
			this.numSprintResetMinRePress.FillColor = Color.FromArgb(16, 18, 18);
			this.numSprintResetMinRePress.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numSprintResetMinRePress.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numSprintResetMinRePress.HoverBarColor = Color.Firebrick;
			this.numSprintResetMinRePress.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numSprintResetMinRePress.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetMinRePress.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numSprintResetMinRePress.Location = new Point(10, 155);
			this.numSprintResetMinRePress.Maximum = 400;
			this.numSprintResetMinRePress.Minimum = 0;
			this.numSprintResetMinRePress.Name = "numSprintResetMinRePress";
			this.numSprintResetMinRePress.Offset = 1f;
			this.numSprintResetMinRePress.ShowText = false;
			this.numSprintResetMinRePress.ShowValue = false;
			this.numSprintResetMinRePress.Size = new Size(240, 20);
			this.numSprintResetMinRePress.TabIndex = 913;
			this.numSprintResetMinRePress.TargetLabel = this.lbSprintResetDelay;
			this.numSprintResetMinRePress.Text = "beautyFlatSlider1";
			this.numSprintResetMinRePress.Value = 300;
			this.numSprintResetMinRePress.WriteInLabel = true;
			this.numSprintResetMinRePress.Scroll += this.numSuperKnockbackDelay_Scroll;
			this.lbSprintResetDelay.AutoResize = false;
			this.lbSprintResetDelay.BackColor = Color.FromArgb(12, 14, 14);
			this.lbSprintResetDelay.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbSprintResetDelay.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbSprintResetDelay.Location = new Point(178, 130);
			this.lbSprintResetDelay.Name = "lbSprintResetDelay";
			this.lbSprintResetDelay.Size = new Size(72, 18);
			this.lbSprintResetDelay.TabIndex = 915;
			this.lbSprintResetDelay.Text = "300 ms";
			this.lbSprintResetDelay.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbSprintResetDelay.TextPadding = new Padding(0);
			this.beautyLabel38.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel38.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel38.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel38.Location = new Point(10, 131);
			this.beautyLabel38.Name = "beautyLabel38";
			this.beautyLabel38.Size = new Size(47, 18);
			this.beautyLabel38.TabIndex = 914;
			this.beautyLabel38.Text = "Delay:";
			this.beautyLabel38.TextPadding = new Padding(0);
			this.SprintResetButton.AnimationSpeed = 0.6f;
			this.SprintResetButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.SprintResetButton.BorderRadius = 4f;
			this.SprintResetButton.BorderSize = 1f;
			this.SprintResetButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.SprintResetButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.SprintResetButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.SprintResetButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.SprintResetButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.SprintResetButton.FillColor = Color.FromArgb(16, 18, 18);
			this.SprintResetButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.SprintResetButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.SprintResetButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SprintResetButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.SprintResetButton.ImageOffset = new Point(0, 0);
			this.SprintResetButton.Location = new Point(173, 50);
			this.SprintResetButton.MinimumSize = new Size(20, 22);
			this.SprintResetButton.MinimumTextWidth = 20;
			this.SprintResetButton.Name = "SprintResetButton";
			this.SprintResetButton.Size = new Size(77, 22);
			this.SprintResetButton.TabIndex = 913;
			this.SprintResetButton.Text = "None";
			this.SprintResetButton.TextOffset = new Point(0, 0);
			this.SprintResetButton.TextPadding = new Padding(0);
			this.SprintResetButton.YOffSet = 0;
			this.SprintResetButton.MouseDown += this.BindButtons_MouseDown;
			this.numSprintResetChance.AnimationTrigger = 0;
			this.numSprintResetChance.BackColor = Color.FromArgb(12, 14, 14);
			this.numSprintResetChance.BarColor = Color.Firebrick;
			this.numSprintResetChance.BorderColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetChance.BorderRadius = 2f;
			this.numSprintResetChance.BorderSize = 1;
			this.numSprintResetChance.FillColor = Color.FromArgb(16, 18, 18);
			this.numSprintResetChance.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.numSprintResetChance.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.numSprintResetChance.HoverBarColor = Color.Firebrick;
			this.numSprintResetChance.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.numSprintResetChance.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.numSprintResetChance.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.numSprintResetChance.Location = new Point(10, 104);
			this.numSprintResetChance.Maximum = 100;
			this.numSprintResetChance.Minimum = 0;
			this.numSprintResetChance.Name = "numSprintResetChance";
			this.numSprintResetChance.Offset = 1f;
			this.numSprintResetChance.ShowText = false;
			this.numSprintResetChance.ShowValue = false;
			this.numSprintResetChance.Size = new Size(240, 20);
			this.numSprintResetChance.TabIndex = 910;
			this.numSprintResetChance.TargetLabel = this.lbSprintResetChance;
			this.numSprintResetChance.Text = "beautyFlatSlider1";
			this.numSprintResetChance.Value = 100;
			this.numSprintResetChance.WriteInLabel = true;
			this.numSprintResetChance.Scroll += this.sliderSuperKnockbackChance_Scroll;
			this.lbSprintResetChance.AutoResize = false;
			this.lbSprintResetChance.BackColor = Color.FromArgb(12, 14, 14);
			this.lbSprintResetChance.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbSprintResetChance.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbSprintResetChance.Location = new Point(178, 80);
			this.lbSprintResetChance.Name = "lbSprintResetChance";
			this.lbSprintResetChance.Size = new Size(72, 18);
			this.lbSprintResetChance.TabIndex = 912;
			this.lbSprintResetChance.Text = "100%";
			this.lbSprintResetChance.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbSprintResetChance.TextPadding = new Padding(0);
			this.beautyLabel43.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel43.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel43.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel43.Location = new Point(10, 80);
			this.beautyLabel43.Name = "beautyLabel43";
			this.beautyLabel43.Size = new Size(59, 18);
			this.beautyLabel43.TabIndex = 911;
			this.beautyLabel43.Text = "Chance:";
			this.beautyLabel43.TextPadding = new Padding(0);
			this.beautyPanel5.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel5.BorderSizeBottom = 1f;
			this.beautyPanel5.BorderSizeLeft = 1f;
			this.beautyPanel5.BorderSizeRight = 1f;
			this.beautyPanel5.BorderSizeTop = 1f;
			this.beautyPanel5.Controls.Add(this.beautyLabel47);
			this.beautyPanel5.Dock = DockStyle.Top;
			this.beautyPanel5.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel5.FullHeight = 350;
			this.beautyPanel5.Location = new Point(0, 0);
			this.beautyPanel5.Name = "beautyPanel5";
			this.beautyPanel5.RadiusBottomLeft = 0f;
			this.beautyPanel5.RadiusBottomRight = 0f;
			this.beautyPanel5.RadiusTopLeft = 6f;
			this.beautyPanel5.RadiusTopRight = 6f;
			this.beautyPanel5.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel5.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel5.ScrollbarWidth = 4;
			this.beautyPanel5.Size = new Size(260, 40);
			this.beautyPanel5.TabIndex = 904;
			this.beautyLabel47.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel47.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel47.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel47.Location = new Point(10, 11);
			this.beautyLabel47.Name = "beautyLabel47";
			this.beautyLabel47.Size = new Size(91, 18);
			this.beautyLabel47.TabIndex = 905;
			this.beautyLabel47.Text = "Sprint Reset";
			this.beautyLabel47.TextPadding = new Padding(0);
			this.cbSprintReset.AutoRoundCorners = true;
			this.cbSprintReset.BackColor = Color.FromArgb(12, 14, 14);
			this.cbSprintReset.Checked = false;
			this.cbSprintReset.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbSprintReset.CheckedState.BorderRadius = 4;
			this.cbSprintReset.CheckedState.BorderThickness = 1;
			this.cbSprintReset.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbSprintReset.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbSprintReset.CheckedState.InnerBorderRadius = 4;
			this.cbSprintReset.CheckedState.InnerBorderThickness = 0;
			this.cbSprintReset.CheckedState.InnerColor = Color.Firebrick;
			this.cbSprintReset.CheckedState.InnerOffset = 2;
			this.cbSprintReset.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbSprintReset.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbSprintReset.LinkedLabel = this.beautyLabel48;
			this.cbSprintReset.Location = new Point(10, 50);
			this.cbSprintReset.Name = "cbSprintReset";
			this.cbSprintReset.Size = new Size(44, 22);
			this.cbSprintReset.TabIndex = 894;
			this.cbSprintReset.Text = "beautyToggleSwitch4";
			this.cbSprintReset.ThumbSize = 12;
			this.cbSprintReset.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbSprintReset.UncheckedState.BorderRadius = 4;
			this.cbSprintReset.UncheckedState.BorderThickness = 1;
			this.cbSprintReset.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbSprintReset.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbSprintReset.UncheckedState.InnerBorderRadius = 4;
			this.cbSprintReset.UncheckedState.InnerBorderThickness = 0;
			this.cbSprintReset.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbSprintReset.UncheckedState.InnerOffset = 30;
			this.cbSprintReset.CheckedChanged += this.cbSuperKnockback_CheckedChanged;
			this.beautyLabel48.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel48.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel48.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel48.Location = new Point(60, 53);
			this.beautyLabel48.Name = "beautyLabel48";
			this.beautyLabel48.Size = new Size(53, 18);
			this.beautyLabel48.TabIndex = 842;
			this.beautyLabel48.Text = "Enable";
			this.beautyLabel48.TextPadding = new Padding(0);
			this.beautyPanel2.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel2.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel2.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel2.BorderSizeBottom = 1f;
			this.beautyPanel2.BorderSizeLeft = 1f;
			this.beautyPanel2.BorderSizeRight = 1f;
			this.beautyPanel2.BorderSizeTop = 1f;
			this.beautyPanel2.Controls.Add(this.cbAimBreakBlocks);
			this.beautyPanel2.Controls.Add(this.beautyLabel37);
			this.beautyPanel2.Controls.Add(this.aimlabeldist);
			this.beautyPanel2.Controls.Add(this.cbLockTarget);
			this.beautyPanel2.Controls.Add(this.beautyLabel35);
			this.beautyPanel2.Controls.Add(this.cbMouseMove);
			this.beautyPanel2.Controls.Add(this.beautyLabel15);
			this.beautyPanel2.Controls.Add(this.cbVertical);
			this.beautyPanel2.Controls.Add(this.slideverticalaim);
			this.beautyPanel2.Controls.Add(this.beautyLabel4);
			this.beautyPanel2.Controls.Add(this.slidehorizontalaim);
			this.beautyPanel2.Controls.Add(this.beautyLabel18);
			this.beautyPanel2.Controls.Add(this.beautyLabel25);
			this.beautyPanel2.Controls.Add(this.labelVertical);
			this.beautyPanel2.Controls.Add(this.labelHorizontal);
			this.beautyPanel2.Controls.Add(this.cbHitboxClosest);
			this.beautyPanel2.Controls.Add(this.AimAssistOnlyWeapon);
			this.beautyPanel2.Controls.Add(this.beautyLabel3);
			this.beautyPanel2.Controls.Add(this.beautyLabel8);
			this.beautyPanel2.Controls.Add(this.AimBindButton);
			this.beautyPanel2.Controls.Add(this.slideDistance);
			this.beautyPanel2.Controls.Add(this.AimAssistFovSlider);
			this.beautyPanel2.Controls.Add(this.beautyLabel14);
			this.beautyPanel2.Controls.Add(this.lbfov);
			this.beautyPanel2.Controls.Add(this.beautyLabel17);
			this.beautyPanel2.Controls.Add(this.AimAssistClickingOnly);
			this.beautyPanel2.Controls.Add(this.beautyLabel10);
			this.beautyPanel2.Controls.Add(this.beautyPanel6);
			this.beautyPanel2.Controls.Add(this.AimEnable);
			this.beautyPanel2.Controls.Add(this.AimAssistThroughWall);
			this.beautyPanel2.Controls.Add(this.beautyLabel16);
			this.beautyPanel2.Controls.Add(this.beautyLabel13);
			this.beautyPanel2.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel2.FullHeight = 350;
			this.beautyPanel2.Location = new Point(281, 21);
			this.beautyPanel2.Name = "beautyPanel2";
			this.beautyPanel2.RadiusBottomLeft = 6f;
			this.beautyPanel2.RadiusBottomRight = 6f;
			this.beautyPanel2.RadiusTopLeft = 6f;
			this.beautyPanel2.RadiusTopRight = 6f;
			this.beautyPanel2.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel2.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel2.ScrollbarWidth = 4;
			this.beautyPanel2.Size = new Size(260, 523);
			this.beautyPanel2.TabIndex = 909;
			this.cbAimBreakBlocks.AnimationSpeed = 0.6f;
			this.cbAimBreakBlocks.BackColor = Color.FromArgb(12, 14, 14);
			this.cbAimBreakBlocks.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbAimBreakBlocks.BorderRadius = 2f;
			this.cbAimBreakBlocks.BorderSize = 1f;
			this.cbAimBreakBlocks.CheckedBorderColor = Color.Firebrick;
			this.cbAimBreakBlocks.CheckedFillColor = Color.Firebrick;
			this.cbAimBreakBlocks.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbAimBreakBlocks.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbAimBreakBlocks.CheckMarkScale = 0.6f;
			this.cbAimBreakBlocks.FillColor = Color.FromArgb(16, 18, 18);
			this.cbAimBreakBlocks.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbAimBreakBlocks.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbAimBreakBlocks.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbAimBreakBlocks.Location = new Point(230, 481);
			this.cbAimBreakBlocks.Name = "cbAimBreakBlocks";
			this.cbAimBreakBlocks.Size = new Size(22, 22);
			this.cbAimBreakBlocks.TabIndex = 934;
			this.cbAimBreakBlocks.TargetLabel = this.beautyLabel37;
			this.cbAimBreakBlocks.Text = "beautyCheckBox3";
			this.cbAimBreakBlocks.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbAimBreakBlocks.CheckedChanged += this.cbAimBreakBlocks_CheckedChanged;
			this.beautyLabel37.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel37.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel37.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel37.Location = new Point(10, 483);
			this.beautyLabel37.Name = "beautyLabel37";
			this.beautyLabel37.Size = new Size(97, 18);
			this.beautyLabel37.TabIndex = 935;
			this.beautyLabel37.Text = "Break Blocks";
			this.beautyLabel37.TextPadding = new Padding(0);
			this.aimlabeldist.AutoResize = false;
			this.aimlabeldist.BackColor = Color.FromArgb(12, 14, 14);
			this.aimlabeldist.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.aimlabeldist.ForeColor = Color.FromArgb(70, 70, 80);
			this.aimlabeldist.Location = new Point(178, 178);
			this.aimlabeldist.Name = "aimlabeldist";
			this.aimlabeldist.Size = new Size(72, 18);
			this.aimlabeldist.TabIndex = 933;
			this.aimlabeldist.Text = "4.00";
			this.aimlabeldist.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.aimlabeldist.TextPadding = new Padding(0);
			this.cbLockTarget.AnimationSpeed = 0.6f;
			this.cbLockTarget.BackColor = Color.FromArgb(12, 14, 14);
			this.cbLockTarget.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbLockTarget.BorderRadius = 2f;
			this.cbLockTarget.BorderSize = 1f;
			this.cbLockTarget.CheckedBorderColor = Color.Firebrick;
			this.cbLockTarget.CheckedFillColor = Color.Firebrick;
			this.cbLockTarget.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbLockTarget.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbLockTarget.CheckMarkScale = 0.6f;
			this.cbLockTarget.FillColor = Color.FromArgb(16, 18, 18);
			this.cbLockTarget.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbLockTarget.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbLockTarget.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbLockTarget.Location = new Point(230, 452);
			this.cbLockTarget.Name = "cbLockTarget";
			this.cbLockTarget.Size = new Size(22, 22);
			this.cbLockTarget.TabIndex = 931;
			this.cbLockTarget.TargetLabel = this.beautyLabel35;
			this.cbLockTarget.Text = "beautyCheckBox3";
			this.cbLockTarget.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbLockTarget.CheckedChanged += this.cbLockTarget_CheckedChanged;
			this.beautyLabel35.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel35.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel35.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel35.Location = new Point(10, 455);
			this.beautyLabel35.Name = "beautyLabel35";
			this.beautyLabel35.Size = new Size(85, 18);
			this.beautyLabel35.TabIndex = 932;
			this.beautyLabel35.Text = "Lock Target";
			this.beautyLabel35.TextPadding = new Padding(0);
			this.cbMouseMove.AnimationSpeed = 0.6f;
			this.cbMouseMove.BackColor = Color.FromArgb(12, 14, 14);
			this.cbMouseMove.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbMouseMove.BorderRadius = 2f;
			this.cbMouseMove.BorderSize = 1f;
			this.cbMouseMove.CheckedBorderColor = Color.Firebrick;
			this.cbMouseMove.CheckedFillColor = Color.Firebrick;
			this.cbMouseMove.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbMouseMove.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbMouseMove.CheckMarkScale = 0.6f;
			this.cbMouseMove.FillColor = Color.FromArgb(16, 18, 18);
			this.cbMouseMove.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbMouseMove.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbMouseMove.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbMouseMove.Location = new Point(230, 424);
			this.cbMouseMove.Name = "cbMouseMove";
			this.cbMouseMove.Size = new Size(22, 22);
			this.cbMouseMove.TabIndex = 929;
			this.cbMouseMove.TargetLabel = this.beautyLabel15;
			this.cbMouseMove.Text = "beautyCheckBox3";
			this.cbMouseMove.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbMouseMove.CheckedChanged += this.cbMouseMove_CheckedChanged;
			this.beautyLabel15.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel15.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel15.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel15.Location = new Point(10, 428);
			this.beautyLabel15.Name = "beautyLabel15";
			this.beautyLabel15.Size = new Size(93, 18);
			this.beautyLabel15.TabIndex = 930;
			this.beautyLabel15.Text = "Mouse move";
			this.beautyLabel15.TextPadding = new Padding(0);
			this.cbVertical.AnimationSpeed = 0.6f;
			this.cbVertical.BackColor = Color.FromArgb(12, 14, 14);
			this.cbVertical.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbVertical.BorderRadius = 2f;
			this.cbVertical.BorderSize = 1f;
			this.cbVertical.CheckedBorderColor = Color.Firebrick;
			this.cbVertical.CheckedFillColor = Color.Firebrick;
			this.cbVertical.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbVertical.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbVertical.CheckMarkScale = 0.6f;
			this.cbVertical.FillColor = Color.FromArgb(16, 18, 18);
			this.cbVertical.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbVertical.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbVertical.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbVertical.Location = new Point(230, 396);
			this.cbVertical.Name = "cbVertical";
			this.cbVertical.Size = new Size(22, 22);
			this.cbVertical.TabIndex = 923;
			this.cbVertical.TargetLabel = this.beautyLabel4;
			this.cbVertical.Text = "beautyCheckBox3";
			this.cbVertical.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbVertical.CheckedChanged += this.cbVertical_CheckedChanged;
			this.beautyLabel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel4.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel4.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel4.Location = new Point(10, 400);
			this.beautyLabel4.Name = "beautyLabel4";
			this.beautyLabel4.Size = new Size(58, 18);
			this.beautyLabel4.TabIndex = 924;
			this.beautyLabel4.Text = "Vertical";
			this.beautyLabel4.TextPadding = new Padding(0);
			this.slideverticalaim.AnimationTrigger = 0;
			this.slideverticalaim.BackColor = Color.FromArgb(12, 14, 14);
			this.slideverticalaim.BarColor = Color.Firebrick;
			this.slideverticalaim.BorderColor = Color.FromArgb(20, 22, 22);
			this.slideverticalaim.BorderRadius = 2f;
			this.slideverticalaim.BorderSize = 1;
			this.slideverticalaim.FillColor = Color.FromArgb(16, 18, 18);
			this.slideverticalaim.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.slideverticalaim.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.slideverticalaim.HoverBarColor = Color.Firebrick;
			this.slideverticalaim.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.slideverticalaim.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.slideverticalaim.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.slideverticalaim.Location = new Point(10, 152);
			this.slideverticalaim.Maximum = 150;
			this.slideverticalaim.Minimum = 5;
			this.slideverticalaim.Name = "slideverticalaim";
			this.slideverticalaim.Offset = 1f;
			this.slideverticalaim.ShowText = false;
			this.slideverticalaim.ShowValue = true;
			this.slideverticalaim.Size = new Size(240, 20);
			this.slideverticalaim.TabIndex = 926;
			this.slideverticalaim.TargetLabel = this.labelVertical;
			this.slideverticalaim.Text = "beautyFlatSlider2";
			this.slideverticalaim.Value = 25;
			this.slideverticalaim.WriteInLabel = true;
			this.slideverticalaim.Scroll += this.slideverticalaim_Scroll;
			this.labelVertical.AutoResize = false;
			this.labelVertical.BackColor = Color.FromArgb(12, 14, 14);
			this.labelVertical.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelVertical.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelVertical.Location = new Point(178, 128);
			this.labelVertical.Name = "labelVertical";
			this.labelVertical.Size = new Size(72, 18);
			this.labelVertical.TabIndex = 928;
			this.labelVertical.Text = "2.5";
			this.labelVertical.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelVertical.TextPadding = new Padding(0);
			this.slidehorizontalaim.AnimationTrigger = 0;
			this.slidehorizontalaim.BackColor = Color.FromArgb(12, 14, 14);
			this.slidehorizontalaim.BarColor = Color.Firebrick;
			this.slidehorizontalaim.BorderColor = Color.FromArgb(20, 22, 22);
			this.slidehorizontalaim.BorderRadius = 2f;
			this.slidehorizontalaim.BorderSize = 1;
			this.slidehorizontalaim.FillColor = Color.FromArgb(16, 18, 18);
			this.slidehorizontalaim.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.slidehorizontalaim.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.slidehorizontalaim.HoverBarColor = Color.Firebrick;
			this.slidehorizontalaim.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.slidehorizontalaim.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.slidehorizontalaim.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.slidehorizontalaim.Location = new Point(11, 102);
			this.slidehorizontalaim.Maximum = 150;
			this.slidehorizontalaim.Minimum = 5;
			this.slidehorizontalaim.Name = "slidehorizontalaim";
			this.slidehorizontalaim.Offset = 1f;
			this.slidehorizontalaim.ShowText = false;
			this.slidehorizontalaim.ShowValue = true;
			this.slidehorizontalaim.Size = new Size(240, 20);
			this.slidehorizontalaim.TabIndex = 923;
			this.slidehorizontalaim.TargetLabel = this.labelHorizontal;
			this.slidehorizontalaim.Text = "beautyFlatSlider1";
			this.slidehorizontalaim.Value = 50;
			this.slidehorizontalaim.WriteInLabel = true;
			this.slidehorizontalaim.Scroll += this.slidehorizontalaim_Scroll;
			this.labelHorizontal.AutoResize = false;
			this.labelHorizontal.BackColor = Color.FromArgb(12, 14, 14);
			this.labelHorizontal.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelHorizontal.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelHorizontal.Location = new Point(180, 78);
			this.labelHorizontal.Name = "labelHorizontal";
			this.labelHorizontal.Size = new Size(72, 18);
			this.labelHorizontal.TabIndex = 925;
			this.labelHorizontal.Text = "5.0";
			this.labelHorizontal.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelHorizontal.TextPadding = new Padding(0);
			this.beautyLabel18.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel18.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel18.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel18.Location = new Point(10, 78);
			this.beautyLabel18.Name = "beautyLabel18";
			this.beautyLabel18.Size = new Size(124, 18);
			this.beautyLabel18.TabIndex = 924;
			this.beautyLabel18.Text = "Speed Horizontal:";
			this.beautyLabel18.TextPadding = new Padding(0);
			this.beautyLabel25.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel25.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel25.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel25.Location = new Point(10, 128);
			this.beautyLabel25.Name = "beautyLabel25";
			this.beautyLabel25.Size = new Size(106, 18);
			this.beautyLabel25.TabIndex = 927;
			this.beautyLabel25.Text = "Speed Vertical:";
			this.beautyLabel25.TextPadding = new Padding(0);
			this.cbHitboxClosest.AnimationSpeed = 0.6f;
			this.cbHitboxClosest.BackColor = Color.FromArgb(12, 14, 14);
			this.cbHitboxClosest.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbHitboxClosest.BorderRadius = 2f;
			this.cbHitboxClosest.BorderSize = 1f;
			this.cbHitboxClosest.CheckedBorderColor = Color.Firebrick;
			this.cbHitboxClosest.CheckedFillColor = Color.Firebrick;
			this.cbHitboxClosest.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbHitboxClosest.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbHitboxClosest.CheckMarkScale = 0.6f;
			this.cbHitboxClosest.FillColor = Color.FromArgb(16, 18, 18);
			this.cbHitboxClosest.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbHitboxClosest.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbHitboxClosest.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbHitboxClosest.Location = new Point(230, 368);
			this.cbHitboxClosest.Name = "cbHitboxClosest";
			this.cbHitboxClosest.Size = new Size(22, 22);
			this.cbHitboxClosest.TabIndex = 921;
			this.cbHitboxClosest.TargetLabel = this.beautyLabel3;
			this.cbHitboxClosest.Text = "beautyCheckBox3";
			this.cbHitboxClosest.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbHitboxClosest.CheckedChanged += this.cbHitboxClosest_CheckedChanged;
			this.beautyLabel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel3.Location = new Point(10, 372);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(105, 18);
			this.beautyLabel3.TabIndex = 922;
			this.beautyLabel3.Text = "Closest Hitbox";
			this.beautyLabel3.TextPadding = new Padding(0);
			this.AimAssistOnlyWeapon.AnimationSpeed = 0.6f;
			this.AimAssistOnlyWeapon.BackColor = Color.FromArgb(12, 14, 14);
			this.AimAssistOnlyWeapon.BorderColor = Color.FromArgb(20, 22, 22);
			this.AimAssistOnlyWeapon.BorderRadius = 2f;
			this.AimAssistOnlyWeapon.BorderSize = 1f;
			this.AimAssistOnlyWeapon.CheckedBorderColor = Color.Firebrick;
			this.AimAssistOnlyWeapon.CheckedFillColor = Color.Firebrick;
			this.AimAssistOnlyWeapon.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.AimAssistOnlyWeapon.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.AimAssistOnlyWeapon.CheckMarkScale = 0.6f;
			this.AimAssistOnlyWeapon.FillColor = Color.FromArgb(16, 18, 18);
			this.AimAssistOnlyWeapon.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.AimAssistOnlyWeapon.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.AimAssistOnlyWeapon.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.AimAssistOnlyWeapon.Location = new Point(230, 340);
			this.AimAssistOnlyWeapon.Name = "AimAssistOnlyWeapon";
			this.AimAssistOnlyWeapon.Size = new Size(22, 22);
			this.AimAssistOnlyWeapon.TabIndex = 919;
			this.AimAssistOnlyWeapon.TargetLabel = this.beautyLabel8;
			this.AimAssistOnlyWeapon.Text = "beautyCheckBox3";
			this.AimAssistOnlyWeapon.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.AimAssistOnlyWeapon.CheckedChanged += this.AimAssistOnlyWeapon_CheckedChanged;
			this.beautyLabel8.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel8.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel8.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel8.Location = new Point(10, 344);
			this.beautyLabel8.Name = "beautyLabel8";
			this.beautyLabel8.Size = new Size(126, 18);
			this.beautyLabel8.TabIndex = 920;
			this.beautyLabel8.Text = "Only with Weapon";
			this.beautyLabel8.TextPadding = new Padding(0);
			this.AimBindButton.AnimationSpeed = 0.6f;
			this.AimBindButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.AimBindButton.BorderRadius = 4f;
			this.AimBindButton.BorderSize = 1f;
			this.AimBindButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.AimBindButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.AimBindButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.AimBindButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.AimBindButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.AimBindButton.FillColor = Color.FromArgb(16, 18, 18);
			this.AimBindButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.AimBindButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.AimBindButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.AimBindButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.AimBindButton.ImageOffset = new Point(0, 0);
			this.AimBindButton.Location = new Point(173, 50);
			this.AimBindButton.MinimumSize = new Size(20, 22);
			this.AimBindButton.MinimumTextWidth = 20;
			this.AimBindButton.Name = "AimBindButton";
			this.AimBindButton.Size = new Size(77, 22);
			this.AimBindButton.TabIndex = 918;
			this.AimBindButton.Text = "None";
			this.AimBindButton.TextOffset = new Point(0, 0);
			this.AimBindButton.TextPadding = new Padding(0);
			this.AimBindButton.YOffSet = 0;
			this.AimBindButton.MouseDown += this.BindButtons_MouseDown;
			this.slideDistance.AnimationTrigger = 0;
			this.slideDistance.BackColor = Color.FromArgb(12, 14, 14);
			this.slideDistance.BarColor = Color.Firebrick;
			this.slideDistance.BorderColor = Color.FromArgb(20, 22, 22);
			this.slideDistance.BorderRadius = 2f;
			this.slideDistance.BorderSize = 1;
			this.slideDistance.FillColor = Color.FromArgb(16, 18, 18);
			this.slideDistance.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.slideDistance.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.slideDistance.HoverBarColor = Color.Firebrick;
			this.slideDistance.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.slideDistance.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.slideDistance.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.slideDistance.Location = new Point(10, 199);
			this.slideDistance.Maximum = 1000;
			this.slideDistance.Minimum = 100;
			this.slideDistance.Name = "slideDistance";
			this.slideDistance.Offset = 1f;
			this.slideDistance.ShowText = false;
			this.slideDistance.ShowValue = false;
			this.slideDistance.Size = new Size(240, 20);
			this.slideDistance.TabIndex = 917;
			this.slideDistance.TargetLabel = this.aimlabeldist;
			this.slideDistance.Text = "beautyFlatSlider1";
			this.slideDistance.Value = 400;
			this.slideDistance.WriteInLabel = true;
			this.slideDistance.Scroll += this.slideDistance_Scroll;
			this.AimAssistFovSlider.AnimationTrigger = 0;
			this.AimAssistFovSlider.BackColor = Color.FromArgb(12, 14, 14);
			this.AimAssistFovSlider.BarColor = Color.Firebrick;
			this.AimAssistFovSlider.BorderColor = Color.FromArgb(20, 22, 22);
			this.AimAssistFovSlider.BorderRadius = 2f;
			this.AimAssistFovSlider.BorderSize = 1;
			this.AimAssistFovSlider.FillColor = Color.FromArgb(16, 18, 18);
			this.AimAssistFovSlider.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.AimAssistFovSlider.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.AimAssistFovSlider.HoverBarColor = Color.Firebrick;
			this.AimAssistFovSlider.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.AimAssistFovSlider.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.AimAssistFovSlider.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.AimAssistFovSlider.Location = new Point(10, 249);
			this.AimAssistFovSlider.Maximum = 180;
			this.AimAssistFovSlider.Minimum = 5;
			this.AimAssistFovSlider.Name = "AimAssistFovSlider";
			this.AimAssistFovSlider.Offset = 1f;
			this.AimAssistFovSlider.ShowText = false;
			this.AimAssistFovSlider.ShowValue = true;
			this.AimAssistFovSlider.Size = new Size(240, 20);
			this.AimAssistFovSlider.TabIndex = 914;
			this.AimAssistFovSlider.TargetLabel = this.lbfov;
			this.AimAssistFovSlider.Text = "beautyFlatSlider1";
			this.AimAssistFovSlider.Value = 60;
			this.AimAssistFovSlider.WriteInLabel = true;
			this.AimAssistFovSlider.Scroll += this.AimAssistFovSlider_Scroll;
			this.lbfov.AutoResize = false;
			this.lbfov.BackColor = Color.FromArgb(12, 14, 14);
			this.lbfov.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbfov.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbfov.Location = new Point(178, 225);
			this.lbfov.Name = "lbfov";
			this.lbfov.Size = new Size(72, 18);
			this.lbfov.TabIndex = 916;
			this.lbfov.Text = "60";
			this.lbfov.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbfov.TextPadding = new Padding(0);
			this.beautyLabel14.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel14.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel14.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel14.Location = new Point(10, 225);
			this.beautyLabel14.Name = "beautyLabel14";
			this.beautyLabel14.Size = new Size(95, 18);
			this.beautyLabel14.TabIndex = 915;
			this.beautyLabel14.Text = "Field of view:";
			this.beautyLabel14.TextPadding = new Padding(0);
			this.beautyLabel17.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel17.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel17.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel17.Location = new Point(10, 175);
			this.beautyLabel17.Name = "beautyLabel17";
			this.beautyLabel17.Size = new Size(69, 18);
			this.beautyLabel17.TabIndex = 909;
			this.beautyLabel17.Text = "Distance:";
			this.beautyLabel17.TextPadding = new Padding(0);
			this.AimAssistClickingOnly.AnimationSpeed = 0.6f;
			this.AimAssistClickingOnly.AnimationStep = 0.9999998f;
			this.AimAssistClickingOnly.BackColor = Color.FromArgb(12, 14, 14);
			this.AimAssistClickingOnly.BorderColor = Color.FromArgb(20, 22, 22);
			this.AimAssistClickingOnly.BorderRadius = 2f;
			this.AimAssistClickingOnly.BorderSize = 1f;
			this.AimAssistClickingOnly.Checked = true;
			this.AimAssistClickingOnly.CheckedBorderColor = Color.Firebrick;
			this.AimAssistClickingOnly.CheckedFillColor = Color.Firebrick;
			this.AimAssistClickingOnly.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.AimAssistClickingOnly.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.AimAssistClickingOnly.CheckMarkScale = 0.6f;
			this.AimAssistClickingOnly.FillColor = Color.FromArgb(16, 18, 18);
			this.AimAssistClickingOnly.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.AimAssistClickingOnly.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.AimAssistClickingOnly.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.AimAssistClickingOnly.Location = new Point(230, 284);
			this.AimAssistClickingOnly.Name = "AimAssistClickingOnly";
			this.AimAssistClickingOnly.Size = new Size(22, 22);
			this.AimAssistClickingOnly.TabIndex = 907;
			this.AimAssistClickingOnly.TargetLabel = this.beautyLabel10;
			this.AimAssistClickingOnly.Text = "beautyCheckBox4";
			this.AimAssistClickingOnly.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.AimAssistClickingOnly.CheckedChanged += this.AimAssistClickingOnly_CheckedChanged;
			this.beautyLabel10.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel10.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel10.ForeColor = Color.FromArgb(119, 119, 129);
			this.beautyLabel10.Location = new Point(10, 286);
			this.beautyLabel10.Name = "beautyLabel10";
			this.beautyLabel10.Size = new Size(94, 18);
			this.beautyLabel10.TabIndex = 908;
			this.beautyLabel10.Text = "Require click";
			this.beautyLabel10.TextPadding = new Padding(0);
			this.beautyPanel6.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel6.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel6.BorderSizeBottom = 1f;
			this.beautyPanel6.BorderSizeLeft = 1f;
			this.beautyPanel6.BorderSizeRight = 1f;
			this.beautyPanel6.BorderSizeTop = 1f;
			this.beautyPanel6.Controls.Add(this.beautyLabel12);
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
			this.beautyLabel12.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel12.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel12.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel12.Location = new Point(10, 11);
			this.beautyLabel12.Name = "beautyLabel12";
			this.beautyLabel12.Size = new Size(82, 18);
			this.beautyLabel12.TabIndex = 905;
			this.beautyLabel12.Text = "Aim Assist";
			this.beautyLabel12.TextPadding = new Padding(0);
			this.AimEnable.AutoRoundCorners = true;
			this.AimEnable.BackColor = Color.FromArgb(12, 14, 14);
			this.AimEnable.Checked = false;
			this.AimEnable.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.AimEnable.CheckedState.BorderRadius = 4;
			this.AimEnable.CheckedState.BorderThickness = 1;
			this.AimEnable.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.AimEnable.CheckedState.InnerBorderColor = Color.Firebrick;
			this.AimEnable.CheckedState.InnerBorderRadius = 4;
			this.AimEnable.CheckedState.InnerBorderThickness = 0;
			this.AimEnable.CheckedState.InnerColor = Color.Firebrick;
			this.AimEnable.CheckedState.InnerOffset = 2;
			this.AimEnable.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.AimEnable.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.AimEnable.LinkedLabel = this.beautyLabel13;
			this.AimEnable.Location = new Point(10, 50);
			this.AimEnable.Name = "AimEnable";
			this.AimEnable.Size = new Size(44, 22);
			this.AimEnable.TabIndex = 894;
			this.AimEnable.Text = "beautyToggleSwitch2";
			this.AimEnable.ThumbSize = 12;
			this.AimEnable.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.AimEnable.UncheckedState.BorderRadius = 4;
			this.AimEnable.UncheckedState.BorderThickness = 1;
			this.AimEnable.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.AimEnable.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.AimEnable.UncheckedState.InnerBorderRadius = 4;
			this.AimEnable.UncheckedState.InnerBorderThickness = 0;
			this.AimEnable.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.AimEnable.UncheckedState.InnerOffset = 30;
			this.AimEnable.CheckedChanged += this.AimEnable_CheckedChanged;
			this.beautyLabel13.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel13.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel13.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel13.Location = new Point(60, 53);
			this.beautyLabel13.Name = "beautyLabel13";
			this.beautyLabel13.Size = new Size(53, 18);
			this.beautyLabel13.TabIndex = 842;
			this.beautyLabel13.Text = "Enable";
			this.beautyLabel13.TextPadding = new Padding(0);
			this.AimAssistThroughWall.AnimationSpeed = 0.6f;
			this.AimAssistThroughWall.BackColor = Color.FromArgb(12, 14, 14);
			this.AimAssistThroughWall.BorderColor = Color.FromArgb(20, 22, 22);
			this.AimAssistThroughWall.BorderRadius = 2f;
			this.AimAssistThroughWall.BorderSize = 1f;
			this.AimAssistThroughWall.CheckedBorderColor = Color.Firebrick;
			this.AimAssistThroughWall.CheckedFillColor = Color.Firebrick;
			this.AimAssistThroughWall.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.AimAssistThroughWall.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.AimAssistThroughWall.CheckMarkScale = 0.6f;
			this.AimAssistThroughWall.FillColor = Color.FromArgb(16, 18, 18);
			this.AimAssistThroughWall.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.AimAssistThroughWall.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.AimAssistThroughWall.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.AimAssistThroughWall.Location = new Point(230, 312);
			this.AimAssistThroughWall.Name = "AimAssistThroughWall";
			this.AimAssistThroughWall.Size = new Size(22, 22);
			this.AimAssistThroughWall.TabIndex = 860;
			this.AimAssistThroughWall.TargetLabel = this.beautyLabel16;
			this.AimAssistThroughWall.Text = "beautyCheckBox3";
			this.AimAssistThroughWall.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.AimAssistThroughWall.CheckedChanged += this.AimAssistThroughWall_CheckedChanged;
			this.beautyLabel16.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel16.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel16.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel16.Location = new Point(10, 314);
			this.beautyLabel16.Name = "beautyLabel16";
			this.beautyLabel16.Size = new Size(89, 18);
			this.beautyLabel16.TabIndex = 861;
			this.beautyLabel16.Text = "ThroughWall";
			this.beautyLabel16.TextPadding = new Padding(0);
			this.beautyPanel9.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel9.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel9.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel9.BorderSizeBottom = 1f;
			this.beautyPanel9.BorderSizeLeft = 1f;
			this.beautyPanel9.BorderSizeRight = 1f;
			this.beautyPanel9.BorderSizeTop = 1f;
			this.beautyPanel9.Controls.Add(this.cbAttacking);
			this.beautyPanel9.Controls.Add(this.beautyLabel33);
			this.beautyPanel9.Controls.Add(this.cbTargeting);
			this.beautyPanel9.Controls.Add(this.beautyLabel32);
			this.beautyPanel9.Controls.Add(this.ticksvl);
			this.beautyPanel9.Controls.Add(this.beautyLabel19);
			this.beautyPanel9.Controls.Add(this.lbticks);
			this.beautyPanel9.Controls.Add(this.VelocityVrt);
			this.beautyPanel9.Controls.Add(this.beautyLabel9);
			this.beautyPanel9.Controls.Add(this.labelVelV);
			this.beautyPanel9.Controls.Add(this.VelocityBindButton);
			this.beautyPanel9.Controls.Add(this.VelocityHrz);
			this.beautyPanel9.Controls.Add(this.beautyLabel26);
			this.beautyPanel9.Controls.Add(this.labelVelH);
			this.beautyPanel9.Controls.Add(this.ChanceSlider);
			this.beautyPanel9.Controls.Add(this.beautyLabel28);
			this.beautyPanel9.Controls.Add(this.lbchancevl);
			this.beautyPanel9.Controls.Add(this.VelocityMovingOnly);
			this.beautyPanel9.Controls.Add(this.beautyLabel29);
			this.beautyPanel9.Controls.Add(this.beautyPanel10);
			this.beautyPanel9.Controls.Add(this.VelocityEnable);
			this.beautyPanel9.Controls.Add(this.beautyLabel31);
			this.beautyPanel9.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel9.FullHeight = 350;
			this.beautyPanel9.Location = new Point(281, 550);
			this.beautyPanel9.Name = "beautyPanel9";
			this.beautyPanel9.RadiusBottomLeft = 6f;
			this.beautyPanel9.RadiusBottomRight = 6f;
			this.beautyPanel9.RadiusTopLeft = 6f;
			this.beautyPanel9.RadiusTopRight = 6f;
			this.beautyPanel9.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel9.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel9.ScrollbarWidth = 4;
			this.beautyPanel9.Size = new Size(260, 369);
			this.beautyPanel9.TabIndex = 908;
			this.cbAttacking.AnimationSpeed = 0.6f;
			this.cbAttacking.BackColor = Color.FromArgb(12, 14, 14);
			this.cbAttacking.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbAttacking.BorderRadius = 2f;
			this.cbAttacking.BorderSize = 1f;
			this.cbAttacking.CheckedBorderColor = Color.Firebrick;
			this.cbAttacking.CheckedFillColor = Color.Firebrick;
			this.cbAttacking.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbAttacking.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbAttacking.CheckMarkScale = 0.6f;
			this.cbAttacking.FillColor = Color.FromArgb(16, 18, 18);
			this.cbAttacking.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbAttacking.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbAttacking.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbAttacking.Location = new Point(228, 336);
			this.cbAttacking.Name = "cbAttacking";
			this.cbAttacking.Size = new Size(22, 22);
			this.cbAttacking.TabIndex = 922;
			this.cbAttacking.TargetLabel = this.beautyLabel33;
			this.cbAttacking.Text = "beautyCheckBox3";
			this.cbAttacking.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbAttacking.CheckedChanged += this.cbAttacking_CheckedChanged;
			this.beautyLabel33.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel33.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel33.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel33.Location = new Point(10, 336);
			this.beautyLabel33.Name = "beautyLabel33";
			this.beautyLabel33.Size = new Size(102, 18);
			this.beautyLabel33.TabIndex = 923;
			this.beautyLabel33.Text = "Only attacking";
			this.beautyLabel33.TextPadding = new Padding(0);
			this.cbTargeting.AnimationSpeed = 0.6f;
			this.cbTargeting.BackColor = Color.FromArgb(12, 14, 14);
			this.cbTargeting.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbTargeting.BorderRadius = 2f;
			this.cbTargeting.BorderSize = 1f;
			this.cbTargeting.CheckedBorderColor = Color.Firebrick;
			this.cbTargeting.CheckedFillColor = Color.Firebrick;
			this.cbTargeting.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbTargeting.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbTargeting.CheckMarkScale = 0.6f;
			this.cbTargeting.FillColor = Color.FromArgb(16, 18, 18);
			this.cbTargeting.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbTargeting.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbTargeting.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbTargeting.Location = new Point(228, 308);
			this.cbTargeting.Name = "cbTargeting";
			this.cbTargeting.Size = new Size(22, 22);
			this.cbTargeting.TabIndex = 920;
			this.cbTargeting.TargetLabel = this.beautyLabel32;
			this.cbTargeting.Text = "beautyCheckBox3";
			this.cbTargeting.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbTargeting.CheckedChanged += this.cbTargeting_CheckedChanged;
			this.beautyLabel32.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel32.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel32.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel32.Location = new Point(10, 309);
			this.beautyLabel32.Name = "beautyLabel32";
			this.beautyLabel32.Size = new Size(101, 18);
			this.beautyLabel32.TabIndex = 921;
			this.beautyLabel32.Text = "Only targeting";
			this.beautyLabel32.TextPadding = new Padding(0);
			this.ticksvl.AnimationTrigger = 0;
			this.ticksvl.BackColor = Color.FromArgb(12, 14, 14);
			this.ticksvl.BarColor = Color.Firebrick;
			this.ticksvl.BorderColor = Color.FromArgb(20, 22, 22);
			this.ticksvl.BorderRadius = 2f;
			this.ticksvl.BorderSize = 1;
			this.ticksvl.FillColor = Color.FromArgb(16, 18, 18);
			this.ticksvl.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.ticksvl.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.ticksvl.HoverBarColor = Color.Firebrick;
			this.ticksvl.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.ticksvl.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ticksvl.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.ticksvl.Location = new Point(12, 248);
			this.ticksvl.Maximum = 10;
			this.ticksvl.Minimum = 0;
			this.ticksvl.Name = "ticksvl";
			this.ticksvl.Offset = 1f;
			this.ticksvl.ShowText = false;
			this.ticksvl.ShowValue = false;
			this.ticksvl.Size = new Size(240, 20);
			this.ticksvl.TabIndex = 917;
			this.ticksvl.TargetLabel = this.lbticks;
			this.ticksvl.Text = "beautyFlatSlider1";
			this.ticksvl.Value = 1;
			this.ticksvl.WriteInLabel = true;
			this.ticksvl.Scroll += this.ticksvl_Scroll;
			this.lbticks.AutoResize = false;
			this.lbticks.BackColor = Color.FromArgb(12, 14, 14);
			this.lbticks.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbticks.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbticks.Location = new Point(180, 227);
			this.lbticks.Name = "lbticks";
			this.lbticks.Size = new Size(72, 18);
			this.lbticks.TabIndex = 919;
			this.lbticks.Text = "1 ms";
			this.lbticks.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbticks.TextPadding = new Padding(0);
			this.beautyLabel19.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel19.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel19.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel19.Location = new Point(11, 227);
			this.beautyLabel19.Name = "beautyLabel19";
			this.beautyLabel19.Size = new Size(45, 18);
			this.beautyLabel19.TabIndex = 918;
			this.beautyLabel19.Text = "Ticks:";
			this.beautyLabel19.TextPadding = new Padding(0);
			this.VelocityVrt.AnimationTrigger = 0;
			this.VelocityVrt.BackColor = Color.FromArgb(12, 14, 14);
			this.VelocityVrt.BarColor = Color.Firebrick;
			this.VelocityVrt.BorderColor = Color.FromArgb(20, 22, 22);
			this.VelocityVrt.BorderRadius = 2f;
			this.VelocityVrt.BorderSize = 1;
			this.VelocityVrt.FillColor = Color.FromArgb(16, 18, 18);
			this.VelocityVrt.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.VelocityVrt.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.VelocityVrt.HoverBarColor = Color.Firebrick;
			this.VelocityVrt.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.VelocityVrt.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.VelocityVrt.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.VelocityVrt.Location = new Point(10, 152);
			this.VelocityVrt.Maximum = 1000;
			this.VelocityVrt.Minimum = 0;
			this.VelocityVrt.Name = "VelocityVrt";
			this.VelocityVrt.Offset = 1f;
			this.VelocityVrt.ShowText = false;
			this.VelocityVrt.ShowValue = false;
			this.VelocityVrt.Size = new Size(240, 20);
			this.VelocityVrt.TabIndex = 914;
			this.VelocityVrt.TargetLabel = this.labelVelV;
			this.VelocityVrt.Text = "beautyFlatSlider1";
			this.VelocityVrt.Value = 1000;
			this.VelocityVrt.WriteInLabel = true;
			this.VelocityVrt.Scroll += this.VelocityVrt_Scroll;
			this.labelVelV.AutoResize = false;
			this.labelVelV.BackColor = Color.FromArgb(12, 14, 14);
			this.labelVelV.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelVelV.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelVelV.Location = new Point(178, 128);
			this.labelVelV.Name = "labelVelV";
			this.labelVelV.Size = new Size(72, 18);
			this.labelVelV.TabIndex = 916;
			this.labelVelV.Text = "100.0";
			this.labelVelV.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelVelV.TextPadding = new Padding(0);
			this.beautyLabel9.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel9.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel9.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel9.Location = new Point(10, 128);
			this.beautyLabel9.Name = "beautyLabel9";
			this.beautyLabel9.Size = new Size(61, 18);
			this.beautyLabel9.TabIndex = 915;
			this.beautyLabel9.Text = "Vertical:";
			this.beautyLabel9.TextPadding = new Padding(0);
			this.VelocityBindButton.AnimationSpeed = 0.6f;
			this.VelocityBindButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.VelocityBindButton.BorderRadius = 4f;
			this.VelocityBindButton.BorderSize = 1f;
			this.VelocityBindButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.VelocityBindButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.VelocityBindButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.VelocityBindButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.VelocityBindButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.VelocityBindButton.FillColor = Color.FromArgb(16, 18, 18);
			this.VelocityBindButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.VelocityBindButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.VelocityBindButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.VelocityBindButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.VelocityBindButton.ImageOffset = new Point(0, 0);
			this.VelocityBindButton.Location = new Point(173, 50);
			this.VelocityBindButton.MinimumSize = new Size(20, 22);
			this.VelocityBindButton.MinimumTextWidth = 20;
			this.VelocityBindButton.Name = "VelocityBindButton";
			this.VelocityBindButton.Size = new Size(77, 22);
			this.VelocityBindButton.TabIndex = 913;
			this.VelocityBindButton.Text = "None";
			this.VelocityBindButton.TextOffset = new Point(0, 0);
			this.VelocityBindButton.TextPadding = new Padding(0);
			this.VelocityBindButton.YOffSet = 0;
			this.VelocityBindButton.MouseDown += this.BindButtons_MouseDown;
			this.VelocityHrz.AnimationTrigger = 0;
			this.VelocityHrz.BackColor = Color.FromArgb(12, 14, 14);
			this.VelocityHrz.BarColor = Color.Firebrick;
			this.VelocityHrz.BorderColor = Color.FromArgb(20, 22, 22);
			this.VelocityHrz.BorderRadius = 2f;
			this.VelocityHrz.BorderSize = 1;
			this.VelocityHrz.FillColor = Color.FromArgb(16, 18, 18);
			this.VelocityHrz.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.VelocityHrz.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.VelocityHrz.HoverBarColor = Color.Firebrick;
			this.VelocityHrz.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.VelocityHrz.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.VelocityHrz.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.VelocityHrz.Location = new Point(10, 102);
			this.VelocityHrz.Maximum = 1000;
			this.VelocityHrz.Minimum = 0;
			this.VelocityHrz.Name = "VelocityHrz";
			this.VelocityHrz.Offset = 1f;
			this.VelocityHrz.ShowText = false;
			this.VelocityHrz.ShowValue = false;
			this.VelocityHrz.Size = new Size(240, 20);
			this.VelocityHrz.TabIndex = 910;
			this.VelocityHrz.TargetLabel = this.labelVelH;
			this.VelocityHrz.Text = "beautyFlatSlider1";
			this.VelocityHrz.Value = 900;
			this.VelocityHrz.WriteInLabel = true;
			this.VelocityHrz.Scroll += this.VelocityHrz_Scroll;
			this.labelVelH.AutoResize = false;
			this.labelVelH.BackColor = Color.FromArgb(12, 14, 14);
			this.labelVelH.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelVelH.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelVelH.Location = new Point(178, 78);
			this.labelVelH.Name = "labelVelH";
			this.labelVelH.Size = new Size(72, 18);
			this.labelVelH.TabIndex = 912;
			this.labelVelH.Text = "90.0";
			this.labelVelH.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelVelH.TextPadding = new Padding(0);
			this.beautyLabel26.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel26.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel26.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel26.Location = new Point(10, 78);
			this.beautyLabel26.Name = "beautyLabel26";
			this.beautyLabel26.Size = new Size(79, 18);
			this.beautyLabel26.TabIndex = 911;
			this.beautyLabel26.Text = "Horizontal:";
			this.beautyLabel26.TextPadding = new Padding(0);
			this.ChanceSlider.AnimationTrigger = 0;
			this.ChanceSlider.BackColor = Color.FromArgb(12, 14, 14);
			this.ChanceSlider.BarColor = Color.Firebrick;
			this.ChanceSlider.BorderColor = Color.FromArgb(20, 22, 22);
			this.ChanceSlider.BorderRadius = 2f;
			this.ChanceSlider.BorderSize = 1;
			this.ChanceSlider.FillColor = Color.FromArgb(16, 18, 18);
			this.ChanceSlider.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.ChanceSlider.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.ChanceSlider.HoverBarColor = Color.Firebrick;
			this.ChanceSlider.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.ChanceSlider.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ChanceSlider.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.ChanceSlider.Location = new Point(11, 201);
			this.ChanceSlider.Maximum = 100;
			this.ChanceSlider.Minimum = 0;
			this.ChanceSlider.Name = "ChanceSlider";
			this.ChanceSlider.Offset = 1f;
			this.ChanceSlider.ShowText = false;
			this.ChanceSlider.ShowValue = false;
			this.ChanceSlider.Size = new Size(240, 20);
			this.ChanceSlider.TabIndex = 907;
			this.ChanceSlider.TargetLabel = this.lbchancevl;
			this.ChanceSlider.Text = "beautyFlatSlider1";
			this.ChanceSlider.Value = 100;
			this.ChanceSlider.WriteInLabel = true;
			this.ChanceSlider.Scroll += this.ChanceSlider_Scroll;
			this.lbchancevl.AutoResize = false;
			this.lbchancevl.BackColor = Color.FromArgb(12, 14, 14);
			this.lbchancevl.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbchancevl.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbchancevl.Location = new Point(179, 180);
			this.lbchancevl.Name = "lbchancevl";
			this.lbchancevl.Size = new Size(72, 18);
			this.lbchancevl.TabIndex = 909;
			this.lbchancevl.Text = "100%";
			this.lbchancevl.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbchancevl.TextPadding = new Padding(0);
			this.beautyLabel28.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel28.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel28.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel28.Location = new Point(10, 180);
			this.beautyLabel28.Name = "beautyLabel28";
			this.beautyLabel28.Size = new Size(59, 18);
			this.beautyLabel28.TabIndex = 908;
			this.beautyLabel28.Text = "Chance:";
			this.beautyLabel28.TextPadding = new Padding(0);
			this.VelocityMovingOnly.AnimationSpeed = 0.6f;
			this.VelocityMovingOnly.BackColor = Color.FromArgb(12, 14, 14);
			this.VelocityMovingOnly.BorderColor = Color.FromArgb(20, 22, 22);
			this.VelocityMovingOnly.BorderRadius = 2f;
			this.VelocityMovingOnly.BorderSize = 1f;
			this.VelocityMovingOnly.CheckedBorderColor = Color.Firebrick;
			this.VelocityMovingOnly.CheckedFillColor = Color.Firebrick;
			this.VelocityMovingOnly.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.VelocityMovingOnly.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.VelocityMovingOnly.CheckMarkScale = 0.6f;
			this.VelocityMovingOnly.FillColor = Color.FromArgb(16, 18, 18);
			this.VelocityMovingOnly.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.VelocityMovingOnly.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.VelocityMovingOnly.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.VelocityMovingOnly.Location = new Point(228, 280);
			this.VelocityMovingOnly.Name = "VelocityMovingOnly";
			this.VelocityMovingOnly.Size = new Size(22, 22);
			this.VelocityMovingOnly.TabIndex = 905;
			this.VelocityMovingOnly.TargetLabel = this.beautyLabel29;
			this.VelocityMovingOnly.Text = "beautyCheckBox3";
			this.VelocityMovingOnly.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.VelocityMovingOnly.CheckedChanged += this.VelocityMovingOnly_CheckedChanged;
			this.beautyLabel29.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel29.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel29.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel29.Location = new Point(10, 283);
			this.beautyLabel29.Name = "beautyLabel29";
			this.beautyLabel29.Size = new Size(87, 18);
			this.beautyLabel29.TabIndex = 906;
			this.beautyLabel29.Text = "Moving only";
			this.beautyLabel29.TextPadding = new Padding(0);
			this.beautyPanel10.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel10.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel10.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel10.BorderSizeBottom = 1f;
			this.beautyPanel10.BorderSizeLeft = 1f;
			this.beautyPanel10.BorderSizeRight = 1f;
			this.beautyPanel10.BorderSizeTop = 1f;
			this.beautyPanel10.Controls.Add(this.beautyLabel30);
			this.beautyPanel10.Dock = DockStyle.Top;
			this.beautyPanel10.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel10.FullHeight = 350;
			this.beautyPanel10.Location = new Point(0, 0);
			this.beautyPanel10.Name = "beautyPanel10";
			this.beautyPanel10.RadiusBottomLeft = 0f;
			this.beautyPanel10.RadiusBottomRight = 0f;
			this.beautyPanel10.RadiusTopLeft = 6f;
			this.beautyPanel10.RadiusTopRight = 6f;
			this.beautyPanel10.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel10.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel10.ScrollbarWidth = 4;
			this.beautyPanel10.Size = new Size(260, 40);
			this.beautyPanel10.TabIndex = 904;
			this.beautyLabel30.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel30.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel30.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel30.Location = new Point(10, 11);
			this.beautyLabel30.Name = "beautyLabel30";
			this.beautyLabel30.Size = new Size(59, 18);
			this.beautyLabel30.TabIndex = 905;
			this.beautyLabel30.Text = "Velocity";
			this.beautyLabel30.TextPadding = new Padding(0);
			this.VelocityEnable.AutoRoundCorners = true;
			this.VelocityEnable.BackColor = Color.FromArgb(12, 14, 14);
			this.VelocityEnable.Checked = false;
			this.VelocityEnable.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.VelocityEnable.CheckedState.BorderRadius = 4;
			this.VelocityEnable.CheckedState.BorderThickness = 1;
			this.VelocityEnable.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.VelocityEnable.CheckedState.InnerBorderColor = Color.Firebrick;
			this.VelocityEnable.CheckedState.InnerBorderRadius = 4;
			this.VelocityEnable.CheckedState.InnerBorderThickness = 0;
			this.VelocityEnable.CheckedState.InnerColor = Color.Firebrick;
			this.VelocityEnable.CheckedState.InnerOffset = 2;
			this.VelocityEnable.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.VelocityEnable.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.VelocityEnable.LinkedLabel = this.beautyLabel31;
			this.VelocityEnable.Location = new Point(10, 50);
			this.VelocityEnable.Name = "VelocityEnable";
			this.VelocityEnable.Size = new Size(44, 22);
			this.VelocityEnable.TabIndex = 894;
			this.VelocityEnable.Text = "beautyToggleSwitch4";
			this.VelocityEnable.ThumbSize = 12;
			this.VelocityEnable.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.VelocityEnable.UncheckedState.BorderRadius = 4;
			this.VelocityEnable.UncheckedState.BorderThickness = 1;
			this.VelocityEnable.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.VelocityEnable.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.VelocityEnable.UncheckedState.InnerBorderRadius = 4;
			this.VelocityEnable.UncheckedState.InnerBorderThickness = 0;
			this.VelocityEnable.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.VelocityEnable.UncheckedState.InnerOffset = 30;
			this.VelocityEnable.CheckedChanged += this.VelocityEnable_CheckedChanged;
			this.beautyLabel31.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel31.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel31.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel31.Location = new Point(60, 53);
			this.beautyLabel31.Name = "beautyLabel31";
			this.beautyLabel31.Size = new Size(53, 18);
			this.beautyLabel31.TabIndex = 842;
			this.beautyLabel31.Text = "Enable";
			this.beautyLabel31.TextPadding = new Padding(0);
			this.beautyPanel7.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel7.BorderSizeBottom = 1f;
			this.beautyPanel7.BorderSizeLeft = 1f;
			this.beautyPanel7.BorderSizeRight = 1f;
			this.beautyPanel7.BorderSizeTop = 1f;
			this.beautyPanel7.Controls.Add(this.cbWallCheck);
			this.beautyPanel7.Controls.Add(this.beautyLabel27);
			this.beautyPanel7.Controls.Add(this.ReachBindButton);
			this.beautyPanel7.Controls.Add(this.DistanceSlider);
			this.beautyPanel7.Controls.Add(this.beautyLabel22);
			this.beautyPanel7.Controls.Add(this.labelReach);
			this.beautyPanel7.Controls.Add(this.HitBoxSl);
			this.beautyPanel7.Controls.Add(this.beautyLabel23);
			this.beautyPanel7.Controls.Add(this.labelHitbox);
			this.beautyPanel7.Controls.Add(this.beautyPanel8);
			this.beautyPanel7.Controls.Add(this.ReachEnable);
			this.beautyPanel7.Controls.Add(this.cbReachWeapon);
			this.beautyPanel7.Controls.Add(this.beautyLabel24);
			this.beautyPanel7.Controls.Add(this.beautyLabel21);
			this.beautyPanel7.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.FullHeight = 350;
			this.beautyPanel7.Location = new Point(11, 281);
			this.beautyPanel7.Name = "beautyPanel7";
			this.beautyPanel7.RadiusBottomLeft = 6f;
			this.beautyPanel7.RadiusBottomRight = 6f;
			this.beautyPanel7.RadiusTopLeft = 6f;
			this.beautyPanel7.RadiusTopRight = 6f;
			this.beautyPanel7.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel7.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel7.ScrollbarWidth = 4;
			this.beautyPanel7.Size = new Size(260, 243);
			this.beautyPanel7.TabIndex = 905;
			this.cbWallCheck.AnimationSpeed = 0.6f;
			this.cbWallCheck.AnimationStep = 6E-45f;
			this.cbWallCheck.BackColor = Color.FromArgb(12, 14, 14);
			this.cbWallCheck.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbWallCheck.BorderRadius = 2f;
			this.cbWallCheck.BorderSize = 1f;
			this.cbWallCheck.CheckedBorderColor = Color.Firebrick;
			this.cbWallCheck.CheckedFillColor = Color.Firebrick;
			this.cbWallCheck.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbWallCheck.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbWallCheck.CheckMarkScale = 0.6f;
			this.cbWallCheck.FillColor = Color.FromArgb(16, 18, 18);
			this.cbWallCheck.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbWallCheck.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbWallCheck.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbWallCheck.Location = new Point(228, 210);
			this.cbWallCheck.Name = "cbWallCheck";
			this.cbWallCheck.Size = new Size(22, 22);
			this.cbWallCheck.TabIndex = 914;
			this.cbWallCheck.TargetLabel = this.beautyLabel27;
			this.cbWallCheck.Text = "beautyCheckBox3";
			this.cbWallCheck.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbWallCheck.CheckedChanged += this.cbWallCheck_CheckedChanged;
			this.beautyLabel27.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel27.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel27.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel27.Location = new Point(10, 210);
			this.beautyLabel27.Name = "beautyLabel27";
			this.beautyLabel27.Size = new Size(89, 18);
			this.beautyLabel27.TabIndex = 915;
			this.beautyLabel27.Text = "ThroughWall";
			this.beautyLabel27.TextPadding = new Padding(0);
			this.ReachBindButton.AnimationSpeed = 0.6f;
			this.ReachBindButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.ReachBindButton.BorderRadius = 4f;
			this.ReachBindButton.BorderSize = 1f;
			this.ReachBindButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.ReachBindButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.ReachBindButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.ReachBindButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.ReachBindButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.ReachBindButton.FillColor = Color.FromArgb(16, 18, 18);
			this.ReachBindButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.ReachBindButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.ReachBindButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ReachBindButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ReachBindButton.ImageOffset = new Point(0, 0);
			this.ReachBindButton.Location = new Point(173, 50);
			this.ReachBindButton.MinimumSize = new Size(20, 22);
			this.ReachBindButton.MinimumTextWidth = 20;
			this.ReachBindButton.Name = "ReachBindButton";
			this.ReachBindButton.Size = new Size(77, 22);
			this.ReachBindButton.TabIndex = 913;
			this.ReachBindButton.Text = "None";
			this.ReachBindButton.TextOffset = new Point(0, 0);
			this.ReachBindButton.TextPadding = new Padding(0);
			this.ReachBindButton.YOffSet = 0;
			this.ReachBindButton.MouseDown += this.BindButtons_MouseDown;
			this.DistanceSlider.AnimationTrigger = 0;
			this.DistanceSlider.BackColor = Color.FromArgb(12, 14, 14);
			this.DistanceSlider.BarColor = Color.Firebrick;
			this.DistanceSlider.BorderColor = Color.FromArgb(20, 22, 22);
			this.DistanceSlider.BorderRadius = 2f;
			this.DistanceSlider.BorderSize = 1;
			this.DistanceSlider.FillColor = Color.FromArgb(16, 18, 18);
			this.DistanceSlider.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.DistanceSlider.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.DistanceSlider.HoverBarColor = Color.Firebrick;
			this.DistanceSlider.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.DistanceSlider.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.DistanceSlider.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.DistanceSlider.Location = new Point(10, 102);
			this.DistanceSlider.Maximum = 600;
			this.DistanceSlider.Minimum = 300;
			this.DistanceSlider.Name = "DistanceSlider";
			this.DistanceSlider.Offset = 1f;
			this.DistanceSlider.ShowText = false;
			this.DistanceSlider.ShowValue = false;
			this.DistanceSlider.Size = new Size(240, 20);
			this.DistanceSlider.TabIndex = 910;
			this.DistanceSlider.TargetLabel = this.labelReach;
			this.DistanceSlider.Text = "beautyFlatSlider1";
			this.DistanceSlider.Value = 300;
			this.DistanceSlider.WriteInLabel = true;
			this.DistanceSlider.Scroll += this.DistanceSlider_Scroll;
			this.labelReach.AutoResize = false;
			this.labelReach.BackColor = Color.FromArgb(12, 14, 14);
			this.labelReach.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelReach.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelReach.Location = new Point(178, 78);
			this.labelReach.Name = "labelReach";
			this.labelReach.Size = new Size(72, 18);
			this.labelReach.TabIndex = 912;
			this.labelReach.Text = "3.00";
			this.labelReach.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelReach.TextPadding = new Padding(0);
			this.beautyLabel22.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel22.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel22.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel22.Location = new Point(10, 78);
			this.beautyLabel22.Name = "beautyLabel22";
			this.beautyLabel22.Size = new Size(69, 18);
			this.beautyLabel22.TabIndex = 911;
			this.beautyLabel22.Text = "Distance:";
			this.beautyLabel22.TextPadding = new Padding(0);
			this.HitBoxSl.AnimationTrigger = 0;
			this.HitBoxSl.BackColor = Color.FromArgb(12, 14, 14);
			this.HitBoxSl.BarColor = Color.Firebrick;
			this.HitBoxSl.BorderColor = Color.FromArgb(20, 22, 22);
			this.HitBoxSl.BorderRadius = 2f;
			this.HitBoxSl.BorderSize = 1;
			this.HitBoxSl.FillColor = Color.FromArgb(16, 18, 18);
			this.HitBoxSl.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.HitBoxSl.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.HitBoxSl.HoverBarColor = Color.Firebrick;
			this.HitBoxSl.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.HitBoxSl.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.HitBoxSl.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.HitBoxSl.Location = new Point(10, 152);
			this.HitBoxSl.Maximum = 100;
			this.HitBoxSl.Minimum = 0;
			this.HitBoxSl.Name = "HitBoxSl";
			this.HitBoxSl.Offset = 1f;
			this.HitBoxSl.ShowText = false;
			this.HitBoxSl.ShowValue = false;
			this.HitBoxSl.Size = new Size(240, 20);
			this.HitBoxSl.TabIndex = 907;
			this.HitBoxSl.TargetLabel = this.labelHitbox;
			this.HitBoxSl.Text = "beautyFlatSlider1";
			this.HitBoxSl.Value = 0;
			this.HitBoxSl.WriteInLabel = true;
			this.HitBoxSl.Scroll += this.HitBoxSl_Scroll;
			this.labelHitbox.AutoResize = false;
			this.labelHitbox.BackColor = Color.FromArgb(12, 14, 14);
			this.labelHitbox.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelHitbox.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelHitbox.Location = new Point(178, 128);
			this.labelHitbox.Name = "labelHitbox";
			this.labelHitbox.Size = new Size(72, 18);
			this.labelHitbox.TabIndex = 909;
			this.labelHitbox.Text = "0.0";
			this.labelHitbox.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelHitbox.TextPadding = new Padding(0);
			this.beautyLabel23.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel23.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel23.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel23.Location = new Point(10, 128);
			this.beautyLabel23.Name = "beautyLabel23";
			this.beautyLabel23.Size = new Size(56, 18);
			this.beautyLabel23.TabIndex = 908;
			this.beautyLabel23.Text = "HitBox:";
			this.beautyLabel23.TextPadding = new Padding(0);
			this.beautyPanel8.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel8.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel8.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel8.BorderSizeBottom = 1f;
			this.beautyPanel8.BorderSizeLeft = 1f;
			this.beautyPanel8.BorderSizeRight = 1f;
			this.beautyPanel8.BorderSizeTop = 1f;
			this.beautyPanel8.Controls.Add(this.beautyLabel20);
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
			this.beautyLabel20.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel20.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel20.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel20.Location = new Point(10, 11);
			this.beautyLabel20.Name = "beautyLabel20";
			this.beautyLabel20.Size = new Size(49, 18);
			this.beautyLabel20.TabIndex = 905;
			this.beautyLabel20.Text = "Reach";
			this.beautyLabel20.TextPadding = new Padding(0);
			this.ReachEnable.AutoRoundCorners = true;
			this.ReachEnable.BackColor = Color.FromArgb(12, 14, 14);
			this.ReachEnable.Checked = false;
			this.ReachEnable.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.ReachEnable.CheckedState.BorderRadius = 4;
			this.ReachEnable.CheckedState.BorderThickness = 1;
			this.ReachEnable.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.ReachEnable.CheckedState.InnerBorderColor = Color.Firebrick;
			this.ReachEnable.CheckedState.InnerBorderRadius = 4;
			this.ReachEnable.CheckedState.InnerBorderThickness = 0;
			this.ReachEnable.CheckedState.InnerColor = Color.Firebrick;
			this.ReachEnable.CheckedState.InnerOffset = 2;
			this.ReachEnable.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.ReachEnable.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.ReachEnable.LinkedLabel = this.beautyLabel21;
			this.ReachEnable.Location = new Point(10, 50);
			this.ReachEnable.Name = "ReachEnable";
			this.ReachEnable.Size = new Size(44, 22);
			this.ReachEnable.TabIndex = 894;
			this.ReachEnable.Text = "beautyToggleSwitch3";
			this.ReachEnable.ThumbSize = 12;
			this.ReachEnable.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.ReachEnable.UncheckedState.BorderRadius = 4;
			this.ReachEnable.UncheckedState.BorderThickness = 1;
			this.ReachEnable.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.ReachEnable.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.ReachEnable.UncheckedState.InnerBorderRadius = 4;
			this.ReachEnable.UncheckedState.InnerBorderThickness = 0;
			this.ReachEnable.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.ReachEnable.UncheckedState.InnerOffset = 30;
			this.ReachEnable.CheckedChanged += this.ReachEnable_CheckedChanged;
			this.beautyLabel21.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel21.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel21.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel21.Location = new Point(60, 53);
			this.beautyLabel21.Name = "beautyLabel21";
			this.beautyLabel21.Size = new Size(53, 18);
			this.beautyLabel21.TabIndex = 842;
			this.beautyLabel21.Text = "Enable";
			this.beautyLabel21.TextPadding = new Padding(0);
			this.cbReachWeapon.AnimationSpeed = 0.6f;
			this.cbReachWeapon.BackColor = Color.FromArgb(12, 14, 14);
			this.cbReachWeapon.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbReachWeapon.BorderRadius = 2f;
			this.cbReachWeapon.BorderSize = 1f;
			this.cbReachWeapon.CheckedBorderColor = Color.Firebrick;
			this.cbReachWeapon.CheckedFillColor = Color.Firebrick;
			this.cbReachWeapon.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbReachWeapon.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbReachWeapon.CheckMarkScale = 0.6f;
			this.cbReachWeapon.FillColor = Color.FromArgb(16, 18, 18);
			this.cbReachWeapon.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbReachWeapon.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbReachWeapon.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbReachWeapon.Location = new Point(228, 182);
			this.cbReachWeapon.Name = "cbReachWeapon";
			this.cbReachWeapon.Size = new Size(22, 22);
			this.cbReachWeapon.TabIndex = 860;
			this.cbReachWeapon.TargetLabel = this.beautyLabel24;
			this.cbReachWeapon.Text = "beautyCheckBox3";
			this.cbReachWeapon.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbReachWeapon.CheckedChanged += this.cbReachWeapon_CheckedChanged;
			this.beautyLabel24.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel24.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel24.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel24.Location = new Point(10, 184);
			this.beautyLabel24.Name = "beautyLabel24";
			this.beautyLabel24.Size = new Size(126, 18);
			this.beautyLabel24.TabIndex = 861;
			this.beautyLabel24.Text = "Only with Weapon";
			this.beautyLabel24.TextPadding = new Padding(0);
			this.beautyPanel3.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel3.BorderSizeBottom = 1f;
			this.beautyPanel3.BorderSizeLeft = 1f;
			this.beautyPanel3.BorderSizeRight = 1f;
			this.beautyPanel3.BorderSizeTop = 1f;
			this.beautyPanel3.Controls.Add(this.cbInventory);
			this.beautyPanel3.Controls.Add(this.beautyLabel34);
			this.beautyPanel3.Controls.Add(this.ClickerBindButton);
			this.beautyPanel3.Controls.Add(this.Randomize);
			this.beautyPanel3.Controls.Add(this.beautyLabel5);
			this.beautyPanel3.Controls.Add(this.Weapon);
			this.beautyPanel3.Controls.Add(this.beautyLabel2);
			this.beautyPanel3.Controls.Add(this.beautyPanel4);
			this.beautyPanel3.Controls.Add(this.ClickerEnable);
			this.beautyPanel3.Controls.Add(this.CPS_Slider);
			this.beautyPanel3.Controls.Add(this.beautyLabel6);
			this.beautyPanel3.Controls.Add(this.labelCPS);
			this.beautyPanel3.Controls.Add(this.cbBreak);
			this.beautyPanel3.Controls.Add(this.beautyLabel7);
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
			this.beautyPanel3.Size = new Size(260, 254);
			this.beautyPanel3.TabIndex = 903;
			this.cbInventory.AnimationSpeed = 0.6f;
			this.cbInventory.BackColor = Color.FromArgb(12, 14, 14);
			this.cbInventory.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbInventory.BorderRadius = 2f;
			this.cbInventory.BorderSize = 1f;
			this.cbInventory.CheckedBorderColor = Color.Firebrick;
			this.cbInventory.CheckedFillColor = Color.Firebrick;
			this.cbInventory.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbInventory.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbInventory.CheckMarkScale = 0.6f;
			this.cbInventory.FillColor = Color.FromArgb(16, 18, 18);
			this.cbInventory.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbInventory.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbInventory.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbInventory.Location = new Point(228, 216);
			this.cbInventory.Name = "cbInventory";
			this.cbInventory.Size = new Size(22, 22);
			this.cbInventory.TabIndex = 910;
			this.cbInventory.TargetLabel = this.beautyLabel34;
			this.cbInventory.Text = "beautyCheckBox3";
			this.cbInventory.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbInventory.CheckedChanged += this.cbInventory_CheckedChanged;
			this.beautyLabel34.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel34.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel34.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel34.Location = new Point(10, 216);
			this.beautyLabel34.Name = "beautyLabel34";
			this.beautyLabel34.Size = new Size(70, 18);
			this.beautyLabel34.TabIndex = 911;
			this.beautyLabel34.Text = "Inventory";
			this.beautyLabel34.TextPadding = new Padding(0);
			this.ClickerBindButton.AnimationSpeed = 0.6f;
			this.ClickerBindButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.ClickerBindButton.BorderRadius = 4f;
			this.ClickerBindButton.BorderSize = 1f;
			this.ClickerBindButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.ClickerBindButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.ClickerBindButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.ClickerBindButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.ClickerBindButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.ClickerBindButton.FillColor = Color.FromArgb(16, 18, 18);
			this.ClickerBindButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.ClickerBindButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.ClickerBindButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ClickerBindButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ClickerBindButton.ImageOffset = new Point(0, 0);
			this.ClickerBindButton.Location = new Point(173, 50);
			this.ClickerBindButton.MinimumSize = new Size(20, 22);
			this.ClickerBindButton.MinimumTextWidth = 20;
			this.ClickerBindButton.Name = "ClickerBindButton";
			this.ClickerBindButton.Size = new Size(77, 22);
			this.ClickerBindButton.TabIndex = 909;
			this.ClickerBindButton.Text = "None";
			this.ClickerBindButton.TextOffset = new Point(0, 0);
			this.ClickerBindButton.TextPadding = new Padding(0);
			this.ClickerBindButton.YOffSet = 0;
			this.ClickerBindButton.MouseDown += this.BindButtons_MouseDown;
			this.Randomize.AnimationSpeed = 0.6f;
			this.Randomize.BackColor = Color.FromArgb(12, 14, 14);
			this.Randomize.BorderColor = Color.FromArgb(20, 22, 22);
			this.Randomize.BorderRadius = 2f;
			this.Randomize.BorderSize = 1f;
			this.Randomize.CheckedBorderColor = Color.Firebrick;
			this.Randomize.CheckedFillColor = Color.Firebrick;
			this.Randomize.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.Randomize.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.Randomize.CheckMarkScale = 0.6f;
			this.Randomize.FillColor = Color.FromArgb(16, 18, 18);
			this.Randomize.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.Randomize.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.Randomize.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.Randomize.Location = new Point(228, 132);
			this.Randomize.Name = "Randomize";
			this.Randomize.Size = new Size(22, 22);
			this.Randomize.TabIndex = 907;
			this.Randomize.TargetLabel = this.beautyLabel5;
			this.Randomize.Text = "beautyCheckBox3";
			this.Randomize.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.Randomize.CheckedChanged += this.Inventory_CheckedChanged;
			this.beautyLabel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel5.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel5.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel5.Location = new Point(10, 134);
			this.beautyLabel5.Name = "beautyLabel5";
			this.beautyLabel5.Size = new Size(88, 18);
			this.beautyLabel5.TabIndex = 908;
			this.beautyLabel5.Text = "Randomizer";
			this.beautyLabel5.TextPadding = new Padding(0);
			this.Weapon.AnimationSpeed = 0.6f;
			this.Weapon.BackColor = Color.FromArgb(12, 14, 14);
			this.Weapon.BorderColor = Color.FromArgb(20, 22, 22);
			this.Weapon.BorderRadius = 2f;
			this.Weapon.BorderSize = 1f;
			this.Weapon.CheckedBorderColor = Color.Firebrick;
			this.Weapon.CheckedFillColor = Color.Firebrick;
			this.Weapon.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.Weapon.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.Weapon.CheckMarkScale = 0.6f;
			this.Weapon.FillColor = Color.FromArgb(16, 18, 18);
			this.Weapon.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.Weapon.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.Weapon.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.Weapon.Location = new Point(228, 188);
			this.Weapon.Name = "Weapon";
			this.Weapon.Size = new Size(22, 22);
			this.Weapon.TabIndex = 905;
			this.Weapon.TargetLabel = this.beautyLabel2;
			this.Weapon.Text = "beautyCheckBox3";
			this.Weapon.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.Weapon.CheckedChanged += this.Weapon_CheckedChanged;
			this.beautyLabel2.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel2.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel2.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel2.Location = new Point(10, 190);
			this.beautyLabel2.Name = "beautyLabel2";
			this.beautyLabel2.Size = new Size(126, 18);
			this.beautyLabel2.TabIndex = 906;
			this.beautyLabel2.Text = "Only with Weapon";
			this.beautyLabel2.TextPadding = new Padding(0);
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
			this.beautyLabel11.Size = new Size(89, 18);
			this.beautyLabel11.TabIndex = 905;
			this.beautyLabel11.Text = "Auto Clicker";
			this.beautyLabel11.TextPadding = new Padding(0);
			this.ClickerEnable.AutoRoundCorners = true;
			this.ClickerEnable.BackColor = Color.FromArgb(12, 14, 14);
			this.ClickerEnable.Checked = false;
			this.ClickerEnable.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.ClickerEnable.CheckedState.BorderRadius = 4;
			this.ClickerEnable.CheckedState.BorderThickness = 1;
			this.ClickerEnable.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.ClickerEnable.CheckedState.InnerBorderColor = Color.Firebrick;
			this.ClickerEnable.CheckedState.InnerBorderRadius = 4;
			this.ClickerEnable.CheckedState.InnerBorderThickness = 0;
			this.ClickerEnable.CheckedState.InnerColor = Color.Firebrick;
			this.ClickerEnable.CheckedState.InnerOffset = 2;
			this.ClickerEnable.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.ClickerEnable.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.ClickerEnable.LinkedLabel = this.beautyLabel1;
			this.ClickerEnable.Location = new Point(10, 50);
			this.ClickerEnable.Name = "ClickerEnable";
			this.ClickerEnable.Size = new Size(44, 22);
			this.ClickerEnable.TabIndex = 894;
			this.ClickerEnable.Text = "beautyToggleSwitch1";
			this.ClickerEnable.ThumbSize = 12;
			this.ClickerEnable.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.ClickerEnable.UncheckedState.BorderRadius = 4;
			this.ClickerEnable.UncheckedState.BorderThickness = 1;
			this.ClickerEnable.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.ClickerEnable.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.ClickerEnable.UncheckedState.InnerBorderRadius = 4;
			this.ClickerEnable.UncheckedState.InnerBorderThickness = 0;
			this.ClickerEnable.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.ClickerEnable.UncheckedState.InnerOffset = 30;
			this.ClickerEnable.CheckedChanged += this.ClickerEnable_CheckedChanged;
			this.beautyLabel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel1.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel1.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel1.Location = new Point(60, 53);
			this.beautyLabel1.Name = "beautyLabel1";
			this.beautyLabel1.Size = new Size(53, 18);
			this.beautyLabel1.TabIndex = 842;
			this.beautyLabel1.Text = "Enable";
			this.beautyLabel1.TextPadding = new Padding(0);
			this.CPS_Slider.AnimationTrigger = 0;
			this.CPS_Slider.BackColor = Color.FromArgb(12, 14, 14);
			this.CPS_Slider.BarColor = Color.Firebrick;
			this.CPS_Slider.BorderColor = Color.FromArgb(20, 22, 22);
			this.CPS_Slider.BorderRadius = 2f;
			this.CPS_Slider.BorderSize = 1;
			this.CPS_Slider.FillColor = Color.FromArgb(16, 18, 18);
			this.CPS_Slider.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.CPS_Slider.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.CPS_Slider.HoverBarColor = Color.Firebrick;
			this.CPS_Slider.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.CPS_Slider.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.CPS_Slider.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.CPS_Slider.Location = new Point(10, 102);
			this.CPS_Slider.Maximum = 250;
			this.CPS_Slider.Minimum = 50;
			this.CPS_Slider.Name = "CPS_Slider";
			this.CPS_Slider.Offset = 1f;
			this.CPS_Slider.ShowText = false;
			this.CPS_Slider.ShowValue = true;
			this.CPS_Slider.Size = new Size(240, 20);
			this.CPS_Slider.TabIndex = 887;
			this.CPS_Slider.TargetLabel = this.labelCPS;
			this.CPS_Slider.Text = "beautyFlatSlider1";
			this.CPS_Slider.Value = 160;
			this.CPS_Slider.WriteInLabel = true;
			this.CPS_Slider.Scroll += this.CPS_Slider_Scroll;
			this.labelCPS.AutoResize = false;
			this.labelCPS.BackColor = Color.FromArgb(12, 14, 14);
			this.labelCPS.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelCPS.ForeColor = Color.FromArgb(70, 70, 80);
			this.labelCPS.Location = new Point(178, 78);
			this.labelCPS.Name = "labelCPS";
			this.labelCPS.Size = new Size(72, 18);
			this.labelCPS.TabIndex = 889;
			this.labelCPS.Text = "16.0";
			this.labelCPS.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.labelCPS.TextPadding = new Padding(0);
			this.beautyLabel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel6.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel6.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel6.Location = new Point(10, 78);
			this.beautyLabel6.Name = "beautyLabel6";
			this.beautyLabel6.Size = new Size(38, 18);
			this.beautyLabel6.TabIndex = 888;
			this.beautyLabel6.Text = "CPS:";
			this.beautyLabel6.TextPadding = new Padding(0);
			this.cbBreak.AnimationSpeed = 0.6f;
			this.cbBreak.AnimationStep = 0.9999998f;
			this.cbBreak.BackColor = Color.FromArgb(12, 14, 14);
			this.cbBreak.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbBreak.BorderRadius = 2f;
			this.cbBreak.BorderSize = 1f;
			this.cbBreak.Checked = true;
			this.cbBreak.CheckedBorderColor = Color.Firebrick;
			this.cbBreak.CheckedFillColor = Color.Firebrick;
			this.cbBreak.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbBreak.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbBreak.CheckMarkScale = 0.6f;
			this.cbBreak.FillColor = Color.FromArgb(16, 18, 18);
			this.cbBreak.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbBreak.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbBreak.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbBreak.Location = new Point(228, 160);
			this.cbBreak.Name = "cbBreak";
			this.cbBreak.Size = new Size(22, 22);
			this.cbBreak.TabIndex = 860;
			this.cbBreak.TargetLabel = this.beautyLabel7;
			this.cbBreak.Text = "beautyCheckBox3";
			this.cbBreak.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbBreak.CheckedChanged += this.cbBreak_CheckedChanged;
			this.beautyLabel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel7.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel7.ForeColor = Color.FromArgb(119, 119, 129);
			this.beautyLabel7.Location = new Point(10, 162);
			this.beautyLabel7.Name = "beautyLabel7";
			this.beautyLabel7.Size = new Size(97, 18);
			this.beautyLabel7.TabIndex = 861;
			this.beautyLabel7.Text = "Break Blocks";
			this.beautyLabel7.TextPadding = new Padding(0);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(12, 14, 14);
			base.Controls.Add(this.DefaultPanel);
			base.Name = "Combat";
			base.Size = new Size(570, 410);
			this.DefaultPanel.ResumeLayout(false);
			this.beautyPanel11.ResumeLayout(false);
			this.beautyPanel11.PerformLayout();
			this.beautyPanel12.ResumeLayout(false);
			this.beautyPanel1.ResumeLayout(false);
			this.beautyPanel1.PerformLayout();
			this.beautyPanel5.ResumeLayout(false);
			this.beautyPanel2.ResumeLayout(false);
			this.beautyPanel2.PerformLayout();
			this.beautyPanel6.ResumeLayout(false);
			this.beautyPanel9.ResumeLayout(false);
			this.beautyPanel9.PerformLayout();
			this.beautyPanel10.ResumeLayout(false);
			this.beautyPanel7.ResumeLayout(false);
			this.beautyPanel7.PerformLayout();
			this.beautyPanel8.ResumeLayout(false);
			this.beautyPanel3.ResumeLayout(false);
			this.beautyPanel3.PerformLayout();
			this.beautyPanel4.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040001BA RID: 442
		public static int ClickerBindInt;

		// Token: 0x040001BB RID: 443
		public static int ReachBindInt;

		// Token: 0x040001BC RID: 444
		public static int VelocityBindInt;

		// Token: 0x040001BD RID: 445
		public static int AimBindInt;

		// Token: 0x040001BE RID: 446
		public static int SprintResetBindInt;

		// Token: 0x040001BF RID: 447
		public static int JumpResetBindInt;

		// Token: 0x040001C0 RID: 448
		public Keys bindSprintReset;

		// Token: 0x040001C1 RID: 449
		public Keys bindJumpReset;

		// Token: 0x040001C2 RID: 450
		public Keys bindClicker;

		// Token: 0x040001C3 RID: 451
		public Keys bindReach;

		// Token: 0x040001C4 RID: 452
		public Keys bindVelocity;

		// Token: 0x040001C5 RID: 453
		public Keys bindAim;

		// Token: 0x040001C6 RID: 454
		private Dictionary<BeautyAutoButton, Keys> currentBinds = new Dictionary<BeautyAutoButton, Keys>();

		// Token: 0x040001C7 RID: 455
		private Dictionary<BeautyAutoButton, Action> currentActions = new Dictionary<BeautyAutoButton, Action>();

		// Token: 0x040001C8 RID: 456
		private IContainer components;

		// Token: 0x040001C9 RID: 457
		private BeautyPanel DefaultPanel;

		// Token: 0x040001CA RID: 458
		private BeautyPanel beautyPanel7;

		// Token: 0x040001CB RID: 459
		public BeautyFlatSlider DistanceSlider;

		// Token: 0x040001CC RID: 460
		private BeautyLabel beautyLabel22;

		// Token: 0x040001CD RID: 461
		public BeautyFlatSlider HitBoxSl;

		// Token: 0x040001CE RID: 462
		private BeautyLabel beautyLabel23;

		// Token: 0x040001CF RID: 463
		private BeautyPanel beautyPanel8;

		// Token: 0x040001D0 RID: 464
		private BeautyLabel beautyLabel20;

		// Token: 0x040001D1 RID: 465
		public BeautyToggleSwitch ReachEnable;

		// Token: 0x040001D2 RID: 466
		private BeautyLabel beautyLabel21;

		// Token: 0x040001D3 RID: 467
		public BeautyCheckBox cbReachWeapon;

		// Token: 0x040001D4 RID: 468
		private BeautyLabel beautyLabel24;

		// Token: 0x040001D5 RID: 469
		private BeautyPanel beautyPanel3;

		// Token: 0x040001D6 RID: 470
		public BeautyCheckBox Randomize;

		// Token: 0x040001D7 RID: 471
		private BeautyLabel beautyLabel5;

		// Token: 0x040001D8 RID: 472
		public BeautyCheckBox Weapon;

		// Token: 0x040001D9 RID: 473
		private BeautyLabel beautyLabel2;

		// Token: 0x040001DA RID: 474
		private BeautyPanel beautyPanel4;

		// Token: 0x040001DB RID: 475
		private BeautyLabel beautyLabel11;

		// Token: 0x040001DC RID: 476
		public BeautyToggleSwitch ClickerEnable;

		// Token: 0x040001DD RID: 477
		private BeautyLabel beautyLabel1;

		// Token: 0x040001DE RID: 478
		public BeautyFlatSlider CPS_Slider;

		// Token: 0x040001DF RID: 479
		private BeautyLabel beautyLabel6;

		// Token: 0x040001E0 RID: 480
		public BeautyCheckBox cbBreak;

		// Token: 0x040001E1 RID: 481
		private BeautyLabel beautyLabel7;

		// Token: 0x040001E2 RID: 482
		private BeautyPanel beautyPanel9;

		// Token: 0x040001E3 RID: 483
		public BeautyFlatSlider VelocityVrt;

		// Token: 0x040001E4 RID: 484
		private BeautyLabel beautyLabel9;

		// Token: 0x040001E5 RID: 485
		public BeautyFlatSlider VelocityHrz;

		// Token: 0x040001E6 RID: 486
		private BeautyLabel beautyLabel26;

		// Token: 0x040001E7 RID: 487
		public BeautyFlatSlider ChanceSlider;

		// Token: 0x040001E8 RID: 488
		private BeautyLabel beautyLabel28;

		// Token: 0x040001E9 RID: 489
		public BeautyCheckBox VelocityMovingOnly;

		// Token: 0x040001EA RID: 490
		private BeautyLabel beautyLabel29;

		// Token: 0x040001EB RID: 491
		private BeautyPanel beautyPanel10;

		// Token: 0x040001EC RID: 492
		private BeautyLabel beautyLabel30;

		// Token: 0x040001ED RID: 493
		public BeautyToggleSwitch VelocityEnable;

		// Token: 0x040001EE RID: 494
		private BeautyLabel beautyLabel31;

		// Token: 0x040001EF RID: 495
		private BeautyPanel beautyPanel2;

		// Token: 0x040001F0 RID: 496
		public BeautyCheckBox AimAssistOnlyWeapon;

		// Token: 0x040001F1 RID: 497
		private BeautyLabel beautyLabel8;

		// Token: 0x040001F2 RID: 498
		public BeautyFlatSlider slideDistance;

		// Token: 0x040001F3 RID: 499
		public BeautyFlatSlider AimAssistFovSlider;

		// Token: 0x040001F4 RID: 500
		private BeautyLabel beautyLabel14;

		// Token: 0x040001F5 RID: 501
		private BeautyLabel beautyLabel17;

		// Token: 0x040001F6 RID: 502
		public BeautyCheckBox AimAssistClickingOnly;

		// Token: 0x040001F7 RID: 503
		private BeautyLabel beautyLabel10;

		// Token: 0x040001F8 RID: 504
		private BeautyPanel beautyPanel6;

		// Token: 0x040001F9 RID: 505
		private BeautyLabel beautyLabel12;

		// Token: 0x040001FA RID: 506
		public BeautyToggleSwitch AimEnable;

		// Token: 0x040001FB RID: 507
		private BeautyLabel beautyLabel13;

		// Token: 0x040001FC RID: 508
		public BeautyCheckBox AimAssistThroughWall;

		// Token: 0x040001FD RID: 509
		private BeautyLabel beautyLabel16;

		// Token: 0x040001FE RID: 510
		public BeautyFlatSlider slidehorizontalaim;

		// Token: 0x040001FF RID: 511
		private BeautyLabel beautyLabel18;

		// Token: 0x04000200 RID: 512
		public BeautyCheckBox cbHitboxClosest;

		// Token: 0x04000201 RID: 513
		private BeautyLabel beautyLabel3;

		// Token: 0x04000202 RID: 514
		public BeautyFlatSlider slideverticalaim;

		// Token: 0x04000203 RID: 515
		private BeautyLabel beautyLabel25;

		// Token: 0x04000204 RID: 516
		public BeautyCheckBox cbVertical;

		// Token: 0x04000205 RID: 517
		private BeautyLabel beautyLabel4;

		// Token: 0x04000206 RID: 518
		public BeautyFlatSlider ticksvl;

		// Token: 0x04000207 RID: 519
		private BeautyLabel beautyLabel19;

		// Token: 0x04000208 RID: 520
		public BeautyCheckBox cbMouseMove;

		// Token: 0x04000209 RID: 521
		private BeautyLabel beautyLabel15;

		// Token: 0x0400020A RID: 522
		public BeautyCheckBox cbWallCheck;

		// Token: 0x0400020B RID: 523
		private BeautyLabel beautyLabel27;

		// Token: 0x0400020C RID: 524
		public BeautyCheckBox cbAttacking;

		// Token: 0x0400020D RID: 525
		private BeautyLabel beautyLabel33;

		// Token: 0x0400020E RID: 526
		public BeautyCheckBox cbTargeting;

		// Token: 0x0400020F RID: 527
		private BeautyLabel beautyLabel32;

		// Token: 0x04000210 RID: 528
		public BeautyCheckBox cbInventory;

		// Token: 0x04000211 RID: 529
		private BeautyLabel beautyLabel34;

		// Token: 0x04000212 RID: 530
		public BeautyCheckBox cbLockTarget;

		// Token: 0x04000213 RID: 531
		private BeautyLabel beautyLabel35;

		// Token: 0x04000214 RID: 532
		public BeautyAutoButton ClickerBindButton;

		// Token: 0x04000215 RID: 533
		public BeautyAutoButton ReachBindButton;

		// Token: 0x04000216 RID: 534
		public BeautyAutoButton VelocityBindButton;

		// Token: 0x04000217 RID: 535
		public BeautyAutoButton AimBindButton;

		// Token: 0x04000218 RID: 536
		public BeautyLabel labelReach;

		// Token: 0x04000219 RID: 537
		public BeautyLabel labelHitbox;

		// Token: 0x0400021A RID: 538
		public BeautyLabel labelCPS;

		// Token: 0x0400021B RID: 539
		public BeautyLabel labelVelV;

		// Token: 0x0400021C RID: 540
		public BeautyLabel labelVelH;

		// Token: 0x0400021D RID: 541
		public BeautyLabel lbchancevl;

		// Token: 0x0400021E RID: 542
		public BeautyLabel lbfov;

		// Token: 0x0400021F RID: 543
		public BeautyLabel labelHorizontal;

		// Token: 0x04000220 RID: 544
		public BeautyLabel labelVertical;

		// Token: 0x04000221 RID: 545
		public BeautyLabel lbticks;

		// Token: 0x04000222 RID: 546
		public BeautyLabel aimlabeldist;

		// Token: 0x04000223 RID: 547
		private BeautyPanel beautyPanel1;

		// Token: 0x04000224 RID: 548
		public BeautyAutoButton SprintResetButton;

		// Token: 0x04000225 RID: 549
		public BeautyFlatSlider numSprintResetChance;

		// Token: 0x04000226 RID: 550
		public BeautyLabel lbSprintResetChance;

		// Token: 0x04000227 RID: 551
		private BeautyLabel beautyLabel43;

		// Token: 0x04000228 RID: 552
		private BeautyPanel beautyPanel5;

		// Token: 0x04000229 RID: 553
		private BeautyLabel beautyLabel47;

		// Token: 0x0400022A RID: 554
		public BeautyToggleSwitch cbSprintReset;

		// Token: 0x0400022B RID: 555
		private BeautyLabel beautyLabel48;

		// Token: 0x0400022C RID: 556
		public BeautyFlatSlider numSprintResetMinRePress;

		// Token: 0x0400022D RID: 557
		public BeautyLabel lbSprintResetDelay;

		// Token: 0x0400022E RID: 558
		private BeautyLabel beautyLabel38;

		// Token: 0x0400022F RID: 559
		public BeautyFlatSlider numSprintResetMaxRePress;

		// Token: 0x04000230 RID: 560
		public BeautyLabel lbSprintResetStop;

		// Token: 0x04000231 RID: 561
		private BeautyLabel beautyLabel40;

		// Token: 0x04000232 RID: 562
		public BeautyComboBox ModeTypeSprint;

		// Token: 0x04000233 RID: 563
		public BeautyComboBox ModeSprint;

		// Token: 0x04000234 RID: 564
		private BeautyLabel beautyLabel41;

		// Token: 0x04000235 RID: 565
		private BeautyLabel beautyLabel36;

		// Token: 0x04000236 RID: 566
		private BeautyPanel beautyPanel11;

		// Token: 0x04000237 RID: 567
		public BeautyFlatSlider numJumpResetDelay;

		// Token: 0x04000238 RID: 568
		public BeautyLabel lbJumpResetDelay;

		// Token: 0x04000239 RID: 569
		private BeautyLabel beautyLabel51;

		// Token: 0x0400023A RID: 570
		public BeautyAutoButton JumpResetButton;

		// Token: 0x0400023B RID: 571
		public BeautyFlatSlider numJumpResetChance;

		// Token: 0x0400023C RID: 572
		public BeautyLabel lbJumpResetChance;

		// Token: 0x0400023D RID: 573
		private BeautyLabel beautyLabel53;

		// Token: 0x0400023E RID: 574
		private BeautyPanel beautyPanel12;

		// Token: 0x0400023F RID: 575
		private BeautyLabel beautyLabel54;

		// Token: 0x04000240 RID: 576
		public BeautyToggleSwitch cbJumpReset;

		// Token: 0x04000241 RID: 577
		private BeautyLabel beautyLabel55;

		// Token: 0x04000242 RID: 578
		public BeautyFlatSlider numJumpResetDuration;

		// Token: 0x04000243 RID: 579
		public BeautyLabel lbJumpResetDuration;

		// Token: 0x04000244 RID: 580
		private BeautyLabel beautyLabel45;

		// Token: 0x04000245 RID: 581
		public BeautyCheckBox cbAimBreakBlocks;

		// Token: 0x04000246 RID: 582
		private BeautyLabel beautyLabel37;
	}
}
