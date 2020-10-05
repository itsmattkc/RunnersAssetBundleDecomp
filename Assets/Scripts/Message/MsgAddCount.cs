using System;

namespace Message
{
	public class MsgAddCount
	{
		public readonly int m_addCount;

		public MsgAddCount(int addCount)
		{
			this.m_addCount = addCount;
		}
	}
}
