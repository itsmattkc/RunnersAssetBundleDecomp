using System;

public class ObjEnmMotoraData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_motora",
		"enm_motora_m",
		"enm_motora_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmMotoraData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}
}
