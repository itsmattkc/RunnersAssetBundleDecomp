using System;

namespace Message
{
	public class MsgServerConnctFailed : MessageBase
	{
		public ServerInterface.StatusCode m_status;

		public MsgServerConnctFailed(ServerInterface.StatusCode status) : base(61517)
		{
			this.m_status = status;
		}
	}
}
