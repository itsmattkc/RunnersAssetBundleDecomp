using System;

namespace Message
{
	public class MsgDeactivateBlock : MessageBase
	{
		public float m_distance;

		public int m_blockNo;

		public int m_activateID;

		public MsgDeactivateBlock(int block, int activateID, float distance) : base(12300)
		{
			this.m_blockNo = block;
			this.m_activateID = activateID;
			this.m_distance = distance;
		}
	}
}
