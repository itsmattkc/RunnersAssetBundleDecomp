using System;
using UnityEngine;

public class MotorThrow : MonoBehaviour
{
	public class ThrowParam
	{
		public GameObject m_obj;

		public float m_speed;

		public float m_gravity;

		public float m_add_x;

		public float m_add_y;

		public float m_rot_speed;

		public float m_rot_downspeed;

		public Vector3 m_up = Vector3.zero;

		public Vector3 m_forward = Vector3.zero;

		public Vector3 m_rot_angle = Vector3.zero;

		public bool m_bound;

		public float m_bound_pos_y;

		public float m_bound_add_y;

		public float m_bound_down_x;

		public float m_bound_down_y;
	}

	private float m_time;

	private float m_rot_speed;

	private bool m_jump = true;

	private float m_add_x;

	private float m_add_y;

	private bool m_bound;

	private MotorThrow.ThrowParam m_param;

	public void Setup(MotorThrow.ThrowParam param)
	{
		this.m_param = param;
		this.m_rot_speed = param.m_rot_speed;
		this.m_add_x = param.m_add_x;
		this.m_add_y = param.m_add_y;
		this.m_time = 0f;
		this.m_bound = false;
		this.m_jump = true;
	}

	public void SetEnd()
	{
		this.m_param = null;
	}

	private void Update()
	{
		if (this.m_param != null)
		{
			this.UpdateThrow(Time.deltaTime, this.m_param.m_obj);
		}
	}

	private void UpdateThrow(float delta, GameObject obj)
	{
		if (obj)
		{
			float num = delta * this.m_param.m_speed;
			float d;
			float d2;
			if (this.m_jump)
			{
				this.m_time += num;
				d = num * this.m_add_x;
				float num2 = this.m_add_y - this.m_time * -this.m_param.m_gravity * 0.15f;
				if (num2 < 0f)
				{
					num2 = 0f;
					this.m_time = 0f;
					this.m_jump = false;
				}
				d2 = delta * num2 * 3f;
			}
			else
			{
				this.m_time += num;
				float num3 = this.m_add_x - this.m_time * 0.1f;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				d = delta * num3 * 3f;
				d2 = this.m_time * this.m_param.m_gravity * delta;
			}
			Vector3 position = obj.transform.position + this.m_param.m_up * d2 + this.m_param.m_forward * d;
			if (this.m_param.m_bound)
			{
				if (position.y < this.m_param.m_bound_pos_y)
				{
					position.y = this.m_param.m_bound_pos_y;
					if (!this.m_bound)
					{
						this.m_add_y = this.m_param.m_bound_add_y;
					}
					else
					{
						this.m_add_y = Mathf.Max(this.m_add_y - this.m_add_y * this.m_param.m_bound_down_y, 0f);
					}
					this.m_add_x = Mathf.Max(this.m_add_x - this.m_add_x * this.m_param.m_bound_down_x, 0f);
					this.m_bound = true;
					this.m_jump = true;
					this.m_time = 0f;
				}
				else if (this.m_bound)
				{
					this.m_add_x = Mathf.Max(this.m_add_x - delta * this.m_add_x * 0.01f, 0f);
					this.m_add_y = Mathf.Max(this.m_add_y - delta * this.m_add_y * 0.01f, 0f);
				}
			}
			obj.transform.position = position;
			if (this.m_rot_speed > 0f)
			{
				float d3 = 60f * delta * this.m_rot_speed;
				obj.transform.rotation = Quaternion.Euler(d3 * this.m_param.m_rot_angle) * obj.transform.rotation;
				this.m_rot_speed = Mathf.Max(this.m_rot_speed - delta * this.m_param.m_rot_downspeed, 0f);
			}
		}
	}
}
