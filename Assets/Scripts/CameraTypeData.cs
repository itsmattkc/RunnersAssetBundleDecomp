using System;

public class CameraTypeData
{
	private static readonly int[] GIMMICK_CMAERA_TBL = new int[]
	{
		0,
		1,
		1,
		1,
		1,
		1
	};

	public static bool IsGimmickCamera(CameraType type)
	{
		return type < CameraType.NUM && CameraTypeData.GIMMICK_CMAERA_TBL[(int)type] == 1;
	}
}
