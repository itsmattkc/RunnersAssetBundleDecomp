using System;

namespace Message
{
	public class MsgObjectDead : MessageBase
	{
		public ChaoAbility m_chaoAbility;

		public MsgObjectDead() : base(24586)
		{
			this.m_chaoAbility = ChaoAbility.UNKNOWN;
		}

		public MsgObjectDead(ChaoAbility chaoAbility) : base(24586)
		{
			this.m_chaoAbility = chaoAbility;
		}
	}
}
