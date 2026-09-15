using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azyre.MH;
using Azyre.Utils;
using BeautyUI;
using BeautyUI.Controls;

namespace Azyre.Categories
{
	// Token: 0x0200004F RID: 79
	public class Destruct : UserControl
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0001CDBE File Offset: 0x0001AFBE
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0001CDC5 File Offset: 0x0001AFC5
		public static Destruct Static { get; set; }

		// Token: 0x060002C4 RID: 708 RVA: 0x0001CDD0 File Offset: 0x0001AFD0
		public string UnixTimeToDateTime(long unixtime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime = dateTime.AddSeconds((double)unixtime).ToLocalTime();
			return dateTime.ToString("MM/dd/yyyy HH:mm");
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0001CE10 File Offset: 0x0001B010
		public Destruct()
		{
			this.InitializeComponent();
			Destruct.Static = this;
			this.ConfigsCombo.BringToFront();
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			foreach (string text in Directory.GetFiles(path))
			{
				if (text.EndsWith(".config") && !(File.ReadAllText(text) == string.Empty))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					try
					{
						this.ConfigsCombo.AddItem(fileNameWithoutExtension, true);
					}
					catch
					{
					}
				}
			}
			this.labelVersion.Text = "Version: 1.0";
			this.labelExpiry.Text = "Expires on: Lifetime";
		}

		// Token: 0x060002C6 RID: 710
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x060002C7 RID: 711
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		// Token: 0x060002C8 RID: 712
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

		// Token: 0x060002C9 RID: 713
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x060002CA RID: 714
		[DllImport("kernel32.dll")]
		public static extern void GetSystemInfo(out Destruct.SYSTEM_INFO lpSystemInfo);

		// Token: 0x060002CB RID: 715
		[DllImport("kernel32.dll")]
		public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out Destruct.MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

		// Token: 0x060002CC RID: 716 RVA: 0x0001CF9C File Offset: 0x0001B19C
		private static Process GetProcessByName(string processName)
		{
			Process[] processesByName = Process.GetProcessesByName(processName);
			if (processesByName.Length != 0)
			{
				return processesByName[0];
			}
			Process result = null;
			try
			{
				string queryString = "SELECT ProcessId FROM Win32_Service WHERE Name='" + processName + "'";
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("root\\CIMV2", queryString).Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						result = Process.GetProcessById((int)((uint)enumerator.Current["ProcessId"]));
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0001D038 File Offset: 0x0001B238
		private static ServiceController GetServiceByName(string serviceName)
		{
			foreach (ServiceController serviceController in ServiceController.GetServices())
			{
				if (serviceController.ServiceName.StartsWith(serviceName, StringComparison.OrdinalIgnoreCase))
				{
					return serviceController;
				}
			}
			return null;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001D070 File Offset: 0x0001B270
		private static int GetProcessIdFromService(ServiceController service)
		{
			using (ManagementObject managementObject = new ManagementObject("Win32_Service.Name='" + service.ServiceName + "'"))
			{
				object obj = managementObject["ProcessId"];
				int result;
				if (obj != null && int.TryParse(obj.ToString(), out result))
				{
					return result;
				}
			}
			return 0;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0001D0DC File Offset: 0x0001B2DC
		public static void Run()
		{
			// KeyAuth removido: limpeza de memoria do KeyAuth desnecessaria.
			return;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001D1D8 File Offset: 0x0001B3D8
		public static bool CanAccessProcess(Process process)
		{
			bool result;
			try
			{
				IntPtr intPtr = Destruct.OpenProcess(48, false, process.Id);
				if (intPtr == IntPtr.Zero)
				{
					result = false;
				}
				else
				{
					Destruct.CloseHandle(intPtr);
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001D228 File Offset: 0x0001B428
		private static bool IsServiceRunning(string serviceName)
		{
			foreach (ServiceController serviceController in ServiceController.GetServices())
			{
				if (serviceController.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
				{
					return serviceController.Status == ServiceControllerStatus.Running;
				}
			}
			return false;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001D268 File Offset: 0x0001B468
		private static Process GetAssociatedProcessFromService(string serviceName)
		{
			ServiceController serviceByName = Destruct.GetServiceByName(serviceName);
			if (serviceByName == null)
			{
				return null;
			}
			int processIdFromService = Destruct.GetProcessIdFromService(serviceByName);
			if (processIdFromService <= 0)
			{
				return null;
			}
			return Process.GetProcessById(processIdFromService);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0001D294 File Offset: 0x0001B494
		private static Destruct.CliArgs CreateCliArgs(string searchString)
		{
			return new Destruct.CliArgs
			{
				searchterm = new List<string>
				{
					searchString
				},
				prepostfix = 10,
				delay = 1000,
				mode = "stdio"
			};
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0001D2CC File Offset: 0x0001B4CC
		public static Dictionary<long, string> memScanString(Process process, Destruct.CliArgs myargs)
		{
			IntPtr intPtr = Destruct.OpenProcess(1040, false, process.Id);
			Destruct.SYSTEM_INFO system_INFO = default(Destruct.SYSTEM_INFO);
			Destruct.GetSystemInfo(out system_INFO);
			IntPtr lpAddress = system_INFO.minimumApplicationAddress;
			IntPtr maximumApplicationAddress = system_INFO.maximumApplicationAddress;
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			while (lpAddress.ToInt64() < maximumApplicationAddress.ToInt64())
			{
				Destruct.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION;
				Destruct.VirtualQueryEx(intPtr, lpAddress, out memory_BASIC_INFORMATION, Marshal.SizeOf(typeof(Destruct.MEMORY_BASIC_INFORMATION)));
				if (memory_BASIC_INFORMATION.Protect == 4 && memory_BASIC_INFORMATION.State == 4096)
				{
					byte[] array = new byte[(int)memory_BASIC_INFORMATION.RegionSize];
					int num;
					Destruct.ReadProcessMemory(intPtr.ToInt32(), memory_BASIC_INFORMATION.BaseAddress, array, (int)memory_BASIC_INFORMATION.RegionSize, out num);
					Encoding.Default.GetString(array);
					foreach (string input in myargs.searchterm)
					{
						foreach (byte[] array2 in Destruct.EncodeBuffer(input))
						{
							int num2 = 0;
							while ((num2 = Destruct.IndexOf(array, array2, num2)) != -1)
							{
								IntPtr intPtr2 = (IntPtr)((long)memory_BASIC_INFORMATION.BaseAddress + (long)num2);
								int count = array2.Length;
								long key = intPtr2.ToInt64();
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, Encoding.Default.GetString(array, num2, count));
								}
								num2 += array2.Length;
							}
						}
					}
				}
				long num3 = memory_BASIC_INFORMATION.RegionSize.ToInt64();
				if (num3 > 2147483647L)
				{
					num3 = 2147483647L;
				}
				lpAddress = IntPtr.Add(memory_BASIC_INFORMATION.BaseAddress, (int)num3);
			}
			Destruct.CloseHandle(intPtr);
			return dictionary;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0001D4C8 File Offset: 0x0001B6C8
		public static int IndexOf(byte[] haystack, byte[] needle, int start = 0)
		{
			for (int i = start; i <= haystack.Length - needle.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < needle.Length; j++)
				{
					if (haystack[i + j] != needle[j])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001D50C File Offset: 0x0001B70C
		public static List<byte[]> EncodeBuffer(string input)
		{
			List<Encoding> list = new List<Encoding>();
			list.Add(Encoding.UTF8);
			list.Add(Encoding.ASCII);
			list.Add(Encoding.Unicode);
			list.Add(Encoding.Default);
			List<byte[]> list2 = new List<byte[]>();
			foreach (Encoding encoding in list)
			{
				list2.Add(encoding.GetBytes(input));
			}
			return list2;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001D598 File Offset: 0x0001B798
		public static void ReplaceStringInProcessMemory(Process process, Dictionary<long, string> targetStrings)
		{
			foreach (KeyValuePair<long, string> keyValuePair in targetStrings)
			{
				long key = keyValuePair.Key;
				string value = keyValuePair.Value;
				byte[] bytes = Encoding.Default.GetBytes(value);
				byte[] array = new byte[bytes.Length];
				int num;
				if (Destruct.ReadProcessMemory(process.Handle.ToInt32(), (IntPtr)key, array, array.Length, out num) && bytes.SequenceEqual(array))
				{
					byte[] array2 = new byte[bytes.Length];
					int num2;
					Destruct.WriteProcessMemory(process.Handle.ToInt32(), (IntPtr)key, array2, (uint)array2.Length, out num2);
				}
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001D66C File Offset: 0x0001B86C
		private void btDestruct_Click(object sender, EventArgs e)
		{
			try
			{
				Destruct.destruct = true;
				dllconnect.EnviarConfiguracoes();
				MessageBox.Show("Destruct flag enabled.", "Destruct", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch
			{
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
		private void SetLeftClickerProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						if (!(prop2 == "Bind"))
						{
							return;
						}
						Combat.ClickerBindInt = int.Parse(val);
						Keys clickerBindInt = (Keys)Combat.ClickerBindInt;
						Combat.Static.bindClicker = clickerBindInt;
						Combat @static = Combat.Static;
						BeautyAutoButton clickerBindButton = Combat.Static.ClickerBindButton;
						BeautyToggleSwitch clickerEnable = Combat.Static.ClickerEnable;
						Keys bindKey = clickerBindInt;
						Action onToggle;
						if ((onToggle = cachedToggle) == null)
						{
							onToggle = (cachedToggle = delegate()
							{
								Imports.Checkar(this, Combat.Static.ClickerEnable);
							});
						}
						@static.RegisterBind(clickerBindButton, clickerEnable, bindKey, onToggle);
						break;
					}
					case 5:
					case 6:
					case 8:
						break;
					case 7:
					{
						char c = prop2[0];
						if (c != 'A')
						{
							if (c != 'E')
							{
								return;
							}
							if (!(prop2 == "Enabled"))
							{
								return;
							}
							Combat.Static.ClickerEnable.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Average"))
							{
								return;
							}
							Combat.Static.CPS_Slider.Value = (int)(double.Parse(val) * 10.0);
							return;
						}
						break;
					}
					case 9:
					{
						char c = prop2[0];
						if (c != 'I')
						{
							if (c != 'R')
							{
								return;
							}
							if (!(prop2 == "Randomize"))
							{
								return;
							}
							Combat.Static.Randomize.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Inventory"))
							{
								return;
							}
							Combat.Static.cbInventory.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 10:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'O')
							{
								return;
							}
							if (!(prop2 == "OnlyWeapon"))
							{
								return;
							}
							Combat.Static.Weapon.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "BreakBlock"))
							{
								return;
							}
							Combat.Static.cbBreak.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		private void SetRightClickerProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 == "Enabled")
				{
					Utilities.Static.cbEnabled.Checked = bool.Parse(val);
					return;
				}
				if (prop2 == "Average")
				{
					Utilities.Static.SliderRight.Value = (int)(double.Parse(val) * 10.0);
					return;
				}
				if (prop2 == "OnlyBlock")
				{
					Utilities.Static.cbBlock.Checked = bool.Parse(val);
					return;
				}
				if (!(prop2 == "Bind"))
				{
					return;
				}
				Utilities.RightClickBindInt = int.Parse(val);
				Keys rightClickBindInt = (Keys)Utilities.RightClickBindInt;
				Utilities.Static.bindRightClick = rightClickBindInt;
				Utilities @static = Utilities.Static;
				BeautyAutoButton bindRight = Utilities.Static.BindRight;
				BeautyToggleSwitch cbEnabled = Utilities.Static.cbEnabled;
				Keys bindKey = rightClickBindInt;
				Action onToggle;
				if ((onToggle = cachedToggle) == null)
				{
					onToggle = (cachedToggle = delegate()
					{
						Imports.Checkar(this, Utilities.Static.cbEnabled);
					});
				}
				@static.RegisterBind(bindRight, cbEnabled, bindKey, onToggle);
			}));
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001D71C File Offset: 0x0001B91C
		private void SetBridgeProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						if (!(prop2 == "Bind"))
						{
							return;
						}
						Movement.BridgeBindInt = int.Parse(val);
						Keys bridgeBindInt = (Keys)Movement.BridgeBindInt;
						Movement.Static.bindBridgeKey = bridgeBindInt;
						Movement @static = Movement.Static;
						BeautyAutoButton bindBridge = Movement.Static.bindBridge;
						BeautyToggleSwitch cbBridge = Movement.Static.cbBridge;
						Keys bindKey = bridgeBindInt;
						Action onToggle;
						if ((onToggle = cachedToggle) == null)
						{
							onToggle = (cachedToggle = delegate()
							{
								Imports.Checkar(this, Movement.Static.cbBridge);
							});
						}
						@static.RegisterBind(bindBridge, cbBridge, bindKey, onToggle);
						break;
					}
					case 5:
					case 6:
					case 14:
						break;
					case 7:
						if (!(prop2 == "Enabled"))
						{
							return;
						}
						Movement.Static.cbBridge.Checked = bool.Parse(val);
						return;
					case 8:
						if (!(prop2 == "AutoSwap"))
						{
							return;
						}
						Movement.Static.cbAutoSwap.Checked = bool.Parse(val);
						return;
					case 9:
						if (!(prop2 == "Randomize"))
						{
							return;
						}
						Movement.Static.cbRandomize.Checked = bool.Parse(val);
						return;
					case 10:
						if (!(prop2 == "EdgeOffset"))
						{
							return;
						}
						Movement.Static.edgeOffset.Value = int.Parse(val);
						return;
					case 11:
					{
						char c = prop2[0];
						if (c != 'L')
						{
							if (c != 'S')
							{
								return;
							}
							if (!(prop2 == "SneakOnJump"))
							{
								return;
							}
							Movement.Static.cbSneakOnJump.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "LookingDown"))
							{
								return;
							}
							Movement.Static.cbLookingDown.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 12:
						if (!(prop2 == "UnsneakDelay"))
						{
							return;
						}
						Movement.Static.unsneakDelay.Value = int.Parse(val);
						return;
					case 13:
						if (!(prop2 == "HoldingBlocks"))
						{
							return;
						}
						Movement.Static.cbHoldingBlocks.Checked = bool.Parse(val);
						return;
					case 15:
						if (!(prop2 == "SneakKeyPressed"))
						{
							return;
						}
						Movement.Static.cbSneakKeyPressed.Checked = bool.Parse(val);
						return;
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0001D758 File Offset: 0x0001B958
		private void SetArrayListProperty(string prop, string val)
		{
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						char c = prop2[3];
						if (c != 'X')
						{
							if (c != 'Y')
							{
								if (c != 'e')
								{
									return;
								}
								if (!(prop2 == "Mode"))
								{
									return;
								}
								Visuals.Static.Alignment.SelectedIndex = int.Parse(val);
								return;
							}
							else
							{
								if (!(prop2 == "PosY"))
								{
									return;
								}
								Visuals.Static.NumericPosY.Value = (int)((decimal)(float.Parse(val, CultureInfo.InvariantCulture) * 10f));
								return;
							}
						}
						else
						{
							if (!(prop2 == "PosX"))
							{
								return;
							}
							Visuals.Static.NumericPosX.Value = (int)((decimal)(float.Parse(val, CultureInfo.InvariantCulture) * 10f));
							return;
						}
						break;
					}
					case 5:
					{
						char c = prop2[1];
						if (c != 'c')
						{
							if (c != 'o')
							{
								if (c != 'p')
								{
									return;
								}
								if (!(prop2 == "Speed"))
								{
									return;
								}
								Visuals.Static.SpeedSlider.Value = (int)(float.Parse(val, CultureInfo.InvariantCulture) * 100f);
								return;
							}
							else
							{
								if (!(prop2 == "Color"))
								{
									return;
								}
								string[] array = val.Split(new char[]
								{
									','
								});
								Visuals.Static.ColorArrayList.SelectedColor = Color.FromArgb((int)(float.Parse(array[3], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array[2], CultureInfo.InvariantCulture) * 255f));
								return;
							}
						}
						else
						{
							if (!(prop2 == "Scale"))
							{
								return;
							}
							Visuals.Static.ScaleAr.Value = (int)(float.Parse(val, CultureInfo.InvariantCulture) * 100f);
							return;
						}
						break;
					}
					case 6:
					{
						char c = prop2[0];
						if (c != 'C')
						{
							if (c != 'R')
							{
								return;
							}
							if (!(prop2 == "Radius"))
							{
								return;
							}
							Visuals.Static.SliderRadius.Value = (int)(float.Parse(val, CultureInfo.InvariantCulture) * 10f);
							return;
						}
						else
						{
							if (!(prop2 == "ColorB"))
							{
								return;
							}
							string[] array2 = val.Split(new char[]
							{
								','
							});
							Visuals.Static.ColorArrayListB.SelectedColor = Color.FromArgb((int)(float.Parse(array2[3], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array2[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array2[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array2[2], CultureInfo.InvariantCulture) * 255f));
							return;
						}
						break;
					}
					case 7:
						if (!(prop2 == "Enabled"))
						{
							return;
						}
						Visuals.Static.cbArraylist.Checked = bool.Parse(val);
						return;
					case 8:
					{
						char c = prop2[7];
						if (c != 'X')
						{
							if (c != 'Y')
							{
								return;
							}
							if (!(prop2 == "PaddingY"))
							{
								return;
							}
							Visuals.Static.SliderPaddingY.Value = (int)(float.Parse(val, CultureInfo.InvariantCulture) * 10f);
							return;
						}
						else
						{
							if (!(prop2 == "PaddingX"))
							{
								return;
							}
							Visuals.Static.SliderPaddingX.Value = (int)(float.Parse(val, CultureInfo.InvariantCulture) * 10f);
							return;
						}
						break;
					}
					case 9:
						if (!(prop2 == "ColorMode"))
						{
							return;
						}
						Visuals.Static.ColorModeCombo.SelectedIndex = int.Parse(val);
						return;
					case 10:
					case 11:
					case 12:
					case 13:
						break;
					case 14:
					{
						char c = prop2[0];
						if (c != 'E')
						{
							if (c != 'S')
							{
								return;
							}
							if (!(prop2 == "ShowBackground"))
							{
								return;
							}
							Visuals.Static.cbBackground.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "ExtraInfoColor"))
							{
								return;
							}
							string[] array3 = val.Split(new char[]
							{
								','
							});
							Visuals.Static.ColorExtraAL.SelectedColor = Color.FromArgb((int)(float.Parse(array3[3], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array3[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array3[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array3[2], CultureInfo.InvariantCulture) * 255f));
						}
						break;
					}
					case 15:
					{
						if (!(prop2 == "BackgroundColor"))
						{
							return;
						}
						string[] array4 = val.Split(new char[]
						{
							','
						});
						Visuals.Static.ColorBackgroundAL.SelectedColor = Color.FromArgb((int)(float.Parse(array4[3], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array4[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array4[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array4[2], CultureInfo.InvariantCulture) * 255f));
						return;
					}
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0001D78C File Offset: 0x0001B98C
		private void SetAimAssistProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 3:
						if (!(prop2 == "Fov"))
						{
							return;
						}
						Combat.Static.AimAssistFovSlider.Value = int.Parse(val);
						return;
					case 4:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'M')
							{
								return;
							}
							if (!(prop2 == "Mode"))
							{
								return;
							}
							Combat.Static.cbMouseMove.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Bind"))
							{
								return;
							}
							Combat.AimBindInt = int.Parse(val);
							Keys aimBindInt = (Keys)Combat.AimBindInt;
							Combat.Static.bindAim = aimBindInt;
							Combat @static = Combat.Static;
							BeautyAutoButton aimBindButton = Combat.Static.AimBindButton;
							BeautyToggleSwitch aimEnable = Combat.Static.AimEnable;
							Keys bindKey = aimBindInt;
							Action onToggle;
							if ((onToggle = cachedToggle) == null)
							{
								onToggle = (cachedToggle = delegate()
								{
									Imports.Checkar(this, Combat.Static.AimEnable);
								});
							}
							@static.RegisterBind(aimBindButton, aimEnable, bindKey, onToggle);
						}
						break;
					}
					case 5:
					case 6:
					case 9:
					case 14:
						break;
					case 7:
						if (!(prop2 == "Enabled"))
						{
							return;
						}
						Combat.Static.AimEnable.Checked = bool.Parse(val);
						return;
					case 8:
					{
						char c = prop2[0];
						if (c != 'D')
						{
							if (c != 'V')
							{
								return;
							}
							if (!(prop2 == "Vertical"))
							{
								return;
							}
							Combat.Static.cbVertical.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Distance"))
							{
								return;
							}
							Combat.Static.slideDistance.Value = (int)(float.Parse(val) * 100f);
							return;
						}
						break;
					}
					case 10:
					{
						char c = prop2[0];
						if (c != 'L')
						{
							if (c != 'O')
							{
								return;
							}
							if (!(prop2 == "OnlyWeapon"))
							{
								return;
							}
							Combat.Static.AimAssistOnlyWeapon.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "LockTarget"))
							{
								return;
							}
							Combat.Static.cbLockTarget.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 11:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'T')
							{
								return;
							}
							if (!(prop2 == "ThroughWall"))
							{
								return;
							}
							Combat.Static.AimAssistThroughWall.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "BreakBlocks"))
							{
								return;
							}
							Combat.Static.cbAimBreakBlocks.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 12:
						if (!(prop2 == "ClickingOnly"))
						{
							return;
						}
						Combat.Static.AimAssistClickingOnly.Checked = bool.Parse(val);
						return;
					case 13:
					{
						char c = prop2[0];
						if (c != 'C')
						{
							if (c != 'S')
							{
								return;
							}
							if (!(prop2 == "SpeedVertical"))
							{
								return;
							}
							Combat.Static.slideverticalaim.Value = (int)(float.Parse(val) * 10f);
							return;
						}
						else
						{
							if (!(prop2 == "ClosestHitbox"))
							{
								return;
							}
							Combat.Static.cbHitboxClosest.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 15:
						if (!(prop2 == "SpeedHorizontal"))
						{
							return;
						}
						Combat.Static.slidehorizontalaim.Value = (int)(float.Parse(val) * 10f);
						return;
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001D7C8 File Offset: 0x0001B9C8
		private void SetReachProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 == "Enabled")
				{
					Combat.Static.ReachEnable.Checked = bool.Parse(val);
					return;
				}
				if (prop2 == "Distance")
				{
					Combat.Static.DistanceSlider.Value = (int)(float.Parse(val) * 100f);
					return;
				}
				if (prop2 == "Hitbox")
				{
					Combat.Static.HitBoxSl.Value = (int)(float.Parse(val) * 100f);
					return;
				}
				if (prop2 == "OnlyWeapon")
				{
					Combat.Static.cbReachWeapon.Checked = bool.Parse(val);
					return;
				}
				if (prop2 == "WallCheck")
				{
					Combat.Static.cbWallCheck.Checked = bool.Parse(val);
					return;
				}
				if (!(prop2 == "Bind"))
				{
					return;
				}
				Combat.ReachBindInt = int.Parse(val);
				Keys reachBindInt = (Keys)Combat.ReachBindInt;
				Combat.Static.bindReach = reachBindInt;
				Combat @static = Combat.Static;
				BeautyAutoButton reachBindButton = Combat.Static.ReachBindButton;
				BeautyToggleSwitch reachEnable = Combat.Static.ReachEnable;
				Keys bindKey = reachBindInt;
				Action onToggle;
				if ((onToggle = cachedToggle) == null)
				{
					onToggle = (cachedToggle = delegate()
					{
						Imports.Checkar(this, Combat.Static.ReachEnable);
					});
				}
				@static.RegisterBind(reachBindButton, reachEnable, bindKey, onToggle);
			}));
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001D804 File Offset: 0x0001BA04
		private void SetVelocityProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						if (!(prop2 == "Bind"))
						{
							return;
						}
						Combat.VelocityBindInt = int.Parse(val);
						Keys velocityBindInt = (Keys)Combat.VelocityBindInt;
						Combat.Static.bindVelocity = velocityBindInt;
						Combat @static = Combat.Static;
						BeautyAutoButton velocityBindButton = Combat.Static.VelocityBindButton;
						BeautyToggleSwitch velocityEnable = Combat.Static.VelocityEnable;
						Keys bindKey = velocityBindInt;
						Action onToggle;
						if ((onToggle = cachedToggle) == null)
						{
							onToggle = (cachedToggle = delegate()
							{
								Imports.Checkar(this, Combat.Static.VelocityEnable);
							});
						}
						@static.RegisterBind(velocityBindButton, velocityEnable, bindKey, onToggle);
						break;
					}
					case 5:
						if (!(prop2 == "Delay"))
						{
							return;
						}
						Combat.Static.ticksvl.Value = int.Parse(val);
						return;
					case 6:
					{
						char c = prop2[0];
						if (c != 'C')
						{
							if (c != 'M')
							{
								return;
							}
							if (!(prop2 == "Moving"))
							{
								return;
							}
							Combat.Static.VelocityMovingOnly.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Chance"))
							{
								return;
							}
							Combat.Static.ChanceSlider.Value = int.Parse(val);
							return;
						}
						break;
					}
					case 7:
						if (!(prop2 == "Enabled"))
						{
							return;
						}
						Combat.Static.VelocityEnable.Checked = bool.Parse(val);
						return;
					case 8:
						if (!(prop2 == "Vertical"))
						{
							return;
						}
						Combat.Static.VelocityVrt.Value = (int)(double.Parse(val) * 10.0);
						return;
					case 9:
					case 11:
					case 12:
						break;
					case 10:
						if (!(prop2 == "Horizontal"))
						{
							return;
						}
						Combat.Static.VelocityHrz.Value = (int)(double.Parse(val) * 10.0);
						return;
					case 13:
					{
						char c = prop2[4];
						if (c != 'A')
						{
							if (c != 'T')
							{
								return;
							}
							if (!(prop2 == "OnlyTargeting"))
							{
								return;
							}
							Combat.Static.cbTargeting.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "OnlyAttacking"))
							{
								return;
							}
							Combat.Static.cbAttacking.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0001D840 File Offset: 0x0001BA40
		private void SetESPProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'M')
							{
								return;
							}
							if (!(prop2 == "Mode"))
							{
								return;
							}
							Visuals.Static.ESPEMode.SelectedIndex = int.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Bind"))
							{
								return;
							}
							Visuals.ESPBindInt = int.Parse(val);
							Keys espbindInt = (Keys)Visuals.ESPBindInt;
							Visuals.Static.bindESP = espbindInt;
							Visuals @static = Visuals.Static;
							BeautyAutoButton bindESP = Visuals.Static.BindESP;
							BeautyToggleSwitch espenable = Visuals.Static.ESPEnable;
							Keys bindKey = espbindInt;
							Action onToggle;
							if ((onToggle = cachedToggle) == null)
							{
								onToggle = (cachedToggle = delegate()
								{
									Imports.Checkar(this, Visuals.Static.ESPEnable);
								});
							}
							@static.RegisterBind(bindESP, espenable, bindKey, onToggle);
						}
						break;
					}
					case 5:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'N')
							{
								return;
							}
							if (!(prop2 == "Names"))
							{
								return;
							}
							Visuals.Static.ESPNames.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Boxes"))
							{
								return;
							}
							Visuals.Static.ESPBoxes.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 6:
					case 8:
					case 10:
						break;
					case 7:
					{
						char c = prop2[0];
						if (c != 'E')
						{
							if (c != 'O')
							{
								return;
							}
							if (!(prop2 == "Outline"))
							{
								return;
							}
							Visuals.Static.cbDrawCorners.Checked = bool.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Enabled"))
							{
								return;
							}
							Visuals.Static.ESPEnable.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 9:
						if (!(prop2 == "Healthbar"))
						{
							return;
						}
						Visuals.Static.ESPHealthbar.Checked = bool.Parse(val);
						return;
					case 11:
					{
						char c = prop2[0];
						if (c != 'D')
						{
							if (c != 'F')
							{
								return;
							}
							if (!(prop2 == "FilledColor"))
							{
								return;
							}
							string[] array = val.Split(new char[]
							{
								','
							});
							Visuals.Static.ColorFill.SelectedColor = Color.FromArgb((int)(float.Parse(array[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array[2], CultureInfo.InvariantCulture) * 255f));
							return;
						}
						else
						{
							if (!(prop2 == "DrawCorners"))
							{
								return;
							}
							Visuals.Static.cbDrawCorners.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					case 12:
					{
						char c = prop2[0];
						if (c != 'D')
						{
							if (c != 'O')
							{
								return;
							}
							if (!(prop2 == "OutlineColor"))
							{
								return;
							}
							string[] array2 = val.Split(new char[]
							{
								','
							});
							Visuals.Static.ColorOutline.SelectedColor = Color.FromArgb((int)(float.Parse(array2[0], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array2[1], CultureInfo.InvariantCulture) * 255f), (int)(float.Parse(array2[2], CultureInfo.InvariantCulture) * 255f));
							return;
						}
						else
						{
							if (!(prop2 == "DrawHurtTime"))
							{
								return;
							}
							Visuals.Static.cbDrawHurtTime.Checked = bool.Parse(val);
							return;
						}
						break;
					}
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001D87C File Offset: 0x0001BA7C
		private void SetSprintResetProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 != null)
				{
					switch (prop2.Length)
					{
					case 4:
					{
						char c = prop2[0];
						if (c != 'B')
						{
							if (c != 'M')
							{
								return;
							}
							if (!(prop2 == "Mode"))
							{
								return;
							}
							Combat.Static.ModeSprint.SelectedIndex = int.Parse(val);
							return;
						}
						else
						{
							if (!(prop2 == "Bind"))
							{
								return;
							}
							Combat.SprintResetBindInt = int.Parse(val);
							Keys sprintResetBindInt = (Keys)Combat.SprintResetBindInt;
							Combat.Static.bindSprintReset = sprintResetBindInt;
							Combat @static = Combat.Static;
							BeautyAutoButton sprintResetButton = Combat.Static.SprintResetButton;
							BeautyToggleSwitch cbSprintReset = Combat.Static.cbSprintReset;
							Keys bindKey = sprintResetBindInt;
							Action onToggle;
							if ((onToggle = cachedToggle) == null)
							{
								onToggle = (cachedToggle = delegate()
								{
									Imports.Checkar(this, Combat.Static.cbSprintReset);
								});
							}
							@static.RegisterBind(sprintResetButton, cbSprintReset, bindKey, onToggle);
						}
						break;
					}
					case 5:
						if (!(prop2 == "Delay"))
						{
							return;
						}
						Combat.Static.numSprintResetMinRePress.Value = int.Parse(val);
						return;
					case 6:
						if (!(prop2 == "Chance"))
						{
							return;
						}
						Combat.Static.numSprintResetChance.Value = int.Parse(val);
						return;
					case 7:
						if (!(prop2 == "Enabled"))
						{
							return;
						}
						Combat.Static.cbSprintReset.Checked = bool.Parse(val);
						return;
					case 8:
						if (!(prop2 == "ModeType"))
						{
							return;
						}
						Combat.Static.ModeTypeSprint.SelectedIndex = int.Parse(val);
						return;
					case 9:
					case 10:
					case 11:
						break;
					case 12:
						if (!(prop2 == "StopDuration"))
						{
							return;
						}
						Combat.Static.numSprintResetMaxRePress.Value = int.Parse(val);
						return;
					default:
						return;
					}
				}
			}));
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001D8B8 File Offset: 0x0001BAB8
		private void SetJumpResetProperty(string prop, string val)
		{
			Action cachedToggle = null;
			base.Invoke(new Action(delegate()
			{
				string prop2 = prop;
				if (prop2 == "Enabled")
				{
					Combat.Static.cbJumpReset.Checked = bool.Parse(val);
					return;
				}
				if (prop2 == "Chance")
				{
					Combat.Static.numJumpResetChance.Value = int.Parse(val);
					return;
				}
				if (prop2 == "Delay")
				{
					Combat.Static.numJumpResetDelay.Value = int.Parse(val);
					return;
				}
				if (prop2 == "JumpDuration")
				{
					Combat.Static.numJumpResetDuration.Value = int.Parse(val);
					return;
				}
				if (!(prop2 == "Bind"))
				{
					return;
				}
				Combat.JumpResetBindInt = int.Parse(val);
				Keys jumpResetBindInt = (Keys)Combat.JumpResetBindInt;
				Combat.Static.bindJumpReset = jumpResetBindInt;
				Combat @static = Combat.Static;
				BeautyAutoButton jumpResetButton = Combat.Static.JumpResetButton;
				BeautyToggleSwitch cbJumpReset = Combat.Static.cbJumpReset;
				Keys bindKey = jumpResetBindInt;
				Action onToggle;
				if ((onToggle = cachedToggle) == null)
				{
					onToggle = (cachedToggle = delegate()
					{
						Imports.Checkar(this, Combat.Static.cbJumpReset);
					});
				}
				@static.RegisterBind(jumpResetButton, cbJumpReset, bindKey, onToggle);
			}));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0001D8F4 File Offset: 0x0001BAF4
		private void AnalisarConfiguracoes(string conteudo)
		{
			foreach (string text in conteudo.Split(new char[]
			{
				'\n'
			}))
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string[] array2 = text.Split(new char[]
					{
						':'
					});
					if (array2.Length >= 3)
					{
						string text2 = array2[0];
						string text3 = array2[1];
						string text4 = string.Join(":", array2.Skip(2)).Trim();
						if (text2 != null)
						{
							switch (text2.Length)
							{
							case 3:
								if (text2 == "ESP")
								{
									this.SetESPProperty(text3, text4);
								}
								break;
							case 5:
							{
								char c = text2[0];
								if (c != 'C')
								{
									if (c != 'R')
									{
										if (c == 'T')
										{
											if (text2 == "Teams")
											{
												Utilities.Static.cbTeams.Checked = bool.Parse(text4);
											}
										}
									}
									else if (text2 == "Reach")
									{
										this.SetReachProperty(text3, text4);
									}
								}
								else if (text2 == "Chams")
								{
									Visuals.Static.cbChams.Checked = bool.Parse(text4);
								}
								break;
							}
							case 6:
								if (text2 == "Sprint")
								{
									if (text3 == "Enabled")
									{
										Movement.Static.cbSprint.Checked = bool.Parse(text4);
									}
									else if (text3 == "Bind")
									{
										Movement.SprintBindInt = int.Parse(text4);
										Keys sprintBindInt = (Keys)Movement.SprintBindInt;
										Movement.Static.bindSprintKey = sprintBindInt;
										Movement.Static.RegisterBind(Movement.Static.btSprint, Movement.Static.cbSprint, sprintBindInt, delegate
										{
											Imports.Checkar(this, Movement.Static.cbSprint);
										});
									}
								}
								break;
							case 7:
								if (text2 == "Antibot")
								{
									Utilities.Static.cbAntibot.Checked = bool.Parse(text4);
								}
								break;
							case 8:
								if (text2 == "Velocity")
								{
									this.SetVelocityProperty(text3, text4);
								}
								break;
							case 9:
							{
								char c = text2[1];
								if (c != 'i')
								{
									if (c != 'r')
									{
										if (c == 'u')
										{
											if (text2 == "JumpReset")
											{
												this.SetJumpResetProperty(text3, text4);
											}
										}
									}
									else if (text2 == "ArrayList")
									{
										this.SetArrayListProperty(text3, text4);
									}
								}
								else if (text2 == "AimAssist")
								{
									this.SetAimAssistProperty(text3, text4);
								}
								break;
							}
							case 10:
								if (text2 == "NoHitDelay")
								{
									Utilities.Static.cbHitDelay.Checked = bool.Parse(text4);
								}
								break;
							case 11:
							{
								char c = text2[0];
								if (c != 'L')
								{
									if (c != 'N')
									{
										if (c == 'S')
										{
											if (text2 == "SprintReset")
											{
												this.SetSprintResetProperty(text3, text4);
											}
										}
									}
									else if (text2 == "NoJumpDelay")
									{
										if (text3 == "Enabled")
										{
											Movement.Static.cbJumpDelay.Checked = bool.Parse(text4);
										}
										else if (text3 == "Bind")
										{
											Movement.JumpBindInt = int.Parse(text4);
											Keys jumpBindInt = (Keys)Movement.JumpBindInt;
											Movement.Static.bindJump = jumpBindInt;
											Movement.Static.RegisterBind(Movement.Static.btJump, Movement.Static.cbJumpDelay, jumpBindInt, delegate
											{
												Imports.Checkar(this, Movement.Static.cbJumpDelay);
											});
										}
									}
								}
								else if (text2 == "LeftClicker")
								{
									this.SetLeftClickerProperty(text3, text4);
								}
								break;
							}
							case 12:
							{
								char c = text2[0];
								if (c != 'B')
								{
									if (c == 'R')
									{
										if (text2 == "RightClicker")
										{
											this.SetRightClickerProperty(text3, text4);
										}
									}
								}
								else if (text2 == "BridgeAssist")
								{
									this.SetBridgeProperty(text3, text4);
								}
								break;
							}
							}
						}
					}
				}
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001DDBC File Offset: 0x0001BFBC
		private string GerarPayload()
		{
			this.payload += string.Format("LeftClicker:Enabled:{0}\n", Combat.Static.ClickerEnable.Checked);
			this.payload += string.Format("LeftClicker:Average:{0}\n", (double)Combat.Static.CPS_Slider.Value / 10.0);
			this.payload += string.Format("LeftClicker:BreakBlock:{0}\n", Combat.Static.cbBreak.Checked);
			this.payload += string.Format("LeftClicker:OnlyWeapon:{0}\n", Combat.Static.Weapon.Checked);
			this.payload += string.Format("LeftClicker:Randomize:{0}\n", Combat.Static.Randomize.Checked);
			this.payload += string.Format("LeftClicker:Inventory:{0}\n", Combat.Static.cbInventory.Checked);
			this.payload += string.Format("Reach:Enabled:{0}\n", Combat.Static.ReachEnable.Checked);
			this.payload += string.Format("Reach:Distance:{0}\n", (float)Combat.Static.DistanceSlider.Value / 100f);
			this.payload += string.Format("Reach:Hitbox:{0}\n", (float)Combat.Static.HitBoxSl.Value / 100f);
			this.payload += string.Format("Reach:OnlyWeapon:{0}\n", Combat.Static.cbReachWeapon.Checked);
			this.payload += string.Format("Reach:WallCheck:{0}\n", Combat.Static.cbWallCheck.Checked);
			this.payload += string.Format("Velocity:Enabled:{0}\n", Combat.Static.VelocityEnable.Checked);
			this.payload += string.Format("Velocity:Horizontal:{0}\n", (double)Combat.Static.VelocityHrz.Value / 10.0);
			this.payload += string.Format("Velocity:Vertical:{0}\n", (double)Combat.Static.VelocityVrt.Value / 10.0);
			this.payload += string.Format("Velocity:Chance:{0}\n", Combat.Static.ChanceSlider.Value);
			this.payload += string.Format("Velocity:Delay:{0}\n", Combat.Static.ticksvl.Value);
			this.payload += string.Format("Velocity:Moving:{0}\n", Combat.Static.VelocityMovingOnly.Checked);
			this.payload += string.Format("Velocity:OnlyAttacking:{0}\n", Combat.Static.cbAttacking.Checked);
			this.payload += string.Format("Velocity:OnlyTargeting:{0}\n", Combat.Static.cbTargeting.Checked);
			this.payload += string.Format("AimAssist:Enabled:{0}\n", Combat.Static.AimEnable.Checked);
			this.payload += string.Format("AimAssist:Distance:{0}\n", (float)Combat.Static.slideDistance.Value / 100f);
			this.payload += string.Format("AimAssist:Fov:{0}\n", Combat.Static.AimAssistFovSlider.Value);
			this.payload += string.Format("AimAssist:SpeedHorizontal:{0}\n", (float)Combat.Static.slidehorizontalaim.Value / 10f);
			this.payload += string.Format("AimAssist:SpeedVertical:{0}\n", (float)Combat.Static.slideverticalaim.Value / 10f);
			this.payload += string.Format("AimAssist:Vertical:{0}\n", Combat.Static.cbVertical.Checked);
			this.payload += string.Format("AimAssist:ClosestHitbox:{0}\n", Combat.Static.cbHitboxClosest.Checked);
			this.payload += string.Format("AimAssist:OnlyWeapon:{0}\n", Combat.Static.AimAssistOnlyWeapon.Checked);
			this.payload += string.Format("AimAssist:ThroughWall:{0}\n", Combat.Static.AimAssistThroughWall.Checked);
			this.payload += string.Format("AimAssist:ClickingOnly:{0}\n", Combat.Static.AimAssistClickingOnly.Checked);
			this.payload += string.Format("AimAssist:LockTarget:{0}\n", Combat.Static.cbLockTarget.Checked);
			this.payload += string.Format("AimAssist:BreakBlocks:{0}\n", Combat.Static.cbAimBreakBlocks.Checked);
			this.payload += string.Format("AimAssist:Mode:{0}\n", Combat.Static.cbMouseMove.Checked);
			this.payload += string.Format("ESP:Enabled:{0}\n", Visuals.Static.ESPEnable.Checked);
			this.payload += string.Format("ESP:Boxes:{0}\n", Visuals.Static.ESPBoxes.Checked);
			this.payload += string.Format("ESP:Mode:{0}\n", Visuals.Static.ESPEMode.SelectedIndex);
			this.payload += string.Format("ESP:DrawCorners:{0}\n", Visuals.Static.cbDrawCorners.Checked);
			this.payload += string.Format("ESP:Healthbar:{0}\n", Visuals.Static.ESPHealthbar.Checked);
			this.payload += string.Format("ESP:Names:{0}\n", Visuals.Static.ESPNames.Checked);
			this.payload += string.Format("ESP:DrawHurtTime:{0}\n", Visuals.Static.cbDrawHurtTime.Checked);
			this.payload += string.Format("ESP:Outline:{0}\n", Visuals.Static.cbDrawCorners.Checked);
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ESP:FilledColor:",
				((float)Visuals.Static.ColorFill.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorFill.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorFill.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				"\n"
			});
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ESP:OutlineColor:",
				((float)Visuals.Static.ColorOutline.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorOutline.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorOutline.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				"\n"
			});
			this.payload += string.Format("RightClicker:Enabled:{0}\n", Utilities.Static.cbEnabled.Checked);
			this.payload += string.Format("RightClicker:Average:{0}\n", (double)Utilities.Static.SliderRight.Value / 10.0);
			this.payload += string.Format("RightClicker:OnlyBlock:{0}\n", Utilities.Static.cbBlock.Checked);
			this.payload += string.Format("NoJumpDelay:Enabled:{0}\n", Movement.Static.cbJumpDelay.Checked);
			this.payload += string.Format("BridgeAssist:Enabled:{0}\n", Movement.Static.cbBridge.Checked);
			this.payload += string.Format("BridgeAssist:EdgeOffset:{0}\n", Movement.Static.edgeOffset.Value);
			this.payload += string.Format("BridgeAssist:UnsneakDelay:{0}\n", Movement.Static.unsneakDelay.Value);
			this.payload += string.Format("BridgeAssist:Randomize:{0}\n", Movement.Static.cbRandomize.Checked);
			this.payload += string.Format("BridgeAssist:SneakOnJump:{0}\n", Movement.Static.cbSneakOnJump.Checked);
			this.payload += string.Format("BridgeAssist:SneakKeyPressed:{0}\n", Movement.Static.cbSneakKeyPressed.Checked);
			this.payload += string.Format("BridgeAssist:HoldingBlocks:{0}\n", Movement.Static.cbHoldingBlocks.Checked);
			this.payload += string.Format("BridgeAssist:LookingDown:{0}\n", Movement.Static.cbLookingDown.Checked);
			this.payload += string.Format("BridgeAssist:AutoSwap:{0}\n", Movement.Static.cbAutoSwap.Checked);
			this.payload += string.Format("Sprint:Enabled:{0}\n", Movement.Static.cbSprint.Checked);
			this.payload += string.Format("ArrayList:Enabled:{0}\n", Visuals.Static.cbArraylist.Checked);
			this.payload += string.Format("ArrayList:Scale:{0}\n", (float)Visuals.Static.ScaleAr.Value / 100f);
			this.payload += string.Format("ArrayList:ShowBackground:{0}\n", Visuals.Static.cbBackground.Checked);
			this.payload += string.Format("ArrayList:Mode:{0}\n", Visuals.Static.Alignment.SelectedIndex);
			this.payload += string.Format("ArrayList:ColorMode:{0}\n", Visuals.Static.ColorModeCombo.SelectedIndex);
			this.payload += string.Format("ArrayList:Speed:{0}\n", (float)Visuals.Static.SpeedSlider.Value / 100f);
			this.payload += string.Format("ArrayList:PosX:{0}\n", (float)Visuals.Static.NumericPosX.Value / 10f);
			this.payload += string.Format("ArrayList:PosY:{0}\n", (float)Visuals.Static.NumericPosY.Value / 10f);
			this.payload += string.Format("ArrayList:PaddingX:{0}\n", (float)Visuals.Static.SliderPaddingX.Value / 10f);
			this.payload += string.Format("ArrayList:PaddingY:{0}\n", (float)Visuals.Static.SliderPaddingY.Value / 10f);
			this.payload += string.Format("ArrayList:Radius:{0}\n", (float)Visuals.Static.SliderRadius.Value / 10f);
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ArrayList:Color:",
				((float)Visuals.Static.ColorArrayList.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayList.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayList.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayList.SelectedColor.A / 255f).ToString(CultureInfo.InvariantCulture),
				"\n"
			});
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ArrayList:ColorB:",
				((float)Visuals.Static.ColorArrayListB.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayListB.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayListB.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorArrayListB.SelectedColor.A / 255f).ToString(CultureInfo.InvariantCulture),
				"\n"
			});
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ArrayList:BackgroundColor:",
				((float)Visuals.Static.ColorBackgroundAL.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorBackgroundAL.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorBackgroundAL.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				",0.55\n"
			});
			this.payload = string.Concat(new string[]
			{
				this.payload,
				"ArrayList:ExtraInfoColor:",
				((float)Visuals.Static.ColorExtraAL.SelectedColor.R / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorExtraAL.SelectedColor.G / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorExtraAL.SelectedColor.B / 255f).ToString(CultureInfo.InvariantCulture),
				",",
				((float)Visuals.Static.ColorExtraAL.SelectedColor.A / 255f).ToString(CultureInfo.InvariantCulture),
				"\n"
			});
			this.payload += string.Format("Chams:Enabled:{0}\n", Visuals.Static.cbChams.Checked);
			this.payload += string.Format("NoHitDelay:Enabled:{0}\n", Utilities.Static.cbHitDelay.Checked);
			this.payload += string.Format("Teams:Enabled:{0}\n", Utilities.Static.cbTeams.Checked);
			this.payload += string.Format("Antibot:Enabled:{0}\n", Utilities.Static.cbAntibot.Checked);
			this.payload += string.Format("LeftClicker:Bind:{0}\n", Combat.ClickerBindInt);
			this.payload += string.Format("Reach:Bind:{0}\n", Combat.ReachBindInt);
			this.payload += string.Format("Velocity:Bind:{0}\n", Combat.VelocityBindInt);
			this.payload += string.Format("AimAssist:Bind:{0}\n", Combat.AimBindInt);
			this.payload += string.Format("RightClicker:Bind:{0}\n", Utilities.RightClickBindInt);
			this.payload += string.Format("BridgeAssist:Bind:{0}\n", Movement.BridgeBindInt);
			this.payload += string.Format("NoJumpDelay:Bind:{0}\n", Movement.JumpBindInt);
			this.payload += string.Format("Sprint:Bind:{0}\n", Movement.SprintBindInt);
			this.payload += string.Format("ESP:Bind:{0}\n", Visuals.ESPBindInt);
			this.payload += string.Format("SprintReset:Bind:{0}\n", Combat.SprintResetBindInt);
			this.payload += string.Format("JumpReset:Bind:{0}\n", Combat.JumpResetBindInt);
			this.payload += string.Format("SprintReset:Enabled:{0}\n", Combat.Static.cbSprintReset.Checked);
			this.payload += string.Format("SprintReset:Chance:{0}\n", Combat.Static.numSprintResetChance.Value);
			this.payload += string.Format("SprintReset:Delay:{0}\n", Combat.Static.numSprintResetMinRePress.Value);
			this.payload += string.Format("SprintReset:StopDuration:{0}\n", Combat.Static.numSprintResetMaxRePress.Value);
			this.payload += string.Format("SprintReset:Mode:{0}\n", Combat.Static.ModeSprint.SelectedIndex);
			this.payload += string.Format("SprintReset:ModeType:{0}\n", Combat.Static.ModeTypeSprint.SelectedIndex);
			this.payload += string.Format("JumpReset:Enabled:{0}\n", Combat.Static.cbJumpReset.Checked);
			this.payload += string.Format("JumpReset:Chance:{0}\n", Combat.Static.numJumpResetChance.Value);
			this.payload += string.Format("JumpReset:Delay:{0}\n", Combat.Static.numJumpResetDelay.Value);
			this.payload += string.Format("JumpReset:JumpDuration:{0}\n", Combat.Static.numJumpResetDuration.Value);
			return this.payload;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001F3A8 File Offset: 0x0001D5A8
		private void CreateButton_Click(object sender, EventArgs e)
		{
			if (this.NameInput.Text == string.Empty)
			{
				return;
			}
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre");
			string path = Path.Combine(text, this.NameInput.Text + ".config");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			this.GerarPayload();
			try
			{
				if (MessageBox.Show("Notice: Saving configurations locally can leave traces in your system (Logs/Prefetch) that are easily detected during a Screenshare.\n\nDo you still want to save this config?", "Security Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
				{
					File.WriteAllText(path, this.payload);
					this.ConfigsCombo.AddItem(this.NameInput.Text, true);
					this.ConfigsCombo.ForeText = this.NameInput.Text;
					MessageBox.Show("Config " + this.NameInput.Text + " successfully created!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this.NameInput.Text = string.Empty;
				}
			}
			catch
			{
				MessageBox.Show("Failed to create " + this.NameInput.Text + " config!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001F4DC File Offset: 0x0001D6DC
		private void ButtonLoad_Click(object sender, EventArgs e)
		{
			if (this.ConfigsCombo.ForeText == "None")
			{
				return;
			}
			string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre"), this.ConfigsCombo.ForeText + ".config");
			if (!File.Exists(path))
			{
				MessageBox.Show("Failed to load " + this.ConfigsCombo.ForeText + " config!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			try
			{
				this.AnalisarConfiguracoes(File.ReadAllText(path));
				this.AtualizarLabels();
				dllconnect.EnviarConfiguracoes();
				MessageBox.Show("Config " + this.ConfigsCombo.ForeText + " successfully loaded!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch
			{
				MessageBox.Show("An error occurred while loading this configuration.\n\nThis usually happens because the config was created in an older version of the client.\n\nPlease delete all old configurations and create new ones.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		public void AtualizarLabels()
		{
			float num = (float)Combat.Static.slideDistance.Value / 100f;
			Combat.Static.aimlabeldist.Text = num.ToString("0.00", CultureInfo.InvariantCulture);
			Combat.Static.lbfov.Text = Combat.Static.AimAssistFovSlider.Value.ToString();
			float num2 = (float)Combat.Static.slidehorizontalaim.Value / 10f;
			Combat.Static.labelHorizontal.Text = num2.ToString("0.0", CultureInfo.InvariantCulture);
			float num3 = (float)Combat.Static.slideverticalaim.Value / 10f;
			Combat.Static.labelVertical.Text = num3.ToString("0.0", CultureInfo.InvariantCulture);
			float num4 = (float)Combat.Static.DistanceSlider.Value / 100f;
			Combat.Static.labelReach.Text = num4.ToString("0.00", CultureInfo.InvariantCulture);
			float num5 = (float)Combat.Static.HitBoxSl.Value / 100f;
			Combat.Static.labelHitbox.Text = num5.ToString("0.00", CultureInfo.InvariantCulture);
			Combat.Static.lbticks.Text = Combat.Static.ticksvl.Value.ToString();
			double num6 = (double)Combat.Static.VelocityHrz.Value / 10.0;
			Combat.Static.labelVelH.Text = num6.ToString("0.0", CultureInfo.InvariantCulture);
			double num7 = (double)Combat.Static.VelocityVrt.Value / 10.0;
			Combat.Static.labelVelV.Text = num7.ToString("0.0", CultureInfo.InvariantCulture);
			Combat.Static.lbchancevl.Text = Combat.Static.ChanceSlider.Value.ToString() + "%";
			float num8 = (float)Combat.Static.CPS_Slider.Value / 10f;
			Combat.Static.labelCPS.Text = num8.ToString("0.0", CultureInfo.InvariantCulture);
			float num9 = (float)Movement.Static.edgeOffset.Value / 100f * 0.3f;
			Movement.Static.edglb.Text = num9.ToString("0.00", CultureInfo.InvariantCulture);
			Movement.Static.delaylb.Text = Movement.Static.unsneakDelay.Value.ToString() + " ms";
			double num10 = (double)Utilities.Static.SliderRight.Value / 10.0;
			Utilities.Static.lbvalue.Text = num10.ToString("0.0", CultureInfo.InvariantCulture);
			float num11 = (float)Visuals.Static.ScaleAr.Value / 100f;
			Visuals.Static.lbarrayscale.Text = num11.ToString("0.00", CultureInfo.InvariantCulture);
			float num12 = (float)Visuals.Static.SpeedSlider.Value / 100f;
			Visuals.Static.lbSpeed.Text = num12.ToString("0.00", CultureInfo.InvariantCulture);
			float num13 = (float)Visuals.Static.NumericPosX.Value / 10f;
			Visuals.Static.lbPosX.Text = num13.ToString("0.0", CultureInfo.InvariantCulture);
			float num14 = (float)Visuals.Static.NumericPosY.Value / 10f;
			Visuals.Static.lbPosY.Text = num14.ToString("0.0", CultureInfo.InvariantCulture);
			float num15 = (float)Visuals.Static.SliderPaddingX.Value / 10f;
			Visuals.Static.lbPaddingX.Text = num15.ToString("0.0", CultureInfo.InvariantCulture);
			float num16 = (float)Visuals.Static.SliderPaddingY.Value / 10f;
			Visuals.Static.lbPaddingY.Text = num16.ToString("0.0", CultureInfo.InvariantCulture);
			float num17 = (float)Visuals.Static.SliderRadius.Value / 10f;
			Visuals.Static.lbRadius.Text = num17.ToString("0.0", CultureInfo.InvariantCulture);
			Combat.Static.lbSprintResetChance.Text = Combat.Static.numSprintResetChance.Value.ToString() + "%";
			Combat.Static.lbSprintResetDelay.Text = Combat.Static.numSprintResetMinRePress.Value.ToString() + " ms";
			Combat.Static.lbSprintResetStop.Text = Combat.Static.numSprintResetMaxRePress.Value.ToString() + " ms";
			Combat.Static.lbJumpResetChance.Text = Combat.Static.numJumpResetChance.Value.ToString() + "%";
			Combat.Static.lbJumpResetDelay.Text = Combat.Static.numJumpResetDelay.Value.ToString() + " ms";
			Combat.Static.lbJumpResetDuration.Text = Combat.Static.numJumpResetDuration.Value.ToString() + " ms";
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0001FB74 File Offset: 0x0001DD74
		private void BindDelete_Click(object sender, EventArgs e)
		{
			if (this.ConfigsCombo.ForeText == "None")
			{
				return;
			}
			string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre"), this.ConfigsCombo.ForeText + ".config");
			if (!File.Exists(path))
			{
				MessageBox.Show("Failed to delete " + this.ConfigsCombo.ForeText + " config!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (MessageBox.Show("Are you sure you want to delete this config? This action is irreversible.", "Delete Config Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return;
			}
			File.Delete(path);
			this.ConfigsCombo.RemoveItem(this.ConfigsCombo.ForeText);
			this.ConfigsCombo.ForeText = "None";
			MessageBox.Show("Config " + this.ConfigsCombo.ForeText + " successfully deleted!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001FC60 File Offset: 0x0001DE60
		private void OpenFolder_Click(object sender, EventArgs e)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			Process.Start("explorer.exe", text);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0001FC9C File Offset: 0x0001DE9C
		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (this.ConfigsCombo.ForeText == "None")
			{
				MessageBox.Show("Select a config first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre");
			string path = Path.Combine(text, this.ConfigsCombo.ForeText + ".config");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			try
			{
				string contents = this.GerarPayload();
				File.WriteAllText(path, contents);
				MessageBox.Show("Config saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch
			{
				MessageBox.Show("Failed to save config!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001FD5C File Offset: 0x0001DF5C
		private void Refresh_Click(object sender, EventArgs e)
		{
			foreach (string text in Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Azyre")))
			{
				if (text.EndsWith(".config") && !(File.ReadAllText(text) == string.Empty))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					try
					{
						this.ConfigsCombo.AddItem(fileNameWithoutExtension, true);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001FDDC File Offset: 0x0001DFDC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001FDFC File Offset: 0x0001DFFC
		private void InitializeComponent()
		{
			this.DefaultPanel = new BeautyPanel();
			this.beautyPanel5 = new BeautyPanel();
			this.ConfigsCombo = new BeautyComboBox();
			this.beautyLabel4 = new BeautyLabel();
			this.beautyLabel26 = new BeautyLabel();
			this.BindDelete = new BeautyAutoButton();
			this.ButtonLoad = new BeautyAutoButton();
			this.NameInput = new BeautyTextBox();
			this.CreateButton = new BeautyAutoButton();
			this.beautyPanel6 = new BeautyPanel();
			this.beautyLabel3 = new BeautyLabel();
			this.beautyPanel3 = new BeautyPanel();
			this.labelExpiry = new BeautyLabel();
			this.labelVersion = new BeautyLabel();
			this.beautyPanel4 = new BeautyPanel();
			this.beautyLabel1 = new BeautyLabel();
			this.beautyPanel1 = new BeautyPanel();
			this.btDestruct = new BeautyAutoButton();
			this.beautyPanel2 = new BeautyPanel();
			this.beautyLabel2 = new BeautyLabel();
			this.SaveButton = new BeautyAutoButton();
			this.OpenFolder = new BeautyAutoButton();
			this.Refresh = new BeautyLabel();
			this.DefaultPanel.SuspendLayout();
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
			this.DefaultPanel.TabIndex = 847;
			this.beautyPanel5.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel5.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel5.BorderSizeBottom = 1f;
			this.beautyPanel5.BorderSizeLeft = 1f;
			this.beautyPanel5.BorderSizeRight = 1f;
			this.beautyPanel5.BorderSizeTop = 1f;
			this.beautyPanel5.Controls.Add(this.Refresh);
			this.beautyPanel5.Controls.Add(this.OpenFolder);
			this.beautyPanel5.Controls.Add(this.SaveButton);
			this.beautyPanel5.Controls.Add(this.ConfigsCombo);
			this.beautyPanel5.Controls.Add(this.beautyLabel4);
			this.beautyPanel5.Controls.Add(this.beautyLabel26);
			this.beautyPanel5.Controls.Add(this.BindDelete);
			this.beautyPanel5.Controls.Add(this.ButtonLoad);
			this.beautyPanel5.Controls.Add(this.NameInput);
			this.beautyPanel5.Controls.Add(this.CreateButton);
			this.beautyPanel5.Controls.Add(this.beautyPanel6);
			this.beautyPanel5.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel5.FullHeight = 350;
			this.beautyPanel5.Location = new Point(283, 21);
			this.beautyPanel5.Name = "beautyPanel5";
			this.beautyPanel5.RadiusBottomLeft = 6f;
			this.beautyPanel5.RadiusBottomRight = 6f;
			this.beautyPanel5.RadiusTopLeft = 6f;
			this.beautyPanel5.RadiusTopRight = 6f;
			this.beautyPanel5.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel5.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel5.ScrollbarWidth = 4;
			this.beautyPanel5.Size = new Size(260, 359);
			this.beautyPanel5.TabIndex = 913;
			this.ConfigsCombo.BorderColor = Color.FromArgb(20, 22, 22);
			this.ConfigsCombo.BorderRadius = 2f;
			this.ConfigsCombo.CheckedForeColor = Color.FromArgb(119, 119, 129);
			this.ConfigsCombo.FillColor = Color.FromArgb(16, 18, 18);
			this.ConfigsCombo.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.ConfigsCombo.ForeColor = Color.FromArgb(119, 119, 129);
			this.ConfigsCombo.ForegroundColor = Color.FromArgb(40, 40, 50);
			this.ConfigsCombo.ForeText = "mush";
			this.ConfigsCombo.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.ConfigsCombo.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ConfigsCombo.HoverForeColor = Color.FromArgb(40, 40, 50);
			this.ConfigsCombo.ItemHeight = 30;
			this.ConfigsCombo.Items = new string[0];
			this.ConfigsCombo.Location = new Point(16, 164);
			this.ConfigsCombo.Name = "ConfigsCombo";
			this.ConfigsCombo.Size = new Size(229, 24);
			this.ConfigsCombo.TabIndex = 928;
			this.ConfigsCombo.Text = "mush";
			this.beautyLabel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel4.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel4.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel4.Location = new Point(16, 140);
			this.beautyLabel4.Name = "beautyLabel4";
			this.beautyLabel4.Size = new Size(85, 18);
			this.beautyLabel4.TabIndex = 931;
			this.beautyLabel4.Text = "My settings";
			this.beautyLabel4.TextPadding = new Padding(0);
			this.beautyLabel26.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyLabel26.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel26.ForeColor = Color.FromArgb(70, 70, 80);
			this.beautyLabel26.Location = new Point(16, 55);
			this.beautyLabel26.Name = "beautyLabel26";
			this.beautyLabel26.Size = new Size(133, 18);
			this.beautyLabel26.TabIndex = 930;
			this.beautyLabel26.Text = "Create New Config";
			this.beautyLabel26.TextPadding = new Padding(0);
			this.BindDelete.AnimationSpeed = 0.6f;
			this.BindDelete.BorderColor = Color.FromArgb(16, 18, 18);
			this.BindDelete.BorderRadius = 4f;
			this.BindDelete.BorderSize = 1f;
			this.BindDelete.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.BindDelete.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.BindDelete.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.BindDelete.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.BindDelete.ExpansionDirection = (BeautyAutoButton.ExpandDirection)2;
			this.BindDelete.FillColor = Color.FromArgb(16, 18, 18);
			this.BindDelete.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.BindDelete.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.BindDelete.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.BindDelete.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.BindDelete.ImageOffset = new Point(0, 0);
			this.BindDelete.Location = new Point(16, 233);
			this.BindDelete.MinimumSize = new Size(20, 22);
			this.BindDelete.MinimumTextWidth = 190;
			this.BindDelete.Name = "BindDelete";
			this.BindDelete.Size = new Size(230, 22);
			this.BindDelete.TabIndex = 929;
			this.BindDelete.Text = "Delete";
			this.BindDelete.TextOffset = new Point(0, 0);
			this.BindDelete.TextPadding = new Padding(0);
			this.BindDelete.YOffSet = 0;
			this.BindDelete.Click += this.BindDelete_Click;
			this.ButtonLoad.AnimationSpeed = 0.6f;
			this.ButtonLoad.BorderColor = Color.FromArgb(16, 18, 18);
			this.ButtonLoad.BorderRadius = 4f;
			this.ButtonLoad.BorderSize = 1f;
			this.ButtonLoad.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.ButtonLoad.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.ButtonLoad.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.ButtonLoad.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.ButtonLoad.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.ButtonLoad.FillColor = Color.FromArgb(16, 18, 18);
			this.ButtonLoad.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.ButtonLoad.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.ButtonLoad.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.ButtonLoad.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.ButtonLoad.ImageOffset = new Point(0, 0);
			this.ButtonLoad.Location = new Point(16, 205);
			this.ButtonLoad.MinimumSize = new Size(20, 22);
			this.ButtonLoad.MinimumTextWidth = 190;
			this.ButtonLoad.Name = "ButtonLoad";
			this.ButtonLoad.Size = new Size(230, 22);
			this.ButtonLoad.TabIndex = 928;
			this.ButtonLoad.Text = "Load";
			this.ButtonLoad.TextOffset = new Point(0, 0);
			this.ButtonLoad.TextPadding = new Padding(0);
			this.ButtonLoad.YOffSet = 0;
			this.ButtonLoad.Click += this.ButtonLoad_Click;
			this.NameInput.BackColor = Color.FromArgb(12, 14, 14);
			this.NameInput.BorderColor = Color.FromArgb(20, 22, 22);
			this.NameInput.BorderRadius = 2f;
			this.NameInput.FillColor = Color.FromArgb(16, 18, 18);
			this.NameInput.Font = new Font("Bahnschrift", 10f, FontStyle.Bold);
			this.NameInput.ForeColor = Color.FromArgb(119, 119, 129);
			this.NameInput.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.NameInput.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.NameInput.Location = new Point(16, 82);
			this.NameInput.Name = "NameInput";
			this.NameInput.PlaceHolderColor = Color.Transparent;
			this.NameInput.PlaceholderText = "";
			this.NameInput.ResetTextOnClick = false;
			this.NameInput.SelectionBackColor = Color.Transparent;
			this.NameInput.Size = new Size(229, 24);
			this.NameInput.TabIndex = 920;
			this.NameInput.TextOffset = new Point(8, 1);
			this.CreateButton.AnimationSpeed = 0.6f;
			this.CreateButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.CreateButton.BorderRadius = 4f;
			this.CreateButton.BorderSize = 1f;
			this.CreateButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.CreateButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.CreateButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.CreateButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.CreateButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.CreateButton.FillColor = Color.FromArgb(16, 18, 18);
			this.CreateButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.CreateButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.CreateButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.CreateButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.CreateButton.ImageOffset = new Point(0, 0);
			this.CreateButton.Location = new Point(16, 112);
			this.CreateButton.MinimumSize = new Size(20, 22);
			this.CreateButton.MinimumTextWidth = 190;
			this.CreateButton.Name = "CreateButton";
			this.CreateButton.Size = new Size(230, 22);
			this.CreateButton.TabIndex = 919;
			this.CreateButton.Text = "Create";
			this.CreateButton.TextOffset = new Point(0, 0);
			this.CreateButton.TextPadding = new Padding(0);
			this.CreateButton.YOffSet = 0;
			this.CreateButton.Click += this.CreateButton_Click;
			this.beautyPanel6.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel6.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel6.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel6.BorderSizeBottom = 1f;
			this.beautyPanel6.BorderSizeLeft = 1f;
			this.beautyPanel6.BorderSizeRight = 1f;
			this.beautyPanel6.BorderSizeTop = 1f;
			this.beautyPanel6.Controls.Add(this.beautyLabel3);
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
			this.beautyLabel3.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel3.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel3.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel3.Location = new Point(10, 11);
			this.beautyLabel3.Name = "beautyLabel3";
			this.beautyLabel3.Size = new Size(111, 18);
			this.beautyLabel3.TabIndex = 905;
			this.beautyLabel3.Text = "Custom configs";
			this.beautyLabel3.TextPadding = new Padding(0);
			this.beautyPanel3.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel3.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel3.BorderSizeBottom = 1f;
			this.beautyPanel3.BorderSizeLeft = 1f;
			this.beautyPanel3.BorderSizeRight = 1f;
			this.beautyPanel3.BorderSizeTop = 1f;
			this.beautyPanel3.Controls.Add(this.labelExpiry);
			this.beautyPanel3.Controls.Add(this.labelVersion);
			this.beautyPanel3.Controls.Add(this.beautyPanel4);
			this.beautyPanel3.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel3.FullHeight = 350;
			this.beautyPanel3.Location = new Point(11, 119);
			this.beautyPanel3.Name = "beautyPanel3";
			this.beautyPanel3.RadiusBottomLeft = 6f;
			this.beautyPanel3.RadiusBottomRight = 6f;
			this.beautyPanel3.RadiusTopLeft = 6f;
			this.beautyPanel3.RadiusTopRight = 6f;
			this.beautyPanel3.ScrollbarColor = Color.FromArgb(135, 135, 255);
			this.beautyPanel3.ScrollbarPadding = new Point(10, 10);
			this.beautyPanel3.ScrollbarWidth = 4;
			this.beautyPanel3.Size = new Size(260, 105);
			this.beautyPanel3.TabIndex = 912;
			this.labelExpiry.BackColor = Color.FromArgb(12, 14, 14);
			this.labelExpiry.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelExpiry.ForeColor = Color.FromArgb(40, 40, 50);
			this.labelExpiry.Location = new Point(11, 73);
			this.labelExpiry.Name = "labelExpiry";
			this.labelExpiry.Size = new Size(62, 18);
			this.labelExpiry.TabIndex = 910;
			this.labelExpiry.Text = "Expires:";
			this.labelExpiry.TextPadding = new Padding(0);
			this.labelVersion.BackColor = Color.FromArgb(12, 14, 14);
			this.labelVersion.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.labelVersion.ForeColor = Color.FromArgb(40, 40, 50);
			this.labelVersion.Location = new Point(11, 49);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new Size(61, 18);
			this.labelVersion.TabIndex = 909;
			this.labelVersion.Text = "Version:";
			this.labelVersion.TextPadding = new Padding(0);
			this.beautyPanel4.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel4.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel4.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel4.BorderSizeBottom = 1f;
			this.beautyPanel4.BorderSizeLeft = 1f;
			this.beautyPanel4.BorderSizeRight = 1f;
			this.beautyPanel4.BorderSizeTop = 1f;
			this.beautyPanel4.Controls.Add(this.beautyLabel1);
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
			this.beautyLabel1.BackColor = Color.FromArgb(16, 18, 18);
			this.beautyLabel1.Font = new Font("Bahnschrift", 11.25f, FontStyle.Bold);
			this.beautyLabel1.ForeColor = Color.FromArgb(40, 40, 50);
			this.beautyLabel1.Location = new Point(10, 11);
			this.beautyLabel1.Name = "beautyLabel1";
			this.beautyLabel1.Size = new Size(41, 18);
			this.beautyLabel1.TabIndex = 905;
			this.beautyLabel1.Text = "Infos";
			this.beautyLabel1.TextPadding = new Padding(0);
			this.beautyPanel1.AutoScrollMinSize = new Size(0, 350);
			this.beautyPanel1.BackColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.BorderColor = Color.FromArgb(20, 22, 22);
			this.beautyPanel1.BorderSizeBottom = 1f;
			this.beautyPanel1.BorderSizeLeft = 1f;
			this.beautyPanel1.BorderSizeRight = 1f;
			this.beautyPanel1.BorderSizeTop = 1f;
			this.beautyPanel1.Controls.Add(this.btDestruct);
			this.beautyPanel1.Controls.Add(this.beautyPanel2);
			this.beautyPanel1.FillColor = Color.FromArgb(12, 14, 14);
			this.beautyPanel1.FullHeight = 350;
			this.beautyPanel1.Location = new Point(11, 21);
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
			this.btDestruct.AnimationSpeed = 0.6f;
			this.btDestruct.BorderColor = Color.FromArgb(16, 18, 18);
			this.btDestruct.BorderRadius = 4f;
			this.btDestruct.BorderSize = 1f;
			this.btDestruct.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.btDestruct.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.btDestruct.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.btDestruct.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.btDestruct.ExpansionDirection = (BeautyAutoButton.ExpandDirection)1;
			this.btDestruct.FillColor = Color.FromArgb(16, 18, 18);
			this.btDestruct.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.btDestruct.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.btDestruct.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.btDestruct.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.btDestruct.ImageOffset = new Point(0, 0);
			this.btDestruct.Location = new Point(10, 55);
			this.btDestruct.MinimumSize = new Size(20, 22);
			this.btDestruct.MinimumTextWidth = 20;
			this.btDestruct.Name = "btDestruct";
			this.btDestruct.Size = new Size(99, 22);
			this.btDestruct.TabIndex = 909;
			this.btDestruct.Text = "Destruct";
			this.btDestruct.TextOffset = new Point(0, 0);
			this.btDestruct.TextPadding = new Padding(0);
			this.btDestruct.YOffSet = 0;
			this.btDestruct.Click += this.btDestruct_Click;
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
			this.beautyLabel2.Size = new Size(95, 18);
			this.beautyLabel2.TabIndex = 905;
			this.beautyLabel2.Text = "Self Destruct";
			this.beautyLabel2.TextPadding = new Padding(0);
			this.SaveButton.AnimationSpeed = 0.6f;
			this.SaveButton.BorderColor = Color.FromArgb(16, 18, 18);
			this.SaveButton.BorderRadius = 4f;
			this.SaveButton.BorderSize = 1f;
			this.SaveButton.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.SaveButton.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.SaveButton.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.SaveButton.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.SaveButton.ExpansionDirection = (BeautyAutoButton.ExpandDirection)2;
			this.SaveButton.FillColor = Color.FromArgb(16, 18, 18);
			this.SaveButton.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.SaveButton.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.SaveButton.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.SaveButton.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.SaveButton.ImageOffset = new Point(0, 0);
			this.SaveButton.Location = new Point(15, 261);
			this.SaveButton.MinimumSize = new Size(20, 22);
			this.SaveButton.MinimumTextWidth = 190;
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new Size(230, 22);
			this.SaveButton.TabIndex = 932;
			this.SaveButton.Text = "Save";
			this.SaveButton.TextOffset = new Point(0, 0);
			this.SaveButton.TextPadding = new Padding(0);
			this.SaveButton.YOffSet = 0;
			this.SaveButton.Click += this.SaveButton_Click;
			this.OpenFolder.AnimationSpeed = 0.6f;
			this.OpenFolder.BorderColor = Color.FromArgb(16, 18, 18);
			this.OpenFolder.BorderRadius = 4f;
			this.OpenFolder.BorderSize = 1f;
			this.OpenFolder.CheckedBorderColor = Color.FromArgb(28, 28, 44);
			this.OpenFolder.CheckedFillColor = Color.FromArgb(28, 28, 44);
			this.OpenFolder.CheckedForeColor = Color.FromArgb(190, 190, 205);
			this.OpenFolder.DefaltForeColor = Color.FromArgb(40, 40, 50);
			this.OpenFolder.ExpansionDirection = (BeautyAutoButton.ExpandDirection)2;
			this.OpenFolder.FillColor = Color.FromArgb(16, 18, 18);
			this.OpenFolder.Font = new Font("Bahnschrift", 10.25f, FontStyle.Bold);
			this.OpenFolder.HoverBorderColor = Color.FromArgb(20, 22, 22);
			this.OpenFolder.HoverFillColor = Color.FromArgb(20, 22, 22);
			this.OpenFolder.HoverForeColor = Color.FromArgb(70, 70, 80);
			this.OpenFolder.ImageOffset = new Point(0, 0);
			this.OpenFolder.Location = new Point(15, 289);
			this.OpenFolder.MinimumSize = new Size(20, 22);
			this.OpenFolder.MinimumTextWidth = 190;
			this.OpenFolder.Name = "OpenFolder";
			this.OpenFolder.Size = new Size(230, 22);
			this.OpenFolder.TabIndex = 933;
			this.OpenFolder.Text = "Open folder";
			this.OpenFolder.TextOffset = new Point(0, 0);
			this.OpenFolder.TextPadding = new Padding(0);
			this.OpenFolder.YOffSet = 0;
			this.OpenFolder.Click += this.OpenFolder_Click;
			this.Refresh.BackColor = Color.FromArgb(12, 14, 14);
			this.Refresh.Font = new Font("Bahnschrift", 13f, FontStyle.Bold);
			this.Refresh.ForeColor = Color.FromArgb(70, 70, 80);
			this.Refresh.Location = new Point(221, 139);
			this.Refresh.Name = "Refresh";
			this.Refresh.Size = new Size(24, 22);
			this.Refresh.TabIndex = 934;
			this.Refresh.Text = "⟳";
			this.Refresh.TextPadding = new Padding(0);
			this.Refresh.Click += this.Refresh_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.DefaultPanel);
			base.Name = "Destruct";
			base.Size = new Size(570, 410);
			this.DefaultPanel.ResumeLayout(false);
			this.beautyPanel5.ResumeLayout(false);
			this.beautyPanel5.PerformLayout();
			this.beautyPanel6.ResumeLayout(false);
			this.beautyPanel3.ResumeLayout(false);
			this.beautyPanel4.ResumeLayout(false);
			this.beautyPanel1.ResumeLayout(false);
			this.beautyPanel1.PerformLayout();
			this.beautyPanel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000257 RID: 599
		public static bool destruct;

		// Token: 0x04000258 RID: 600
		private const int PROCESS_QUERY_INFORMATION = 1024;

		// Token: 0x04000259 RID: 601
		private const int PROCESS_WM_READ = 16;

		// Token: 0x0400025A RID: 602
		private const int PAGE_READWRITE = 4;

		// Token: 0x0400025B RID: 603
		private const int MEM_COMMIT = 4096;

		// Token: 0x0400025C RID: 604
		private string payload = "";

		// Token: 0x0400025D RID: 605
		private IContainer components;

		// Token: 0x0400025E RID: 606
		private BeautyPanel DefaultPanel;

		// Token: 0x0400025F RID: 607
		private BeautyPanel beautyPanel1;

		// Token: 0x04000260 RID: 608
		private BeautyAutoButton btDestruct;

		// Token: 0x04000261 RID: 609
		private BeautyPanel beautyPanel2;

		// Token: 0x04000262 RID: 610
		private BeautyLabel beautyLabel2;

		// Token: 0x04000263 RID: 611
		private BeautyPanel beautyPanel3;

		// Token: 0x04000264 RID: 612
		private BeautyPanel beautyPanel4;

		// Token: 0x04000265 RID: 613
		private BeautyLabel beautyLabel1;

		// Token: 0x04000266 RID: 614
		private BeautyLabel labelExpiry;

		// Token: 0x04000267 RID: 615
		private BeautyLabel labelVersion;

		// Token: 0x04000268 RID: 616
		private BeautyPanel beautyPanel5;

		// Token: 0x04000269 RID: 617
		private BeautyPanel beautyPanel6;

		// Token: 0x0400026A RID: 618
		private BeautyLabel beautyLabel3;

		// Token: 0x0400026B RID: 619
		private BeautyAutoButton CreateButton;

		// Token: 0x0400026C RID: 620
		private BeautyTextBox NameInput;

		// Token: 0x0400026D RID: 621
		private BeautyAutoButton BindDelete;

		// Token: 0x0400026E RID: 622
		private BeautyAutoButton ButtonLoad;

		// Token: 0x0400026F RID: 623
		private BeautyLabel beautyLabel4;

		// Token: 0x04000270 RID: 624
		private BeautyLabel beautyLabel26;

		// Token: 0x04000271 RID: 625
		public BeautyComboBox ConfigsCombo;

		// Token: 0x04000272 RID: 626
		private BeautyAutoButton OpenFolder;

		// Token: 0x04000273 RID: 627
		private BeautyAutoButton SaveButton;

		// Token: 0x04000274 RID: 628
		private new BeautyLabel Refresh;

		// Token: 0x02000050 RID: 80
		public struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x04000275 RID: 629
			public IntPtr BaseAddress;

			// Token: 0x04000276 RID: 630
			public IntPtr AllocationBase;

			// Token: 0x04000277 RID: 631
			public int AllocationProtect;

			// Token: 0x04000278 RID: 632
			public IntPtr RegionSize;

			// Token: 0x04000279 RID: 633
			public int State;

			// Token: 0x0400027A RID: 634
			public int Protect;

			// Token: 0x0400027B RID: 635
			public int Type;
		}

		// Token: 0x02000051 RID: 81
		public struct SYSTEM_INFO
		{
			// Token: 0x0400027C RID: 636
			public ushort processorArchitecture;

			// Token: 0x0400027D RID: 637
			private ushort reserved;

			// Token: 0x0400027E RID: 638
			public uint pageSize;

			// Token: 0x0400027F RID: 639
			public IntPtr minimumApplicationAddress;

			// Token: 0x04000280 RID: 640
			public IntPtr maximumApplicationAddress;

			// Token: 0x04000281 RID: 641
			public IntPtr activeProcessorMask;

			// Token: 0x04000282 RID: 642
			public uint numberOfProcessors;

			// Token: 0x04000283 RID: 643
			public uint processorType;

			// Token: 0x04000284 RID: 644
			public uint allocationGranularity;

			// Token: 0x04000285 RID: 645
			public ushort processorLevel;

			// Token: 0x04000286 RID: 646
			public ushort processorRevision;
		}

		// Token: 0x02000052 RID: 82
		public class CliArgs
		{
			// Token: 0x170000CD RID: 205
			// (get) Token: 0x060002F1 RID: 753 RVA: 0x000220D4 File Offset: 0x000202D4
			// (set) Token: 0x060002F2 RID: 754 RVA: 0x000220DC File Offset: 0x000202DC
			public List<string> searchterm { get; set; }

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x060002F3 RID: 755 RVA: 0x000220E5 File Offset: 0x000202E5
			// (set) Token: 0x060002F4 RID: 756 RVA: 0x000220ED File Offset: 0x000202ED
			public int prepostfix { get; set; }

			// Token: 0x170000CF RID: 207
			// (get) Token: 0x060002F5 RID: 757 RVA: 0x000220F6 File Offset: 0x000202F6
			// (set) Token: 0x060002F6 RID: 758 RVA: 0x000220FE File Offset: 0x000202FE
			public int delay { get; set; }

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x060002F7 RID: 759 RVA: 0x00022107 File Offset: 0x00020307
			// (set) Token: 0x060002F8 RID: 760 RVA: 0x0002210F File Offset: 0x0002030F
			public string mode { get; set; }
		}
	}
}
