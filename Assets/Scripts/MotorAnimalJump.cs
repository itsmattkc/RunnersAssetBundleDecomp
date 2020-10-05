using System;
using UnityEngine;

public class MotorAnimalJump : MonoBehaviour
{
	public struct JumpParam
	{
		public GameObject m_obj;

		public float m_speed;

		public float m_gravity;

		public float m_add_x;

		public Vector3 m_up;

		public Vector3 m_forward;

		public bool m_bound;

		public float m_bound_add_y;

		public float m_bound_down_x;

		public float m_bound_down_y;
	}

	private const float HitLength = 1f;

	private float m_time;

	private bool m_jump = true;

	private float m_add_x;

	private float m_add_y;

	private bool m_setup;

	private MotorAnimalJump.JumpParam m_param;

	public void Setup(ref MotorAnimalJump.JumpParam param)
	{
		this.m_param = param;
		this.m_add_x = param.m_add_x;
		this.m_add_y = this.m_param.m_bound_add_y;
		this.m_jump = true;
		this.m_time = 0f;
		this.m_setup = true;
	}

	public void SetEnd()
	{
		this.m_setup = false;
	}

	private void Update()
	{
		if (this.m_setup)
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
			Vector3 vector = obj.transform.position + this.m_param.m_up * d2 + this.m_param.m_forward * d;
			if (this.m_param.m_bound)
			{
				Vector3 zero = Vector3.zero;
				if (ObjUtil.CheckGroundHit(vector, this.m_param.m_up, 1f, 1f, out zero))
				{
					vector.y = zero.y;
					this.m_add_y = Mathf.Max(this.m_add_y - this.m_add_y * this.m_param.m_bound_down_y, 0f);
					this.m_add_x = Mathf.Max(this.m_add_x - this.m_add_x * this.m_param.m_bound_down_x, 0f);
					this.m_jump = true;
					this.m_time = 0f;
				}
				else
				{
					this.m_add_x = Mathf.Max(this.m_add_x - delta * this.m_add_x * 0.01f, 0f);
					this.m_add_y = Mathf.Max(this.m_add_y - delta * this.m_add_y * 0.01f, 0f);
				}
			}
			obj.transform.position = vector;
		}
	}
}
