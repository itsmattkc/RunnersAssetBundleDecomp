using System;
using System.Collections.Generic;
using UnityEngine;

public class ui_rankingbit_scroll : MonoBehaviour
{
	[SerializeField]
	private GameObject m_orgRankingUser;

	[SerializeField]
	private List<GameObject> m_pattern;

	[SerializeField]
	private UIDragPanelContents m_dragContents;

	private ui_ranking_scroll m_child;

	private RankingResultBitWindow m_parent;

	private RankingUtil.Ranker m_ranker;

	private UILabel m_differencevalue;

	private RankingUtil.RankChange m_change;

	private int m_addRank;

	private int m_currentUpRank;

	private float m_frac;

	private bool m_isMydata;

	private float m_moveTime;

	private float m_moveTimeMax;

	private Vector3 m_movePosition;

	private Vector3 m_basePosition;

	private Vector3 m_vector;

	public bool isMydata
	{
		get
		{
			return this.m_isMydata;
		}
	}

	private void Update()
	{
		if (this.m_moveTimeMax > 0f && this.m_moveTime > 0f)
		{
			this.m_moveTime -= Time.deltaTime;
			if (this.m_moveTime <= 0f)
			{
				this.m_moveTime = 0f;
				this.m_moveTimeMax = 0f;
				base.transform.localPosition = this.m_movePosition;
				base.transform.localScale = new Vector3(1f, 1f, 1f);
				this.m_child.UpdateViewRank(-1);
				if (this.m_isMydata)
				{
					this.m_parent.AutoMoveScrollEnd();
				}
			}
			else
			{
				if (this.m_moveTimeMax - this.m_moveTime < 0.2f)
				{
					float num = (this.m_moveTimeMax - this.m_moveTime) / 0.2f * 2f;
					if (num > 1f)
					{
						num = 1f - (num - 1f) * 0.5f;
					}
					if (this.m_isMydata)
					{
						base.transform.localScale = new Vector3(1f + num * 0.2f, 1f + num * 0.2f, 1f);
					}
					else
					{
						base.transform.localScale = new Vector3(1f - num * 0.2f, 1f - num * 0.2f, 1f);
					}
				}
				else if (this.m_moveTime < 0.1f)
				{
					float num = (base.transform.localScale.x - 1f) * 0.2f;
					base.transform.localScale = new Vector3(base.transform.localScale.x - num, base.transform.localScale.y - num, 1f);
				}
				float num2 = this.m_moveTime / this.m_moveTimeMax;
				if (this.m_moveTimeMax - this.m_moveTime < 0.25f)
				{
					num2 = 1f;
				}
				else if (this.m_moveTime < 0.25f)
				{
					num2 = 0f;
				}
				else
				{
					num2 = (this.m_moveTime - 0.25f) / (this.m_moveTimeMax - 0.5f);
				}
				float num3 = 1f - num2 * num2;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				this.UpdateRank(num3);
				base.transform.localPosition = new Vector3(this.m_basePosition.x + this.m_vector.x * num3, this.m_basePosition.y + this.m_vector.y * num3, this.m_basePosition.z + this.m_vector.z * num3);
			}
			if (this.m_isMydata)
			{
				this.m_parent.AutoMoveScroll(base.transform.localPosition, true);
			}
		}
	}

	public void MoveStart(float time)
	{
		if (this.m_vector.x != 0f || this.m_vector.y != 0f)
		{
			this.m_moveTime = time;
			this.m_moveTimeMax = time;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void UpdateRank(float rate)
	{
		if (this.m_addRank != 0 && this.m_frac > 0f)
		{
			bool flag = false;
			int num = 0;
			for (float num2 = 0f; num2 < 1f; num2 += this.m_frac)
			{
				if (num2 >= rate)
				{
					flag = true;
					break;
				}
				num++;
			}
			if (!flag)
			{
				this.m_child.UpdateViewRank(-1);
				this.m_frac = 0f;
			}
			else
			{
				int num3;
				if (this.m_addRank > 0)
				{
					num3 = this.m_addRank - num;
				}
				else
				{
					num3 = this.m_addRank + num;
				}
				if (this.m_differencevalue != null && this.m_currentUpRank != this.m_addRank - num3)
				{
					this.m_currentUpRank = this.m_addRank - num3;
					if (this.m_currentUpRank >= this.m_addRank)
					{
						this.m_currentUpRank = this.m_addRank;
					}
					this.m_differencevalue.text = "+" + this.m_currentUpRank.ToString();
				}
				this.m_child.UpdateViewRank(this.m_ranker.rankIndex + 1 + num3);
			}
		}
	}

	public void UpdateView(RankingResultBitWindow parent, RankingUtil.RankChange change, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, RankingUtil.Ranker ranker, int addRank, Vector3 movePos, Vector3 basePos)
	{
		this.m_currentUpRank = 0;
		this.m_frac = 0f;
		this.m_moveTime = 0f;
		this.m_moveTimeMax = 0f;
		this.m_change = change;
		this.m_ranker = ranker;
		this.m_movePosition = movePos;
		this.m_basePosition = basePos;
		this.m_vector = this.m_movePosition - this.m_basePosition;
		base.transform.localPosition = this.m_basePosition;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		this.m_addRank = addRank;
		this.m_parent = parent;
		this.m_dragContents.draggablePanel = this.m_parent.draggable;
		this.SetPattern(this.m_change);
		this.SetUser(scoreType, rankerType, ranker);
		if (this.m_addRank != 0)
		{
			this.m_frac = 1f / (float)(Mathf.Abs(this.m_addRank) + 1);
		}
		if (this.m_child != null && this.m_change == RankingUtil.RankChange.UP)
		{
			this.m_isMydata = true;
			this.m_child.SetMyRanker(this.m_isMydata);
			if (this.m_differencevalue != null)
			{
				this.m_differencevalue.text = "+" + this.m_currentUpRank.ToString();
			}
			UISprite[] componentsInChildren = this.m_child.gameObject.GetComponentsInChildren<UISprite>();
			UILabel[] componentsInChildren2 = this.m_child.gameObject.GetComponentsInChildren<UILabel>();
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				UISprite[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					UISprite uISprite = array[i];
					uISprite.depth += 50;
				}
			}
			if (componentsInChildren2 != null && componentsInChildren2.Length > 0)
			{
				UILabel[] array2 = componentsInChildren2;
				for (int j = 0; j < array2.Length; j++)
				{
					UILabel uILabel = array2[j];
					uILabel.depth += 50;
				}
			}
		}
		if (this.m_addRank != 0 && this.m_child != null)
		{
			this.m_child.UpdateViewRank(ranker.rankIndex + 1 + this.m_addRank);
		}
	}

	private void SetUser(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, RankingUtil.Ranker ranker)
	{
		if (this.m_child == null && this.m_orgRankingUser != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.m_orgRankingUser) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = default(Vector3);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			this.m_child = gameObject.GetComponentInChildren<ui_ranking_scroll>();
		}
		if (this.m_child != null)
		{
			this.m_child.UpdateView(scoreType, rankerType, ranker, false);
		}
	}

	private void SetPattern(RankingUtil.RankChange change)
	{
		if (this.m_pattern != null && this.m_pattern.Count > 0)
		{
			for (int i = 0; i < this.m_pattern.Count; i++)
			{
				if (change != RankingUtil.RankChange.UP)
				{
					if (change != RankingUtil.RankChange.DOWN)
					{
						this.m_pattern[i].SetActive(false);
					}
					else if (this.m_pattern[i].name.IndexOf("down") != -1)
					{
						this.m_pattern[i].SetActive(true);
					}
					else
					{
						this.m_pattern[i].SetActive(false);
					}
				}
				else if (this.m_pattern[i].name.IndexOf("up") != -1)
				{
					this.m_pattern[i].SetActive(true);
					this.m_differencevalue = this.m_pattern[i].GetComponentInChildren<UILabel>();
				}
				else
				{
					this.m_pattern[i].SetActive(false);
				}
			}
		}
	}

	public void Remove()
	{
		if (this.m_child != null)
		{
			UnityEngine.Object.Destroy(this.m_child.gameObject);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
