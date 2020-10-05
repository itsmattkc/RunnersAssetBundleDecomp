using System;

namespace Message
{
	public class MsgPhantomActEnd : MessageBase
	{
		public readonly PhantomType m_type;

		public MsgPhantomActEnd(PhantomType type) : base(12351)
		{
			this.m_type = type;
		}
	}
}
