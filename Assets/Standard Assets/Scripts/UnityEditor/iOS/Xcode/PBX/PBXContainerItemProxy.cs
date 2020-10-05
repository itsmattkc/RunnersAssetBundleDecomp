using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXContainerItemProxy : PBXObject
	{
		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"containerPortal/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXContainerItemProxy.checkerData;
			}
		}

		public static PBXContainerItemProxy Create(string containerRef, string proxyType, string remoteGlobalGUID, string remoteInfo)
		{
			PBXContainerItemProxy pBXContainerItemProxy = new PBXContainerItemProxy();
			pBXContainerItemProxy.guid = PBXGUID.Generate();
			pBXContainerItemProxy.SetPropertyString("isa", "PBXContainerItemProxy");
			pBXContainerItemProxy.SetPropertyString("containerPortal", containerRef);
			pBXContainerItemProxy.SetPropertyString("proxyType", proxyType);
			pBXContainerItemProxy.SetPropertyString("remoteGlobalIDString", remoteGlobalGUID);
			pBXContainerItemProxy.SetPropertyString("remoteInfo", remoteInfo);
			return pBXContainerItemProxy;
		}
	}
}
