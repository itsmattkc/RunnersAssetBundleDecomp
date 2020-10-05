using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXProjectSection : KnownSectionBase<PBXProjectObject>
	{
		public PBXProjectObject project
		{
			get
			{
				using (SortedDictionary<string, PBXProjectObject>.Enumerator enumerator = this.entries.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<string, PBXProjectObject> current = enumerator.Current;
						return current.Value;
					}
				}
				return null;
			}
		}

		public PBXProjectSection() : base("PBXProject")
		{
		}
	}
}
