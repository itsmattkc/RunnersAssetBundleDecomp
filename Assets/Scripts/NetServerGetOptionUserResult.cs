using LitJson;
using System;

public class NetServerGetOptionUserResult : NetBase
{
	private ServerOptionUserResult m_userResult = new ServerOptionUserResult();

	public ServerOptionUserResult UserResult
	{
		get
		{
			return this.m_userResult;
		}
	}

	protected override void DoRequest()
	{
		base.SetAction("Option/userResult");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_OptionUserResult(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_OptionUserResult(JsonData jdata)
	{
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "optionUserResult");
		if (jsonObject != null)
		{
			this.m_userResult.m_totalSumHightScore = NetUtil.GetJsonLong(jsonObject, "totalSumHightScore");
			this.m_userResult.m_quickTotalSumHightScore = NetUtil.GetJsonLong(jsonObject, "quickTotalSumHightScore");
			this.m_userResult.m_numTakeAllRings = NetUtil.GetJsonLong(jsonObject, "numTakeAllRings");
			this.m_userResult.m_numTakeAllRedRings = NetUtil.GetJsonLong(jsonObject, "numTakeAllRedRings");
			this.m_userResult.m_numChaoRoulette = NetUtil.GetJsonInt(jsonObject, "numChaoRoulette");
			this.m_userResult.m_numItemRoulette = NetUtil.GetJsonInt(jsonObject, "numItemRoulette");
			this.m_userResult.m_numJackPot = NetUtil.GetJsonInt(jsonObject, "numJackPot");
			this.m_userResult.m_numMaximumJackPotRings = NetUtil.GetJsonInt(jsonObject, "numMaximumJackPotRings");
			this.m_userResult.m_numSupport = NetUtil.GetJsonInt(jsonObject, "numSupport");
		}
	}
}
