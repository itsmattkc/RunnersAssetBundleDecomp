using SaveData;
using System;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
	public class OptionInfo
	{
		public string label;

		public string icon;

		public OptionType type;
	}

	[SerializeField]
	private UIRectItemStorage m_itemStorage;

	[SerializeField]
	private UIScrollBar m_scrollBar;

	private GameObject m_backButtonObj;

	private bool m_inited;

	private bool m_systemSaveFlag;

	public readonly OptionUI.OptionInfo[] m_optionInfos = new OptionUI.OptionInfo[]
	{
		new OptionUI.OptionInfo
		{
			label = "users_results",
			icon = "ui_option_icon_note"
		},
		new OptionUI.OptionInfo
		{
			label = "buying_info",
			icon = "ui_option_icon_note",
			type = OptionType.BUYING_HISTORY
		},
		new OptionUI.OptionInfo
		{
			label = "push_notification",
			icon = "ui_option_icon_gear",
			type = OptionType.PUSH_NOTIFICATION
		},
		new OptionUI.OptionInfo
		{
			label = "weight_saving",
			icon = "ui_option_icon_gear",
			type = OptionType.WEIGHT_SAVING
		},
		new OptionUI.OptionInfo
		{
			label = "id_check",
			icon = "ui_option_icon_note",
			type = OptionType.ID_CHECK
		},
		new OptionUI.OptionInfo
		{
			label = "tutorial",
			icon = "ui_option_icon_note",
			type = OptionType.TUTORIAL
		},
		new OptionUI.OptionInfo
		{
			label = "past_results",
			icon = "ui_option_icon_arrow",
			type = OptionType.PAST_RESULTS
		},
		new OptionUI.OptionInfo
		{
			label = "staff_credit",
			icon = "ui_option_icon_note",
			type = OptionType.STAFF_CREDIT
		},
		new OptionUI.OptionInfo
		{
			label = "terms_of_service",
			icon = "ui_option_icon_arrow",
			type = OptionType.TERMS_OF_SERVICE
		},
		new OptionUI.OptionInfo
		{
			label = "user_name",
			icon = "ui_option_icon_gear",
			type = OptionType.USER_NAME
		},
		new OptionUI.OptionInfo
		{
			label = "sound_config",
			icon = "ui_option_icon_gear",
			type = OptionType.SOUND_CONFIG
		},
		new OptionUI.OptionInfo
		{
			label = "invite_friend",
			icon = "ui_option_icon_gear",
			type = OptionType.INVITE_FRIEND
		},
		new OptionUI.OptionInfo
		{
			label = "acceptance_of_invite",
			icon = "ui_option_icon_gear",
			type = OptionType.ACCEPT_INVITE
		},
		new OptionUI.OptionInfo
		{
			label = "facebook_access",
			icon = "ui_option_icon_gear",
			type = OptionType.FACEBOOK_ACCESS
		},
		new OptionUI.OptionInfo
		{
			label = "help",
			icon = "ui_option_icon_arrow",
			type = OptionType.HELP
		},
		new OptionUI.OptionInfo
		{
			label = "cash_clear",
			icon = "ui_option_icon_gear",
			type = OptionType.CACHE_CLEAR
		},
		new OptionUI.OptionInfo
		{
			label = "copyright",
			icon = "ui_option_icon_note",
			type = OptionType.COPYRIGHT
		},
		new OptionUI.OptionInfo
		{
			label = "back_title",
			icon = "ui_option_icon_arrow",
			type = OptionType.BACK_TITLE
		}
	};

	public bool SystemSaveFlag
	{
		set
		{
			this.m_systemSaveFlag = value;
		}
	}

	public bool IsEndSetup
	{
		get
		{
			return this.m_inited;
		}
	}

	private void Start()
	{
		base.enabled = false;
		this.m_backButtonObj = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_cmn_back");
	}

	private void Initialize()
	{
		if (!this.m_inited)
		{
			this.UpdateRectItemStorage();
			this.m_inited = true;
		}
		this.m_scrollBar.value = 0f;
		this.m_systemSaveFlag = false;
		this.CheckESRBButtonView();
	}

	private void UpdateRectItemStorage()
	{
		if (this.m_itemStorage != null)
		{
			int num = this.m_optionInfos.Length;
			this.m_itemStorage.maxItemCount = num;
			int num2 = num % this.m_itemStorage.maxColumns;
			this.m_itemStorage.maxRows = num / this.m_itemStorage.maxColumns + num2;
			this.m_itemStorage.Restart();
			ui_option_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_scroll>(true);
			int num3 = componentsInChildren.Length;
			int num4 = this.m_itemStorage.maxRows * this.m_itemStorage.maxColumns;
			for (int i = 0; i < num4; i++)
			{
				if (i < num3 && i < this.m_itemStorage.maxItemCount)
				{
					componentsInChildren[i].gameObject.SetActive(true);
					componentsInChildren[i].UpdateView(this.m_optionInfos[i], this);
				}
				else
				{
					componentsInChildren[i].gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetButtonTrigger(bool flag)
	{
		ui_option_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_scroll>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(componentsInChildren[i].gameObject, "Btn_option_top");
			if (gameObject != null)
			{
				BoxCollider component = gameObject.GetComponent<BoxCollider>();
				if (component != null)
				{
					component.isTrigger = flag;
				}
			}
		}
		if (this.m_backButtonObj != null)
		{
			BoxCollider component2 = this.m_backButtonObj.GetComponent<BoxCollider>();
			if (component2 != null)
			{
				component2.isTrigger = flag;
			}
		}
	}

	private void OnGUI()
	{
	}

	private void CheckESRBButtonView()
	{
		RegionManager instance = RegionManager.Instance;
		ui_option_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_option_scroll>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			if (componentsInChildren[i].OptionInfo != null)
			{
				if (componentsInChildren[i].OptionInfo.type == OptionType.FACEBOOK_ACCESS || componentsInChildren[i].OptionInfo.type == OptionType.PUSH_NOTIFICATION)
				{
					bool enableImageButton = false;
					if (instance != null && instance.IsUseSNS())
					{
						enableImageButton = true;
					}
					componentsInChildren[i].SetEnableImageButton(enableImageButton);
				}
			}
		}
	}

	private void OnStartOptionUI()
	{
		this.Initialize();
	}

	private void OnEndOptionUI()
	{
		if (this.m_systemSaveFlag)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				instance.SaveSystemData();
			}
			this.m_systemSaveFlag = false;
		}
	}
}
