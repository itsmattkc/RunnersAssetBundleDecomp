using System;

namespace Message
{
	public class MsgAddItemToManager : MessageBase
	{
		public readonly ItemType m_itemType;

		public MsgAddItemToManager(ItemType itemType) : base(12291)
		{
			this.m_itemType = itemType;
		}
	}
}
