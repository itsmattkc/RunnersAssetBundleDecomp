using System;

namespace UnityEditor.iOS.Xcode
{
	public class PlistElementBoolean : PlistElement
	{
		public bool value;

		public PlistElementBoolean(bool v)
		{
			this.value = v;
		}
	}
}
