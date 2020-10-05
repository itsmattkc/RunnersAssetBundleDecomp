using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerSetBirthday : NetBase
{
	private string _birthday_k__BackingField;

	public string birthday
	{
		get;
		set;
	}

	public NetServerSetBirthday() : this(string.Empty)
	{
	}

	public NetServerSetBirthday(string birthday)
	{
		this.birthday = birthday;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/setBirthday");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setBirthdayString = instance.GetSetBirthdayString(this.birthday);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(setBirthdayString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		ServerInterface.SettingState.m_birthday = NetUtil.GetJsonString(jdata, "birthday");
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Birthday()
	{
		base.WriteActionParamValue("birthday", this.birthday);
	}
}
