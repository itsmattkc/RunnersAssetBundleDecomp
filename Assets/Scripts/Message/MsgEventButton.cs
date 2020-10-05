using System;

namespace Message
{
	public class MsgEventButton : MessageBase
	{
		public enum ButtonType
		{
			RAID_BOSS,
			RAID_BOSS_BACK,
			SPECIAL_STAGE,
			SPECIAL_STAGE_BACK,
			COLLECT_EVENT,
			COLLECT_EVENT_BACK,
			UNKNOWN
		}

		private MsgEventButton.ButtonType m_buttonType = MsgEventButton.ButtonType.UNKNOWN;

		public MsgEventButton.ButtonType Type
		{
			get
			{
				return this.m_buttonType;
			}
		}

		public MsgEventButton(MsgEventButton.ButtonType type) : base(57344)
		{
			this.m_buttonType = type;
		}
	}
}
