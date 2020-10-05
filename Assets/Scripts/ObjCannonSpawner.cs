using System;
using UnityEngine;

public class ObjCannonSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjCannonParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjCannonParameter objCannonParameter = srcParameter as ObjCannonParameter;
		if (objCannonParameter != null)
		{
			ObjCannon component = base.GetComponent<ObjCannon>();
			if (component)
			{
				component.SetObjCannonParameter(objCannonParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
