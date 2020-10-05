using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Component/MotorAnimalFly")]
public class MotorAnimalFly : MonoBehaviour
{
	private const float UP_SPEED = 2f;

	private float m_moveSpeed;

	private float m_moveDistance;

	private float m_groundDistance;

	private float m_addSpeedX;

	private Vector3 m_angleX = Vector3.zero;

	private Vector3 m_center_position = Vector3.zero;

	private float m_time;

	private bool m_hitCheck;

	private bool m_setup;

	private void Update()
	{
		if (this.m_setup)
		{
			float deltaTime = Time.deltaTime;
			this.m_time += deltaTime;
			float num = Mathf.Sin(6.28318548f * this.m_time * this.m_moveSpeed);
			float d = this.m_moveDistance * num;
			float d2 = this.m_addSpeedX * this.m_time;
			base.transform.position = this.m_center_position + base.transform.up * d + this.m_angleX * d2;
			if (this.m_hitCheck)
			{
				Vector3 zero = Vector3.zero;
				if (ObjUtil.CheckGroundHit(this.m_center_position, base.transform.up, 1f, this.m_moveDistance + this.m_groundDistance, out zero))
				{
					this.m_center_position += Vector3.up * deltaTime * 2f;
				}
			}
		}
	}

	public void SetupParam(float speed, float distance, float add_speed_x, Vector3 angle_x, float ground_distance, bool hitCheck)
	{
		this.m_moveSpeed = speed;
		this.m_moveDistance = distance;
		this.m_addSpeedX = add_speed_x;
		this.m_angleX = angle_x;
		float d = 0f;
		SphereCollider component = base.GetComponent<SphereCollider>();
		if (component != null)
		{
			d = component.radius;
		}
		this.m_center_position = base.transform.position + Vector3.up * d;
		this.m_groundDistance = ground_distance;
		this.m_hitCheck = hitCheck;
		this.m_setup = true;
		this.m_time = 0f;
	}

	public void SetEnd()
	{
		this.m_setup = false;
	}
}
