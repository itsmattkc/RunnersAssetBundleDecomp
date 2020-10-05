using System;

namespace Player
{
	public class JumpParameter : StateEnteringParameter
	{
		public bool m_onAir;

		public override void Reset()
		{
			this.m_onAir = false;
		}

		public void Set(bool onair)
		{
			this.m_onAir = onair;
		}
	}
}
