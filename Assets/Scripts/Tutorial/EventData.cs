using System;

namespace Tutorial
{
	public class EventData
	{
		private static readonly EventClearType[] EVENT_TYPE_TBL;

		static EventData()
		{
			// Note: this type is marked as 'beforefieldinit'.
			EventClearType[] expr_06 = new EventClearType[8];
			expr_06[4] = EventClearType.NO_DAMAGE;
			expr_06[5] = EventClearType.NO_MISS;
			EventData.EVENT_TYPE_TBL = expr_06;
		}

		public static EventClearType GetEventClearType(EventID id)
		{
			if (id < EventID.NUM)
			{
				return EventData.EVENT_TYPE_TBL[(int)id];
			}
			return EventClearType.CLEAR;
		}
	}
}
