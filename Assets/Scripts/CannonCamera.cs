using System;

public class CannonCamera : GimmickCameraBase
{
	public CannonCamera() : base(CameraType.CANNON)
	{
	}

	protected override GimmickCameraBase.GimmickCameraParameter GetGimmickCameraParameter(CameraManager manager)
	{
		CameraManager.CannonEditParameter cannonParameter = manager.CannonParameter;
		return new GimmickCameraBase.GimmickCameraParameter
		{
			m_waitTime = cannonParameter.m_waitTime,
			m_upScrollViewPort = cannonParameter.m_upScrollViewPort,
			m_leftScrollViewPort = cannonParameter.m_leftScrollViewPort,
			m_depthScrollViewPort = cannonParameter.m_depthScrollViewPort,
			m_scrollTime = cannonParameter.m_scrollTime
		};
	}
}
