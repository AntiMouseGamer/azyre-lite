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
using BeautyUI2.Controls;

namespace Azyre.Categories
{
	// Token: 0x0200006A RID: 106
	public class Visuals : UserControl
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0002AE1A File Offset: 0x0002901A
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0002AE21 File Offset: 0x00029021
		public static Visuals Static { get; set; }

		// Token: 0x06000352 RID: 850 RVA: 0x0002AE2C File Offset: 0x0002902C
		public Visuals()
		{
			this.InitializeComponent();
			Visuals.Static = this;
			this.Alignment.BringToFront();
			this.ESPEMode.BringToFront();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0002AEB8 File Offset: 0x000290B8
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
				Keys oldKey;
				if (this.currentBinds.TryGetValue(btn, out oldKey))
				{
					List<Action> list;
					List<Action> moved = null;
					if (Binds.keybinds.TryGetValue(oldKey, out list))
					{
						moved = new List<Action>(list);
						Binds.keybinds.Remove(oldKey);
						Binds.keysToCheck.Remove(oldKey);
						Binds.keyStates.Remove(oldKey);
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
					if (moved != null)
					{
						foreach (Action a in moved)
						{
							if (!Binds.keybinds[key].Contains(a))
							{
								Binds.keybinds[key].Add(a);
							}
						}
					}
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

		// Token: 0x06000354 RID: 852 RVA: 0x0002AEF8 File Offset: 0x000290F8
		public void RegisterBind(BeautyAutoButton bindButton, BeautyToggleSwitch toggleSwitch, Keys bindKey, Action onToggle)
		{
			if (this.currentBinds.ContainsKey(bindButton))
			{
				Keys keys = this.currentBinds[bindButton];
				if (Binds.keybinds.ContainsKey(keys) && Binds.keybinds[keys].Count == 0)
				{
					Binds.keybinds.Remove(keys);
					Binds.keysToCheck.Remove(keys);
					Binds.keyStates.Remove(keys);
				}
			}
			if (bindKey == Keys.None)
			{
				bindButton.Text = "None";
				this.currentBinds.Remove(bindButton);
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
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0002B000 File Offset: 0x00029200
		private void ESPEnable_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0002B030 File Offset: 0x00029230
		private void ESPBoxes_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011521 File Offset: 0x0000F721
		private void ESPHealthbar_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011521 File Offset: 0x0000F721
		private void ESPEMode_IndexChanged(object sender, int e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011521 File Offset: 0x0000F721
		private void ESPNames_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbOutline_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000B19B File Offset: 0x0000939B
		private void DefaultPanel_MouseDown(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbArraylist_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0002B060 File Offset: 0x00029260
		private void ScaleAr_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.ScaleAr.Value / 100f;
			this.lbarrayscale.Text = num.ToString("0.0", CultureInfo.InvariantCulture) + " px";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbBackground_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbSideBar_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011521 File Offset: 0x0000F721
		private void cbChams_CheckedChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorArrayList_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorFill_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorOutline_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00011521 File Offset: 0x0000F721
		private void Alignment_IndexChanged(object sender, int e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorArrayListB_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorModeCombo_IndexChanged(object sender, int e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0002B0AC File Offset: 0x000292AC
		private void SpeedSlider_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.SpeedSlider.Value / 100f;
			this.lbSpeed.Text = num.ToString("0.00", CultureInfo.InvariantCulture) + "x";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0002B0F8 File Offset: 0x000292F8
		private void NumericPosX_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.NumericPosX.Value / 10f;
			this.lbPosX.Text = num.ToString("0.0", CultureInfo.InvariantCulture) + " px";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0002B144 File Offset: 0x00029344
		private void NumericPosY_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.NumericPosY.Value / 10f;
			this.lbPosY.Text = num.ToString("0.0", CultureInfo.InvariantCulture) + " px";
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0002B190 File Offset: 0x00029390
		private void SliderPaddingX_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.SliderPaddingX.Value / 10f;
			this.lbPaddingX.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0002B1D4 File Offset: 0x000293D4
		private void SliderPaddingY_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.SliderPaddingY.Value / 10f;
			this.lbPaddingY.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0002B218 File Offset: 0x00029418
		private void SliderRadius_Scroll(object sender, ScrollEventArgs e)
		{
			float num = (float)this.SliderRadius.Value / 10f;
			this.lbRadius.Text = num.ToString("0.0", CultureInfo.InvariantCulture);
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorBackgroundAL_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00011521 File Offset: 0x0000F721
		private void ColorExtraAL_SelectedColorChanged(object sender, EventArgs e)
		{
			dllconnect.EnviarConfiguracoes();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0002B259 File Offset: 0x00029459
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0002B278 File Offset: 0x00029478
		private void InitializeComponent()
		{
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel7 = new BeautyPanel();
			this.btSprint = new BeautyAutoButton();
			this.beautyPanel8 = new BeautyPanel();
			this.beautyLabel9 = new BeautyLabel();
			this.cbChams = new BeautyToggleSwitch();
			this.beautyLabel10 = new BeautyLabel();
			this.beautyPanel2 = new BeautyPanel();
			this.beautyLabel26 = new BeautyLabel();
			this.SliderRadius = new BeautyFlatSlider();
			this.lbRadius = new BeautyLabel();
			this.beautyLabel24 = new BeautyLabel();
			this.SliderPaddingY = new BeautyFlatSlider();
			this.lbPaddingY = new BeautyLabel();
			this.beautyLabel22 = new BeautyLabel();
			this.SliderPaddingX = new BeautyFlatSlider();
			this.lbPaddingX = new BeautyLabel();
			this.beautyLabel20 = new BeautyLabel();
			this.NumericPosY = new BeautyFlatSlider();
			this.lbPosY = new BeautyLabel();
			this.beautyLabel18 = new BeautyLabel();
			this.NumericPosX = new BeautyFlatSlider();
			this.lbPosX = new BeautyLabel();
			this.beautyLabel16 = new BeautyLabel();
			this.beautyLabel14 = new BeautyLabel();
			this.SpeedSlider = new BeautyFlatSlider();
			this.lbSpeed = new BeautyLabel();
			this.beautyLabel13 = new BeautyLabel();
			this.ColorModeCombo = new BeautyComboBox();
			this.beautyLabel4 = new BeautyLabel();
			this.Alignment = new BeautyComboBox();
			this.beautyLabel3 = new BeautyLabel();
			this.beautyLabel7 = new BeautyLabel();
			this.cbBackground = new BeautyCheckBox();
			this.beautyLabel5 = new BeautyLabel();
			this.ScaleAr = new BeautyFlatSlider();
			this.lbarrayscale = new BeautyLabel();
			this.beautyLabel6 = new BeautyLabel();
			this.beautyPanel3 = new BeautyPanel();
			this.beautyLabel11 = new BeautyLabel();
			this.cbArraylist = new BeautyToggleSwitch();
			this.beautyLabel12 = new BeautyLabel();
			this.beautyPanel1 = new BeautyPanel();
			this.cbDrawHurtTime = new BeautyCheckBox();
			this.beautyLabel2 = new BeautyLabel();
			this.cbDrawCorners = new BeautyCheckBox();
			this.beautyLabel1 = new BeautyLabel();
			this.ESPEMode = new BeautyComboBox();
			this.beautyLabel37 = new BeautyLabel();
			this.beautyLabel36 = new BeautyLabel();
			this.beautyLabel33 = new BeautyLabel();
			this.ESPNames = new BeautyCheckBox();
			this.beautyLabel32 = new BeautyLabel();
			this.ESPHealthbar = new BeautyCheckBox();
			this.beautyLabel27 = new BeautyLabel();
			this.ESPBoxes = new BeautyCheckBox();
			this.beautyLabel25 = new BeautyLabel();
			this.BindESP = new BeautyAutoButton();
			this.beautyPanel5 = new BeautyPanel();
			this.beautyLabel34 = new BeautyLabel();
			this.ESPEnable = new BeautyToggleSwitch();
			this.beautyLabel35 = new BeautyLabel();
			this.ColorExtraAL = new BeautyColorPicker();
			this.ColorBackgroundAL = new BeautyColorPicker();
			this.ColorArrayListB = new BeautyColorPicker();
			this.ColorArrayList = new BeautyColorPicker();
			this.ColorOutline = new BeautyColorPicker();
			this.ColorFill = new BeautyColorPicker();
			this.DefaultPanel.SuspendLayout();
			this.beautyPanel7.SuspendLayout();
			this.beautyPanel8.SuspendLayout();
			this.beautyPanel2.SuspendLayout();
			this.beautyPanel3.SuspendLayout();
			this.beautyPanel1.SuspendLayout();
			this.beautyPanel5.SuspendLayout();
			base.SuspendLayout();
			this.DefaultPanel.AutoScroll = true;
			this.DefaultPanel.AutoScrollMinSize = new Size(0, 800);
			this.DefaultPanel.BackColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.BorderColor = Color.FromArgb(20, 22, 22);
			this.DefaultPanel.BorderSizeBottom = 0f;
			this.DefaultPanel.BorderSizeLeft = 0f;
			this.DefaultPanel.BorderSizeRight = 0f;
			this.DefaultPanel.BorderSizeTop = 0f;
			this.DefaultPanel.Controls.Add(this.beautyPanel7);
			this.DefaultPanel.Controls.Add(this.beautyPanel2);
			this.DefaultPanel.Controls.Add(this.beautyPanel1);
			this.DefaultPanel.Dock = DockStyle.Fill;
			this.DefaultPanel.FillColor = Color.FromArgb(12, 14, 14);
			this.DefaultPanel.FullHeight = 800;
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
			this.beautyPanel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel7.BorderSizeBottom = 1f;
			this.beautyPanel7.BorderSizeLeft = 1f;
			this.beautyPanel7.BorderSizeRight = 1f;
			this.beautyPanel7.BorderSizeTop = 1f;
			this.beautyPanel7.Controls.Add(this.btSprint);
			this.beautyPanel7.Controls.Add(this.beautyPanel8);
			this.beautyPanel7.Controls.Add(this.cbChams);
			this.beautyPanel7.Controls.Add(this.beautyLabel10);
			this.beautyPanel7.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel7.FullHeight = 0;
			this.beautyPanel7.Location = new Point(11, 429);
			this.beautyPanel7.Name = "beautyPanel7";
			this.beautyPanel7.RadiusBottomLeft = 6f;
			this.beautyPanel7.RadiusBottomRight = 6f;
			this.beautyPanel7.RadiusTopLeft = 6f;
			this.beautyPanel7.RadiusTopRight = 6f;
			this.beautyPanel7.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel7.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel7.ScrollbarWidth = 4;
			this.beautyPanel7.Size = new Size(260, 93);
			this.beautyPanel7.TabIndex = 916;
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
			this.beautyLabel9.Size = new Size(54, 18);
			this.beautyLabel9.TabIndex = 905;
			this.beautyLabel9.Text = "Chams";
			this.beautyLabel9.TextPadding = new Padding(0);
			this.cbChams.AutoRoundCorners = true;
			this.cbChams.BackColor = Color.FromArgb(12, 14, 14);
			this.cbChams.Checked = false;
			this.cbChams.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbChams.CheckedState.BorderRadius = 4;
			this.cbChams.CheckedState.BorderThickness = 1;
			this.cbChams.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbChams.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbChams.CheckedState.InnerBorderRadius = 4;
			this.cbChams.CheckedState.InnerBorderThickness = 0;
			this.cbChams.CheckedState.InnerColor = Color.Firebrick;
			this.cbChams.CheckedState.InnerOffset = 2;
			this.cbChams.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbChams.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbChams.LinkedLabel = this.beautyLabel10;
			this.cbChams.Location = new Point(10, 50);
			this.cbChams.Name = "cbChams";
			this.cbChams.Size = new Size(44, 22);
			this.cbChams.TabIndex = 894;
			this.cbChams.Text = "beautyToggleSwitch2";
			this.cbChams.ThumbSize = 12;
			this.cbChams.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbChams.UncheckedState.BorderRadius = 4;
			this.cbChams.UncheckedState.BorderThickness = 1;
			this.cbChams.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbChams.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbChams.UncheckedState.InnerBorderRadius = 4;
			this.cbChams.UncheckedState.InnerBorderThickness = 0;
			this.cbChams.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbChams.UncheckedState.InnerOffset = 30;
			this.cbChams.CheckedChanged += this.cbChams_CheckedChanged;
			this.beautyLabel10.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel10.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel10.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel10.Location = new Point(60, 53);
			this.beautyLabel10.Name = "beautyLabel10";
			this.beautyLabel10.Size = new Size(53, 18);
			this.beautyLabel10.TabIndex = 842;
			this.beautyLabel10.Text = "Enable";
			this.beautyLabel10.TextPadding = new Padding(0);
			this.beautyPanel2.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel2.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel2.BorderSizeBottom = 1f;
			this.beautyPanel2.BorderSizeLeft = 1f;
			this.beautyPanel2.BorderSizeRight = 1f;
			this.beautyPanel2.BorderSizeTop = 1f;
			this.beautyPanel2.Controls.Add(this.ColorExtraAL);
			this.beautyPanel2.Controls.Add(this.beautyLabel26);
			this.beautyPanel2.Controls.Add(this.ColorBackgroundAL);
			this.beautyPanel2.Controls.Add(this.SliderRadius);
			this.beautyPanel2.Controls.Add(this.beautyLabel24);
			this.beautyPanel2.Controls.Add(this.lbRadius);
			this.beautyPanel2.Controls.Add(this.SliderPaddingY);
			this.beautyPanel2.Controls.Add(this.beautyLabel22);
			this.beautyPanel2.Controls.Add(this.lbPaddingY);
			this.beautyPanel2.Controls.Add(this.SliderPaddingX);
			this.beautyPanel2.Controls.Add(this.beautyLabel20);
			this.beautyPanel2.Controls.Add(this.lbPaddingX);
			this.beautyPanel2.Controls.Add(this.NumericPosY);
			this.beautyPanel2.Controls.Add(this.beautyLabel18);
			this.beautyPanel2.Controls.Add(this.lbPosY);
			this.beautyPanel2.Controls.Add(this.NumericPosX);
			this.beautyPanel2.Controls.Add(this.beautyLabel16);
			this.beautyPanel2.Controls.Add(this.lbPosX);
			this.beautyPanel2.Controls.Add(this.ColorArrayListB);
			this.beautyPanel2.Controls.Add(this.beautyLabel14);
			this.beautyPanel2.Controls.Add(this.SpeedSlider);
			this.beautyPanel2.Controls.Add(this.beautyLabel13);
			this.beautyPanel2.Controls.Add(this.lbSpeed);
			this.beautyPanel2.Controls.Add(this.ColorModeCombo);
			this.beautyPanel2.Controls.Add(this.beautyLabel4);
			this.beautyPanel2.Controls.Add(this.Alignment);
			this.beautyPanel2.Controls.Add(this.beautyLabel3);
			this.beautyPanel2.Controls.Add(this.ColorArrayList);
			this.beautyPanel2.Controls.Add(this.beautyLabel7);
			this.beautyPanel2.Controls.Add(this.cbBackground);
			this.beautyPanel2.Controls.Add(this.beautyLabel5);
			this.beautyPanel2.Controls.Add(this.ScaleAr);
			this.beautyPanel2.Controls.Add(this.beautyLabel6);
			this.beautyPanel2.Controls.Add(this.lbarrayscale);
			this.beautyPanel2.Controls.Add(this.beautyPanel3);
			this.beautyPanel2.Controls.Add(this.cbArraylist);
			this.beautyPanel2.Controls.Add(this.beautyLabel12);
			this.beautyPanel2.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel2.FullHeight = 0;
			this.beautyPanel2.Location = new Point(281, 21);
			this.beautyPanel2.Name = "beautyPanel2";
			this.beautyPanel2.RadiusBottomLeft = 6f;
			this.beautyPanel2.RadiusBottomRight = 6f;
			this.beautyPanel2.RadiusTopLeft = 6f;
			this.beautyPanel2.RadiusTopRight = 6f;
			this.beautyPanel2.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel2.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel2.ScrollbarWidth = 4;
			this.beautyPanel2.Size = new Size(260, 743);
			this.beautyPanel2.TabIndex = 915;
			this.beautyLabel26.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel26.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel26.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel26.Location = new Point(11, 531);
			this.beautyLabel26.Name = "beautyLabel26";
			this.beautyLabel26.Size = new Size(70, 18);
			this.beautyLabel26.TabIndex = 962;
			this.beautyLabel26.Text = "Info color";
			this.beautyLabel26.TextPadding = new Padding(0);
			this.SliderRadius.AnimationTrigger = 0;
			this.SliderRadius.BackColor = Color.FromArgb(12, 14, 14);
			this.SliderRadius.BarColor = Color.Firebrick;
			this.SliderRadius.BorderColor = Color.FromArgb(20, 22, 22);
			this.SliderRadius.BorderRadius = 2f;
			this.SliderRadius.BorderSize = 1;
			this.SliderRadius.FillColor = Color.FromArgb(16, 18, 18);
			this.SliderRadius.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.SliderRadius.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.SliderRadius.HoverBarColor = Color.Firebrick;
			this.SliderRadius.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.SliderRadius.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SliderRadius.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.SliderRadius.Location = new Point(11, 408);
			this.SliderRadius.Maximum = 35;
			this.SliderRadius.Minimum = 0;
			this.SliderRadius.Name = "SliderRadius";
			this.SliderRadius.Offset = 1f;
			this.SliderRadius.ShowText = false;
			this.SliderRadius.ShowValue = true;
			this.SliderRadius.Size = new Size(240, 20);
			this.SliderRadius.TabIndex = 958;
			this.SliderRadius.TargetLabel = this.lbRadius;
			this.SliderRadius.Text = "beautyFlatSlider5";
			this.SliderRadius.Value = 30;
			this.SliderRadius.WriteInLabel = true;
			this.SliderRadius.Scroll += this.SliderRadius_Scroll;
			this.lbRadius.AutoResize = false;
			this.lbRadius.BackColor = Color.FromArgb(12, 14, 14);
			this.lbRadius.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbRadius.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbRadius.Location = new Point(178, 384);
			this.lbRadius.Name = "lbRadius";
			this.lbRadius.Size = new Size(72, 18);
			this.lbRadius.TabIndex = 960;
			this.lbRadius.Text = "3.0";
			this.lbRadius.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbRadius.TextPadding = new Padding(0);
			this.beautyLabel24.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel24.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel24.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel24.Location = new Point(11, 384);
			this.beautyLabel24.Name = "beautyLabel24";
			this.beautyLabel24.Size = new Size(57, 18);
			this.beautyLabel24.TabIndex = 959;
			this.beautyLabel24.Text = "Radius:";
			this.beautyLabel24.TextPadding = new Padding(0);
			this.SliderPaddingY.AnimationTrigger = 0;
			this.SliderPaddingY.BackColor = Color.FromArgb(12, 14, 14);
			this.SliderPaddingY.BarColor = Color.Firebrick;
			this.SliderPaddingY.BorderColor = Color.FromArgb(20, 22, 22);
			this.SliderPaddingY.BorderRadius = 2f;
			this.SliderPaddingY.BorderSize = 1;
			this.SliderPaddingY.FillColor = Color.FromArgb(16, 18, 18);
			this.SliderPaddingY.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.SliderPaddingY.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.SliderPaddingY.HoverBarColor = Color.Firebrick;
			this.SliderPaddingY.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.SliderPaddingY.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SliderPaddingY.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.SliderPaddingY.Location = new Point(11, 358);
			this.SliderPaddingY.Maximum = 500;
			this.SliderPaddingY.Minimum = 0;
			this.SliderPaddingY.Name = "SliderPaddingY";
			this.SliderPaddingY.Offset = 1f;
			this.SliderPaddingY.ShowText = false;
			this.SliderPaddingY.ShowValue = true;
			this.SliderPaddingY.Size = new Size(240, 20);
			this.SliderPaddingY.TabIndex = 955;
			this.SliderPaddingY.TargetLabel = this.lbPaddingY;
			this.SliderPaddingY.Text = "beautyFlatSlider4";
			this.SliderPaddingY.Value = 0;
			this.SliderPaddingY.WriteInLabel = true;
			this.SliderPaddingY.Scroll += this.SliderPaddingY_Scroll;
			this.lbPaddingY.AutoResize = false;
			this.lbPaddingY.BackColor = Color.FromArgb(12, 14, 14);
			this.lbPaddingY.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbPaddingY.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbPaddingY.Location = new Point(179, 334);
			this.lbPaddingY.Name = "lbPaddingY";
			this.lbPaddingY.Size = new Size(72, 18);
			this.lbPaddingY.TabIndex = 957;
			this.lbPaddingY.Text = "0.0";
			this.lbPaddingY.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbPaddingY.TextPadding = new Padding(0);
			this.beautyLabel22.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel22.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel22.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel22.Location = new Point(11, 334);
			this.beautyLabel22.Name = "beautyLabel22";
			this.beautyLabel22.Size = new Size(47, 18);
			this.beautyLabel22.TabIndex = 956;
			this.beautyLabel22.Text = "Pad Y:";
			this.beautyLabel22.TextPadding = new Padding(0);
			this.SliderPaddingX.AnimationTrigger = 0;
			this.SliderPaddingX.BackColor = Color.FromArgb(12, 14, 14);
			this.SliderPaddingX.BarColor = Color.Firebrick;
			this.SliderPaddingX.BorderColor = Color.FromArgb(20, 22, 22);
			this.SliderPaddingX.BorderRadius = 2f;
			this.SliderPaddingX.BorderSize = 1;
			this.SliderPaddingX.FillColor = Color.FromArgb(16, 18, 18);
			this.SliderPaddingX.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.SliderPaddingX.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.SliderPaddingX.HoverBarColor = Color.Firebrick;
			this.SliderPaddingX.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.SliderPaddingX.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SliderPaddingX.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.SliderPaddingX.Location = new Point(10, 304);
			this.SliderPaddingX.Maximum = 500;
			this.SliderPaddingX.Minimum = 0;
			this.SliderPaddingX.Name = "SliderPaddingX";
			this.SliderPaddingX.Offset = 1f;
			this.SliderPaddingX.ShowText = false;
			this.SliderPaddingX.ShowValue = true;
			this.SliderPaddingX.Size = new Size(240, 20);
			this.SliderPaddingX.TabIndex = 952;
			this.SliderPaddingX.TargetLabel = this.lbPaddingX;
			this.SliderPaddingX.Text = "beautyFlatSlider3";
			this.SliderPaddingX.Value = 50;
			this.SliderPaddingX.WriteInLabel = true;
			this.SliderPaddingX.Scroll += this.SliderPaddingX_Scroll;
			this.lbPaddingX.AutoResize = false;
			this.lbPaddingX.BackColor = Color.FromArgb(12, 14, 14);
			this.lbPaddingX.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbPaddingX.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbPaddingX.Location = new Point(178, 280);
			this.lbPaddingX.Name = "lbPaddingX";
			this.lbPaddingX.Size = new Size(72, 18);
			this.lbPaddingX.TabIndex = 954;
			this.lbPaddingX.Text = "5.0";
			this.lbPaddingX.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbPaddingX.TextPadding = new Padding(0);
			this.beautyLabel20.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel20.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel20.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel20.Location = new Point(10, 280);
			this.beautyLabel20.Name = "beautyLabel20";
			this.beautyLabel20.Size = new Size(50, 18);
			this.beautyLabel20.TabIndex = 953;
			this.beautyLabel20.Text = "Pad X:";
			this.beautyLabel20.TextPadding = new Padding(0);
			this.NumericPosY.AnimationTrigger = 0;
			this.NumericPosY.BackColor = Color.FromArgb(12, 14, 14);
			this.NumericPosY.BarColor = Color.Firebrick;
			this.NumericPosY.BorderColor = Color.FromArgb(20, 22, 22);
			this.NumericPosY.BorderRadius = 2f;
			this.NumericPosY.BorderSize = 1;
			this.NumericPosY.FillColor = Color.FromArgb(16, 18, 18);
			this.NumericPosY.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.NumericPosY.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.NumericPosY.HoverBarColor = Color.Firebrick;
			this.NumericPosY.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.NumericPosY.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.NumericPosY.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.NumericPosY.Location = new Point(10, 254);
			this.NumericPosY.Maximum = 300;
			this.NumericPosY.Minimum = 0;
			this.NumericPosY.Name = "NumericPosY";
			this.NumericPosY.Offset = 1f;
			this.NumericPosY.ShowText = false;
			this.NumericPosY.ShowValue = true;
			this.NumericPosY.Size = new Size(240, 20);
			this.NumericPosY.TabIndex = 949;
			this.NumericPosY.TargetLabel = this.lbPosY;
			this.NumericPosY.Text = "beautyFlatSlider2";
			this.NumericPosY.Value = 0;
			this.NumericPosY.WriteInLabel = true;
			this.NumericPosY.Scroll += this.NumericPosY_Scroll;
			this.lbPosY.AutoResize = false;
			this.lbPosY.BackColor = Color.FromArgb(12, 14, 14);
			this.lbPosY.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbPosY.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbPosY.Location = new Point(178, 230);
			this.lbPosY.Name = "lbPosY";
			this.lbPosY.Size = new Size(72, 18);
			this.lbPosY.TabIndex = 951;
			this.lbPosY.Text = "10.0 px";
			this.lbPosY.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbPosY.TextPadding = new Padding(0);
			this.beautyLabel18.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel18.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel18.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel18.Location = new Point(10, 230);
			this.beautyLabel18.Name = "beautyLabel18";
			this.beautyLabel18.Size = new Size(46, 18);
			this.beautyLabel18.TabIndex = 950;
			this.beautyLabel18.Text = "Pos Y:";
			this.beautyLabel18.TextPadding = new Padding(0);
			this.NumericPosX.AnimationTrigger = 0;
			this.NumericPosX.BackColor = Color.FromArgb(12, 14, 14);
			this.NumericPosX.BarColor = Color.Firebrick;
			this.NumericPosX.BorderColor = Color.FromArgb(20, 22, 22);
			this.NumericPosX.BorderRadius = 2f;
			this.NumericPosX.BorderSize = 1;
			this.NumericPosX.FillColor = Color.FromArgb(16, 18, 18);
			this.NumericPosX.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.NumericPosX.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.NumericPosX.HoverBarColor = Color.Firebrick;
			this.NumericPosX.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.NumericPosX.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.NumericPosX.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.NumericPosX.Location = new Point(10, 204);
			this.NumericPosX.Maximum = 300;
			this.NumericPosX.Minimum = 0;
			this.NumericPosX.Name = "NumericPosX";
			this.NumericPosX.Offset = 1f;
			this.NumericPosX.ShowText = false;
			this.NumericPosX.ShowValue = true;
			this.NumericPosX.Size = new Size(240, 20);
			this.NumericPosX.TabIndex = 946;
			this.NumericPosX.TargetLabel = this.lbPosX;
			this.NumericPosX.Text = "beautyFlatSlider1";
			this.NumericPosX.Value = 0;
			this.NumericPosX.WriteInLabel = true;
			this.NumericPosX.Scroll += this.NumericPosX_Scroll;
			this.lbPosX.AutoResize = false;
			this.lbPosX.BackColor = Color.FromArgb(12, 14, 14);
			this.lbPosX.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbPosX.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbPosX.Location = new Point(178, 180);
			this.lbPosX.Name = "lbPosX";
			this.lbPosX.Size = new Size(72, 18);
			this.lbPosX.TabIndex = 948;
			this.lbPosX.Text = "0.0 px";
			this.lbPosX.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbPosX.TextPadding = new Padding(0);
			this.beautyLabel16.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel16.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel16.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel16.Location = new Point(10, 180);
			this.beautyLabel16.Name = "beautyLabel16";
			this.beautyLabel16.Size = new Size(49, 18);
			this.beautyLabel16.TabIndex = 947;
			this.beautyLabel16.Text = "Pos X:";
			this.beautyLabel16.TextPadding = new Padding(0);
			this.beautyLabel14.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel14.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel14.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel14.Location = new Point(11, 505);
			this.beautyLabel14.Name = "beautyLabel14";
			this.beautyLabel14.Size = new Size(57, 18);
			this.beautyLabel14.TabIndex = 945;
			this.beautyLabel14.Text = "Color B";
			this.beautyLabel14.TextPadding = new Padding(0);
			this.SpeedSlider.AnimationTrigger = 0;
			this.SpeedSlider.BackColor = Color.FromArgb(12, 14, 14);
			this.SpeedSlider.BarColor = Color.Firebrick;
			this.SpeedSlider.BorderColor = Color.FromArgb(20, 22, 22);
			this.SpeedSlider.BorderRadius = 2f;
			this.SpeedSlider.BorderSize = 1;
			this.SpeedSlider.FillColor = Color.FromArgb(16, 18, 18);
			this.SpeedSlider.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.SpeedSlider.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.SpeedSlider.HoverBarColor = Color.Firebrick;
			this.SpeedSlider.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.SpeedSlider.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SpeedSlider.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.SpeedSlider.Location = new Point(10, 154);
			this.SpeedSlider.Maximum = 1000;
			this.SpeedSlider.Minimum = 0;
			this.SpeedSlider.Name = "SpeedSlider";
			this.SpeedSlider.Offset = 1f;
			this.SpeedSlider.ShowText = false;
			this.SpeedSlider.ShowValue = true;
			this.SpeedSlider.Size = new Size(240, 20);
			this.SpeedSlider.TabIndex = 941;
			this.SpeedSlider.TargetLabel = this.lbSpeed;
			this.SpeedSlider.Text = "beautyFlatSlider1";
			this.SpeedSlider.Value = 50;
			this.SpeedSlider.WriteInLabel = true;
			this.SpeedSlider.Scroll += this.SpeedSlider_Scroll;
			this.lbSpeed.AutoResize = false;
			this.lbSpeed.BackColor = Color.FromArgb(12, 14, 14);
			this.lbSpeed.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbSpeed.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbSpeed.Location = new Point(178, 130);
			this.lbSpeed.Name = "lbSpeed";
			this.lbSpeed.Size = new Size(72, 18);
			this.lbSpeed.TabIndex = 943;
			this.lbSpeed.Text = "0.50 x";
			this.lbSpeed.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbSpeed.TextPadding = new Padding(0);
			this.beautyLabel13.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel13.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel13.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel13.Location = new Point(10, 129);
			this.beautyLabel13.Name = "beautyLabel13";
			this.beautyLabel13.Size = new Size(52, 18);
			this.beautyLabel13.TabIndex = 942;
			this.beautyLabel13.Text = "Speed:";
			this.beautyLabel13.TextPadding = new Padding(0);
			this.ColorModeCombo.BorderColor = Color.FromArgb(20, 22, 22);
			this.ColorModeCombo.BorderRadius = 2f;
			this.ColorModeCombo.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ColorModeCombo.FillColor = Color.FromArgb(16, 18, 18);
			this.ColorModeCombo.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.ColorModeCombo.ForeColor = Color.FromArgb(120, 120, 130);
			this.ColorModeCombo.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.ColorModeCombo.ForeText = "Static";
			this.ColorModeCombo.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ColorModeCombo.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ColorModeCombo.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.ColorModeCombo.ItemHeight = 30;
			this.ColorModeCombo.Items = new string[]
			{
				"Static",
				"Chroma",
				"Wave"
			};
			this.ColorModeCombo.Location = new Point(146, 595);
			this.ColorModeCombo.Name = "ColorModeCombo";
			this.ColorModeCombo.Size = new Size(104, 24);
			this.ColorModeCombo.TabIndex = 939;
			this.ColorModeCombo.Text = "Static";
			this.ColorModeCombo.IndexChanged += this.ColorModeCombo_IndexChanged;
			this.beautyLabel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel4.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel4.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel4.Location = new Point(11, 596);
			this.beautyLabel4.Name = "beautyLabel4";
			this.beautyLabel4.Size = new Size(84, 18);
			this.beautyLabel4.TabIndex = 940;
			this.beautyLabel4.Text = "Color mode";
			this.beautyLabel4.TextPadding = new Padding(0);
			this.Alignment.BorderColor = Color.FromArgb(20, 22, 22);
			this.Alignment.BorderRadius = 2f;
			this.Alignment.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.Alignment.FillColor = Color.FromArgb(16, 18, 18);
			this.Alignment.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.Alignment.ForeColor = Color.FromArgb(120, 120, 130);
			this.Alignment.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.Alignment.ForeText = "Right";
			this.Alignment.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.Alignment.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.Alignment.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.Alignment.ItemHeight = 30;
			this.Alignment.Items = new string[]
			{
				"Right",
				"Left"
			};
			this.Alignment.Location = new Point(179, 558);
			this.Alignment.Name = "Alignment";
			this.Alignment.Size = new Size(74, 24);
			this.Alignment.TabIndex = 926;
			this.Alignment.Text = "Right";
			this.Alignment.IndexChanged += this.Alignment_IndexChanged;
			this.beautyLabel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel3.Location = new Point(11, 563);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(76, 18);
			this.beautyLabel3.TabIndex = 938;
			this.beautyLabel3.Text = "Alignment";
			this.beautyLabel3.TextPadding = new Padding(0);
			this.beautyLabel7.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel7.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel7.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel7.Location = new Point(11, 478);
			this.beautyLabel7.Name = "beautyLabel7";
			this.beautyLabel7.Size = new Size(57, 18);
			this.beautyLabel7.TabIndex = 936;
			this.beautyLabel7.Text = "Color A";
			this.beautyLabel7.TextPadding = new Padding(0);
			this.cbBackground.AnimationSpeed = 0.6f;
			this.cbBackground.BackColor = Color.FromArgb(12, 14, 14);
			this.cbBackground.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbBackground.BorderRadius = 2f;
			this.cbBackground.BorderSize = 1f;
			this.cbBackground.CheckedBorderColor = Color.Firebrick;
			this.cbBackground.CheckedFillColor = Color.Firebrick;
			this.cbBackground.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbBackground.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbBackground.CheckMarkScale = 0.6f;
			this.cbBackground.FillColor = Color.FromArgb(16, 18, 18);
			this.cbBackground.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbBackground.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbBackground.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbBackground.Location = new Point(229, 449);
			this.cbBackground.Name = "cbBackground";
			this.cbBackground.Size = new Size(22, 22);
			this.cbBackground.TabIndex = 932;
			this.cbBackground.TargetLabel = this.beautyLabel5;
			this.cbBackground.Text = "HealthBar";
			this.cbBackground.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbBackground.CheckedChanged += this.cbBackground_CheckedChanged;
			this.beautyLabel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel5.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel5.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel5.Location = new Point(11, 449);
			this.beautyLabel5.Name = "beautyLabel5";
			this.beautyLabel5.Size = new Size(87, 18);
			this.beautyLabel5.TabIndex = 933;
			this.beautyLabel5.Text = "Background";
			this.beautyLabel5.TextPadding = new Padding(0);
			this.ScaleAr.AnimationTrigger = 0;
			this.ScaleAr.BackColor = Color.FromArgb(12, 14, 14);
			this.ScaleAr.BarColor = Color.Firebrick;
			this.ScaleAr.BorderColor = Color.FromArgb(20, 22, 22);
			this.ScaleAr.BorderRadius = 2f;
			this.ScaleAr.BorderSize = 1;
			this.ScaleAr.FillColor = Color.FromArgb(16, 18, 18);
			this.ScaleAr.Font = new Font("Segoe UI Semibold", 9.75f, FontStyle.Bold);
			this.ScaleAr.ForegroundColor = Color.FromArgb(70, 70, 80);
			this.ScaleAr.HoverBarColor = Color.Firebrick;
			this.ScaleAr.HoverBorderColor = Color.FromArgb(24, 24, 26);
			this.ScaleAr.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ScaleAr.HoverForegroundColor = Color.FromArgb(120, 120, 130);
			this.ScaleAr.Location = new Point(10, 104);
			this.ScaleAr.Maximum = 200;
			this.ScaleAr.Minimum = 0;
			this.ScaleAr.Name = "ScaleAr";
			this.ScaleAr.Offset = 1f;
			this.ScaleAr.ShowText = false;
			this.ScaleAr.ShowValue = true;
			this.ScaleAr.Size = new Size(240, 20);
			this.ScaleAr.TabIndex = 928;
			this.ScaleAr.TargetLabel = this.lbarrayscale;
			this.ScaleAr.Text = "beautyFlatSlider1";
			this.ScaleAr.Value = 100;
			this.ScaleAr.WriteInLabel = true;
			this.ScaleAr.Scroll += this.ScaleAr_Scroll;
			this.lbarrayscale.AutoResize = false;
			this.lbarrayscale.BackColor = Color.FromArgb(12, 14, 14);
			this.lbarrayscale.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.lbarrayscale.ForeColor = Color.FromArgb(70, 70, 80);
			this.lbarrayscale.Location = new Point(178, 80);
			this.lbarrayscale.Name = "lbarrayscale";
			this.lbarrayscale.Size = new Size(72, 18);
			this.lbarrayscale.TabIndex = 930;
			this.lbarrayscale.Text = "1.0 px";
			this.lbarrayscale.TextAlign = (BeautyLabel.HorizontalAlignment)2;
			this.lbarrayscale.TextPadding = new Padding(0);
			this.beautyLabel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel6.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel6.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel6.Location = new Point(10, 80);
			this.beautyLabel6.Name = "beautyLabel6";
			this.beautyLabel6.Size = new Size(47, 18);
			this.beautyLabel6.TabIndex = 929;
			this.beautyLabel6.Text = "Scale:";
			this.beautyLabel6.TextPadding = new Padding(0);
			this.beautyPanel3.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel3.BorderSizeBottom = 1f;
			this.beautyPanel3.BorderSizeLeft = 1f;
			this.beautyPanel3.BorderSizeRight = 1f;
			this.beautyPanel3.BorderSizeTop = 1f;
			this.beautyPanel3.Controls.Add(this.beautyLabel11);
			this.beautyPanel3.Dock = DockStyle.Top;
			this.beautyPanel3.FillColor = Color.FromArgb(16, 18, 18);
			this.beautyPanel3.FullHeight = 350;
			this.beautyPanel3.Location = new Point(0, 0);
			this.beautyPanel3.Name = "beautyPanel3";
			this.beautyPanel3.RadiusBottomLeft = 0f;
			this.beautyPanel3.RadiusBottomRight = 0f;
			this.beautyPanel3.RadiusTopLeft = 6f;
			this.beautyPanel3.RadiusTopRight = 6f;
			this.beautyPanel3.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel3.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel3.ScrollbarWidth = 4;
			this.beautyPanel3.Size = new Size(260, 40);
			this.beautyPanel3.TabIndex = 904;
			this.beautyLabel11.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel11.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel11.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel11.Location = new Point(10, 11);
			this.beautyLabel11.Name = "beautyLabel11";
			this.beautyLabel11.Size = new Size(75, 18);
			this.beautyLabel11.TabIndex = 905;
			this.beautyLabel11.Text = "Array List";
			this.beautyLabel11.TextPadding = new Padding(0);
			this.cbArraylist.AutoRoundCorners = true;
			this.cbArraylist.BackColor = Color.FromArgb(12, 14, 14);
			this.cbArraylist.Checked = false;
			this.cbArraylist.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.cbArraylist.CheckedState.BorderRadius = 4;
			this.cbArraylist.CheckedState.BorderThickness = 1;
			this.cbArraylist.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.cbArraylist.CheckedState.InnerBorderColor = Color.Firebrick;
			this.cbArraylist.CheckedState.InnerBorderRadius = 4;
			this.cbArraylist.CheckedState.InnerBorderThickness = 0;
			this.cbArraylist.CheckedState.InnerColor = Color.Firebrick;
			this.cbArraylist.CheckedState.InnerOffset = 2;
			this.cbArraylist.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.cbArraylist.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.cbArraylist.LinkedLabel = this.beautyLabel12;
			this.cbArraylist.Location = new Point(10, 50);
			this.cbArraylist.Name = "cbArraylist";
			this.cbArraylist.Size = new Size(44, 22);
			this.cbArraylist.TabIndex = 894;
			this.cbArraylist.Text = "beautyToggleSwitch3";
			this.cbArraylist.ThumbSize = 12;
			this.cbArraylist.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbArraylist.UncheckedState.BorderRadius = 4;
			this.cbArraylist.UncheckedState.BorderThickness = 1;
			this.cbArraylist.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.cbArraylist.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.cbArraylist.UncheckedState.InnerBorderRadius = 4;
			this.cbArraylist.UncheckedState.InnerBorderThickness = 0;
			this.cbArraylist.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.cbArraylist.UncheckedState.InnerOffset = 30;
			this.cbArraylist.CheckedChanged += this.cbArraylist_CheckedChanged;
			this.beautyLabel12.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel12.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel12.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel12.Location = new Point(60, 52);
			this.beautyLabel12.Name = "beautyLabel12";
			this.beautyLabel12.Size = new Size(53, 18);
			this.beautyLabel12.TabIndex = 842;
			this.beautyLabel12.Text = "Enable";
			this.beautyLabel12.TextPadding = new Padding(0);
			this.beautyPanel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel1.BorderSizeBottom = 1f;
			this.beautyPanel1.BorderSizeLeft = 1f;
			this.beautyPanel1.BorderSizeRight = 1f;
			this.beautyPanel1.BorderSizeTop = 1f;
			this.beautyPanel1.Controls.Add(this.ColorOutline);
			this.beautyPanel1.Controls.Add(this.ColorFill);
			this.beautyPanel1.Controls.Add(this.cbDrawHurtTime);
			this.beautyPanel1.Controls.Add(this.beautyLabel2);
			this.beautyPanel1.Controls.Add(this.cbDrawCorners);
			this.beautyPanel1.Controls.Add(this.beautyLabel1);
			this.beautyPanel1.Controls.Add(this.ESPEMode);
			this.beautyPanel1.Controls.Add(this.beautyLabel37);
			this.beautyPanel1.Controls.Add(this.beautyLabel36);
			this.beautyPanel1.Controls.Add(this.beautyLabel33);
			this.beautyPanel1.Controls.Add(this.ESPNames);
			this.beautyPanel1.Controls.Add(this.beautyLabel32);
			this.beautyPanel1.Controls.Add(this.ESPHealthbar);
			this.beautyPanel1.Controls.Add(this.beautyLabel27);
			this.beautyPanel1.Controls.Add(this.ESPBoxes);
			this.beautyPanel1.Controls.Add(this.beautyLabel25);
			this.beautyPanel1.Controls.Add(this.BindESP);
			this.beautyPanel1.Controls.Add(this.beautyPanel5);
			this.beautyPanel1.Controls.Add(this.ESPEnable);
			this.beautyPanel1.Controls.Add(this.beautyLabel35);
			this.beautyPanel1.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.FullHeight = 0;
			this.beautyPanel1.Location = new Point(11, 21);
			this.beautyPanel1.Name = "beautyPanel1";
			this.beautyPanel1.RadiusBottomLeft = 6f;
			this.beautyPanel1.RadiusBottomRight = 6f;
			this.beautyPanel1.RadiusTopLeft = 6f;
			this.beautyPanel1.RadiusTopRight = 6f;
			this.beautyPanel1.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel1.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel1.ScrollbarWidth = 4;
			this.beautyPanel1.Size = new Size(260, 402);
			this.beautyPanel1.TabIndex = 912;
			this.cbDrawHurtTime.AnimationSpeed = 0.6f;
			this.cbDrawHurtTime.BackColor = Color.FromArgb(12, 14, 14);
			this.cbDrawHurtTime.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbDrawHurtTime.BorderRadius = 2f;
			this.cbDrawHurtTime.BorderSize = 1f;
			this.cbDrawHurtTime.CheckedBorderColor = Color.Firebrick;
			this.cbDrawHurtTime.CheckedFillColor = Color.Firebrick;
			this.cbDrawHurtTime.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbDrawHurtTime.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbDrawHurtTime.CheckMarkScale = 0.6f;
			this.cbDrawHurtTime.FillColor = Color.FromArgb(16, 18, 18);
			this.cbDrawHurtTime.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbDrawHurtTime.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbDrawHurtTime.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbDrawHurtTime.Location = new Point(228, 202);
			this.cbDrawHurtTime.Name = "cbDrawHurtTime";
			this.cbDrawHurtTime.Size = new Size(22, 22);
			this.cbDrawHurtTime.TabIndex = 928;
			this.cbDrawHurtTime.TargetLabel = this.beautyLabel2;
			this.cbDrawHurtTime.Text = "HealthBar";
			this.cbDrawHurtTime.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel2.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel2.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel2.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel2.Location = new Point(9, 205);
			this.beautyLabel2.Name = "beautyLabel2";
			this.beautyLabel2.Size = new Size(113, 18);
			this.beautyLabel2.TabIndex = 929;
			this.beautyLabel2.Text = "Draw Hurt Time";
			this.beautyLabel2.TextPadding = new Padding(0);
			this.cbDrawCorners.AnimationSpeed = 0.6f;
			this.cbDrawCorners.BackColor = Color.FromArgb(12, 14, 14);
			this.cbDrawCorners.BorderColor = Color.FromArgb(20, 22, 22);
			this.cbDrawCorners.BorderRadius = 2f;
			this.cbDrawCorners.BorderSize = 1f;
			this.cbDrawCorners.CheckedBorderColor = Color.Firebrick;
			this.cbDrawCorners.CheckedFillColor = Color.Firebrick;
			this.cbDrawCorners.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.cbDrawCorners.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.cbDrawCorners.CheckMarkScale = 0.6f;
			this.cbDrawCorners.FillColor = Color.FromArgb(16, 18, 18);
			this.cbDrawCorners.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.cbDrawCorners.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.cbDrawCorners.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.cbDrawCorners.Location = new Point(228, 171);
			this.cbDrawCorners.Name = "cbDrawCorners";
			this.cbDrawCorners.Size = new Size(22, 22);
			this.cbDrawCorners.TabIndex = 926;
			this.cbDrawCorners.TargetLabel = this.beautyLabel1;
			this.cbDrawCorners.Text = "HealthBar";
			this.cbDrawCorners.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.cbDrawCorners.CheckedChanged += this.cbOutline_CheckedChanged;
			this.beautyLabel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel1.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel1.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel1.Location = new Point(9, 171);
			this.beautyLabel1.Name = "beautyLabel1";
			this.beautyLabel1.Size = new Size(101, 18);
			this.beautyLabel1.TabIndex = 927;
			this.beautyLabel1.Text = "Draw Corners";
			this.beautyLabel1.TextPadding = new Padding(0);
			this.ESPEMode.BorderColor = Color.FromArgb(20, 22, 22);
			this.ESPEMode.BorderRadius = 2f;
			this.ESPEMode.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ESPEMode.FillColor = Color.FromArgb(16, 18, 18);
			this.ESPEMode.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.ESPEMode.ForeColor = Color.FromArgb(120, 120, 130);
			this.ESPEMode.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.ESPEMode.ForeText = "2D";
			this.ESPEMode.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ESPEMode.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ESPEMode.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.ESPEMode.ItemHeight = 30;
			this.ESPEMode.Items = new string[]
			{
				"2D",
				"3D"
			};
			this.ESPEMode.Location = new Point(176, 306);
			this.ESPEMode.Name = "ESPEMode";
			this.ESPEMode.Size = new Size(74, 24);
			this.ESPEMode.TabIndex = 925;
			this.ESPEMode.Text = "2D";
			this.ESPEMode.IndexChanged += this.ESPEMode_IndexChanged;
			this.beautyLabel37.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel37.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel37.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel37.Location = new Point(11, 306);
			this.beautyLabel37.Name = "beautyLabel37";
			this.beautyLabel37.Size = new Size(70, 18);
			this.beautyLabel37.TabIndex = 924;
			this.beautyLabel37.Text = "Box style";
			this.beautyLabel37.TextPadding = new Padding(0);
			this.beautyLabel36.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel36.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel36.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel36.Location = new Point(10, 273);
			this.beautyLabel36.Name = "beautyLabel36";
			this.beautyLabel36.Size = new Size(92, 18);
			this.beautyLabel36.TabIndex = 922;
			this.beautyLabel36.Text = "Outline color";
			this.beautyLabel36.TextPadding = new Padding(0);
			this.beautyLabel33.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel33.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel33.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel33.Location = new Point(10, 240);
			this.beautyLabel33.Name = "beautyLabel33";
			this.beautyLabel33.Size = new Size(65, 18);
			this.beautyLabel33.TabIndex = 920;
			this.beautyLabel33.Text = "Fill color";
			this.beautyLabel33.TextPadding = new Padding(0);
			this.ESPNames.AnimationSpeed = 0.6f;
			this.ESPNames.BackColor = Color.FromArgb(12, 14, 14);
			this.ESPNames.BorderColor = Color.FromArgb(20, 22, 22);
			this.ESPNames.BorderRadius = 2f;
			this.ESPNames.BorderSize = 1f;
			this.ESPNames.CheckedBorderColor = Color.Firebrick;
			this.ESPNames.CheckedFillColor = Color.Firebrick;
			this.ESPNames.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ESPNames.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.ESPNames.CheckMarkScale = 0.6f;
			this.ESPNames.FillColor = Color.FromArgb(16, 18, 18);
			this.ESPNames.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ESPNames.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ESPNames.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ESPNames.Location = new Point(228, 140);
			this.ESPNames.Name = "ESPNames";
			this.ESPNames.Size = new Size(22, 22);
			this.ESPNames.TabIndex = 918;
			this.ESPNames.TargetLabel = this.beautyLabel32;
			this.ESPNames.Text = "HealthBar";
			this.ESPNames.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.ESPNames.CheckedChanged += this.ESPNames_CheckedChanged;
			this.beautyLabel32.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel32.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel32.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel32.Location = new Point(9, 139);
			this.beautyLabel32.Name = "beautyLabel32";
			this.beautyLabel32.Size = new Size(77, 18);
			this.beautyLabel32.TabIndex = 919;
			this.beautyLabel32.Text = "Nametags";
			this.beautyLabel32.TextPadding = new Padding(0);
			this.ESPHealthbar.AnimationSpeed = 0.6f;
			this.ESPHealthbar.BackColor = Color.FromArgb(12, 14, 14);
			this.ESPHealthbar.BorderColor = Color.FromArgb(20, 22, 22);
			this.ESPHealthbar.BorderRadius = 2f;
			this.ESPHealthbar.BorderSize = 1f;
			this.ESPHealthbar.CheckedBorderColor = Color.Firebrick;
			this.ESPHealthbar.CheckedFillColor = Color.Firebrick;
			this.ESPHealthbar.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ESPHealthbar.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.ESPHealthbar.CheckMarkScale = 0.6f;
			this.ESPHealthbar.FillColor = Color.FromArgb(16, 18, 18);
			this.ESPHealthbar.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ESPHealthbar.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ESPHealthbar.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ESPHealthbar.Location = new Point(228, 110);
			this.ESPHealthbar.Name = "ESPHealthbar";
			this.ESPHealthbar.Size = new Size(22, 22);
			this.ESPHealthbar.TabIndex = 916;
			this.ESPHealthbar.TargetLabel = this.beautyLabel27;
			this.ESPHealthbar.Text = "HealthBar";
			this.ESPHealthbar.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.ESPHealthbar.CheckedChanged += this.ESPHealthbar_CheckedChanged;
			this.beautyLabel27.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel27.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel27.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel27.Location = new Point(9, 108);
			this.beautyLabel27.Name = "beautyLabel27";
			this.beautyLabel27.Size = new Size(77, 18);
			this.beautyLabel27.TabIndex = 917;
			this.beautyLabel27.Text = "Health bar";
			this.beautyLabel27.TextPadding = new Padding(0);
			this.ESPBoxes.AnimationSpeed = 0.6f;
			this.ESPBoxes.BackColor = Color.FromArgb(12, 14, 14);
			this.ESPBoxes.BorderColor = Color.FromArgb(20, 22, 22);
			this.ESPBoxes.BorderRadius = 2f;
			this.ESPBoxes.BorderSize = 1f;
			this.ESPBoxes.CheckedBorderColor = Color.Firebrick;
			this.ESPBoxes.CheckedFillColor = Color.Firebrick;
			this.ESPBoxes.CheckedForeColor = Color.FromArgb(120, 120, 130);
			this.ESPBoxes.CheckMarkColor = Color.FromArgb(25, 25, 25);
			this.ESPBoxes.CheckMarkScale = 0.6f;
			this.ESPBoxes.FillColor = Color.FromArgb(16, 18, 18);
			this.ESPBoxes.HoverBorderColor = Color.FromArgb(36, 38, 38);
			this.ESPBoxes.HoverFillColor = Color.FromArgb(26, 28, 28);
			this.ESPBoxes.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ESPBoxes.Location = new Point(228, 81);
			this.ESPBoxes.Name = "ESPBoxes";
			this.ESPBoxes.Size = new Size(22, 22);
			this.ESPBoxes.TabIndex = 914;
			this.ESPBoxes.TargetLabel = this.beautyLabel25;
			this.ESPBoxes.Text = "beautyCheckBox4";
			this.ESPBoxes.UncheckedForeColor = Color.FromArgb(40, 40, 50);
			this.ESPBoxes.CheckedChanged += this.ESPBoxes_CheckedChanged;
			this.beautyLabel25.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel25.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel25.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel25.Location = new Point(9, 78);
			this.beautyLabel25.Name = "beautyLabel25";
			this.beautyLabel25.Size = new Size(34, 18);
			this.beautyLabel25.TabIndex = 915;
			this.beautyLabel25.Text = "Box";
			this.beautyLabel25.TextPadding = new Padding(0);
			this.BindESP.AnimationSpeed = 0.6f;
			this.BindESP.BorderColor = Color.FromArgb(16, 18, 18);
			this.BindESP.BorderRadius = 4f;
			this.BindESP.BorderSize = 1f;
			this.BindESP.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.BindESP.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.BindESP.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.BindESP.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.BindESP.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.BindESP.FillColor = Color.FromArgb(16, 18, 18);
			this.BindESP.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.BindESP.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.BindESP.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.BindESP.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.BindESP.ImageOffset = new Point(0, 0);
			this.BindESP.Location = new Point(173, 53);
			this.BindESP.MinimumSize = new Size(20, 22);
			this.BindESP.MinimumTextWidth = 20;
			this.BindESP.Name = "BindESP";
			this.BindESP.Size = new Size(77, 22);
			this.BindESP.TabIndex = 913;
			this.BindESP.Text = "None";
			this.BindESP.TextOffset = new Point(0, 0);
			this.BindESP.TextPadding = new Padding(0);
			this.BindESP.YOffSet = 0;
			this.BindESP.MouseDown += this.BindButtons_MouseDown;
			this.beautyPanel5.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel5.BorderSizeBottom = 1f;
			this.beautyPanel5.BorderSizeLeft = 1f;
			this.beautyPanel5.BorderSizeRight = 1f;
			this.beautyPanel5.BorderSizeTop = 1f;
			this.beautyPanel5.Controls.Add(this.beautyLabel34);
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
			this.beautyLabel34.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel34.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel34.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel34.Location = new Point(10, 11);
			this.beautyLabel34.Name = "beautyLabel34";
			this.beautyLabel34.Size = new Size(57, 18);
			this.beautyLabel34.TabIndex = 905;
			this.beautyLabel34.Text = "Visuals";
			this.beautyLabel34.TextPadding = new Padding(0);
			this.ESPEnable.AutoRoundCorners = true;
			this.ESPEnable.BackColor = Color.FromArgb(12, 14, 14);
			this.ESPEnable.Checked = false;
			this.ESPEnable.CheckedState.BorderColor = Color.FromArgb(48, 20, 20);
			this.ESPEnable.CheckedState.BorderRadius = 4;
			this.ESPEnable.CheckedState.BorderThickness = 1;
			this.ESPEnable.CheckedState.FillColor = Color.FromArgb(48, 20, 20);
			this.ESPEnable.CheckedState.InnerBorderColor = Color.Firebrick;
			this.ESPEnable.CheckedState.InnerBorderRadius = 4;
			this.ESPEnable.CheckedState.InnerBorderThickness = 0;
			this.ESPEnable.CheckedState.InnerColor = Color.Firebrick;
			this.ESPEnable.CheckedState.InnerOffset = 2;
			this.ESPEnable.LabelCheckedColor = Color.FromArgb(120, 120, 130);
			this.ESPEnable.LabelUncheckedColor = Color.FromArgb(40, 40, 50);
			this.ESPEnable.LinkedLabel = this.beautyLabel35;
			this.ESPEnable.Location = new Point(10, 50);
			this.ESPEnable.Name = "ESPEnable";
			this.ESPEnable.Size = new Size(44, 22);
			this.ESPEnable.TabIndex = 894;
			this.ESPEnable.Text = "beautyToggleSwitch3";
			this.ESPEnable.ThumbSize = 12;
			this.ESPEnable.UncheckedState.BorderColor = Color.FromArgb(20, 22, 22);
			this.ESPEnable.UncheckedState.BorderRadius = 4;
			this.ESPEnable.UncheckedState.BorderThickness = 1;
			this.ESPEnable.UncheckedState.FillColor = Color.FromArgb(16, 18, 18);
			this.ESPEnable.UncheckedState.InnerBorderColor = Color.FromArgb(40, 40, 50);
			this.ESPEnable.UncheckedState.InnerBorderRadius = 4;
			this.ESPEnable.UncheckedState.InnerBorderThickness = 0;
			this.ESPEnable.UncheckedState.InnerColor = Color.FromArgb(40, 40, 50);
			this.ESPEnable.UncheckedState.InnerOffset = 30;
			this.ESPEnable.CheckedChanged += this.ESPEnable_CheckedChanged;
			this.beautyLabel35.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel35.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel35.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel35.Location = new Point(60, 53);
			this.beautyLabel35.Name = "beautyLabel35";
			this.beautyLabel35.Size = new Size(53, 18);
			this.beautyLabel35.TabIndex = 842;
			this.beautyLabel35.Text = "Enable";
			this.beautyLabel35.TextPadding = new Padding(0);
			this.ColorExtraAL.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorExtraAL.Cursor = Cursors.Hand;
			this.ColorExtraAL.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorExtraAL.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorExtraAL.Image = null;
			this.ColorExtraAL.Location = new Point(213, 531);
			this.ColorExtraAL.Name = "ColorExtraAL";
			this.ColorExtraAL.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorExtraAL.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorExtraAL.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorExtraAL.PopupHeaderHeight = 0;
			this.ColorExtraAL.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorExtraAL.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorExtraAL.PopupPadding = new Padding(14);
			this.ColorExtraAL.PopupSize = new Size(300, 200);
			this.ColorExtraAL.PopupSliderCornerRadius = 2;
			this.ColorExtraAL.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorExtraAL.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorExtraAL.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorExtraAL.PopupTitleIconSize = 6;
			this.ColorExtraAL.Radius = 2;
			this.ColorExtraAL.SelectedColor = Color.White;
			this.ColorExtraAL.ShowPopupTitle = false;
			this.ColorExtraAL.Size = new Size(39, 18);
			this.ColorExtraAL.SliderHoverThumbColor = Color.White;
			this.ColorExtraAL.SliderPressedThumbColor = Color.White;
			this.ColorExtraAL.SliderThumbColor = Color.White;
			this.ColorExtraAL.TabIndex = 961;
			this.ColorExtraAL.Text = "Theme Color";
			this.ColorExtraAL.SelectedColorChanged += this.ColorExtraAL_SelectedColorChanged;
			this.ColorBackgroundAL.Alpha = 0;
			this.ColorBackgroundAL.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorBackgroundAL.Cursor = Cursors.Hand;
			this.ColorBackgroundAL.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorBackgroundAL.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorBackgroundAL.Image = null;
			this.ColorBackgroundAL.Location = new Point(202, 450);
			this.ColorBackgroundAL.Name = "ColorBackgroundAL";
			this.ColorBackgroundAL.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorBackgroundAL.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorBackgroundAL.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorBackgroundAL.PopupHeaderHeight = 0;
			this.ColorBackgroundAL.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorBackgroundAL.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorBackgroundAL.PopupPadding = new Padding(14);
			this.ColorBackgroundAL.PopupSize = new Size(300, 200);
			this.ColorBackgroundAL.PopupSliderCornerRadius = 2;
			this.ColorBackgroundAL.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorBackgroundAL.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorBackgroundAL.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorBackgroundAL.PopupTitleIconSize = 6;
			this.ColorBackgroundAL.Radius = 2;
			this.ColorBackgroundAL.SelectedColor = Color.FromArgb(0, 0, 0, 0);
			this.ColorBackgroundAL.ShowPopupTitle = false;
			this.ColorBackgroundAL.Size = new Size(20, 20);
			this.ColorBackgroundAL.SliderHoverThumbColor = Color.White;
			this.ColorBackgroundAL.SliderPressedThumbColor = Color.White;
			this.ColorBackgroundAL.SliderThumbColor = Color.White;
			this.ColorBackgroundAL.TabIndex = 918;
			this.ColorBackgroundAL.Text = "Theme Color";
			this.ColorBackgroundAL.SelectedColorChanged += this.ColorBackgroundAL_SelectedColorChanged;
			this.ColorArrayListB.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayListB.Cursor = Cursors.Hand;
			this.ColorArrayListB.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorArrayListB.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorArrayListB.Image = null;
			this.ColorArrayListB.Location = new Point(213, 505);
			this.ColorArrayListB.Name = "ColorArrayListB";
			this.ColorArrayListB.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayListB.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorArrayListB.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayListB.PopupHeaderHeight = 0;
			this.ColorArrayListB.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorArrayListB.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorArrayListB.PopupPadding = new Padding(14);
			this.ColorArrayListB.PopupSize = new Size(300, 200);
			this.ColorArrayListB.PopupSliderCornerRadius = 2;
			this.ColorArrayListB.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorArrayListB.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayListB.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayListB.PopupTitleIconSize = 6;
			this.ColorArrayListB.Radius = 2;
			this.ColorArrayListB.SelectedColor = Color.DarkRed;
			this.ColorArrayListB.ShowPopupTitle = false;
			this.ColorArrayListB.Size = new Size(39, 18);
			this.ColorArrayListB.SliderHoverThumbColor = Color.White;
			this.ColorArrayListB.SliderPressedThumbColor = Color.White;
			this.ColorArrayListB.SliderThumbColor = Color.White;
			this.ColorArrayListB.TabIndex = 944;
			this.ColorArrayListB.Text = "Theme Color";
			this.ColorArrayListB.SelectedColorChanged += this.ColorArrayListB_SelectedColorChanged;
			this.ColorArrayList.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayList.Cursor = Cursors.Hand;
			this.ColorArrayList.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorArrayList.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorArrayList.Image = null;
			this.ColorArrayList.Location = new Point(213, 481);
			this.ColorArrayList.Name = "ColorArrayList";
			this.ColorArrayList.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayList.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorArrayList.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayList.PopupHeaderHeight = 0;
			this.ColorArrayList.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorArrayList.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorArrayList.PopupPadding = new Padding(14);
			this.ColorArrayList.PopupSize = new Size(300, 200);
			this.ColorArrayList.PopupSliderCornerRadius = 2;
			this.ColorArrayList.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorArrayList.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayList.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorArrayList.PopupTitleIconSize = 6;
			this.ColorArrayList.Radius = 2;
			this.ColorArrayList.SelectedColor = Color.Firebrick;
			this.ColorArrayList.ShowPopupTitle = false;
			this.ColorArrayList.Size = new Size(39, 18);
			this.ColorArrayList.SliderHoverThumbColor = Color.White;
			this.ColorArrayList.SliderPressedThumbColor = Color.White;
			this.ColorArrayList.SliderThumbColor = Color.White;
			this.ColorArrayList.TabIndex = 917;
			this.ColorArrayList.Text = "Theme Color";
			this.ColorArrayList.SelectedColorChanged += this.ColorArrayList_SelectedColorChanged;
			this.ColorOutline.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorOutline.Cursor = Cursors.Hand;
			this.ColorOutline.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorOutline.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorOutline.Image = null;
			this.ColorOutline.Location = new Point(211, 273);
			this.ColorOutline.Name = "ColorOutline";
			this.ColorOutline.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorOutline.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorOutline.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorOutline.PopupHeaderHeight = 0;
			this.ColorOutline.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorOutline.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorOutline.PopupPadding = new Padding(14);
			this.ColorOutline.PopupSize = new Size(300, 200);
			this.ColorOutline.PopupSliderCornerRadius = 2;
			this.ColorOutline.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorOutline.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorOutline.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorOutline.PopupTitleIconSize = 6;
			this.ColorOutline.Radius = 2;
			this.ColorOutline.SelectedColor = Color.Firebrick;
			this.ColorOutline.ShowPopupTitle = false;
			this.ColorOutline.Size = new Size(39, 18);
			this.ColorOutline.SliderHoverThumbColor = Color.White;
			this.ColorOutline.SliderPressedThumbColor = Color.White;
			this.ColorOutline.SliderThumbColor = Color.White;
			this.ColorOutline.TabIndex = 931;
			this.ColorOutline.Text = "Theme Color";
			this.ColorOutline.SelectedColorChanged += this.ColorOutline_SelectedColorChanged;
			this.ColorFill.BackColor = Color.FromArgb(12, 14, 14);
			this.ColorFill.Cursor = Cursors.Hand;
			this.ColorFill.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorFill.ForeColor = Color.FromArgb(40, 40, 50);
			this.ColorFill.Image = null;
			this.ColorFill.Location = new Point(211, 240);
			this.ColorFill.Name = "ColorFill";
			this.ColorFill.PopupBackColor = Color.FromArgb(12, 14, 14);
			this.ColorFill.PopupBorderColor = Color.FromArgb(20, 22, 22);
			this.ColorFill.PopupHeaderBackColor = Color.FromArgb(12, 14, 14);
			this.ColorFill.PopupHeaderHeight = 0;
			this.ColorFill.PopupHeaderIconOffset = new Point(0, 0);
			this.ColorFill.PopupHeaderTextOffset = new Point(0, 0);
			this.ColorFill.PopupPadding = new Padding(14);
			this.ColorFill.PopupSize = new Size(300, 200);
			this.ColorFill.PopupSliderCornerRadius = 2;
			this.ColorFill.PopupTitleFont = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.ColorFill.PopupTitleForeColor = Color.FromArgb(12, 14, 14);
			this.ColorFill.PopupTitleIconColor = Color.FromArgb(12, 14, 14);
			this.ColorFill.PopupTitleIconSize = 6;
			this.ColorFill.Radius = 2;
			this.ColorFill.SelectedColor = Color.Firebrick;
			this.ColorFill.ShowPopupTitle = false;
			this.ColorFill.Size = new Size(39, 18);
			this.ColorFill.SliderHoverThumbColor = Color.White;
			this.ColorFill.SliderPressedThumbColor = Color.White;
			this.ColorFill.SliderThumbColor = Color.White;
			this.ColorFill.TabIndex = 930;
			this.ColorFill.Text = "Menu accent";
			this.ColorFill.SelectedColorChanged += this.ColorFill_SelectedColorChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(12, 14, 14);
			base.Controls.Add(this.DefaultPanel);
			base.Name = "Visuals";
			base.Size = new Size(570, 410);
			this.DefaultPanel.ResumeLayout(false);
			this.beautyPanel7.ResumeLayout(false);
			this.beautyPanel7.PerformLayout();
			this.beautyPanel8.ResumeLayout(false);
			this.beautyPanel2.ResumeLayout(false);
			this.beautyPanel3.ResumeLayout(false);
			this.beautyPanel1.ResumeLayout(false);
			this.beautyPanel1.PerformLayout();
			this.beautyPanel5.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000329 RID: 809
		public Keys bindESP;

		// Token: 0x0400032A RID: 810
		public static int ESPBindInt;

		// Token: 0x0400032B RID: 811
		private Dictionary<BeautyAutoButton, Keys> currentBinds = new Dictionary<BeautyAutoButton, Keys>();

		// Token: 0x0400032C RID: 812
		private IContainer components;

		// Token: 0x0400032D RID: 813
		private BeautyPanel DefaultPanel;

		// Token: 0x0400032E RID: 814
		private BeautyPanel beautyPanel1;

		// Token: 0x0400032F RID: 815
		public BeautyComboBox ESPEMode;

		// Token: 0x04000330 RID: 816
		private BeautyLabel beautyLabel37;

		// Token: 0x04000331 RID: 817
		private BeautyLabel beautyLabel36;

		// Token: 0x04000332 RID: 818
		private BeautyLabel beautyLabel33;

		// Token: 0x04000333 RID: 819
		public BeautyCheckBox ESPNames;

		// Token: 0x04000334 RID: 820
		private BeautyLabel beautyLabel32;

		// Token: 0x04000335 RID: 821
		public BeautyCheckBox ESPHealthbar;

		// Token: 0x04000336 RID: 822
		private BeautyLabel beautyLabel27;

		// Token: 0x04000337 RID: 823
		public BeautyCheckBox ESPBoxes;

		// Token: 0x04000338 RID: 824
		private BeautyLabel beautyLabel25;

		// Token: 0x04000339 RID: 825
		private BeautyPanel beautyPanel5;

		// Token: 0x0400033A RID: 826
		private BeautyLabel beautyLabel34;

		// Token: 0x0400033B RID: 827
		public BeautyToggleSwitch ESPEnable;

		// Token: 0x0400033C RID: 828
		private BeautyLabel beautyLabel35;

		// Token: 0x0400033D RID: 829
		public BeautyCheckBox cbDrawCorners;

		// Token: 0x0400033E RID: 830
		private BeautyLabel beautyLabel1;

		// Token: 0x0400033F RID: 831
		public BeautyCheckBox cbDrawHurtTime;

		// Token: 0x04000340 RID: 832
		private BeautyLabel beautyLabel2;

		// Token: 0x04000341 RID: 833
		private BeautyPanel beautyPanel2;

		// Token: 0x04000342 RID: 834
		private BeautyPanel beautyPanel3;

		// Token: 0x04000343 RID: 835
		private BeautyLabel beautyLabel11;

		// Token: 0x04000344 RID: 836
		public BeautyToggleSwitch cbArraylist;

		// Token: 0x04000345 RID: 837
		private BeautyLabel beautyLabel12;

		// Token: 0x04000346 RID: 838
		public BeautyFlatSlider ScaleAr;

		// Token: 0x04000347 RID: 839
		private BeautyLabel beautyLabel6;

		// Token: 0x04000348 RID: 840
		private BeautyLabel beautyLabel7;

		// Token: 0x04000349 RID: 841
		public BeautyCheckBox cbBackground;

		// Token: 0x0400034A RID: 842
		private BeautyLabel beautyLabel5;

		// Token: 0x0400034B RID: 843
		private BeautyPanel beautyPanel7;

		// Token: 0x0400034C RID: 844
		private BeautyAutoButton btSprint;

		// Token: 0x0400034D RID: 845
		private BeautyPanel beautyPanel8;

		// Token: 0x0400034E RID: 846
		private BeautyLabel beautyLabel9;

		// Token: 0x0400034F RID: 847
		public BeautyToggleSwitch cbChams;

		// Token: 0x04000350 RID: 848
		private BeautyLabel beautyLabel10;

		// Token: 0x04000351 RID: 849
		public BeautyColorPicker ColorArrayList;

		// Token: 0x04000352 RID: 850
		public BeautyColorPicker ColorOutline;

		// Token: 0x04000353 RID: 851
		public BeautyColorPicker ColorFill;

		// Token: 0x04000354 RID: 852
		public BeautyComboBox Alignment;

		// Token: 0x04000355 RID: 853
		private BeautyLabel beautyLabel3;

		// Token: 0x04000356 RID: 854
		public BeautyAutoButton BindESP;

		// Token: 0x04000357 RID: 855
		public BeautyLabel lbarrayscale;

		// Token: 0x04000358 RID: 856
		public BeautyComboBox ColorModeCombo;

		// Token: 0x04000359 RID: 857
		private BeautyLabel beautyLabel4;

		// Token: 0x0400035A RID: 858
		public BeautyFlatSlider SpeedSlider;

		// Token: 0x0400035B RID: 859
		public BeautyLabel lbSpeed;

		// Token: 0x0400035C RID: 860
		private BeautyLabel beautyLabel13;

		// Token: 0x0400035D RID: 861
		public BeautyColorPicker ColorArrayListB;

		// Token: 0x0400035E RID: 862
		private BeautyLabel beautyLabel14;

		// Token: 0x0400035F RID: 863
		public BeautyFlatSlider NumericPosX;

		// Token: 0x04000360 RID: 864
		public BeautyLabel lbPosX;

		// Token: 0x04000361 RID: 865
		private BeautyLabel beautyLabel16;

		// Token: 0x04000362 RID: 866
		public BeautyFlatSlider SliderPaddingX;

		// Token: 0x04000363 RID: 867
		public BeautyLabel lbPaddingX;

		// Token: 0x04000364 RID: 868
		private BeautyLabel beautyLabel20;

		// Token: 0x04000365 RID: 869
		public BeautyFlatSlider NumericPosY;

		// Token: 0x04000366 RID: 870
		public BeautyLabel lbPosY;

		// Token: 0x04000367 RID: 871
		private BeautyLabel beautyLabel18;

		// Token: 0x04000368 RID: 872
		public BeautyColorPicker ColorBackgroundAL;

		// Token: 0x04000369 RID: 873
		public BeautyFlatSlider SliderRadius;

		// Token: 0x0400036A RID: 874
		public BeautyLabel lbRadius;

		// Token: 0x0400036B RID: 875
		private BeautyLabel beautyLabel24;

		// Token: 0x0400036C RID: 876
		public BeautyFlatSlider SliderPaddingY;

		// Token: 0x0400036D RID: 877
		public BeautyLabel lbPaddingY;

		// Token: 0x0400036E RID: 878
		private BeautyLabel beautyLabel22;

		// Token: 0x0400036F RID: 879
		public BeautyColorPicker ColorExtraAL;

		// Token: 0x04000370 RID: 880
		private BeautyLabel beautyLabel26;
	}
}
