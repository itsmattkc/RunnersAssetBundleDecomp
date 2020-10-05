using System;
using System.Collections.Generic;

namespace Message
{
	public class MsgGetDailyBattleDataHistorySucceed : MessageBase
	{
		public List<ServerDailyBattleDataPair> battleDataPairList;

		public MsgGetDailyBattleDataHistorySucceed() : base(61475)
		{
			this.battleDataPairList = new List<ServerDailyBattleDataPair>();
		}
	}
}
