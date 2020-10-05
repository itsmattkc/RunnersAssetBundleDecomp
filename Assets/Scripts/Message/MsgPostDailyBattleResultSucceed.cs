using System;

namespace Message
{
	public class MsgPostDailyBattleResultSucceed : MessageBase
	{
		public ServerDailyBattleStatus battleStatus;

		public ServerDailyBattleDataPair battleDataPair;

		public bool rewardFlag;

		public ServerDailyBattleDataPair rewardBattleDataPair;

		public MsgPostDailyBattleResultSucceed() : base(61473)
		{
			this.battleStatus = new ServerDailyBattleStatus();
			this.battleDataPair = new ServerDailyBattleDataPair();
			this.rewardFlag = false;
			this.rewardBattleDataPair = null;
		}
	}
}
