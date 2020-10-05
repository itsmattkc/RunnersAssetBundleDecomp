using System;
using System.Runtime.InteropServices;

public struct QuickModeTimerNativeParam
{
	public int goldCount;

	public int silverCount;

	public int bronzeCount;

	public int continuCount;

	public int mainCharaExtendTime;

	public int subCharaExtendTime;

	public int totalTime;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public int[] playTime;

	public void Init(int gold, int silver, int bronze, int continueCount, int main, int sub, int total, long playTime)
	{
		this.goldCount = gold;
		this.silverCount = silver;
		this.bronzeCount = bronze;
		this.continuCount = continueCount;
		this.mainCharaExtendTime = main;
		this.subCharaExtendTime = sub;
		this.totalTime = total;
		BindingLinkUtility.LongToIntArray(out this.playTime, playTime);
	}
}
