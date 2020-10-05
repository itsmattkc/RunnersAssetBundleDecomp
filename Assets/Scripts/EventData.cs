using System;

public class EventData
{
	public PointEventData[] point;

	public bool IsBossEvent()
	{
		int num = this.point.Length;
		return num == 6 && this.point[5].boss.boss_flag == 1;
	}

	public BossEvent GetBossEvent()
	{
		if (this.IsBossEvent())
		{
			return this.point[5].boss;
		}
		return new BossEvent();
	}
}
