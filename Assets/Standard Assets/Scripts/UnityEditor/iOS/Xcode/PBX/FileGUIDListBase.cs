using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class FileGUIDListBase : PBXObject
	{
		public GUIDList files;

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"files/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return FileGUIDListBase.checkerData;
			}
		}

		public override void UpdateProps()
		{
			base.SetPropertyList("files", this.files);
		}

		public override void UpdateVars()
		{
			this.files = base.GetPropertyList("files");
		}
	}
}
