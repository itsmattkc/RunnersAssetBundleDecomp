using SaveData;
using System;

public class CharaParam
{
	public SystemData.CharaTutorialFlagStatus m_flagStatus;

	public HudTutorial.Id m_tutorialID;

	public CharaParam(SystemData.CharaTutorialFlagStatus flagStatus, HudTutorial.Id tutorialID)
	{
		this.m_flagStatus = flagStatus;
		this.m_tutorialID = tutorialID;
	}
}
