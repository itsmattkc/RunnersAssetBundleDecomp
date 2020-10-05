using System;
using UnityEngine;

public class ObjLoopTerrainSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjLoopTerrainParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjLoopTerrainParameter objLoopTerrainParameter = srcParameter as ObjLoopTerrainParameter;
		if (objLoopTerrainParameter != null)
		{
			this.m_parameter = objLoopTerrainParameter;
			ObjLoopTerrain component = base.GetComponent<ObjLoopTerrain>();
			if (component != null)
			{
				if (objLoopTerrainParameter.m_pathName.Length > 0)
				{
					component.SetPathName(objLoopTerrainParameter.m_pathName);
				}
				component.SetZOffset(objLoopTerrainParameter.m_pathZOffset);
			}
			BoxCollider component2 = base.GetComponent<BoxCollider>();
			if (component2)
			{
				component2.size = objLoopTerrainParameter.Size;
				component2.center = objLoopTerrainParameter.Center;
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
			this.m_parameter.Size = component.size;
			this.m_parameter.Center = component.center;
		}
		return this.m_parameter;
	}
}
