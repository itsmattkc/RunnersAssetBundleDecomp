using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXGroup : PBXObject
	{
		public GUIDList children;

		public PBXSourceTree tree;

		public string name;

		public string path;

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"children/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXGroup.checkerData;
			}
		}

		public static PBXGroup Create(string name, string path, PBXSourceTree tree)
		{
			if (name.Contains("/"))
			{
				throw new Exception("Group name must not contain '/'");
			}
			PBXGroup pBXGroup = new PBXGroup();
			pBXGroup.guid = PBXGUID.Generate();
			pBXGroup.SetPropertyString("isa", "PBXGroup");
			pBXGroup.name = name;
			pBXGroup.path = path;
			pBXGroup.tree = PBXSourceTree.Group;
			pBXGroup.children = new GUIDList();
			return pBXGroup;
		}

		public static PBXGroup CreateRelative(string name)
		{
			return PBXGroup.Create(name, name, PBXSourceTree.Group);
		}

		public override void UpdateProps()
		{
			base.SetPropertyList("children", this.children);
			if (this.name == this.path)
			{
				base.SetPropertyString("name", null);
			}
			else
			{
				base.SetPropertyString("name", this.name);
			}
			if (this.path == string.Empty)
			{
				base.SetPropertyString("path", null);
			}
			else
			{
				base.SetPropertyString("path", this.path);
			}
			base.SetPropertyString("sourceTree", FileTypeUtils.SourceTreeDesc(this.tree));
		}

		public override void UpdateVars()
		{
			this.children = base.GetPropertyList("children");
			this.path = base.GetPropertyString("path");
			this.name = base.GetPropertyString("name");
			if (this.name == null)
			{
				this.name = this.path;
			}
			if (this.path == null)
			{
				this.path = string.Empty;
			}
			this.tree = FileTypeUtils.ParseSourceTree(base.GetPropertyString("sourceTree"));
		}
	}
}
