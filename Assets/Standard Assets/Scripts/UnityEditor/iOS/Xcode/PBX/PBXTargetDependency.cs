using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXTargetDependency : PBXObject
	{
		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"target/*",
			"targetProxy/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXTargetDependency.checkerData;
			}
		}

		public static PBXTargetDependency Create(string target, string targetProxy)
		{
			PBXTargetDependency pBXTargetDependency = new PBXTargetDependency();
			pBXTargetDependency.guid = PBXGUID.Generate();
			pBXTargetDependency.SetPropertyString("isa", "PBXTargetDependency");
			pBXTargetDependency.SetPropertyString("target", target);
			pBXTargetDependency.SetPropertyString("targetProxy", targetProxy);
			return pBXTargetDependency;
		}
	}
}
