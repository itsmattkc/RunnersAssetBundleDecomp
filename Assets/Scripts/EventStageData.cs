using System;

public class EventStageData
{
	public int bg_id;

	public string stage_key;

	public string stageCueSheetName;

	public string bossStagCueSheetName;

	public string quickStageCueSheetName;

	public string stageBGM;

	public string bossStagBGM;

	public string quickStageBGM;

	public bool IsEndlessModeBGM()
	{
		return !string.IsNullOrEmpty(this.stageCueSheetName) && !string.IsNullOrEmpty(this.bossStagCueSheetName) && !string.IsNullOrEmpty(this.stageBGM) && !string.IsNullOrEmpty(this.bossStagBGM);
	}

	public bool IsQuickModeBGM()
	{
		return !string.IsNullOrEmpty(this.quickStageCueSheetName) && !string.IsNullOrEmpty(this.quickStageBGM);
	}
}
