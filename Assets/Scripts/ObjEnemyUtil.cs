using GameScore;
using System;

public class ObjEnemyUtil
{
	public enum EnemyEffectSize : uint
	{
		S,
		M,
		L,
		NUM
	}

	public enum EnemyType : uint
	{
		NORMAL,
		METAL,
		RARE,
		NUM
	}

	private static readonly string[] EFFECT_FILES = new string[]
	{
		"ef_en_dead_s01",
		"ef_en_dead_m01",
		"ef_en_dead_l01"
	};

	private static readonly string[] SE_NAMETBL = new string[]
	{
		"enm_dead",
		"enm_metal_dead",
		"enm_gold_dead"
	};

	private static readonly int[] SOCRE_TABLE = new int[]
	{
		Data.EnemyNormal,
		Data.EnemyMetal,
		Data.EnemyRare
	};

	public static int[] GetDefaultScoreTable()
	{
		return ObjEnemyUtil.SOCRE_TABLE;
	}

	public static string GetEffectName(ObjEnemyUtil.EnemyEffectSize size)
	{
		if ((ulong)size < (ulong)((long)ObjEnemyUtil.EFFECT_FILES.Length))
		{
			return ObjEnemyUtil.EFFECT_FILES[(int)((UIntPtr)size)];
		}
		return string.Empty;
	}

	public static string GetSEName(ObjEnemyUtil.EnemyType type)
	{
		if ((ulong)type < (ulong)((long)ObjEnemyUtil.SE_NAMETBL.Length))
		{
			return ObjEnemyUtil.SE_NAMETBL[(int)((UIntPtr)type)];
		}
		return string.Empty;
	}

	public static string GetModelName(ObjEnemyUtil.EnemyType type, string[] model_files)
	{
		if (model_files != null && (ulong)type < (ulong)((long)model_files.Length))
		{
			return model_files[(int)((UIntPtr)type)];
		}
		return string.Empty;
	}

	public static int GetScore(ObjEnemyUtil.EnemyType type, int[] score_table)
	{
		if ((ulong)type < (ulong)((long)score_table.Length))
		{
			return score_table[(int)((UIntPtr)type)];
		}
		return 0;
	}
}
