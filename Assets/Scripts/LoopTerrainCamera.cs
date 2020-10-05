using System;

public class LoopTerrainCamera : GimmickCameraBase
{
	public LoopTerrainCamera() : base(CameraType.LOOP_TERRAIN)
	{
	}

	protected override GimmickCameraBase.GimmickCameraParameter GetGimmickCameraParameter(CameraManager manager)
	{
		CameraManager.LoopTerrainEditParameter loopTerrainParameter = manager.LoopTerrainParameter;
		return new GimmickCameraBase.GimmickCameraParameter
		{
			m_waitTime = loopTerrainParameter.m_waitTime,
			m_upScrollViewPort = loopTerrainParameter.m_upScrollViewPort,
			m_leftScrollViewPort = loopTerrainParameter.m_leftScrollViewPort,
			m_depthScrollViewPort = loopTerrainParameter.m_depthScrollViewPort,
			m_scrollTime = loopTerrainParameter.m_scrollTime
		};
	}

	public override void Update(CameraManager manager, float deltaTime)
	{
		if (this.m_mode != GimmickCameraBase.Mode.Idle)
		{
			base.Update(manager, deltaTime);
		}
	}
}
