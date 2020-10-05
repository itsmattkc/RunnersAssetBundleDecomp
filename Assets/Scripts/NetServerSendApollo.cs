using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerSendApollo : NetBase
{
	private int _type_k__BackingField;

	private string[] _value_k__BackingField;

	public int type
	{
		get;
		set;
	}

	public string[] value
	{
		get;
		set;
	}

	public NetServerSendApollo() : this(-1, null)
	{
	}

	public NetServerSendApollo(int type, string[] value)
	{
		this.type = type;
		this.value = value;
	}

	protected override void DoRequest()
	{
		base.SetAction("Sgn/sendApollo");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			List<string> list = new List<string>();
			if (this.value != null)
			{
				string[] value = this.value;
				for (int i = 0; i < value.Length; i++)
				{
					string item = value[i];
					list.Add(item);
				}
			}
			string sendApolloString = instance.GetSendApolloString(this.type, list);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(sendApolloString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Data()
	{
		base.WriteActionParamValue("type", this.type);
		if (this.value != null && this.value.Length != 0)
		{
			base.WriteActionParamArray("value", new List<object>(this.value));
		}
	}
}
