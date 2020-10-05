using System;
using UnityEngine;

public class ObjEnemySwingSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjEnemySwingParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjEnemySwingParameter objEnemySwingParameter = srcParameter as ObjEnemySwingParameter;
		if (objEnemySwingParameter != null)
		{
			ObjEnemySwing component = base.GetComponent<ObjEnemySwing>();
			if (component)
			{
				component.SetObjEnemySwingParameter(objEnemySwingParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
