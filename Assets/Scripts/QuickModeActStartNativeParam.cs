using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public struct QuickModeActStartNativeParam
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public int[] modifire;

	public int m_tutorial;

	public void Init(List<int> itemList, int tutorial)
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
		this.m_tutorial = tutorial;
	}
}
