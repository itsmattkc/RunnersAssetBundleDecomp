using System;

namespace Message
{
	public class MsgAddScore
	{
		public readonly int m_score;

		public MsgAddScore(int score)
		{
			this.m_score = score;
		}
	}
}
