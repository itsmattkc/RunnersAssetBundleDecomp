using System;

namespace Message
{
	public class MsgAbilityEffectStart : MessageBase
	{
		public readonly ChaoAbility m_ability;

		public readonly string m_effectName;

		public readonly bool m_loop;

		public readonly bool m_center;

		public MsgAbilityEffectStart(ChaoAbility ability, string effectName, bool loop, bool center) : base(16387)
		{
			this.m_ability = ability;
			this.m_effectName = effectName;
			this.m_loop = loop;
			this.m_center = center;
		}
	}
}
