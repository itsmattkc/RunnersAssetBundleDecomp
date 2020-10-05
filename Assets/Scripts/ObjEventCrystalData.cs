using GameScore;
using System;

public class ObjEventCrystalData
{
	private static readonly CtystalParam[] PARAM_TBL = new CtystalParam[]
	{
		new CtystalParam(false, "ObjEventCrystal", string.Empty, "ef_ob_get_crystal_rd_s01", "obj_crystal_red", Data.EventCrystal, false),
		new CtystalParam(true, "ObjEventCrystal10", string.Empty, "ef_ob_get_crystal_rd_l01", "obj_big_crystal", Data.EventCrystal10, false)
	};

	public static CtystalParam GetCtystalParam(EventCtystalType type)
	{
		if (type < (EventCtystalType)ObjEventCrystalData.PARAM_TBL.Length)
		{
			return ObjEventCrystalData.PARAM_TBL[(int)type];
		}
		return ObjEventCrystalData.PARAM_TBL[0];
	}
}
