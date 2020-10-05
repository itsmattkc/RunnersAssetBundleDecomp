using System;

public class ObjEnmBeetonData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_beeton",
		"enm_beeton_m",
		"enm_beeton_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmBeetonData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}
}
