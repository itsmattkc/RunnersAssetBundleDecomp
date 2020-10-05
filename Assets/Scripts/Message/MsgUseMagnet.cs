using System;
using UnityEngine;

namespace Message
{
	public class MsgUseMagnet : MessageBase
	{
		public readonly GameObject m_obj;

		public readonly GameObject m_target;

		public readonly float m_time;

		public MsgUseMagnet(GameObject obj, float time) : base(12360)
		{
			this.m_obj = obj;
			this.m_target = null;
			this.m_time = time;
		}

		public MsgUseMagnet(GameObject obj, GameObject target, float time) : base(12360)
		{
			this.m_obj = obj;
			this.m_target = target;
			this.m_time = time;
		}
	}
}
