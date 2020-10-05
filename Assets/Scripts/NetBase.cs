using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class NetBase
{
	public enum emState
	{
		Executing,
		Succeeded,
		AvailableFailed,
		UnavailableFailed
	}

	private static string mUndefinedComparer = "ERROR_CODE(32)";

	private string m_netBaseStr = "close";

	private string m_jMsgStr = "MessageJP";

	private string m_eMsgStr = "MessageEN";

	protected bool mEmulation;

	private URLRequest mRequest;

	private JsonData mResultJson;

	private JsonData mResultParamJson;

	private string mActionName;

	private string mJsonFromDll = string.Empty;

	private int mDebugLogDisplayLevel;

	private bool mMaintenance;

	private float mStartTime;

	private float mEndTime;

	private bool m_secureFlag = true;

	private JsonWriter _paramWriter_k__BackingField;

	private NetBase.emState _state_k__BackingField;

	private int _result_k__BackingField;

	private string _errorMessage_k__BackingField;

	private int _meintenanceValue_k__BackingField;

	private int _dataVersion_k__BackingField;

	private string _meintenanceMessage_k__BackingField;

	private float _elapsedTime_k__BackingField;

	private int _versionValue_k__BackingField;

	private string _versionString_k__BackingField;

	private int _assetVersionValue_k__BackingField;

	private string _assetVersionString_k__BackingField;

	private string _infoVersionString_k__BackingField;

	private long _serverTime_k__BackingField;

	private ulong _seqNum_k__BackingField;

	private int _clientDataVersion_k__BackingField;

	private bool _enableUndefinedCompare_k__BackingField;

	private Action<NetBase> _responseEvent_k__BackingField;

	private static long _LastNetServerTime_k__BackingField;

	private static long _TimeDifference_k__BackingField;

	public static string undefinedComparer
	{
		set
		{
			NetBase.mUndefinedComparer = value;
		}
	}

	protected JsonWriter paramWriter
	{
		get;
		private set;
	}

	public NetBase.emState state
	{
		get;
		protected set;
	}

	public int result
	{
		get;
		set;
	}

	public ServerInterface.StatusCode resultStCd
	{
		get
		{
			return (ServerInterface.StatusCode)this.result;
		}
		set
		{
			this.result = (int)value;
		}
	}

	public string errorMessage
	{
		get;
		private set;
	}

	public int meintenanceValue
	{
		get;
		private set;
	}

	public int dataVersion
	{
		get;
		private set;
	}

	public string meintenanceMessage
	{
		get;
		private set;
	}

	public float elapsedTime
	{
		get;
		private set;
	}

	public int versionValue
	{
		get;
		private set;
	}

	public string versionString
	{
		get;
		private set;
	}

	public int assetVersionValue
	{
		get;
		private set;
	}

	public string assetVersionString
	{
		get;
		private set;
	}

	public string infoVersionString
	{
		get;
		private set;
	}

	public long serverTime
	{
		get;
		private set;
	}

	public ulong seqNum
	{
		get;
		private set;
	}

	public int clientDataVersion
	{
		get;
		private set;
	}

	protected bool enableUndefinedCompare
	{
		private get;
		set;
	}

	public URLRequest urlRequest
	{
		get
		{
			return this.mRequest;
		}
	}

	public Action<NetBase> responseEvent
	{
		private get;
		set;
	}

	public static long LastNetServerTime
	{
		get;
		set;
	}

	private static long TimeDifference
	{
		get;
		set;
	}

	public NetBase()
	{
		this.mEmulation = false;
		this.mRequest = new URLRequest();
		this.paramWriter = new JsonWriter();
		this.state = NetBase.emState.Executing;
		this.mResultJson = null;
		this.mResultParamJson = null;
		this.mActionName = null;
		this.elapsedTime = 0f;
		this.enableUndefinedCompare = true;
		this.responseEvent = null;
	}

	public void Request()
	{
		global::Debug.Log("NetBase.Request [" + this + "]", DebugTraceManager.TraceType.SERVER);
		this.state = NetBase.emState.Executing;
		this.mRequest.beginRequest = new Action(this.BeginRequest);
		this.mRequest.success = new Action<WWW>(this.DidSuccess);
		this.mRequest.failure = new Action<WWW, bool, bool>(this.DidFailure);
		this.mRequest.Emulation = this.mEmulation;
		this.elapsedTime = 0f;
		this.mStartTime = Time.realtimeSinceStartup;
		URLRequestManager.Instance.Request(this.mRequest);
	}

	private void BeginRequest()
	{
		this.paramWriter.WriteObjectStart();
		this.SetCommonRequestParam();
		this.DoRequest();
		this.SetRequestUrl();
		this.SetRequestParam();
		this.SetSecureRequestParam();
		this.paramWriter.Reset();
		this.paramWriter = null;
	}

	private void SetRequestUrl()
	{
		this.mRequest.url = NetBaseUtil.ActionServerURL + this.mActionName;
	}

	private bool IsJsonFromDll()
	{
		return !string.IsNullOrEmpty(this.mJsonFromDll);
	}

	private bool IsSecure()
	{
		if (!this.IsJsonFromDll())
		{
			return false;
		}
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		return !(instance == null) && instance.IsSecure();
	}

	protected void SetSecureFlag(bool flag)
	{
		this.m_secureFlag = flag;
	}

	private void SetCommonRequestParam()
	{
		ServerLoginState loginState = ServerInterface.LoginState;
		bool isLoggedIn = loginState.IsLoggedIn;
		string sessionId = loginState.sessionId;
		string version = CurrentBundleVersion.version;
		ulong num = loginState.seqNum + 1uL;
		if (isLoggedIn)
		{
			this.WriteActionParamValue("sessionId", sessionId);
		}
		this.WriteActionParamValue("version", version);
		this.WriteActionParamValue("seq", num);
	}

	private void SetRequestParam()
	{
		this.paramWriter.WriteObjectEnd();
		string text = string.Empty;
		bool flag = this.IsJsonFromDll();
		if (flag)
		{
			text = this.mJsonFromDll;
		}
		else
		{
			text = this.paramWriter.ToString();
		}
		if (0 < text.Length)
		{
			this.mRequest.AddParam("param", text);
		}
	}

	private void SetSecureRequestParam()
	{
		string value = "0";
		string value2 = string.Empty;
		if (this.IsSecure())
		{
			value = "1";
			value2 = CryptoUtility.code;
		}
		this.mRequest.AddParam("secure", value);
		this.mRequest.AddParam("key", value2);
	}

	public void DidSuccess(WWW www)
	{
		if (www != null)
		{
			this.DidSuccess(www.text);
		}
		else
		{
			this.DidSuccess(string.Empty);
		}
	}

	public void DidSuccess(string responseText)
	{
		this.mEndTime = Time.realtimeSinceStartup;
		this.elapsedTime = this.mEndTime - this.mStartTime;
		this.OutputResponseText(responseText);
		this.state = NetBase.emState.Succeeded;
		if (URLRequestManager.Instance.Emulation || this.mEmulation)
		{
			this.DoEmulation();
		}
		else
		{
			if (responseText.Length == 0)
			{
				global::Debug.Log(this + " responce is empty.", DebugTraceManager.TraceType.SERVER);
				this.state = NetBase.emState.UnavailableFailed;
				return;
			}
			if (this.IsUndefinedAction(responseText))
			{
				this.DoEmulation();
			}
			else
			{
				this.mResultJson = JsonMapper.ToObject(responseText);
				bool flag = false;
				if (this.IsJsonFromDll())
				{
					string jsonString = NetUtil.GetJsonString(this.mResultJson, "key");
					CryptoUtility.code = jsonString;
					this.mResultParamJson = this.mResultJson;
					string jsonString2 = NetUtil.GetJsonString(this.mResultJson, "param");
					CPlusPlusLink instance = CPlusPlusLink.Instance;
					if (instance != null && jsonString2 != null)
					{
						global::Debug.Log("CPlusPlusLink.DecodeServerResponseText");
						responseText = instance.DecodeServerResponseText(jsonString2);
						this.mResultParamJson = JsonMapper.ToObject(responseText);
						global::Debug.Log("DecryptString = " + responseText);
						CryptoUtility.code = jsonString2.Substring(0, 16);
						flag = true;
					}
				}
				if (!flag)
				{
					this.mResultParamJson = NetUtil.GetJsonObject(this.mResultJson, "param");
				}
				this.AnalyazeCommonParam(false);
				if (this.IsMaintenance())
				{
					return;
				}
				ServerInterface.StatusCode resultStCd = this.resultStCd;
				switch (resultStCd + 20132)
				{
				case ServerInterface.StatusCode.Ok:
				case (ServerInterface.StatusCode)1:
				case (ServerInterface.StatusCode)2:
					break;
				default:
					switch (resultStCd + 10106)
					{
					case ServerInterface.StatusCode.Ok:
					case (ServerInterface.StatusCode)1:
					case (ServerInterface.StatusCode)2:
						break;
					default:
						if (resultStCd != ServerInterface.StatusCode.AlreadySentEnergy && resultStCd != ServerInterface.StatusCode.AlreadyRequestedEnergy && resultStCd != ServerInterface.StatusCode.AlreadyRemovedPrePurchase && resultStCd != ServerInterface.StatusCode.AlreadyExistedPrePurchase && resultStCd != ServerInterface.StatusCode.VersionForApplication && resultStCd != ServerInterface.StatusCode.ReceiveFailureMessage && resultStCd != ServerInterface.StatusCode.AlreadyInvitedFriend && resultStCd != ServerInterface.StatusCode.RouletteUseLimit && resultStCd != ServerInterface.StatusCode.EnergyLimitPurchaseTrigger && resultStCd != ServerInterface.StatusCode.CharacterLevelLimit)
						{
							if (resultStCd == ServerInterface.StatusCode.TimeOut)
							{
								this.state = NetBase.emState.UnavailableFailed;
								this.result = -7;
								return;
							}
							if (resultStCd != ServerInterface.StatusCode.Ok)
							{
								this.state = NetBase.emState.UnavailableFailed;
								return;
							}
							if (this.mResultParamJson != null)
							{
								this.DoDidSuccess(this.mResultParamJson);
							}
							goto IL_27E;
						}
						break;
					}
					break;
				}
				this.state = NetBase.emState.AvailableFailed;
				if (this.IsEscapeErrorMode())
				{
					this.DoEscapeErrorMode(this.mResultParamJson);
				}
				return;
			}
		}
		IL_27E:
		if (this.responseEvent != null)
		{
			this.responseEvent(this);
		}
	}

	public void DidFailure(WWW www, bool timeOut, bool notReachability)
	{
		if (notReachability)
		{
			global::Debug.LogWarning("!!!!!!!!!!!!!!!!!!!!!!!!!!! " + this + ".DidFailure : NotReachability", DebugTraceManager.TraceType.SERVER);
			this.resultStCd = ServerInterface.StatusCode.NotReachability;
		}
		else if (timeOut)
		{
			global::Debug.LogWarning("!!!!!!!!!!!!!!!!!!!!!!!!!!! " + this + ".DidFailure : TimeOut", DebugTraceManager.TraceType.SERVER);
			this.resultStCd = ServerInterface.StatusCode.TimeOut;
		}
		else if (www.error != null)
		{
			global::Debug.LogWarning(string.Concat(new object[]
			{
				"!!!!!!!!!!!!!!!!!!!!!!!!!!! ",
				this,
				".DidFailure : ",
				www.error
			}), DebugTraceManager.TraceType.SERVER);
			bool flag = www.error.Contains("400") || www.error.Contains("401") || www.error.Contains("402") || www.error.Contains("403") || www.error.Contains("404") || www.error.Contains("405") || www.error.Contains("406") || www.error.Contains("407") || www.error.Contains("408") || www.error.Contains("409") || www.error.Contains("410") || www.error.Contains("411") || www.error.Contains("412") || www.error.Contains("413") || www.error.Contains("414") || www.error.Contains("415");
			bool flag2 = www.error.Contains("502") || www.error.Contains("503") || www.error.Contains("504");
			bool flag3 = www.error.Contains("500") || www.error.Contains("501") || www.error.Contains("505");
			if (flag)
			{
				this.resultStCd = ServerInterface.StatusCode.CliendError;
			}
			else if (flag3)
			{
				this.resultStCd = ServerInterface.StatusCode.InternalServerError;
			}
			else if (flag2)
			{
				this.resultStCd = ServerInterface.StatusCode.ServerBusy;
			}
			else if (www.error.Contains("unreachable"))
			{
				this.resultStCd = ServerInterface.StatusCode.NotReachability;
			}
			else
			{
				this.resultStCd = ServerInterface.StatusCode.NotReachability;
			}
		}
		else
		{
			this.resultStCd = ServerInterface.StatusCode.OtherError;
		}
		this.state = NetBase.emState.UnavailableFailed;
	}

	private void DoEmulation()
	{
		global::Debug.LogWarning(this + ".DidSuccess : Emulation", DebugTraceManager.TraceType.SERVER);
		this.AnalyazeCommonParam(true);
		if (this.resultStCd == ServerInterface.StatusCode.Ok)
		{
			this.DoDidSuccessEmulation();
		}
	}

	private bool IsUndefinedAction(string result)
	{
		return this.enableUndefinedCompare && result.Contains(NetBase.mUndefinedComparer);
	}

	private void OutputResponseText(string text)
	{
		if (text == null)
		{
			return;
		}
	}

	private void AnalyazeCommonParam(bool emulation)
	{
		this.result = 0;
		this.errorMessage = string.Empty;
		this.meintenanceValue = 0;
		this.dataVersion = -1;
		this.meintenanceMessage = string.Empty;
		this.versionString = "1.0.0";
		this.infoVersionString = string.Empty;
		this.versionValue = NetBaseUtil.GetVersionValue(this.versionString);
		this.assetVersionString = "1.0.0";
		this.assetVersionValue = NetBaseUtil.GetVersionValue(this.assetVersionString);
		this.serverTime = 0L;
		this.seqNum = 0uL;
		ServerInterface.LoginState.serverVersionValue = this.versionValue;
		if (!emulation)
		{
			this.result = NetUtil.GetJsonInt(this.mResultParamJson, "statusCode");
			this.errorMessage = NetUtil.GetJsonString(this.mResultParamJson, "errorMessage");
			this.meintenanceValue = NetUtil.GetJsonInt(this.mResultParamJson, "maintenance");
			this.dataVersion = NetUtil.GetJsonInt(this.mResultParamJson, "data_version");
			this.meintenanceMessage = NetUtil.GetJsonString(this.mResultParamJson, "maintenance_text");
			this.versionString = NetUtil.GetJsonString(this.mResultParamJson, "version");
			this.versionValue = NetBaseUtil.GetVersionValue(this.versionString);
			this.assetVersionString = NetUtil.GetJsonString(this.mResultParamJson, "assets_version");
			this.assetVersionValue = NetBaseUtil.GetVersionValue(this.assetVersionString);
			this.infoVersionString = NetUtil.GetJsonString(this.mResultParamJson, "info_version");
			this.serverTime = NetUtil.GetJsonLong(this.mResultParamJson, "server_time");
			string jsonString = NetUtil.GetJsonString(this.mResultParamJson, this.m_netBaseStr + "Url");
			string jsonString2 = NetUtil.GetJsonString(this.mResultParamJson, this.m_netBaseStr + this.m_eMsgStr);
			string jsonString3 = NetUtil.GetJsonString(this.mResultParamJson, this.m_netBaseStr + this.m_jMsgStr);
			this.seqNum = (ulong)NetUtil.GetJsonLong(this.mResultParamJson, "seq");
			this.clientDataVersion = NetUtil.GetJsonInt(this.mResultParamJson, "client_data_version");
			NetBase.LastNetServerTime = this.serverTime;
			NetBase.TimeDifference = this.serverTime - (long)NetUtil.GetCurrentUnixTime();
			ServerInterface.LoginState.seqNum = this.seqNum;
			ServerInterface.LoginState.DataVersion = this.dataVersion;
			ServerInterface.LoginState.AssetsVersion = this.assetVersionValue;
			ServerInterface.LoginState.AssetsVersionString = this.assetVersionString;
			ServerInterface.LoginState.InfoVersionString = this.infoVersionString;
			ServerInterface.NextVersionState.m_buyRSRNum = NetUtil.GetJsonInt(this.mResultParamJson, "numBuyRedRingsANDROID");
			ServerInterface.NextVersionState.m_freeRSRNum = NetUtil.GetJsonInt(this.mResultParamJson, "numRedRingsANDROID");
			ServerInterface.NextVersionState.m_userName = NetUtil.GetJsonString(this.mResultParamJson, "userName");
			ServerInterface.NextVersionState.m_url = jsonString;
			ServerInterface.NextVersionState.m_eMsg = jsonString2;
			ServerInterface.NextVersionState.m_jMsg = jsonString3;
		}
		else
		{
			this.serverTime = (long)NetUtil.GetCurrentUnixTime();
			NetBase.LastNetServerTime = this.serverTime;
			NetBase.TimeDifference = this.serverTime - (long)NetUtil.GetCurrentUnixTime();
		}
		if (this.result != 0)
		{
			global::Debug.LogWarning(string.Concat(new object[]
			{
				">>>>>>>>>>>>> ",
				this.ToString(),
				" : Result = ",
				this.result,
				" <<<<<<<<<<<<<"
			}));
		}
	}

	public bool IsExecuting()
	{
		return this.state == NetBase.emState.Executing;
	}

	public bool IsSucceeded()
	{
		return this.state == NetBase.emState.Succeeded && !this.IsMaintenance();
	}

	public bool IsFailed()
	{
		return this.state == NetBase.emState.AvailableFailed || this.state == NetBase.emState.UnavailableFailed;
	}

	public bool IsAvailableFailed()
	{
		return this.state == NetBase.emState.AvailableFailed;
	}

	public bool IsUnavailableFailed()
	{
		return this.state == NetBase.emState.UnavailableFailed;
	}

	public bool IsNotReachability()
	{
		return this.IsFailed() && this.resultStCd == ServerInterface.StatusCode.NotReachability;
	}

	public bool IsNoAccessTimeOut()
	{
		return this.IsFailed() && (this.resultStCd == ServerInterface.StatusCode.ExpirationSession || this.resultStCd == ServerInterface.StatusCode.TimeOut);
	}

	public bool IsNeededLoginFailed()
	{
		if (this.IsFailed())
		{
		}
		return false;
	}

	public bool IsMaintenance()
	{
		return 0 != this.meintenanceValue;
	}

	protected void SetAction(string action)
	{
		this.mActionName = action + "/?";
	}

	protected void WriteActionParamValue(string propertyName, object value)
	{
		NetUtil.WriteValue(this.paramWriter, propertyName, value);
	}

	protected void WriteActionParamObject(string objectName, Dictionary<string, object> properties)
	{
		NetUtil.WriteObject(this.paramWriter, objectName, properties);
	}

	protected void WriteActionParamArray(string arrayName, List<object> objects)
	{
		NetUtil.WriteArray(this.paramWriter, arrayName, objects);
	}

	protected void WriteJsonString(string jsonString)
	{
		this.mJsonFromDll = jsonString;
	}

	protected void DisplayDebugInfo()
	{
		this.mDebugLogDisplayLevel = 0;
		this.DebugLogInner(this.mResultParamJson);
	}

	private void DebugLogInner(JsonData jdata)
	{
		this.mDebugLogDisplayLevel++;
		string text = string.Empty;
		for (int i = 0; i < this.mDebugLogDisplayLevel; i++)
		{
			text += "  ";
		}
		string text2 = string.Empty;
		string text3 = string.Empty;
		int count = jdata.Count;
		for (int j = 0; j < count; j++)
		{
			JsonData jsonData = jdata[j];
			if (jsonData.IsArray)
			{
				text2 = jdata.GetKey(j);
				global::Debug.Log(text + "ARRAY  key[" + text2 + "]", DebugTraceManager.TraceType.SERVER);
			}
			else if (jsonData.IsInt)
			{
				text2 = jdata.GetKey(j);
				text3 = (string)jdata[j];
				global::Debug.Log(string.Concat(new string[]
				{
					text,
					"INT    key[",
					text2,
					"]  value[]",
					text3
				}), DebugTraceManager.TraceType.SERVER);
			}
			else if (jsonData.IsLong)
			{
				text2 = jdata.GetKey(j);
				text3 = (string)jdata[j];
				global::Debug.Log(string.Concat(new string[]
				{
					text,
					"LONG   key[",
					text2,
					"]  value[]",
					text3
				}), DebugTraceManager.TraceType.SERVER);
			}
			else if (jsonData.IsObject)
			{
				global::Debug.Log(string.Concat(new object[]
				{
					text,
					"OBJECT[",
					j,
					"]"
				}), DebugTraceManager.TraceType.SERVER);
			}
			else if (jsonData.IsString)
			{
				text2 = jdata.GetKey(j);
				text3 = (string)jdata[j];
				global::Debug.Log(string.Concat(new string[]
				{
					text,
					"STRING :  key[",
					text2,
					"]  value[",
					text3,
					"]"
				}), DebugTraceManager.TraceType.SERVER);
			}
		}
		this.mDebugLogDisplayLevel--;
	}

	protected bool GetFlag(JsonData jdata, string key)
	{
		return NetUtil.GetJsonInt(jdata, key) != 0;
	}

	protected virtual bool IsEscapeErrorMode()
	{
		return false;
	}

	protected virtual void DoEscapeErrorMode(JsonData jdata)
	{
	}

	protected abstract void DoRequest();

	protected abstract void DoDidSuccess(JsonData jdata);

	protected abstract void DoDidSuccessEmulation();

	public static DateTime GetCurrentTime()
	{
		return NetUtil.GetLocalDateTime((long)NetUtil.GetCurrentUnixTime() + NetBase.TimeDifference);
	}
}
