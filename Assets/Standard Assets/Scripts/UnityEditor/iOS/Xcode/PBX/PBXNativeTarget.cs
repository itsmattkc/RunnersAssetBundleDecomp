using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXNativeTarget : PBXObject
	{
		public GUIDList phases;

		public string buildConfigList;

		public string name;

		public GUIDList dependencies;

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"buildPhases/*",
			"buildRules/*",
			"dependencies/*",
			"productReference/*",
			"buildConfigurationList/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXNativeTarget.checkerData;
			}
		}

		public static PBXNativeTarget Create(string name, string productRef, string productType, string buildConfigList)
		{
			PBXNativeTarget pBXNativeTarget = new PBXNativeTarget();
			pBXNativeTarget.guid = PBXGUID.Generate();
			pBXNativeTarget.SetPropertyString("isa", "PBXNativeTarget");
			pBXNativeTarget.buildConfigList = buildConfigList;
			pBXNativeTarget.phases = new GUIDList();
			pBXNativeTarget.SetPropertyList("buildRules", new List<string>());
			pBXNativeTarget.dependencies = new GUIDList();
			pBXNativeTarget.name = name;
			pBXNativeTarget.SetPropertyString("productName", name);
			pBXNativeTarget.SetPropertyString("productReference", productRef);
			pBXNativeTarget.SetPropertyString("productType", productType);
			return pBXNativeTarget;
		}

		public override void UpdateProps()
		{
			base.SetPropertyString("buildConfigurationList", this.buildConfigList);
			base.SetPropertyString("name", this.name);
			base.SetPropertyList("buildPhases", this.phases);
			base.SetPropertyList("dependencies", this.dependencies);
		}

		public override void UpdateVars()
		{
			this.buildConfigList = base.GetPropertyString("buildConfigurationList");
			this.name = base.GetPropertyString("name");
			this.phases = base.GetPropertyList("buildPhases");
			this.dependencies = base.GetPropertyList("dependencies");
		}
	}
}
