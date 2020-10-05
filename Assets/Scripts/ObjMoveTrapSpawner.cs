using System;
using UnityEngine;

public class ObjMoveTrapSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjMoveTrapParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjMoveTrapParameter objMoveTrapParameter = srcParameter as ObjMoveTrapParameter;
		if (objMoveTrapParameter != null)
		{
			ObjMoveTrap component = base.GetComponent<ObjMoveTrap>();
			if (component)
			{
				component.SetObjMoveTrapParameter(objMoveTrapParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
