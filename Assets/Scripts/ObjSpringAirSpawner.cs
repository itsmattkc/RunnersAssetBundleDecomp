using System;
using UnityEngine;

public class ObjSpringAirSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjSpringAirParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjSpringAirParameter objSpringAirParameter = srcParameter as ObjSpringAirParameter;
		if (objSpringAirParameter != null)
		{
			ObjSpringAir component = base.GetComponent<ObjSpringAir>();
			if (component)
			{
				component.SetObjSpringAirParameter(objSpringAirParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
