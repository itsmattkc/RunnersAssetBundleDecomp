using System;

namespace Boss
{
	public class BossStateAppearEvent2 : BossStateAppearEventBase
	{
		protected override EVENTBOSS_STATE_ID GetNextChangeState()
		{
			return EVENTBOSS_STATE_ID.AttackEvent2;
		}
	}
}
