using System;
using UnityEngine;

public class ObjBossEggmanMap3Spawner : SpawnableBehavior
{
	[SerializeField]
	private ObjBossEggmanMap3Parameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjBossEggmanMap3Parameter objBossEggmanMap3Parameter = srcParameter as ObjBossEggmanMap3Parameter;
		if (objBossEggmanMap3Parameter != null)
		{
			ObjBossEggmanMap3 component = base.GetComponent<ObjBossEggmanMap3>();
			if (component)
			{
				component.SetObjBossEggmanMap3Parameter(objBossEggmanMap3Parameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
