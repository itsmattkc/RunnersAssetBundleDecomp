using Message;
using System;
using Text;
using UnityEngine;

public class ui_mm_new_page_ad_banner : MonoBehaviour
{
	public class BannerInfo
	{
		public int index;

		public int type;

		public NetNoticeItem item;

		public InformationWindow.Information info;
	}

	[SerializeField]
	private UITexture m_uiTexture;

	[SerializeField]
	private GameObject m_badgeAlert;

	private EasySnsFeed m_easySnsFeed;

	private ui_mm_new_page_ad_banner.BannerInfo m_bannerInfo;

	private InformationWindow m_infoWindow;

	private bool m_buttonPressFlag;

	private bool m_init;

	private void Start()
	{
		base.enabled = false;
		UIButtonMessage component = base.gameObject.GetComponent<UIButtonMessage>();
		if (component != null)
		{
			component.functionName = "OnClickScroll";
		}
	}

	public void Update()
	{
		if (this.m_easySnsFeed != null)
		{
			EasySnsFeed.Result result = this.m_easySnsFeed.Update();
			if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
			{
				this.m_easySnsFeed = null;
				base.enabled = false;
			}
		}
		if (this.m_infoWindow != null)
		{
			if (this.m_buttonPressFlag)
			{
				if (this.m_infoWindow.IsEnd() && this.m_easySnsFeed == null)
				{
					base.enabled = false;
				}
			}
			else if (this.m_infoWindow.IsButtonPress(InformationWindow.ButtonType.LEFT))
			{
				this.ClickLeftButton();
				this.m_buttonPressFlag = true;
			}
			else if (this.m_infoWindow.IsButtonPress(InformationWindow.ButtonType.RIGHT))
			{
				this.ClickRightButton();
				this.m_buttonPressFlag = true;
			}
			else if (this.m_infoWindow.IsButtonPress(InformationWindow.ButtonType.CLOSE))
			{
				this.ClickCloseButton();
				this.m_buttonPressFlag = true;
			}
		}
	}

	private void CreateEasySnsFeed()
	{
		if (this.m_bannerInfo != null)
		{
			this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/Anchor_5_MC", TextUtility.GetCommonText("ItemRoulette", "feed_jackpot_caption"), this.m_bannerInfo.info.bodyText, null);
		}
	}

	private void RequestGetEventList()
	{
		if (EventManager.Instance != null)
		{
			if (EventManager.Instance.IsSetEventStateInfo)
			{
				this.RequestLoadEventResource();
			}
			else
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerGetEventReward(EventManager.Instance.Id, base.gameObject);
				}
			}
		}
	}

	private void ClickLeftButton()
	{
		if (this.m_bannerInfo != null)
		{
			switch (this.m_bannerInfo.info.pattern)
			{
			case InformationWindow.ButtonPattern.FEED_BROWSER:
				Application.OpenURL(this.m_bannerInfo.info.url);
				break;
			case InformationWindow.ButtonPattern.FEED_ROULETTE:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO_ROULETTE, false);
				break;
			case InformationWindow.ButtonPattern.FEED_SHOP:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
				break;
			case InformationWindow.ButtonPattern.FEED_EVENT:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.PLAY_EVENT, false);
				break;
			case InformationWindow.ButtonPattern.FEED_EVENT_LIST:
				this.RequestGetEventList();
				break;
			}
		}
	}

	private void ClickRightButton()
	{
		if (this.m_bannerInfo != null)
		{
			switch (this.m_bannerInfo.info.pattern)
			{
			case InformationWindow.ButtonPattern.FEED:
				this.CreateEasySnsFeed();
				break;
			case InformationWindow.ButtonPattern.SHOP_CANCEL:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
				break;
			case InformationWindow.ButtonPattern.FEED_BROWSER:
				this.CreateEasySnsFeed();
				break;
			case InformationWindow.ButtonPattern.FEED_ROULETTE:
				this.CreateEasySnsFeed();
				break;
			case InformationWindow.ButtonPattern.FEED_SHOP:
				this.CreateEasySnsFeed();
				break;
			case InformationWindow.ButtonPattern.FEED_EVENT:
				this.CreateEasySnsFeed();
				break;
			case InformationWindow.ButtonPattern.BROWSER:
				Application.OpenURL(this.m_bannerInfo.info.url);
				break;
			case InformationWindow.ButtonPattern.ROULETTE:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO_ROULETTE, false);
				break;
			case InformationWindow.ButtonPattern.SHOP:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
				break;
			case InformationWindow.ButtonPattern.EVENT:
				HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.PLAY_EVENT, false);
				break;
			case InformationWindow.ButtonPattern.EVENT_LIST:
				this.RequestGetEventList();
				break;
			}
		}
	}

	private void ClickCloseButton()
	{
	}

	private void SaveInformation()
	{
		if (this.m_bannerInfo != null && this.m_bannerInfo.item != null && !ServerInterface.NoticeInfo.IsCheckedForMenuIcon(this.m_bannerInfo.item))
		{
			ServerInterface.NoticeInfo.m_isShowedNoticeInfo = true;
			ServerInterface.NoticeInfo.UpdateChecked(this.m_bannerInfo.item);
			if (this.m_badgeAlert != null)
			{
				this.m_badgeAlert.SetActive(false);
			}
		}
	}

	public void UpdateView(ui_mm_new_page_ad_banner.BannerInfo bannerinfo)
	{
		this.m_bannerInfo = bannerinfo;
		if (this.m_bannerInfo == null)
		{
			return;
		}
		switch (this.m_bannerInfo.info.rankingType)
		{
		case InformationWindow.RankingType.NON:
			this.SetInfoBanner();
			break;
		case InformationWindow.RankingType.WORLD:
			this.SetRankingBannerAll();
			break;
		case InformationWindow.RankingType.LEAGUE:
			this.SetEndlessRankingBannerLeague();
			break;
		case InformationWindow.RankingType.EVENT:
			this.SetInfoBanner();
			break;
		case InformationWindow.RankingType.QUICK_LEAGUE:
			this.SetQuickRankingBannerLeague();
			break;
		}
		if (this.m_bannerInfo != null && this.m_bannerInfo.item != null && !ServerInterface.NoticeInfo.IsCheckedForMenuIcon(this.m_bannerInfo.item) && this.m_badgeAlert != null)
		{
			this.m_badgeAlert.SetActive(true);
		}
		this.m_init = true;
	}

	private void SetInfoBanner()
	{
		if (this.m_bannerInfo != null && !string.IsNullOrEmpty(this.m_bannerInfo.info.imageId))
		{
			InformationImageManager.Instance.Load(this.m_bannerInfo.info.imageId, true, new Action<Texture2D>(this.OnLoadCallback));
		}
	}

	private bool SetRankingBannerAll()
	{
		bool result = false;
		if (this.m_uiTexture != null)
		{
			string name = "ui_tex_ranking_all";
			GameObject gameObject = GameObject.Find(name);
			if (gameObject != null)
			{
				AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
				this.m_uiTexture.mainTexture = component.m_tex;
			}
		}
		return result;
	}

	private bool SetEndlessRankingBannerLeague()
	{
		bool result = false;
		if (this.m_uiTexture != null)
		{
			string name = "ui_tex_ranking_rival_endless_" + TextUtility.GetSuffixe();
			GameObject gameObject = GameObject.Find(name);
			if (gameObject != null)
			{
				AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
				this.m_uiTexture.mainTexture = component.m_tex;
			}
		}
		return result;
	}

	private void SetQuickRankingBannerLeague()
	{
		if (this.m_uiTexture != null)
		{
			string name = "ui_tex_ranking_rival_quick_" + TextUtility.GetSuffixe();
			GameObject gameObject = GameObject.Find(name);
			if (gameObject != null)
			{
				AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
				this.m_uiTexture.mainTexture = component.m_tex;
			}
		}
	}

	public void OnLoadCallback(Texture2D texture)
	{
		if (this.m_uiTexture != null && texture != null)
		{
			this.m_uiTexture.mainTexture = texture;
		}
	}

	public void UpdateTexture(Texture texture)
	{
		if (texture != null)
		{
			this.m_uiTexture.mainTexture = texture;
		}
	}

	private void OnClickScroll()
	{
		if (!this.m_init)
		{
			return;
		}
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "NewsWindow");
			if (gameObject != null)
			{
				SoundManager.SePlay("sys_menu_decide", "SE");
				if (this.m_bannerInfo != null)
				{
					base.enabled = true;
					this.m_buttonPressFlag = false;
					this.m_infoWindow = base.gameObject.GetComponent<InformationWindow>();
					if (this.m_infoWindow == null)
					{
						this.m_infoWindow = base.gameObject.AddComponent<InformationWindow>();
					}
					if (this.m_infoWindow != null)
					{
						this.m_infoWindow.Create(this.m_bannerInfo.info, gameObject);
					}
				}
			}
		}
		this.SaveInformation();
	}

	public void OnEndChildPage()
	{
	}

	public void OnButtonEventCallBack()
	{
		this.CreateEeventList();
	}

	private void CreateEeventList()
	{
		if (EventManager.Instance != null)
		{
			switch (EventManager.Instance.Type)
			{
			case EventManager.EventType.SPECIAL_STAGE:
				EventRewardWindow.Create(EventManager.Instance.SpecialStageInfo);
				break;
			case EventManager.EventType.RAID_BOSS:
				EventRewardWindow.Create(EventManager.Instance.RaidBossInfo);
				break;
			case EventManager.EventType.COLLECT_OBJECT:
				EventRewardWindow.Create(EventManager.Instance.EtcEventInfo);
				break;
			}
		}
	}

	private void RequestLoadEventResource()
	{
		GameObjectUtil.SendMessageFindGameObject("MainMenuButtonEvent", "OnMenuEventClicked", base.gameObject, SendMessageOptions.DontRequireReceiver);
	}

	private void ServerGetEventReward_Succeeded(MsgGetEventRewardSucceed msg)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventState(EventManager.Instance.Id, base.gameObject);
		}
	}

	private void ServerGetEventState_Succeeded(MsgGetEventStateSucceed msg)
	{
		if (EventManager.Instance != null)
		{
			EventManager.Instance.SetEventInfo();
		}
		this.RequestLoadEventResource();
	}
}
