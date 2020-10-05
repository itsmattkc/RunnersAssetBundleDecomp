using System;
using UnityEngine;

namespace Message
{
	public class MsgAttackGuard : MessageBase
	{
		public readonly GameObject m_sender;

		public MsgAttackGuard(GameObject sender) : base(16386)
		{
			this.m_sender = sender;
		}
	}
}
