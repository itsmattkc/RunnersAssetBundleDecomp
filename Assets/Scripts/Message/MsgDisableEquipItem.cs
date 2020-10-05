using System;

namespace Message
{
	public class MsgDisableEquipItem : MessageBase
	{
		public bool m_disable;

		public MsgDisableEquipItem(bool disable) : base(12320)
		{
			this.m_disable = disable;
		}
	}
}
