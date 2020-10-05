using System;

namespace Message
{
	public class MsgGetDailyBattleStatusSucceed : MessageBase
	{
		public ServerDailyBattleStatus battleStatus;

		public DateTime endTime;

		public MsgGetDailyBattleStatusSucceed() : base(61471)
		{
			this.battleStatus = new ServerDailyBattleStatus();
			this.endTime = NetBase.GetCurrentTime();
		}
	}
}
