using System;

namespace Message
{
	public class MsgHudCockpitSetup : MessageBase
	{
		public BossType m_bossType;

		public bool m_spCrystal;

		public bool m_animal;

		public bool m_backMainMenuCheck;

		public bool m_firstTutorial;

		public MsgHudCockpitSetup(BossType bossType, bool spCrystal, bool animal, bool backMainMenuCheck, bool firstTutorial) : base(49155)
		{
			this.m_bossType = bossType;
			this.m_spCrystal = spCrystal;
			this.m_animal = animal;
			this.m_backMainMenuCheck = backMainMenuCheck;
			this.m_firstTutorial = firstTutorial;
		}
	}
}
