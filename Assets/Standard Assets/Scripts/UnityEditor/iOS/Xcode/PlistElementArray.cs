using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
	public class PlistElementArray : PlistElement
	{
		public List<PlistElement> values = new List<PlistElement>();

		public void AddString(string val)
		{
			this.values.Add(new PlistElementString(val));
		}

		public void AddInteger(int val)
		{
			this.values.Add(new PlistElementInteger(val));
		}

		public void AddBoolean(bool val)
		{
			this.values.Add(new PlistElementBoolean(val));
		}

		public PlistElementArray AddArray()
		{
			PlistElementArray plistElementArray = new PlistElementArray();
			this.values.Add(plistElementArray);
			return plistElementArray;
		}

		public PlistElementDict AddDict()
		{
			PlistElementDict plistElementDict = new PlistElementDict();
			this.values.Add(plistElementDict);
			return plistElementDict;
		}
	}
}
