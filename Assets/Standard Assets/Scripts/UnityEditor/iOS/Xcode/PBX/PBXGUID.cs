using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXGUID
	{
		internal delegate string GuidGenerator();

		private static PBXGUID.GuidGenerator guidGenerator = new PBXGUID.GuidGenerator(PBXGUID.DefaultGuidGenerator);

		internal static string DefaultGuidGenerator()
		{
			return Guid.NewGuid().ToString("N").Substring(8).ToUpper();
		}

		internal static void SetGuidGenerator(PBXGUID.GuidGenerator generator)
		{
			PBXGUID.guidGenerator = generator;
		}

		public static string Generate()
		{
			return PBXGUID.guidGenerator();
		}
	}
}
