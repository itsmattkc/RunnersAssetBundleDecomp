using System;

public class ServerDailyBattleDataPair
{
	public bool isDummyData;

	public DateTime starTime;

	public DateTime endTime;

	public ServerDailyBattleData myBattleData;

	public ServerDailyBattleData rivalBattleData;

	public string starDateString
	{
		get
		{
			return GeneralUtil.GetDateString(this.starTime, 0);
		}
	}

	public string endDateString
	{
		get
		{
			return GeneralUtil.GetDateString(this.endTime, 0);
		}
	}

	public bool isToday
	{
		get
		{
			bool result = false;
			if (this.starTime.Ticks != this.endTime.Ticks && !this.isDummyData)
			{
				DateTime currentTime = NetBase.GetCurrentTime();
				if (this.endTime.Ticks >= currentTime.Ticks)
				{
					result = true;
				}
			}
			return result;
		}
	}

	public int goOnWin
	{
		get
		{
			int result = 0;
			if (this.myBattleData != null && !string.IsNullOrEmpty(this.myBattleData.userId))
			{
				result = this.myBattleData.goOnWin;
			}
			return result;
		}
	}

	public int winFlag
	{
		get
		{
			int result = 0;
			if (this.myBattleData != null && !string.IsNullOrEmpty(this.myBattleData.userId))
			{
				if (this.rivalBattleData != null && !string.IsNullOrEmpty(this.rivalBattleData.userId))
				{
					if (this.myBattleData.maxScore > this.rivalBattleData.maxScore)
					{
						result = 3;
					}
					else if (this.myBattleData.maxScore == this.rivalBattleData.maxScore)
					{
						result = 2;
					}
					else
					{
						result = 1;
					}
				}
				else
				{
					result = 4;
				}
			}
			return result;
		}
	}

	public ServerDailyBattleDataPair()
	{
		this.isDummyData = false;
		this.starTime = NetBase.GetCurrentTime();
		this.endTime = NetBase.GetCurrentTime();
		this.myBattleData = new ServerDailyBattleData();
		this.rivalBattleData = new ServerDailyBattleData();
	}

	public ServerDailyBattleDataPair(ServerDailyBattleDataPair data)
	{
		this.isDummyData = data.isDummyData;
		this.starTime = data.starTime;
		this.endTime = data.endTime;
		this.myBattleData = new ServerDailyBattleData();
		this.rivalBattleData = new ServerDailyBattleData();
		data.myBattleData.CopyTo(this.myBattleData);
		data.rivalBattleData.CopyTo(this.rivalBattleData);
	}

	public ServerDailyBattleDataPair(DateTime start, DateTime end)
	{
		this.isDummyData = true;
		this.starTime = start;
		this.endTime = end;
		this.myBattleData = new ServerDailyBattleData();
		this.rivalBattleData = new ServerDailyBattleData();
	}

	public void Dump()
	{
		this.myBattleData.Dump();
		this.rivalBattleData.Dump();
	}

	public void CopyTo(ServerDailyBattleDataPair dest)
	{
		dest.isDummyData = this.isDummyData;
		dest.starTime = this.starTime;
		dest.endTime = this.endTime;
		this.myBattleData.CopyTo(dest.myBattleData);
		this.rivalBattleData.CopyTo(dest.rivalBattleData);
	}
}
