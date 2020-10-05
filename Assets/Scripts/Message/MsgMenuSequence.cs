using System;

namespace Message
{
	public class MsgMenuSequence : MessageBase
	{
		public enum SequeneceType
		{
			MAIN,
			TITLE,
			STAGE,
			STAGE_CHECK,
			EQUIP_ENTRANCE,
			PRESENT_BOX,
			DAILY_CHALLENGE,
			DAILY_BATTLE,
			CHARA_MAIN,
			CHAO,
			ITEM,
			PLAY_ITEM,
			OPTION,
			RANKING,
			RANKING_END,
			INFOMATION,
			ROULETTE,
			CHAO_ROULETTE,
			ITEM_ROULETTE,
			SHOP,
			EPISODE,
			EPISODE_PLAY,
			EPISODE_RANKING,
			QUICK,
			QUICK_RANKING,
			PLAY_AT_EPISODE_PAGE,
			MAIN_PLAY_BUTTON,
			TUTORIAL_PAGE_MOVE,
			CLOSE_DAILY_MISSION_WINDOW,
			EVENT_TOP,
			EVENT_SPECIAL,
			EVENT_RAID,
			EVENT_COLLECT,
			BACK,
			NON = -1
		}

		private MsgMenuSequence.SequeneceType m_sequenece_type;

		public MsgMenuSequence.SequeneceType Sequenece
		{
			get
			{
				return this.m_sequenece_type;
			}
		}

		public MsgMenuSequence(MsgMenuSequence.SequeneceType sequenece_type) : base(57344)
		{
			this.m_sequenece_type = sequenece_type;
		}
	}
}
