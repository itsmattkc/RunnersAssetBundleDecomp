using System;
using UnityEngine;

public class ObjBreakSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjBreakParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjBreakParameter objBreakParameter = srcParameter as ObjBreakParameter;
		if (objBreakParameter != null)
		{
			GameObject modelObject = objBreakParameter.m_modelObject;
			ObjBreak component = base.GetComponent<ObjBreak>();
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
				}
				component.SetObjName(modelObject.name);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
