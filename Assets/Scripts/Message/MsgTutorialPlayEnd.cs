using System;
using Tutorial;
using UnityEngine;

namespace Message
{
	public class MsgTutorialPlayEnd : MessageBase
	{
		public readonly bool m_complete;

		public readonly bool m_retry;

		public readonly EventID m_nextEventID;

		public readonly Vector3 m_pos;

		public readonly Quaternion m_rot;

		public MsgTutorialPlayEnd(bool complete, bool retry, EventID nextEventID, Vector3 pos, Quaternion rot) : base(12335)
		{
			this.m_complete = complete;
			this.m_retry = retry;
			this.m_nextEventID = nextEventID;
			this.m_pos = pos;
			this.m_rot = rot;
		}
	}
}
