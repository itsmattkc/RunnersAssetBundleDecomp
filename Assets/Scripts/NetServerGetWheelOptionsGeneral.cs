using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetWheelOptionsGeneral : NetBase
{
	public int paramEventId;

	public int paramSpinId;

	private ServerWheelOptionsGeneral _resultWheelOptionsGeneral_k__BackingField;

	public ServerWheelOptionsGeneral resultWheelOptionsGeneral
	{
		get;
		private set;
	}

	public NetServerGetWheelOptionsGeneral(int eventId, int spinId)
	{
		this.paramEventId = eventId;
		this.paramSpinId = spinId;
	}

	protected override void DoRequest()
	{
		base.SetAction("RaidbossSpin/getRaidbossWheelOptions");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getWheelSpinGeneralString = instance.GetGetWheelSpinGeneralString(this.paramEventId, this.paramSpinId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getWheelSpinGeneralString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_WheelOptionsGeneral(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
		base.WriteActionParamValue("id", this.paramSpinId);
	}

	private void GetResponse_WheelOptionsGeneral(JsonData jdata)
	{
		this.resultWheelOptionsGeneral = NetUtil.AnalyzeWheelOptionsGeneralJson(jdata, "raidbossWheelOptions");
	}
}
