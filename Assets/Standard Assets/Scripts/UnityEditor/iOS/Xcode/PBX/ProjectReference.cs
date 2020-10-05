using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class ProjectReference
	{
		public string group;

		public string projectRef;

		public static ProjectReference Create(string group, string projectRef)
		{
			return new ProjectReference
			{
				group = group,
				projectRef = projectRef
			};
		}
	}
}
