using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class RankingFriendOptionWindow : MonoBehaviour
{
	private enum LabelId
	{
		CAPTION,
		BODY,
		ACTIVE_FRIEND,
		PAGE,
		SORT_ORDER,
		NEXT,
		BACK,
		NUM
	}

	private enum SortOrder
	{
		Ascending,
		Descending,
		NUM
	}

	private sealed class _SetUp_c__Iterator51 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ActiveAnimation _m_activeAnim___0;

		internal SocialInterface _socialInterface___1;

		internal List<SocialUserData>.Enumerator __s_619___2;

		internal SocialUserData _user___3;

		internal int _PC;

		internal object _current;

		internal RankingFriendOptionWindow __f__this;

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
				this.__f__this.m_isAnimationEnd = false;
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				this.__f__this.m_panel.alpha = 1f;
				this.__f__this.m_scrollViewPanel.alpha = 1f;
				if (this.__f__this.animation != null)
				{
					this._m_activeAnim___0 = ActiveAnimation.Play(this.__f__this.animation, Direction.Forward);
					EventDelegate.Add(this._m_activeAnim___0.onFinished, new EventDelegate.Callback(this.__f__this.OnFinishedActiveAnimation), true);
				}
				this.__f__this.m_showConfirmWindow = false;
				this.__f__this.m_page = 0;
				this.__f__this.m_sortOrder = RankingFriendOptionWindow.SortOrder.Ascending;
				this.__f__this.m_uiLabels[0].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_caption").text;
				this.__f__this.m_uiLabels[1].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_body").text;
				this.__f__this.m_buttonPage.SetActive(true);
				this.__f__this.m_friendList.Clear();
				this._socialInterface___1 = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
				if (this._socialInterface___1 != null && this._socialInterface___1.IsLoggedIn)
				{
					this.__s_619___2 = this._socialInterface___1.AllFriendList.GetEnumerator();
					try
					{
						while (this.__s_619___2.MoveNext())
						{
							this._user___3 = this.__s_619___2.Current;
							this.__f__this.m_friendList.Add(this._user___3);
						}
					}
					finally
					{
						((IDisposable)this.__s_619___2).Dispose();
					}
				}
				this.__f__this.m_pageMax = (this.__f__this.m_friendList.Count - 1) / this.__f__this.CHOSE_FRIEND_MAX;
				this.__f__this.m_chosenFriend = new List<SocialUserData>(this._socialInterface___1.FriendList);
				this.__f__this.m_simpleScrolls = GameObjectUtil.FindChildGameObjectsComponents<RankingSimpleScroll>(this.__f__this.gameObject, "ui_rankingsimple_scroll(Clone)");
				this.__f__this.ScrollListUpdate();
				this.__f__this.Sort();
				this.__f__this.EntryBackKeyCallBack();
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

	private sealed class _UpdateDraggablePanel_c__Iterator52 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal RankingFriendOptionWindow __f__this;

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
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				this.__f__this.m_draggablePanel.SendMessage("OnVerticalBar");
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

	private sealed class _ChooseFriend_c__AnonStoreyFD
	{
		internal SocialUserData user;

		internal bool __m__69(SocialUserData chooseFriend)
		{
			return chooseFriend.Id == this.user.Id;
		}
	}

	private UILabel[] m_uiLabels = new UILabel[7];

	private UIDraggablePanel m_draggablePanel;

	private GameObject m_buttonPage;

	private UIPanel m_panel;

	private UIPanel m_scrollViewPanel;

	private List<RankingSimpleScroll> m_simpleScrolls = new List<RankingSimpleScroll>();

	private List<SocialUserData> m_friendList = new List<SocialUserData>();

	private List<SocialUserData> m_chosenFriend = new List<SocialUserData>();

	private List<SocialUserData> m_confirmList = new List<SocialUserData>();

	private readonly int CHOSE_FRIEND_MAX = 50;

	private readonly int LOAD_FRIEND_IMAGE_NUM = 1;

	private readonly int VISIBLE_IMAGE_MAX = 6;

	private int m_loadedFriendImageNum;

	private int m_activeScrollCount;

	private int m_page;

	private int m_pageMax;

	private ActiveAnimation m_activeAnim;

	private bool m_isAnimationEnd;

	private bool m_showConfirmWindow;

	private string[] m_sortOrderText = new string[2];

	private RankingFriendOptionWindow.SortOrder m_sortOrder;

	private void Start()
	{
		GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "body");
		this.m_uiLabels[1] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_body");
		this.m_uiLabels[0] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption");
		this.m_uiLabels[2] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_num_activefriend");
		this.m_uiLabels[3] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_page");
		this.m_uiLabels[4] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_0d");
		this.m_uiLabels[5] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_next");
		this.m_uiLabels[6] = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_prev");
		this.m_uiLabels[5].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_page_next").text;
		this.m_uiLabels[6].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_page_back").text;
		this.m_sortOrderText[0] = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_sort_ascending").text;
		this.m_sortOrderText[1] = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_sort_descending").text;
		this.m_draggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "ScrollView");
		this.m_buttonPage = GameObjectUtil.FindChildGameObject(base.gameObject, "btn_page");
		this.m_panel = base.GetComponent<UIPanel>();
		if (this.m_panel != null)
		{
			this.m_panel.alpha = 0f;
		}
		this.m_scrollViewPanel = GameObjectUtil.FindChildGameObjectComponent<UIPanel>(base.gameObject, "ScrollView");
		if (this.m_scrollViewPanel != null)
		{
			this.m_scrollViewPanel.alpha = 0f;
		}
	}

	public IEnumerator SetUp()
	{
		RankingFriendOptionWindow._SetUp_c__Iterator51 _SetUp_c__Iterator = new RankingFriendOptionWindow._SetUp_c__Iterator51();
		_SetUp_c__Iterator.__f__this = this;
		return _SetUp_c__Iterator;
	}

	private void Update()
	{
		if (this.m_draggablePanel != null && this.m_isAnimationEnd)
		{
			for (int i = 0; i < this.VISIBLE_IMAGE_MAX; i++)
			{
				float num = (float)(this.m_loadedFriendImageNum - this.VISIBLE_IMAGE_MAX) / (float)this.m_activeScrollCount;
				if (this.m_draggablePanel.verticalScrollBar.value <= num)
				{
					return;
				}
				this.NextImageLoad();
			}
		}
	}

	private void NextImageLoad()
	{
		for (int i = 0; i < this.LOAD_FRIEND_IMAGE_NUM; i++)
		{
			if (this.m_loadedFriendImageNum < this.m_simpleScrolls.Count)
			{
				if (!this.m_simpleScrolls[this.m_loadedFriendImageNum].gameObject.activeSelf)
				{
					return;
				}
				this.m_simpleScrolls[this.m_loadedFriendImageNum].LoadImage();
				this.m_loadedFriendImageNum++;
			}
		}
	}

	private void OnFinishedActiveAnimation()
	{
		this.m_isAnimationEnd = true;
	}

	private void LabelUpdate()
	{
		this.m_uiLabels[3].text = 1 + this.m_page + "/" + (1 + this.m_pageMax);
		this.m_uiLabels[2].text = this.m_chosenFriend.Count + "/" + this.CHOSE_FRIEND_MAX;
	}

	public void ScrollListUpdate()
	{
		this.m_loadedFriendImageNum = 0;
		this.m_activeScrollCount = 0;
		List<SocialUserData> list = (!this.m_showConfirmWindow) ? this.m_friendList : this.m_confirmList;
		int num = this.m_page * this.m_simpleScrolls.Count;
		foreach (RankingSimpleScroll current in this.m_simpleScrolls)
		{
			if (list.Count > num)
			{
				current.gameObject.SetActive(true);
				current.SetUserData(list[num]);
				current.m_toggle.value = false;
				foreach (SocialUserData current2 in this.m_chosenFriend)
				{
					if (current2.Id == list[num].Id)
					{
						current.m_toggle.value = true;
						break;
					}
				}
				this.m_activeScrollCount++;
			}
			else
			{
				current.gameObject.SetActive(false);
			}
			num++;
		}
		this.m_draggablePanel.ResetPosition();
		this.LabelUpdate();
	}

	private void SetUpConfirmWindow()
	{
		this.m_showConfirmWindow = true;
		this.m_page = 0;
		this.m_confirmList = new List<SocialUserData>(this.m_chosenFriend);
		this.Sort();
		this.m_uiLabels[0].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_confirmation_caption").text;
		this.m_uiLabels[1].text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_friend_select_confirmation_body").text;
		this.m_buttonPage.SetActive(false);
		if (GetComponent<Animation>() != null)
		{
			this.m_isAnimationEnd = false;
			this.m_activeAnim = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Forward);
			EventDelegate.Add(this.m_activeAnim.onFinished, new EventDelegate.Callback(this.OnFinishedActiveAnimation), true);
		}
	}

	public void ChooseFriend(SocialUserData user, UIToggle toggle)
	{
		if (user != null)
		{
			if (toggle.value)
			{
				if (this.CHOSE_FRIEND_MAX > this.m_chosenFriend.Count)
				{
					SoundManager.SePlay("sys_menu_decide", "SE");
					this.m_chosenFriend.Add(user);
				}
				else
				{
					toggle.value = false;
				}
			}
			else
			{
				SoundManager.SePlay("sys_window_close", "SE");
				this.m_chosenFriend.RemoveAll((SocialUserData chooseFriend) => chooseFriend.Id == user.Id);
			}
		}
		this.LabelUpdate();
		base.StartCoroutine("UpdateDraggablePanel");
	}

	public IEnumerator UpdateDraggablePanel()
	{
		RankingFriendOptionWindow._UpdateDraggablePanel_c__Iterator52 _UpdateDraggablePanel_c__Iterator = new RankingFriendOptionWindow._UpdateDraggablePanel_c__Iterator52();
		_UpdateDraggablePanel_c__Iterator.__f__this = this;
		return _UpdateDraggablePanel_c__Iterator;
	}

	public void PageUp()
	{
		if (this.m_page > 0)
		{
			this.m_page--;
			this.ScrollListUpdate();
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public void PageDown()
	{
		if (this.m_page < this.m_pageMax)
		{
			this.m_page++;
			this.ScrollListUpdate();
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public void OnClickOkButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (!this.m_showConfirmWindow)
		{
			this.SetUpConfirmWindow();
			return;
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.FriendList = new List<SocialUserData>(this.m_chosenFriend);
			FacebookUtil.SaveFriendIdList(new List<SocialUserData>(this.m_chosenFriend));
		}
		this.m_showConfirmWindow = false;
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			SingletonGameObject<RankingManager>.Instance.Reset(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingRankerType.FRIEND);
			SingletonGameObject<RankingManager>.Instance.Reset(RankingUtil.RankingMode.QUICK, RankingUtil.RankingRankerType.FRIEND);
			if (EventManager.Instance.Type != EventManager.EventType.SPECIAL_STAGE || (SpecialStageWindow.Instance != null && !SpecialStageWindow.Instance.enabledAnchorObjects))
			{
				RankingUI rankingUI = GameObjectUtil.FindGameObjectComponent<RankingUI>("ui_mm_ranking_page(Clone)");
				if (rankingUI != null)
				{
					rankingUI.SendMessage("OnClickFriendOptionOk");
				}
			}
		}
	}

	public void OnClickNoButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public void OnFinishedAnimationCallback()
	{
		this.RemoveBackKeyCallBack();
		base.gameObject.SetActive(false);
	}

	public void OnPressSortName()
	{
		RankingFriendOptionWindow.SortOrder sortOrder = this.m_sortOrder;
		if (sortOrder != RankingFriendOptionWindow.SortOrder.Ascending)
		{
			if (sortOrder == RankingFriendOptionWindow.SortOrder.Descending)
			{
				this.m_sortOrder = RankingFriendOptionWindow.SortOrder.Ascending;
			}
		}
		else
		{
			this.m_sortOrder = RankingFriendOptionWindow.SortOrder.Descending;
		}
		this.Sort();
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	public void Sort()
	{
		this.m_uiLabels[4].text = this.m_sortOrderText[(int)this.m_sortOrder];
		List<SocialUserData> list = (!this.m_showConfirmWindow) ? this.m_friendList : this.m_confirmList;
		list.Sort(delegate(SocialUserData a, SocialUserData b)
		{
			RankingFriendOptionWindow.SortOrder sortOrder = this.m_sortOrder;
			if (sortOrder == RankingFriendOptionWindow.SortOrder.Ascending)
			{
				return string.Compare(a.Name, b.Name, true);
			}
			if (sortOrder != RankingFriendOptionWindow.SortOrder.Descending)
			{
				return string.Compare(a.Name, b.Name, true);
			}
			return string.Compare(b.Name, a.Name, true);
		});
		this.ScrollListUpdate();
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
