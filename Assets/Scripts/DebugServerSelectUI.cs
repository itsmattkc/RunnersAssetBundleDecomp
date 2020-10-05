using App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DebugServerSelectUI : MonoBehaviour
{
	private enum ButtonType
	{
		IDLE = -1,
		LOCAL1,
		LOCAL2,
		LOCAL3,
		LOCAL4,
		LOCAL5,
		DEVELOP1,
		DEVELOP2,
		DEVELOP3,
		RELEASE,
		TITLE
	}

	private readonly Vector2 SCREEN_RECT_SIZE = new Vector2(1600f, 900f);

	private Vector2 m_GUIScale = default(Vector2);

	private Dictionary<DebugServerSelectUI.ButtonType, Rect> m_ButtonDict = new Dictionary<DebugServerSelectUI.ButtonType, Rect>();

	private GUIStyle m_buttonStyle;

	private GUIStyle m_labelStyle;

	private bool m_GUIInited;

	private int m_fontSize;

	private int m_commandCount;

	private readonly int POP_BUTTON_COUNT = 10;

	private Rect m_hiddenButtonRect;

	private readonly string SAVE_FILE_NAME = "DefaultActionServerType.txt";

	private Dictionary<string, string> m_DefaultActionServerType = new Dictionary<string, string>();

	private StringBuilder m_versionLabel = new StringBuilder();

	private StringBuilder m_serverLabel = new StringBuilder();

	private DebugServerSelectUI.ButtonType m_buttonType;

	private readonly string[] ButtonName = new string[]
	{
		"Local1",
		"Local2",
		"Local3",
		"Local4",
		"Local5",
		"Develop1",
		"Develop2",
		"Develop3",
		"Release",
		"Title"
	};

	private void Start()
	{
		this.m_buttonType = DebugServerSelectUI.ButtonType.IDLE;
		this.LoadFile();
		if (this.m_DefaultActionServerType.ContainsKey(CurrentBundleVersion.version))
		{
			string text = this.m_DefaultActionServerType[CurrentBundleVersion.version];
			try
			{
				Env.actionServerType = (Env.ActionServerType)((int)Enum.Parse(typeof(Env.ActionServerType), text));
			}
			catch (ArgumentException var_1_52)
			{
				global::Debug.Log("Load ServerType Error " + text);
			}
		}
		this.m_versionLabel.Append("Ver : ");
		this.m_versionLabel.Append(CurrentBundleVersion.version);
		this.m_serverLabel.Append("Server : ");
		this.m_serverLabel.Append(Env.actionServerType);
		this.m_fontSize = 32;
		this.m_GUIScale = new Vector2((float)Screen.width / this.SCREEN_RECT_SIZE.x, (float)Screen.height / this.SCREEN_RECT_SIZE.y);
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.LOCAL1, this.MatchScreenSize(new Rect(100f, 200f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.LOCAL2, this.MatchScreenSize(new Rect(400f, 200f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.LOCAL3, this.MatchScreenSize(new Rect(700f, 200f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.LOCAL4, this.MatchScreenSize(new Rect(1000f, 200f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.LOCAL5, this.MatchScreenSize(new Rect(1300f, 200f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.DEVELOP1, this.MatchScreenSize(new Rect(100f, 400f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.DEVELOP2, this.MatchScreenSize(new Rect(400f, 400f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.DEVELOP3, this.MatchScreenSize(new Rect(700f, 400f, 250f, 150f)));
		this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.TITLE, this.MatchScreenSize(new Rect(1000f, 600f, 550f, 150f)));
		this.m_hiddenButtonRect = this.MatchScreenSize(new Rect(0f, 0f, 200f, 150f));
	}

	private void OnGUI()
	{
		if (!this.m_GUIInited)
		{
			this.m_buttonStyle = new GUIStyle(GUI.skin.button);
			this.m_buttonStyle.fontSize = (int)((float)this.m_fontSize * this.m_GUIScale.x);
			this.m_labelStyle = new GUIStyle(GUI.skin.label);
			this.m_labelStyle.fontSize = (int)((float)this.m_fontSize * this.m_GUIScale.x);
			this.m_labelStyle.alignment = TextAnchor.UpperCenter;
			this.m_GUIInited = true;
		}
		GUI.Label(this.MatchScreenSize(new Rect(0f, 60f, this.SCREEN_RECT_SIZE.x, 100f)), this.m_versionLabel.ToString(), this.m_labelStyle);
		GUI.Label(this.MatchScreenSize(new Rect(0f, 120f, this.SCREEN_RECT_SIZE.x, 100f)), this.m_serverLabel.ToString(), this.m_labelStyle);
		foreach (DebugServerSelectUI.ButtonType current in this.m_ButtonDict.Keys)
		{
			if (current == this.m_buttonType)
			{
				GUI.color = Color.yellow;
				GUI.Box(this.m_ButtonDict[current], string.Empty);
			}
			if (GUI.Button(this.m_ButtonDict[current], this.ButtonName[(int)current], this.m_buttonStyle))
			{
				this.OnClickButton(current);
				this.m_serverLabel.Length = 0;
				this.m_serverLabel.Append("Server : ");
				this.m_serverLabel.Append(Env.actionServerType);
			}
			if (current == this.m_buttonType)
			{
				GUI.color = Color.white;
			}
		}
		if (GUI.Button(this.m_hiddenButtonRect, string.Empty, GUIStyle.none))
		{
			this.m_commandCount++;
			if (this.m_hiddenButtonRect.x == 0f)
			{
				this.m_hiddenButtonRect.x = (float)Screen.width - this.m_hiddenButtonRect.width;
			}
			else
			{
				this.m_hiddenButtonRect.x = 0f;
			}
			if (this.m_commandCount == this.POP_BUTTON_COUNT)
			{
				this.m_ButtonDict.Add(DebugServerSelectUI.ButtonType.RELEASE, this.MatchScreenSize(new Rect(100f, 600f, 250f, 150f)));
			}
		}
	}

	private void OnClickButton(DebugServerSelectUI.ButtonType type)
	{
		this.m_buttonType = type;
		switch (type)
		{
		case DebugServerSelectUI.ButtonType.LOCAL1:
			Env.actionServerType = Env.ActionServerType.LOCAL1;
			return;
		case DebugServerSelectUI.ButtonType.LOCAL2:
			Env.actionServerType = Env.ActionServerType.LOCAL2;
			return;
		case DebugServerSelectUI.ButtonType.LOCAL3:
			Env.actionServerType = Env.ActionServerType.LOCAL3;
			return;
		case DebugServerSelectUI.ButtonType.LOCAL4:
			Env.actionServerType = Env.ActionServerType.LOCAL4;
			return;
		case DebugServerSelectUI.ButtonType.LOCAL5:
			Env.actionServerType = Env.ActionServerType.LOCAL5;
			return;
		case DebugServerSelectUI.ButtonType.DEVELOP1:
			Env.actionServerType = Env.ActionServerType.DEVELOP;
			return;
		case DebugServerSelectUI.ButtonType.DEVELOP2:
			Env.actionServerType = Env.ActionServerType.DEVELOP2;
			return;
		case DebugServerSelectUI.ButtonType.DEVELOP3:
			Env.actionServerType = Env.ActionServerType.DEVELOP3;
			return;
		case DebugServerSelectUI.ButtonType.RELEASE:
			Env.actionServerType = Env.ActionServerType.RELEASE;
			return;
		case DebugServerSelectUI.ButtonType.TITLE:
			if (this.m_DefaultActionServerType.ContainsKey(CurrentBundleVersion.version))
			{
				this.m_DefaultActionServerType[CurrentBundleVersion.version] = Env.actionServerType.ToString();
			}
			else
			{
				this.m_DefaultActionServerType.Add(CurrentBundleVersion.version, Env.actionServerType.ToString());
			}
			this.SaveFile();
			UnityEngine.SceneManagement.SceneManager.LoadScene(TitleDefine.TitleSceneName);
			return;
		default:
			return;
		}
	}

	private Rect MatchScreenSize(Rect rect)
	{
		Rect result = new Rect(rect);
		result.x *= this.m_GUIScale.x;
		result.width *= this.m_GUIScale.x;
		result.y *= this.m_GUIScale.y;
		result.height *= this.m_GUIScale.y;
		return result;
	}

	private void SaveFile()
	{
		string path = Application.persistentDataPath + "/" + this.SAVE_FILE_NAME;
		StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);
		if (streamWriter != null)
		{
			foreach (KeyValuePair<string, string> current in this.m_DefaultActionServerType)
			{
				streamWriter.Write(current.Key + "," + current.Value + "\n");
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
					if (array != null && array.Length > 1)
					{
						this.m_DefaultActionServerType.Add(array[0], array[1]);
					}
				}
				streamReader.Close();
			}
		}
	}
}
