using System;

namespace Message
{
	public class MsgPopCamera : MessageBase
	{
		public CameraType m_type;

		public float m_interpolateTime;

		public MsgPopCamera(CameraType type, float interpolateTime) : base(32769)
		{
			this.m_type = type;
			this.m_interpolateTime = interpolateTime;
		}
	}
}
