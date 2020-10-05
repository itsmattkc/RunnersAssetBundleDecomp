using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Component/MagnetControl")]
public class MagnetControl : MonoBehaviour
{
	private const float MAGNET_SPEED = 0.2f;

	private PlayerInformation m_playerInfo;

	private GameObject m_object;

	private GameObject m_target;

	private float m_time;

	private float m_waitTime;

	private float m_speed;

	private bool m_active;

	private void Start()
	{
		this.m_playerInfo = ObjUtil.GetPlayerInformation();
	}

	private void OnEnable()
	{
		base.enabled = true;
	}

	private void OnDisable()
	{
		this.Reset();
	}

	private void Update()
	{
		if (!this.m_active)
		{
			return;
		}
		if (this.m_object)
		{
			float num = this.m_speed;
			if (this.m_waitTime > 0f)
			{
				this.m_waitTime -= Time.deltaTime;
				num = this.m_speed * 0.1f;
			}
			this.m_time += Time.deltaTime;
			float num2 = 0.1f - this.m_time * num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			Vector3 zero = Vector3.zero;
			Vector3 target = (!(this.m_target != null)) ? this.m_playerInfo.Position : this.m_target.transform.position;
			this.m_object.transform.position = Vector3.SmoothDamp(this.m_object.transform.position, target, ref zero, num2);
		}
	}

	public void OnUseMagnet(MsgUseMagnet msg)
	{
		if (msg.m_obj)
		{
			this.m_object = msg.m_obj;
			this.m_target = msg.m_target;
			this.m_waitTime = msg.m_time;
			this.m_active = true;
			float num = ObjUtil.GetPlayerAddSpeed();
			if (num < 0f)
			{
				num = 0f;
			}
			this.m_speed = 0.2f + 0.02f * num;
		}
	}

	public void Reset()
	{
		this.m_speed = 0f;
		this.m_object = null;
		this.m_target = null;
		this.m_waitTime = 0f;
		this.m_time = 0f;
		this.m_active = false;
		base.enabled = true;
	}
}
