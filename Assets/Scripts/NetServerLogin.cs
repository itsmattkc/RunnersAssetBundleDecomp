using App;
using LitJson;
using SaveData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetServerLogin : NetBase
{
	private int m_apolloPlatform2 = 1;

	private string _paramUserId_k__BackingField;

	private string _paramPassword_k__BackingField;

	private string _paramMigrationPassWord_k__BackingField;

	private string _resultSessionId_k__BackingField;

	private long _resultEnergyRefreshTime_k__BackingField;

	private ServerItemState _resultInviteBaseIncentive_k__BackingField;

	private ServerItemState _resultRentalBaseIncentive_k__BackingField;

	private string _userName_k__BackingField;

	private string _userId_k__BackingField;

	private string _password_k__BackingField;

	private string _key_k__BackingField;

	private int _sessionTimeLimit_k__BackingField;

	private int _energyRecoveryMax_k__BackingField;

	public string paramUserId
	{
		get;
		set;
	}

	public string paramPassword
	{
		get;
		set;
	}

	public string paramMigrationPassWord
	{
		get;
		set;
	}

	public string resultSessionId
	{
		get;
		private set;
	}

	public long resultEnergyRefreshTime
	{
		get;
		private set;
	}

	public ServerItemState resultInviteBaseIncentive
	{
		get;
		private set;
	}

	public ServerItemState resultRentalBaseIncentive
	{
		get;
		private set;
	}

	public string userName
	{
		get;
		private set;
	}

	public string userId
	{
		get;
		private set;
	}

	public string password
	{
		get;
		private set;
	}

	public string key
	{
		get;
		private set;
	}

	public int sessionTimeLimit
	{
		get;
		private set;
	}

	public int energyRecoveryMax
	{
		get;
		private set;
	}

	public NetServerLogin() : this(string.Empty, string.Empty)
	{
	}

	public NetServerLogin(string userId, string password)
	{
		this.paramUserId = userId;
		this.paramPassword = password;
		this.paramMigrationPassWord = string.Empty;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/login");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string userId = (this.paramUserId == null) ? string.Empty : this.paramUserId;
			string password = (this.paramPassword == null) ? string.Empty : this.paramPassword;
			string migrationPassword = (this.paramMigrationPassWord == null) ? string.Empty : this.paramMigrationPassWord;
			int platform = 2;
			string device = string.Empty;
			string deviceModel = SystemInfo.deviceModel;
			device = deviceModel.Replace(" ", "_");
			int language = (int)Env.language;
			int salesLocale = 0;
			int storeId = 2;
			int apolloPlatform = this.m_apolloPlatform2;
			string loginString = instance.GetLoginString(userId, password, migrationPassword, platform, device, language, salesLocale, storeId, apolloPlatform);
			base.WriteJsonString(loginString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_SessionId(jdata);
		this.GetResponse_SettingData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultSessionId = "dummy";
	}

	private void SetParameter_LineAuth()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(1);
		dictionary.Add("userId", (this.paramUserId == null) ? string.Empty : this.paramUserId);
		dictionary.Add("password", (this.paramPassword == null) ? string.Empty : this.paramPassword);
		dictionary.Add("migrationPassword", (this.paramMigrationPassWord == null) ? string.Empty : this.paramMigrationPassWord);
		base.WriteActionParamObject("lineAuth", dictionary);
		dictionary.Clear();
	}

	private void SetParameter_Platform()
	{
		int num = 2;
		base.WriteActionParamValue("platform", num);
	}

	private void SetParameter_Device()
	{
		string value = string.Empty;
		string deviceModel = SystemInfo.deviceModel;
		value = deviceModel.Replace(" ", "_");
		base.WriteActionParamValue("device", value);
	}

	private void SetParameter_Language()
	{
		int language = (int)Env.language;
		base.WriteActionParamValue("language", language);
	}

	private void SetParameter_Locate()
	{
		base.WriteActionParamValue("salesLocate", 0);
	}

	private void SetParameter_StoreId()
	{
		int num = 2;
		base.WriteActionParamValue("storeId", num);
	}

	private void SetParameter_SnsPlatform()
	{
		int apolloPlatform = this.m_apolloPlatform2;
		base.WriteActionParamValue("platform_sns", apolloPlatform);
	}

	private void GetResponse_SessionId(JsonData jdata)
	{
		this.resultSessionId = NetUtil.GetJsonString(jdata, "sessionId");
	}

	private void GetResponse_SettingData(JsonData jdata)
	{
		this.resultEnergyRefreshTime = NetUtil.GetJsonLong(jdata, "energyRecoveryTime");
		this.resultInviteBaseIncentive = NetUtil.AnalyzeItemStateJson(jdata, "inviteBasicIncentiv");
		this.resultRentalBaseIncentive = NetUtil.AnalyzeItemStateJson(jdata, "chaoRentalBasicIncentiv");
		this.userName = NetUtil.GetJsonString(jdata, "userName");
		this.sessionTimeLimit = NetUtil.GetJsonInt(jdata, "sessionTimeLimit");
		this.userId = NetUtil.GetJsonString(jdata, "userId");
		this.password = NetUtil.GetJsonString(jdata, "password");
		this.energyRecoveryMax = NetUtil.GetJsonInt(jdata, "energyRecoveryMax");
	}

	protected override bool IsEscapeErrorMode()
	{
		return true;
	}

	protected override void DoEscapeErrorMode(JsonData jdata)
	{
		this.userId = NetUtil.GetJsonString(jdata, "userId");
		this.password = NetUtil.GetJsonString(jdata, "password");
		this.key = NetUtil.GetJsonString(jdata, "key");
		string jsonString = NetUtil.GetJsonString(jdata, "countryCode");
		if (!string.IsNullOrEmpty(jsonString))
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			SystemData systemdata = instance.GetSystemdata();
			systemdata.country = jsonString;
			SystemSaveManager.CheckIAPMessage();
		}
	}
}
