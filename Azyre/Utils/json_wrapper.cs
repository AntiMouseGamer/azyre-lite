using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Azyre.Utils
{
	// Token: 0x02000037 RID: 55
	public class json_wrapper
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000E294 File Offset: 0x0000C494
		public static bool is_serializable(Type to_check)
		{
			return to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E2B4 File Offset: 0x0000C4B4
		public json_wrapper(object obj_to_work_with)
		{
			this.current_object = obj_to_work_with;
			Type type = this.current_object.GetType();
			this.serializer = new DataContractJsonSerializer(type);
			if (!json_wrapper.is_serializable(type))
			{
				throw new Exception(string.Format("the object {0} isn't a serializable", this.current_object));
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E304 File Offset: 0x0000C504
		public object string_to_object(string json)
		{
			object result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(json)))
			{
				result = this.serializer.ReadObject(memoryStream);
			}
			return result;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E34C File Offset: 0x0000C54C
		public T string_to_generic<T>(string json)
		{
			return (T)((object)this.string_to_object(json));
		}

		// Token: 0x04000177 RID: 375
		private DataContractJsonSerializer serializer;

		// Token: 0x04000178 RID: 376
		private object current_object;
	}
}
