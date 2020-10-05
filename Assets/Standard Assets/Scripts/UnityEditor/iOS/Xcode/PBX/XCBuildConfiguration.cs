using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class XCBuildConfiguration : PBXObject
	{
		protected SortedDictionary<string, BuildConfigEntry> entries = new SortedDictionary<string, BuildConfigEntry>();

		public string name
		{
			get
			{
				return base.GetPropertyString("name");
			}
		}

		private static string EscapeWithQuotesIfNeeded(string name, string value)
		{
			if (name != "LIBRARY_SEARCH_PATHS")
			{
				return value;
			}
			if (!value.Contains(" "))
			{
				return value;
			}
			if (value.First<char>() == '"' && value.Last<char>() == '"')
			{
				return value;
			}
			return "\"" + value + "\"";
		}

		public void SetProperty(string name, string value)
		{
			this.entries[name] = BuildConfigEntry.FromNameValue(name, XCBuildConfiguration.EscapeWithQuotesIfNeeded(name, value));
		}

		public void AddProperty(string name, string value)
		{
			if (this.entries.ContainsKey(name))
			{
				this.entries[name].AddValue(XCBuildConfiguration.EscapeWithQuotesIfNeeded(name, value));
			}
			else
			{
				this.SetProperty(name, value);
			}
		}

		public void UpdateProperties(string name, string[] addValues, string[] removeValues)
		{
			if (this.entries.ContainsKey(name))
			{
				HashSet<string> hashSet = new HashSet<string>(this.entries[name].val);
				if (removeValues != null)
				{
					for (int i = 0; i < removeValues.Length; i++)
					{
						string value = removeValues[i];
						hashSet.Remove(XCBuildConfiguration.EscapeWithQuotesIfNeeded(name, value));
					}
				}
				if (addValues != null)
				{
					for (int j = 0; j < addValues.Length; j++)
					{
						string value2 = addValues[j];
						hashSet.Add(XCBuildConfiguration.EscapeWithQuotesIfNeeded(name, value2));
					}
				}
				this.entries[name].val = new List<string>(hashSet);
			}
		}

		public static XCBuildConfiguration Create(string name)
		{
			XCBuildConfiguration xCBuildConfiguration = new XCBuildConfiguration();
			xCBuildConfiguration.guid = PBXGUID.Generate();
			xCBuildConfiguration.SetPropertyString("isa", "XCBuildConfiguration");
			xCBuildConfiguration.SetPropertyString("name", name);
			return xCBuildConfiguration;
		}

		public override void UpdateProps()
		{
			PBXElementDict pBXElementDict = this.m_Properties.CreateDict("buildSettings");
			foreach (KeyValuePair<string, BuildConfigEntry> current in this.entries)
			{
				if (current.Value.val.Count != 0)
				{
					if (current.Value.val.Count == 1)
					{
						pBXElementDict.SetString(current.Key, current.Value.val[0]);
					}
					else
					{
						PBXElementArray pBXElementArray = pBXElementDict.CreateArray(current.Key);
						foreach (string current2 in current.Value.val)
						{
							pBXElementArray.AddString(current2);
						}
					}
				}
			}
		}

		public override void UpdateVars()
		{
			this.entries = new SortedDictionary<string, BuildConfigEntry>();
			if (this.m_Properties.Contains("buildSettings"))
			{
				PBXElementDict pBXElementDict = this.m_Properties["buildSettings"].AsDict();
				foreach (string current in pBXElementDict.values.Keys)
				{
					PBXElement pBXElement = pBXElementDict[current];
					if (pBXElement is PBXElementString)
					{
						if (this.entries.ContainsKey(current))
						{
							this.entries[current].val.Add(pBXElement.AsString());
						}
						else
						{
							this.entries.Add(current, BuildConfigEntry.FromNameValue(current, pBXElement.AsString()));
						}
					}
					else if (pBXElement is PBXElementArray)
					{
						foreach (PBXElement current2 in pBXElement.AsArray().values)
						{
							if (current2 is PBXElementString)
							{
								if (this.entries.ContainsKey(current))
								{
									this.entries[current].val.Add(current2.AsString());
								}
								else
								{
									this.entries.Add(current, BuildConfigEntry.FromNameValue(current, current2.AsString()));
								}
							}
						}
					}
				}
			}
		}
	}
}
