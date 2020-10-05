using System;
using UnityEngine;

namespace Message
{
	public class MsgStageReplace : MessageBase
	{
		public PlayerSpeed m_speedLevel;

		public Vector3 m_position;

		public Quaternion m_rotation;

		public string m_stageName;

		public MsgStageReplace(PlayerSpeed speedLevel, Vector3 pos, Quaternion rot, string stagename) : base(12309)
		{
			this.m_speedLevel = speedLevel;
			this.m_position = pos;
			this.m_rotation = rot;
			this.m_stageName = stagename;
		}
	}
}
