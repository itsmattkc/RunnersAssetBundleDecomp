using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public struct ActStartNativeParam
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public int[] modifire;

	public bool tutorial;

	public int eventId;

	public void Init(List<int> itemList, bool tutorial, int eventId)
	{
		this.modifire = new int[6];
		for (int i = 0; i < 6; i++)
		{
			if (i < itemList.Count)
			{
				this.modifire[i] = itemList[i];
			}
			else
			{
				this.modifire[i] = -1;
			}
		}
		this.tutorial = tutorial;
		this.eventId = eventId;
	}
}
