using System;

namespace Message
{
	public class MsgAbilityEffectEnd : MessageBase
	{
		public readonly ChaoAbility m_ability;

		public readonly string m_effectName;

		public MsgAbilityEffectEnd(ChaoAbility ability, string effectName) : base(16388)
		{
			this.m_ability = ability;
			this.m_effectName = effectName;
		}
	}
}
