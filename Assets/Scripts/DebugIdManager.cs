using App;
using SaveData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DebugIdManager : MonoBehaviour
{
	private class UserData
	{
		public string id;

		public string pass;

		public string date;

		public UserData(string id, string pass, string date)
		{
			this.id = id;
			this.pass = pass;
			this.date = date;
		}
	}

	private readonly string SAVE_FILE_NAME = "DebugId.txt";

	private Dictionary<string, DebugIdManager.UserData> m_userDataDict = new Dictionary<string, DebugIdManager.UserData>();

	private string actionServerType;

	private string m_saveLabel = "ID Save";

	private string m_loadLabel;

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		this.actionServerType = Env.actionServerType.ToString();
		this.LoadFile();
		this.SetLoadLabel();
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			BoxCollider boxCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(gameObject, "Btn_mainmenu");
			if (boxCollider != null)
			{
				boxCollider.center -= new Vector3(300f, 0f, 0f);
			}
		}
	}

	private void SetLoadLabel()
	{
		if (this.m_userDataDict.ContainsKey(this.actionServerType))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ID Load\n\n");
			stringBuilder.Append(this.m_userDataDict[this.actionServerType].id);
			stringBuilder.Append("\n");
			stringBuilder.Append(this.actionServerType);
			stringBuilder.Append("\n");
			stringBuilder.Append(this.m_userDataDict[this.actionServerType].date);
			this.m_loadLabel = stringBuilder.ToString();
		}
		else
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("ID Load\n\n");
			stringBuilder2.Append("----------\n");
			stringBuilder2.Append(this.actionServerType);
			stringBuilder2.Append("\n--/-- --:--:--");
			this.m_loadLabel = stringBuilder2.ToString();
		}
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect((float)(Screen.width - 150), (float)(Screen.height / 2 - 80), 140f, 60f), this.m_saveLabel) && SystemSaveManager.Instance != null)
		{
			if (SystemSaveManager.GetGameID() == "0")
			{
				global::Debug.LogWarning("Game ID has not been set!");
				return;
			}
			if (this.m_userDataDict.ContainsKey(this.actionServerType))
			{
				this.m_userDataDict[this.actionServerType].id = SystemSaveManager.GetGameID();
				this.m_userDataDict[this.actionServerType].pass = SystemSaveManager.GetGamePassword();
				this.m_userDataDict[this.actionServerType].date = DateTime.Now.ToString("MM/dd HH:mm:ss");
			}
			else
			{
				this.m_userDataDict.Add(this.actionServerType, new DebugIdManager.UserData(SystemSaveManager.GetGameID(), SystemSaveManager.GetGamePassword(), DateTime.Now.ToString("MM/dd HH:mm:ss")));
			}
			this.SaveFile();
			this.SetLoadLabel();
		}
		if (GUI.Button(new Rect((float)(Screen.width - 150), (float)(Screen.height / 2), 140f, 90f), this.m_loadLabel) && SystemSaveManager.Instance != null && this.m_userDataDict.ContainsKey(this.actionServerType))
		{
			SystemSaveManager.SetGameID(this.m_userDataDict[this.actionServerType].id);
			SystemSaveManager.SetGamePassword(this.m_userDataDict[this.actionServerType].pass);
			SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
			if (systemdata != null)
			{
				SystemSaveManager.Instance.SaveSystemData();
			}
			HudMenuUtility.GoToTitleScene();
		}
	}

	private void SaveFile()
	{
		string path = Application.persistentDataPath + "/" + this.SAVE_FILE_NAME;
		StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);
		if (streamWriter != null)
		{
			foreach (string current in this.m_userDataDict.Keys)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(current);
				stringBuilder.Append(",");
				stringBuilder.Append(this.m_userDataDict[current].id);
				stringBuilder.Append(",");
				stringBuilder.Append(this.m_userDataDict[current].pass);
				stringBuilder.Append(",");
				stringBuilder.Append(this.m_userDataDict[current].date);
				stringBuilder.Append("\n");
				streamWriter.Write(stringBuilder.ToString());
			}
			streamWriter.Close();
		}
	}

	private void LoadFile()
	{
		string path = Application.persistentDataPath + "/" + this.SAVE_FILE_NAME;
		if (File.Exists(path))
		{
			StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
			if (streamReader != null)
			{
				while (streamReader.Peek() >= 0)
				{
					string text = streamReader.ReadLine();
					string[] array = text.Split(new char[]
					{
						','
					});
					if (array != null && array.Length > 3)
					{
						this.m_userDataDict.Add(array[0], new DebugIdManager.UserData(array[1], array[2], array[3]));
					}
				}
				streamReader.Close();
			}
		}
	}
}
