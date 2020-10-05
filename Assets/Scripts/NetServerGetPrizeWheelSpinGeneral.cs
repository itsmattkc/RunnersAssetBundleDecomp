using LitJson;
using System;

public class NetServerGetPrizeWheelSpinGeneral : NetBase
{
	public int paramEventId;

	public int paramSpinType;

	public ServerPrizeState resultPrizeState;

	public NetServerGetPrizeWheelSpinGeneral(int eventId, int spinType)
	{
		this.paramEventId = eventId;
		this.paramSpinType = spinType;
	}

	protected override void DoRequest()
	{
		base.SetAction("RaidbossSpin/getPrizeRaidbossWheelSpin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getPrizeWheelSpinGeneralString = instance.GetGetPrizeWheelSpinGeneralString(this.paramEventId, this.paramSpinType);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getPrizeWheelSpinGeneralString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PrizeWheelSpinGeneral(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_WheelSpinGeneral()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
		base.WriteActionParamValue("raidbossWheelSpinType", this.paramSpinType);
	}

	private void GetResponse_PrizeWheelSpinGeneral(JsonData jdata)
	{
		this.resultPrizeState = NetUtil.AnalyzePrizeWheelSpinGeneral(jdata);
	}
}
