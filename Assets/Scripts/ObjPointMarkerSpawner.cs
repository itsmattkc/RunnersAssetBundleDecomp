using System;
using UnityEngine;

public class ObjPointMarkerSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjPointMarkerParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjPointMarkerParameter objPointMarkerParameter = srcParameter as ObjPointMarkerParameter;
		if (objPointMarkerParameter != null)
		{
			ObjPointMarker component = base.GetComponent<ObjPointMarker>();
			if (component)
			{
				component.SetType((PointMarkerType)objPointMarkerParameter.m_type);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
