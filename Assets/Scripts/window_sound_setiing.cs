using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class window_sound_setiing : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private UISlider m_BGMSlider;

	[SerializeField]
	private UISlider m_SESlider;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_headerSubTextLabel;

	[SerializeField]
	private UILabel m_BGMTextLabel;

	[SerializeField]
	private UILabel m_SETextLabel;

	[SerializeField]
	private int m_tickMarkNum = 6;

	private List<float> m_tickMarkValue = new List<float>();

	private UIPlayAnimation m_uiAnimation;

	private int m_preBgmVolume;

	private int m_preSeVolume;

	private SoundManager.PlayId m_playID;

	private bool m_isEnd;

	private bool m_isOverwrite;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public bool IsOverwrite
	{
		get
		{
			return this.m_isOverwrite;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		if (this.m_BGMSlider != null)
		{
			this.m_BGMSlider.value = SoundManager.BgmVolume;
			EventDelegate.Add(this.m_BGMSlider.onChange, new EventDelegate.Callback(this.OnChangeBGMSlider));
		}
		if (this.m_SESlider != null)
		{
			this.m_SESlider.value = SoundManager.SeVolume;
			EventDelegate.Add(this.m_SESlider.onChange, new EventDelegate.Callback(this.OnChangeSESlider));
		}
		this.m_preBgmVolume = Mathf.Clamp((int)(SoundManager.BgmVolume * 100f), 0, 100);
		this.m_preSeVolume = Mathf.Clamp((int)(SoundManager.SeVolume * 100f), 0, 100);
		float num = 1f / (float)this.m_tickMarkNum;
		if (num > 0f)
		{
			for (int i = 0; i < this.m_tickMarkNum; i++)
			{
				if (i != this.m_tickMarkNum - 1)
				{
					float item = num * (float)(i + 1);
					this.m_tickMarkValue.Add(item);
				}
				else
				{
					this.m_tickMarkValue.Add(1f);
				}
			}
		}
		TextUtility.SetCommonText(this.m_headerTextLabel, "Option", "sound_config");
		TextUtility.SetCommonText(this.m_headerSubTextLabel, "Option", "sound_config_info");
		TextUtility.SetCommonText(this.m_BGMTextLabel, "Option", "sound_bgm");
		TextUtility.SetCommonText(this.m_SETextLabel, "Option", "sound_se");
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
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void Update()
	{
	}

	private void OnChangeBGMSlider()
	{
		SoundManager.BgmVolume = this.m_BGMSlider.value;
	}

	private void OnChangeSESlider()
	{
		float seVolume = SoundManager.SeVolume;
		SoundManager.SeVolume = this.m_SESlider.value;
		this.CheckSEPlay(seVolume, this.m_SESlider.value);
	}

	private void CheckSEPlay(float preValue, float currentValue)
	{
		int num = -1;
		for (int i = 0; i < this.m_tickMarkNum; i++)
		{
			if (currentValue >= this.m_tickMarkValue[i])
			{
				num = i;
			}
		}
		int num2 = -1;
		for (int j = 0; j < this.m_tickMarkNum; j++)
		{
			if (preValue >= this.m_tickMarkValue[j])
			{
				num2 = j;
			}
		}
		if (num != num2)
		{
			if (preValue == 1f && num2 - num == 1)
			{
				return;
			}
			if (this.m_playID != SoundManager.PlayId.NONE)
			{
				SoundManager.SeStop(this.m_playID);
			}
			this.m_playID = SoundManager.SePlay("obj_ring", "SE");
		}
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		int num = Mathf.Clamp((int)(SoundManager.BgmVolume * 100f), 0, 100);
		int num2 = Mathf.Clamp((int)(SoundManager.SeVolume * 100f), 0, 100);
		if (num != this.m_preBgmVolume || num2 != this.m_preSeVolume)
		{
			this.m_isOverwrite = true;
		}
		if (this.m_isOverwrite)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.bgmVolume = num;
					systemdata.seVolume = num2;
				}
			}
		}
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	public void PlayOpenWindow()
	{
		this.m_isEnd = false;
		this.m_isOverwrite = false;
		this.m_preBgmVolume = Mathf.Clamp((int)(SoundManager.BgmVolume * 100f), 0, 100);
		this.m_preSeVolume = Mathf.Clamp((int)(SoundManager.SeVolume * 100f), 0, 100);
		if (this.m_uiAnimation != null)
		{
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
