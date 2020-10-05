using AnimationOrTween;
using DataTable;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class HudContinueBuyRsRing : MonoBehaviour
{
	private enum State
	{
		IDLE,
		WAIT_CLICK_BUTTON,
		PURCHASING,
		SHOW_RESULT
	}

	private enum Result
	{
		NONE = -1,
		SUCCESS,
		FAILED,
		CANCELED
	}

	private sealed class _OpenLegalWindow_c__Iterator24 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string _url___0;

		internal GameObject _htmlParserGameObject___1;

		internal HtmlParser _htmlParser___2;

		internal string _legalText___3;

		internal int _PC;

		internal object _current;

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
				SoundManager.SePlay("sys_menu_decide", "SE");
				this._url___0 = NetUtil.GetWebPageURL(InformationDataTable.Type.SHOP_LEGAL);
				this._htmlParserGameObject___1 = HtmlParserFactory.Create(this._url___0, HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
				if (this._htmlParserGameObject___1 == null)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				break;
			case 2u:
				goto IL_B1;
			case 3u:
				goto IL_DA;
			default:
				return false;
			}
			this._htmlParser___2 = this._htmlParserGameObject___1.GetComponent<HtmlParser>();
			if (this._htmlParser___2 == null)
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			IL_B1:
			if (!(this._htmlParser___2 != null))
			{
				goto IL_15E;
			}
			IL_DA:
			if (!this._htmlParser___2.IsEndParse)
			{
				this._current = null;
				this._PC = 3;
				return true;
			}
			this._legalText___3 = this._htmlParser___2.ParsedString;
			UnityEngine.Object.Destroy(this._htmlParserGameObject___1);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RsrBuyLegal",
				anchor_path = "Camera/Anchor_5_MC",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "Title", "gw_legal_caption").text,
				message = this._legalText___3
			});
			IL_15E:
			this._PC = -1;
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

	private bool m_isEndPlay;

	private HudContinueBuyRsRing.State m_state;

	private HudContinueBuyRsRing.Result m_result = HudContinueBuyRsRing.Result.NONE;

	private static readonly int ProductIndex;

	public ServerCampaignData m_campaignData;

	private List<Constants.Campaign.emType> CampaignTypeList = new List<Constants.Campaign.emType>
	{
		Constants.Campaign.emType.PurchaseAddRsrings,
		Constants.Campaign.emType.PurchaseAddRsringsNoChargeUser
	};

	public bool IsEndPlay
	{
		get
		{
			return this.m_isEndPlay;
		}
		private set
		{
		}
	}

	public bool IsSuccess
	{
		get
		{
			return this.m_result == HudContinueBuyRsRing.Result.SUCCESS;
		}
	}

	public bool IsFailed
	{
		get
		{
			return this.m_result == HudContinueBuyRsRing.Result.FAILED;
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_result == HudContinueBuyRsRing.Result.CANCELED;
		}
	}

	public void Setup()
	{
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_item_rs_1");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickBuyButton";
		}
		UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_shop_legal");
		if (uIButtonMessage2 != null)
		{
			uIButtonMessage2.target = base.gameObject;
			uIButtonMessage2.functionName = "OnClickTradeLowButton";
		}
		UIButtonMessage uIButtonMessage3 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_close");
		if (uIButtonMessage3 != null)
		{
			uIButtonMessage3.target = base.gameObject;
			uIButtonMessage3.functionName = "OnClickCloseButton";
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_sale_icon_1");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		NativeObserver instance = NativeObserver.Instance;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (instance != null && loggedInServerInterface != null)
		{
			this.FinishedToGetProductList(NativeObserver.IAPResult.ProductsRequestCompleted);
		}
	}

	public void PlayStart()
	{
		base.gameObject.SetActive(true);
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation.Play(component, Direction.Forward);
		}
		ServerCampaignData serverCampaignData = null;
		int num = 10;
		int num2 = num;
		if (GameObject.Find("ServerInterface") != null && ServerInterface.RedStarItemList != null && ServerInterface.RedStarItemList.Count > 0)
		{
			ServerRedStarItemState serverRedStarItemState = ServerInterface.RedStarItemList[HudContinueBuyRsRing.ProductIndex];
			if (serverRedStarItemState != null)
			{
				foreach (Constants.Campaign.emType current in this.CampaignTypeList)
				{
					int storeItemId = serverRedStarItemState.m_storeItemId;
					serverCampaignData = ServerInterface.CampaignState.GetCampaignInSession(current, storeItemId);
					if (serverCampaignData != null)
					{
						break;
					}
				}
				float num3 = ServerCampaignData.fContentBasis;
				if (serverCampaignData != null)
				{
					num3 = (float)serverCampaignData.iContent;
				}
				num2 = serverRedStarItemState.m_numItem;
				num = (int)((float)num2 * num3 / ServerCampaignData.fContentBasis);
			}
		}
		bool flag = serverCampaignData != null;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_sale_icon_1");
		if (gameObject != null)
		{
			gameObject.SetActive(flag);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "img_bonus_icon_bg_1");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(flag);
		}
		if (flag)
		{
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "img_sale_icon_bg_1");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(true);
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3, "Lbl_rs_gift_1");
				if (uILabel != null)
				{
					int num4 = num - num2;
					TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", "label_rsring_bonus");
					if (text != null)
					{
						text.ReplaceTag("{COUNT}", HudUtility.GetFormatNumString<int>(num4));
						uILabel.text = text.text;
						UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uILabel.gameObject, "Lbl_bonus_icon_sdw_1");
						if (uILabel2 != null)
						{
							uILabel2.text = text.text;
						}
					}
				}
			}
		}
		else
		{
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "img_sale_icon_bg_1");
			if (gameObject4 != null)
			{
				gameObject4.SetActive(false);
			}
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_rs_quantity_1");
		if (uILabel3 != null)
		{
			uILabel3.text = num2.ToString();
		}
		this.m_isEndPlay = false;
		this.m_state = HudContinueBuyRsRing.State.WAIT_CLICK_BUTTON;
		this.m_result = HudContinueBuyRsRing.Result.NONE;
	}

	private void Start()
	{
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case HudContinueBuyRsRing.State.SHOW_RESULT:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
			}
			break;
		}
		if ((GeneralWindow.IsCreated("RsrBuyLegal") || GeneralWindow.IsCreated("AgeVerificationError")) && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
		}
	}

	private void FinishedToGetProductList(NativeObserver.IAPResult result)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "item_1");
		if (gameObject == null)
		{
			return;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_price_1");
		if (uILabel == null)
		{
			return;
		}
		NativeObserver instance = NativeObserver.Instance;
		if (instance == null)
		{
			return;
		}
		string productPrice = instance.GetProductPrice(instance.GetProductName(HudContinueBuyRsRing.ProductIndex));
		if (productPrice == null)
		{
			return;
		}
		uILabel.text = productPrice;
	}

	private void OnClickBuyButton()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerSettingState settingState = ServerInterface.SettingState;
			NativeObserver instance = NativeObserver.Instance;
			string productName = instance.GetProductName(HudContinueBuyRsRing.ProductIndex);
			int mixedStringToInt = HudUtility.GetMixedStringToInt(instance.GetProductPrice(productName));
			if (!HudUtility.CheckPurchaseOver(settingState.m_birthday, settingState.m_monthPurchase, mixedStringToInt))
			{
				instance.BuyProduct(productName, new NativeObserver.PurchaseSuccessCallback(this.PurchaseSuccessCallback), new NativeObserver.PurchaseFailedCallback(this.PurchaseFailedCallback), new Action(this.PurchaseCanceledCallback));
			}
			else
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "AgeVerificationError",
					caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", "gw_age_verification_error_caption").text,
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", "gw_age_verification_error_text").text,
					anchor_path = "Camera/Anchor_5_MC",
					buttonType = GeneralWindow.ButtonType.Ok
				});
			}
		}
	}

	private void OnClickTradeLowButton()
	{
		base.StartCoroutine(this.OpenLegalWindow());
	}

	private IEnumerator OpenLegalWindow()
	{
		return new HudContinueBuyRsRing._OpenLegalWindow_c__Iterator24();
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallbck), true);
			}
		}
	}

	public void OnPushBackKey()
	{
		this.OnClickCloseButton();
	}

	private void PurchaseSuccessCallback(int result)
	{
		SoundManager.SePlay("sys_buy_real_money", "SE");
		global::Debug.Log("HudContinue.PurchaseSuccessCallback");
		string replaceString = "1";
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerRedStarItemState serverRedStarItemState = ServerInterface.RedStarItemList[HudContinueBuyRsRing.ProductIndex];
			if (serverRedStarItemState != null)
			{
				replaceString = serverRedStarItemState.m_numItem.ToString();
			}
		}
		TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", "gw_purchase_success_text");
		if (text != null)
		{
			text.ReplaceTag("{COUNT}", replaceString);
		}
		if (ServerInterface.LoggedInServerInterface != null)
		{
			int storeItemId = ServerInterface.RedStarItemList[HudContinueBuyRsRing.ProductIndex].m_storeItemId;
			global::Debug.Log("HudContinueBuyRsRing.PurchaseSuccessCallback:result = " + storeItemId.ToString());
		}
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetRedStarExchangeList(0, base.gameObject);
		}
		else
		{
			this.ServerGetRedStarExchangeList_Succeeded(null);
		}
	}

	private void ServerGetRedStarExchangeList_Succeeded(MsgGetRedStarExchangeListSucceed msg)
	{
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(GetComponent<Animation>(), Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallbck), true);
			}
		}
		this.m_state = HudContinueBuyRsRing.State.SHOW_RESULT;
		this.m_result = HudContinueBuyRsRing.Result.SUCCESS;
	}

	private void PurchaseFailedCallback(NativeObserver.FailStatus status)
	{
		string cellID = "gw_purchase_failed_caption";
		string cellID2 = "gw_purchase_failed_text";
		if (status == NativeObserver.FailStatus.Deferred)
		{
			cellID = "gw_purchase_deferred_caption";
			cellID2 = "gw_purchase_deferred_text";
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			caption = TextUtility.GetCommonText("Shop", cellID),
			message = TextUtility.GetCommonText("Shop", cellID2),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.Ok,
			isPlayErrorSe = true
		});
		this.m_state = HudContinueBuyRsRing.State.SHOW_RESULT;
		this.m_result = HudContinueBuyRsRing.Result.FAILED;
	}

	private void PurchaseCanceledCallback()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			caption = TextUtility.GetCommonText("Shop", "gw_purchase_canceled_caption"),
			message = TextUtility.GetCommonText("Shop", "gw_purchase_canceled_text"),
			anchor_path = "Camera/Anchor_5_MC",
			buttonType = GeneralWindow.ButtonType.Ok,
			isPlayErrorSe = true
		});
		this.m_state = HudContinueBuyRsRing.State.SHOW_RESULT;
		this.m_result = HudContinueBuyRsRing.Result.CANCELED;
	}

	private void OnFinishedAnimationCallbck()
	{
		base.gameObject.SetActive(false);
		this.m_isEndPlay = true;
		this.m_state = HudContinueBuyRsRing.State.IDLE;
		this.m_result = HudContinueBuyRsRing.Result.CANCELED;
	}
}
