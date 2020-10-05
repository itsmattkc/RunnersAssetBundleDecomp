using System;
using System.Collections.Generic;

public class SocialTaskManager
{
	private List<SocialTaskBase> m_taskList;

	public SocialTaskManager()
	{
		this.m_taskList = new List<SocialTaskBase>();
	}

	public void RequestProcess(SocialTaskBase task)
	{
		if (task == null)
		{
			return;
		}
		string taskName = task.GetTaskName();
		UnityEngine.Debug.Log("SocialTaskManager:Request Process  " + taskName);
		this.m_taskList.Add(task);
	}

	public void Update()
	{
		if (this.m_taskList.Count <= 0)
		{
			return;
		}
		List<SocialTaskBase> list = new List<SocialTaskBase>();
		SocialTaskBase socialTaskBase = this.m_taskList[0];
		socialTaskBase.Update();
		if (socialTaskBase.IsDone())
		{
			string taskName = socialTaskBase.GetTaskName();
			UnityEngine.Debug.Log("SocialTaskManager:" + taskName + " is Done");
			list.Add(socialTaskBase);
		}
		if (list.Count > 0)
		{
			foreach (SocialTaskBase current in list)
			{
				this.m_taskList.Remove(current);
			}
			list.Clear();
		}
	}
}
