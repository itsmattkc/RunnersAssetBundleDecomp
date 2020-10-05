using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Component/MotorSwing")]
public class MotorSwing : MonoBehaviour
{
	private float m_moveSpeed;

	private float m_moveDistanceX;

	private float m_moveDistanceY;

	private Vector3 m_center_position = Vector3.zero;

	private Vector3 m_angle_x = Vector3.zero;

	private float m_time;

	private void Start()
	{
		this.m_center_position = base.transform.position;
	}

	private void Update()
	{
		if (this.m_moveSpeed > 0f)
		{
			this.m_time += Time.deltaTime;
			float num = Mathf.Sin(6.28318548f * this.m_time * this.m_moveSpeed);
			float d = this.m_moveDistanceX * num;
			float d2 = this.m_moveDistanceY * num;
			Vector3 b = base.transform.up * d2 + this.m_angle_x * d;
			base.transform.position = this.m_center_position + b;
		}
	}

	public void SetParam(float speed, float x, float y, Vector3 agl)
	{
		this.m_moveSpeed = speed;
		this.m_moveDistanceX = x;
		this.m_moveDistanceY = y;
		this.m_angle_x = agl;
	}
}
