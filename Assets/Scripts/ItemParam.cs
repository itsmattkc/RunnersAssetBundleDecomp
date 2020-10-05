using SaveData;
using System;

public class ItemParam
{
	public string m_name;

	public string m_objName;

	public SystemData.ItemTutorialFlagStatus m_flagStatus;

	public HudTutorial.Id m_tutorialID;

	public ItemParam(string name, string objName, SystemData.ItemTutorialFlagStatus flagStatus, HudTutorial.Id tutorialID)
	{
		this.m_name = name;
		this.m_objName = objName;
		this.m_flagStatus = flagStatus;
		this.m_tutorialID = tutorialID;
	}
}
