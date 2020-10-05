using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjSpringAir")]
public class ObjSpringAir : ObjSpring
{
	private const string ModelName = "obj_cmn_springAir";

	protected override string GetModelName()
	{
		return "obj_cmn_springAir";
	}

	protected override string GetMotionName()
	{
		return "obj_springAir_bounce";
	}

	public void SetObjSpringAirParameter(ObjSpringAirParameter param)
	{
		base.SetObjSpringParameter(new ObjSpringParameter
		{
			firstSpeed = param.firstSpeed,
			outOfcontrol = param.outOfcontrol
		});
	}
}
