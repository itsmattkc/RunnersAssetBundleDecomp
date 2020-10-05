using System;
using UnityEngine;

public class HudCharacterSubPanel : MonoBehaviour
{
	private enum DataType
	{
		IMAGE,
		IMAGE_SILHOUETTE,
		NUM
	}

	private const string COMMON_PATH = "Anchor_5_MC/2_Character/Btn_2_player/";

	private string[] m_path_name = new string[]
	{
		"img_player_sub",
		"img_player_sub_default"
	};

	private GameObject[] m_data_obj = new GameObject[2];

	private UIToggle m_toggle;

	private bool m_init_flag;

	private CharaType m_charaType = CharaType.UNKNOWN;

	private TextureRequestChara m_textureRequest;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void Initialize()
	{
		this.m_init_flag = true;
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null)
		{
			for (uint num = 0u; num < 2u; num += 1u)
			{
				Transform transform = mainMenuUIObject.transform.FindChild("Anchor_5_MC/2_Character/Btn_2_player/" + this.m_path_name[(int)((UIntPtr)num)]);
				if (transform != null)
				{
					this.m_data_obj[(int)((UIntPtr)num)] = transform.gameObject;
				}
				else
				{
					this.m_init_flag = false;
				}
			}
		}
	}

	public void OnUpdateSaveDataDisplay()
	{
		if (!this.m_init_flag)
		{
			this.Initialize();
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			CharaType subChara = instance.PlayerData.SubChara;
			if (HudCharacterPanelUtil.CheckValidChara(subChara))
			{
				HudCharacterPanelUtil.SetGameObjectActive(this.m_data_obj[(int)((UIntPtr)0)], true);
				HudCharacterPanelUtil.SetGameObjectActive(this.m_data_obj[(int)((UIntPtr)1)], false);
				if (this.m_charaType != subChara)
				{
					this.m_charaType = subChara;
					if (this.m_textureRequest != null)
					{
					}
					UITexture component = this.m_data_obj[(int)((UIntPtr)0)].GetComponent<UITexture>();
					if (component != null)
					{
						this.m_textureRequest = new TextureRequestChara(subChara, component);
						TextureAsyncLoadManager.Instance.Request(this.m_textureRequest);
					}
				}
			}
			else
			{
				HudCharacterPanelUtil.SetGameObjectActive(this.m_data_obj[(int)((UIntPtr)1)], true);
				HudCharacterPanelUtil.SetGameObjectActive(this.m_data_obj[(int)((UIntPtr)0)], false);
			}
		}
	}

	private void SetCheckFlag(bool check_flag)
	{
		if (this.m_toggle != null)
		{
			this.m_toggle.value = check_flag;
		}
	}
}
