using System;

namespace Message
{
	public class MsgStockItem : MessageBase
	{
		public ItemType m_itemType;

		public MsgStockItem(ItemType itemType) : base(12295)
		{
			this.m_itemType = itemType;
		}
	}
}
