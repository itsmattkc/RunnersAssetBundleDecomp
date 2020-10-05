using System;
using UnityEngine;

public class ObjBossEggmanFeverSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjBossEggmanFeverParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjBossEggmanFeverParameter objBossEggmanFeverParameter = srcParameter as ObjBossEggmanFeverParameter;
		if (objBossEggmanFeverParameter != null)
		{
			ObjBossEggmanFever component = base.GetComponent<ObjBossEggmanFever>();
			if (component)
			{
				component.SetObjBossEggmanFeverParameter(objBossEggmanFeverParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
