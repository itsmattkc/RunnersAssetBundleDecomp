using System;
using UnityEngine;

public class GimmickCameraBase : CameraState
{
	protected enum Mode
	{
		Idle,
		Wait,
		Move
	}

	protected struct GimmickCameraParameter
	{
		public float m_waitTime;

		public float m_upScrollViewPort;

		public float m_leftScrollViewPort;

		public float m_depthScrollViewPort;

		public float m_scrollTime;
	}

	protected GimmickCameraBase.Mode m_mode;

	private GimmickCameraBase.GimmickCameraParameter m_gimmickCameraParam = default(GimmickCameraBase.GimmickCameraParameter);

	private float m_time;

	private Vector3 m_speed;

	private Vector3 m_playerPosition;

	private Camera m_camera;

	public GimmickCameraBase(CameraType type) : base(type)
	{
	}

	public override void OnEnter(CameraManager manager)
	{
		base.SetCameraParameter(manager.GetParameter());
		this.m_gimmickCameraParam = this.GetGimmickCameraParameter(manager);
		this.m_mode = GimmickCameraBase.Mode.Wait;
		this.m_time = 0f;
		this.m_speed = Vector3.zero;
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		this.m_playerPosition = playerInformation.Position;
		this.m_camera = manager.GetComponent<Camera>();
	}

	public override void Update(CameraManager manager, float deltaTime)
	{
		PlayerInformation playerInformation = manager.GetPlayerInformation();
		Vector3 playerPosition = this.m_playerPosition;
		Vector3 position = playerInformation.Position;
		this.m_playerPosition = position;
		Vector3 target = this.m_param.m_target + (this.m_playerPosition - playerPosition);
		if (this.m_camera)
		{
			Vector3 viewPort = this.m_camera.WorldToViewportPoint(position);
			this.UpdateGimmickCamera(manager, deltaTime, viewPort, ref target);
			this.m_param.m_target = target;
			this.m_param.m_position = this.m_param.m_target + manager.GetTargetToCamera();
		}
	}

	public override void OnDrawGizmos(CameraManager manager)
	{
		Gizmos.DrawRay(this.m_playerPosition, this.m_param.m_upDirection * 0.5f);
	}

	protected virtual GimmickCameraBase.GimmickCameraParameter GetGimmickCameraParameter(CameraManager manager)
	{
		return this.m_gimmickCameraParam;
	}

	private void UpdateGimmickCamera(CameraManager manager, float deltaTime, Vector3 viewPort, ref Vector3 nowTarget)
	{
		GimmickCameraBase.Mode mode = this.m_mode;
		if (mode != GimmickCameraBase.Mode.Wait)
		{
			if (mode == GimmickCameraBase.Mode.Move)
			{
				viewPort.x = GimmickCameraBase.UpdateScroll(viewPort.x, this.m_gimmickCameraParam.m_leftScrollViewPort, this.m_speed.x * deltaTime);
				viewPort.y = GimmickCameraBase.UpdateScroll(viewPort.y, this.m_gimmickCameraParam.m_upScrollViewPort, this.m_speed.y * deltaTime);
				viewPort.z = GimmickCameraBase.UpdateScroll(viewPort.z, this.m_gimmickCameraParam.m_depthScrollViewPort, this.m_speed.z * deltaTime);
				nowTarget = this.GetNowTarget(viewPort, nowTarget);
				this.m_time += deltaTime;
				if (this.m_time > this.m_gimmickCameraParam.m_scrollTime)
				{
					this.m_mode = GimmickCameraBase.Mode.Idle;
				}
			}
		}
		else
		{
			this.m_time += deltaTime;
			if (this.m_time > this.m_gimmickCameraParam.m_waitTime)
			{
				this.m_speed.x = GimmickCameraBase.GetSpeed(viewPort.x, this.m_gimmickCameraParam.m_leftScrollViewPort, this.m_gimmickCameraParam.m_scrollTime, 0.01f);
				this.m_speed.y = GimmickCameraBase.GetSpeed(viewPort.y, this.m_gimmickCameraParam.m_upScrollViewPort, this.m_gimmickCameraParam.m_scrollTime, 0.01f);
				this.m_speed.z = GimmickCameraBase.GetSpeed(viewPort.z, this.m_gimmickCameraParam.m_depthScrollViewPort, this.m_gimmickCameraParam.m_scrollTime, 0.01f);
				this.m_mode = GimmickCameraBase.Mode.Move;
			}
		}
	}

	private Vector3 GetNowTarget(Vector3 viewPort, Vector3 nowTarget)
	{
		if (this.m_camera)
		{
			Vector3 b = this.m_camera.ViewportToWorldPoint(viewPort);
			return nowTarget + (this.m_playerPosition - b);
		}
		return nowTarget;
	}

	private static float GetSpeed(float value, float tgt_value, float time, float minmax)
	{
		float value2 = (value - tgt_value) / time;
		return GimmickCameraBase.GetMinMax(value2, minmax);
	}

	private static float GetMinMax(float value, float minmax)
	{
		float num = Mathf.Abs(value);
		float b = Mathf.Abs(minmax);
		num = Mathf.Max(num, b);
		return (value >= 0f) ? num : (-num);
	}

	private static float UpdateScroll(float value, float tgt_value, float speed)
	{
		float num = value - speed;
		if (speed < 0f)
		{
			if (num > tgt_value)
			{
				num = tgt_value;
			}
		}
		else if (num < tgt_value)
		{
			num = tgt_value;
		}
		return num;
	}
}
