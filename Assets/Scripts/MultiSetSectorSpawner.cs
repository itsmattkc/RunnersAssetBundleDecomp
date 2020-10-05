using System;
using UnityEngine;

public class MultiSetSectorSpawner : SpawnableBehavior
{
	[SerializeField]
	private MultiSetSectorParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		MultiSetSectorParameter multiSetSectorParameter = srcParameter as MultiSetSectorParameter;
		if (multiSetSectorParameter != null)
		{
			this.m_parameter = multiSetSectorParameter;
			GameObject @object = multiSetSectorParameter.m_object;
			MultiSetSector component = base.GetComponent<MultiSetSector>();
			if (@object != null && component != null)
			{
				component.Setup();
				int count = multiSetSectorParameter.m_count;
				float radius = multiSetSectorParameter.m_radius;
				float angle = multiSetSectorParameter.m_angle;
				if (angle > 0f)
				{
					float num = base.transform.eulerAngles.z * 2f;
					float num2 = (180f - (angle + num)) * 0.5f;
					float num3 = angle / ((float)count - 1f);
					for (int i = 0; i < count; i++)
					{
						float f = 0.0174532924f * (num3 * (float)i + num2);
						float x = radius * Mathf.Cos(f);
						float y = radius * Mathf.Sin(f);
						Vector3 b = new Vector3(x, y, 0f);
						Vector3 pos = base.transform.position + b;
						component.AddObject(@object, pos, Quaternion.identity);
					}
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.m_parameter.m_radius);
		if (ObjUtil.IsUseTemporarySet())
		{
			GameObject @object = this.m_parameter.m_object;
			if (@object != null)
			{
				SphereCollider component = @object.GetComponent<SphereCollider>();
				BoxCollider component2 = @object.GetComponent<BoxCollider>();
				int count = this.m_parameter.m_count;
				float radius = this.m_parameter.m_radius;
				float angle = this.m_parameter.m_angle;
				if (angle > 0f)
				{
					float num = base.transform.eulerAngles.z * 2f;
					float num2 = (180f - (angle + num)) * 0.5f;
					float num3 = angle / ((float)count - 1f);
					for (int i = 0; i < count; i++)
					{
						float f = 0.0174532924f * (num3 * (float)i + num2);
						float x = radius * Mathf.Cos(f);
						float y = radius * Mathf.Sin(f);
						Vector3 b = new Vector3(x, y, 0f);
						Vector3 a = base.transform.position + b;
						Gizmos.color = Color.green;
						if (component != null)
						{
							Gizmos.DrawWireSphere(a + component.center, component.radius);
						}
						else if (component2 != null)
						{
							Gizmos.DrawWireCube(a + component2.center, component2.size);
						}
					}
				}
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
