using System;
using UnityEngine;

public class MultiSetParaloopItemPointCircleSpawner : SpawnableBehavior
{
	[SerializeField]
	private MultiSetParaloopItemPointCircleParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		MultiSetParaloopItemPointCircleParameter multiSetParaloopItemPointCircleParameter = srcParameter as MultiSetParaloopItemPointCircleParameter;
		if (multiSetParaloopItemPointCircleParameter != null)
		{
			this.m_parameter = multiSetParaloopItemPointCircleParameter;
			GameObject @object = multiSetParaloopItemPointCircleParameter.m_object;
			MultiSetParaloopItemPointCircle component = base.GetComponent<MultiSetParaloopItemPointCircle>();
			if (@object != null && component != null)
			{
				if (!ObjUtil.IsUseTemporarySet())
				{
					BoxCollider component2 = base.GetComponent<BoxCollider>();
					if (component2)
					{
						component2.size = multiSetParaloopItemPointCircleParameter.GetSize();
						component2.center = multiSetParaloopItemPointCircleParameter.GetCenter();
					}
				}
				component.Setup();
				component.SetID(multiSetParaloopItemPointCircleParameter.m_tblID);
				component.AddObject(@object, base.transform.position, Quaternion.identity);
			}
		}
	}

	private void OnDrawGizmos()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		if (component)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(base.transform.position + component.center, component.size);
		}
		if (ObjUtil.IsUseTemporarySet())
		{
			Gizmos.DrawWireSphere(base.transform.position, 0.5f);
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
			this.m_parameter.SetCenter(component.center);
		}
		return this.m_parameter;
	}
}
