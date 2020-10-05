using System;

namespace Message
{
	public class MsgContinueResult : MessageBase
	{
		public readonly bool m_result;

		public MsgContinueResult(bool result) : base(12352)
		{
			this.m_result = result;
		}
	}
}
