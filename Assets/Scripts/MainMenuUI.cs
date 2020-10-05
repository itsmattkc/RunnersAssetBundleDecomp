using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	private MainMenuDeckTab m_deckTab;

	private HudEpisodeButton m_episodeButton;

	private HudEpisodeBanner m_episodeBanner;

	private HudCampaignBanner m_quickCampainBaner;

	private HudCampaignBanner m_endlessCampainBaner;

	private HudQuickModeStagePicture m_quickModeStagePicture;

	private HudMainMenuRankingButton m_endlessModeRankingButton;

	private HudMainMenuRankingButton m_quickModeRankingButton;

	private HudDailyBattleButton m_dailyBattleButton;

	private HudQuestionButton m_quickModeQuestionButton;

	private HudQuestionButton m_endlessModeQuestionButton;

	public void UpdateView()
	{
		if (this.m_episodeBanner != null)
		{
			this.m_episodeBanner.UpdateView();
		}
		if (this.m_deckTab != null)
		{
			this.m_deckTab.UpdateView();
		}
		if (this.m_dailyBattleButton != null)
		{
			this.m_dailyBattleButton.UpdateView();
		}
		if (this.m_quickCampainBaner != null)
		{
			this.m_quickCampainBaner.UpdateView();
		}
		if (this.m_endlessCampainBaner != null)
		{
			this.m_endlessCampainBaner.UpdateView();
		}
	}

	private void Start()
	{
		if (this.m_deckTab == null)
		{
			this.m_deckTab = base.gameObject.AddComponent<MainMenuDeckTab>();
		}
		if (this.m_episodeButton == null)
		{
			this.m_episodeButton = new HudEpisodeButton();
			bool isBossStage = MileageMapUtility.IsBossStage();
			int mileageStageIndex = MileageMapDataManager.Instance.MileageStageIndex;
			CharacterAttribute[] characterAttribute = MileageMapUtility.GetCharacterAttribute(mileageStageIndex);
			CharacterAttribute charaAttribute = characterAttribute[0];
			this.m_episodeButton.Initialize(base.gameObject, isBossStage, charaAttribute);
		}
		if (this.m_episodeBanner == null)
		{
			this.m_episodeBanner = new HudEpisodeBanner();
			this.m_episodeBanner.Initialize(base.gameObject);
		}
		if (this.m_quickCampainBaner == null)
		{
			this.m_quickCampainBaner = base.gameObject.AddComponent<HudCampaignBanner>();
			this.m_quickCampainBaner.Initialize(base.gameObject, true);
		}
		if (this.m_endlessCampainBaner == null)
		{
			this.m_endlessCampainBaner = base.gameObject.AddComponent<HudCampaignBanner>();
			this.m_endlessCampainBaner.Initialize(base.gameObject, false);
		}
		if (this.m_quickModeStagePicture == null)
		{
			this.m_quickModeStagePicture = new HudQuickModeStagePicture();
			this.m_quickModeStagePicture.Initialize(base.gameObject);
		}
		if (this.m_endlessModeRankingButton == null)
		{
			this.m_endlessModeRankingButton = new HudMainMenuRankingButton();
			this.m_endlessModeRankingButton.Intialize(base.gameObject, false);
		}
		if (this.m_quickModeRankingButton == null)
		{
			this.m_quickModeRankingButton = new HudMainMenuRankingButton();
			this.m_quickModeRankingButton.Intialize(base.gameObject, true);
		}
		if (this.m_dailyBattleButton == null)
		{
			this.m_dailyBattleButton = new HudDailyBattleButton();
			this.m_dailyBattleButton.Initialize(base.gameObject);
		}
		if (this.m_quickModeQuestionButton == null)
		{
			this.m_quickModeQuestionButton = base.gameObject.AddComponent<HudQuestionButton>();
			this.m_quickModeQuestionButton.Initialize(true);
		}
		if (this.m_endlessModeQuestionButton == null)
		{
			this.m_endlessModeQuestionButton = base.gameObject.AddComponent<HudQuestionButton>();
			this.m_endlessModeQuestionButton.Initialize(false);
		}
		HudMenuUtility.SendChangeHeaderText("ui_Header_MainPage2");
		BackKeyManager.AddMainMenuUI(base.gameObject);
	}

	private void Update()
	{
		if (this.m_endlessModeRankingButton != null)
		{
			this.m_endlessModeRankingButton.Update();
		}
		if (this.m_quickModeRankingButton != null)
		{
			this.m_quickModeRankingButton.Update();
		}
		if (this.m_dailyBattleButton != null)
		{
			this.m_dailyBattleButton.Update();
		}
	}

	private void OnUpdateQuickModeData()
	{
		if (this.m_quickModeStagePicture != null)
		{
			this.m_quickModeStagePicture.UpdateDisplay();
		}
		if (this.m_quickCampainBaner != null)
		{
			this.m_quickCampainBaner.UpdateView();
		}
		if (this.m_endlessCampainBaner != null)
		{
			this.m_endlessCampainBaner.UpdateView();
		}
	}
}
