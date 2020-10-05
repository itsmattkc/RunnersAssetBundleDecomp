using AnimationOrTween;
using DataTable;
using System;
using Text;
using UnityEngine;

public class ChaoSetWindowUI : MonoBehaviour
{
	public struct ChaoInfo
	{
		public int id;

		public int level;

		public ChaoData.Rarity rarity;

		public CharacterAttribute charaAttribute;

		public string name;

		public string detail;

		public bool onMask;

		public ChaoInfo(ChaoData chaoData)
		{
			this.id = chaoData.id;
			this.level = chaoData.level;
			this.rarity = chaoData.rarity;
			this.charaAttribute = chaoData.charaAtribute;
			this.name = chaoData.name;
			this.detail = chaoData.GetDetailLevelPlusSP(this.level);
			this.onMask = false;
		}
	}

	public enum WindowType
	{
		WITH_BUTTON,
		WINDOW_ONLY
	}

	private static bool m_isActive;

	private ChaoSetWindowUI.ChaoInfo m_chaoInfo;

	[SerializeField]
	private UISprite m_chaoSprite;

	[SerializeField]
	private UITexture m_chaoTexture;

	[SerializeField]
	private UISprite m_chaoRankSprite;

	[SerializeField]
	private UILabel m_chaoNameLabel;

	[SerializeField]
	private UILabel m_chaoLevelLabel;

	[SerializeField]
	private UISprite m_chaoTypeSprite;

	[SerializeField]
	private UILabel m_chaoTypeLabel;

	[SerializeField]
	private UISprite m_bonusTypeSprite;

	[SerializeField]
	private UILabel m_bonusLabel;

	private UIDraggablePanel m_draggablePanel;

	private bool m_tutorial;

	public static bool isActive
	{
		get
		{
			return ChaoSetWindowUI.m_isActive;
		}
	}

	public static ChaoSetWindowUI GetWindow()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			return GameObjectUtil.FindChildGameObjectComponent<ChaoSetWindowUI>(gameObject, "ChaoSetWindowUI");
		}
		return null;
	}

	public void OpenWindow(ChaoSetWindowUI.ChaoInfo chaoInfo, ChaoSetWindowUI.WindowType windowType)
	{
		ChaoSetWindowUI.m_isActive = true;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "chao_set_window");
		if (animation != null)
		{
			animation.gameObject.SetActive(true);
			ActiveAnimation.Play(animation, Direction.Forward);
		}
		SoundManager.SePlay("sys_window_open", "SE");
		this.OnSetChao(chaoInfo);
		string text = string.Empty;
		if (windowType != ChaoSetWindowUI.WindowType.WITH_BUTTON)
		{
			if (windowType == ChaoSetWindowUI.WindowType.WINDOW_ONLY)
			{
				this.OnSetActiveButton(false);
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "ui_Lbl_caption_detail").text;
			}
		}
		else
		{
			this.OnSetActiveButton(true);
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "ui_Lbl_caption").text;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption");
		if (uILabel != null)
		{
			uILabel.text = text;
		}
		this.m_tutorial = ChaoSetUI.IsChaoTutorial();
		if (this.m_tutorial)
		{
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHAOSELECT_MAIN);
		}
	}

	private void Start()
	{
		this.SetAnimationCallBack("Btn_set_main");
		this.SetAnimationCallBack("Btn_set_sub");
		this.SetAnimationCallBack("Btn_window_close");
	}

	private void SetAnimationCallBack(string objName)
	{
		UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(base.gameObject, objName);
		if (uIPlayAnimation != null)
		{
			EventDelegate.Add(uIPlayAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
		}
	}

	private void OnDestroy()
	{
		ChaoSetWindowUI.m_isActive = false;
		if (this.m_chaoTexture != null && this.m_chaoTexture.mainTexture != null)
		{
			this.m_chaoTexture.mainTexture = null;
		}
	}

	private void OnSetChao(ChaoSetWindowUI.ChaoInfo chaoInfo)
	{
		this.m_chaoInfo = chaoInfo;
		ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(this.m_chaoTexture, null, true);
		ChaoTextureManager.Instance.GetTexture(this.m_chaoInfo.id, info);
		this.m_chaoTexture.enabled = true;
		if (this.m_chaoRankSprite != null)
		{
			this.m_chaoRankSprite.spriteName = "ui_chao_set_bg_ll_" + (int)this.m_chaoInfo.rarity;
		}
		if (this.m_chaoNameLabel != null)
		{
			this.m_chaoNameLabel.text = this.m_chaoInfo.name;
		}
		if (this.m_chaoLevelLabel != null)
		{
			this.m_chaoLevelLabel.text = TextUtility.GetTextLevel(this.m_chaoInfo.level.ToString());
		}
		string text = this.m_chaoInfo.charaAttribute.ToString().ToLower();
		if (this.m_chaoTypeSprite != null)
		{
			this.m_chaoTypeSprite.spriteName = "ui_chao_set_type_icon_" + text;
		}
		if (this.m_chaoTypeLabel != null)
		{
			this.m_chaoTypeLabel.text = TextUtility.GetCommonText("CharaAtribute", text);
		}
		if (this.m_chaoTexture != null)
		{
			if (this.m_chaoInfo.onMask)
			{
				this.m_chaoTexture.color = Color.black;
			}
			else
			{
				this.m_chaoTexture.color = Color.white;
			}
		}
		if (this.m_bonusLabel != null)
		{
			this.m_bonusLabel.text = this.m_chaoInfo.detail;
		}
		if (this.m_draggablePanel == null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "ScrollView");
			if (gameObject != null)
			{
				this.m_draggablePanel = gameObject.GetComponent<UIDraggablePanel>();
			}
		}
		if (this.m_draggablePanel != null)
		{
			this.m_draggablePanel.ResetPosition();
		}
	}

	private void OnSetActiveButton(bool isActive)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_set_main");
		if (gameObject != null)
		{
			gameObject.SetActive(isActive);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_set_sub");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(isActive);
		}
	}

	private void OnClickMain()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ChaoSetUI chaoSetUI = GameObjectUtil.FindGameObjectComponent<ChaoSetUI>("ChaoSetUI");
		if (chaoSetUI != null)
		{
			chaoSetUI.RegistChao(0, this.m_chaoInfo.id);
		}
		if (this.m_tutorial)
		{
			this.CreateTutorialWindow();
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.CHAOSELECT_MAIN);
		}
	}

	private void OnClickSub()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ChaoSetUI chaoSetUI = GameObjectUtil.FindGameObjectComponent<ChaoSetUI>("ChaoSetUI");
		if (chaoSetUI != null)
		{
			chaoSetUI.RegistChao(1, this.m_chaoInfo.id);
		}
	}

	private void OnClickClose()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (this.m_chaoInfo.id == data.chao_id && this.m_chaoTexture != null)
		{
			this.m_chaoTexture.mainTexture = data.tex;
			this.m_chaoTexture.enabled = true;
		}
	}

	private void OnFinishedAnimationCallback()
	{
		if (this.m_chaoTexture != null && this.m_chaoTexture.mainTexture != null)
		{
			this.m_chaoTexture.mainTexture = null;
			this.m_chaoTexture.enabled = false;
		}
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
		ChaoSetWindowUI.m_isActive = false;
		this.m_chaoInfo = default(ChaoSetWindowUI.ChaoInfo);
	}

	private void CreateTutorialWindow()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "ChaoTutorial",
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "tutorial_ready_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "tutorial_ready_message").text,
			buttonType = GeneralWindow.ButtonType.Ok
		});
	}
}
