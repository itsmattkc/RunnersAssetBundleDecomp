using System;
using UnityEngine;

public class CameraState
{
	protected CameraParameter m_param;

	private CameraType m_type;

	public CameraState(CameraType type)
	{
		this.m_param = default(CameraParameter);
		this.m_param.m_upDirection = Vector3.up;
		this.m_type = type;
	}

	public virtual void OnEnter(CameraManager manager)
	{
	}

	public virtual void OnLeave(CameraManager manager)
	{
	}

	public virtual void Update(CameraManager manager, float deltaTime)
	{
	}

	public void GetCameraParameter(ref CameraParameter parameter)
	{
		parameter.m_target = this.m_param.m_target;
		parameter.m_position = this.m_param.m_position;
		parameter.m_upDirection = this.m_param.m_upDirection;
	}

	public void SetCameraParameter(CameraParameter param)
	{
		this.m_param.m_target = param.m_target;
		this.m_param.m_position = param.m_position;
		this.m_param.m_upDirection = param.m_upDirection;
	}

	public CameraType GetCameraType()
	{
		return this.m_type;
	}

	public virtual void OnDrawGizmos(CameraManager manager)
	{
	}
}
