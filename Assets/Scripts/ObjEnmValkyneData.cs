using System;

public class ObjEnmValkyneData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_valkyne",
		"enm_valkyne_m",
		"enm_valkyne_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmValkyneData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}
}
