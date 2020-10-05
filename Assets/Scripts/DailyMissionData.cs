using System;

public class DailyMissionData
{
	private enum Reward
	{
		DAYS = 7
	}

	public int id;

	public long progress;

	public int date;

	public int[] reward_id = new int[7];

	public int[] reward_count = new int[7];

	public int reward_max;

	public int max;

	public int clear_count;

	public bool missions_complete;

	public DailyMissionData()
	{
		this.id = 1;
		this.progress = 0L;
		this.date = 1;
		this.reward_max = 3;
		for (int i = 0; i < 7; i++)
		{
			this.reward_id[i] = 0;
			this.reward_count[i] = 1;
		}
		this.clear_count = 0;
		this.missions_complete = false;
	}

	public void CopyTo(DailyMissionData dst)
	{
		dst.id = this.id;
		dst.progress = this.progress;
		dst.date = this.date;
		dst.reward_max = this.reward_max;
		for (int i = 0; i < 7; i++)
		{
			dst.reward_id[i] = this.reward_id[i];
			dst.reward_count[i] = this.reward_count[i];
		}
		dst.clear_count = this.clear_count;
		dst.missions_complete = this.missions_complete;
	}
}
