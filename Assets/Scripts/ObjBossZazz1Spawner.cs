using System;
using UnityEngine;

public class ObjBossZazz1Spawner : SpawnableBehavior
{
	[SerializeField]
	private ObjBossZazz1Parameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjBossZazz1Parameter objBossZazz1Parameter = srcParameter as ObjBossZazz1Parameter;
		if (objBossZazz1Parameter != null)
		{
			ObjBossZazz1 component = base.GetComponent<ObjBossZazz1>();
			if (component)
			{
				component.SetObjBossZazz1Parameter(objBossZazz1Parameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
