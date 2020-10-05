using System;

namespace Message
{
	public class MsgResetScore
	{
		public int m_score;

		public int m_animal;

		public int m_ring;

		public int m_red_ring;

		public int m_final_score;

		public MsgResetScore()
		{
			this.m_score = 0;
			this.m_animal = 0;
			this.m_ring = 0;
			this.m_red_ring = 0;
			this.m_final_score = 0;
		}

		public MsgResetScore(int score, int animal, int ring)
		{
			this.m_score = score;
			this.m_animal = animal;
			this.m_ring = ring;
			this.m_red_ring = 0;
			this.m_final_score = 0;
		}
	}
}
