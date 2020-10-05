using System;

namespace Message
{
	public class MsgMenuButtonEvent : MessageBase
	{
		private ButtonInfoTable.ButtonType m_buttonType;

		public bool m_clearHistories;

		public ButtonInfoTable.ButtonType ButtonType
		{
			get
			{
				return this.m_buttonType;
			}
		}

		public MsgMenuButtonEvent(ButtonInfoTable.ButtonType type) : base(57344)
		{
			this.m_buttonType = type;
		}
	}
}
