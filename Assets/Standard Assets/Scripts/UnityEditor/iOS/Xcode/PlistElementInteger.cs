using System;

namespace UnityEditor.iOS.Xcode
{
	public class PlistElementInteger : PlistElement
	{
		public int value;

		public PlistElementInteger(int v)
		{
			this.value = v;
		}
	}
}
