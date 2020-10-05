using AnimationOrTween;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RaidBossDamageRewardWindow : WindowBase
{
	private enum BUTTON_ACT
	{
		CLOSE,
		INFO,
		NONE
	}

	private const float UPDATE_TIME = 0.25f;

	[SerializeField]
	private UIPanel mainPanel;

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIDraggablePanel m_listPanel;

	[SerializeField]
	private GameObject m_topInfo;

	[SerializeField]
	private GameObject m_topReward;

	[SerializeField]
	private UILabel m_topRewardItem;

	[SerializeField]
	private UILabel m_headerLabel;

	private bool m_close;

	private RaidBossDamageRewardWindow.BUTTON_ACT m_btnAct = RaidBossDamageRewardWindow.BUTTON_ACT.NONE;

	private RaidBossData m_bossData;

	private UIRectItemStorage m_storage;

	private ui_event_raid_scroll m_bossObject;

	private ui_damage_reward_scroll m_playerObject;

	private RaidBossWindow m_parent;

	private bool m_useResult;

	private static RaidBossDamageRewardWindow s_instance;

	public bool useResult
	{
		get
		{
			return this.m_useResult;
		}
		set
		{
			this.m_useResult = value;
		}
	}

	private static RaidBossDamageRewardWindow Instance
	{
		get
		{
			return RaidBossDamageRewardWindow.s_instance;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void CallbackRaidBossDataUpdate(RaidBossData data)
	{
		this.m_bossData = data;
		if (this.m_bossData != null)
		{
			this.m_listPanel.enabled = true;
			this.m_storage = this.m_listPanel.GetComponentInChildren<UIRectItemStorage>();
			if (this.m_headerLabel != null)
			{
				if (this.m_bossData.end && this.m_bossData.clear)
				{
					this.m_headerLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_raid_reward").text;
				}
				else
				{
					this.m_headerLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_raid_damage").text;
				}
			}
			if (this.m_topRewardItem != null && this.m_bossData.end && this.m_bossData.clear)
			{
				string rewardText = this.m_bossData.GetRewardText();
				if (!string.IsNullOrEmpty(rewardText))
				{
					string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "raid_reward_text").text;
					this.m_topRewardItem.text = TextUtility.Replaces(text, new Dictionary<string, string>
					{
						{
							"{PARAM}",
							rewardText
						}
					});
				}
			}
			if (this.m_bossData.end && this.m_bossData.clear)
			{
				this.m_topInfo.SetActive(false);
				this.m_topReward.SetActive(true);
				UIRectItemStorage[] componentsInChildren = this.m_topReward.GetComponentsInChildren<UIRectItemStorage>();
				if (componentsInChildren != null)
				{
					UIRectItemStorage[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						UIRectItemStorage uIRectItemStorage = array[i];
						uIRectItemStorage.Restart();
					}
				}
				this.m_bossObject = this.m_topReward.GetComponentInChildren<ui_event_raid_scroll>();
				this.m_playerObject = null;
			}
			else
			{
				this.m_topInfo.SetActive(true);
				this.m_topReward.SetActive(false);
				UIRectItemStorage[] componentsInChildren2 = this.m_topInfo.GetComponentsInChildren<UIRectItemStorage>();
				if (componentsInChildren2 != null)
				{
					UIRectItemStorage[] array2 = componentsInChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						UIRectItemStorage uIRectItemStorage2 = array2[j];
						uIRectItemStorage2.Restart();
					}
				}
				this.m_bossObject = this.m_topInfo.GetComponentInChildren<ui_event_raid_scroll>();
				this.m_playerObject = this.m_topInfo.GetComponentInChildren<ui_damage_reward_scroll>();
				this.m_playerObject.SetClickCollision(!this.m_useResult);
			}
			int num = 0;
			long num2 = 0L;
			if (this.m_bossData.listData != null && this.m_bossData.listData.Count > 0)
			{
				num = this.m_bossData.listData.Count;
				foreach (RaidBossUser current in this.m_bossData.listData)
				{
					if (num2 < current.damage)
					{
						num2 = current.damage;
					}
				}
			}
			this.m_storage.maxItemCount = (this.m_storage.maxRows = num);
			this.m_storage.Restart();
			if (this.m_storage != null)
			{
				ui_damage_reward_scroll[] componentsInChildren3 = this.m_storage.GetComponentsInChildren<ui_damage_reward_scroll>();
				if (componentsInChildren3 != null)
				{
					for (int k = 0; k < componentsInChildren3.Length; k++)
					{
						if (num > k)
						{
							if (this.m_bossData.listData[k].damage == num2)
							{
								this.m_bossData.listData[k].rankIndex = 0;
							}
							else
							{
								this.m_bossData.listData[k].rankIndex = 1;
							}
							componentsInChildren3[k].UpdateView(this.m_bossData.listData[k], this.m_bossData);
							componentsInChildren3[k].SetClickCollision(!this.m_useResult);
							if (this.m_bossData.myData != null && this.m_bossData.myData.id == this.m_bossData.listData[k].id)
							{
								componentsInChildren3[k].SetMyRanker(true);
							}
						}
						else
						{
							UnityEngine.Object.Destroy(componentsInChildren3[k].gameObject);
						}
					}
				}
			}
			if (this.m_bossObject != null)
			{
				this.m_bossObject.UpdateView(this.m_bossData, null, false);
			}
			if (this.m_playerObject != null)
			{
				if (this.m_bossData.myData != null)
				{
					if (this.m_bossData.myData.damage == num2)
					{
						this.m_bossData.myData.rankIndex = 0;
					}
					else
					{
						this.m_bossData.myData.rankIndex = 1;
					}
					this.m_playerObject.UpdateView(this.m_bossData.myData, this.m_bossData);
					this.m_playerObject.SetMyRanker(true);
				}
				else
				{
					UnityEngine.Object.Destroy(this.m_playerObject.gameObject);
					this.m_playerObject = null;
				}
			}
			if (this.m_listPanel != null)
			{
				this.m_listPanel.ResetPosition();
			}
		}
	}

	public void Setup(RaidBossData data, RaidBossWindow parent)
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		this.m_parent = parent;
		this.mainPanel.alpha = 1f;
		this.m_close = false;
		this.m_btnAct = RaidBossDamageRewardWindow.BUTTON_ACT.NONE;
		this.m_useResult = false;
		if (this.m_storage != null)
		{
			this.m_storage.maxItemCount = (this.m_storage.maxRows = 0);
			this.m_storage.Restart();
		}
		if (data != null)
		{
			this.m_bossData = data;
			this.m_topInfo.SetActive(false);
			this.m_topReward.SetActive(false);
			if (this.m_headerLabel != null)
			{
				if (this.m_bossData.end && this.m_bossData.clear)
				{
					this.m_headerLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_raid_reward").text;
				}
				else
				{
					this.m_headerLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_raid_damage").text;
				}
			}
			if (this.m_topRewardItem != null && this.m_bossData.end && this.m_bossData.clear)
			{
				string rewardText = this.m_bossData.GetRewardText();
				if (!string.IsNullOrEmpty(rewardText))
				{
					string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "raid_reward_text").text;
					this.m_topRewardItem.text = text.Replace("{PARAM}", rewardText);
				}
				else
				{
					this.m_topRewardItem.text = string.Empty;
				}
			}
		}
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_open", "SE");
		}
	}

	public void OnClickClose()
	{
		this.m_btnAct = RaidBossDamageRewardWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			RaidBossDamageRewardWindow.BUTTON_ACT btnAct = this.m_btnAct;
			if (btnAct != RaidBossDamageRewardWindow.BUTTON_ACT.INFO)
			{
				if (this.m_parent != null)
				{
					TimeSpan timeLimit = this.m_bossData.GetTimeLimit();
					if (this.m_bossData.end || timeLimit.Ticks <= 0L)
					{
						RaidBossInfo.currentRaidData = null;
						this.m_parent.RequestServerGetEventUserRaidBossList();
					}
				}
				base.gameObject.SetActive(false);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			this.CallbackRaidBossDataUpdate(this.m_bossData);
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (ranking_window.isActive)
		{
			return;
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
		this.OnClickClose();
	}

	public static bool IsEnabled()
	{
		bool result = false;
		if (RaidBossDamageRewardWindow.s_instance != null)
		{
			result = RaidBossDamageRewardWindow.s_instance.gameObject.activeSelf;
		}
		return result;
	}

	public static RaidBossDamageRewardWindow Create(RaidBossData data, RaidBossWindow parent = null)
	{
		if (RaidBossDamageRewardWindow.s_instance != null)
		{
			RaidBossDamageRewardWindow.s_instance.gameObject.SetActive(true);
			RaidBossDamageRewardWindow.s_instance.Setup(data, parent);
			return RaidBossDamageRewardWindow.s_instance;
		}
		return null;
	}

	private void Awake()
	{
		this.SetInstance();
		base.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (RaidBossDamageRewardWindow.s_instance == this)
		{
			RaidBossDamageRewardWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (RaidBossDamageRewardWindow.s_instance == null)
		{
			RaidBossDamageRewardWindow.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
