using AnimationOrTween;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RankingResultLeague : WindowBase
{
	private enum Mode
	{
		Idle,
		Wait,
		End
	}

	private RankingResultLeague.Mode m_mode;

	private RankingServerInfoConverter m_rankingData;

	private Animation m_animation;

	private UILabel m_lblInfo;

	private UILabel m_lblLeague;

	private UISprite m_imgLeague;

	private UISprite m_imgLeagueStar;

	private bool m_quickMode;

	private bool m_isOpened;

	private bool m_close;

	private void OnDestroy()
	{
		base.Destroy();
	}

	public void Setup(string message, bool quick)
	{
		base.gameObject.SetActive(true);
		this.m_quickMode = quick;
		this.m_close = false;
		this.m_rankingData = new RankingServerInfoConverter(message);
		this.m_animation = base.GetComponentInChildren<Animation>();
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_open", "SE");
		}
		this.m_lblInfo = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_league_resilt_ex");
		this.m_lblLeague = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_league");
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_word_down");
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_word_up");
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_word_stay");
		uISprite.transform.localScale = new Vector3(0f, 0f, 1f);
		uISprite2.transform.localScale = new Vector3(0f, 0f, 1f);
		uISprite3.transform.localScale = new Vector3(0f, 0f, 1f);
		this.m_imgLeague = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon_league");
		this.m_imgLeagueStar = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon_league_sub");
		LeagueType currentLeague = this.m_rankingData.currentLeague;
		this.m_imgLeague.spriteName = "ui_ranking_league_icon_" + RankingUtil.GetLeagueCategoryName(currentLeague).ToLower();
		this.m_imgLeagueStar.spriteName = "ui_ranking_league_icon_" + RankingUtil.GetLeagueCategoryClass(currentLeague);
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption");
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption_sh");
		if (uILabel != null && uILabel2 != null)
		{
			string commonText = TextUtility.GetCommonText("Ranking", (!this.m_quickMode) ? "ui_Lbl_caption_endless_result" : "ui_Lbl_caption_quickmode_result");
			uILabel.text = commonText;
			uILabel2.text = commonText;
		}
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_league_stay").text;
		switch (this.m_rankingData.leagueResult)
		{
		case RankingServerInfoConverter.ResultType.Up:
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_league_up").text;
			break;
		case RankingServerInfoConverter.ResultType.Down:
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_league_down").text;
			break;
		}
		string text2 = TextUtility.Replaces(text, new Dictionary<string, string>
		{
			{
				"{PARAM}",
				RankingUtil.GetLeagueName(currentLeague)
			}
		});
		this.m_lblInfo.text = this.m_rankingData.rankingResultLeagueText;
		this.m_lblLeague.text = text2;
		this.m_mode = RankingResultLeague.Mode.Wait;
	}

	public bool IsEnd()
	{
		return this.m_mode != RankingResultLeague.Mode.Wait;
	}

	public void OnClickNoButton()
	{
		this.m_close = true;
		this.m_isOpened = false;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_rankingData != null && this.m_rankingData.leagueResult != RankingServerInfoConverter.ResultType.Error && !this.m_close)
		{
			switch (this.m_rankingData.leagueResult)
			{
			case RankingServerInfoConverter.ResultType.Up:
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_ranking_league_up_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.ResultAnimationFinishCallback), true);
				SoundManager.SePlay("sys_league_up", "SE");
				break;
			}
			case RankingServerInfoConverter.ResultType.Stay:
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_ranking_league_stay_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.ResultAnimationFinishCallback), true);
				SoundManager.SePlay("sys_league_stay", "SE");
				break;
			}
			case RankingServerInfoConverter.ResultType.Down:
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_ranking_league_down_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.ResultAnimationFinishCallback), true);
				SoundManager.SePlay("sys_league_down", "SE");
				break;
			}
			}
			this.m_isOpened = true;
		}
		else if (this.m_close)
		{
			base.gameObject.SetActive(false);
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				Transform parent2 = parent.transform.parent;
				if (parent2 != null && parent2.name == "LeagueResultWindowUI")
				{
					parent2.gameObject.SetActive(false);
				}
			}
			this.m_mode = RankingResultLeague.Mode.End;
		}
	}

	private void ResultAnimationFinishCallback()
	{
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_isOpened)
		{
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
		if (msg != null)
		{
			msg.StaySequence();
		}
	}

	public static RankingResultLeague Create(NetNoticeItem item)
	{
		return RankingResultLeague.Create(item.Message, item.Id == (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID);
	}

	public static RankingResultLeague Create(string message, bool quick)
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "LeagueResultWindowUI");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "league_window");
				if (gameObject2 != null)
				{
					RankingResultLeague rankingResultLeague = gameObject2.GetComponent<RankingResultLeague>();
					if (rankingResultLeague == null)
					{
						rankingResultLeague = gameObject2.AddComponent<RankingResultLeague>();
					}
					if (rankingResultLeague != null)
					{
						rankingResultLeague.Setup(message, quick);
					}
					return rankingResultLeague;
				}
			}
		}
		return null;
	}
}
