using Message;
using NoahUnity;
using SaveData;
using System;
using UnityEngine;

public class ServerSessionWatcher : MonoBehaviour
{
	public enum ValidateType
	{
		NONE = -1,
		PRELOGIN,
		LOGIN,
		LOGIN_OR_RELOGIN,
		COUNT
	}

	private enum SessionState
	{
		NONE = -1,
		NEED_LOGIN,
		VALID,
		COUNT
	}

	public delegate void ValidateSessionEndCallback(bool isSuccess);

	private ServerSessionWatcher.ValidateType m_validateType;

	private ServerSessionWatcher.ValidateSessionEndCallback m_callback;

	private string m_loginKey = string.Empty;

	private bool m_isSendNoahId;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ValidateSession(ServerSessionWatcher.ValidateType type, ServerSessionWatcher.ValidateSessionEndCallback callback, string loginKey)
	{
		this.m_loginKey = loginKey;
		this.ValidateSession(type, callback);
	}

	public void ValidateSession(ServerSessionWatcher.ValidateType type, ServerSessionWatcher.ValidateSessionEndCallback callback)
	{
		this.m_validateType = type;
		this.m_callback = callback;
		ServerSessionWatcher.SessionState sessionState = this.CheckSessionState();
		ServerSessionWatcher.SessionState sessionState2 = sessionState;
		if (sessionState2 != ServerSessionWatcher.SessionState.NEED_LOGIN)
		{
			if (sessionState2 != ServerSessionWatcher.SessionState.VALID)
			{
				this.Fail();
			}
			else
			{
				this.Success();
			}
		}
		else
		{
			switch (this.m_validateType)
			{
			case ServerSessionWatcher.ValidateType.PRELOGIN:
				this.Login();
				break;
			case ServerSessionWatcher.ValidateType.LOGIN:
				this.Login();
				break;
			case ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN:
				this.Login();
				break;
			default:
				this.Fail();
				break;
			}
		}
	}

	public void InvalidateSession()
	{
		ServerLoginState loginState = ServerInterface.LoginState;
		if (loginState == null)
		{
			return;
		}
		loginState.sessionTimeLimit = 0;
		loginState.seqNum = 0uL;
	}

	private ServerSessionWatcher.SessionState CheckSessionState()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface == null)
		{
			return ServerSessionWatcher.SessionState.NEED_LOGIN;
		}
		ServerLoginState loginState = ServerInterface.LoginState;
		DateTime localDateTime = NetUtil.GetLocalDateTime((long)loginState.sessionTimeLimit);
		DateTime currentTime = NetUtil.GetCurrentTime();
		if (NetUtil.IsAlreadySessionTimeOut(localDateTime, currentTime))
		{
			return ServerSessionWatcher.SessionState.NEED_LOGIN;
		}
		return ServerSessionWatcher.SessionState.VALID;
	}

	private void Success()
	{
		global::Debug.Log("ServerSessionWatcher:Succeeded");
		this.m_callback(true);
	}

	private void Fail()
	{
		global::Debug.Log("ServerSessionWatcher:Failed");
		this.m_callback(false);
	}

	private void Login()
	{
		global::Debug.Log("ServerSessionWatcher:Login");
		if (!string.IsNullOrEmpty(this.m_loginKey))
		{
			string systemSaveDataGameId = TitleUtil.GetSystemSaveDataGameId();
			string systemSaveDataPassword = TitleUtil.GetSystemSaveDataPassword();
			string password = NetUtil.CalcMD5String(string.Concat(new string[]
			{
				this.m_loginKey,
				":dho5v5yy7n2uswa5iblb:",
				systemSaveDataGameId,
				":",
				systemSaveDataPassword
			}));
			ServerInterface serverInterface = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
			if (serverInterface != null)
			{
				serverInterface.RequestServerLogin(systemSaveDataGameId, password, base.gameObject);
			}
		}
		else
		{
			string systemSaveDataGameId2 = TitleUtil.GetSystemSaveDataGameId();
			ServerInterface serverInterface2 = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
			if (serverInterface2 != null)
			{
				serverInterface2.RequestServerLogin(systemSaveDataGameId2, string.Empty, base.gameObject);
			}
		}
	}

	private void Relogin()
	{
		global::Debug.Log("ServerSessionWatcher:ReLogin");
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerReLogin(base.gameObject);
		}
	}

	private void ServerLogin_Succeeded(MsgLoginSucceed msg)
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		bool flag = TitleUtil.SetSystemSaveDataGameId(msg.m_userId);
		bool flag2 = TitleUtil.SetSystemSaveDataPassword(msg.m_password);
		if ((flag || flag2) && instance != null)
		{
			instance.SaveSystemData();
		}
		if (flag)
		{
			FoxManager.SendLtvPoint(FoxLtvType.RegisterAccount);
		}
		this.m_loginKey = string.Empty;
		this.Success();
		bool flag3 = false;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				string noahID = Noah.Instance.GetNoahID();
				string noahId = systemdata.noahId;
				global::Debug.Log("CurrentNoahID=" + noahID);
				global::Debug.Log("PreviousNoahID=" + noahId);
				if (noahID != noahId)
				{
					flag3 = true;
				}
			}
		}
		if (!this.m_isSendNoahId && flag3)
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				string noahID2 = Noah.Instance.GetNoahID();
				if (!string.IsNullOrEmpty(noahID2))
				{
					loggedInServerInterface.RequestServerSetNoahId(noahID2, base.gameObject);
				}
			}
			this.m_isSendNoahId = true;
			global::Debug.Log("NoahId sended");
		}
		else
		{
			global::Debug.Log("NoahId don't sended");
		}
	}

	private void ServerLogin_Failed(MessageBase msg)
	{
		if (msg == null)
		{
			return;
		}
		MsgServerPasswordError msgServerPasswordError = msg as MsgServerPasswordError;
		if (msgServerPasswordError == null)
		{
			return;
		}
		this.m_loginKey = msgServerPasswordError.m_key;
		bool flag = TitleUtil.SetSystemSaveDataGameId(msgServerPasswordError.m_userId);
		bool flag2 = TitleUtil.SetSystemSaveDataPassword(msgServerPasswordError.m_password);
		if (flag || flag2)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				instance.SaveSystemData();
			}
		}
		if (flag)
		{
			FoxManager.SendLtvPoint(FoxLtvType.RegisterAccount);
		}
		if (this.m_validateType == ServerSessionWatcher.ValidateType.PRELOGIN)
		{
			this.m_callback(true);
			return;
		}
		global::Debug.Log("GameModeTitle.UserId: " + msgServerPasswordError.m_userId);
		global::Debug.Log("GameModeTitle.Password: " + msgServerPasswordError.m_password);
		this.Login();
	}

	public void ServerReLogin_Succeeded()
	{
		this.Success();
	}

	private void ServerSetNoahId_Succeeded(MsgSetNoahIdSucceed msg)
	{
		global::Debug.Log("GameModeTitle.ServerSetNoahId_Succeeded");
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				string noahID = Noah.Instance.GetNoahID();
				systemdata.noahId = noahID;
				instance.SaveSystemData();
			}
		}
	}
}
