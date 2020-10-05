using System;
using UnityEngine;

public class StartCamera : CameraState
{
	private enum Type
	{
		NEAR,
		BACK,
		FAR
	}

	private StartCamera.Type m_type;

	private Vector3 m_nearTargetOffset;

	private Vector3 m_nearCameraOffset;

	private Vector3 m_playerPosition;

	private float m_timer;

	private float m_rate;

	private float m_perRate;

	public StartCamera() : base(CameraType.START_ACT)
	{
	}

	public override void OnEnter(CameraManager manager)
	{
		base.SetCameraParameter(manager.GetParameter());
		this.m_nearTargetOffset = manager.StartActParameter.m_targetOffset;
		this.m_nearCameraOffset = manager.StartActParameter.m_cameraOffset;
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		this.m_playerPosition = playerInformation.Position;
		this.m_type = StartCamera.Type.NEAR;
		this.m_rate = 0f;
		this.m_perRate = 0f;
		this.m_timer = manager.StartActParameter.m_nearStayTime;
	}

	public override void Update(CameraManager manager, float deltaTime)
	{
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		Vector3 position = playerInformation.Position;
		this.m_playerPosition.x = position.x;
		this.m_playerPosition.y = position.y;
		this.m_playerPosition.z = 0f;
		Vector3 vector = this.m_playerPosition + this.m_nearTargetOffset;
		Vector3 vector2 = vector + this.m_nearCameraOffset;
		vector2.z -= vector.z;
		Vector3 vector3 = vector;
		Vector3 vector4 = vector2;
		CameraState cameraByType = manager.GetCameraByType(CameraType.DEFAULT);
		if (cameraByType != null)
		{
			CameraParameter param = this.m_param;
			cameraByType.GetCameraParameter(ref param);
			vector3 = param.m_target;
			vector4 = param.m_position;
		}
		switch (this.m_type)
		{
		case StartCamera.Type.NEAR:
			this.m_param.m_target = vector;
			this.m_param.m_position = vector2;
			this.m_timer -= deltaTime;
			if (this.m_timer < 0f)
			{
				if (manager.StartActParameter.m_nearToFarTime > 0f)
				{
					this.m_rate = 0f;
					this.m_perRate = 1f / manager.StartActParameter.m_nearToFarTime;
					this.m_type = StartCamera.Type.BACK;
				}
				else
				{
					this.m_type = StartCamera.Type.FAR;
				}
			}
			break;
		case StartCamera.Type.BACK:
			this.m_rate += this.m_perRate * deltaTime;
			if (this.m_rate > 1f)
			{
				this.m_param.m_target = vector3;
				this.m_param.m_position = vector4;
				this.m_type = StartCamera.Type.FAR;
			}
			else
			{
				this.m_param.m_target = Vector3.Lerp(vector, vector3, this.m_rate);
				this.m_param.m_position = Vector3.Lerp(vector2, vector4, this.m_rate);
			}
			break;
		case StartCamera.Type.FAR:
			this.m_param.m_target = vector3;
			this.m_param.m_position = vector4;
			break;
		}
	}
}
