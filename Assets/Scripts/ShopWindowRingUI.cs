using AnimationOrTween;
using Message;
using System;
using Text;
using UnityEngine;

public class ShopWindowRingUI : MonoBehaviour
{
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

	private int m_productIndex;

	private ShopUI.EexchangeItem m_exchangeItem;

	private GameObject m_windowObject;

	[SerializeField]
	public UILabel m_presentLabel;

	private void Start()
	{
		this.m_windowObject = GameObjectUtil.FindChildGameObject(base.gameObject, "window");
	}

	private void Update()
	{
		this.UpdateView();
		if (GeneralWindow.IsCreated("RingShortError") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject.transform.root.gameObject, "Btn_tab_rsring");
				if (uIToggle != null)
				{
					uIToggle.value = true;
				}
			}
			GeneralWindow.Close();
		}
		if (GeneralWindow.IsCreated("RingCountError") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
		}
	}

	public void OpenWindow(int productIndex, ShopUI.EexchangeItem exchangeItem)
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		this.m_productIndex = productIndex;
		this.m_exchangeItem = exchangeItem;
		if (this.m_windowObject != null)
		{
			this.m_windowObject.SetActive(true);
		}
		SoundManager.SePlay("sys_window_open", "SE");
		this.UpdateView();
		Animation animation = GameObjectUtil.FindGameObjectComponent<Animation>("shop_ring_window");
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

	private void OnClickBuyRing()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		if ((ulong)SaveDataManager.Instance.ItemData.RedRingCount < (ulong)((long)this.m_exchangeItem.m_storeItem.m_price))
		{
			bool flag = ServerInterface.IsRSREnable();
			GeneralWindow.ButtonType buttonType = (!flag) ? GeneralWindow.ButtonType.Ok : GeneralWindow.ButtonType.YesNo;
			string message = (!flag) ? TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text_2") : ShopUI.GetText("gw_rsring_short_error_text", null);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RingShortError",
				buttonType = buttonType,
				caption = ShopUI.GetText("gw_rsring_short_error_caption", null),
				message = message,
				isPlayErrorSe = true
			});
		}
		else if (SaveDataManager.Instance.ItemData.RingCount >= 9999999u || (ulong)(9999999u - SaveDataManager.Instance.ItemData.RingCount) < (ulong)((long)this.m_exchangeItem.quantity))
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RingCountError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = ShopUI.GetText("gw_ring_count_error_caption", null),
				message = ShopUI.GetText("gw_ring_count_error_text", null),
				isPlayErrorSe = true
			});
		}
		else
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerRedStarExchange(this.m_exchangeItem.m_storeItem.m_storeItemId, base.gameObject);
			}
			else
			{
				SoundManager.SePlay("sys_buy", "SE");
				SaveDataManager.Instance.ItemData.RedRingCount -= (uint)this.m_exchangeItem.m_storeItem.m_price;
				SaveDataManager.Instance.ItemData.RingCount += (uint)this.m_exchangeItem.quantity;
				HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			}
		}
	}

	private void ServerRedStarExchange_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		SoundManager.SePlay("sys_buy", "SE");
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
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
}
