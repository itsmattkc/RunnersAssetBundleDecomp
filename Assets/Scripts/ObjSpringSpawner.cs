using System;
using UnityEngine;

public class ObjSpringSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjSpringParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjSpringParameter objSpringParameter = srcParameter as ObjSpringParameter;
		if (objSpringParameter != null)
		{
			ObjSpring component = base.GetComponent<ObjSpring>();
			if (component)
			{
				component.SetObjSpringParameter(objSpringParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
