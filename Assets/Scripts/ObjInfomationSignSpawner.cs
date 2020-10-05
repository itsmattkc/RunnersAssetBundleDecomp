using System;
using UnityEngine;

public class ObjInfomationSignSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjInfomationSignParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjInfomationSignParameter objInfomationSignParameter = srcParameter as ObjInfomationSignParameter;
		if (objInfomationSignParameter != null)
		{
			GameObject infomationObject = objInfomationSignParameter.m_infomationObject;
			ObjInfomationSign component = base.GetComponent<ObjInfomationSign>();
			if (infomationObject != null && component != null)
			{
				if (component.InfomationObject != null)
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(infomationObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.parent = base.transform;
					gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
					component.InfomationObject = gameObject;
				}
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
