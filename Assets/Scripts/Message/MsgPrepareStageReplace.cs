using System;

namespace Message
{
	public class MsgPrepareStageReplace : MessageBase
	{
		public PlayerSpeed m_speedLevel;

		public string m_stageName;

		public MsgPrepareStageReplace(PlayerSpeed speedLevel, string stagename) : base(12310)
		{
			this.m_speedLevel = speedLevel;
			this.m_stageName = stagename;
		}
	}
}
