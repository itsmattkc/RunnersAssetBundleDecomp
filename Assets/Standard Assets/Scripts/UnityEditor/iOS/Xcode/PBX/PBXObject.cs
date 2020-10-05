using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXObject
	{
		public string guid;

		protected PBXElementDict m_Properties = new PBXElementDict();

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker();

		internal virtual PropertyCommentChecker checker
		{
			get
			{
				return PBXObject.checkerData;
			}
		}

		internal virtual bool shouldCompact
		{
			get
			{
				return false;
			}
		}

		internal void SetPropertiesWhenSerializing(PBXElementDict props)
		{
			this.m_Properties = props;
		}

		internal PBXElementDict GetPropertiesWhenSerializing()
		{
			return this.m_Properties;
		}

		protected string GetPropertyString(string name)
		{
			PBXElement pBXElement = this.m_Properties[name];
			if (pBXElement == null)
			{
				return null;
			}
			return pBXElement.AsString();
		}

		protected void SetPropertyString(string name, string value)
		{
			if (value == null)
			{
				this.m_Properties.Remove(name);
			}
			else
			{
				this.m_Properties.SetString(name, value);
			}
		}

		protected List<string> GetPropertyList(string name)
		{
			PBXElement pBXElement = this.m_Properties[name];
			if (pBXElement == null)
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (PBXElement current in pBXElement.AsArray().values)
			{
				list.Add(current.AsString());
			}
			return list;
		}

		protected void SetPropertyList(string name, List<string> value)
		{
			if (value == null)
			{
				this.m_Properties.Remove(name);
			}
			else
			{
				PBXElementArray pBXElementArray = this.m_Properties.CreateArray(name);
				foreach (string current in value)
				{
					pBXElementArray.AddString(current);
				}
			}
		}

		public virtual void UpdateProps()
		{
		}

		public virtual void UpdateVars()
		{
		}
	}
}
