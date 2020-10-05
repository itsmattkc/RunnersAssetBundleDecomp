using System;

public class ObjEnmNarlData
{
	private static readonly string[] MODEL_FILES = new string[]
	{
		"enm_narl",
		"enm_narl_m",
		"enm_narl_g"
	};

	public static string[] GetModelFiles()
	{
		return ObjEnmNarlData.MODEL_FILES;
	}

	public static ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.M;
	}
}
