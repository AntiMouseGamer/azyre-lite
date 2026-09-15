using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Azyre.Properties
{
	// Token: 0x0200002C RID: 44
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x000054B4 File Offset: 0x000036B4
		internal Resources()
		{
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000D3B1 File Offset: 0x0000B5B1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Azyre.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000D3DD File Offset: 0x0000B5DD
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x0400014C RID: 332
		private static ResourceManager resourceMan;

		// Token: 0x0400014D RID: 333
		private static CultureInfo resourceCulture;
	}
}
