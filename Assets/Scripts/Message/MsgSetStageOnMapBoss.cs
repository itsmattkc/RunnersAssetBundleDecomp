using System;
using UnityEngine;

namespace Message
{
	public class MsgSetStageOnMapBoss : MessageBase
	{
		public Vector3 m_position;

		public Quaternion m_rotation;

		public string m_stageName;

		public BossType m_bossType;

		public MsgSetStageOnMapBoss(Vector3 pos, Quaternion rot, string stagename, BossType bossType) : base(12312)
		{
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_stageName = stagename;
			this.m_bossType = bossType;
		}
	}
}
