using Message;
using System;
using UnityEngine;

public class GameFollowCamera : CameraState
{
	private const float ZOffset = 0f;

	private const float PathPosSensitive = 6f;

	private const float downScrollLineSensitive = 2f;

	private const float pathDistanceSensitive = 0.5f;

	private Vector3 m_defaultTargetOffset;

	private Vector3 m_playerPosition;

	private float m_distPathToTarget;

	private Vector3 m_sideViewPos;

	private float m_downScrollLine;

	public GameFollowCamera() : base(CameraType.DEFAULT)
	{
	}

	public override void OnEnter(CameraManager manager)
	{
		base.SetCameraParameter(manager.GetParameter());
		this.m_defaultTargetOffset = manager.GetTargetOffset();
		this.m_downScrollLine = manager.EditParameter.m_downScrollLine;
		this.ResetParameters(manager);
	}

	public override void Update(CameraManager manager, float deltaTime)
	{
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		Vector3 vector = playerInformation.SideViewPathPos;
		Vector3 position = playerInformation.Position;
		float num = manager.m_startCameraPos.y + 1f;
		float y = vector.y;
		float y2 = position.y;
		vector = Vector3.Lerp(this.m_sideViewPos, vector, 6f * deltaTime);
		float num2 = y2 - y;
		float num3 = Vector3.Dot(playerInformation.VerticalVelocity, -playerInformation.GravityDir);
		float to = manager.EditParameter.m_downScrollLine;
		if (!playerInformation.IsOnGround() && num3 < -2f && num2 > num)
		{
			to = manager.EditParameter.m_downScrollLineOnDown;
		}
		this.m_downScrollLine = Mathf.Lerp(this.m_downScrollLine, to, 2f * deltaTime);
		float num4 = manager.EditParameter.m_upScrollLine + this.m_distPathToTarget;
		float num5 = this.m_downScrollLine + this.m_distPathToTarget;
		float upScrollLimit = manager.EditParameter.m_upScrollLimit;
		float downScrollLimit = manager.EditParameter.m_downScrollLimit;
		if (num2 > num4)
		{
			this.m_distPathToTarget += num2 - num4;
		}
		if (num2 < num5)
		{
			this.m_distPathToTarget += num2 - num5;
		}
		if (num2 < num && num2 > 0f)
		{
			this.m_distPathToTarget = Mathf.Lerp(this.m_distPathToTarget, manager.m_startCameraPos.y, 0.5f * deltaTime);
		}
		this.m_distPathToTarget = Mathf.Clamp(this.m_distPathToTarget, downScrollLimit, upScrollLimit);
		this.m_playerPosition.x = position.x;
		this.m_playerPosition.y = vector.y + this.m_distPathToTarget;
		this.m_playerPosition.z = 0f;
		this.m_param.m_target.x = this.m_playerPosition.x + this.m_defaultTargetOffset.x;
		this.m_param.m_target.y = this.m_playerPosition.y + this.m_defaultTargetOffset.y;
		this.m_param.m_target.z = 0f;
		this.m_param.m_position = this.m_param.m_target + manager.GetTargetToCamera();
		this.m_param.m_target.z = 0f;
		this.m_sideViewPos = vector;
	}

	public override void OnDrawGizmos(CameraManager manager)
	{
		Gizmos.DrawRay(this.m_playerPosition, this.m_param.m_upDirection * 0.5f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(this.m_playerPosition, 0.5f);
		Vector3 center = new Vector3(this.m_playerPosition.x, this.m_sideViewPos.y + this.m_distPathToTarget, this.m_playerPosition.z);
		center.y = this.m_sideViewPos.y + this.m_distPathToTarget + manager.EditParameter.m_upScrollLine;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(center, 0.5f);
		center.y = this.m_sideViewPos.y + this.m_distPathToTarget + this.m_downScrollLine;
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(center, 0.5f);
	}

	private void ResetParameters(CameraManager manager)
	{
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		this.m_playerPosition = playerInformation.Position;
		float y = playerInformation.Position.y;
		float y2 = playerInformation.SideViewPathPos.y;
		this.m_distPathToTarget = y - y2;
		this.m_sideViewPos = playerInformation.SideViewPathPos;
	}

	public void OnMsgTutorialResetForRetry(CameraManager manager, MsgTutorialResetForRetry msg)
	{
		this.ResetParameters(manager);
	}
}
