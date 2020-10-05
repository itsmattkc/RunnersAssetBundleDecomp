using DataTable;
using System;
using Text;
using UnityEngine;

public class HudChaoPanel : MonoBehaviour
{
	private enum DataType
	{
		MAIN_IMAGE,
		MAIN_SILHOUETTE,
		SUB_IMAGE,
		SUB_SILHOUETTE,
		NUM
	}

	private string[] m_path_name = new string[]
	{
		"Btn_1_chao/img_chao_main",
		"Btn_1_chao/img_chao_main_default",
		"Btn_1_chao/img_chao_sub",
		"Btn_1_chao/img_chao_sub_default"
	};

	private GameObject[] m_data_obj = new GameObject[4];

	private bool m_init_flag;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void OnDestroy()
	{
	}

	private void Initialize()
	{
		this.m_init_flag = true;
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(mainMenuUIObject, "2_Character");
			if (gameObject != null)
			{
				for (uint num = 0u; num < 4u; num += 1u)
				{
					Transform transform = gameObject.transform.FindChild(this.m_path_name[(int)((UIntPtr)num)]);
					if (transform != null)
					{
						this.m_data_obj[(int)((UIntPtr)num)] = transform.gameObject;
					}
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
			int mainChaoID = instance.PlayerData.MainChaoID;
			this.SetChaoImage(mainChaoID, this.m_data_obj[(int)((UIntPtr)0)], this.m_data_obj[(int)((UIntPtr)1)]);
			int subChaoID = instance.PlayerData.SubChaoID;
			this.SetChaoImage(subChaoID, this.m_data_obj[(int)((UIntPtr)2)], this.m_data_obj[(int)((UIntPtr)3)]);
		}
	}

	private void SetChaoImage(int chao_id, GameObject imageObj, GameObject silhouetteObj)
	{
		if (imageObj == null)
		{
			return;
		}
		if (silhouetteObj == null)
		{
			return;
		}
		if (chao_id >= 0)
		{
			imageObj.SetActive(true);
			silhouetteObj.SetActive(false);
			UITexture component = imageObj.GetComponent<UITexture>();
			if (component != null)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(component, null, true);
				ChaoTextureManager.Instance.GetTexture(chao_id, info);
				component.enabled = true;
			}
		}
		else
		{
			silhouetteObj.SetActive(true);
			imageObj.SetActive(false);
		}
	}

	private void SetChaoName(int chao_id, GameObject obj)
	{
		if (obj != null)
		{
			UILabel component = obj.GetComponent<UILabel>();
			if (component != null)
			{
				if (chao_id >= 0)
				{
					string chaoText = TextUtility.GetChaoText("Chao", "name_for_menu_" + chao_id.ToString("D4"));
					if (chaoText != null)
					{
						component.text = chaoText;
					}
				}
				else
				{
					component.text = string.Empty;
				}
			}
		}
	}

	private void SetChaoBonusName(int chao_id, GameObject obj)
	{
		if (obj != null)
		{
			UILabel component = obj.GetComponent<UILabel>();
			if (component != null)
			{
				component.text = HudUtility.GetChaoMenuAbilityText(chao_id);
			}
		}
	}

	private void SetChaoLevel(int chao_id, GameObject obj)
	{
		if (obj != null)
		{
			UILabel component = obj.GetComponent<UILabel>();
			if (component != null)
			{
				if (chao_id >= 0)
				{
					ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
					if (chaoData != null)
					{
						int level = chaoData.level;
						if (level > -1)
						{
							obj.SetActive(true);
							component.text = TextUtility.GetTextLevel(level.ToString());
						}
					}
				}
				else
				{
					obj.SetActive(false);
				}
			}
		}
	}

	private void SetChaoTypeImage(int chao_id, GameObject obj)
	{
		if (obj != null)
		{
			if (chao_id >= 0)
			{
				obj.SetActive(true);
				UISprite component = obj.GetComponent<UISprite>();
				if (component != null)
				{
					ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
					if (chaoData != null)
					{
						string str = chaoData.charaAtribute.ToString().ToLower();
						component.spriteName = "ui_chao_set_type_icon_" + str;
					}
				}
			}
			else
			{
				obj.SetActive(false);
			}
		}
	}

	private void SetRareImage(int chao_id, GameObject obj)
	{
		if (obj != null)
		{
			obj.SetActive(true);
			UISprite component = obj.GetComponent<UISprite>();
			if (component != null)
			{
				if (chao_id >= 0)
				{
					ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
					if (chaoData != null)
					{
						component.spriteName = "ui_chao_set_bg_ll_" + (int)chaoData.rarity;
					}
				}
				else
				{
					component.spriteName = "ui_chao_set_bg_ll_3";
				}
			}
		}
	}

	private void SetChaoTexture(GameObject obj, Texture tex)
	{
		if (obj != null)
		{
			obj.SetActive(true);
			UITexture component = obj.GetComponent<UITexture>();
			if (component != null)
			{
				component.enabled = (tex != null);
				if (tex != null)
				{
					component.mainTexture = tex;
				}
			}
		}
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			if (data.chao_id == instance.PlayerData.MainChaoID)
			{
				this.SetChaoTexture(this.m_data_obj[(int)((UIntPtr)0)], data.tex);
			}
			else if (data.chao_id == instance.PlayerData.SubChaoID)
			{
				this.SetChaoTexture(this.m_data_obj[(int)((UIntPtr)2)], data.tex);
			}
		}
	}
}
