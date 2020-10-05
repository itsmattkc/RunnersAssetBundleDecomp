using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ui_daily_battle_scroll : MonoBehaviour
{
	private sealed class _LoadUserFaceTexture_c__AnonStoreyFB
	{
		internal UITexture uiTex;

		internal void __m__66(Texture2D _faceTexture)
		{
			this.uiTex.mainTexture = _faceTexture;
			this.uiTex.alpha = 1f;
		}
	}

	public const float IMAGE_DELAY = 0.2f;

	private static float s_updateLastTime;

	[SerializeField]
	private UIButtonMessage m_buttonMessage;

	[SerializeField]
	private UILabel m_date;

	[SerializeField]
	private GameObject m_winSet;

	[SerializeField]
	private GameObject m_loseSet;

	[SerializeField]
	private GameObject m_playerL;

	[SerializeField]
	private GameObject m_playerR;

	[SerializeField]
	private GameObject m_noMatchingL;

	[SerializeField]
	private GameObject m_noMatchingR;

	private GameObject m_nonParticipating;

	private ServerDailyBattleDataPair m_dailyBattleData;

	private RankingUtil.Ranker m_rankerL;

	private RankingUtil.Ranker m_rankerR;

	private bool m_isImgLoad;

	private BoxCollider m_collider;

	private UITexture m_playerTexL;

	private UISprite m_playerLeagueIconMainL;

	private UISprite m_playerLeagueIconSubL;

	private UISprite m_playerFriendIconL;

	private UILabel m_playerScoreL;

	private UILabel m_playerNameL;

	private UITexture m_playerTexR;

	private UISprite m_playerLeagueIconMainR;

	private UISprite m_playerLeagueIconSubR;

	private UISprite m_playerFriendIconR;

	private UILabel m_playerScoreR;

	private UILabel m_playerNameR;

	private void Start()
	{
	}

	public void Update()
	{
	}

	public void DrawClear()
	{
		if (this.m_playerTexL != null && this.m_playerTexL.alpha > 0f)
		{
			UnityEngine.Object.DestroyImmediate(this.m_playerTexL.mainTexture);
			this.m_playerTexL.alpha = 0f;
		}
		if (this.m_playerTexR != null && this.m_playerTexR.alpha > 0f)
		{
			UnityEngine.Object.DestroyImmediate(this.m_playerTexR.mainTexture);
			this.m_playerTexR.alpha = 0f;
		}
		this.m_isImgLoad = false;
	}

	public void InitSetupObject()
	{
		if (this.m_buttonMessage != null)
		{
			this.m_buttonMessage.target = base.gameObject;
			this.m_buttonMessage.functionName = "OnClickDailyBattelScroll";
		}
		if (this.m_nonParticipating == null)
		{
			this.m_nonParticipating = GameObjectUtil.FindChildGameObject(base.gameObject, "duel_lose_default_set");
			if (this.m_nonParticipating != null)
			{
				this.m_nonParticipating.SetActive(false);
			}
		}
		if (this.m_collider == null)
		{
			this.m_collider = base.gameObject.GetComponent<BoxCollider>();
		}
		if (this.m_collider != null)
		{
			this.m_collider.enabled = false;
		}
		if (this.m_date != null)
		{
			this.m_date.text = string.Empty;
		}
		if (this.m_winSet != null)
		{
			this.m_winSet.SetActive(false);
		}
		if (this.m_loseSet != null)
		{
			this.m_loseSet.SetActive(false);
		}
		if (this.m_noMatchingL != null)
		{
			this.m_noMatchingL.SetActive(false);
		}
		if (this.m_noMatchingR != null)
		{
			this.m_noMatchingR.SetActive(false);
		}
		if (this.m_playerL != null)
		{
			this.m_playerTexL = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_playerL, "img_icon_friends");
			this.m_playerLeagueIconMainL = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerL, "img_icon_league");
			this.m_playerLeagueIconSubL = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerL, "img_icon_league_sub");
			this.m_playerFriendIconL = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerL, "img_friend_icon");
			this.m_playerScoreL = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_playerL, "Lbl_score");
			this.m_playerNameL = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_playerL, "Lbl_username");
			this.m_playerL.SetActive(false);
		}
		if (this.m_playerR != null)
		{
			this.m_playerTexR = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_playerR, "img_icon_friends");
			this.m_playerLeagueIconMainR = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerR, "img_icon_league");
			this.m_playerLeagueIconSubR = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerR, "img_icon_league_sub");
			this.m_playerFriendIconR = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_playerR, "img_friend_icon");
			this.m_playerScoreR = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_playerR, "Lbl_score");
			this.m_playerNameR = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_playerR, "Lbl_username");
			this.m_playerR.SetActive(false);
		}
		if (this.m_playerTexL != null && this.m_playerTexR != null)
		{
			this.m_playerTexL.alpha = 0f;
			this.m_playerTexR.alpha = 0f;
		}
		if (this.m_playerLeagueIconMainL != null && this.m_playerLeagueIconSubL != null && this.m_playerLeagueIconMainR != null && this.m_playerLeagueIconSubR != null)
		{
			this.m_playerLeagueIconMainL.spriteName = string.Empty;
			this.m_playerLeagueIconSubL.spriteName = string.Empty;
			this.m_playerLeagueIconMainR.spriteName = string.Empty;
			this.m_playerLeagueIconSubR.spriteName = string.Empty;
		}
		if (this.m_playerFriendIconL != null && this.m_playerFriendIconR != null)
		{
			this.m_playerFriendIconL.spriteName = string.Empty;
			this.m_playerFriendIconR.spriteName = string.Empty;
		}
		if (this.m_playerScoreL != null && this.m_playerNameL != null && this.m_playerScoreR != null && this.m_playerNameR != null)
		{
			this.m_playerScoreL.text = "0";
			this.m_playerScoreR.text = "0";
			this.m_playerNameL.text = string.Empty;
			this.m_playerNameR.text = string.Empty;
		}
	}

	public void UpdateView(ServerDailyBattleDataPair data)
	{
		this.m_dailyBattleData = data;
		ui_daily_battle_scroll.s_updateLastTime = Time.realtimeSinceStartup;
		base.enabled = true;
		this.InitSetupObject();
		this.m_rankerL = null;
		this.m_rankerR = null;
		if (this.m_dailyBattleData != null)
		{
			if (this.m_collider != null && !this.m_dailyBattleData.isDummyData)
			{
				this.m_collider.enabled = true;
			}
			if (this.m_date != null)
			{
				if (!this.m_dailyBattleData.isDummyData && this.m_dailyBattleData.winFlag > 0)
				{
					this.m_date.text = this.m_dailyBattleData.starDateString;
					if (this.m_nonParticipating != null)
					{
						this.m_nonParticipating.SetActive(false);
					}
				}
				else
				{
					string starDateString = this.m_dailyBattleData.starDateString;
					string dateStringHour = GeneralUtil.GetDateStringHour(this.m_dailyBattleData.endTime, -24);
					if (starDateString == dateStringHour)
					{
						this.m_date.text = starDateString;
					}
					else
					{
						this.m_date.text = starDateString + " - " + dateStringHour;
					}
					if (this.m_nonParticipating != null)
					{
						this.m_nonParticipating.SetActive(true);
					}
				}
			}
			if (this.m_winSet != null && this.m_loseSet != null)
			{
				if (this.m_dailyBattleData.winFlag >= 2)
				{
					this.m_winSet.SetActive(true);
					this.m_loseSet.SetActive(false);
				}
				else
				{
					this.m_winSet.SetActive(false);
					this.m_loseSet.SetActive(true);
				}
			}
			if (this.m_dailyBattleData.myBattleData != null && !string.IsNullOrEmpty(this.m_dailyBattleData.myBattleData.userId))
			{
				this.m_rankerL = new RankingUtil.Ranker(this.m_dailyBattleData.myBattleData);
			}
			if (this.m_dailyBattleData.rivalBattleData != null && !string.IsNullOrEmpty(this.m_dailyBattleData.rivalBattleData.userId))
			{
				this.m_rankerR = new RankingUtil.Ranker(this.m_dailyBattleData.rivalBattleData);
			}
			if (this.m_rankerL != null)
			{
				if (this.m_playerLeagueIconMainL != null && this.m_playerLeagueIconSubL != null)
				{
					this.m_playerLeagueIconMainL.spriteName = RankingUtil.GetLeagueIconName((LeagueType)this.m_rankerL.leagueIndex);
					this.m_playerLeagueIconSubL.spriteName = RankingUtil.GetLeagueIconName2((LeagueType)this.m_rankerL.leagueIndex);
				}
				if (this.m_playerFriendIconL != null)
				{
					this.m_playerFriendIconL.spriteName = RankingUtil.GetFriendIconSpriteName(this.m_rankerL);
				}
				if (this.m_playerScoreL != null && this.m_playerNameL != null)
				{
					this.m_playerNameL.text = this.m_rankerL.userName;
					this.m_playerScoreL.text = HudUtility.GetFormatNumString<long>(this.m_rankerL.score);
				}
				this.m_playerL.SetActive(true);
			}
			else
			{
				this.m_playerL.SetActive(false);
				if (this.m_noMatchingL != null)
				{
					this.m_noMatchingL.SetActive(true);
				}
			}
			if (this.m_rankerR != null)
			{
				if (this.m_playerLeagueIconMainR != null && this.m_playerLeagueIconSubR != null)
				{
					this.m_playerLeagueIconMainR.spriteName = RankingUtil.GetLeagueIconName((LeagueType)this.m_rankerR.leagueIndex);
					this.m_playerLeagueIconSubR.spriteName = RankingUtil.GetLeagueIconName2((LeagueType)this.m_rankerR.leagueIndex);
				}
				if (this.m_playerFriendIconR != null)
				{
					this.m_playerFriendIconR.spriteName = RankingUtil.GetFriendIconSpriteName(this.m_rankerR);
				}
				if (this.m_playerScoreR != null && this.m_playerNameR != null)
				{
					this.m_playerNameR.text = this.m_rankerR.userName;
					this.m_playerScoreR.text = HudUtility.GetFormatNumString<long>(this.m_rankerR.score);
				}
				this.m_playerR.SetActive(true);
			}
			else
			{
				this.m_playerR.SetActive(false);
				if (this.m_noMatchingR != null)
				{
					this.m_noMatchingR.SetActive(true);
				}
			}
		}
		this.LoadImage();
	}

	private void LoadImage()
	{
		if (!this.m_isImgLoad)
		{
			if (this.m_rankerL != null)
			{
				this.LoadUserFaceTexture(this.m_rankerL, this.m_playerTexL);
			}
			if (this.m_rankerR != null)
			{
				this.LoadUserFaceTexture(this.m_rankerR, this.m_playerTexR);
			}
		}
		this.m_isImgLoad = true;
	}

	private void LoadUserFaceTexture(RankingUtil.Ranker ranker, UITexture uiTex)
	{
		if (ranker != null && (ranker.isFriend || ranker.isMy) && uiTex != null)
		{
			uiTex.mainTexture = RankingUtil.GetProfilePictureTexture(ranker, delegate(Texture2D _faceTexture)
			{
				uiTex.mainTexture = _faceTexture;
				uiTex.alpha = 1f;
			});
			if (uiTex.mainTexture != null)
			{
				uiTex.alpha = 1f;
			}
		}
	}

	private void OnClickDailyBattelScroll()
	{
		if (this.m_dailyBattleData != null && Mathf.Abs(ui_daily_battle_scroll.s_updateLastTime - Time.realtimeSinceStartup) > 1f)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			DailyBattleDetailWindow.Open(this.m_dailyBattleData);
		}
	}
}
