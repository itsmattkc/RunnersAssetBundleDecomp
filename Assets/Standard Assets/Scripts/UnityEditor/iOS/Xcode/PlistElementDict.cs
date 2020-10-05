using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
	public class PlistElementDict : PlistElement
	{
		private SortedDictionary<string, PlistElement> m_PrivateValue = new SortedDictionary<string, PlistElement>();

		public IDictionary<string, PlistElement> values
		{
			get
			{
				return this.m_PrivateValue;
			}
		}

		public new PlistElement this[string key]
		{
			get
			{
				if (this.values.ContainsKey(key))
				{
					return this.values[key];
				}
				return null;
			}
			set
			{
				this.values[key] = value;
			}
		}

		public void SetInteger(string key, int val)
		{
			this.values[key] = new PlistElementInteger(val);
		}

		public void SetString(string key, string val)
		{
			this.values[key] = new PlistElementString(val);
		}

		public void SetBoolean(string key, bool val)
		{
			this.values[key] = new PlistElementBoolean(val);
		}

		public PlistElementArray CreateArray(string key)
		{
			PlistElementArray plistElementArray = new PlistElementArray();
			this.values[key] = plistElementArray;
			return plistElementArray;
		}

		public PlistElementDict CreateDict(string key)
		{
			PlistElementDict plistElementDict = new PlistElementDict();
			this.values[key] = plistElementDict;
			return plistElementDict;
		}
	}
}
