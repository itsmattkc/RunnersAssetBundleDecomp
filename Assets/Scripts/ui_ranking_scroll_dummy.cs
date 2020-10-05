using System;
using UnityEngine;

public class ui_ranking_scroll_dummy : MonoBehaviour
{
	[SerializeField]
	private UIDragPanelContents dragPanelContent;

	[SerializeField]
	private UISprite dummySprite;

	[SerializeField]
	private UIButtonMessage button;

	[SerializeField]
	private UILabel label;

	private GameObject m_rankerObject;

	private RankingUtil.Ranker m_rankerData;

	public RankingUtil.Ranker myRankerData;

	public UIRectItemStorageRanking storage;

	public SpecialStageWindow spWindow;

	public RankingUI rankingUI;

	public RankingUtil.RankingScoreType scoreType;

	public RankingUtil.RankingRankerType rankerType;

	public bool end;

	public bool top;

	public int slot;

	private BoxCollider m_boxCollider;

	public RankingUtil.Ranker rankerData
	{
		get
		{
			return this.m_rankerData;
		}
		set
		{
			if (value != null && this.m_rankerData == null)
			{
				this.m_rankerData = value;
			}
			else if (value == null)
			{
				this.m_rankerData = null;
			}
		}
	}

	public bool isMyData
	{
		get
		{
			bool result = false;
			if (this.myRankerData != null && this.m_rankerData != null && this.m_rankerData.id == this.myRankerData.id)
			{
				result = true;
			}
			return result;
		}
	}

	public bool isMask
	{
		get
		{
			bool result = false;
			if (this.dummySprite != null)
			{
				result = this.dummySprite.gameObject.activeSelf;
			}
			return result;
		}
	}

	public void SetActiveObject(bool act, float delay = 0f)
	{
		if (this.dragPanelContent != null)
		{
			this.dragPanelContent.draggablePanel = this.storage.parentPanel;
		}
		if (this.m_boxCollider == null)
		{
			this.m_boxCollider = base.gameObject.GetComponentInChildren<BoxCollider>();
		}
		if (act)
		{
			if (this.m_rankerObject == null && this.storage != null)
			{
				this.m_rankerObject = this.storage.GetFreeObject();
				if (this.m_rankerObject != null)
				{
					this.UpdateRanker(delay);
				}
				else if (this.m_rankerData != null && this.spWindow != null)
				{
					RankingUtil.Ranker currentRanker = this.spWindow.GetCurrentRanker(this.slot);
					if (currentRanker != null && !this.m_rankerData.CheckRankerIdentity(currentRanker))
					{
						this.m_rankerData = currentRanker;
						this.UpdateRanker(delay);
					}
				}
				else if (this.m_rankerData != null && this.rankingUI != null)
				{
					RankingUtil.Ranker currentRanker2 = this.rankingUI.GetCurrentRanker(this.slot);
					if (currentRanker2 != null && !this.m_rankerData.CheckRankerIdentity(currentRanker2))
					{
						this.m_rankerData = currentRanker2;
						this.UpdateRanker(delay);
					}
				}
			}
		}
		else
		{
			if (this.button != null)
			{
				this.button.target = null;
			}
			if (this.label != null)
			{
				this.label.text = string.Empty;
			}
			this.SetMask(1f);
			if (this.m_rankerObject != null)
			{
				this.DrawClear();
				this.m_rankerObject.SetActive(false);
				this.m_rankerObject = null;
			}
			this.top = false;
		}
		if (this.m_boxCollider != null)
		{
			this.m_boxCollider.enabled = act;
		}
		base.gameObject.SetActive(act);
	}

	public void DrawClear()
	{
		if (this.m_rankerObject != null)
		{
			ui_ranking_scroll componentInChildren = this.m_rankerObject.GetComponentInChildren<ui_ranking_scroll>();
			if (componentInChildren != null)
			{
				componentInChildren.DrawClear();
			}
		}
	}

	public void SetMask(float alpha = 0f)
	{
		if (this.dummySprite != null)
		{
			if (alpha <= 0f)
			{
				this.dummySprite.gameObject.SetActive(false);
				this.dummySprite.alpha = 0f;
				base.enabled = false;
			}
			else
			{
				this.dummySprite.gameObject.SetActive(true);
				this.dummySprite.alpha = alpha;
				base.enabled = true;
			}
		}
	}

	public bool IsCreating(float line = 0f)
	{
		return this.storage != null && this.storage.IsCreating(line);
	}

	public void CheckCreate()
	{
		if (this.storage != null)
		{
			this.storage.CheckCreate();
		}
	}

	private void UpdateRanker(float delay)
	{
		if (this.m_rankerObject != null)
		{
			this.m_rankerObject.transform.localPosition = base.transform.localPosition;
			if (this.m_rankerData != null)
			{
				this.m_rankerObject.SetActive(true);
				ui_ranking_scroll componentInChildren = this.m_rankerObject.GetComponentInChildren<ui_ranking_scroll>();
				if (componentInChildren != null)
				{
					bool myCell = false;
					if (this.myRankerData != null && this.m_rankerData.id == this.myRankerData.id)
					{
						myCell = true;
					}
					componentInChildren.UpdateViewAsync(this.scoreType, this.rankerType, this.m_rankerData, this.end, myCell, delay, this);
					if (this.button != null)
					{
						this.button.target = null;
					}
					if (this.label != null)
					{
						this.label.text = string.Empty;
					}
					this.end = false;
				}
				UIRectItemStorageSlot component = this.m_rankerObject.GetComponent<UIRectItemStorageSlot>();
				if (component != null)
				{
					component.storage = null;
					component.storageRanking = this.storage;
					component.slot = this.m_rankerData.rankIndex;
				}
			}
		}
	}

	private void Update()
	{
		if (this.dummySprite != null)
		{
			if ((double)this.dummySprite.alpha < 1.0)
			{
				this.SetMask(this.dummySprite.alpha - Time.deltaTime * 10f);
			}
		}
		else
		{
			base.enabled = false;
		}
	}
}
