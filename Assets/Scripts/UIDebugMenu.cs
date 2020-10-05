using App;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIDebugMenu : UIDebugMenuTask
{
	private enum MenuType
	{
		GAME,
		TITLE,
		AHIEVEMENT,
		EVENT,
		SERVER,
		ACTIVE_DEBUG_SERVER,
		MIGRATION,
		USER_MOVE,
		FACEBOOK,
		NOTIFICATION,
		CAMPAIGN,
		LOCAL1,
		LOCAL2,
		LOCAL3,
		DEVELOP,
		NUM
	}

	private sealed class _InitCoroutine_c__Iterator11 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal UIDebugMenu __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				if (DebugSaveServerUrl.Url != null)
				{
					this.__f__this.m_debugServerUrlField.text = DebugSaveServerUrl.Url;
				}
				else
				{
					this.__f__this.m_debugServerUrlField.text = NetBaseUtil.ActionServerURL;
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private List<string> MenuObjName = new List<string>
	{
		"Game",
		"Title",
		"Achievement",
		"Event",
		"Server",
		"for Dev./ActiveDebugServer",
		"for Dev./migration",
		"for Dev./user_move",
		"Facebook",
		"Notification",
		"Campaign",
		"LOCAL1",
		"LOCAL2",
		"LOCAL3",
		"DEVELOP"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 150f, 50f),
		new Rect(400f, 200f, 150f, 50f),
		new Rect(600f, 200f, 150f, 50f),
		new Rect(200f, 260f, 150f, 50f),
		new Rect(200f, 380f, 150f, 50f),
		new Rect(200f, 500f, 175f, 50f),
		new Rect(600f, 330f, 150f, 50f),
		new Rect(600f, 390f, 150f, 50f),
		new Rect(600f, 470f, 150f, 50f),
		new Rect(800f, 200f, 150f, 50f),
		new Rect(400f, 260f, 150f, 50f),
		new Rect(200f, 570f, 150f, 50f),
		new Rect(400f, 570f, 150f, 50f),
		new Rect(600f, 570f, 150f, 50f),
		new Rect(800f, 570f, 150f, 50f)
	};

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuTextField m_LoginIDField;

	private NetDebugLogin m_debugLogin;

	private UIDebugMenuTextField m_debugServerUrlField;

	private string m_clickedButtonName;

	protected override void OnStartFromTask()
	{
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		for (int i = 0; i < 15; i++)
		{
			string name = this.MenuObjName[i];
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (!(gameObject == null))
			{
				string childName = this.MenuObjName[i];
				this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
				base.AddChild(childName, gameObject);
			}
		}
		this.m_LoginIDField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		string fieldText = string.Empty;
		string gameID = SystemSaveManager.GetGameID();
		fieldText = gameID;
		this.m_LoginIDField.Setup(new Rect(200f, 350f, 375f, 30f), "Enter Login ID.", fieldText);
		GameObject gameObject2 = GameObject.Find("DebugGameObject");
		if (gameObject2 != null)
		{
			gameObject2.AddComponent<LoadURLComponent>();
		}
		this.m_debugServerUrlField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_debugServerUrlField.Setup(new Rect(200f, 470f, 375f, 30f), "forDev:デバッグサーバーURL入力(例：http://157.109.83.27/sdl/)");
		base.StartCoroutine(this.InitCoroutine());
		base.TransitionFrom();
	}

	protected override void OnTransitionTo()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(false);
		}
		if (this.m_LoginIDField != null)
		{
			this.m_LoginIDField.SetActive(false);
		}
		if (this.m_debugServerUrlField != null)
		{
			this.m_debugServerUrlField.SetActive(false);
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(true);
		}
		if (this.m_LoginIDField != null)
		{
			this.m_LoginIDField.SetActive(true);
		}
		if (this.m_debugServerUrlField != null)
		{
			this.m_debugServerUrlField.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		this.m_clickedButtonName = name;
		bool flag = true;
		if (this.m_clickedButtonName == this.MenuObjName[11] || this.m_clickedButtonName == this.MenuObjName[12] || this.m_clickedButtonName == this.MenuObjName[13] || this.m_clickedButtonName == this.MenuObjName[14] || this.m_clickedButtonName == this.MenuObjName[5] || this.m_clickedButtonName == this.MenuObjName[1] || this.m_clickedButtonName == this.MenuObjName[8] || this.m_clickedButtonName == this.MenuObjName[7])
		{
			flag = false;
		}
		if (flag)
		{
			ServerSessionWatcher serverSessionWatcher = GameObjectUtil.FindGameObjectComponent<ServerSessionWatcher>("NetMonitor");
			if (serverSessionWatcher != null)
			{
				serverSessionWatcher.ValidateSession(ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN, new ServerSessionWatcher.ValidateSessionEndCallback(this.ValidateSessionCallback));
			}
		}
		else
		{
			this.ValidateSessionCallback(true);
		}
	}

	private void ValidateSessionCallback(bool isSuccess)
	{
		if (this.m_clickedButtonName == this.MenuObjName[11])
		{
			Env.actionServerType = Env.ActionServerType.LOCAL1;
		}
		else if (this.m_clickedButtonName == this.MenuObjName[12])
		{
			Env.actionServerType = Env.ActionServerType.LOCAL2;
		}
		else if (this.m_clickedButtonName == this.MenuObjName[13])
		{
			Env.actionServerType = Env.ActionServerType.LOCAL3;
		}
		else if (this.m_clickedButtonName == this.MenuObjName[14])
		{
			Env.actionServerType = Env.ActionServerType.DEVELOP;
		}
		if (this.m_clickedButtonName == this.MenuObjName[11] || this.m_clickedButtonName == this.MenuObjName[12] || this.m_clickedButtonName == this.MenuObjName[13] || this.m_clickedButtonName == this.MenuObjName[14])
		{
			NetBaseUtil.DebugServerUrl = null;
			string text = NetBaseUtil.ActionServerURL;
		}
		else if (this.m_clickedButtonName == this.MenuObjName[5])
		{
			string text = this.m_debugServerUrlField.text;
			NetBaseUtil.DebugServerUrl = text;
			DebugSaveServerUrl.SaveURL(text);
		}
		else if (this.m_clickedButtonName == this.MenuObjName[1])
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(TitleDefine.TitleSceneName);
		}
		else
		{
			base.TransitionToChild(this.m_clickedButtonName);
		}
	}

	protected override void OnGuiFromTask()
	{
		GUI.Label(new Rect(400f, 510f, 300f, 60f), "現在のURL\n" + NetBaseUtil.ActionServerURL);
	}

	private IEnumerator InitCoroutine()
	{
		UIDebugMenu._InitCoroutine_c__Iterator11 _InitCoroutine_c__Iterator = new UIDebugMenu._InitCoroutine_c__Iterator11();
		_InitCoroutine_c__Iterator.__f__this = this;
		return _InitCoroutine_c__Iterator;
	}
}
