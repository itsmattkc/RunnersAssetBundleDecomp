using System;
using System.IO;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXFileReference : PBXObject
	{
		public string path;

		public string name;

		public PBXSourceTree tree;

		internal override bool shouldCompact
		{
			get
			{
				return true;
			}
		}

		public static PBXFileReference CreateFromFile(string path, string projectFileName, PBXSourceTree tree)
		{
			string guid = PBXGUID.Generate();
			PBXFileReference pBXFileReference = new PBXFileReference();
			pBXFileReference.SetPropertyString("isa", "PBXFileReference");
			pBXFileReference.guid = guid;
			pBXFileReference.path = path;
			pBXFileReference.name = projectFileName;
			pBXFileReference.tree = tree;
			return pBXFileReference;
		}

		public override void UpdateProps()
		{
			string text = null;
			if (this.name != null)
			{
				text = Path.GetExtension(this.name);
			}
			else if (this.path != null)
			{
				text = Path.GetExtension(this.path);
			}
			if (text != null)
			{
				if (FileTypeUtils.IsFileTypeExplicit(text))
				{
					base.SetPropertyString("explicitFileType", FileTypeUtils.GetTypeName(text));
				}
				else
				{
					base.SetPropertyString("lastKnownFileType", FileTypeUtils.GetTypeName(text));
				}
			}
			if (this.path == this.name)
			{
				base.SetPropertyString("name", null);
			}
			else
			{
				base.SetPropertyString("name", this.name);
			}
			if (this.path == null)
			{
				base.SetPropertyString("path", string.Empty);
			}
			else
			{
				base.SetPropertyString("path", this.path);
			}
			base.SetPropertyString("sourceTree", FileTypeUtils.SourceTreeDesc(this.tree));
		}

		public override void UpdateVars()
		{
			this.name = base.GetPropertyString("name");
			this.path = base.GetPropertyString("path");
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
