using System;

namespace Message
{
	public class MsgUsedItem : MessageBase
	{
		public readonly ItemType m_itemType;

		public MsgUsedItem(ItemType itemType) : base(12297)
		{
			this.m_itemType = itemType;
		}
	}
}
