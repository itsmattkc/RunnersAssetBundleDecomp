using System;

public class ObjEnmDendenData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_denden",
		"enm_denden_m",
		"enm_denden_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmDendenData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.M;
	}
}
