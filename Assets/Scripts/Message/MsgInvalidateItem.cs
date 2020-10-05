using System;

namespace Message
{
	public class MsgInvalidateItem : MessageBase
	{
		public readonly ItemType m_itemType;

		public MsgInvalidateItem(ItemType itemType) : base(12289)
		{
			this.m_itemType = itemType;
		}
	}
}
