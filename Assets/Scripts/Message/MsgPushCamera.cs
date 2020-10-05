using System;
using UnityEngine;

namespace Message
{
	public class MsgPushCamera : MessageBase
	{
		public CameraType m_type;

		public UnityEngine.Object m_parameter;

		public float m_interpolateTime;

		public MsgPushCamera(CameraType type, float interpolateTime, UnityEngine.Object parameter = null) : base(32768)
		{
			this.m_type = type;
			this.m_parameter = parameter;
			this.m_interpolateTime = interpolateTime;
		}
	}
}
