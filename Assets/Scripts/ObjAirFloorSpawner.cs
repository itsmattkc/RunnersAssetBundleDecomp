using System;
using UnityEngine;

public class ObjAirFloorSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjFloorParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjFloorParameter objFloorParameter = srcParameter as ObjFloorParameter;
		if (objFloorParameter != null)
		{
			GameObject modelObject = objFloorParameter.m_modelObject;
			ObjAirFloor component = base.GetComponent<ObjAirFloor>();
			if (modelObject != null && component != null)
			{
				if (component.ModelObject != null)
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(modelObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.parent = base.transform;
					component.ModelObject = gameObject;
					gameObject.layer = LayerMask.NameToLayer("Plane");
				}
				component.Setup(modelObject.name);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
