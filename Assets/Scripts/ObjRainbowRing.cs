using GameScore;
using System;

public class ObjRainbowRing : ObjDashRing
{
	protected override string GetModelName()
	{
		return "obj_cmn_rainbowring";
	}

	protected override string GetEffectName()
	{
		return "ef_ob_pass_rainbowring01";
	}

	protected override string GetSEName()
	{
		return "obj_rainbowring";
	}

	protected override int GetScore()
	{
		return Data.RainbowRing;
	}

	public void SetObjRainbowRingParameter(ObjRainbowRingParameter param)
	{
		base.SetObjDashRingParameter(new ObjDashRingParameter
		{
			firstSpeed = param.firstSpeed,
			outOfcontrol = param.outOfcontrol
		});
	}
}
