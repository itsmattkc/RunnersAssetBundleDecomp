using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class NetMonitor : MonoBehaviour
{
	private enum State
	{
		IDLE,
		PREPARE,
		SESSION_TIMEOUT,
		WAIT_CONNECTING,
		CONNECTING,
		ERROR_HANDLING
	}

	private sealed class _PrepareConnectCoroutine_c__Iterator5C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal HudNetworkConnect _connect___0;

		internal float _currentTime___1;

		internal ServerSessionWatcher _watcher___2;

		internal int _PC;

		internal object _current;

		internal NetMonitor __f__this;

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
				if (!this.__f__this.m_isServerBusy)
				{
					goto IL_D1;
				}
				this.__f__this.m_isServerBusy = false;
				this._connect___0 = this.__f__this.gameObject.GetComponent<HudNetworkConnect>();
				if (this._connect___0 != null)
				{
					this._connect___0.PlayStart(HudNetworkConnect.DisplayType.ALL);
				}
				this._currentTime___1 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._currentTime___1 >= 3f)
			{
				this._currentTime___1 += Time.unscaledDeltaTime;
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._connect___0 != null)
			{
				this._connect___0.PlayEnd();
			}
			IL_D1:
			this._watcher___2 = this.__f__this.gameObject.GetComponent<ServerSessionWatcher>();
			if (this._watcher___2 != null)
			{
				this._watcher___2.ValidateSession(this.__f__this.m_validateType, new ServerSessionWatcher.ValidateSessionEndCallback(this.__f__this.ValidateSessionCallback));
			}
			this._PC = -1;
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

	private static NetMonitor m_instance;

	private ErrorHandleBase m_errorHandler;

	private NetMonitor.State m_state;

	private ServerRetryProcess m_retryProcess;

	private float m_hudDisplayWait;

	private float m_currentWait;

	private HudNetworkConnect.DisplayType m_ConnectDisplayType;

	private ServerSessionWatcher.ValidateType m_validateType;

	private int m_connectFailedCount;

	private bool m_isEndPrepare;

	private bool m_isSuccessPrepare;

	private static readonly int CountToAskGiveUp = 3;

	private bool m_isServerBusy;

	public static NetMonitor Instance
	{
		get
		{
			return NetMonitor.m_instance;
		}
	}

	public bool IsIdle()
	{
		return this.m_state == NetMonitor.State.IDLE;
	}

	private void Start()
	{
		if (NetMonitor.m_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			NetMonitor.m_instance = this;
			base.gameObject.AddComponent<HudNetworkConnect>();
			base.gameObject.AddComponent<ServerSessionWatcher>();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		switch (this.m_state)
		{
		case NetMonitor.State.SESSION_TIMEOUT:
			if (NetworkErrorWindow.IsButtonPressed)
			{
				HudMenuUtility.GoToTitleScene();
				this.m_state = NetMonitor.State.IDLE;
			}
			break;
		case NetMonitor.State.WAIT_CONNECTING:
			if (this.m_hudDisplayWait < 0f)
			{
				this.m_state = NetMonitor.State.CONNECTING;
			}
			else
			{
				this.m_currentWait += Time.deltaTime;
				if (this.m_currentWait >= this.m_hudDisplayWait)
				{
					HudNetworkConnect component = base.gameObject.GetComponent<HudNetworkConnect>();
					if (component != null)
					{
						component.Setup();
						component.PlayStart(this.m_ConnectDisplayType);
					}
					this.m_state = NetMonitor.State.CONNECTING;
				}
			}
			break;
		case NetMonitor.State.ERROR_HANDLING:
			if (this.m_errorHandler != null)
			{
				this.m_errorHandler.Update();
				if (this.m_errorHandler.IsEnd())
				{
					this.m_state = NetMonitor.State.IDLE;
					this.m_errorHandler.EndErrorHandle();
					this.m_errorHandler = null;
				}
			}
			break;
		}
	}

	private void OnDestroy()
	{
	}

	public void PrepareConnect()
	{
		this.PrepareConnect(ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN);
	}

	public void PrepareConnect(ServerSessionWatcher.ValidateType validateType)
	{
		this.m_isEndPrepare = false;
		this.m_isSuccessPrepare = false;
		this.m_validateType = validateType;
		base.StartCoroutine(this.PrepareConnectCoroutine(validateType));
	}

	public IEnumerator PrepareConnectCoroutine(ServerSessionWatcher.ValidateType validateType)
	{
		NetMonitor._PrepareConnectCoroutine_c__Iterator5C _PrepareConnectCoroutine_c__Iterator5C = new NetMonitor._PrepareConnectCoroutine_c__Iterator5C();
		_PrepareConnectCoroutine_c__Iterator5C.__f__this = this;
		return _PrepareConnectCoroutine_c__Iterator5C;
	}

	public bool IsEndPrepare()
	{
		return this.m_isEndPrepare;
	}

	public bool IsSuccessPrepare()
	{
		return this.m_isSuccessPrepare;
	}

	public void StartMonitor(ServerRetryProcess process)
	{
		this.StartMonitor(process, 0f, HudNetworkConnect.DisplayType.ALL);
	}

	public void StartMonitor(ServerRetryProcess process, float hudDisplayWait, HudNetworkConnect.DisplayType displayType)
	{
		this.m_retryProcess = process;
		this.m_hudDisplayWait = hudDisplayWait;
		this.m_currentWait = 0f;
		this.m_ConnectDisplayType = displayType;
		this.m_state = NetMonitor.State.WAIT_CONNECTING;
	}

	public void EndMonitorForward(MessageBase msg, GameObject callbackObject, string callbackFuncName)
	{
		if (msg != null)
		{
			if (msg.ID == 61517)
			{
				this.m_connectFailedCount++;
				MsgServerConnctFailed msgServerConnctFailed = msg as MsgServerConnctFailed;
				if (msgServerConnctFailed != null)
				{
					ServerInterface.StatusCode status = msgServerConnctFailed.m_status;
					switch (status + 10)
					{
					case ServerInterface.StatusCode.Ok:
					case (ServerInterface.StatusCode)3:
						if (this.m_connectFailedCount < NetMonitor.CountToAskGiveUp)
						{
							ErrorHandleRetry errorHandleRetry = new ErrorHandleRetry();
							errorHandleRetry.SetRetryProcess(this.m_retryProcess);
							this.m_errorHandler = errorHandleRetry;
						}
						else
						{
							ErrorHandleAskGiveUpRetry errorHandleAskGiveUpRetry = new ErrorHandleAskGiveUpRetry();
							errorHandleAskGiveUpRetry.SetRetryProcess(this.m_retryProcess);
							this.m_errorHandler = errorHandleAskGiveUpRetry;
						}
						goto IL_20F;
					case (ServerInterface.StatusCode)1:
					case (ServerInterface.StatusCode)2:
						IL_53:
						if (status == ServerInterface.StatusCode.VersionDifference)
						{
							this.m_errorHandler = new ErrorHandleVersionDifference();
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.ServerSecurityError)
						{
							ErrorHandleSecurityError errorHandleSecurityError = new ErrorHandleSecurityError();
							errorHandleSecurityError.SetRetryProcess(this.m_retryProcess);
							this.m_errorHandler = errorHandleSecurityError;
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.ExpirationSession)
						{
							ErrorHandleExpirationSession errorHandleExpirationSession = new ErrorHandleExpirationSession();
							errorHandleExpirationSession.SetRetryProcess(this.m_retryProcess);
							errorHandleExpirationSession.SetSessionValidateType(this.m_validateType);
							this.m_errorHandler = errorHandleExpirationSession;
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.MissingPlayer)
						{
							this.m_errorHandler = new ErrorHandleMissingPlayer();
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.VersionForApplication)
						{
							ErrorHandleVersionForApplication errorHandleVersionForApplication = new ErrorHandleVersionForApplication();
							errorHandleVersionForApplication.SetRetryProcess(this.m_retryProcess);
							this.m_errorHandler = errorHandleVersionForApplication;
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.AlreadyInvitedFriend)
						{
							this.m_errorHandler = new ErrorHandleAlreadyInvited();
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.ServerMaintenance)
						{
							this.m_errorHandler = new ErrorHandleMaintenance();
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.ServerNextVersion)
						{
							this.m_errorHandler = new ErrorHandleServerNextVersion();
							goto IL_20F;
						}
						if (status == ServerInterface.StatusCode.InvalidReceiptData)
						{
							this.m_errorHandler = new ErrorHandleInvalidReciept();
							goto IL_20F;
						}
						if (status != ServerInterface.StatusCode.ServerBusy)
						{
							this.m_errorHandler = new ErrorHandleUnExpectedError();
							goto IL_20F;
						}
						this.m_errorHandler = new ErrorHandleUnExpectedError();
						this.m_isServerBusy = true;
						goto IL_20F;
					}
					goto IL_53;
				}
				IL_20F:;
			}
			else if (msg.ID == 61520)
			{
				this.m_connectFailedCount++;
				if (this.m_connectFailedCount < NetMonitor.CountToAskGiveUp)
				{
					ErrorHandleRetry errorHandleRetry2 = new ErrorHandleRetry();
					errorHandleRetry2.SetRetryProcess(this.m_retryProcess);
					this.m_errorHandler = errorHandleRetry2;
				}
				else
				{
					ErrorHandleAskGiveUpRetry errorHandleAskGiveUpRetry2 = new ErrorHandleAskGiveUpRetry();
					errorHandleAskGiveUpRetry2.SetRetryProcess(this.m_retryProcess);
					this.m_errorHandler = errorHandleAskGiveUpRetry2;
				}
			}
			else
			{
				this.OnResetConnectFailedCount();
			}
			if (this.m_errorHandler != null)
			{
				this.m_errorHandler.Setup(callbackObject, callbackFuncName, msg);
			}
		}
	}

	public void EndMonitorBackward()
	{
		HudNetworkConnect component = base.gameObject.GetComponent<HudNetworkConnect>();
		if (component != null)
		{
			component.PlayEnd();
		}
		if (this.m_errorHandler != null)
		{
			this.m_errorHandler.StartErrorHandle();
			this.m_state = NetMonitor.State.ERROR_HANDLING;
		}
		else
		{
			this.m_state = NetMonitor.State.IDLE;
		}
	}

	public void ServerReLogin_Succeeded()
	{
		this.m_isEndPrepare = true;
		this.m_isSuccessPrepare = true;
	}

	private void ValidateSessionCallback(bool isSuccess)
	{
		this.m_isEndPrepare = true;
		this.m_isSuccessPrepare = isSuccess;
		if (!this.m_isSuccessPrepare)
		{
			NetworkErrorWindow.Create(new NetworkErrorWindow.CInfo
			{
				buttonType = NetworkErrorWindow.ButtonType.Ok,
				anchor_path = string.Empty,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_session_timeout_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "NetworkError", "ui_Lbl_session_timeout_text").text
			});
			this.m_state = NetMonitor.State.SESSION_TIMEOUT;
		}
	}

	private void OnResetConnectFailedCount()
	{
		this.m_connectFailedCount = 0;
	}
}
