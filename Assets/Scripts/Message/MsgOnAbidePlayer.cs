using System;
using UnityEngine;

namespace Message
{
	public class MsgOnAbidePlayer : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public float m_height;

		public GameObject m_abideObject;

		public bool m_succeed;

		public MsgOnAbidePlayer(Vector3 pos, Quaternion rot, float height, GameObject obj) : base(24580)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_height = height;
			this.m_abideObject = obj;
			this.m_succeed = false;
		}
	}
}
