using System;

namespace Message
{
	public class MsgChaoState : MessageBase
	{
		public enum State
		{
			COME_IN,
			STOP,
			STOP_END,
			LAST_CHANCE,
			LAST_CHANCE_END
		}

		private MsgChaoState.State m_state;

		public MsgChaoState.State state
		{
			get
			{
				return this.m_state;
			}
		}

		public MsgChaoState(MsgChaoState.State state) : base(21760)
		{
			this.m_state = state;
		}
	}
}
