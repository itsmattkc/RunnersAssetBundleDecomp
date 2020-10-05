using System;

public abstract class SocialTaskBase
{
	public enum ProcessState
	{
		NONE = -1,
		IDLE,
		PROCESSING,
		END
	}

	private SocialTaskBase.ProcessState m_state;

	public SocialTaskBase()
	{
		this.m_state = SocialTaskBase.ProcessState.IDLE;
	}

	public bool IsDone()
	{
		return this.m_state == SocialTaskBase.ProcessState.END;
	}

	public void Update()
	{
		switch (this.m_state)
		{
		case SocialTaskBase.ProcessState.IDLE:
			this.OnStartProcess();
			this.m_state = SocialTaskBase.ProcessState.PROCESSING;
			break;
		case SocialTaskBase.ProcessState.PROCESSING:
			this.OnUpdate();
			if (this.OnIsEndProcess())
			{
				this.m_state = SocialTaskBase.ProcessState.END;
			}
			break;
		}
	}

	public string GetTaskName()
	{
		return this.OnGetTaskName();
	}

	protected abstract void OnStartProcess();

	protected abstract void OnUpdate();

	protected abstract bool OnIsEndProcess();

	protected abstract string OnGetTaskName();
}
