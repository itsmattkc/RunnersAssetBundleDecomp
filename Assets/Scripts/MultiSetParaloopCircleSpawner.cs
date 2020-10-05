using System;
using UnityEngine;

public class MultiSetParaloopCircleSpawner : SpawnableBehavior
{
	[SerializeField]
	private MultiSetParaloopCircleParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		MultiSetParaloopCircleParameter multiSetParaloopCircleParameter = srcParameter as MultiSetParaloopCircleParameter;
		if (multiSetParaloopCircleParameter != null)
		{
			this.m_parameter = multiSetParaloopCircleParameter;
			GameObject @object = multiSetParaloopCircleParameter.m_object;
			MultiSetParaloopCircle component = base.GetComponent<MultiSetParaloopCircle>();
			if (@object != null && component != null)
			{
				if (!ObjUtil.IsUseTemporarySet())
				{
					BoxCollider component2 = base.GetComponent<BoxCollider>();
					if (component2)
					{
						component2.size = multiSetParaloopCircleParameter.GetSize();
						component2.center = multiSetParaloopCircleParameter.GetCenter();
					}
				}
				component.Setup();
				int count = multiSetParaloopCircleParameter.m_count;
				float radius = multiSetParaloopCircleParameter.m_radius;
				float num = 360f / (float)count;
				for (int i = 0; i < count; i++)
				{
					float f = 0.0174532924f * (num * (float)i);
					float x = radius * Mathf.Cos(f);
					float y = radius * Mathf.Sin(f);
					Vector3 b = new Vector3(x, y, 0f);
					Vector3 pos = base.transform.position + b;
					component.AddObject(@object, pos, Quaternion.identity);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.m_parameter.m_radius);
		BoxCollider component = base.GetComponent<BoxCollider>();
		if (component)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(base.transform.position + component.center, component.size);
		}
		if (ObjUtil.IsUseTemporarySet())
		{
			GameObject @object = this.m_parameter.m_object;
			if (@object != null)
			{
				SphereCollider component2 = @object.GetComponent<SphereCollider>();
				BoxCollider component3 = @object.GetComponent<BoxCollider>();
				int count = this.m_parameter.m_count;
				float radius = this.m_parameter.m_radius;
				float num = 360f / (float)count;
				for (int i = 0; i < count; i++)
				{
					float f = 0.0174532924f * (num * (float)i);
					float x = radius * Mathf.Cos(f);
					float y = radius * Mathf.Sin(f);
					Vector3 b = new Vector3(x, y, 0f);
					Vector3 a = base.transform.position + b;
					if (component2 != null)
					{
						Gizmos.DrawWireSphere(a + component2.center, component2.radius);
					}
					else if (component3 != null)
					{
						Gizmos.DrawWireCube(a + component3.center, component3.size);
					}
				}
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
			this.m_parameter.SetCenter(component.center);
		}
		return this.m_parameter;
	}
}
