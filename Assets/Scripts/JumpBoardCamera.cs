using System;

public class JumpBoardCamera : GimmickCameraBase
{
	public JumpBoardCamera() : base(CameraType.JUMPBOARD)
	{
	}

	protected override GimmickCameraBase.GimmickCameraParameter GetGimmickCameraParameter(CameraManager manager)
	{
		CameraManager.JumpBoardEditParameter jumpBoardParameter = manager.JumpBoardParameter;
		return new GimmickCameraBase.GimmickCameraParameter
		{
			m_waitTime = jumpBoardParameter.m_waitTime,
			m_upScrollViewPort = jumpBoardParameter.m_upScrollViewPort,
			m_leftScrollViewPort = jumpBoardParameter.m_leftScrollViewPort,
			m_depthScrollViewPort = jumpBoardParameter.m_depthScrollViewPort,
			m_scrollTime = jumpBoardParameter.m_scrollTime
		};
	}
}
