using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXElementString : PBXElement
	{
		public string value;

		public PBXElementString(string v)
		{
			this.value = v;
		}
	}
}
