using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXElementDict : PBXElement
	{
		private SortedDictionary<string, PBXElement> m_PrivateValue = new SortedDictionary<string, PBXElement>();

		public IDictionary<string, PBXElement> values
		{
			get
			{
				return this.m_PrivateValue;
			}
		}

		public new PBXElement this[string key]
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

		public bool Contains(string key)
		{
			return this.values.ContainsKey(key);
		}

		public void Remove(string key)
		{
			this.values.Remove(key);
		}

		public void SetString(string key, string val)
		{
			this.values[key] = new PBXElementString(val);
		}

		public PBXElementArray CreateArray(string key)
		{
			PBXElementArray pBXElementArray = new PBXElementArray();
			this.values[key] = pBXElementArray;
			return pBXElementArray;
		}

		public PBXElementDict CreateDict(string key)
		{
			PBXElementDict pBXElementDict = new PBXElementDict();
			this.values[key] = pBXElementDict;
			return pBXElementDict;
		}
	}
}
