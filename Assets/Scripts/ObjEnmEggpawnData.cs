using System;

public class ObjEnmEggpawnData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_eggpawn",
		"enm_eggpawn_m",
		"enm_eggpawn_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmEggpawnData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.M;
	}
}
