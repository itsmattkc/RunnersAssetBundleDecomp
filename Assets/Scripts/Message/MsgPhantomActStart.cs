using System;

namespace Message
{
	public class MsgPhantomActStart : MessageBase
	{
		public readonly PhantomType m_type;

		public MsgPhantomActStart(PhantomType type) : base(12350)
		{
			this.m_type = type;
		}
	}
}
