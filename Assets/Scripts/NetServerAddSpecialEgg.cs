using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerAddSpecialEgg : NetBase
{
	private int _numSpecialEgg_k__BackingField;

	private int _resultSpecialEgg_k__BackingField;

	public int numSpecialEgg
	{
		get;
		set;
	}

	public int resultSpecialEgg
	{
		get;
		set;
	}

	public NetServerAddSpecialEgg() : this(0)
	{
	}

	public NetServerAddSpecialEgg(int numSpecialEgg)
	{
		this.numSpecialEgg = numSpecialEgg;
		this.resultSpecialEgg = 0;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/addSpecialEgg");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string addSpecialEggString = instance.GetAddSpecialEggString(this.numSpecialEgg);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(addSpecialEggString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_Data(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Data()
	{
		base.WriteActionParamValue("numSpecialEgg", this.numSpecialEgg);
	}

	private void GetResponse_Data(JsonData jdata)
	{
		this.resultSpecialEgg = NetUtil.GetJsonInt(jdata, "numSpecialEgg");
	}
}
