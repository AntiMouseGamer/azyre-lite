using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azyre.Categories;
using Azyre.Utils;

namespace Azyre.MH
{
	// Token: 0x0200003F RID: 63
	public static class dllconnect
	{
		public static string LastPipeStatus = "iniciando...";

		public static string LastPipeError = "";

		public static void LogPipe(string msg)
		{
			try
			{
				File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "azyre_log.txt"), DateTime.Now.ToString("HH:mm:ss") + " " + msg + Environment.NewLine);
			}
			catch
			{
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000E85A File Offset: 0x0000CA5A
		public static void CriarServidorPipe()
		{
			Task.Run(delegate()
			{
				for (;;)
				{
					try
					{
						if (dllconnect.pipeServer != null)
						{
							try
							{
								dllconnect.pipeServer.Dispose();
							}
							catch
							{
							}
							dllconnect.pipeServer = null;
						}
						dllconnect.LastPipeStatus = "aguardando DLL...";
						dllconnect.LogPipe("pipe criado, aguardando conexao da DLL...");
						dllconnect.pipeServer = new NamedPipeServerStream("azyre", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
						dllconnect.pipeServer.WaitForConnection();
						dllconnect.LastPipeStatus = "DLL conectada";
						dllconnect.LogPipe("DLL conectou no pipe.");
						for (int i = 0; i < 40; i++)
						{
							if (dllconnect.pipeServer == null || !dllconnect.pipeServer.IsConnected)
							{
								break;
							}
							if (dllconnect.TryEnviarConfiguracoes())
							{
								break;
							}
							Task.Delay(500).Wait();
						}
						while (dllconnect.pipeServer != null && dllconnect.pipeServer.IsConnected)
						{
							Task.Delay(1000).Wait();
						}
						dllconnect.LastPipeStatus = "DLL desconectou, recriando pipe...";
						dllconnect.LogPipe("pipe desconectado, recriando servidor...");
						Task.Delay(1000).Wait();
					}
					catch (Exception ex)
					{
						dllconnect.LastPipeError = ex.Message;
						dllconnect.LogPipe("ERRO pipe: " + ex.GetType().Name + ": " + ex.Message);
						Task.Delay(2000).Wait();
					}
				}
			});
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000E884 File Offset: 0x0000CA84
		public static void EnviarConfiguracoes()
		{
			dllconnect.TryEnviarConfiguracoes();
		}

		public static bool TryEnviarConfiguracoes()
		{
			if (dllconnect.pipeServer == null || !dllconnect.pipeServer.IsConnected)
			{
				dllconnect.LastPipeError = "pipe desconectado";
				return false;
			}
			try
			{
				string s = JsonSerializer.Serialize(new
					{
						LicenseKey = Environment.UserName,
						Destruct = Destruct.destruct,
						LeftClicker = new
						{
							Enabled = Combat.Static.ClickerEnable.Checked,
							Average = (double)Combat.Static.CPS_Slider.Value / 10.0,
							BreakBlock = Combat.Static.cbBreak.Checked,
							OnlyWeapon = Combat.Static.Weapon.Checked,
							Randomize = Combat.Static.Randomize.Checked,
							Inventory = Combat.Static.cbInventory.Checked
						},
						Reach = new
						{
							Enabled = Combat.Static.ReachEnable.Checked,
							Distance = (float)Combat.Static.DistanceSlider.Value / 100f,
							Hitbox = (float)Combat.Static.HitBoxSl.Value / 100f,
							OnlyWeapon = Combat.Static.cbReachWeapon.Checked,
							WallCheck = Combat.Static.cbWallCheck.Checked
						},
						Velocity = new
						{
							Enabled = Combat.Static.VelocityEnable.Checked,
							Horizontal = (double)Combat.Static.VelocityHrz.Value / 10.0,
							Vertical = (double)Combat.Static.VelocityVrt.Value / 10.0,
							Chance = Combat.Static.ChanceSlider.Value,
							Delay = Combat.Static.ticksvl.Value,
							Moving = Combat.Static.VelocityMovingOnly.Checked,
							OnlyAttacking = Combat.Static.cbAttacking.Checked,
							OnlyTargeting = Combat.Static.cbTargeting.Checked
						},
						AimAssist = new
						{
							Enabled = Combat.Static.AimEnable.Checked,
							Distance = (float)Combat.Static.slideDistance.Value / 100f,
							Fov = Combat.Static.AimAssistFovSlider.Value,
							SpeedHorizontal = (float)Combat.Static.slidehorizontalaim.Value / 10f,
							SpeedVertical = (float)Combat.Static.slideverticalaim.Value / 10f,
							Vertical = Combat.Static.cbVertical.Checked,
							ClosestHitbox = Combat.Static.cbHitboxClosest.Checked,
							OnlyWeapon = Combat.Static.AimAssistOnlyWeapon.Checked,
							ThroughWall = Combat.Static.AimAssistThroughWall.Checked,
							ClickingOnly = Combat.Static.AimAssistClickingOnly.Checked,
							LockTarget = Combat.Static.cbLockTarget.Checked,
							BreakBlock = Combat.Static.cbAimBreakBlocks.Checked,
							Mode = Combat.Static.cbMouseMove.Checked
						},
						ESP = new
						{
							Enabled = Visuals.Static.ESPEnable.Checked,
							Boxes = Visuals.Static.ESPBoxes.Checked,
							Mode = Visuals.Static.ESPEMode.SelectedIndex,
							DrawCorners = Visuals.Static.cbDrawCorners.Checked,
							Healthbar = Visuals.Static.ESPHealthbar.Checked,
							Names = Visuals.Static.ESPNames.Checked,
							DrawHurtTime = Visuals.Static.cbDrawHurtTime.Checked,
							Outline = Visuals.Static.cbDrawCorners.Checked,
							FilledColor = new float[]
							{
								(float)Visuals.Static.ColorFill.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorFill.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorFill.SelectedColor.B / 255f
							},
							OutlineColor = new float[]
							{
								(float)Visuals.Static.ColorOutline.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorOutline.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorOutline.SelectedColor.B / 255f
							}
						},
						RightClicker = new
						{
							Enabled = Utilities.Static.cbEnabled.Checked,
							Average = (double)Utilities.Static.SliderRight.Value / 10.0,
							OnlyBlock = Utilities.Static.cbBlock.Checked
						},
						NoJumpDelay = new
						{
							Enabled = Movement.Static.cbJumpDelay.Checked
						},
						BridgeAssist = new
						{
							Enabled = Movement.Static.cbBridge.Checked,
							EdgeOffset = Movement.Static.edgeOffset.Value,
							UnsneakDelay = Movement.Static.unsneakDelay.Value,
							Randomize = Movement.Static.cbRandomize.Checked,
							SneakOnJump = Movement.Static.cbSneakOnJump.Checked,
							SneakKeyPressed = Movement.Static.cbSneakKeyPressed.Checked,
							HoldingBlocks = Movement.Static.cbHoldingBlocks.Checked,
							LookingDown = Movement.Static.cbLookingDown.Checked,
							AutoSwap = Movement.Static.cbAutoSwap.Checked
						},
						Sprint = new
						{
							Enabled = Movement.Static.cbSprint.Checked
						},
						ArrayList = new
						{
							Enabled = Visuals.Static.cbArraylist.Checked,
							Scale = (float)Visuals.Static.ScaleAr.Value / 100f,
							ShowBackground = Visuals.Static.cbBackground.Checked,
							Mode = Visuals.Static.Alignment.SelectedIndex,
							ColorMode = Visuals.Static.ColorModeCombo.SelectedIndex,
							Speed = (float)Visuals.Static.SpeedSlider.Value / 100f,
							PosX = (float)Visuals.Static.NumericPosX.Value / 10f,
							PosY = (float)Visuals.Static.NumericPosY.Value / 10f,
							PaddingX = (float)Visuals.Static.SliderPaddingX.Value / 10f,
							PaddingY = (float)Visuals.Static.SliderPaddingY.Value / 10f,
							Radius = (float)Visuals.Static.SliderRadius.Value / 10f,
							Color = new float[]
							{
								(float)Visuals.Static.ColorArrayList.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorArrayList.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorArrayList.SelectedColor.B / 255f,
								(float)Visuals.Static.ColorArrayList.SelectedColor.A / 255f
							},
							ColorB = new float[]
							{
								(float)Visuals.Static.ColorArrayListB.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorArrayListB.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorArrayListB.SelectedColor.B / 255f,
								(float)Visuals.Static.ColorArrayListB.SelectedColor.A / 255f
							},
							BackgroundColor = new float[]
							{
								(float)Visuals.Static.ColorBackgroundAL.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorBackgroundAL.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorBackgroundAL.SelectedColor.B / 255f,
								(float)Visuals.Static.ColorBackgroundAL.SelectedColor.A / 255f
							},
							ExtraInfoColor = new float[]
							{
								(float)Visuals.Static.ColorExtraAL.SelectedColor.R / 255f,
								(float)Visuals.Static.ColorExtraAL.SelectedColor.G / 255f,
								(float)Visuals.Static.ColorExtraAL.SelectedColor.B / 255f,
								(float)Visuals.Static.ColorExtraAL.SelectedColor.A / 255f
							}
						},
						Chams = new
						{
							Enabled = Visuals.Static.cbChams.Checked
						},
						NoHitDelay = new
						{
							Enabled = Utilities.Static.cbHitDelay.Checked
						},
						Teams = new
						{
							Enabled = Utilities.Static.cbTeams.Checked
						},
						Antibot = new
						{
							Enabled = Utilities.Static.cbAntibot.Checked
						},
						SprintReset = new
						{
							Enabled = Combat.Static.cbSprintReset.Checked,
							Chance = Combat.Static.numSprintResetChance.Value,
							Delay = Combat.Static.numSprintResetMinRePress.Value,
							StopDuration = Combat.Static.numSprintResetMaxRePress.Value,
							Mode = Combat.Static.ModeSprint.SelectedIndex,
							ModeType = Combat.Static.ModeTypeSprint.SelectedIndex
						},
						JumpReset = new
						{
							Enabled = Combat.Static.cbJumpReset.Checked,
							Chance = Combat.Static.numJumpResetChance.Value,
							Delay = Combat.Static.numJumpResetDelay.Value,
							JumpDuration = Combat.Static.numJumpResetDuration.Value
						}
					}, new JsonSerializerOptions
					{
						WriteIndented = false
					});
					byte[] array = dllconnect.AesEncrypt(Encoding.UTF8.GetBytes(s), dllconnect.AES_KEY, dllconnect.AES_IV);
					dllconnect.pipeServer.Write(array, 0, array.Length);
					dllconnect.pipeServer.Flush();
					dllconnect.LastPipeStatus = "config enviada (" + array.Length + " bytes)";
					dllconnect.LastPipeError = "";
					dllconnect.LogPipe("config enviada: " + array.Length + " bytes criptografados.");
					return true;
				}
				catch (Exception ex)
				{
					dllconnect.LastPipeError = ex.GetType().Name + ": " + ex.Message;
					dllconnect.LogPipe("ERRO envio: " + ex.GetType().Name + ": " + ex.Message);
					return false;
				}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000F198 File Offset: 0x0000D398
		private static byte[] AesEncrypt(byte[] data, byte[] key, byte[] iv)
		{
			byte[] result;
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV))
				{
					result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
				}
			}
			return result;
		}

		// Token: 0x0400018A RID: 394
		private static readonly byte[] AES_KEY = new byte[]
		{
			18,
			84,
			136,
			161,
			127,
			99,
			74,
			221,
			20,
			34,
			152,
			188,
			63,
			88,
			113,
			51
		};

		// Token: 0x0400018B RID: 395
		private static readonly byte[] AES_IV = new byte[]
		{
			154,
			49,
			193,
			8,
			82,
			196,
			237,
			187,
			87,
			22,
			98,
			68,
			17,
			160,
			251,
			85
		};

		// Token: 0x0400018C RID: 396
		public static NamedPipeServerStream pipeServer;
	}
}
