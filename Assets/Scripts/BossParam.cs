using SaveData;
using System;

public class BossParam
{
	public string m_name;

	public SystemData.FlagStatus m_flagStatus;

	public HudTutorial.Id m_tutorialID;

	public BossCharaType m_bossCharaType;

	public BossCategory m_bossCategory;

	public int m_layerNumber;

	public int m_indexNumber;

	public BossParam(string in_name, SystemData.FlagStatus flagStatus, HudTutorial.Id tutorialID, BossCharaType bossCharaType, BossCategory category, int number1, int number2)
	{
		this.m_name = in_name;
		this.m_flagStatus = flagStatus;
		this.m_tutorialID = tutorialID;
		this.m_bossCharaType = bossCharaType;
		this.m_bossCategory = category;
		this.m_layerNumber = number1;
		this.m_indexNumber = number2;
	}
}
