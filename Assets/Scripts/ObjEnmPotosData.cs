using System;

public class ObjEnmPotosData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_potos",
		"enm_potos_m",
		"enm_potos_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmPotosData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}
}
