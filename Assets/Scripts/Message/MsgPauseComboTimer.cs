using System;

namespace Message
{
	public class MsgPauseComboTimer : MessageBase
	{
		public enum State
		{
			PAUSE,
			PAUSE_TIMER,
			PLAY,
			PLAY_SET,
			PLAY_RESET
		}

		public MsgPauseComboTimer.State m_value;

		public float m_time;

		public MsgPauseComboTimer(MsgPauseComboTimer.State value) : base(12356)
		{
			this.m_value = value;
			this.m_time = -1f;
		}

		public MsgPauseComboTimer(MsgPauseComboTimer.State value, float time) : base(12356)
		{
			this.m_value = value;
			this.m_time = time;
		}
	}
}
