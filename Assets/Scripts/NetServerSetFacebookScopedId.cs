using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerSetFacebookScopedId : NetBase
{
	private string _paramUserId_k__BackingField;

	public string paramUserId
	{
		get;
		set;
	}

	public NetServerSetFacebookScopedId() : this(string.Empty)
	{
	}

	public NetServerSetFacebookScopedId(string userId)
	{
		this.paramUserId = userId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/setFacebookScopedId");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setFacebookScopedIdString = instance.GetSetFacebookScopedIdString(this.paramUserId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(setFacebookScopedIdString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_UserId()
	{
		base.WriteActionParamValue("facebookId", this.paramUserId);
	}
}
