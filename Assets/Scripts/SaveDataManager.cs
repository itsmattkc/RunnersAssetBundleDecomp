using SaveData;
using System;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
	private static PlayerData m_player_data = new PlayerData();

	private static CharaData m_chara_data = new CharaData();

	private static ChaoData m_chao_data = new ChaoData();

	private static ItemData m_item_data = new ItemData();

	private static OptionData m_option_data = new OptionData();

	private static ConnectData m_connect_data = new ConnectData();

	private static SaveDataManager instance = null;

	public static SaveDataManager Instance
	{
		get
		{
			if (SaveDataManager.instance == null)
			{
				SaveDataManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(SaveDataManager)) as SaveDataManager);
			}
			return SaveDataManager.instance;
		}
	}

	public PlayerData PlayerData
	{
		get
		{
			return SaveDataManager.m_player_data;
		}
		set
		{
			SaveDataManager.m_player_data = value;
		}
	}

	public CharaData CharaData
	{
		get
		{
			return SaveDataManager.m_chara_data;
		}
		set
		{
			SaveDataManager.m_chara_data = value;
		}
	}

	public ChaoData ChaoData
	{
		get
		{
			return SaveDataManager.m_chao_data;
		}
		set
		{
			SaveDataManager.m_chao_data = value;
		}
	}

	public ItemData ItemData
	{
		get
		{
			return SaveDataManager.m_item_data;
		}
		set
		{
			SaveDataManager.m_item_data = value;
		}
	}

	public OptionData OptionData
	{
		get
		{
			return SaveDataManager.m_option_data;
		}
		set
		{
			SaveDataManager.m_option_data = value;
		}
	}

	public ConnectData ConnectData
	{
		get
		{
			return SaveDataManager.m_connect_data;
		}
		set
		{
			SaveDataManager.m_connect_data = value;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		base.enabled = false;
	}

	private bool CheckInstance()
	{
		if (SaveDataManager.instance == null)
		{
			this.LoadSaveData();
			SaveDataManager.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(SaveDataManager.instance);
			return true;
		}
		if (this == SaveDataManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	public void SaveAllData()
	{
	}

	public void SavePlayerData()
	{
	}

	public void SaveCharaData()
	{
	}

	public void SaveChaoData()
	{
	}

	public void SaveItemData()
	{
	}

	public void SaveOptionData()
	{
	}

	public void LoadSaveData()
	{
	}
}
