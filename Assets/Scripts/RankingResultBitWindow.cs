using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RankingResultBitWindow : MonoBehaviour
{
	private const float RANK_CHANGE_EFFECT_START_TIME = 2f;

	private const float RANK_CHANGE_EFFECT_MOVE_TIME = 0.9f;

	private const int RANKER_DATA_SIZE_H = 94;

	private const int RANKER_LIST_INIT_X = 0;

	private const int RANKER_LIST_INIT_Y = 153;

	private const int RANKER_LIST_ADD_Y = -94;

	[SerializeField]
	private GameObject m_orgRankingbit;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIPanel m_panel;

	[SerializeField]
	private UIDraggablePanel m_draggable;

	[SerializeField]
	private GameObject m_frontCollider;

	private List<ui_rankingbit_scroll> m_rankerList;

	private RankingUtil.RankingMode m_rankingMode;

	private RankingUtil.RankChange m_change;

	private int m_currentRank = -1;

	private int m_oldRank = -1;

	private float m_openTime;

	private float m_autoMoveSpeed;

	private float m_autoMoveTime;

	private bool m_isMove;

	private bool m_isEnd;

	private Vector3 m_autoMoveTargetPos;

	private static RankingResultBitWindow s_instance;

	public UIDraggablePanel draggable
	{
		get
		{
			return this.m_draggable;
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public static RankingResultBitWindow Instance
	{
		get
		{
			GameObject gameObject = GameObject.Find("UI Root (2D)");
			if (gameObject != null)
			{
				RankingResultBitWindow rankingResultBitWindow = GameObjectUtil.FindChildGameObjectComponent<RankingResultBitWindow>(gameObject, "RankingResultBitWindow");
				if (rankingResultBitWindow != null)
				{
					rankingResultBitWindow.gameObject.SetActive(true);
				}
			}
			return RankingResultBitWindow.s_instance;
		}
	}

	private void Update()
	{
		if (base.gameObject.activeSelf)
		{
			this.m_openTime += Time.deltaTime;
			if (this.m_openTime >= 2f && !this.m_isMove && this.m_rankerList != null && this.m_rankerList.Count > 0)
			{
				foreach (ui_rankingbit_scroll current in this.m_rankerList)
				{
					current.MoveStart(0.9f + (float)Mathf.Abs(this.m_oldRank - this.m_currentRank) * 0.03f);
				}
				this.m_isMove = true;
			}
			if (this.m_autoMoveTime > 0f)
			{
				this.m_autoMoveTime -= Time.deltaTime;
				if (this.m_rankerList != null)
				{
					this.AutoMove();
				}
			}
		}
	}

	public void AutoMove()
	{
		if (this.m_autoMoveSpeed > 0f && this.m_draggable != null && this.m_autoMoveTime > 0f)
		{
			float num = this.m_draggable.panel.transform.localPosition.y * -1f + this.m_draggable.panel.clipRange.w * 0.5f + -47f;
			float num2 = this.m_draggable.panel.transform.localPosition.y * -1f + this.m_draggable.panel.clipRange.w * -0.5f + 47f;
			float num3 = 0f;
			if (this.m_autoMoveTargetPos.y < num2)
			{
				num3 = this.m_autoMoveTargetPos.y - num2;
				if (num3 <= this.m_autoMoveSpeed * -1f)
				{
					num3 = this.m_autoMoveSpeed * -1f;
					this.m_autoMoveSpeed = 0f;
				}
			}
			else if (this.m_autoMoveTargetPos.y > num)
			{
				num3 = this.m_autoMoveTargetPos.y - num;
				if (num3 >= this.m_autoMoveSpeed)
				{
					num3 = this.m_autoMoveSpeed;
					this.m_autoMoveSpeed = 0f;
				}
			}
			float num4 = this.m_draggable.panel.transform.localPosition.y * -1f + num3;
			if (num4 >= -0.75f)
			{
				num4 = -0.75f;
			}
			else if (num4 <= -0.75f + (float)(this.m_rankerList.Count * -94))
			{
				num4 = -0.75f + (float)(this.m_rankerList.Count * -94);
			}
			this.m_draggable.panel.clipRange = new Vector4(0f, num4, this.m_draggable.panel.clipRange.z, this.m_draggable.panel.clipRange.w);
			this.m_draggable.panel.transform.localPosition = new Vector3(0f, this.m_draggable.panel.clipRange.y * -1f, this.m_draggable.panel.transform.localPosition.z);
		}
		else
		{
			this.m_autoMoveSpeed = 0f;
			this.m_autoMoveTime = 0f;
		}
	}

	public void AutoMoveScrollEnd()
	{
		SoundManager.SePlay("sys_rank_kettei", "SE");
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(false);
		}
	}

	public void AutoMoveScroll(Vector3 position, bool up)
	{
		if (up)
		{
			this.m_autoMoveTargetPos = new Vector3(position.x, position.y + 188f, 0f);
		}
		else
		{
			this.m_autoMoveTargetPos = new Vector3(position.x, position.y, 0f);
		}
		this.m_autoMoveSpeed = 10000f;
		this.m_autoMoveTime = 0.2f;
		if (this.m_rankerList != null)
		{
			this.AutoMove();
		}
	}

	private void OnClickNoButton()
	{
		SingletonGameObject<RankingManager>.Instance.ResetRankingRankChange(this.m_rankingMode);
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public void Close()
	{
		this.ResetRankerList();
		this.m_openTime = 0f;
		this.m_isMove = false;
		this.m_isEnd = true;
		if (this.m_panel != null)
		{
			this.m_panel.alpha = 0f;
		}
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(false);
		}
		this.RemoveBackKeyCallBack();
		base.gameObject.SetActive(false);
	}

	public void Init()
	{
		if (this.m_panel != null)
		{
			this.m_panel.alpha = 0f;
		}
		this.m_openTime = 0f;
		this.m_autoMoveSpeed = 0f;
		this.m_isMove = false;
		this.m_isEnd = false;
	}

	public bool Open(RankingUtil.RankingMode rankingMode)
	{
		this.m_rankingMode = rankingMode;
		if (this.m_panel != null)
		{
			this.m_panel.alpha = 0f;
		}
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(true);
		}
		SoundManager.SePlay("sys_window_open", "SE");
		this.m_autoMoveSpeed = 0f;
		base.gameObject.SetActive(true);
		RankingUtil.RankingRankerType rankerType = RankingUtil.RankingRankerType.RIVAL;
		RankingUtil.RankingScoreType endlessRivalRankingScoreType = RankingManager.EndlessRivalRankingScoreType;
		this.m_change = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(this.m_rankingMode, endlessRivalRankingScoreType, rankerType, out this.m_currentRank, out this.m_oldRank);
		this.m_isEnd = false;
		if (this.m_change != RankingUtil.RankChange.NONE)
		{
			SingletonGameObject<RankingManager>.Instance.GetRanking(this.m_rankingMode, endlessRivalRankingScoreType, rankerType, 0, new RankingManager.CallbackRankingData(this.CallbackRanking));
			return true;
		}
		base.gameObject.SetActive(false);
		if (this.m_frontCollider != null)
		{
			this.m_frontCollider.SetActive(false);
		}
		return false;
	}

	private ui_rankingbit_scroll CreateRankerData(int index, bool mydata, RankingUtil.Ranker ranker, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type)
	{
		this.m_openTime = 0f;
		this.m_isMove = false;
		GameObject gameObject = UnityEngine.Object.Instantiate(this.m_orgRankingbit) as GameObject;
		gameObject.transform.parent = this.m_draggable.transform;
		int rankIndex = ranker.rankIndex;
		Vector3 vector = new Vector3(0f, (float)(153 + ranker.rankIndex * -94), 0f);
		Vector3 movePos = new Vector3(0f, (float)(153 + ranker.rankIndex * -94), 0f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		ranker.userDataType = RankingUtil.UserDataType.RANK_UP;
		ui_rankingbit_scroll componentInChildren = gameObject.GetComponentInChildren<ui_rankingbit_scroll>();
		if (componentInChildren != null)
		{
			if (mydata)
			{
				vector = new Vector3(0f, (float)(153 + (rankIndex + (this.m_oldRank - this.m_currentRank)) * -94), 0f);
				componentInChildren.UpdateView(this, this.m_change, score, type, ranker, this.m_oldRank - this.m_currentRank, movePos, vector);
			}
			else
			{
				if (ranker.isFriend)
				{
					ranker.isSentEnergy = true;
				}
				if (ranker.rankIndex + 1 > this.m_currentRank && ranker.rankIndex + 1 <= this.m_oldRank)
				{
					vector = new Vector3(0f, (float)(153 + (rankIndex - 1) * -94), 0f);
					componentInChildren.UpdateView(this, RankingUtil.RankChange.DOWN, score, type, ranker, -1, movePos, vector);
				}
				else
				{
					componentInChildren.UpdateView(this, RankingUtil.RankChange.NONE, score, type, ranker, 0, vector, vector);
				}
			}
			if (this.m_rankerList == null)
			{
				this.m_rankerList = new List<ui_rankingbit_scroll>();
			}
			this.m_rankerList.Add(componentInChildren);
		}
		return componentInChildren;
	}

	private void CallbackRanking(List<RankingUtil.Ranker> rankerList, RankingUtil.RankingScoreType score, RankingUtil.RankingRankerType type, int page, bool isNext, bool isPrev, bool isCashData)
	{
		if (rankerList != null && rankerList.Count > 1 && rankerList[0] != null && this.m_orgRankingbit != null)
		{
			if (this.m_frontCollider != null)
			{
				this.m_frontCollider.SetActive(true);
			}
			if (this.m_oldRank > rankerList.Count - 1)
			{
				this.m_oldRank = rankerList.Count - 1;
			}
			ui_rankingbit_scroll ui_rankingbit_scroll = null;
			for (int i = 1; i < rankerList.Count; i++)
			{
				if (rankerList[i].id == rankerList[0].id)
				{
					ui_rankingbit_scroll = this.CreateRankerData(i - 1, true, rankerList[i], score, type);
				}
				else
				{
					this.CreateRankerData(i - 1, false, rankerList[i], score, type);
				}
			}
			if (this.m_panel != null)
			{
				this.m_panel.alpha = 1f;
			}
			this.m_draggable.ResetPosition();
			if (ui_rankingbit_scroll != null)
			{
				this.AutoMoveScroll(ui_rankingbit_scroll.transform.localPosition, false);
			}
			if (this.m_animation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim2", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
			}
		}
		else
		{
			this.m_change = RankingUtil.RankChange.NONE;
			if (this.m_panel != null)
			{
				this.m_panel.alpha = 0f;
			}
			base.gameObject.SetActive(false);
		}
	}

	private void ResetRankerList()
	{
		if (this.m_rankerList != null)
		{
			if (this.m_rankerList.Count > 0)
			{
				foreach (ui_rankingbit_scroll current in this.m_rankerList)
				{
					current.Remove();
				}
				this.m_rankerList.Clear();
			}
			this.m_rankerList = null;
		}
	}

	public void AddRanker(ui_rankingbit_scroll ranker)
	{
	}

	private void AnimationFinishCallback()
	{
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void OnDestroy()
	{
		if (RankingResultBitWindow.s_instance == this)
		{
			this.RemoveBackKeyCallBack();
			RankingResultBitWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RankingResultBitWindow.s_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			RankingResultBitWindow.s_instance = this;
			this.EntryBackKeyCallBack();
			RankingResultBitWindow.s_instance.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void EntryBackKeyCallBack()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	private void RemoveBackKeyCallBack()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
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
