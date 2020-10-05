using System;
using Text;
using UnityEngine;

public class HudEventResult : MonoBehaviour
{
	public enum EventType
	{
		SP_STAGE,
		RAID_BOSS,
		ANIMAL,
		NUM
	}

	public enum AnimType
	{
		IDLE,
		IN,
		IN_BONUS,
		WAIT_ADD_COLLECT_OBJECT,
		ADD_COLLECT_OBJECT,
		SHOW_QUOTA_LIST,
		OUT_WAIT,
		OUT,
		NUM
	}

	private enum State
	{
		STATE_IDLE,
		STATE_TIME_UP_WINDOW_START,
		STATE_TIME_UP_WINDOW,
		STATE_RESULT_START,
		STATE_RESULT,
		NUM
	}

	public delegate void AnimationEndCallback(HudEventResult.AnimType animType);

	private HudEventResultParts m_parts;

	private HudEventResult.AnimType m_currentAnim;

	private bool m_isEndResult;

	private bool m_isEndOutAnim;

	private bool m_eventTimeup;

	private long m_beforeTotalPoint;

	private HudEventResult.State m_state;

	public bool IsEndResult
	{
		get
		{
			return this.m_isEndResult;
		}
		private set
		{
		}
	}

	public bool IsEndOutAnim
	{
		get
		{
			return this.m_isEndOutAnim;
		}
		private set
		{
		}
	}

	public bool IsBackkeyEnable()
	{
		bool result = true;
		if (this.m_parts != null)
		{
			result = this.m_parts.IsBackkeyEnable();
		}
		return result;
	}

	public void Setup(bool eventTimeup)
	{
		if (this.m_parts != null)
		{
			UnityEngine.Object.Destroy(this.m_parts.gameObject);
			this.m_parts = null;
		}
		this.m_eventTimeup = eventTimeup;
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			instance.SetEventInfo();
		}
		HudEventResultParts component = base.gameObject.GetComponent<HudEventResultParts>();
		if (component != null)
		{
			this.m_parts = component;
			this.m_parts.Init(base.gameObject, this.m_beforeTotalPoint, new HudEventResult.AnimationEndCallback(this.EventResultAnimEndCallback));
		}
	}

	public void PlayStart()
	{
		this.m_isEndResult = false;
		base.gameObject.SetActive(true);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "EventResult_Anim");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (this.m_eventTimeup)
		{
			this.m_state = HudEventResult.State.STATE_TIME_UP_WINDOW_START;
		}
		else
		{
			this.m_state = HudEventResult.State.STATE_RESULT_START;
		}
	}

	public void PlayOutAnimation()
	{
		if (this.m_eventTimeup)
		{
			this.m_isEndOutAnim = true;
			return;
		}
		this.m_isEndOutAnim = false;
		if (this.m_parts != null)
		{
			this.m_currentAnim = HudEventResult.AnimType.OUT;
			this.m_parts.PlayAnimation(this.m_currentAnim);
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Result");
			if (gameObject2 != null)
			{
				Vector3 localPosition = base.gameObject.transform.localPosition;
				Vector3 localScale = base.gameObject.transform.localScale;
				base.gameObject.transform.parent = gameObject2.transform;
				base.gameObject.transform.localPosition = localPosition;
				base.gameObject.transform.localScale = localScale;
			}
		}
		base.gameObject.SetActive(false);
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			switch (instance.Type)
			{
			case EventManager.EventType.SPECIAL_STAGE:
				base.gameObject.AddComponent<HudEventResultSpStage>();
				if (instance.SpecialStageInfo != null)
				{
					this.m_beforeTotalPoint = instance.SpecialStageInfo.totalPoint;
				}
				break;
			case EventManager.EventType.RAID_BOSS:
				base.gameObject.AddComponent<HudEventResultRaidBoss>();
				if (instance.RaidBossInfo != null)
				{
					this.m_beforeTotalPoint = instance.RaidBossInfo.totalPoint;
				}
				break;
			case EventManager.EventType.COLLECT_OBJECT:
				base.gameObject.AddComponent<HudEventResultCollect>();
				if (instance.EtcEventInfo != null)
				{
					this.m_beforeTotalPoint = instance.EtcEventInfo.totalPoint;
				}
				break;
			}
		}
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case HudEventResult.State.STATE_TIME_UP_WINDOW_START:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "EventTimeupResult",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "event_finished_game_result_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "event_finished_game_result").text
			});
			this.m_state = HudEventResult.State.STATE_TIME_UP_WINDOW;
			break;
		case HudEventResult.State.STATE_TIME_UP_WINDOW:
			if (GeneralWindow.IsCreated("EventTimeupResult") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.m_isEndOutAnim = true;
				this.m_state = HudEventResult.State.STATE_RESULT;
			}
			break;
		case HudEventResult.State.STATE_RESULT_START:
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "EventResult_Anim");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			if (this.m_parts != null)
			{
				this.m_currentAnim = HudEventResult.AnimType.IN;
				this.m_parts.PlayAnimation(this.m_currentAnim);
			}
			this.m_state = HudEventResult.State.STATE_RESULT;
			break;
		}
		}
	}

	private void EventResultAnimEndCallback(HudEventResult.AnimType animType)
	{
		this.m_currentAnim = animType + 1;
		if (this.m_currentAnim == HudEventResult.AnimType.OUT_WAIT)
		{
			this.m_isEndResult = true;
			return;
		}
		if (this.m_currentAnim >= HudEventResult.AnimType.NUM)
		{
			this.m_isEndOutAnim = true;
			this.m_currentAnim = HudEventResult.AnimType.IDLE;
			return;
		}
		if (this.m_parts != null)
		{
			this.m_parts.PlayAnimation(this.m_currentAnim);
		}
	}
}
