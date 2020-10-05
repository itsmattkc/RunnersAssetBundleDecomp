using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RankingResultWorldRanking : WindowBase
{
	public enum ResultType
	{
		WORLD_RANKING,
		QUICK_WORLD_RANKING,
		EVENT_RANKING,
		NUM
	}

	private sealed class _OnInAnimationFinishCallback_c__Iterator53 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal Animation _animation___0;

		internal int _PC;

		internal object _current;

		internal RankingResultWorldRanking __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._animation___0 = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.__f__this.gameObject, "ranking_window");
				if (this._animation___0 != null)
				{
					ActiveAnimation.Play(this._animation___0, "ui_ranking_world_event_Anim", Direction.Forward);
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private bool m_isSetup;

	private bool m_isOpened;

	private bool m_isEnd;

	private RankingResultWorldRanking.ResultType m_resultType;

	private InfoDecoder m_decoder;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
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
	}

	private RankingResultWorldRanking.ResultType GetResultType(int id)
	{
		if (id == NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID)
		{
			return RankingResultWorldRanking.ResultType.WORLD_RANKING;
		}
		if (id == NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID)
		{
			return RankingResultWorldRanking.ResultType.QUICK_WORLD_RANKING;
		}
		if (id == NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
		{
			return RankingResultWorldRanking.ResultType.EVENT_RANKING;
		}
		return RankingResultWorldRanking.ResultType.WORLD_RANKING;
	}

	public void Setup(NetNoticeItem item)
	{
		RankingResultWorldRanking.ResultType resultType = this.GetResultType((int)item.Id);
		this.Setup(resultType, item.Message);
	}

	public void Setup(RankingResultWorldRanking.ResultType resultType, string messageInfo)
	{
		base.gameObject.SetActive(true);
		this.m_resultType = resultType;
		switch (this.m_resultType)
		{
		case RankingResultWorldRanking.ResultType.WORLD_RANKING:
			this.m_decoder = new InfoDecoderWorldRanking(messageInfo);
			break;
		case RankingResultWorldRanking.ResultType.QUICK_WORLD_RANKING:
			this.m_decoder = new InfoDecoderWorldRanking(messageInfo);
			break;
		case RankingResultWorldRanking.ResultType.EVENT_RANKING:
			this.m_decoder = new InfoDecoderEvent(messageInfo);
			break;
		}
		if (this.m_decoder == null)
		{
			return;
		}
		if (!this.m_isSetup)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
			if (gameObject != null)
			{
				UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickCloseButton";
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "blinder");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage2 = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage2 == null)
				{
					uIButtonMessage2 = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "OnClickCloseButton";
			}
			this.m_isSetup = true;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption");
		if (uILabel != null)
		{
			uILabel.text = this.m_decoder.GetCaption();
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption_sh");
		if (uILabel2 != null)
		{
			uILabel2.text = this.m_decoder.GetCaption();
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_ranking_ex");
		if (uILabel3 != null)
		{
			uILabel3.text = this.m_decoder.GetResultString();
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon_medal_blue");
		if (uISprite != null)
		{
			uISprite.spriteName = this.m_decoder.GetMedalSpriteName();
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "word_anim");
		if (gameObject3 != null)
		{
			gameObject3.transform.localScale = new Vector3(0f, 0f, 1f);
		}
	}

	public void PlayStart()
	{
		this.m_isEnd = false;
		this.m_isOpened = false;
		SoundManager.SePlay("sys_window_open", "SE");
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "ranking_window");
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_cmn_window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimationFinishCallack), true);
			SoundManager.SePlay("sys_result_best", "SE");
		}
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		this.m_isOpened = false;
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "ranking_window");
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_cmn_window_Anim", Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OutAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_close", "SE");
		}
	}

	private void InAnimationFinishCallack()
	{
		SoundManager.SePlay("sys_league_up", "SE");
		base.StartCoroutine(this.OnInAnimationFinishCallback());
		this.m_isOpened = true;
	}

	private IEnumerator OnInAnimationFinishCallback()
	{
		RankingResultWorldRanking._OnInAnimationFinishCallback_c__Iterator53 _OnInAnimationFinishCallback_c__Iterator = new RankingResultWorldRanking._OnInAnimationFinishCallback_c__Iterator53();
		_OnInAnimationFinishCallback_c__Iterator.__f__this = this;
		return _OnInAnimationFinishCallback_c__Iterator;
	}

	private void OutAnimationFinishCallback()
	{
		this.m_isEnd = true;
		base.gameObject.SetActive(false);
	}

	public static RankingResultWorldRanking GetResultWorldRanking()
	{
		RankingResultWorldRanking result = null;
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "menu_Anim");
			if (gameObject2 != null)
			{
				result = GameObjectUtil.FindChildGameObjectComponent<RankingResultWorldRanking>(gameObject2, "WorldRankingWindowUI");
			}
		}
		return result;
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
}
