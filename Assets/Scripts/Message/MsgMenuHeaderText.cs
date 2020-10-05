using System;

namespace Message
{
	public class MsgMenuHeaderText : MessageBase
	{
		public string m_cellName = string.Empty;

		public MsgMenuHeaderText(string cellName) : base(57344)
		{
			this.m_cellName = cellName;
		}
	}
}
