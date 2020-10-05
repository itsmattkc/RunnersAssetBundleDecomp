using System;

namespace Message
{
	public class MsgChangeCharaButton : MessageBase
	{
		public bool value;

		public bool pause;

		public MsgChangeCharaButton(bool value_, bool pause_) : base(12315)
		{
			this.value = value_;
			this.pause = pause_;
		}
	}
}
