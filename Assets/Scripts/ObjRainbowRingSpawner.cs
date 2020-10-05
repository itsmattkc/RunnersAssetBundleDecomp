using System;
using UnityEngine;

public class ObjRainbowRingSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjRainbowRingParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjRainbowRingParameter objRainbowRingParameter = srcParameter as ObjRainbowRingParameter;
		if (objRainbowRingParameter != null)
		{
			ObjRainbowRing component = base.GetComponent<ObjRainbowRing>();
			if (component)
			{
				component.SetObjRainbowRingParameter(objRainbowRingParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
