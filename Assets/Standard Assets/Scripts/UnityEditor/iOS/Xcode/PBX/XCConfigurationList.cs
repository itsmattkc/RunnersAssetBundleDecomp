using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class XCConfigurationList : PBXObject
	{
		public GUIDList buildConfigs;

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"buildConfigurations/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return XCConfigurationList.checkerData;
			}
		}

		public static XCConfigurationList Create()
		{
			XCConfigurationList xCConfigurationList = new XCConfigurationList();
			xCConfigurationList.guid = PBXGUID.Generate();
			xCConfigurationList.SetPropertyString("isa", "XCConfigurationList");
			xCConfigurationList.buildConfigs = new GUIDList();
			xCConfigurationList.SetPropertyString("defaultConfigurationIsVisible", "0");
			return xCConfigurationList;
		}

		public override void UpdateProps()
		{
			base.SetPropertyList("buildConfigurations", this.buildConfigs);
		}

		public override void UpdateVars()
		{
			this.buildConfigs = base.GetPropertyList("buildConfigurations");
		}
	}
}
