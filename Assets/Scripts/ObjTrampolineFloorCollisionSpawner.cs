using System;
using UnityEngine;

public class ObjTrampolineFloorCollisionSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjTrampolineFloorCollisionParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjTrampolineFloorCollisionParameter objTrampolineFloorCollisionParameter = srcParameter as ObjTrampolineFloorCollisionParameter;
		if (objTrampolineFloorCollisionParameter != null)
		{
			if (!ObjUtil.IsUseTemporarySet())
			{
				BoxCollider component = base.GetComponent<BoxCollider>();
				if (component)
				{
					component.size = objTrampolineFloorCollisionParameter.GetSize();
				}
			}
			ObjTrampolineFloorCollision component2 = base.GetComponent<ObjTrampolineFloorCollision>();
			if (component2)
			{
				component2.SetObjCollisionParameter(objTrampolineFloorCollisionParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}

	public override SpawnableParameter GetParameterForExport()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		if (component)
		{
			this.m_parameter.SetSize(component.size);
		}
		return this.m_parameter;
	}
}
