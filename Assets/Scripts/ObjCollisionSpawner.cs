using System;
using UnityEngine;

public class ObjCollisionSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjCollisionParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjCollisionParameter objCollisionParameter = srcParameter as ObjCollisionParameter;
		if (objCollisionParameter != null && !ObjUtil.IsUseTemporarySet())
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			if (component)
			{
				component.size = objCollisionParameter.GetSize();
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
