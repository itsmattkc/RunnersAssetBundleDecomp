using System;
using System.Runtime.CompilerServices;

namespace Text
{
	internal class CellData
	{
		private string _m_cellID_k__BackingField;

		private string _m_cellString_k__BackingField;

		public string m_cellID
		{
			get;
			private set;
		}

		public string m_cellString
		{
			get;
			private set;
		}

		public CellData(string cellID, string cellString)
		{
			this.m_cellID = cellID;
			this.m_cellString = cellString;
		}
	}
}
