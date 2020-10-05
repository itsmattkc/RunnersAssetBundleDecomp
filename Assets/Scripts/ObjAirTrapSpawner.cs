using System;
using UnityEngine;

public class ObjAirTrapSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjAirTrapParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjAirTrapParameter objAirTrapParameter = srcParameter as ObjAirTrapParameter;
		if (objAirTrapParameter != null)
		{
			ObjAirTrap component = base.GetComponent<ObjAirTrap>();
			if (component)
			{
				component.SetObjAirTrapParameter(objAirTrapParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
