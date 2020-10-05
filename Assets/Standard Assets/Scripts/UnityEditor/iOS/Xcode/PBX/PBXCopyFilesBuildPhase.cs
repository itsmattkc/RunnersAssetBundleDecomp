using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXCopyFilesBuildPhase : FileGUIDListBase
	{
		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"files/*"
		});

		public string name;

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXCopyFilesBuildPhase.checkerData;
			}
		}

		public static PBXCopyFilesBuildPhase Create(string name, string subfolderSpec)
		{
			PBXCopyFilesBuildPhase pBXCopyFilesBuildPhase = new PBXCopyFilesBuildPhase();
			pBXCopyFilesBuildPhase.guid = PBXGUID.Generate();
			pBXCopyFilesBuildPhase.SetPropertyString("isa", "PBXCopyFilesBuildPhase");
			pBXCopyFilesBuildPhase.SetPropertyString("buildActionMask", "2147483647");
			pBXCopyFilesBuildPhase.SetPropertyString("dstPath", string.Empty);
			pBXCopyFilesBuildPhase.SetPropertyString("dstSubfolderSpec", subfolderSpec);
			pBXCopyFilesBuildPhase.files = new List<string>();
			pBXCopyFilesBuildPhase.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
			pBXCopyFilesBuildPhase.name = name;
			return pBXCopyFilesBuildPhase;
		}

		public override void UpdateProps()
		{
			base.SetPropertyList("files", this.files);
			base.SetPropertyString("name", this.name);
		}

		public override void UpdateVars()
		{
			this.files = base.GetPropertyList("files");
			this.name = base.GetPropertyString("name");
		}
	}
}
