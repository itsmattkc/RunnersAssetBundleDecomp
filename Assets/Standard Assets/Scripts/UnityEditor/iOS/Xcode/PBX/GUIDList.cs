using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class GUIDList : IEnumerable<string>, IEnumerable
	{
		private sealed class _RemoveGUID_c__AnonStorey8
		{
			internal string guid;

			internal bool __m__8(string x)
			{
				return x == this.guid;
			}
		}

		private List<string> m_List = new List<string>();

		public int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		public GUIDList()
		{
		}

		public GUIDList(List<string> data)
		{
			this.m_List = data;
		}

		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return this.m_List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_List.GetEnumerator();
		}

		public void AddGUID(string guid)
		{
			this.m_List.Add(guid);
		}

		public void RemoveGUID(string guid)
		{
			this.m_List.RemoveAll((string x) => x == guid);
		}

		public bool Contains(string guid)
		{
			return this.m_List.Contains(guid);
		}

		public void Clear()
		{
			this.m_List.Clear();
		}

		public static implicit operator List<string>(GUIDList list)
		{
			return list.m_List;
		}

		public static implicit operator GUIDList(List<string> data)
		{
			return new GUIDList(data);
		}
	}
}
