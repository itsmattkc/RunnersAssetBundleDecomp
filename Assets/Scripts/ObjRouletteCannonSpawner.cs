using System;
using UnityEngine;

public class ObjRouletteCannonSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjRouletteCannonParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjRouletteCannonParameter objRouletteCannonParameter = srcParameter as ObjRouletteCannonParameter;
		if (objRouletteCannonParameter != null)
		{
			ObjRouletteCannon component = base.GetComponent<ObjRouletteCannon>();
			if (component)
			{
				component.SetObjRouletteCannonParameter(objRouletteCannonParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
