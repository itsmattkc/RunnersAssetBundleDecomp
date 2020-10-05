using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXProjectObject : PBXObject
	{
		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"buildConfigurationList/*",
			"mainGroup/*",
			"projectReferences/*/ProductGroup/*",
			"projectReferences/*/ProjectRef/*",
			"targets/*"
		});

		public List<ProjectReference> projectReferences = new List<ProjectReference>();

		public string buildConfigList;

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXProjectObject.checkerData;
			}
		}

		public string mainGroup
		{
			get
			{
				return base.GetPropertyString("mainGroup");
			}
		}

		public List<string> targets
		{
			get
			{
				return base.GetPropertyList("targets");
			}
		}

		public void AddReference(string productGroup, string projectRef)
		{
			this.projectReferences.Add(ProjectReference.Create(productGroup, projectRef));
		}

		public override void UpdateProps()
		{
			this.m_Properties.values.Remove("projectReferences");
			if (this.projectReferences.Count > 0)
			{
				PBXElementArray pBXElementArray = this.m_Properties.CreateArray("projectReferences");
				foreach (ProjectReference current in this.projectReferences)
				{
					PBXElementDict pBXElementDict = pBXElementArray.AddDict();
					pBXElementDict.SetString("ProductGroup", current.group);
					pBXElementDict.SetString("ProjectRef", current.projectRef);
				}
			}
			base.SetPropertyString("buildConfigurationList", this.buildConfigList);
		}

		public override void UpdateVars()
		{
			this.projectReferences = new List<ProjectReference>();
			if (this.m_Properties.Contains("projectReferences"))
			{
				PBXElementArray pBXElementArray = this.m_Properties["projectReferences"].AsArray();
				foreach (PBXElement current in pBXElementArray.values)
				{
					PBXElementDict pBXElementDict = current.AsDict();
					if (pBXElementDict.Contains("ProductGroup") && pBXElementDict.Contains("ProjectRef"))
					{
						string group = pBXElementDict["ProductGroup"].AsString();
						string projectRef = pBXElementDict["ProjectRef"].AsString();
						this.projectReferences.Add(ProjectReference.Create(group, projectRef));
					}
				}
			}
			this.buildConfigList = base.GetPropertyString("buildConfigurationList");
		}
	}
}
