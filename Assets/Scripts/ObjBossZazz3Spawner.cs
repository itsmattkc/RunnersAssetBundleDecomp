using System;
using UnityEngine;

public class ObjBossZazz3Spawner : SpawnableBehavior
{
	[SerializeField]
	private ObjBossZazz3Parameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjBossZazz3Parameter objBossZazz3Parameter = srcParameter as ObjBossZazz3Parameter;
		if (objBossZazz3Parameter != null)
		{
			ObjBossZazz3 component = base.GetComponent<ObjBossZazz3>();
			if (component)
			{
				component.SetObjBossZazz3Parameter(objBossZazz3Parameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
