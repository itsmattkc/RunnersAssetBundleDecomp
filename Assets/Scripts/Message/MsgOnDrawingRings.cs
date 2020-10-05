using System;
using UnityEngine;

namespace Message
{
	public class MsgOnDrawingRings : MessageBase
	{
		public ChaoAbility m_chaoAbility;

		public GameObject m_target;

		public MsgOnDrawingRings() : base(24583)
		{
			this.m_chaoAbility = ChaoAbility.UNKNOWN;
		}

		public MsgOnDrawingRings(ChaoAbility chaoAbility) : base(24583)
		{
			this.m_chaoAbility = chaoAbility;
		}

		public MsgOnDrawingRings(GameObject target) : base(24583)
		{
			this.m_target = target;
			this.m_chaoAbility = ChaoAbility.UNKNOWN;
		}
	}
}
