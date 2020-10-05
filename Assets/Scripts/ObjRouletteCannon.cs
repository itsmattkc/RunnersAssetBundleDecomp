using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjRouletteCannon")]
public class ObjRouletteCannon : ObjCannon
{
	private const string ModelName = "obj_cmn_roulettecannon";

	private float m_angle;

	protected override string GetModelName()
	{
		return "obj_cmn_roulettecannon";
	}

	protected override string GetShotEffectName()
	{
		return "ef_ob_roulette_canon_st01";
	}

	protected override string GetShotAnimName()
	{
		return "roulettecannon_shot";
	}

	protected override bool IsRoulette()
	{
		return true;
	}

	public void SetObjRouletteCannonParameter(ObjRouletteCannonParameter param)
	{
		this.m_angle = param.angle;
		base.SetObjCannonParameter(new ObjCannonParameter
		{
			firstSpeed = param.firstSpeed,
			outOfcontrol = param.outOfcontrol,
			moveSpeed = param.moveSpeed,
			moveArea = 0f
		});
	}

	protected override Quaternion GetStartRot()
	{
		return Quaternion.Euler(0f, 0f, -this.m_angle) * base.transform.rotation;
	}
}
