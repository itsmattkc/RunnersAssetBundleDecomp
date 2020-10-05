using System;

public class ServerDailyBattleStatus
{
	public int numWin;

	public int numLose;

	public int numDraw;

	public int numLoseByDefault;

	public int goOnWin;

	public int goOnLose;

	public ServerDailyBattleStatus()
	{
		this.numWin = 0;
		this.numLose = 0;
		this.numDraw = 0;
		this.numLoseByDefault = 0;
		this.goOnWin = 0;
		this.goOnLose = 0;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Format("ServerDailyBattleStatus  numWin:{0} numLose:{1} numDraw:{2} numLoseByDefault:{3} goOnWin:{4} goOnLose:{5}", new object[]
		{
			this.numWin,
			this.numLose,
			this.numDraw,
			this.numLoseByDefault,
			this.goOnWin,
			this.goOnLose
		}));
	}

	public void CopyTo(ServerDailyBattleStatus dest)
	{
		dest.numWin = this.numWin;
		dest.numLose = this.numLose;
		dest.numDraw = this.numDraw;
		dest.numLoseByDefault = this.numLoseByDefault;
		dest.goOnWin = this.goOnWin;
		dest.goOnLose = this.goOnLose;
	}
}
