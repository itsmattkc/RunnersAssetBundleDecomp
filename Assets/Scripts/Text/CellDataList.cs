using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Text
{
	internal class CellDataList
	{
		private Dictionary<string, CellData> m_cellDataList;

		private string _m_categoryName_k__BackingField;

		public string m_categoryName
		{
			get;
			private set;
		}

		public CellDataList(string categoryName)
		{
			this.m_categoryName = categoryName;
			this.m_cellDataList = new Dictionary<string, CellData>();
		}

		public void Add(CellData cellData)
		{
			if (cellData == null)
			{
				return;
			}
			if (this.m_cellDataList.ContainsKey(cellData.m_cellID))
			{
				UnityEngine.Debug.LogWarning("CellDataList.Add() same cellID = " + cellData.m_cellID);
				return;
			}
			this.m_cellDataList.Add(cellData.m_cellID, cellData);
		}

		public CellData Get(string searchId)
		{
			if (!this.m_cellDataList.ContainsKey(searchId))
			{
				return null;
			}
			return this.m_cellDataList[searchId];
		}

		public int GetCount()
		{
			return this.m_cellDataList.Count;
		}
	}
}
