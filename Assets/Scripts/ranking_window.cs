using AnimationOrTween;
using Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Text;
using UnityEngine;

public class ranking_window : WindowBase
{
	private static bool s_rankingWindowActive;

	[SerializeField]
	private Animation m_windowAnimation;

	[SerializeField]
	private GameObject[] m_scoreTypeObjects = new GameObject[2];

	[SerializeField]
	private UISprite m_rankingSprite;

	[SerializeField]
	private UILabel m_rankingLabel;

	[SerializeField]
	private UITexture m_faceTexture;

	[SerializeField]
	private UILabel m_userIdLabel;

	[SerializeField]
	private UISprite m_friendIconSprite;

	[SerializeField]
	private UILabel m_userNameLabel;

	[SerializeField]
	private UILabel m_mapRankLabel;

	[SerializeField]
	private UILabel m_daysLabel;

	[SerializeField]
	private UILabel m_charaNameLabel;

	[SerializeField]
	private UILabel m_charaLevelLabel;

	[SerializeField]
	private UISprite m_charaSprite;

	[SerializeField]
	private UISprite m_subCharaSprite;

	[SerializeField]
	private UISprite[] m_ChaoIconSprite = new UISprite[2];

	[SerializeField]
	private UISprite[] m_chaoBgSprite = new UISprite[2];

	[SerializeField]
	private UILabel[] m_chaoLevelLabel = new UILabel[2];

	[SerializeField]
	private UIImageButton m_sendButton;

	[SerializeField]
	private UISprite m_leagueIcon;

	[SerializeField]
	private UISprite m_leagueIcon2;

	private RankingUtil.RankingScoreType m_scoreType;

	private RankingUtil.RankingRankerType m_rankerType;

	private RankingUtil.Ranker m_ranker;

	private UITexture[] m_chaoIconTex = new UITexture[2];

	private UITexture m_mainCharaTex;

	private UITexture m_subCharaTex;

	public static bool isActive
	{
		get
		{
			return ranking_window.s_rankingWindowActive;
		}
	}

	private void Awake()
	{
		ranking_window.s_rankingWindowActive = false;
		if (this.m_charaSprite != null)
		{
			this.m_charaSprite.gameObject.SetActive(false);
		}
		if (this.m_subCharaSprite != null)
		{
			this.m_subCharaSprite.gameObject.SetActive(false);
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "window_row_2");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "img_main_charicon_tex");
			if (gameObject2 != null)
			{
				this.m_mainCharaTex = gameObject2.GetComponent<UITexture>();
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "img_sub_charicon_tex");
			if (gameObject3 != null)
			{
				this.m_subCharaTex = gameObject3.GetComponent<UITexture>();
			}
		}
		this.m_chaoIconTex[0] = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "img_chao_icon_main");
		this.m_chaoIconTex[1] = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "img_chao_icon_sub");
	}

	private void Update()
	{
		if (GeneralWindow.IsCreated("SendChallenge") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
		}
	}

	public void Open(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, RankingUtil.Ranker ranker, bool sendBtnDisable)
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		SoundManager.SePlay("sys_window_open", "SE");
		base.gameObject.SetActive(true);
		ranking_window.s_rankingWindowActive = true;
		ActiveAnimation.Play(this.m_windowAnimation, "ui_menu_ranking_window_Anim", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
		this.UpdateView(scoreType, rankerType, ranker, sendBtnDisable);
	}

	public void Open(RaidBossUser user)
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		SoundManager.SePlay("sys_window_open", "SE");
		base.gameObject.SetActive(true);
		ranking_window.s_rankingWindowActive = true;
		ActiveAnimation.Play(this.m_windowAnimation, "ui_menu_ranking_window_Anim", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
		this.UpdateView(user);
	}

	private void OnClose()
	{
		ranking_window.s_rankingWindowActive = false;
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void UpdateView()
	{
		bool sendBtnDisable = false;
		this.UpdateView(this.m_scoreType, this.m_rankerType, this.m_ranker, sendBtnDisable);
	}

	public void UpdateView(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, RankingUtil.Ranker ranker, bool sendBtnDisable)
	{
		this.m_scoreType = scoreType;
		this.m_rankerType = rankerType;
		this.m_ranker = ranker;
		if (this.m_ranker == null)
		{
			return;
		}
		this.m_scoreTypeObjects[0].SetActive(this.m_scoreType == RankingUtil.RankingScoreType.HIGH_SCORE && ranker.userDataType != RankingUtil.UserDataType.DAILY_BATTLE);
		this.m_scoreTypeObjects[1].SetActive(this.m_scoreType == RankingUtil.RankingScoreType.TOTAL_SCORE && ranker.userDataType != RankingUtil.UserDataType.DAILY_BATTLE);
		if (ranker.rankIndex < 3)
		{
			this.m_rankingSprite.spriteName = "ui_ranking_scroll_num_" + (ranker.rankIndex + 1);
		}
		else
		{
			this.m_rankingLabel.text = (ranker.rankIndex + 1).ToString();
		}
		this.m_rankingSprite.gameObject.SetActive(ranker.rankIndex < 3 && ranker.userDataType != RankingUtil.UserDataType.DAILY_BATTLE);
		this.m_rankingLabel.gameObject.SetActive(ranker.rankIndex >= 3 && ranker.userDataType != RankingUtil.UserDataType.DAILY_BATTLE);
		this.m_faceTexture.mainTexture = RankingUtil.GetProfilePictureTexture(ranker, delegate(Texture2D _faceTexture)
		{
			this.m_faceTexture.mainTexture = _faceTexture;
		});
		this.m_userIdLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_id").text + " " + ranker.id;
		this.m_friendIconSprite.spriteName = RankingUtil.GetFriendIconSpriteName(ranker);
		this.m_userNameLabel.text = ranker.userName;
		this.m_mapRankLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Lbl_word_rank").text + " " + ranker.dispMapRank;
		this.m_leagueIcon.spriteName = RankingUtil.GetLeagueIconName((LeagueType)ranker.leagueIndex);
		this.m_leagueIcon2.spriteName = RankingUtil.GetLeagueIconName2((LeagueType)ranker.leagueIndex);
		TimeSpan timeSpan = NetUtil.GetCurrentTime() - ranker.loginTime;
		this.m_daysLabel.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", (timeSpan.Days <= 0) ? ((timeSpan.Hours <= 0) ? "login_minutes" : "login_hours") : "login_days").text, new Dictionary<string, string>
		{
			{
				"{DAYS}",
				timeSpan.Days.ToString()
			},
			{
				"{HOURS}",
				timeSpan.Hours.ToString()
			},
			{
				"{MINUTES}",
				timeSpan.Minutes.ToString()
			}
		});
		this.m_charaNameLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)ranker.charaType]).text;
		this.m_charaLevelLabel.text = TextUtility.GetTextLevel(ranker.charaLevel.ToString("D3"));
		this.SetChara(this.m_mainCharaTex, ranker.charaType);
		this.SetChara(this.m_subCharaTex, ranker.charaSubType);
		this.SetChao(0, ranker.mainChaoId, ranker.mainChaoRarity, ranker.mainChaoLevel);
		this.SetChao(1, ranker.subChaoId, ranker.subChaoRarity, ranker.subChaoLevel);
		bool activeSelf = this.m_sendButton.gameObject.activeSelf;
		if (sendBtnDisable)
		{
			if (activeSelf)
			{
				this.m_sendButton.gameObject.SetActive(false);
			}
		}
		else
		{
			if (!activeSelf)
			{
				this.m_sendButton.gameObject.SetActive(true);
			}
			this.m_sendButton.isEnabled = (this.m_ranker.isFriend && !this.m_ranker.isSentEnergy);
		}
	}

	public void UpdateView(RaidBossUser user)
	{
		this.m_ranker = user.ConvertRankerData();
		if (this.m_ranker == null)
		{
			return;
		}
		this.m_scoreTypeObjects[0].SetActive(false);
		this.m_scoreTypeObjects[1].SetActive(false);
		this.m_rankingSprite.gameObject.SetActive(false);
		this.m_rankingLabel.gameObject.SetActive(false);
		this.m_faceTexture.mainTexture = RankingUtil.GetProfilePictureTexture(this.m_ranker, delegate(Texture2D _faceTexture)
		{
			this.m_faceTexture.mainTexture = _faceTexture;
		});
		this.m_userIdLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_id").text + " " + this.m_ranker.id;
		this.m_friendIconSprite.spriteName = RankingUtil.GetFriendIconSpriteName(this.m_ranker);
		this.m_userNameLabel.text = this.m_ranker.userName;
		this.m_mapRankLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Lbl_word_rank").text + " " + this.m_ranker.dispMapRank;
		this.m_leagueIcon.spriteName = RankingUtil.GetLeagueIconName((LeagueType)this.m_ranker.leagueIndex);
		this.m_leagueIcon2.spriteName = RankingUtil.GetLeagueIconName2((LeagueType)this.m_ranker.leagueIndex);
		TimeSpan timeSpan = NetUtil.GetCurrentTime() - this.m_ranker.loginTime;
		this.m_daysLabel.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", (timeSpan.Days <= 0) ? ((timeSpan.Hours <= 0) ? "login_minutes" : "login_hours") : "login_days").text, new Dictionary<string, string>
		{
			{
				"{DAYS}",
				timeSpan.Days.ToString()
			},
			{
				"{HOURS}",
				timeSpan.Hours.ToString()
			},
			{
				"{MINUTES}",
				timeSpan.Minutes.ToString()
			}
		});
		this.m_charaNameLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)this.m_ranker.charaType]).text;
		this.m_charaLevelLabel.text = TextUtility.GetTextLevel(this.m_ranker.charaLevel.ToString("D3"));
		this.SetChara(this.m_mainCharaTex, this.m_ranker.charaType);
		this.SetChara(this.m_subCharaTex, this.m_ranker.charaSubType);
		this.SetChao(0, this.m_ranker.mainChaoId, this.m_ranker.mainChaoRarity, this.m_ranker.mainChaoLevel);
		this.SetChao(1, this.m_ranker.subChaoId, this.m_ranker.subChaoRarity, this.m_ranker.subChaoLevel);
		this.m_sendButton.isEnabled = (this.m_ranker.isFriend && !this.m_ranker.isSentEnergy);
	}

	private void OnClickSend()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSendEnergy(this.m_ranker.id, base.gameObject);
		}
		else
		{
			this.ServerSendEnergy_Succeeded(null);
		}
	}

	private void ServerSendEnergy_Succeeded(MsgSendEnergySucceed msg)
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "SendChallenge",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "gw_send_challenge_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "gw_send_challenge_text").text
		});
		GameObjectUtil.SendMessageFindGameObject("ui_mm_ranking_page(Clone)", "OnUpdateSentEnergy", this.m_ranker.id, SendMessageOptions.DontRequireReceiver);
		string cellName = "get_present_" + this.m_ranker.language.ToString();
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "PushNotice", cellName).text;
		if (!string.IsNullOrEmpty(text))
		{
			PnoteNotification.SendMessage(text, this.m_ranker.id, PnoteNotification.LaunchOption.SendEnergy);
		}
		RankingManager.UpdateSendChallenge(this.m_ranker.id);
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			SocialUserData socialUserData = null;
			List<SocialUserData> friendList = socialInterface.FriendList;
			foreach (SocialUserData current in friendList)
			{
				if (current != null)
				{
					if (current.CustomData.GameId == this.m_ranker.id)
					{
						socialUserData = current;
						break;
					}
				}
			}
			if (socialUserData != null)
			{
			}
		}
		if (this.m_windowAnimation != null)
		{
			ActiveAnimation.Play(this.m_windowAnimation, "ui_menu_ranking_window_Anim", Direction.Reverse, EnableCondition.DoNothing, DisableCondition.DisableAfterReverse);
		}
	}

	private void ServerSendEnergy_Failed(ServerInterface.StatusCode status)
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "SendChallenge",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "gw_send_challenge_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "gw_send_challenge_error").text
		});
		if (this.m_windowAnimation != null)
		{
			ActiveAnimation.Play(this.m_windowAnimation, "ui_menu_ranking_window_Anim", Direction.Reverse, EnableCondition.DoNothing, DisableCondition.DisableAfterReverse);
		}
	}

	public static void Open(GameObject parent, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, RankingUtil.Ranker ranker, bool sendBtnDisable)
	{
		ranking_window ranking_window = GameObjectUtil.FindChildGameObjectComponent<ranking_window>(parent, "RankingWindowUI");
		if (ranking_window != null)
		{
			ranking_window.Open(scoreType, rankerType, ranker, sendBtnDisable);
		}
	}

	public static void RaidOpen(RaidBossUser user)
	{
		GameObject parent = GameObject.Find("UI Root (2D)");
		ranking_window ranking_window = GameObjectUtil.FindChildGameObjectComponent<ranking_window>(parent, "RankingWindowUI");
		if (ranking_window != null)
		{
			ranking_window.Open(user);
		}
	}

	private void SetChara(UITexture tex, CharaType charaType)
	{
		if (tex == null)
		{
			return;
		}
		if (charaType == CharaType.UNKNOWN)
		{
			tex.gameObject.SetActive(false);
		}
		else
		{
			tex.gameObject.SetActive(true);
			TextureRequestChara request = new TextureRequestChara(charaType, tex);
			TextureAsyncLoadManager.Instance.Request(request);
		}
	}

	private void SetChao(int index, int chaoId, int chaoRarity, int chaoLevel)
	{
		this.SetChaoTexture(this.m_chaoIconTex[index], chaoId);
		if (chaoId >= 0)
		{
			this.m_chaoBgSprite[index].gameObject.SetActive(true);
			this.m_chaoLevelLabel[index].gameObject.SetActive(true);
			this.m_chaoBgSprite[index].spriteName = "ui_tex_chao_bg_" + chaoRarity;
			this.m_chaoLevelLabel[index].text = TextUtility.GetTextLevel(chaoLevel.ToString());
		}
		else
		{
			this.m_chaoBgSprite[index].gameObject.SetActive(true);
			this.m_chaoLevelLabel[index].gameObject.SetActive(false);
			this.m_chaoBgSprite[index].spriteName = "ui_tex_chao_bg_x";
		}
		global::Debug.Log("SetChao m_ChaoIconSprite:" + (this.m_ChaoIconSprite != null));
	}

	private void SetChaoTexture(UITexture uiTex, int chaoId)
	{
		if (uiTex != null)
		{
			if (chaoId >= 0)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uiTex, null, true);
				ChaoTextureManager.Instance.GetTexture(chaoId, info);
				uiTex.gameObject.SetActive(true);
			}
			else
			{
				uiTex.gameObject.SetActive(false);
			}
		}
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		if (gameObject != null)
		{
			UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
			if (component != null)
			{
				component.SendMessage("OnClick");
			}
		}
	}
}
