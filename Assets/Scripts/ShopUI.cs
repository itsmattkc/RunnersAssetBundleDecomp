using DataTable;
using NoahUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
	private enum ServerEexchangeType
	{
		RSRING,
		RING,
		CHALLENGE,
		Count
	}

	[Serializable]
	public class EexchangeItem
	{
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
		public UILabel m_saleQuantityLabel;

		[HideInInspector]
		public ServerRedStarItemState m_storeItem;

		[HideInInspector]
		public ServerCampaignData m_campaignData;

		[HideInInspector]
		public bool isCampaign
		{
			get
			{
				return this.m_campaignData != null;
			}
		}

		[HideInInspector]
		public int quantity
		{
			get
			{
				if (this.m_storeItem == null)
				{
					return 0;
				}
				float num = ServerCampaignData.fContentBasis;
				if (this.m_campaignData != null)
				{
					num = (float)this.m_campaignData.iContent;
				}
				return (int)((float)this.m_storeItem.m_numItem * num / ServerCampaignData.fContentBasis);
			}
		}
	}

	[Serializable]
	private class EexchangeType
	{
		[SerializeField]
		public ShopUI.EexchangeItem[] m_exchangeItems = new ShopUI.EexchangeItem[5];
	}

	private sealed class _StartShopCoroutine_c__Iterator55 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string buttonName;

		internal UIToggle _uiToggle___0;

		internal ShopUI.ServerEexchangeType _type___1;

		internal ShopUI.EexchangeType _exchangeType___2;

		internal ShopUI.EexchangeItem[] __s_712___3;

		internal int __s_713___4;

		internal ShopUI.EexchangeItem _exchangeItem___5;

		internal int _PC;

		internal object _current;

		internal string ___buttonName;

		internal ShopUI __f__this;

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
				this._uiToggle___0 = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(this.__f__this.gameObject.transform.root.gameObject, this.buttonName);
				if (this._uiToggle___0 != null)
				{
					this._uiToggle___0.value = true;
				}
				this._type___1 = ShopUI.ServerEexchangeType.RSRING;
				while (this._type___1 < ShopUI.ServerEexchangeType.Count)
				{
					this._exchangeType___2 = this.__f__this.m_exchangeTypes[(int)this._type___1];
					if (this._exchangeType___2 != null)
					{
						this.__s_712___3 = this._exchangeType___2.m_exchangeItems;
						this.__s_713___4 = 0;
						while (this.__s_713___4 < this.__s_712___3.Length)
						{
							this._exchangeItem___5 = this.__s_712___3[this.__s_713___4];
							if (this._exchangeItem___5 != null)
							{
								this._exchangeItem___5.m_quantityLabel.gameObject.SetActive(false);
								this._exchangeItem___5.m_costLabel.gameObject.SetActive(false);
								if (this._exchangeItem___5.m_bonusGameObject != null)
								{
									this._exchangeItem___5.m_bonusGameObject.SetActive(false);
								}
								this._exchangeItem___5.m_saleSprite.gameObject.SetActive(false);
							}
							this.__s_713___4++;
						}
					}
					this._type___1++;
				}
				this.__f__this.OnStartShop();
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

	private sealed class _OpenLegalWindow_c__Iterator56 : IDisposable, IEnumerator, IEnumerator<object>
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
				BackKeyManager.InvalidFlag = true;
				HudMenuUtility.SetConnectAlertMenuButtonUI(true);
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
				goto IL_AD;
			case 3u:
				goto IL_D6;
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
			IL_AD:
			if (!(this._htmlParser___2 != null))
			{
				goto IL_154;
			}
			IL_D6:
			if (!this._htmlParser___2.IsEndParse)
			{
				this._current = null;
				this._PC = 3;
				return true;
			}
			this._legalText___3 = this._htmlParser___2.ParsedString;
			UnityEngine.Object.Destroy(this._htmlParserGameObject___1);
			BackKeyManager.InvalidFlag = false;
			HudMenuUtility.SetConnectAlertMenuButtonUI(false);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RsrBuyLegal",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("Shop", "ui_Lbl_word_legal_caption"),
				message = this._legalText___3
			});
			IL_154:
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

	public const int EXCHANGE_ITEM_PACK_COUNT = 5;

	[SerializeField]
	private ShopUI.EexchangeType[] m_exchangeTypes = new ShopUI.EexchangeType[3];

	[SerializeField]
	private GameObject[] m_exchangeObjects = new GameObject[3];

	[SerializeField]
	public int m_tabOffsetX;

	private GameObject m_freeGetButton;

	private GameObject m_tabRSR;

	private string[] m_redStarPrice = new string[5];

	private bool m_isInitShop;

	private static List<Constants.Campaign.emType>[] s_campaignTypes = new List<Constants.Campaign.emType>[]
	{
		new List<Constants.Campaign.emType>
		{
			Constants.Campaign.emType.PurchaseAddRsrings,
			Constants.Campaign.emType.PurchaseAddRsringsNoChargeUser
		},
		new List<Constants.Campaign.emType>
		{
			Constants.Campaign.emType.PurchaseAddRings
		},
		new List<Constants.Campaign.emType>
		{
			Constants.Campaign.emType.PurchaseAddEnergys
		}
	};

	private bool _isStarted_k__BackingField;

	public bool IsInitShop
	{
		get
		{
			return this.m_isInitShop;
		}
	}

	private bool isStarted
	{
		get;
		set;
	}

	private void SetCurrentType(ShopUI.ServerEexchangeType type)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.m_exchangeObjects[i] != null)
			{
				if (type == (ShopUI.ServerEexchangeType)i)
				{
					this.m_exchangeObjects[i].SetActive(true);
				}
				else
				{
					this.m_exchangeObjects[i].SetActive(false);
				}
			}
		}
	}

	public static GameObject CalcSaleIconObject(GameObject rootObject, int itemIndex)
	{
		if (rootObject == null)
		{
			return null;
		}
		string name = "img_sale_icon_" + (itemIndex + 1).ToString();
		return GameObjectUtil.FindChildGameObject(rootObject, name);
	}

	private void Start()
	{
		this.isStarted = true;
	}

	private void OnStartShopRedStarRing()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.RSRING);
		base.StartCoroutine(this.StartShopCoroutine("Btn_tab_rsring"));
	}

	private void OnStartShopRing()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.RING);
		base.StartCoroutine(this.StartShopCoroutine("Btn_tab_ring"));
	}

	private void OnStartShopChallenge()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.CHALLENGE);
		base.StartCoroutine(this.StartShopCoroutine("Btn_tab_challenge"));
	}

	private void OnStartShopEvent()
	{
		if (ShopUI.isRaidbossEvent())
		{
			base.StartCoroutine(this.StartShopCoroutine("Btn_tab_raidboss"));
		}
		else
		{
			this.OnStartShopChallenge();
		}
	}

	private IEnumerator StartShopCoroutine(string buttonName)
	{
		ShopUI._StartShopCoroutine_c__Iterator55 _StartShopCoroutine_c__Iterator = new ShopUI._StartShopCoroutine_c__Iterator55();
		_StartShopCoroutine_c__Iterator.buttonName = buttonName;
		_StartShopCoroutine_c__Iterator.___buttonName = buttonName;
		_StartShopCoroutine_c__Iterator.__f__this = this;
		return _StartShopCoroutine_c__Iterator;
	}

	private void OnStartShop()
	{
		HudMenuUtility.SendEnableShopButton(false);
		if (this.m_tabRSR == null)
		{
			this.m_tabRSR = GameObjectUtil.FindChildGameObject(base.gameObject.transform.root.gameObject, "tab_rsring");
		}
		if (!ServerInterface.IsRSREnable())
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject.transform.root.gameObject, "Btn_tab_rsring");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			this.SetExchangeItems(ShopUI.ServerEexchangeType.RSRING, ServerInterface.RedStarItemList);
			this.SetExchangeItems(ShopUI.ServerEexchangeType.RING, ServerInterface.RedStarExchangeRingItemList);
			this.SetExchangeItems(ShopUI.ServerEexchangeType.CHALLENGE, ServerInterface.RedStarExchangeEnergyItemList);
		}
		NativeObserver instance = NativeObserver.Instance;
		List<string> list = new List<string>();
		for (int i = 0; i < NativeObserver.ProductCount; i++)
		{
			list.Add(instance.GetProductName(i));
		}
		instance.RequestProductsInfo(list, new NativeObserver.IAPDelegate(this.FinishedToGetProductList));
		this.m_freeGetButton = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_shop_free_get_rt");
		if (this.m_freeGetButton != null)
		{
			this.m_freeGetButton.SetActive(false);
			UIButtonMessage uIButtonMessage = this.m_freeGetButton.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = this.m_freeGetButton.AddComponent<UIButtonMessage>();
			}
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickGetFreeRsRing";
			uIButtonMessage.enabled = true;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_shop_free_get_ct");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_shop_legal");
		if (gameObject3 != null)
		{
			RegionManager instance2 = RegionManager.Instance;
			if (instance2 != null)
			{
				bool active = false;
				if (instance2.IsJapan())
				{
					active = true;
				}
				gameObject3.SetActive(active);
			}
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "billing_notice");
		if (gameObject4 != null && loggedInServerInterface != null && ServerInterface.SettingState != null)
		{
			if (ServerInterface.SettingState.m_isPurchased || !ServerInterface.IsRSREnable())
			{
				gameObject4.SetActive(false);
			}
			else
			{
				gameObject4.SetActive(true);
			}
		}
		GameObject gameObject5 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_tab_raidboss");
		if (gameObject5 != null)
		{
			gameObject5.SetActive(false);
		}
		this.m_isInitShop = true;
	}

	private void SetExchangeItems(ShopUI.ServerEexchangeType type, List<ServerRedStarItemState> storeItems)
	{
		ShopUI.EexchangeType eexchangeType = this.m_exchangeTypes[(int)type];
		if (eexchangeType == null)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (i < storeItems.Count)
			{
				ShopUI.EexchangeItem eexchangeItem = eexchangeType.m_exchangeItems[i];
				eexchangeItem.m_storeItem = storeItems[i];
				eexchangeItem.m_campaignData = null;
				foreach (Constants.Campaign.emType current in ShopUI.s_campaignTypes[(int)type])
				{
					eexchangeItem.m_campaignData = ServerInterface.CampaignState.GetCampaignInSession(current, eexchangeItem.m_storeItem.m_storeItemId);
					if (eexchangeItem.m_campaignData != null)
					{
						break;
					}
				}
			}
			else if (type == ShopUI.ServerEexchangeType.RSRING)
			{
				this.SetRSRItemBtn(i);
			}
		}
	}

	private void SetRSRItemBtn(int index)
	{
		if (this.m_tabRSR != null)
		{
			int num = index + 1;
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_tabRSR, "item_" + num.ToString());
			if (gameObject != null)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_item_rs_" + num.ToString());
				if (gameObject2 != null)
				{
					UIImageButton component = gameObject2.GetComponent<UIImageButton>();
					if (component != null)
					{
						component.isEnabled = false;
					}
				}
			}
		}
	}

	private void OnShopBackButtonClicked()
	{
		NativeObserver instance = NativeObserver.Instance;
		if (instance != null)
		{
			instance.ResetIapDelegate();
		}
		HudMenuUtility.SendEnableShopButton(true);
	}

	private void FinishedToGetProductList(NativeObserver.IAPResult result)
	{
		string text = string.Format("Finished to get Product List from AppStore!(result:{0})", (int)result);
		ShopUI.EexchangeType eexchangeType = this.m_exchangeTypes[0];
		if (eexchangeType == null)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			ShopUI.EexchangeItem eexchangeItem = eexchangeType.m_exchangeItems[i];
			if (eexchangeItem.m_storeItem != null)
			{
				string productPrice = NativeObserver.Instance.GetProductPrice(eexchangeItem.m_storeItem.m_productId);
				if (productPrice != null)
				{
					this.m_redStarPrice[i] = productPrice;
				}
				else
				{
					this.m_redStarPrice[i] = eexchangeItem.m_storeItem.m_priceDisp;
				}
			}
			else
			{
				this.m_redStarPrice[i] = string.Empty;
			}
		}
		Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
		this.UpdateView();
	}

	private void UpdateView()
	{
		if (!this.m_isInitShop)
		{
			return;
		}
		for (ShopUI.ServerEexchangeType serverEexchangeType = ShopUI.ServerEexchangeType.RSRING; serverEexchangeType < ShopUI.ServerEexchangeType.Count; serverEexchangeType++)
		{
			ShopUI.EexchangeType eexchangeType = this.m_exchangeTypes[(int)serverEexchangeType];
			if (eexchangeType != null)
			{
				for (int i = 0; i < 5; i++)
				{
					ShopUI.EexchangeItem eexchangeItem = eexchangeType.m_exchangeItems[i];
					if (eexchangeItem != null)
					{
						if (eexchangeItem.m_storeItem != null)
						{
							int num = eexchangeItem.m_storeItem.m_numItem - this.GetBonusCount(serverEexchangeType, i);
							eexchangeItem.m_quantityLabel.text = HudUtility.GetFormatNumString<int>(num);
							eexchangeItem.m_quantityLabel.gameObject.SetActive(true);
							if (serverEexchangeType == ShopUI.ServerEexchangeType.RSRING)
							{
								if (!string.IsNullOrEmpty(this.m_redStarPrice[i]))
								{
									eexchangeItem.m_costLabel.text = this.m_redStarPrice[i];
								}
							}
							else
							{
								eexchangeItem.m_costLabel.text = HudUtility.GetFormatNumString<int>(eexchangeItem.m_storeItem.m_price);
							}
							eexchangeItem.m_costLabel.gameObject.SetActive(true);
							string presentString = this.GetPresentString(serverEexchangeType, i);
							bool active = presentString != null;
							eexchangeItem.m_saleSprite.gameObject.SetActive(active);
							bool isCampaign = eexchangeItem.isCampaign;
							GameObject gameObject = ShopUI.CalcSaleIconObject(eexchangeItem.m_saleSprite.gameObject, i);
							if (gameObject != null)
							{
								gameObject.SetActive(isCampaign);
							}
							if (eexchangeItem.m_bonusGameObject != null && presentString != null)
							{
								eexchangeItem.m_saleQuantityLabel.text = presentString;
								UILabel[] bonusLabels = eexchangeItem.m_bonusLabels;
								for (int j = 0; j < bonusLabels.Length; j++)
								{
									UILabel uILabel = bonusLabels[j];
									uILabel.text = presentString;
								}
							}
						}
					}
				}
			}
		}
	}

	private string GetPresentString(ShopUI.ServerEexchangeType type, int itemIndex)
	{
		ShopUI.EexchangeType eexchangeType = this.m_exchangeTypes[(int)type];
		if (eexchangeType == null)
		{
			return null;
		}
		if (eexchangeType.m_exchangeItems[itemIndex] == null)
		{
			return null;
		}
		int numItem = eexchangeType.m_exchangeItems[itemIndex].m_storeItem.m_numItem;
		ShopUI.EexchangeItem eexchangeItem = eexchangeType.m_exchangeItems[itemIndex];
		bool isCampaign = eexchangeItem.isCampaign;
		if (isCampaign)
		{
			int quantity = eexchangeItem.quantity;
			int num = quantity - numItem;
			int bonusCount = this.GetBonusCount(type, itemIndex);
			int bonusCount2 = num + bonusCount;
			return this.CalcBonusString(type, bonusCount2);
		}
		return this.CalcBonusString(type, this.GetBonusCount(type, itemIndex));
	}

	private int GetBonusCount(ShopUI.ServerEexchangeType type, int itemIndex)
	{
		ShopUI.EexchangeType eexchangeType = this.m_exchangeTypes[(int)type];
		if (eexchangeType == null)
		{
			return 0;
		}
		int numItem = eexchangeType.m_exchangeItems[0].m_storeItem.m_numItem;
		int num = (type != ShopUI.ServerEexchangeType.RSRING) ? eexchangeType.m_exchangeItems[0].m_storeItem.m_price : HudUtility.GetMixedStringToInt(eexchangeType.m_exchangeItems[0].m_costLabel.text);
		ShopUI.EexchangeItem eexchangeItem = eexchangeType.m_exchangeItems[itemIndex];
		if (eexchangeItem == null)
		{
			return 0;
		}
		int numItem2 = eexchangeType.m_exchangeItems[itemIndex].m_storeItem.m_numItem;
		int num2 = (type != ShopUI.ServerEexchangeType.RSRING) ? eexchangeItem.m_storeItem.m_price : HudUtility.GetMixedStringToInt(eexchangeItem.m_costLabel.text);
		return (int)((float)numItem2 - (float)numItem * (float)num2 / (float)num);
	}

	private string CalcBonusString(ShopUI.ServerEexchangeType type, int bonusCount)
	{
		string result = null;
		if (bonusCount > 0)
		{
			result = ShopUI.GetText("label_" + type.ToString().ToLower() + "_bonus", new Dictionary<string, string>
			{
				{
					"{COUNT}",
					HudUtility.GetFormatNumString<int>(bonusCount)
				}
			});
		}
		return result;
	}

	private void Update()
	{
		this.UpdateView();
		if (GeneralWindow.IsCreated("RsrBuyLegal") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
		}
		if (GeneralWindow.IsCreated("EventEndError") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_cmn_back");
			if (uIButtonMessage != null)
			{
				uIButtonMessage.SendMessage("OnClick");
			}
		}
		if (this.m_freeGetButton != null && !this.m_freeGetButton.activeSelf)
		{
			bool flag = true;
			RegionManager instance = RegionManager.Instance;
			if (instance != null && !instance.IsJapan())
			{
				flag = false;
			}
			if (!NoahHandler.Instance.IsEndConnect)
			{
				flag = false;
			}
			else if (!NoahHandler.Instance.IsEndSetGUID)
			{
				flag = false;
			}
			else if (!Noah.Instance.GetOfferFlag())
			{
				flag = false;
			}
			if (flag)
			{
				this.m_freeGetButton.SetActive(true);
			}
		}
	}

	private void OnClickRsr1()
	{
		this.BuyRsring(0);
	}

	private void OnClickRsr2()
	{
		this.BuyRsring(1);
	}

	private void OnClickRsr3()
	{
		this.BuyRsring(2);
	}

	private void OnClickRsr4()
	{
		this.BuyRsring(3);
	}

	private void OnClickRsr5()
	{
		this.BuyRsring(4);
	}

	private void OnClickRing1()
	{
		this.BuyRing(0);
	}

	private void OnClickRing2()
	{
		this.BuyRing(1);
	}

	private void OnClickRing3()
	{
		this.BuyRing(2);
	}

	private void OnClickRing4()
	{
		this.BuyRing(3);
	}

	private void OnClickRing5()
	{
		this.BuyRing(4);
	}

	private void OnClickChallenge1()
	{
		this.BuyChallenge(0);
	}

	private void OnClickChallenge2()
	{
		this.BuyChallenge(1);
	}

	private void OnClickChallenge3()
	{
		this.BuyChallenge(2);
	}

	private void OnClickChallenge4()
	{
		this.BuyChallenge(3);
	}

	private void OnClickChallenge5()
	{
		this.BuyChallenge(4);
	}

	private void OnClickRaidbossEnergy1()
	{
		this.BuyRaidbossEnergy(0);
	}

	private void OnClickRaidbossEnergy2()
	{
		this.BuyRaidbossEnergy(1);
	}

	private void OnClickRaidbossEnergy3()
	{
		this.BuyRaidbossEnergy(2);
	}

	private void OnClickRaidbossEnergy4()
	{
		this.BuyRaidbossEnergy(3);
	}

	private void OnClickRaidbossEnergy5()
	{
		this.BuyRaidbossEnergy(4);
	}

	private void BuyRsring(int i)
	{
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		ShopWindowRsringUI shopWindowRsringUI = GameObjectUtil.FindChildGameObjectComponent<ShopWindowRsringUI>(gameObject, "ShopWindowRsringUI");
		if (shopWindowRsringUI != null)
		{
			shopWindowRsringUI.gameObject.SetActive(true);
			shopWindowRsringUI.OpenWindow(i, this.m_exchangeTypes[0].m_exchangeItems[i], new ShopWindowRsringUI.PurchaseEndCallback(this.PurchaseSuccessCallback));
		}
	}

	private void BuyRing(int i)
	{
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		ShopWindowRingUI shopWindowRingUI = GameObjectUtil.FindChildGameObjectComponent<ShopWindowRingUI>(gameObject, "ShopWindowRingUI");
		if (shopWindowRingUI != null)
		{
			shopWindowRingUI.gameObject.SetActive(true);
			shopWindowRingUI.OpenWindow(i, this.m_exchangeTypes[1].m_exchangeItems[i]);
		}
	}

	private void BuyChallenge(int i)
	{
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		ShopWindowChallengeUI shopWindowChallengeUI = GameObjectUtil.FindChildGameObjectComponent<ShopWindowChallengeUI>(gameObject, "ShopWindowChallengeUI");
		if (shopWindowChallengeUI != null)
		{
			shopWindowChallengeUI.gameObject.SetActive(true);
			shopWindowChallengeUI.OpenWindow(i, this.m_exchangeTypes[2].m_exchangeItems[i]);
		}
	}

	private void BuyRaidbossEnergy(int i)
	{
		if (!ShopUI.isRaidbossEvent())
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "EventEndError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = ShopUI.GetText("gw_raid_count_error_caption", null),
				message = ShopUI.GetText("gw_raid_count_error_text", null),
				isPlayErrorSe = true
			});
			return;
		}
		GameObject gameObject = base.gameObject.transform.parent.gameObject;
		ShopWindowRaidUI shopWindowRaidUI = GameObjectUtil.FindChildGameObjectComponent<ShopWindowRaidUI>(gameObject, "ShopWindowRaidUI");
		if (shopWindowRaidUI != null)
		{
			shopWindowRaidUI.gameObject.SetActive(true);
		}
	}

	private void OnClickLegal()
	{
		base.StartCoroutine(this.OpenLegalWindow());
	}

	private IEnumerator OpenLegalWindow()
	{
		return new ShopUI._OpenLegalWindow_c__Iterator56();
	}

	private void SetShopBtnObj(bool flag)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_8_BC");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "shop_btn");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(flag);
			}
		}
	}

	private void OnClickGetFreeRsRing()
	{
		global::Debug.Log("ShopUI.OnClickGetFreeRsRing");
		SaveDataManager.Instance.ConnectData.ReplaceMessageBox = true;
		Noah.Instance.Offer(NoahHandler.Instance.GetGUID(), Noah.Orientation.Landscape);
	}

	public void OnRingTabChange()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.RING);
		this.SetShopBtnObj(true);
		UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject.transform.root.gameObject, "Btn_tab_ring");
		if (uIToggle != null && uIToggle.value && this.isStarted)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public void OnRsringTabChange()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.RSRING);
		this.SetShopBtnObj(true);
		UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject.transform.root.gameObject, "Btn_tab_rsring");
		if (uIToggle != null && uIToggle.value && this.isStarted)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public void OnChallengeTabChange()
	{
		this.SetCurrentType(ShopUI.ServerEexchangeType.CHALLENGE);
		this.SetShopBtnObj(true);
		UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject.transform.root.gameObject, "Btn_tab_challenge");
		if (uIToggle != null && uIToggle.value && this.isStarted)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public void OnRaidbossEnergyTabChange()
	{
		this.SetShopBtnObj(false);
		UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject.transform.root.gameObject, "Btn_tab_raidboss");
		if (uIToggle != null && uIToggle.value && this.isStarted)
		{
			SoundManager.SePlay("sys_page_skip", "SE");
		}
	}

	public static string GetText(string cellName, Dictionary<string, string> dicReplaces = null)
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", cellName).text;
		if (dicReplaces != null)
		{
			text = TextUtility.Replaces(text, dicReplaces);
		}
		return text;
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(List<ServerRedStarItemState> itemList)
	{
		foreach (ServerRedStarItemState current in itemList)
		{
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

	public void OnApplicationPause(bool flag)
	{
		if (this.m_freeGetButton != null)
		{
			this.m_freeGetButton.SetActive(false);
		}
	}

	private void PurchaseSuccessCallback(bool isSuccess)
	{
		if (isSuccess)
		{
			this.SetExchangeItems(ShopUI.ServerEexchangeType.RSRING, ServerInterface.RedStarItemList);
			Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
			this.UpdateView();
		}
	}

	public static bool isRaidbossEvent()
	{
		bool result = false;
		EventManager instance = EventManager.Instance;
		if (instance != null && instance.Type == EventManager.EventType.RAID_BOSS)
		{
			result = instance.IsChallengeEvent();
		}
		return result;
	}
}
