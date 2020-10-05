using System;

namespace Message
{
	public class MsgUpdateDailyBattleStatusSucceed : MessageBase
	{
		public ServerDailyBattleStatus battleStatus;

		public DateTime endTime;

		public bool rewardFlag;

		public ServerDailyBattleDataPair rewardBattleDataPair;

		public MsgUpdateDailyBattleStatusSucceed() : base(61472)
		{
			this.battleStatus = new ServerDailyBattleStatus();
			this.endTime = NetBase.GetCurrentTime();
			this.rewardFlag = false;
			this.rewardBattleDataPair = null;
		}
	}
}
