using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Azyre.Properties
{
	// Token: 0x0200002D RID: 45
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000D3EC File Offset: 0x0000B5EC
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x0400014E RID: 334
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
