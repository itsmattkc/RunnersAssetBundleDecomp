using System;

namespace Message
{
	public class MsgTerminateItem : MessageBase
	{
		public readonly ItemType m_itemType;

		public MsgTerminateItem(ItemType itemType) : base(12290)
		{
			this.m_itemType = itemType;
		}
	}
}
