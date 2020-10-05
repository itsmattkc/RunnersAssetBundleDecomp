using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PropertyCommentChecker
	{
		private int m_Level;

		private bool m_All;

		private List<List<string>> m_Props;

		protected PropertyCommentChecker(int level, List<List<string>> props)
		{
			this.m_Level = level;
			this.m_All = false;
			this.m_Props = props;
		}

		public PropertyCommentChecker()
		{
			this.m_Level = 0;
			this.m_All = false;
			this.m_Props = new List<List<string>>();
		}

		public PropertyCommentChecker(IEnumerable<string> props)
		{
			this.m_Level = 0;
			this.m_All = false;
			this.m_Props = new List<List<string>>();
			foreach (string current in props)
			{
				this.m_Props.Add(new List<string>(current.Split(new char[]
				{
					'/'
				})));
			}
		}

		private bool CheckContained(string prop)
		{
			if (this.m_All)
			{
				return true;
			}
			foreach (List<string> current in this.m_Props)
			{
				if (current.Count == this.m_Level + 1)
				{
					if (current[this.m_Level] == prop)
					{
						bool result = true;
						return result;
					}
					if (current[this.m_Level] == "*")
					{
						this.m_All = true;
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		public bool CheckStringValueInArray(string value)
		{
			return this.CheckContained(value);
		}

		public bool CheckKeyInDict(string key)
		{
			return this.CheckContained(key);
		}

		public bool CheckStringValueInDict(string key, string value)
		{
			foreach (List<string> current in this.m_Props)
			{
				if (current.Count == this.m_Level + 2 && (((current[this.m_Level] == "*" || current[this.m_Level] == key) && current[this.m_Level + 1] == "*") || current[this.m_Level + 1] == value))
				{
					return true;
				}
			}
			return false;
		}

		public PropertyCommentChecker NextLevel(string prop)
		{
			List<List<string>> list = new List<List<string>>();
			foreach (List<string> current in this.m_Props)
			{
				if (current.Count > this.m_Level + 1)
				{
					if (current[this.m_Level] == "*" || current[this.m_Level] == prop)
					{
						list.Add(current);
					}
				}
			}
			return new PropertyCommentChecker(this.m_Level + 1, list);
		}
	}
}
