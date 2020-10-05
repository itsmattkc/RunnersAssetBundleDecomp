using System;

namespace Message
{
	public class MsgMenuItemSetStart : MessageBase
	{
		public enum SetMode
		{
			NORMAL,
			TUTORIAL,
			TUTORIAL_SUBCHARA
		}

		public MsgMenuItemSetStart.SetMode m_setMode;

		public MsgMenuItemSetStart(MsgMenuItemSetStart.SetMode mode) : base(57344)
		{
			this.m_setMode = mode;
		}
	}
}
