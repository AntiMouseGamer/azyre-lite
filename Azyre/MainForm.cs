using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeautyUI;
using BeautyUI.Components;
using BeautyUI2.Controls;
using Bleak;
using Azyre.MH;
using Azyre.Utils;

namespace Azyre
{
	// Token: 0x02000021 RID: 33
	public partial class MainForm : Form
	{
		// Token: 0x060001B3 RID: 435
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x060001B4 RID: 436
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B06E File Offset: 0x0000926E
		public MainForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B07C File Offset: 0x0000927C
		private async void MainForm_Load(object sender, EventArgs e)
		{
			SetStage(0, "Iniciando...");
			try
			{
				File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "azyre_log.txt"));
			}
			catch
			{
			}
			Azyre.MH.dllconnect.LogPipe("=== loader iniciado ===");
			try
			{
				Timer statusTimer = new Timer();
				statusTimer.Interval = 1000;
				statusTimer.Tick += delegate
				{
					try
					{
						this.Text = "Azyre - Pipe: " + Azyre.MH.dllconnect.LastPipeStatus + (string.IsNullOrEmpty(Azyre.MH.dllconnect.LastPipeError) ? "" : " | ERRO: " + Azyre.MH.dllconnect.LastPipeError);
					}
					catch
					{
					}
				};
				statusTimer.Start();
			}
			catch
			{
			}
			try
			{
				Azyre.Utils.Binds.setupBindListener();
			}
			catch (Exception ex)
			{
				Azyre.MH.dllconnect.LogPipe("ERRO binds: " + ex.Message);
				FailLoader("Erro nos binds: " + ex.Message);
				return;
			}
			try
			{
				Azyre.Utils.Binds.ListenForKeyPress();
			}
			catch
			{
			}

			SetStage(0, "Verificando arquivos...");
			string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Azyre.dll");
			if (!File.Exists(dllPath))
			{
				Azyre.MH.dllconnect.LogPipe("ERRO: Azyre.dll nao encontrada em " + dllPath);
				FailLoader("Azyre.dll não encontrada na pasta do programa.\nColoque a Azyre.dll junto do Azyre.exe.");
				return;
			}
			Azyre.MH.dllconnect.LogPipe("Azyre.dll encontrada: " + dllPath);

			SetStage(1, "Iniciando pipe...");
			try
			{
				Azyre.MH.dllconnect.CriarServidorPipe();
			}
			catch (Exception ex)
			{
				Azyre.MH.dllconnect.LogPipe("ERRO pipe: " + ex.Message);
				FailLoader("Erro no pipe: " + ex.Message);
				return;
			}

			Process mc = await FindMinecraftAsync();
			if (mc == null)
			{
				Azyre.MH.dllconnect.LogPipe("Minecraft nao encontrado (usuario cancelou). Fechando.");
				base.Close();
				return;
			}
			try
			{
				Azyre.MH.dllconnect.LogPipe("Minecraft encontrado: " + mc.ProcessName + " (pid " + mc.Id + ").");
			}
			catch
			{
			}

			SetStage(3, "Injetando...");
			Azyre.MH.dllconnect.LogPipe("injetando Azyre.dll no pid " + mc.Id + "...");
			if (!await Task.Run(() => InjectDll(mc.Id, dllPath)))
			{
				Azyre.MH.dllconnect.LogPipe("ERRO: injecao falhou.");
				base.Close();
				return;
			}
			Azyre.MH.dllconnect.LogPipe("injecao OK.");

			SetStage(4, "Carregando...");
			await Task.Delay(600);
			await SwitchToMainUI();
			Azyre.MH.dllconnect.LogPipe("UI principal aberta. Aguardando DLL conectar no pipe...");
			await WaitForPipeAsync();
		}

		private void FailLoader(string message)
		{
			Azyre.MH.dllconnect.LogPipe("FALHA loader: " + message);
			try
			{
				MessageBox.Show(message, "Azyre", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
			}
			try
			{
				base.Close();
			}
			catch
			{
			}
		}

		private void SetStage(int stage, string text)
		{
			try
			{
				if (this.beautyDotsLoader1 != null && !this.beautyDotsLoader1.IsDisposed)
				{
					int count = this.beautyDotsLoader1.StageCount;
					this.beautyDotsLoader1.CurrentStage = count > 0 ? stage % (count + 1) : stage;
					this.beautyDotsLoader1.StageText = text;
					this.beautyDotsLoader1.Refresh();
				}
			}
			catch
			{
			}
		}

		private async Task<Process> FindMinecraftAsync()
		{
			int waited = 0;
			for (;;)
			{
				Process mc = FindMinecraftProcess();
				if (mc != null)
				{
					return mc;
				}
				SetStage(1 + (waited % 2), "Procurando Minecraft... (" + waited + "s)");
				await Task.Delay(1000);
				waited++;
				if (waited >= 30)
				{
					DialogResult r = MessageBox.Show("Minecraft não encontrado (procurei por javaw.exe e java.exe). Abra o Minecraft e clique em Repetir para tentar de novo.", "Azyre", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
					if (r != DialogResult.Retry)
					{
						return null;
					}
					waited = 0;
				}
			}
		}

		private static Process FindMinecraftProcess()
		{
			try
			{
				List<Process> candidates = new List<Process>();
				try { candidates.AddRange(Process.GetProcessesByName("javaw")); } catch { }
				try { candidates.AddRange(Process.GetProcessesByName("java")); } catch { }
				if (candidates.Count == 0)
				{
					return null;
				}
				foreach (Process p in candidates)
				{
					try
					{
						if (p.HasExited)
						{
							continue;
						}
						string title = p.MainWindowTitle ?? string.Empty;
						if (title.ToLowerInvariant().Contains("minecraft"))
						{
							return p;
						}
					}
					catch
					{
					}
				}
				foreach (Process p in candidates)
				{
					try
					{
						if (!p.HasExited)
						{
							return p;
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private bool InjectDll(int pid, string dllPath)
		{
			try
			{
				if (MainForm.injector != null)
				{
					try
					{
						MainForm.injector.Dispose();
					}
					catch
					{
					}
				}
				MainForm.injector = new Injector(InjectionMethod.ManualMap, pid, dllPath, false);
				if (MainForm.injector.InjectDll() == IntPtr.Zero)
				{
					MessageBox.Show("Falha ao injetar a Azyre.dll no Minecraft.", "Azyre", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Falha ao injetar: " + ex.Message, "Azyre", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private async Task WaitForPipeAsync()
		{
			try
			{
				for (int i = 0; i < 300; i++)
				{
					if (Azyre.MH.dllconnect.pipeServer != null && Azyre.MH.dllconnect.pipeServer.IsConnected)
					{
						Azyre.MH.dllconnect.LogPipe("pipe conectado apos UI, reenviando config...");
						Azyre.MH.dllconnect.EnviarConfiguracoes();
						return;
					}
					await Task.Delay(100);
				}
				Azyre.MH.dllconnect.LogPipe("AVISO: 30s sem conexao da DLL no pipe.");
			}
			catch
			{
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B0B4 File Offset: 0x000092B4
		private async Task RunLoader()
		{
			try
			{
				if (this.beautyDotsLoader1 == null)
				{
					return;
				}
				int count = this.beautyDotsLoader1.StageCount;
				if (count <= 0)
				{
					count = 5;
				}
				for (int i = 0; i < count; i++)
				{
					this.beautyDotsLoader1.CurrentStage = i;
					this.beautyDotsLoader1.StageText = "Loading...";
					await Task.Delay(400);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B0F8 File Offset: 0x000092F8
		private async Task SwitchToMainUI()
		{
			try
			{
				this.BackPanel.Controls.Clear();
				Main main = new Main();
				main.Dock = DockStyle.Fill;
				this.BackPanel.Controls.Add(main);
				await Task.CompletedTask;
			}
			catch
			{
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B13C File Offset: 0x0000933C
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (MainForm.injector != null)
				{
					try
					{
						MainForm.injector.Dispose();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B16C File Offset: 0x0000936C
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			// No action needed after the form is closed.
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B19B File Offset: 0x0000939B
		private void CombatButton_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0400010E RID: 270
		public static Injector injector;
	}
}
