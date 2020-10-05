using System;

public class CharaSEUtil
{
	public class CharaSEData
	{
		public string m_jump;

		public string m_jump2;

		public string m_spin;

		public string m_fly;

		public string m_attack;

		public CharaSEData(string jump, string jump2, string spin, string fly, string attack)
		{
			this.m_jump = jump;
			this.m_jump2 = jump2;
			this.m_spin = spin;
			this.m_fly = fly;
			this.m_attack = attack;
		}
	}

	private static readonly CharaSEUtil.CharaSEData[] CHARA_SE_TBL = new CharaSEUtil.CharaSEData[]
	{
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump_2", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump_2", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_large", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_large", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_large", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_large", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump_2", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_cla", "act_jump_cla", "act_spindash_cla", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump_2", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump_large", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flight_silver", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump", "act_spindash", "act_flytype_fly", "act_powertype_attack"),
		new CharaSEUtil.CharaSEData("act_jump", "act_2ndjump_2", "act_spindash", "act_flytype_fly", "act_powertype_attack")
	};

	private static bool EnableChara(CharaType charaType)
	{
		return CharaType.SONIC <= charaType && charaType < CharaType.NUM;
	}

	public static void PlayJumpSE(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			SoundManager.SePlay(CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_jump, "SE");
		}
		else
		{
			SoundManager.SePlay("act_jump", "SE");
		}
	}

	public static void Play2ndJumpSE(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			SoundManager.SePlay(CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_jump2, "SE");
		}
		else
		{
			SoundManager.SePlay("act_2ndjump", "SE");
		}
	}

	public static string GetSpinDashSEName(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			return CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_spin;
		}
		return "act_spindash";
	}

	public static void PlaySpinDashSE(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			SoundManager.SePlay(CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_spin, "SE");
		}
		else
		{
			SoundManager.SePlay("act_spindash", "SE");
		}
	}

	public static void PlayFlySE(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			SoundManager.SePlay(CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_fly, "SE");
		}
		else
		{
			SoundManager.SePlay("act_flytype_fly", "SE");
		}
	}

	public static void PlayPowerAttackSE(CharaType charaType)
	{
		if (CharaSEUtil.EnableChara(charaType))
		{
			SoundManager.SePlay(CharaSEUtil.CHARA_SE_TBL[(int)charaType].m_attack, "SE");
		}
		else
		{
			SoundManager.SePlay("act_powertype_attack", "SE");
		}
	}
}
