using AnimationOrTween;
using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UI;
using UnityEngine;

public class ConnectAlertMaskUI : MonoBehaviour
{
	public enum dispCategory
	{
		CHAO_INFO,
		TIPS_INFO,
		END
	}

	[SerializeField]
	private GameObject m_alertGameObject;

	[SerializeField]
	private GameObject m_eventObject;

	[SerializeField]
	private Animation m_screenAnimation;

	[SerializeField]
	private UISprite m_eventLogImg;

	private static ConnectAlertMaskUI s_instance;

	private GameObject m_chaoObject;

	private GameObject m_tipsObject;

	private UILabel m_lblName;

	private UILabel m_lblBonus;

	private UISprite m_imgBg;

	private UISprite m_imgIcon;

	private UITexture m_imgChao;

	private int m_chaoId = -1;

	private List<UIAtlas> m_atlas = new List<UIAtlas>();

	private Action m_onFinishedFadeOutCallbackAction;

	private void Awake()
	{
		ConnectAlertMaskUI.s_instance = this;
		this.CheckChaoObject();
	}

	private void OnDestroy()
	{
		if (ConnectAlertMaskUI.s_instance != null)
		{
			ConnectAlertMaskUI.s_instance.RemoveAtlasList();
			ConnectAlertMaskUI.s_instance.DeleteTexture();
		}
		ConnectAlertMaskUI.s_instance = null;
	}

	public void SetChaoInfo()
	{
		this.CheckChaoObject();
		if (this.m_chaoObject != null)
		{
			if (this.m_tipsObject != null)
			{
				this.m_tipsObject.SetActive(false);
			}
			this.SetChaoInfoData();
		}
	}

	private void CheckChaoObject()
	{
		if (this.m_chaoObject == null)
		{
			this.m_chaoObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_chao");
			if (this.m_chaoObject != null)
			{
				this.m_lblName = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_chaoObject, "Lbl_chao_name");
				this.m_lblBonus = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_chaoObject, "Lbl_chao_bonus");
				this.m_imgBg = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_chaoObject, "img_chao_rank_bg");
				this.m_imgIcon = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_chaoObject, "img_chao_type");
				this.m_imgChao = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_chaoObject, "img_chao");
			}
		}
	}

	private bool SetChaoInfoData()
	{
		int loadingChaoId = ChaoTextureManager.Instance.LoadingChaoId;
		ChaoData chaoData = ChaoTable.GetChaoData(loadingChaoId);
		global::Debug.Log("ConnectAlertMaskUI:SetChaoInfoData  DisplayChaoID = " + loadingChaoId.ToString());
		if (chaoData != null)
		{
			this.SetEventBanner();
			if (this.m_chaoObject != null)
			{
				this.m_chaoObject.SetActive(true);
			}
			this.m_lblName.text = chaoData.name;
			int chaoLevel = ChaoTable.ChaoMaxLevel();
			this.m_lblBonus.text = chaoData.GetLoadingPageDetailLevelPlusSP(chaoLevel);
			if (!string.IsNullOrEmpty(this.m_lblBonus.text))
			{
				UILabel expr_95 = this.m_lblBonus;
				expr_95.text = expr_95.text + "\n" + TextUtility.GetChaoText("Chao", "level_max");
			}
			switch (chaoData.rarity)
			{
			case ChaoData.Rarity.NORMAL:
				this.m_imgBg.spriteName = "ui_chao_set_bg_load_0";
				break;
			case ChaoData.Rarity.RARE:
				this.m_imgBg.spriteName = "ui_chao_set_bg_load_1";
				break;
			case ChaoData.Rarity.SRARE:
				this.m_imgBg.spriteName = "ui_chao_set_bg_load_2";
				break;
			}
			switch (chaoData.charaAtribute)
			{
			case CharacterAttribute.SPEED:
				this.m_imgIcon.spriteName = "ui_chao_set_type_icon_speed";
				break;
			case CharacterAttribute.FLY:
				this.m_imgIcon.spriteName = "ui_chao_set_type_icon_fly";
				break;
			case CharacterAttribute.POWER:
				this.m_imgIcon.spriteName = "ui_chao_set_type_icon_power";
				break;
			}
			if (this.m_imgChao != null)
			{
				this.m_chaoId = ChaoWindowUtility.GetIdFromServerId(chaoData.id + 400000);
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(this.m_imgChao, null, true);
				ChaoTextureManager.Instance.GetTexture(this.m_chaoId, info);
				this.m_imgChao.enabled = true;
			}
			return true;
		}
		global::Debug.Log("ConnectAlertMaskUI:SetChaoInfoData  ChaoInfoData is Null!!");
		if (this.m_chaoObject != null)
		{
			this.m_chaoObject.SetActive(false);
		}
		return false;
	}

	public void SetTipsInfo()
	{
		this.CheckTipsObject();
		if (this.m_tipsObject != null)
		{
			if (this.m_chaoObject != null)
			{
				this.m_chaoObject.SetActive(false);
			}
			this.SetTipsInfoData();
		}
	}

	private void CheckTipsObject()
	{
		this.m_tipsObject = GameObjectUtil.FindChildGameObject(base.gameObject, "info_tips");
	}

	private void SetTipsInfoData()
	{
		this.SetEventBanner();
		this.m_tipsObject.SetActive(true);
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_tipsObject, "Lbl_tips_body");
		if (uILabel != null)
		{
			int categoryCellCount = TextManager.GetCategoryCellCount(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tips");
			string text = string.Empty;
			if (categoryCellCount > 0)
			{
				int num = UnityEngine.Random.Range(1, categoryCellCount);
				string cellID = "tips_message_" + num;
				text = TextUtility.GetCommonText("Tips", cellID);
			}
			if (text != null)
			{
				uILabel.text = text;
			}
		}
	}

	public void SetTipsCategory(ConnectAlertMaskUI.dispCategory category)
	{
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_tips_category");
		if (uILabel != null)
		{
			if (category == ConnectAlertMaskUI.dispCategory.CHAO_INFO)
			{
				uILabel.text = TextUtility.GetCommonText("MainMenu", "loading_chaoInfo_caption");
				this.SetChaoInfo();
			}
			else
			{
				uILabel.text = TextUtility.GetCommonText("MainMenu", "loading_tipsInfo_caption");
				this.SetTipsInfo();
			}
		}
	}

	public void PlayReverse()
	{
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_screenAnimation, Direction.Reverse);
		if (activeAnimation != null)
		{
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimation), true);
		}
	}

	private void SetEventBanner()
	{
		bool eventObject = false;
		if (EventManager.Instance != null && EventManager.Instance.Type != EventManager.EventType.UNKNOWN)
		{
			eventObject = EventManager.Instance.IsInEvent();
			if (this.m_eventObject != null && EventManager.Instance.Type == EventManager.EventType.ADVERT)
			{
				string advertEventTitleText = this.GetAdvertEventTitleText();
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_eventObject, "ui_Lbl_event_caption");
				if (uILabel != null)
				{
					uILabel.text = advertEventTitleText;
				}
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_eventObject, "ui_Lbl_event_caption_sh");
				if (uILabel2 != null)
				{
					uILabel2.text = advertEventTitleText;
				}
				UILocalizeText uILocalizeText = GameObjectUtil.FindChildGameObjectComponent<UILocalizeText>(this.m_eventObject, "ui_Lbl_event_caption");
				if (uILocalizeText != null)
				{
					uILocalizeText.enabled = false;
				}
				UILocalizeText uILocalizeText2 = GameObjectUtil.FindChildGameObjectComponent<UILocalizeText>(this.m_eventObject, "ui_Lbl_event_caption_sh");
				if (uILocalizeText2 != null)
				{
					uILocalizeText2.enabled = false;
				}
			}
		}
		this.SetEventObject(eventObject);
	}

	private string GetAdvertEventTitleText()
	{
		string result = string.Empty;
		if (EventManager.Instance != null)
		{
			EventManager.AdvertEventType advertType = EventManager.Instance.AdvertType;
			if (advertType != EventManager.AdvertEventType.UNKNOWN)
			{
				switch (advertType)
				{
				case EventManager.AdvertEventType.ROULETTE:
				{
					EyeCatcherCharaData[] eyeCatcherCharaDatas = EventManager.Instance.GetEyeCatcherCharaDatas();
					if (eyeCatcherCharaDatas != null)
					{
						bool flag = false;
						EyeCatcherCharaData[] array = eyeCatcherCharaDatas;
						for (int i = 0; i < array.Length; i++)
						{
							EyeCatcherCharaData eyeCatcherCharaData = array[i];
							if (eyeCatcherCharaData.id >= -1 && eyeCatcherCharaData.id != 0)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "ui_Lbl_word_header_event_roulette_c");
						}
						else
						{
							result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "ui_Lbl_word_header_event_roulette_o");
						}
					}
					else
					{
						result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "ui_Lbl_word_header_event_roulette_o");
					}
					break;
				}
				case EventManager.AdvertEventType.CHARACTER:
					result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "ui_Lbl_word_header_event_character");
					break;
				case EventManager.AdvertEventType.SHOP:
					result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Common", "ui_Lbl_word_header_event_shop");
					break;
				}
			}
		}
		return result;
	}

	public void SetEventObject(bool enabled)
	{
		if (this.m_eventObject != null)
		{
			if (enabled && AtlasManager.Instance != null)
			{
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_eventObject, "bg_deco");
				if (uISprite != null)
				{
					this.m_atlas.Add(uISprite.atlas);
				}
				if (this.m_eventLogImg != null)
				{
					this.m_atlas.Add(this.m_eventLogImg.atlas);
				}
				AtlasManager.Instance.ReplaceAtlasForMenuLoading(this.m_atlas.ToArray());
			}
			this.m_eventObject.SetActive(enabled);
		}
	}

	private void OnFinishedAnimation()
	{
		if (this.m_onFinishedFadeOutCallbackAction != null)
		{
			this.m_onFinishedFadeOutCallbackAction();
		}
		this.DeleteTexture();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (this.m_chaoId == data.chao_id && this.m_imgChao != null)
		{
			this.m_imgChao.enabled = true;
			this.m_imgChao.mainTexture = data.tex;
		}
	}

	private void RemoveAtlasList()
	{
		if (this.m_eventObject != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_eventObject, "bg_deco");
			if (uISprite != null)
			{
				uISprite.atlas = null;
			}
		}
		if (this.m_eventLogImg != null)
		{
			this.m_eventLogImg.atlas = null;
		}
		this.m_atlas.Clear();
	}

	private void DeleteTexture()
	{
		if (this.m_imgChao != null && this.m_imgChao.mainTexture != null)
		{
			this.m_imgChao.mainTexture = null;
		}
		if (this.m_eventLogImg != null)
		{
		}
		if (this.m_chaoId > 0)
		{
			ChaoTextureManager.Instance.RemoveChaoTexture(this.m_chaoId);
		}
		this.m_chaoId = -1;
	}

	public static void StartScreen()
	{
		if (ConnectAlertMaskUI.s_instance != null)
		{
			ConnectAlertMaskUI.s_instance.m_alertGameObject.SetActive(true);
			ConnectAlertMaskUI.dispCategory dispCategory = ConnectAlertMaskUI.dispCategory.CHAO_INFO;
			ConnectAlertMaskUI.s_instance.SetTipsCategory(dispCategory);
			global::Debug.Log("ConnectAlertMaskUI:StartScreen  dispCategory = " + dispCategory.ToString());
		}
	}

	public static void EndScreen(Action onFinishedFadeOutCallbackAction = null)
	{
		if (ConnectAlertMaskUI.s_instance != null)
		{
			ConnectAlertMaskUI.s_instance.SetEventObject(false);
			ConnectAlertMaskUI.s_instance.m_onFinishedFadeOutCallbackAction = onFinishedFadeOutCallbackAction;
			ConnectAlertMaskUI.s_instance.PlayReverse();
		}
	}
}
