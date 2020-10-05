using System;

namespace Message
{
	public class MsgChangeCurrentBlock : MessageBase
	{
		public int m_blockNo;

		public int m_activateID;

		public int m_layerNo;

		public MsgChangeCurrentBlock(int block, int layer, int activateID) : base(12302)
		{
			this.m_blockNo = block;
			this.m_activateID = activateID;
			this.m_layerNo = layer;
		}
	}
}
