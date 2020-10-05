using System;

namespace Message
{
	public class MsgTutorialAction : MessageBase
	{
		public HudTutorial.Id m_id;

		public MsgTutorialAction(HudTutorial.Id id) : base(12344)
		{
			this.m_id = id;
		}
	}
}
