using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ui_event_raid_scroll : MonoBehaviour
{
	private const float UPDATE_TIME = 0.25f;

	private const float RELOAD_DELAY_TIME = 1f;

	[SerializeField]
	private UISprite m_bossIcon;

	[SerializeField]
	private UISprite m_bossRarity;

	[SerializeField]
	private UILabel m_bossName;

	[SerializeField]
	private UILabel m_bossNameSh;

	[SerializeField]
	private UILabel m_bossLv;

	[SerializeField]
	private UILabel m_bossLife;

	[SerializeField]
	private UISlider m_bossLifeBar;

	[SerializeField]
	private UILabel m_discoverer;

	[SerializeField]
	private UILabel m_limit;

	[SerializeField]
	private GameObject m_rightsideBtns;

	[SerializeField]
	private GameObject m_rightsideBtn0;

	[SerializeField]
	private GameObject m_rightsideBtn1;

	[SerializeField]
	private GameObject m_rightsideBtn2;

	private RaidBossWindow m_parent;

	private RaidBossData m_bossData;

	private bool m_infoWindow;

	private float m_targetFrameTime = 0.0166666675f;

	private float m_reloadDelay;

	private float m_timeCounter = 0.25f;

	private void Start()
	{
		this.m_timeCounter = 0.25f;
	}

	private void Update()
	{
		this.m_timeCounter -= this.m_targetFrameTime;
		if (this.m_timeCounter <= 0f)
		{
			this.PeriodicUpdate();
			this.m_timeCounter = 0.25f;
		}
		if (this.m_reloadDelay > 0f && this.m_infoWindow && this.m_parent != null)
		{
			this.m_reloadDelay -= Time.deltaTime;
			if (this.m_reloadDelay <= 0f)
			{
				if (this.m_rightsideBtn0 != null)
				{
					UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_rightsideBtn0, "Btn_log");
					UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_rightsideBtn0, "Btn_play");
					if (uIImageButton != null)
					{
						uIImageButton.isEnabled = false;
					}
					if (uIImageButton2 != null)
					{
						uIImageButton2.isEnabled = false;
					}
				}
				if (this.m_parent.isLoading || !GeneralUtil.IsNetwork())
				{
					this.m_reloadDelay = 1f;
					global::Debug.Log("ui_event_raid_scroll reload retry!  IsNetwork:" + GeneralUtil.IsNetwork());
				}
				else
				{
					RaidBossInfo.currentRaidData = null;
					this.m_parent.RequestServerGetEventUserRaidBossList();
					this.m_reloadDelay = 0f;
				}
			}
		}
	}

	private void PeriodicUpdate()
	{
		if (this.m_bossData != null)
		{
			this.m_limit.text = this.m_bossData.GetTimeLimitString(true);
			if (this.m_reloadDelay <= 0f && this.m_infoWindow && this.m_parent != null && !this.m_bossData.end && this.m_bossData.GetTimeLimit().Ticks <= 0L)
			{
				this.m_reloadDelay = 1f;
			}
		}
	}

	public void UpdateView(RaidBossData bossData, RaidBossWindow parent, bool infoWindow = false)
	{
		this.m_parent = parent;
		this.m_bossData = bossData;
		this.m_timeCounter = 0.25f;
		this.m_targetFrameTime = 1f / (float)Application.targetFrameRate;
		this.m_infoWindow = infoWindow;
		this.m_reloadDelay = 0f;
		if (this.m_bossData != null)
		{
			if (this.m_bossName != null && this.m_bossNameSh != null)
			{
				UILabel arg_89_0 = this.m_bossName;
				string name = this.m_bossData.name;
				this.m_bossNameSh.text = name;
				arg_89_0.text = name;
			}
			if (this.m_discoverer != null)
			{
				if (this.m_bossData.IsDiscoverer())
				{
					ServerSettingState settingState = ServerInterface.SettingState;
					if (settingState != null)
					{
						this.m_discoverer.text = settingState.m_userName;
					}
					else
					{
						this.m_discoverer.text = this.m_bossData.discoverer;
					}
				}
				else
				{
					this.m_discoverer.text = this.m_bossData.discoverer;
				}
			}
			if (this.m_bossLife != null)
			{
				this.m_bossLife.text = string.Format("{0}/{1}", this.m_bossData.hp, this.m_bossData.hpMax);
			}
			if (this.m_bossLifeBar != null)
			{
				this.m_bossLifeBar.value = this.m_bossData.GetHpRate();
				this.m_bossLifeBar.numberOfSteps = 1;
				this.m_bossLifeBar.ForceUpdate();
			}
			if (this.m_bossLv != null)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_LevelNumber").text;
				this.m_bossLv.text = TextUtility.Replace(text, "{PARAM}", this.m_bossData.lv.ToString());
			}
			if (this.m_bossIcon != null && this.m_bossRarity != null)
			{
				this.m_bossIcon.spriteName = "ui_gp_gauge_boss_icon_raid_" + this.m_bossData.rarity;
				if (this.m_bossData.rarity >= 2)
				{
					this.m_bossRarity.spriteName = "ui_event_raidboss_window_bosslevel_2";
					this.m_bossRarity.enabled = true;
				}
				else if (this.m_bossData.rarity >= 1)
				{
					this.m_bossRarity.spriteName = "ui_event_raidboss_window_bosslevel_1";
					this.m_bossRarity.enabled = true;
				}
				else
				{
					this.m_bossRarity.spriteName = "ui_event_raidboss_window_bosslevel_0";
					this.m_bossRarity.enabled = true;
				}
			}
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "bar_base");
			if (uISprite != null)
			{
				if (this.m_bossData.IsDiscoverer())
				{
					uISprite.spriteName = "ui_event_raidboss_window_boss_bar_1";
				}
				else
				{
					uISprite.spriteName = "ui_event_raidboss_window_boss_bar_0";
				}
			}
		}
		this.PeriodicUpdate();
		this.UpdateBtn(infoWindow);
	}

	private void UpdateBtn(bool infoWindow = false)
	{
		if (this.m_bossData != null)
		{
			if (infoWindow)
			{
				bool flag = false;
				if (this.m_rightsideBtns != null)
				{
					this.m_rightsideBtns.SetActive(true);
					if (this.m_rightsideBtn0 != null)
					{
						if (!this.m_bossData.end)
						{
							this.m_rightsideBtn0.SetActive(true);
							UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_rightsideBtn0, "Btn_log");
							UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_rightsideBtn0, "Btn_play");
							if (uIImageButton != null && this.m_bossData != null)
							{
								if (this.m_bossData.IsDiscoverer() && !this.m_bossData.participation)
								{
									uIImageButton.isEnabled = false;
								}
								else
								{
									uIImageButton.isEnabled = true;
								}
							}
							if (uIImageButton2 != null && this.m_bossData != null)
							{
								uIImageButton2.isEnabled = true;
							}
							flag = true;
						}
						else
						{
							this.m_rightsideBtn0.SetActive(false);
						}
					}
					if (this.m_rightsideBtn1 != null)
					{
						if (!flag)
						{
							if (this.m_bossData.end && this.m_bossData.clear)
							{
								this.m_rightsideBtn1.SetActive(true);
								flag = true;
							}
							else
							{
								this.m_rightsideBtn1.SetActive(false);
							}
						}
						else
						{
							this.m_rightsideBtn1.SetActive(false);
						}
					}
					if (this.m_rightsideBtn2 != null)
					{
						if (!flag)
						{
							if ((this.m_bossData.participation || this.m_bossData.IsDiscoverer()) && this.m_bossData.end && !this.m_bossData.clear)
							{
								this.m_rightsideBtn2.SetActive(true);
							}
							else
							{
								this.m_rightsideBtn2.SetActive(false);
							}
						}
						else
						{
							this.m_rightsideBtn2.SetActive(false);
						}
					}
				}
			}
			else if (this.m_rightsideBtns != null)
			{
				this.m_rightsideBtns.SetActive(false);
				if (this.m_rightsideBtn0 != null)
				{
					this.m_rightsideBtn0.SetActive(false);
				}
				if (this.m_rightsideBtn1 != null)
				{
					this.m_rightsideBtn1.SetActive(false);
				}
				if (this.m_rightsideBtn2 != null)
				{
					this.m_rightsideBtn2.SetActive(false);
				}
			}
		}
	}

	private void OnClickPlay()
	{
		if (this.m_bossData != null && this.m_parent != null)
		{
			if (GeneralUtil.IsNetwork())
			{
				TimeSpan timeLimit = this.m_bossData.GetTimeLimit();
				if (timeLimit.TotalSeconds > 0.5 || timeLimit.TotalSeconds < -0.25)
				{
					this.m_parent.OnClickBossPlayButton(this.m_bossData);
				}
			}
			else
			{
				this.m_parent.ShowNoCommunication();
			}
		}
	}

	private void OnClickInfo()
	{
		if (this.m_bossData != null && this.m_parent != null)
		{
			if (GeneralUtil.IsNetwork())
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					int eventId = -1;
					if (EventManager.Instance != null)
					{
						eventId = EventManager.Instance.Id;
					}
					loggedInServerInterface.RequestServerGetEventRaidBossUserList(eventId, this.m_bossData.id, base.gameObject);
				}
				else
				{
					this.ServerGetEventRaidBossUserList_Succeeded(null);
				}
			}
			else
			{
				this.m_parent.ShowNoCommunication();
			}
		}
	}

	private void ServerGetEventRaidBossUserList_Succeeded(MsgGetEventRaidBossUserListSucceed msg)
	{
		if (this.m_bossData != null && EventManager.Instance != null)
		{
			RaidBossInfo raidBossInfo = EventManager.Instance.RaidBossInfo;
			if (raidBossInfo != null)
			{
				List<RaidBossData> raidData = raidBossInfo.raidData;
				foreach (RaidBossData current in raidData)
				{
					if (current != null && current.id == this.m_bossData.id)
					{
						this.m_bossData = current;
						break;
					}
				}
			}
			if (this.m_bossData.end && this.m_bossData.clear)
			{
				this.m_parent.OnClickBossRewardButton(this.m_bossData);
				this.SetMessageBoxConnect();
			}
			else
			{
				this.m_parent.OnClickBossInfoButton(this.m_bossData);
			}
		}
	}

	private void ServerGetEventRaidBossUserList_Failed(MsgGetEventRaidBossUserListSucceed msg)
	{
		this.m_parent.OnClickBossRewardButton(this.m_bossData);
	}

	private void SetMessageBoxConnect()
	{
		if (SaveDataManager.Instance != null && SaveDataManager.Instance.ConnectData != null)
		{
			SaveDataManager.Instance.ConnectData.ReplaceMessageBox = true;
		}
	}
}
