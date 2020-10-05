using System;
using Text;
using UnityEngine;

public class window_buying_history : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_redRingText;

	[SerializeField]
	private UILabel m_redRingGetScoreText;

	[SerializeField]
	private UILabel m_redRingBuyScoreText;

	[SerializeField]
	private UILabel m_redRingGetText;

	[SerializeField]
	private UILabel m_redRingBuyText;

	[SerializeField]
	private UILabel m_ringText;

	[SerializeField]
	private UILabel m_ringGetScoreText;

	[SerializeField]
	private UILabel m_ringBuyScoreText;

	[SerializeField]
	private UILabel m_ringGetText;

	[SerializeField]
	private UILabel m_ringBuyText;

	[SerializeField]
	private UILabel m_energyText;

	[SerializeField]
	private UILabel m_energyGetScoreText;

	[SerializeField]
	private UILabel m_energyBuyScoreText;

	[SerializeField]
	private UILabel m_energyGetText;

	[SerializeField]
	private UILabel m_energyBuyText;

	private bool m_isEnd;

	private UIPlayAnimation m_uiAnimation;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		base.enabled = false;
		if (this.m_closeBtn != null)
		{
			UIPlayAnimation component = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
			}
			UIButtonMessage component2 = this.m_closeBtn.GetComponent<UIButtonMessage>();
			if (component2 == null)
			{
				this.m_closeBtn.AddComponent<UIButtonMessage>();
				component2 = this.m_closeBtn.GetComponent<UIButtonMessage>();
			}
			if (component2 != null)
			{
				component2.enabled = true;
				component2.trigger = UIButtonMessage.Trigger.OnClick;
				component2.target = base.gameObject;
				component2.functionName = "OnClickCloseButton";
			}
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component3 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component3;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		this.UpdateView();
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void OnDestroy()
	{
		global::Debug.LogWarning("window_buying_history::OnDestroy()");
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	private void UpdateView()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		if (ServerInterface.PlayerState != null)
		{
			num = ServerInterface.PlayerState.m_numBuyRedRings;
			num2 = ServerInterface.PlayerState.m_numBuyRings;
			num3 = ServerInterface.PlayerState.m_numBuyEnergy;
			num4 = ServerInterface.PlayerState.m_numRedRings - num;
			num5 = ServerInterface.PlayerState.m_numRings - num2;
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				num6 = (int)(instance.PlayerData.ChallengeCount - (uint)num3);
			}
		}
		num = Mathf.Clamp(num, 0, 99999999);
		num2 = Mathf.Clamp(num2, 0, 99999999);
		num3 = Mathf.Clamp(num3, 0, 99999999);
		num4 = Mathf.Clamp(num4, 0, 99999999);
		num5 = Mathf.Clamp(num5, 0, 99999999);
		num6 = Mathf.Clamp(num6, 0, 99999999);
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "buying_info");
		TextUtility.SetCommonText(this.m_redRingText, "Item", "red_star_ring");
		TextUtility.SetCommonText(this.m_ringText, "Item", "ring");
		TextUtility.SetCommonText(this.m_energyText, "Item", "energy");
		TextUtility.SetCommonText(this.m_redRingGetText, "Option", "take");
		TextUtility.SetCommonText(this.m_ringGetText, "Option", "take");
		TextUtility.SetCommonText(this.m_energyGetText, "Option", "take");
		TextUtility.SetCommonText(this.m_redRingBuyText, "Option", "buy");
		TextUtility.SetCommonText(this.m_ringBuyText, "Option", "buy");
		TextUtility.SetCommonText(this.m_energyBuyText, "Option", "buy");
		TextUtility.SetCommonText(this.m_redRingGetScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num4));
		TextUtility.SetCommonText(this.m_ringGetScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num5));
		TextUtility.SetCommonText(this.m_energyGetScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num6));
		TextUtility.SetCommonText(this.m_redRingBuyScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num));
		TextUtility.SetCommonText(this.m_ringBuyScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num2));
		TextUtility.SetCommonText(this.m_energyBuyScoreText, "Score", "number_of_pieces", "{NUM}", HudUtility.GetFormatNumString<int>(num3));
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		if (this.m_uiAnimation != null)
		{
			this.UpdateView();
			this.m_uiAnimation.Play(true);
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		UIButtonMessage component = this.m_closeBtn.GetComponent<UIButtonMessage>();
		if (component != null)
		{
			component.SendMessage("OnClick");
		}
	}
}
