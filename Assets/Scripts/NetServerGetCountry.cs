using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetCountry : NetBase
{
	private int _resultCountryId_k__BackingField;

	private string _resultCountryCode_k__BackingField;

	public int resultCountryId
	{
		get;
		set;
	}

	public string resultCountryCode
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/getCountry");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_Country(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_Country(JsonData jdata)
	{
		this.resultCountryId = NetUtil.GetJsonInt(jdata, "countryId");
		this.resultCountryCode = NetUtil.GetJsonString(jdata, "countryCode");
	}
}
