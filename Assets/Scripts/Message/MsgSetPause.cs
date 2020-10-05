using System;

namespace Message
{
	public class MsgSetPause : MessageBase
	{
		public bool m_backMainMenu;

		public bool m_backKey;

		public MsgSetPause(bool backMainMenu, bool backKey) : base(12359)
		{
			this.m_backMainMenu = backMainMenu;
			this.m_backKey = backKey;
		}
	}
}
