using System;

namespace Player
{
	public class RunLoopPathParameter : StateEnteringParameter
	{
		public PathComponent m_pathComponent;

		public override void Reset()
		{
			this.m_pathComponent = null;
		}

		public void Set(PathComponent component)
		{
			this.m_pathComponent = component;
		}
	}
}
