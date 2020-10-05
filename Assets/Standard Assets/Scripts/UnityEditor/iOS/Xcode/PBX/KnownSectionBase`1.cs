using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class KnownSectionBase<T> : SectionBase where T : PBXObject, new()
	{
		public SortedDictionary<string, T> entries = new SortedDictionary<string, T>();

		private string m_Name;

		public T this[string guid]
		{
			get
			{
				if (this.entries.ContainsKey(guid))
				{
					return this.entries[guid];
				}
				return (T)((object)null);
			}
		}

		public KnownSectionBase(string sectionName)
		{
			this.m_Name = sectionName;
		}

		public override void AddObject(string key, PBXElementDict value)
		{
			T t = Activator.CreateInstance<T>();
			t.guid = key;
			t.SetPropertiesWhenSerializing(value);
			t.UpdateVars();
			this.entries[t.guid] = t;
		}

		public override void WriteSection(StringBuilder sb, GUIDToCommentMap comments)
		{
			if (this.entries.Count == 0)
			{
				return;
			}
			sb.AppendFormat("\n\n/* Begin {0} section */", this.m_Name);
			foreach (T current in this.entries.Values)
			{
				current.UpdateProps();
				sb.AppendFormat("\n\t\t{0} = ", comments.Write(current.guid));
				Serializer.WriteDict(sb, current.GetPropertiesWhenSerializing(), 2, current.shouldCompact, current.checker, comments);
				sb.Append(";");
			}
			sb.AppendFormat("\n/* End {0} section */", this.m_Name);
		}

		public void AddEntry(T obj)
		{
			this.entries[obj.guid] = obj;
		}

		public void RemoveEntry(string guid)
		{
			if (this.entries.ContainsKey(guid))
			{
				this.entries.Remove(guid);
			}
		}
	}
}
