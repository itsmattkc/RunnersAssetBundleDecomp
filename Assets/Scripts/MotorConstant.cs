using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Component/MotorConstant")]
public class MotorConstant : MonoBehaviour
{
	private enum State
	{
		Wait,
		Move,
		Idle
	}

	private const string m_anim_name = "move";

	private float m_moveSpeed;

	private float m_moveDistance;

	private float m_startMoveDistance;

	private MotorConstant.State m_state;

	private float m_addDistance;

	private PlayerInformation m_playerInfo;

	private Animator m_animator;

	private Vector3 m_angle = Vector3.zero;

	private string m_move_SEName = string.Empty;

	private string m_move_SECatName = string.Empty;

	private uint m_move_SEID;

	private void Start()
	{
		this.m_playerInfo = ObjUtil.GetPlayerInformation();
		this.m_animator = base.GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case MotorConstant.State.Wait:
			this.UpdateWait();
			break;
		case MotorConstant.State.Move:
			this.UpdateMove();
			break;
		}
	}

	private void OnDestroy()
	{
		this.SetMoveSE(false);
	}

	public void SetParam(float speed, float dst, float start_dst, Vector3 agl, string se_category, string se_name)
	{
		this.m_moveSpeed = speed;
		this.m_moveDistance = dst;
		this.m_startMoveDistance = start_dst;
		this.m_angle = agl;
		this.m_move_SECatName = se_category;
		this.m_move_SEName = se_name;
	}

	private void UpdateWait()
	{
		float playerDistance = this.GetPlayerDistance();
		if (!this.m_moveSpeed.Equals(0f) && this.m_moveDistance > 0f && playerDistance < this.m_startMoveDistance)
		{
			this.SetMoveAnimation(true);
			this.SetMoveSE(true);
			this.m_state = MotorConstant.State.Move;
		}
	}

	private void UpdateMove()
	{
		float num = this.m_moveSpeed * Time.deltaTime;
		base.transform.position += this.m_angle * num;
		this.m_addDistance += Mathf.Abs(num);
		if (this.m_addDistance >= this.m_moveDistance)
		{
			this.SetMoveAnimation(false);
			this.SetMoveSE(false);
			this.m_state = MotorConstant.State.Idle;
		}
	}

	private float GetPlayerDistance()
	{
		if (this.m_playerInfo)
		{
			Vector3 position = base.transform.position;
			return Mathf.Abs(Vector3.Distance(position, this.m_playerInfo.Position));
		}
		return 0f;
	}

	private void SetMoveAnimation(bool flag)
	{
		if (this.m_animator)
		{
			this.m_animator.SetBool("move", flag);
		}
	}

	private void SetMoveSE(bool flag)
	{
		if (!this.m_move_SEName.Equals(string.Empty) && !this.m_move_SECatName.Equals(string.Empty))
		{
			if (flag)
			{
				if (this.m_move_SEID == 0u)
				{
					this.m_move_SEID = (uint)ObjUtil.LightPlaySE(this.m_move_SEName, this.m_move_SECatName);
				}
			}
			else if (this.m_move_SEID != 0u)
			{
				ObjUtil.StopSE((SoundManager.PlayId)this.m_move_SEID);
				this.m_move_SEID = 0u;
			}
		}
	}
}
