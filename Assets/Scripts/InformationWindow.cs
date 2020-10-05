using System;
using UnityEngine;

public class InformationWindow : WindowBase
{
	public enum ButtonPattern
	{
		NONE = -1,
		TEXT,
		OK,
		FEED,
		SHOP_CANCEL,
		FEED_BROWSER,
		FEED_ROULETTE,
		FEED_SHOP,
		FEED_EVENT,
		FEED_EVENT_LIST,
		BROWSER,
		ROULETTE,
		SHOP,
		EVENT,
		EVENT_LIST,
		ROULETTE_IFNO,
		QUICK_EVENT_INFO,
		DESIGNATED_AREA_TEXT,
		DESIGNATED_AREA_IMAGE,
		NUM
	}

	public struct Information
	{
		public string imageId;

		public string caption;

		public string bodyText;

		public string url;

		public Texture2D texture;

		public InformationWindow.ButtonPattern pattern;

		public InformationWindow.RankingType rankingType;
	}

	public enum RankingType
	{
		NON,
		WORLD,
		LEAGUE,
		EVENT,
		QUICK_LEAGUE
	}

	public enum ButtonType
	{
		NONE = -1,
		LEFT,
		RIGHT,
		CLOSE,
		NUM
	}

	public enum ObjNameType
	{
		ROOT,
		LEFT,
		RIGHT,
		NUM
	}

	private GameObject m_prefab;

	private readonly string[,] ButtonName;

	private readonly string[] CallbackFuncName;

	private bool[] m_pressedFlag;

	private InformationWindow.Information m_info;

	private RankingResultWorldRanking m_rankingResultWorld;

	private RankingResultWorldRanking m_eventRankingResult;

	private RankingResultLeague m_rankingResultLeague;

	private bool m_created;

	private bool m_endFlag;

	public UITexture bannerTexture
	{
		get
		{
			return base.gameObject.GetComponentInChildren<UITexture>();
		}
	}

	public InformationWindow()
	{
		string[,] expr_09 = new string[18, 3];
		expr_09[0, 0] = "pattern_0";
		expr_09[0, 1] = "Btn_1_ok";
		expr_09[1, 0] = "pattern_0";
		expr_09[1, 1] = "Btn_1_ok";
		expr_09[2, 0] = "pattern_0";
		expr_09[2, 1] = "Btn_2_post";
		expr_09[3, 0] = "pattern_2";
		expr_09[3, 1] = "Btn_cancel";
		expr_09[3, 2] = "Btn_shop";
		expr_09[4, 0] = "pattern_3";
		expr_09[4, 1] = "Btn_1_browser";
		expr_09[4, 2] = "Btn_post";
		expr_09[5, 0] = "pattern_3";
		expr_09[5, 1] = "Btn_3_roulette";
		expr_09[5, 2] = "Btn_post";
		expr_09[6, 0] = "pattern_3";
		expr_09[6, 1] = "Btn_4_shop";
		expr_09[6, 2] = "Btn_post";
		expr_09[7, 0] = "pattern_3";
		expr_09[7, 1] = "Btn_5_event";
		expr_09[7, 2] = "Btn_post";
		expr_09[8, 0] = "pattern_3";
		expr_09[8, 1] = "Btn_6_event_list";
		expr_09[8, 2] = "Btn_post";
		expr_09[9, 0] = "pattern_0";
		expr_09[9, 1] = "Btn_3_browser";
		expr_09[10, 0] = "pattern_0";
		expr_09[10, 1] = "Btn_5_roulette";
		expr_09[11, 0] = "pattern_0";
		expr_09[11, 1] = "Btn_6_shop";
		expr_09[12, 0] = "pattern_0";
		expr_09[12, 1] = "Btn_7_event";
		expr_09[13, 0] = "pattern_0";
		expr_09[13, 1] = "Btn_8_event_list";
		expr_09[14, 0] = "pattern_0";
		expr_09[14, 1] = "Btn_1_ok";
		expr_09[15, 0] = "pattern_0";
		expr_09[15, 1] = "Btn_1_ok";
		expr_09[16, 0] = "pattern_0";
		expr_09[16, 1] = "Btn_1_ok";
		expr_09[17, 0] = "pattern_0";
		expr_09[17, 1] = "Btn_1_ok";
		this.ButtonName = expr_09;
		this.CallbackFuncName = new string[]
		{
			"OnClickLeftButton",
			"OnClickRightButton",
			"OnClickCloseButton"
		};
		this.m_pressedFlag = new bool[3];
		
	}

	public bool IsButtonPress(InformationWindow.ButtonType type)
	{
		return this.m_pressedFlag[(int)type];
	}

	public bool IsEnd()
	{
		return this.m_endFlag;
	}

	private void ResetScrollBar()
	{
		if (this.m_prefab != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, "textView");
			if (gameObject != null)
			{
				UIScrollBar uIScrollBar = GameObjectUtil.FindChildGameObjectComponent<UIScrollBar>(gameObject, "Scroll_Bar");
				if (uIScrollBar != null)
				{
					uIScrollBar.value = 0f;
				}
			}
		}
	}

	private void SetRootObjActive(string rootName, bool activeFlag)
	{
		if (this.m_prefab != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, rootName);
			if (gameObject != null)
			{
				gameObject.SetActive(activeFlag);
			}
		}
	}

	private void SetObjActive(GameObject obj, bool activeFlag)
	{
		if (obj != null)
		{
			obj.SetActive(activeFlag);
		}
	}

	private void SetClickBtnCallBack(GameObject buttonRoot, string objectName, string callbackFuncName)
	{
		if (buttonRoot != null && !string.IsNullOrEmpty(objectName))
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(buttonRoot, objectName);
			if (gameObject != null)
			{
				UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
				if (component != null)
				{
					component.onFinished.Clear();
					EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
				}
				UIButtonMessage component2 = gameObject.GetComponent<UIButtonMessage>();
				if (component2 != null)
				{
					component2.target = base.gameObject;
					component2.functionName = callbackFuncName;
				}
			}
		}
	}

	private void SetActiveBtn(GameObject buttonRoot, string objectName)
	{
		if (buttonRoot != null && !string.IsNullOrEmpty(objectName))
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(buttonRoot, objectName);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	public void SetTexture(Texture2D texture)
	{
		if (texture != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, "img_ad_tex");
			if (gameObject != null)
			{
				UITexture component = gameObject.GetComponent<UITexture>();
				if (component != null)
				{
					gameObject.SetActive(true);
					component.enabled = true;
					component.height = texture.height;
					component.mainTexture = texture;
				}
			}
		}
	}

	public void Create(InformationWindow.Information info, GameObject newsWindowObj)
	{
		this.m_info = info;
		this.m_endFlag = false;
		for (int i = 0; i < this.m_pressedFlag.Length; i++)
		{
			this.m_pressedFlag[i] = false;
		}
		switch (info.rankingType)
		{
		case InformationWindow.RankingType.NON:
			this.m_created = true;
			this.CreateNormal(info, newsWindowObj);
			break;
		case InformationWindow.RankingType.WORLD:
			this.CreateWorldRanking(info);
			break;
		case InformationWindow.RankingType.LEAGUE:
			this.CreateLeagueRanking(info, false);
			break;
		case InformationWindow.RankingType.EVENT:
			this.CreateEvent(info);
			break;
		case InformationWindow.RankingType.QUICK_LEAGUE:
			this.CreateLeagueRanking(info, true);
			break;
		}
	}

	private void CreateLeagueRanking(InformationWindow.Information info, bool quick)
	{
		this.m_rankingResultLeague = RankingResultLeague.Create(info.bodyText, quick);
	}

	private void CreateWorldRanking(InformationWindow.Information info)
	{
		this.m_rankingResultWorld = RankingResultWorldRanking.GetResultWorldRanking();
		if (this.m_rankingResultWorld != null)
		{
			this.m_rankingResultWorld.Setup(RankingResultWorldRanking.ResultType.WORLD_RANKING, info.bodyText);
			this.m_rankingResultWorld.PlayStart();
		}
	}

	private void CreateEvent(InformationWindow.Information info)
	{
		this.m_eventRankingResult = RankingResultWorldRanking.GetResultWorldRanking();
		if (this.m_eventRankingResult != null)
		{
			this.m_eventRankingResult.Setup(RankingResultWorldRanking.ResultType.EVENT_RANKING, info.bodyText);
			this.m_eventRankingResult.PlayStart();
		}
	}

	private void CreateNormal(InformationWindow.Information info, GameObject newsWindowObj)
	{
		if (this.m_prefab == null)
		{
			this.m_prefab = newsWindowObj;
		}
		if (this.m_prefab != null)
		{
			this.SetCallBack();
			this.m_prefab.SetActive(false);
			this.SetRootObjActive("pattern_0", false);
			this.SetRootObjActive("pattern_1", false);
			this.SetRootObjActive("pattern_2", false);
			this.SetRootObjActive("pattern_3", false);
			this.SetRootObjActive("pattern_close", true);
			this.SetRootObjActive("textView", true);
			this.ResetScrollBar();
			int pattern = (int)info.pattern;
			string name = this.ButtonName[pattern, 0];
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, name);
			this.SetObjActive(gameObject, true);
			int childCount = gameObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				if (!(gameObject2 == null))
				{
					gameObject2.SetActive(false);
				}
			}
			this.SetActiveBtn(gameObject, this.ButtonName[pattern, 1]);
			this.SetActiveBtn(gameObject, this.ButtonName[pattern, 2]);
			GameObject buttonRoot = GameObjectUtil.FindChildGameObject(this.m_prefab, "pattern_close");
			this.SetActiveBtn(buttonRoot, "Btn_close");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_prefab, "Lbl_body");
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_prefab, "img_ad_tex");
			if (gameObject4 != null)
			{
				UITexture component = gameObject4.GetComponent<UITexture>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			this.SetObjActive(gameObject3, true);
			this.SetObjActive(gameObject4, false);
			UILabel component2 = gameObject3.GetComponent<UILabel>();
			if (component2 != null)
			{
				switch (info.rankingType)
				{
				case InformationWindow.RankingType.NON:
					component2.text = info.bodyText;
					break;
				}
			}
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_prefab, "Lbl_caption");
			if (uILabel != null)
			{
				uILabel.text = info.caption;
			}
			if (info.pattern != InformationWindow.ButtonPattern.TEXT && info.pattern != InformationWindow.ButtonPattern.DESIGNATED_AREA_TEXT)
			{
				if (info.rankingType == InformationWindow.RankingType.NON)
				{
					this.SetObjActive(gameObject3, false);
				}
				InformationImageManager instance = InformationImageManager.Instance;
				if (instance != null && !string.IsNullOrEmpty(info.imageId))
				{
					instance.Load(info.imageId, false, new Action<Texture2D>(this.OnLoadedTextureCallback));
				}
			}
			this.SetESRB(info.pattern, gameObject);
			this.PlayAnimation();
			base.enabled = true;
		}
	}

	private void PlayAnimation()
	{
		if (this.m_prefab != null)
		{
			this.m_prefab.SetActive(true);
			UIPlayAnimation uIPlayAnimation = base.gameObject.GetComponent<UIPlayAnimation>();
			if (uIPlayAnimation == null)
			{
				uIPlayAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
			}
			if (uIPlayAnimation != null)
			{
				Animation component = this.m_prefab.GetComponent<Animation>();
				uIPlayAnimation.target = component;
				uIPlayAnimation.clipName = "ui_cmn_window_Anim";
				uIPlayAnimation.Play(true);
			}
		}
	}

	private void OnClickLeftButton()
	{
		this.m_pressedFlag[0] = true;
		this.m_created = false;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickRightButton()
	{
		this.m_pressedFlag[1] = true;
		this.m_created = false;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickCloseButton()
	{
		this.m_pressedFlag[2] = true;
		this.m_created = false;
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private void Update()
	{
		switch (this.m_info.rankingType)
		{
		case InformationWindow.RankingType.NON:
			base.enabled = false;
			break;
		case InformationWindow.RankingType.WORLD:
			if (this.m_rankingResultWorld != null && this.m_rankingResultWorld.IsEnd)
			{
				this.m_endFlag = true;
				base.enabled = false;
			}
			break;
		case InformationWindow.RankingType.LEAGUE:
		case InformationWindow.RankingType.QUICK_LEAGUE:
			if (this.m_rankingResultLeague != null && this.m_rankingResultLeague.IsEnd())
			{
				this.m_endFlag = true;
				base.enabled = false;
			}
			break;
		case InformationWindow.RankingType.EVENT:
			if (this.m_eventRankingResult != null && this.m_eventRankingResult.IsEnd)
			{
				this.m_endFlag = true;
				base.enabled = false;
			}
			break;
		}
	}

	private void SetESRB(InformationWindow.ButtonPattern pattern, GameObject parentObj)
	{
		if (parentObj != null)
		{
			GameObject gameObject;
			switch (pattern)
			{
			case InformationWindow.ButtonPattern.FEED:
				gameObject = GameObjectUtil.FindChildGameObject(parentObj, this.ButtonName[(int)pattern, 1]);
				goto IL_6C;
			case InformationWindow.ButtonPattern.FEED_BROWSER:
			case InformationWindow.ButtonPattern.FEED_ROULETTE:
			case InformationWindow.ButtonPattern.FEED_SHOP:
			case InformationWindow.ButtonPattern.FEED_EVENT:
			case InformationWindow.ButtonPattern.FEED_EVENT_LIST:
				gameObject = GameObjectUtil.FindChildGameObject(parentObj, this.ButtonName[(int)pattern, 2]);
				goto IL_6C;
			}
			return;
			IL_6C:
			if (gameObject != null && RegionManager.Instance != null && !RegionManager.Instance.IsUseSNS())
			{
				UIImageButton component = gameObject.GetComponent<UIImageButton>();
				if (component != null)
				{
					component.isEnabled = false;
				}
			}
		}
	}

	private void SetCallBack()
	{
		this.SetClickBtnCallBack(this.m_prefab, "Btn_close", this.CallbackFuncName[2]);
		GameObject buttonRoot = GameObjectUtil.FindChildGameObject(this.m_prefab, "pattern_0");
		this.SetClickBtnCallBack(buttonRoot, "Btn_1_ok", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_2_post", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_3_browser", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_5_roulette", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_6_shop", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_7_event", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot, "Btn_8_event_list", this.CallbackFuncName[1]);
		GameObject buttonRoot2 = GameObjectUtil.FindChildGameObject(this.m_prefab, "pattern_2");
		this.SetClickBtnCallBack(buttonRoot2, "Btn_shop", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot2, "Btn_cancel", this.CallbackFuncName[0]);
		GameObject buttonRoot3 = GameObjectUtil.FindChildGameObject(this.m_prefab, "pattern_3");
		this.SetClickBtnCallBack(buttonRoot3, "Btn_post", this.CallbackFuncName[1]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_0_unpost", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_1_browser", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_2_item", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_3_roulette", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_4_shop", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_5_event", this.CallbackFuncName[0]);
		this.SetClickBtnCallBack(buttonRoot3, "Btn_6_event_list", this.CallbackFuncName[0]);
	}

	private void OnFinishedAnimationCallback()
	{
		if (this.m_prefab != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, "img_ad_tex");
			if (gameObject != null)
			{
				UITexture component = gameObject.GetComponent<UITexture>();
				if (component != null && component.mainTexture != null)
				{
					component.mainTexture = null;
				}
			}
			this.m_prefab.SetActive(false);
		}
		this.m_endFlag = true;
	}

	private void OnLoadedTextureCallback(Texture2D texture)
	{
		this.SetTexture(texture);
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_created)
		{
			if (msg != null)
			{
				msg.StaySequence();
			}
			if (this.m_prefab != null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefab, "Btn_close");
				if (gameObject != null && gameObject.activeSelf)
				{
					UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
					if (component != null)
					{
						component.SendMessage("OnClick");
					}
				}
			}
		}
	}
}
