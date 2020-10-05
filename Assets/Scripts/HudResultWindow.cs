using System;
using UnityEngine;

public class HudResultWindow : MonoBehaviour
{
	private enum Type
	{
		CHAO_1,
		CHAO_2,
		CHAO_COUNT,
		CAMPAIGN,
		CHARA_1,
		CHARA_2,
		MILEAGE,
		NUM
	}

	[Serializable]
	private class ScoreInfo
	{
		[SerializeField]
		public UILabel m_score;

		[SerializeField]
		public UILabel m_ring;

		[SerializeField]
		public UILabel m_animal;

		[SerializeField]
		public UILabel m_distance;
	}

	[Serializable]
	private class TexInfo
	{
		[SerializeField]
		public UITexture m_scoreTex;

		[SerializeField]
		public UITexture m_ringTex;

		[SerializeField]
		public UITexture m_animalTex;

		[SerializeField]
		public UITexture m_distanceTex;
	}

	[Serializable]
	private class SprInfo
	{
		[SerializeField]
		public UISprite m_scoreTex;

		[SerializeField]
		public UISprite m_ringTex;

		[SerializeField]
		public UISprite m_animalTex;

		[SerializeField]
		public UISprite m_distanceTex;
	}

	[SerializeField]
	private HudResultWindow.ScoreInfo[] m_scoreInfos = new HudResultWindow.ScoreInfo[7];

	[SerializeField]
	private HudResultWindow.TexInfo m_chao1TexInfos = new HudResultWindow.TexInfo();

	[SerializeField]
	private HudResultWindow.TexInfo m_chao2TexInfos = new HudResultWindow.TexInfo();

	[SerializeField]
	private HudResultWindow.SprInfo m_chara1TexInfos = new HudResultWindow.SprInfo();

	[SerializeField]
	private HudResultWindow.SprInfo m_chara2TexInfos = new HudResultWindow.SprInfo();

	[SerializeField]
	private UILabel m_totalScore;

	private GameObject m_result;

	public void Setup(GameObject result, bool bossResult)
	{
		this.m_result = result;
		StageScoreManager instance = StageScoreManager.Instance;
		if (instance == null)
		{
			return;
		}
		SaveDataManager instance2 = SaveDataManager.Instance;
		if (instance2 == null)
		{
			return;
		}
		StageAbilityManager instance3 = StageAbilityManager.Instance;
		if (instance3 == null)
		{
			return;
		}
		this.SetScore(instance.BonusCountMainChaoData, instance3.MainChaoBonusValueRate, this.m_scoreInfos[0], bossResult);
		this.SetChaoTexture(this.m_chao1TexInfos, instance2.PlayerData.MainChaoID, instance3.MainChaoBonusValueRate, bossResult);
		this.SetScore(instance.BonusCountSubChaoData, instance3.SubChaoBonusValueRate, this.m_scoreInfos[1], bossResult);
		this.SetChaoTexture(this.m_chao2TexInfos, instance2.PlayerData.SubChaoID, instance3.SubChaoBonusValueRate, bossResult);
		this.SetScore(instance.BonusCountChaoCountData, instance3.CountChaoBonusValueRate, this.m_scoreInfos[2], bossResult);
		this.SetText(this.m_scoreInfos[2].m_ring, -1L);
		this.SetText(this.m_scoreInfos[2].m_animal, -1L);
		this.SetText(this.m_scoreInfos[2].m_distance, -1L);
		this.SetScore(instance.BonusCountCampaignData, GameResultUtility.GetCampaignBonusRate(instance3), this.m_scoreInfos[3], bossResult);
		this.SetText(this.m_scoreInfos[3].m_score, -1L);
		this.SetText(this.m_scoreInfos[3].m_animal, -1L);
		this.SetText(this.m_scoreInfos[3].m_distance, -1L);
		this.SetScore(instance.BonusCountMainCharaData, instance3.MainCharaBonusValueRate, this.m_scoreInfos[4], bossResult);
		this.SetCharaTexture(this.m_chara1TexInfos, instance2.PlayerData.MainChara, instance3.MainCharaBonusValueRate, bossResult);
		this.SetScore(instance.BonusCountSubCharaData, instance3.SubCharaBonusValueRate, this.m_scoreInfos[5], bossResult);
		this.SetCharaTexture(this.m_chara2TexInfos, instance2.PlayerData.SubChara, instance3.SubCharaBonusValueRate, bossResult);
		this.SetScore(instance.MileageBonusScoreData, instance3.MileageBonusScoreRate, this.m_scoreInfos[6], bossResult);
		if (bossResult)
		{
			this.SetText(this.m_scoreInfos[6].m_ring, -1L);
			this.SetText(this.m_totalScore, -1L);
		}
		else
		{
			this.SetText(this.m_totalScore, (instance3.MileageBonusScoreRate.final_score <= 0f) ? (-1L) : instance.MileageBonusScoreData.final_score);
		}
	}

	private void OnClickNoButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		if (this.m_result != null)
		{
			this.m_result.SendMessage("OnClickDetailsEndButton");
		}
	}

	private void SetScore(StageScoreManager.ResultData resultData, StageAbilityManager.BonusRate bonusRate, HudResultWindow.ScoreInfo scoreInfo, bool bossResult)
	{
		if (resultData == null)
		{
			return;
		}
		if (scoreInfo == null)
		{
			return;
		}
		this.SetText(scoreInfo.m_ring, (bonusRate.ring <= 0f) ? (-1L) : resultData.ring);
		if (bossResult)
		{
			this.SetText(scoreInfo.m_score, -1L);
			this.SetText(scoreInfo.m_animal, -1L);
			this.SetText(scoreInfo.m_distance, -1L);
		}
		else
		{
			this.SetText(scoreInfo.m_score, (bonusRate.score <= 0f) ? (-1L) : resultData.score);
			this.SetText(scoreInfo.m_animal, (bonusRate.animal <= 0f) ? (-1L) : resultData.animal);
			this.SetText(scoreInfo.m_distance, (bonusRate.distance <= 0f) ? (-1L) : resultData.distance);
		}
	}

	private void SetText(UILabel label, long score)
	{
		if (label == null)
		{
			return;
		}
		if (score >= 0L)
		{
			label.text = GameResultUtility.GetScoreFormat(score);
			label.gameObject.SetActive(true);
		}
		else
		{
			label.gameObject.SetActive(false);
		}
	}

	private void SetChaoTexture(HudResultWindow.TexInfo texInfo, int chaoId, StageAbilityManager.BonusRate bonusRate, bool bossResult)
	{
		HudUtility.SetChaoTexture(texInfo.m_ringTex, (bonusRate.ring <= 0f) ? (-1) : chaoId, true);
		if (bossResult)
		{
			HudUtility.SetChaoTexture(texInfo.m_scoreTex, -1, true);
			HudUtility.SetChaoTexture(texInfo.m_animalTex, -1, true);
			HudUtility.SetChaoTexture(texInfo.m_distanceTex, -1, true);
		}
		else
		{
			HudUtility.SetChaoTexture(texInfo.m_scoreTex, (bonusRate.score <= 0f) ? (-1) : chaoId, true);
			HudUtility.SetChaoTexture(texInfo.m_animalTex, (bonusRate.animal <= 0f) ? (-1) : chaoId, true);
			HudUtility.SetChaoTexture(texInfo.m_distanceTex, (bonusRate.distance <= 0f) ? (-1) : chaoId, true);
		}
	}

	private void SetCharaTexture(HudResultWindow.SprInfo texInfo, CharaType charaType, StageAbilityManager.BonusRate bonusRate, bool bossResult)
	{
		GameResultUtility.SetCharaTexture(texInfo.m_ringTex, (bonusRate.ring <= 0f) ? CharaType.UNKNOWN : charaType);
		if (bossResult)
		{
			GameResultUtility.SetCharaTexture(texInfo.m_scoreTex, CharaType.UNKNOWN);
			GameResultUtility.SetCharaTexture(texInfo.m_animalTex, CharaType.UNKNOWN);
			GameResultUtility.SetCharaTexture(texInfo.m_distanceTex, CharaType.UNKNOWN);
		}
		else
		{
			GameResultUtility.SetCharaTexture(texInfo.m_scoreTex, (bonusRate.score <= 0f) ? CharaType.UNKNOWN : charaType);
			GameResultUtility.SetCharaTexture(texInfo.m_animalTex, (bonusRate.animal <= 0f) ? CharaType.UNKNOWN : charaType);
			GameResultUtility.SetCharaTexture(texInfo.m_distanceTex, (bonusRate.distance <= 0f) ? CharaType.UNKNOWN : charaType);
		}
	}
}
