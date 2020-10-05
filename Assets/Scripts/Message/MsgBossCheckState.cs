using System;

namespace Message
{
	public class MsgBossCheckState : MessageBase
	{
		public enum State
		{
			IDLE,
			ATTACK_OK
		}

		private MsgBossCheckState.State m_state;

		public MsgBossCheckState(MsgBossCheckState.State state) : base(12323)
		{
			this.m_state = state;
		}

		public bool IsAttackOK()
		{
			return this.m_state == MsgBossCheckState.State.ATTACK_OK;
		}
	}
}
