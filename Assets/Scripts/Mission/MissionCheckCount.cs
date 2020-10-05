using System;

namespace Mission
{
	public class MissionCheckCount : MissionCheck
	{
		private long m_count;

		private EventID m_eventID;

		public MissionCheckCount(EventID id)
		{
			this.m_eventID = id;
		}

		public override void ProcEvent(MissionEvent missionEvent)
		{
			this.m_count = this.CheckCompletedAddCount(missionEvent, this.m_eventID, this.m_count);
		}

		public override void SetInitialValue(long initialValue)
		{
			this.m_count = initialValue;
		}

		public override long GetValue()
		{
			return this.m_count;
		}

		private long CheckCompletedAddCount(MissionEvent missionEvent, EventID check_id, long in_count)
		{
			long num = in_count;
			if (!base.IsCompleted() && check_id == missionEvent.m_id)
			{
				num += missionEvent.m_num;
				if (num >= (long)this.m_data.quota)
				{
					base.SetCompleted();
				}
			}
			return num;
		}
	}
}
