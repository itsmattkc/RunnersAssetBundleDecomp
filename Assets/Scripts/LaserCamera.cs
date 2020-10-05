using System;
using UnityEngine;

public class LaserCamera : CameraState
{
	private enum Mode
	{
		Stay,
		MoveFast,
		MoveSlow,
		MoveConst
	}

	private LaserCamera.Mode m_mode;

	private Vector3 m_playerPosition;

	public LaserCamera() : base(CameraType.LASER)
	{
	}

	public override void OnEnter(CameraManager manager)
	{
		base.SetCameraParameter(manager.GetParameter());
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		this.m_playerPosition = playerInformation.Position;
		this.m_mode = LaserCamera.Mode.Stay;
	}

	public override void Update(CameraManager manager, float deltaTime)
	{
		Camera component = manager.GetComponent<Camera>();
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		Vector3 playerPosition = this.m_playerPosition;
		Vector3 position = playerInformation.Position;
		this.m_playerPosition = position;
		Vector3 position2 = component.WorldToViewportPoint(position);
		CameraManager.LaserEditParameter laserParameter = manager.LaserParameter;
		float upScrollViewPort = laserParameter.m_upScrollViewPort;
		float downScrollViewPort = laserParameter.m_downScrollViewPort;
		if (position2.y > upScrollViewPort)
		{
			position2.y = upScrollViewPort;
		}
		if (position2.y < downScrollViewPort)
		{
			position2.y = downScrollViewPort;
		}
		switch (this.m_mode)
		{
		case LaserCamera.Mode.Stay:
			if (position2.x > laserParameter.m_rightScrollViewPort)
			{
				this.m_mode = LaserCamera.Mode.MoveFast;
			}
			return;
		case LaserCamera.Mode.MoveFast:
		{
			float num = (position2.x - laserParameter.m_leftScrollViewPort) / laserParameter.m_fastScrollTime;
			position2.x -= num * deltaTime;
			if (position2.x < laserParameter.m_leftScrollViewPort)
			{
				position2.x = laserParameter.m_leftScrollViewPort;
				this.m_mode = LaserCamera.Mode.MoveConst;
			}
			break;
		}
		case LaserCamera.Mode.MoveSlow:
		{
			float fastScrollTime = laserParameter.m_fastScrollTime;
			position2.x += fastScrollTime * deltaTime;
			if (position2.x > laserParameter.m_rightScrollViewPort)
			{
				this.m_mode = LaserCamera.Mode.MoveFast;
			}
			break;
		}
		case LaserCamera.Mode.MoveConst:
			position2.x = laserParameter.m_leftScrollViewPort;
			break;
		}
		Vector3 b = component.ViewportToWorldPoint(position2);
		Vector3 b2 = (this.m_mode != LaserCamera.Mode.MoveConst) ? (this.m_playerPosition - b) : Vector3.zero;
		Vector3 target = this.m_param.m_target + (this.m_playerPosition - playerPosition) + b2;
		this.m_param.m_target = target;
		this.m_param.m_position = this.m_param.m_target + manager.GetTargetToCamera();
	}

	public override void OnDrawGizmos(CameraManager manager)
	{
		Gizmos.DrawRay(this.m_playerPosition, this.m_param.m_upDirection * 0.5f);
	}
}
