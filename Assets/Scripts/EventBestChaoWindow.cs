using AnimationOrTween;
using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class EventBestChaoWindow : MonoBehaviour
{
	private abstract class ButtonDecorate
	{
		public abstract void Decorate(GameObject gameObject, ServerItem item);

		public abstract void OpenWindow(ServerItem item);
	}

	private class ChaoButtonDecorate : EventBestChaoWindow.ButtonDecorate
	{
		public void Decorate(GameObject gameObject, ServerItem item)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "chao");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
				UIButtonMessage uIButtonMessage = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = gameObject;
				uIButtonMessage.functionName = "OnButtonClickedCallback";
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "chara");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
			ChaoData chaoData = ChaoTable.GetChaoData(item.chaoId);
			if (chaoData == null)
			{
				return;
			}
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_chao_bg");
			if (uISprite != null)
			{
				uISprite.spriteName = "ui_tex_chao_bg_" + ((int)chaoData.rarity).ToString();
			}
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_chao_type_icon");
			if (uISprite2 != null)
			{
				CharacterAttribute charaAtribute = chaoData.charaAtribute;
				string spriteName = "ui_chao_set_type_icon_" + charaAtribute.ToString().ToLower();
				uISprite2.spriteName = spriteName;
			}
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_chao_feature");
			if (uILabel != null)
			{
				string featuredDetail = chaoData.GetFeaturedDetail();
				if (!string.IsNullOrEmpty(featuredDetail))
				{
					uILabel.text = featuredDetail;
				}
			}
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_chao_name");
			if (uILabel2 != null)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", string.Format("name{0:D4}", item.chaoId)).text;
				uILabel2.text = text;
			}
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "texture_chao");
			if (uITexture != null)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
				ChaoTextureManager.Instance.GetTexture(item.chaoId, info);
			}
		}

		public void OpenWindow(ServerItem item)
		{
			ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
			if (window != null)
			{
				ChaoData chaoData = ChaoTable.GetChaoData(item.chaoId);
				if (chaoData != null)
				{
					ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
					chaoInfo.level = ChaoTable.ChaoMaxLevel();
					chaoInfo.detail = chaoData.GetDetailLevelPlusSP(chaoInfo.level);
					if (chaoInfo.level == ChaoTable.ChaoMaxLevel())
					{
						chaoInfo.detail = chaoInfo.detail + "\n" + TextUtility.GetChaoText("Chao", "level_max");
					}
					window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
				}
			}
		}
	}

	private class CharaButtonDecorate : EventBestChaoWindow.ButtonDecorate
	{
		public void Decorate(GameObject gameObject, ServerItem item)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "chara");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
				UIButtonMessage uIButtonMessage = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = gameObject;
				uIButtonMessage.functionName = "OnButtonClickedCallback";
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "chao");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
			CharaType charaType = item.charaType;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_player_genus");
			if (uISprite != null)
			{
				uISprite.spriteName = HudUtility.GetTeamAttributeSpriteName(charaType);
				UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_player_speacies");
				if (uISprite2 != null)
				{
					uISprite2.spriteName = HudUtility.GetCharaAttributeSpriteName(charaType);
				}
			}
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_chara_feature");
			if (uILabel != null)
			{
				string cellName = "featured_footnote_chara" + item.idIndex.ToString("D4");
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", cellName).text;
				uILabel.text = text;
			}
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_chara_name");
			if (uILabel2 != null)
			{
				string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)charaType]).text;
				uILabel2.text = text2;
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject, "sprite_chara");
			if (gameObject4 != null)
			{
				gameObject4.SetActive(false);
			}
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject, "img_player_tex");
			if (uITexture != null)
			{
				TextureRequestChara request = new TextureRequestChara(charaType, uITexture);
				TextureAsyncLoadManager.Instance.Request(request);
			}
		}

		public void OpenWindow(ServerItem item)
		{
			CharaType charaType = item.charaType;
			PlayerSetWindowUI playerSetWindowUI = PlayerSetWindowUI.Create(charaType, null, PlayerSetWindowUI.WINDOW_MODE.DEFAULT);
			if (playerSetWindowUI != null)
			{
				playerSetWindowUI.Setup(charaType, null, PlayerSetWindowUI.WINDOW_MODE.DEFAULT);
			}
		}
	}

	private class ChaoWindowButton : MonoBehaviour
	{
		private ServerItem m_item;

		private EventBestChaoWindow.ButtonDecorate m_decorater;

		public void Setup()
		{
		}

		public void SetItemId(ServerItem item)
		{
			this.m_item = item;
			base.gameObject.SetActive(true);
			ServerItem.IdType idType = item.idType;
			if (idType != ServerItem.IdType.CHARA)
			{
				if (idType == ServerItem.IdType.CHAO)
				{
					this.m_decorater = new EventBestChaoWindow.ChaoButtonDecorate();
				}
			}
			else
			{
				this.m_decorater = new EventBestChaoWindow.CharaButtonDecorate();
			}
			if (this.m_decorater != null)
			{
				this.m_decorater.Decorate(base.gameObject, item);
			}
		}

		private void OnButtonClickedCallback()
		{
			if (this.m_decorater != null)
			{
				this.m_decorater.OpenWindow(this.m_item);
			}
		}

		private void OnSetChaoTexture(ChaoTextureManager.TextureData tex)
		{
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "texture_chao");
			if (uITexture == null)
			{
				return;
			}
			uITexture.gameObject.SetActive(true);
			uITexture.mainTexture = tex.tex;
		}
	}

	private class EventBestChaoPage : MonoBehaviour
	{
		private enum Button
		{
			None = -1,
			Left,
			Center,
			Right,
			Num
		}

		private class ChaoInfo
		{
			public ServerItem item;

			public EventBestChaoWindow.ChaoWindowButton button;
		}

		private static readonly int CHAO_MAX = 2;

		private static readonly string[] ButtonName = new string[]
		{
			"2_1_window",
			"1_1_window",
			"2_2_window"
		};

		private EventBestChaoWindow.ChaoWindowButton[] m_commonButton = new EventBestChaoWindow.ChaoWindowButton[3];

		private UILabel m_pageLabel;

		private bool m_isInSetup;

		private List<EventBestChaoWindow.EventBestChaoPage.ChaoInfo> m_chaos = new List<EventBestChaoWindow.EventBestChaoPage.ChaoInfo>();

		private int m_currentPageNum;

		public void BeginSetup()
		{
			this.m_isInSetup = true;
			for (int i = 0; i < 3; i++)
			{
				if (!(this.m_commonButton[i] != null))
				{
					string name = EventBestChaoWindow.EventBestChaoPage.ButtonName[i];
					GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
					if (!(gameObject == null))
					{
						EventBestChaoWindow.ChaoWindowButton chaoWindowButton = gameObject.AddComponent<EventBestChaoWindow.ChaoWindowButton>();
						chaoWindowButton.Setup();
						this.m_commonButton[i] = chaoWindowButton;
					}
				}
			}
			if (this.m_pageLabel == null)
			{
				this.m_pageLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_page_number");
			}
			if (this.m_chaos != null)
			{
				this.m_chaos.Clear();
			}
		}

		public void AddItem(ServerItem item)
		{
			if (!this.m_isInSetup)
			{
				return;
			}
			EventBestChaoWindow.EventBestChaoPage.ChaoInfo chaoInfo = new EventBestChaoWindow.EventBestChaoPage.ChaoInfo();
			chaoInfo.item = item;
			chaoInfo.button = null;
			this.m_chaos.Add(chaoInfo);
		}

		public void EndSetup()
		{
			int count = this.m_chaos.Count;
			for (int i = 0; i < count - 1; i++)
			{
				if (i % EventBestChaoWindow.EventBestChaoPage.CHAO_MAX == 0)
				{
					EventBestChaoWindow.ChaoWindowButton chaoWindowButton = this.m_commonButton[0];
					if (!(chaoWindowButton == null))
					{
						this.m_chaos[i].button = chaoWindowButton;
					}
				}
				else
				{
					EventBestChaoWindow.ChaoWindowButton chaoWindowButton2 = this.m_commonButton[2];
					if (!(chaoWindowButton2 == null))
					{
						this.m_chaos[i].button = chaoWindowButton2;
					}
				}
			}
			if (count % EventBestChaoWindow.EventBestChaoPage.CHAO_MAX == 0)
			{
				EventBestChaoWindow.ChaoWindowButton chaoWindowButton3 = this.m_commonButton[2];
				if (chaoWindowButton3 == null)
				{
					return;
				}
				this.m_chaos[count - 1].button = chaoWindowButton3;
			}
			else
			{
				EventBestChaoWindow.ChaoWindowButton chaoWindowButton4 = this.m_commonButton[1];
				if (chaoWindowButton4 == null)
				{
					return;
				}
				this.m_chaos[count - 1].button = chaoWindowButton4;
			}
			this.PageChange(this.m_currentPageNum);
			this.m_isInSetup = false;
		}

		public int GetPageCount()
		{
			int count = this.m_chaos.Count;
			int num = count / EventBestChaoWindow.EventBestChaoPage.CHAO_MAX;
			if (count % 2 != 0)
			{
				num++;
			}
			return num;
		}

		public int GetCurrentPageNum()
		{
			return this.m_currentPageNum;
		}

		public void GoNextPage()
		{
			if (this.m_currentPageNum >= this.GetPageCount() - 1)
			{
				return;
			}
			this.m_currentPageNum++;
			this.PageChange(this.m_currentPageNum);
		}

		public void GoPrevPage()
		{
			if (this.m_currentPageNum <= 0)
			{
				return;
			}
			this.m_currentPageNum--;
			this.PageChange(this.m_currentPageNum);
		}

		private void PageChange(int pageIndex)
		{
			EventBestChaoWindow.ChaoWindowButton[] commonButton = this.m_commonButton;
			for (int i = 0; i < commonButton.Length; i++)
			{
				EventBestChaoWindow.ChaoWindowButton chaoWindowButton = commonButton[i];
				if (!(chaoWindowButton == null))
				{
					chaoWindowButton.gameObject.SetActive(false);
				}
			}
			int num = EventBestChaoWindow.EventBestChaoPage.CHAO_MAX * pageIndex;
			int num2 = num + EventBestChaoWindow.EventBestChaoPage.CHAO_MAX;
			int count = this.m_chaos.Count;
			if (num2 > count)
			{
				num2 = count;
			}
			for (int j = num; j < num2; j++)
			{
				EventBestChaoWindow.EventBestChaoPage.ChaoInfo chaoInfo = this.m_chaos[j];
				if (chaoInfo != null)
				{
					ServerItem item = chaoInfo.item;
					EventBestChaoWindow.ChaoWindowButton button = chaoInfo.button;
					if (!(button == null))
					{
						button.SetItemId(item);
					}
				}
			}
			if (this.m_pageLabel != null)
			{
				int num3 = pageIndex + 1;
				int pageCount = this.GetPageCount();
				this.m_pageLabel.text = num3.ToString() + "/" + pageCount.ToString();
			}
		}
	}

	private EventBestChaoWindow.EventBestChaoPage m_chaoPage;

	private bool m_isSetup;

	private static bool m_created;

	public static bool Created
	{
		get
		{
			return EventBestChaoWindow.m_created;
		}
	}

	public static EventBestChaoWindow GetWindow()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			return GameObjectUtil.FindChildGameObjectComponent<EventBestChaoWindow>(gameObject, "EventBestchaoWindow");
		}
		return null;
	}

	public void OpenWindow(List<ServerItem> itemList)
	{
		if (itemList == null)
		{
			return;
		}
		SoundManager.SePlay("sys_window_open", "SE");
		BackKeyManager.AddWindowCallBack(base.gameObject);
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, false);
		}
		EventBestChaoWindow.m_created = true;
		base.gameObject.SetActive(true);
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
				uIButtonMessage.functionName = "CloseButtonClickedCallback";
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_window_pager_L");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage2 = gameObject2.GetComponent<UIButtonMessage>();
				if (uIButtonMessage2 == null)
				{
					uIButtonMessage2 = gameObject2.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "LeftButtonClickedCallback";
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_window_pager_R");
			if (gameObject3 != null)
			{
				UIButtonMessage uIButtonMessage3 = gameObject3.GetComponent<UIButtonMessage>();
				if (uIButtonMessage3 == null)
				{
					uIButtonMessage3 = gameObject3.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage3.target = base.gameObject;
				uIButtonMessage3.functionName = "RightButtonClickedCallback";
			}
			this.m_isSetup = false;
		}
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "bestchao_window");
		if (animation != null)
		{
			ActiveAnimation.Play(animation, Direction.Forward);
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "chao_window");
		if (gameObject4 != null)
		{
			int childCount = gameObject4.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = gameObject4.transform.GetChild(i);
				if (!(child == null))
				{
					GameObject gameObject5 = child.gameObject;
					if (!(gameObject5 == null))
					{
						gameObject5.SetActive(false);
					}
				}
			}
		}
		if (this.m_chaoPage == null)
		{
			this.m_chaoPage = base.gameObject.AddComponent<EventBestChaoWindow.EventBestChaoPage>();
		}
		int count = itemList.Count;
		this.m_chaoPage.BeginSetup();
		for (int j = 0; j < count; j++)
		{
			this.m_chaoPage.AddItem(itemList[j]);
		}
		this.m_chaoPage.EndSetup();
		this.DisplayPageScrollButton();
	}

	public void OpenWindow(List<int> chaoIdList)
	{
		if (chaoIdList == null)
		{
			return;
		}
		List<ServerItem> list = new List<ServerItem>();
		foreach (int current in chaoIdList)
		{
			ServerItem item = ServerItem.CreateFromChaoId(current);
			if (item.chaoId != -1)
			{
				list.Add(item);
			}
		}
		this.OpenWindow(list);
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	private void DisplayPageScrollButton()
	{
		if (this.m_chaoPage == null)
		{
			return;
		}
		int currentPageNum = this.m_chaoPage.GetCurrentPageNum();
		int pageCount = this.m_chaoPage.GetPageCount();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_window_pager_L");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_window_pager_R");
		if (gameObject == null || gameObject2 == null)
		{
			return;
		}
		bool active = true;
		bool active2 = true;
		if (currentPageNum == 0)
		{
			active = false;
		}
		if (currentPageNum == pageCount - 1)
		{
			active2 = false;
		}
		gameObject.SetActive(active);
		gameObject2.SetActive(active2);
	}

	private void CloseButtonClickedCallback()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "bestchao_window");
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OutAnimationFinishCallback), true);
		}
	}

	private void LeftButtonClickedCallback()
	{
		SoundManager.SePlay("sys_page_skip", "SE");
		if (this.m_chaoPage != null)
		{
			this.m_chaoPage.GoPrevPage();
		}
		this.DisplayPageScrollButton();
	}

	private void RightButtonClickedCallback()
	{
		SoundManager.SePlay("sys_page_skip", "SE");
		if (this.m_chaoPage != null)
		{
			this.m_chaoPage.GoNextPage();
		}
		this.DisplayPageScrollButton();
	}

	private void OutAnimationFinishCallback()
	{
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, true);
		}
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
		EventBestChaoWindow.m_created = false;
		base.gameObject.SetActive(false);
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_close");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.SendMessage("OnClick");
		}
	}
}
