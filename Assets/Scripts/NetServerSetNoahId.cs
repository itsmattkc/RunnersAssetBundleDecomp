using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerSetNoahId : NetBase
{
	private string _noahId_k__BackingField;

	public string noahId
	{
		get;
		set;
	}

	public NetServerSetNoahId() : this(string.Empty)
	{
	}

	public NetServerSetNoahId(string noahId)
	{
		this.noahId = noahId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Sgn/setNoahId");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setNoahIdString = instance.GetSetNoahIdString(this.noahId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(setNoahIdString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_NoahId()
	{
		base.WriteActionParamValue("noahId", this.noahId);
	}
}
