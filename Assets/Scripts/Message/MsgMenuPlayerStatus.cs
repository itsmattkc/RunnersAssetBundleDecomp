using System;

namespace Message
{
	public class MsgMenuPlayerStatus : MessageBase
	{
		public enum StatusType
		{
			USE_SUB_CHAR
		}

		private MsgMenuPlayerStatus.StatusType m_status;

		public MsgMenuPlayerStatus.StatusType Status
		{
			get
			{
				return this.m_status;
			}
		}

		public MsgMenuPlayerStatus(MsgMenuPlayerStatus.StatusType status) : base(57344)
		{
			this.m_status = status;
		}
	}
}
