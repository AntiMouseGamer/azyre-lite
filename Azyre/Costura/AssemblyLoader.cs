using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura
{
	// Token: 0x02000073 RID: 115
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		// Token: 0x0600037D RID: 893 RVA: 0x00031812 File Offset: 0x0002FA12
		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return string.Empty;
			}
			return culture.Name;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00031824 File Offset: 0x0002FA24
		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00031894 File Offset: 0x0002FA94
		private static string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = (AssemblyLoader.CultureToString(requestedAssemblyName.CultureInfo) + "." + text).ToLowerInvariant();
			}
			return text;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000318E4 File Offset: 0x0002FAE4
		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00031918 File Offset: 0x0002FB18
		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
					{
						MemoryStream memoryStream = new MemoryStream();
						AssemblyLoader.CopyTo(deflateStream, memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0003199C File Offset: 0x0002FB9C
		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			string fullName;
			if (resourceNames.TryGetValue(name, out fullName))
			{
				return AssemblyLoader.LoadStream(fullName);
			}
			return null;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000319BC File Offset: 0x0002FBBC
		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000319E4 File Offset: 0x0002FBE4
		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string assemblyResourceName = AssemblyLoader.GetAssemblyResourceName(requestedAssemblyName);
			byte[] rawAssembly;
			using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, assemblyResourceName))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = AssemblyLoader.ReadStream(stream);
			}
			using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, assemblyResourceName))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00031A70 File Offset: 0x0002FC70
		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			string name = e.Name;
			AssemblyName assemblyName = new AssemblyName(name);
			object obj = AssemblyLoader.nullCacheLock;
			lock (obj)
			{
				if (AssemblyLoader.nullCache.ContainsKey(name))
				{
					return null;
				}
			}
			Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
			if (assembly == null)
			{
				object obj2 = AssemblyLoader.nullCacheLock;
				lock (obj2)
				{
					AssemblyLoader.nullCache[name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00031B40 File Offset: 0x0002FD40
		// Note: this type is marked as 'beforefieldinit'.
		static AssemblyLoader()
		{
			AssemblyLoader.assemblyNames.Add("beautyui", "costura.beautyui.dll.compressed");
			AssemblyLoader.assemblyNames.Add("bleak", "costura.bleak.dll.compressed");
			AssemblyLoader.assemblyNames.Add("costura", "costura.costura.dll.compressed");
			AssemblyLoader.symbolNames.Add("costura", "costura.costura.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("microsoft.bcl.asyncinterfaces", "costura.microsoft.bcl.asyncinterfaces.dll.compressed");
			AssemblyLoader.assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.buffers", "costura.system.buffers.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.io.pipelines", "costura.system.io.pipelines.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.memory", "costura.system.memory.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.security.cryptography.pkcs", "costura.system.security.cryptography.pkcs.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.text.encodings.web", "costura.system.text.encodings.web.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.text.json", "costura.system.text.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.threading.tasks.extensions", "costura.system.threading.tasks.extensions.dll.compressed");
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00031CA4 File Offset: 0x0002FEA4
		public static void Attach(bool subscribe)
		{
			if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
			{
				return;
			}
			if (subscribe)
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				AppDomain appDomain = currentDomain;
				ResolveEventHandler value;
				if ((value = AssemblyLoader.ResolveCache.CachedResolveHandler) == null)
				{
					value = (AssemblyLoader.ResolveCache.CachedResolveHandler = new ResolveEventHandler(AssemblyLoader.ResolveAssembly));
				}
				appDomain.AssemblyResolve += value;
			}
		}

		// Token: 0x04000388 RID: 904
		private static object nullCacheLock = new object();

		// Token: 0x04000389 RID: 905
		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		// Token: 0x0400038A RID: 906
		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		// Token: 0x0400038B RID: 907
		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		// Token: 0x0400038C RID: 908
		private static int isAttached;

		// Token: 0x02000074 RID: 116
		[CompilerGenerated]
		private static class ResolveCache
		{
			// Token: 0x0400038D RID: 909
			public static ResolveEventHandler CachedResolveHandler;
		}
	}
}
