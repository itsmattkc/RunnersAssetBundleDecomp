using System;

namespace Message
{
	public class MsgResetDailyBattleMatchingSucceed : MessageBase
	{
		public ServerPlayerState playerState;

		public ServerDailyBattleDataPair battleDataPair;

		public DateTime endTime;

		public MsgResetDailyBattleMatchingSucceed() : base(61477)
		{
			this.playerState = new ServerPlayerState();
			this.battleDataPair = new ServerDailyBattleDataPair();
			this.endTime = NetBase.GetCurrentTime();
		}
	}
}
