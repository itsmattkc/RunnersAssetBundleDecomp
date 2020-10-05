using AnimationOrTween;
using Message;
using System;
using UnityEngine;

public class AgeVerificationWindow : MonoBehaviour
{
	private GameObject m_prefabObject;

	private GameObject m_sceneLoaderObj;

	private AgeVerificationYear m_yearButton;

	private AgeVerificationButton m_monthButtons;

	private AgeVerificationButton m_dayButtons;

	private bool m_isEnd;

	private bool m_isLoad;

	public bool IsReady
	{
		get
		{
			return this.m_isLoad;
		}
		private set
		{
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
		private set
		{
		}
	}

	public bool NoInput
	{
		get
		{
			return (this.m_yearButton != null && this.m_yearButton.NoInput) || (this.m_monthButtons != null && this.m_monthButtons.NoInput) || (this.m_dayButtons != null && this.m_dayButtons.NoInput);
		}
	}

	public void Setup(string anchorPath)
	{
		if (!this.m_isLoad)
		{
			this.SetWindowData();
			this.m_isLoad = true;
		}
	}

	private void SetWindowData()
	{
		this.m_prefabObject = base.gameObject;
		if (this.m_prefabObject != null)
		{
			this.m_yearButton = this.m_prefabObject.AddComponent<AgeVerificationYear>();
			if (this.m_yearButton != null)
			{
				this.m_yearButton.SetCallback(new AgeVerificationButton.ButtonClickedCallback(this.ButtonClickedCallback));
				this.m_yearButton.Setup();
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "month_set");
			if (gameObject != null)
			{
				this.m_monthButtons = gameObject.AddComponent<AgeVerificationButton>();
				UILabel label = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_month_x0");
				this.m_monthButtons.SetLabel(AgeVerificationButton.LabelType.TYPE_TEN, label);
				GameObject upObject = GameObjectUtil.FindChildGameObject(gameObject, "Btn_month_x0_up");
				GameObject downObject = GameObjectUtil.FindChildGameObject(gameObject, "Btn_month_x0_down");
				this.m_monthButtons.SetButton(upObject, downObject);
				this.m_monthButtons.Setup(new AgeVerificationButton.ButtonClickedCallback(this.ButtonClickedCallback));
				for (int i = 1; i <= 12; i++)
				{
					this.m_monthButtons.AddValuePreset(i);
				}
				this.m_monthButtons.SetDefaultValue(1);
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "day_set");
			if (gameObject2 != null)
			{
				this.m_dayButtons = gameObject2.AddComponent<AgeVerificationButton>();
				UILabel label2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_day_x0");
				this.m_dayButtons.SetLabel(AgeVerificationButton.LabelType.TYPE_TEN, label2);
				GameObject upObject2 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_day_up");
				GameObject downObject2 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_day_down");
				this.m_dayButtons.SetButton(upObject2, downObject2);
				this.m_dayButtons.Setup(new AgeVerificationButton.ButtonClickedCallback(this.ButtonClickedCallback));
				for (int j = 1; j <= 31; j++)
				{
					this.m_dayButtons.AddValuePreset(j);
				}
				this.m_dayButtons.SetDefaultValue(1);
			}
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_prefabObject, "Btn_ok");
			if (uIButtonMessage != null)
			{
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OkButtonClickedCallback";
			}
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_prefabObject, "Btn_ok");
			if (uIImageButton != null)
			{
				uIImageButton.isEnabled = false;
			}
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "window");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "blinder");
			if (gameObject4 != null)
			{
				gameObject4.SetActive(false);
			}
		}
	}

	public void PlayStart()
	{
		this.m_isEnd = false;
		if (this.m_prefabObject == null)
		{
			return;
		}
		this.m_prefabObject.SetActive(true);
		Animation component = this.m_prefabObject.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		ActiveAnimation.Play(component, Direction.Forward);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "window");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "blinder");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(true);
		}
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	public int GetYearValue()
	{
		if (this.m_yearButton != null)
		{
			return this.m_yearButton.CurrentValue;
		}
		return 1970;
	}

	public int GetMonthValue()
	{
		if (this.m_monthButtons != null)
		{
			return this.m_monthButtons.CurrentValue;
		}
		return 1;
	}

	public int GetDayValue()
	{
		if (this.m_dayButtons != null)
		{
			return this.m_dayButtons.CurrentValue;
		}
		return 1;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void ButtonClickedCallback()
	{
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_prefabObject, "Btn_ok");
		if (uIImageButton != null)
		{
			bool isEnabled = true;
			int yearValue = this.GetYearValue();
			int monthValue = this.GetMonthValue();
			int dayValue = this.GetDayValue();
			if (!AgeVerificationUtility.IsValidDate(yearValue, monthValue, dayValue))
			{
				isEnabled = false;
			}
			if (this.NoInput)
			{
				isEnabled = false;
			}
			uIImageButton.isEnabled = isEnabled;
		}
	}

	private void OkButtonClickedCallback()
	{
		global::Debug.Log("AgeVerificationWindow.OKButtonClickedCallback");
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			string text = this.GetYearValue().ToString();
			string text2 = this.GetMonthValue().ToString();
			string text3 = this.GetDayValue().ToString();
			string birthday = string.Concat(new string[]
			{
				text,
				"-",
				text2,
				"-",
				text3
			});
			loggedInServerInterface.RequestServerSetBirthday(birthday, base.gameObject);
		}
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void ServerSetBirthday_Succeeded(MsgSetBirthday msg)
	{
		this.m_isEnd = true;
		RankingUI.CheckSnsUse();
		if (this.m_prefabObject == null)
		{
			return;
		}
		Animation component = this.m_prefabObject.GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
		if (activeAnimation == null)
		{
			return;
		}
		EventDelegate.Add(activeAnimation.onFinished, new EventDelegate(delegate
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_prefabObject, "blinder");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			BackKeyManager.RemoveWindowCallBack(base.gameObject);
		}), true);
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
	}
}
