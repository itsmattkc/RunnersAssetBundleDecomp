using DataTable;
using System;

namespace Mission
{
	public abstract class MissionCheck
	{
		protected MissionData m_data;

		protected MissionCompleteFunc m_funcMissionComplete;

		protected bool m_isCompleted;

		public abstract void ProcEvent(MissionEvent missionEvent);

		public virtual void Update(float deltaTime)
		{
		}

		public virtual void SetInitialValue(long initialValue)
		{
		}

		public virtual long GetValue()
		{
			return 0L;
		}

		public void SetData(MissionData data)
		{
			this.m_data = data;
		}

		public MissionData GetData()
		{
			return this.m_data;
		}

		public virtual void SetDataExtra()
		{
		}

		public bool IsCompleted()
		{
			return this.m_isCompleted;
		}

		public int GetIndex()
		{
			return this.m_data.id;
		}

		public void SetOnCompleteFunc(MissionCompleteFunc func)
		{
			this.m_funcMissionComplete = (MissionCompleteFunc)Delegate.Combine(this.m_funcMissionComplete, func);
		}

		public virtual void DebugComplete(int missionNo)
		{
			this.SetCompleted();
		}

		protected void SetCompleted()
		{
			this.m_isCompleted = true;
			if (this.m_funcMissionComplete != null)
			{
				this.m_funcMissionComplete(this.m_data);
			}
		}
	}
}
