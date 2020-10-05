using System;

namespace Player
{
	public class ChangePhantomParameter : StateEnteringParameter
	{
		public PhantomType m_changeType;

		public float m_time;

		public PhantomType ChangeType
		{
			get
			{
				return this.m_changeType;
			}
		}

		public float Timer
		{
			get
			{
				return this.m_time;
			}
		}

		public override void Reset()
		{
			this.m_changeType = PhantomType.NONE;
			this.m_time = 0f;
		}

		public void Set(PhantomType type, float time)
		{
			this.m_changeType = type;
			this.m_time = time;
		}

		public void Set(PhantomType type)
		{
			this.m_changeType = type;
		}
	}
}
