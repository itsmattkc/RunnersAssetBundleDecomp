using System;

public class ObjEnmSpinaData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_spina",
		"enm_spina_m",
		"enm_spina_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmSpinaData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}
}
