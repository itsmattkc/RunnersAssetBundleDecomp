using System;
using UnityEngine;

public class MotorShot : MonoBehaviour
{
	private enum State
	{
		Idle,
		Shot,
		After
	}

	public class ShotParam
	{
		public GameObject m_obj;

		public float m_gravity;

		public float m_rot_speed;

		public float m_rot_downspeed;

		public Vector3 m_rot_angle = Vector3.zero;

		public Quaternion m_shot_rotation = Quaternion.identity;

		public float m_shot_time;

		public float m_shot_speed;

		public float m_shot_downspeed;

		public bool m_bound;

		public float m_bound_pos_y;

		public float m_bound_add_y;

		public float m_bound_down_x;

		public float m_bound_down_y;

		public float m_after_speed;

		public float m_after_add_x;

		public Vector3 m_after_up = Vector3.zero;

		public Vector3 m_after_forward = Vector3.zero;
	}

	private float m_time;

	private float m_rot_speed;

	private float m_add_x;

	private float m_add_y;

	private float m_shot_speed;

	private bool m_jump;

	private bool m_bound;

	private MotorShot.State m_state;

	private MotorShot.ShotParam m_param;

	public void Setup(MotorShot.ShotParam param)
	{
		this.m_param = param;
		this.m_time = 0f;
		this.m_rot_speed = param.m_rot_speed;
		this.m_add_x = param.m_after_add_x;
		this.m_add_y = 0f;
		this.m_shot_speed = param.m_shot_speed;
		this.m_jump = false;
		this.m_bound = false;
		if (this.m_param.m_shot_time > 0f)
		{
			this.m_state = MotorShot.State.Shot;
		}
		else
		{
			this.m_state = MotorShot.State.After;
		}
	}

	public void SetEnd()
	{
		this.m_param = null;
		this.m_state = MotorShot.State.Idle;
	}

	private void Update()
	{
		if (this.m_param != null)
		{
			float deltaTime = Time.deltaTime;
			MotorShot.State state = this.m_state;
			if (state != MotorShot.State.Shot)
			{
				if (state == MotorShot.State.After)
				{
					this.UpdateAfter(deltaTime, this.m_param.m_obj);
					this.UpdateRot(deltaTime, this.m_param.m_obj);
				}
			}
			else
			{
				this.UpdateShot(deltaTime, this.m_param.m_obj);
				this.UpdateRot(deltaTime, this.m_param.m_obj);
			}
		}
	}

	private void UpdateShot(float delta, GameObject obj)
	{
		if (obj)
		{
			this.m_time += delta;
			if (this.m_time > this.m_param.m_shot_time)
			{
				this.m_time = 0f;
				this.m_jump = false;
				this.m_bound = false;
				this.m_state = MotorShot.State.After;
			}
			else
			{
				Vector3 a = this.m_param.m_shot_rotation * Vector3.up * this.m_shot_speed;
				Vector3 b = a * delta;
				Vector3 vector = obj.transform.position + b;
				float num = this.m_shot_speed * delta * this.m_param.m_shot_downspeed;
				this.m_shot_speed -= num;
				if (num < 0f)
				{
					this.m_shot_speed = 0f;
					this.m_param.m_shot_time = 0f;
				}
				if (this.m_param.m_bound)
				{
					vector = this.SetBound(delta, vector);
				}
				obj.transform.position = vector;
			}
		}
	}

	private void UpdateAfter(float delta, GameObject obj)
	{
		if (obj)
		{
			float num = delta * this.m_param.m_after_speed;
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
			Vector3 vector = obj.transform.position + this.m_param.m_after_up * d2 + this.m_param.m_after_forward * d;
			if (this.m_param.m_bound)
			{
				vector = this.SetBound(delta, vector);
			}
			obj.transform.position = vector;
		}
	}

	private Vector3 SetBound(float delta, Vector3 pos)
	{
		Vector3 result = pos;
		if (result.y < this.m_param.m_bound_pos_y)
		{
			result.y = this.m_param.m_bound_pos_y;
			if (this.m_param.m_bound_add_y > 0f)
			{
				if (!this.m_bound)
				{
					this.m_add_y = this.m_param.m_bound_add_y;
				}
				else
				{
					this.m_add_y = Mathf.Max(this.m_add_y - this.m_add_y * this.m_param.m_bound_down_y, 0f);
				}
				this.m_add_x = Mathf.Max(this.m_add_x - this.m_add_x * this.m_param.m_bound_down_x, 0f);
				this.m_time = 0f;
				this.m_bound = true;
				this.m_jump = true;
				if (this.m_state == MotorShot.State.Shot)
				{
					this.m_state = MotorShot.State.After;
				}
			}
			else if (this.m_state == MotorShot.State.Shot)
			{
				this.m_state = MotorShot.State.Idle;
			}
		}
		else if (this.m_bound)
		{
			this.m_add_x = Mathf.Max(this.m_add_x - delta * this.m_add_x * 0.01f, 0f);
			this.m_add_y = Mathf.Max(this.m_add_y - delta * this.m_add_y * 0.01f, 0f);
		}
		return result;
	}

	private void UpdateRot(float delta, GameObject obj)
	{
		if (obj)
		{
			float d = 60f * delta * this.m_rot_speed;
			obj.transform.rotation = Quaternion.Euler(d * this.m_param.m_rot_angle) * obj.transform.rotation;
			this.m_rot_speed = Mathf.Max(this.m_rot_speed - delta * this.m_param.m_rot_downspeed, 0f);
		}
	}
}
