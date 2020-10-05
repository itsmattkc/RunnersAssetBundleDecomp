using System;

namespace Message
{
	public class MsgChangeCharaEnable : MessageBase
	{
		public bool value;

		public MsgChangeCharaEnable(bool value_) : base(12314)
		{
			this.value = value_;
		}
	}
}
