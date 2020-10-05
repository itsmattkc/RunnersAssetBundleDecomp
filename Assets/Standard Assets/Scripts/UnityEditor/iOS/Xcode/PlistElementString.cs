using System;

namespace UnityEditor.iOS.Xcode
{
	public class PlistElementString : PlistElement
	{
		public string value;

		public PlistElementString(string v)
		{
			this.value = v;
		}
	}
}
