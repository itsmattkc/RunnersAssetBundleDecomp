using AnimationOrTween;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopWindowRsringUI : MonoBehaviour
{
	public delegate void PurchaseEndCallback(bool isSuccess);

	private sealed class _PurchaseCallbackEmulateCoroutine_c__Iterator57 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int recultCode;

		internal int _PC;

		internal object _current;

		internal int ___recultCode;

		internal ShopWindowRsringUI __f__this;

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
				this._current = new WaitForSeconds(1f);
				this._PC = 1;
				return true;
			case 1u:
				switch (this.recultCode)
				{
				case 0:
					if (ServerInterface.LoggedInServerInterface == null)
					{
						SaveDataManager.Instance.ItemData.RedRingCount += (uint)this.__f__this.m_exchangeItem.quantity;
					}
					this.__f__this.PurchaseSuccessCallback(0);
					break;
				case 1:
					this.__f__this.PurchaseFailedCallback(NativeObserver.FailStatus.Deferred);
					break;
				case 2:
					this.__f__this.PurchaseCanceledCallback();
					break;
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

	[SerializeField]
	private GameObject[] m_itemsGameObject = new GameObject[5];

	[SerializeField]
	private GameObject m_panelGameObject;

	[SerializeField]
	public UILabel m_quantityLabel;

	[SerializeField]
	public UILabel m_costLabel;

	[SerializeField]
	public GameObject m_bonusGameObject;

	[SerializeField]
	public UILabel[] m_bonusLabels = new UILabel[2];

	[SerializeField]
	public UISprite m_saleSprite;

	[SerializeField]
	private GameObject m_blinderGameObject;

	private int m_productIndex;

	private ShopUI.EexchangeItem m_exchangeItem;

	private AgeVerification m_ageVerification;

	private GameObject m_windowObject;

	private ShopWindowRsringUI.PurchaseEndCallback m_callback;

	[SerializeField]
	public UILabel m_presentLabel;

	private void Start()
	{
		if (this.m_blinderGameObject == null)
		{
			this.m_blinderGameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "blinder");
		}
		this.m_windowObject = GameObjectUtil.FindChildGameObject(base.gameObject, "window");
	}

	private void Update()
	{
		this.UpdateView();
		if (this.m_ageVerification != null)
		{
			if (this.m_ageVerification.IsEnd)
			{
				this.m_ageVerification = null;
				this.OpenWindow(this.m_productIndex, this.m_exchangeItem, this.m_callback);
			}
		}
		else if (GeneralWindow.IsCreated("RsringCountError") || GeneralWindow.IsCreated("PurchaseFailed") || GeneralWindow.IsCreated("PurchaseCanceled") || GeneralWindow.IsCreated("AgeVerificationError"))
		{
			if (GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
			}
		}
		else
		{
			ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow != null && itemGetWindow.IsCreated("PurchaseSuccess") && itemGetWindow.IsEnd)
			{
				itemGetWindow.Reset();
			}
		}
	}

	public void OpenWindow(int productIndex, ShopUI.EexchangeItem exchangeItem, ShopWindowRsringUI.PurchaseEndCallback callback)
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		this.m_productIndex = productIndex;
		this.m_exchangeItem = exchangeItem;
		this.m_callback = callback;
		if (this.m_windowObject != null)
		{
			this.m_windowObject.SetActive(true);
		}
		SoundManager.SePlay("sys_window_open", "SE");
		this.UpdateView();
		Animation animation = GameObjectUtil.FindGameObjectComponent<Animation>("shop_rsring_window");
		if (animation != null)
		{
			ActiveAnimation.Play(animation, "ui_shop_window_Anim", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
		}
	}

	private void OnClickClose()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public void OnFinishedCloseAnim()
	{
		for (int i = 0; i < this.m_itemsGameObject.Length; i++)
		{
			this.m_itemsGameObject[i].SetActive(false);
		}
		this.m_panelGameObject.SetActive(false);
		if (this.m_windowObject != null)
		{
			this.m_windowObject.SetActive(false);
		}
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	private void UpdateView()
	{
		if (this.m_exchangeItem == null)
		{
			return;
		}
		for (int i = 0; i < this.m_itemsGameObject.Length; i++)
		{
			this.m_itemsGameObject[i].SetActive(i == this.m_productIndex);
		}
		this.m_panelGameObject.SetActive(true);
		this.m_quantityLabel.text = this.m_exchangeItem.m_quantityLabel.text;
		this.m_costLabel.text = this.m_exchangeItem.m_costLabel.text;
		this.m_bonusGameObject.SetActive(this.m_exchangeItem.m_bonusGameObject != null && this.m_exchangeItem.m_bonusGameObject.activeSelf);
		if (this.m_exchangeItem.m_bonusGameObject != null)
		{
			for (int j = 0; j < this.m_bonusLabels.Length; j++)
			{
				this.m_bonusLabels[j].text = this.m_exchangeItem.m_bonusLabels[j].text;
			}
			this.m_presentLabel.text = this.m_exchangeItem.m_bonusLabels[0].text;
		}
		this.m_saleSprite.gameObject.SetActive(this.m_exchangeItem.m_saleSprite.gameObject.activeSelf);
		GameObject gameObject = ShopUI.CalcSaleIconObject(this.m_exchangeItem.m_saleSprite.gameObject, this.m_productIndex);
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_saleSprite.gameObject, "img_sale_icon");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(gameObject.activeSelf);
			}
		}
	}

	private void OnClickOk()
	{
		if ((ulong)(9999999u - SaveDataManager.Instance.ItemData.RedRingCount) < (ulong)((long)this.m_exchangeItem.quantity))
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RsringCountError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = ShopUI.GetText("gw_rsring_count_error_caption", null),
				message = ShopUI.GetText("gw_rsring_count_error_text", null),
				isPlayErrorSe = true
			});
		}
		else if (!this.CheckPurchaseOver(HudUtility.GetMixedStringToInt(this.m_exchangeItem.m_costLabel.text)))
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_blinderGameObject.SetActive(true);
			NativeObserver instance = NativeObserver.Instance;
			bool flag = ServerInterface.LoggedInServerInterface != null;
			if (flag && instance != null)
			{
				instance.BuyProduct(instance.GetProductName(this.m_productIndex), new NativeObserver.PurchaseSuccessCallback(this.PurchaseSuccessCallback), new NativeObserver.PurchaseFailedCallback(this.PurchaseFailedCallback), new Action(this.PurchaseCanceledCallback));
			}
			else
			{
				base.StartCoroutine(this.PurchaseCallbackEmulateCoroutine(UnityEngine.Random.Range(0, 3)));
			}
		}
	}

	private IEnumerator PurchaseCallbackEmulateCoroutine(int recultCode)
	{
		ShopWindowRsringUI._PurchaseCallbackEmulateCoroutine_c__Iterator57 _PurchaseCallbackEmulateCoroutine_c__Iterator = new ShopWindowRsringUI._PurchaseCallbackEmulateCoroutine_c__Iterator57();
		_PurchaseCallbackEmulateCoroutine_c__Iterator.recultCode = recultCode;
		_PurchaseCallbackEmulateCoroutine_c__Iterator.___recultCode = recultCode;
		_PurchaseCallbackEmulateCoroutine_c__Iterator.__f__this = this;
		return _PurchaseCallbackEmulateCoroutine_c__Iterator;
	}

	private bool CheckPurchaseOver(int addPurchase)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		ServerSettingState serverSettingState;
		if (loggedInServerInterface != null)
		{
			serverSettingState = ServerInterface.SettingState;
		}
		else
		{
			serverSettingState = new ServerSettingState();
			serverSettingState.m_monthPurchase = 1000;
			serverSettingState.m_birthday = string.Empty;
			serverSettingState.m_birthday = "1990-9-30";
		}
		if (string.IsNullOrEmpty(serverSettingState.m_birthday))
		{
			this.m_ageVerification = base.gameObject.GetComponent<AgeVerification>();
			if (this.m_ageVerification == null)
			{
				this.m_ageVerification = base.gameObject.AddComponent<AgeVerification>();
			}
			this.m_ageVerification.Setup("Camera/menu_Anim/ShopPage/" + base.gameObject.name + "/Anchor_5_MC");
			this.m_ageVerification.PlayStart();
			return true;
		}
		bool flag = HudUtility.CheckPurchaseOver(serverSettingState.m_birthday, serverSettingState.m_monthPurchase, addPurchase);
		if (flag)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "AgeVerificationError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = ShopUI.GetText("gw_age_verification_error_caption", null),
				message = ShopUI.GetText("gw_age_verification_error_text", new Dictionary<string, string>
				{
					{
						"{PURCHASE}",
						HudUtility.GetFormatNumString<int>(serverSettingState.m_monthPurchase)
					}
				}),
				isPlayErrorSe = true
			});
			return true;
		}
		return false;
	}

	private void PurchaseSuccessCallback(int scValue)
	{
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
		if (itemGetWindow != null)
		{
			itemGetWindow.Create(new ItemGetWindow.CInfo
			{
				name = "PurchaseSuccess",
				caption = ShopUI.GetText("gw_purchase_success_caption", null),
				serverItemId = this.m_exchangeItem.m_storeItem.m_itemId,
				imageCount = ShopUI.GetText("gw_purchase_success_text", new Dictionary<string, string>
				{
					{
						"{COUNT}",
						HudUtility.GetFormatNumString<int>(this.m_exchangeItem.quantity)
					}
				})
			});
		}
		SoundManager.SePlay("sys_buy_real_money", "SE");
		if (this.m_exchangeItem != null && this.m_exchangeItem.m_storeItem != null)
		{
			int price = this.m_exchangeItem.m_storeItem.m_price;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetRedStarExchangeList(0, base.gameObject);
		}
		else
		{
			this.ServerGetRedStarExchangeList_Succeeded(null);
		}
	}

	private void PurchaseFailedCallback(NativeObserver.FailStatus status)
	{
		string cellName = "gw_purchase_failed_caption";
		string cellName2 = "gw_purchase_failed_text";
		if (status == NativeObserver.FailStatus.Deferred)
		{
			cellName = "gw_purchase_deferred_caption";
			cellName2 = "gw_purchase_deferred_text";
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "PurchaseFailed",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = ShopUI.GetText(cellName, null),
			message = ShopUI.GetText(cellName2, null),
			isPlayErrorSe = true
		});
		this.m_blinderGameObject.SetActive(false);
		if (this.m_callback != null)
		{
			this.m_callback(false);
			this.m_callback = null;
		}
	}

	private void PurchaseCanceledCallback()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "PurchaseCanceled",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = ShopUI.GetText("gw_purchase_canceled_caption", null),
			message = ShopUI.GetText("gw_purchase_canceled_text", null),
			isPlayErrorSe = true
		});
		this.m_blinderGameObject.SetActive(false);
		if (this.m_callback != null)
		{
			this.m_callback(false);
			this.m_callback = null;
		}
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_windowObject == null)
		{
			return;
		}
		if (!this.m_windowObject.activeSelf)
		{
			return;
		}
		msg.StaySequence();
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_close");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.SendMessage("OnClick");
		}
	}

	private void ServerGetRedStarExchangeList_Succeeded(MsgGetRedStarExchangeListSucceed msg)
	{
		this.m_blinderGameObject.SetActive(false);
		if (this.m_callback != null)
		{
			this.m_callback(true);
			this.m_callback = null;
		}
	}
}
