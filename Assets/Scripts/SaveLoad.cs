using SaveData;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

public class SaveLoad
{
	private const string PLAYER_FILE_PATH = "Assets/Runners Assets/Save/save_data_player.xml";

	private const string CHAO_FILE_PATH = "Assets/Runners Assets/Save/save_data_chao.xml";

	private const string OPTION_FILE_PATH = "Assets/Runners Assets/Save/save_data_option.xml";

	private const string ITEM_FILE_PATH = "Assets/Runners Assets/Save/save_data_item.xml";

	private const string CHARA_FILE_PATH = "Assets/Runners Assets/Save/save_data_chara.xml";

	public static void SaveData<Type>(Type obj)
	{
		Type type = obj.GetType();
		if (type == typeof(PlayerData))
		{
			SaveLoad.CreateSaveData<Type>(obj, "Assets/Runners Assets/Save/save_data_player.xml");
		}
		else if (type == typeof(CharaData))
		{
			SaveLoad.CreateSaveData<Type>(obj, "Assets/Runners Assets/Save/save_data_chara.xml");
		}
		else if (type == typeof(ChaoData))
		{
			SaveLoad.CreateSaveData<Type>(obj, "Assets/Runners Assets/Save/save_data_chao.xml");
		}
		else if (type == typeof(ItemData))
		{
			SaveLoad.CreateSaveData<Type>(obj, "Assets/Runners Assets/Save/save_data_item.xml");
		}
		else if (type == typeof(OptionData))
		{
			SaveLoad.CreateSaveData<Type>(obj, "Assets/Runners Assets/Save/save_data_option.xml");
		}
	}

	public static void LoadSaveData<Type>(ref Type obj)
	{
		Type type = obj.GetType();
		if (type == typeof(PlayerData))
		{
			SaveLoad.LoadSaveData<Type>(ref obj, "Assets/Runners Assets/Save/save_data_player.xml");
		}
		else if (type == typeof(CharaData))
		{
			SaveLoad.LoadSaveData<Type>(ref obj, "Assets/Runners Assets/Save/save_data_chara.xml");
		}
		else if (type == typeof(ChaoData))
		{
			SaveLoad.LoadSaveData<Type>(ref obj, "Assets/Runners Assets/Save/save_data_chao.xml");
		}
		else if (type == typeof(ItemData))
		{
			SaveLoad.LoadSaveData<Type>(ref obj, "Assets/Runners Assets/Save/save_data_item.xml");
		}
		else if (type == typeof(OptionData))
		{
			SaveLoad.LoadSaveData<Type>(ref obj, "Assets/Runners Assets/Save/save_data_option.xml");
		}
	}

	private static void LoadSaveData<Type>(ref Type obj, string path)
	{
		if (SaveLoad.CheckSaveData(path))
		{
			if (!SaveLoad.LoadXMLSaveData<Type>(ref obj, path))
			{
				SaveLoad.CreateSaveData<Type>(obj, path);
			}
		}
		else
		{
			SaveLoad.CreateSaveData<Type>(obj, path);
		}
	}

	private static void CreateSaveData<Type>(Type obj, string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
		StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);
		if (streamWriter != null)
		{
			if (obj != null)
			{
				xmlSerializer.Serialize(streamWriter, obj);
			}
			streamWriter.Close();
		}
	}

	private static bool LoadXMLSaveData<Type>(ref Type obj, string path)
	{
		bool result = false;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Type));
		StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
		if (streamReader != null)
		{
			object obj2 = (Type)((object)xmlSerializer.Deserialize(streamReader));
			if (obj2 != null)
			{
				obj = (Type)((object)obj2);
				result = true;
			}
			streamReader.Close();
		}
		return result;
	}

	public static bool CheckSaveData(string path)
	{
		return File.Exists(path);
	}
}
