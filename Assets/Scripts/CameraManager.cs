using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[Serializable]
	public class CameraEditParameter
	{
		public float m_upScrollLine = 3f;

		public float m_downScrollLine = -1f;

		public float m_upScrollLimit = 10f;

		public float m_downScrollLimit = -5f;

		public float m_downScrollLineOnDown = 2f;
	}

	[Serializable]
	public class LaserEditParameter
	{
		public float m_upScrollViewPort = 0.8f;

		public float m_downScrollViewPort = 0.2f;

		public float m_rightScrollViewPort = 0.758f;

		public float m_leftScrollViewPort = 0.1f;

		public float m_fastScrollTime = 0.2f;

		public float m_slowScrollSpeed = 0.5f;
	}

	[Serializable]
	public class JumpBoardEditParameter
	{
		public float m_waitTime;

		public float m_upScrollViewPort = 0.6f;

		public float m_leftScrollViewPort = 0.2f;

		public float m_depthScrollViewPort = 26f;

		public float m_scrollTime = 0.5f;
	}

	[Serializable]
	public class CannonEditParameter
	{
		public float m_waitTime;

		public float m_upScrollViewPort = 0.3f;

		public float m_leftScrollViewPort = 0.2f;

		public float m_depthScrollViewPort = 26f;

		public float m_scrollTime = 0.5f;
	}

	[Serializable]
	public class LoopTerrainEditParameter
	{
		public float m_waitTime;

		public float m_upScrollViewPort = 0.3f;

		public float m_leftScrollViewPort = 0.1f;

		public float m_depthScrollViewPort = 18f;

		public float m_scrollTime;
	}

	[Serializable]
	public class StartActEditParameter
	{
		public Vector3 m_cameraOffset = new Vector3(0f, 0f, -10f);

		public Vector3 m_targetOffset = new Vector3(0f, 0.5f, 0f);

		public float m_nearStayTime = 2.5f;

		public float m_nearToFarTime = 1f;
	}

	private PlayerInformation m_playerInfo;

	private Vector3 m_defaultTargetOffset;

	private Vector3 m_playerPosition;

	private float m_prevDistanceGround;

	private Vector3 m_targetToCamera;

	private Vector3 m_differencePos;

	private Vector3 m_differenceTarget;

	private bool m_differenceApproachFlag;

	private List<CameraState> m_cameraList = new List<CameraState>();

	private float m_interpolateTime;

	private float m_interpolateRate;

	private float m_ratePerSec;

	private CameraParameter m_param = default(CameraParameter);

	private CameraParameter m_topParam = default(CameraParameter);

	private CameraParameter m_lowerParam = default(CameraParameter);

	public Vector3 m_startCameraPos = new Vector3(4.5f, 2f, -18f);

	public CameraManager.CameraEditParameter m_editParameter = new CameraManager.CameraEditParameter();

	public CameraManager.LaserEditParameter m_laserParameter = new CameraManager.LaserEditParameter();

	public CameraManager.JumpBoardEditParameter m_jumpBoardParameter = new CameraManager.JumpBoardEditParameter();

	public CameraManager.CannonEditParameter m_cannonParameter = new CameraManager.CannonEditParameter();

	public CameraManager.LoopTerrainEditParameter m_loopTerrainParameter = new CameraManager.LoopTerrainEditParameter();

	public CameraManager.StartActEditParameter m_startActParameter = new CameraManager.StartActEditParameter();

	[SerializeField]
	private bool m_drawInfo;

	[SerializeField]
	private bool m_debugInterpolate;

	[SerializeField]
	private float m_debugPushInterpolateTime = 0.5f;

	[SerializeField]
	private float m_debugPopInterpolateTime = 0.5f;

	private Rect m_window;

	public CameraManager.CameraEditParameter EditParameter
	{
		get
		{
			return this.m_editParameter;
		}
	}

	public CameraManager.LaserEditParameter LaserParameter
	{
		get
		{
			return this.m_laserParameter;
		}
	}

	public CameraManager.JumpBoardEditParameter JumpBoardParameter
	{
		get
		{
			return this.m_jumpBoardParameter;
		}
	}

	public CameraManager.CannonEditParameter CannonParameter
	{
		get
		{
			return this.m_cannonParameter;
		}
	}

	public CameraManager.LoopTerrainEditParameter LoopTerrainParameter
	{
		get
		{
			return this.m_loopTerrainParameter;
		}
	}

	public CameraManager.StartActEditParameter StartActParameter
	{
		get
		{
			return this.m_startActParameter;
		}
	}

	private void Start()
	{
		Camera camera = GetComponent<Camera>();
		float num = camera.fieldOfView;
		ScreenType screenType = ScreenUtil.GetScreenType();
		float num2 = 1.5f;
		if (screenType == ScreenType.iPhone5)
		{
			float num3 = 1.77777779f;
			num /= num3 / num2;
			camera.fieldOfView = num;
		}
		else if (screenType == ScreenType.iPad)
		{
			float num4 = 1.33333337f;
			num /= num4 / num2;
			camera.fieldOfView = num;
		}
		this.m_playerInfo = GameObject.Find("PlayerInformation").GetComponent<PlayerInformation>();
		this.m_playerPosition = this.m_playerInfo.Position;
		base.transform.position = this.m_startCameraPos;
		this.m_param.m_target = base.transform.position;
		this.m_param.m_target.z = this.m_playerPosition.z;
		this.m_param.m_position = base.transform.position;
		this.m_param.m_upDirection = Vector3.up;
		this.m_defaultTargetOffset = base.transform.position - this.m_playerPosition;
		this.m_defaultTargetOffset.z = 0f;
		base.transform.position = this.m_param.m_position;
		base.transform.LookAt(this.m_param.m_target, this.m_param.m_upDirection);
		this.m_targetToCamera = this.m_param.m_position - this.m_param.m_target;
		GameFollowCamera state = new GameFollowCamera();
		this.PushCamera(state);
	}

	private void LateUpdate()
	{
		if (this.m_playerInfo == null)
		{
			return;
		}
		if (this.m_playerInfo.IsDead())
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (deltaTime == 0f)
		{
			return;
		}
		foreach (CameraState current in this.m_cameraList)
		{
			current.Update(this, deltaTime);
		}
		int num = this.m_cameraList.Count - 1;
		if (this.IsNowInterpolate())
		{
			this.m_interpolateRate += this.m_ratePerSec * deltaTime;
			if (this.m_interpolateTime > 0f)
			{
				this.m_interpolateTime -= deltaTime;
				if (this.m_interpolateTime <= 0f)
				{
					if (this.m_interpolateRate < 0.5f)
					{
						CameraState state = this.m_cameraList[num];
						this.PopCamera(state);
					}
					this.m_interpolateTime = 0f;
				}
			}
			if (this.m_interpolateRate > 1f)
			{
				this.m_interpolateRate = 1f;
			}
			else if (this.m_interpolateRate < 0f)
			{
				this.m_interpolateRate = 0f;
			}
		}
		num = this.m_cameraList.Count - 1;
		if (this.IsNowInterpolate())
		{
			if (this.m_ratePerSec < 0f)
			{
				CameraState cameraState = this.m_cameraList[num - 1];
				cameraState.GetCameraParameter(ref this.m_lowerParam);
				if (this.m_differenceApproachFlag)
				{
					CameraState cameraState2 = this.m_cameraList[num];
					cameraState2.GetCameraParameter(ref this.m_topParam);
					this.m_differencePos = this.m_lowerParam.m_position - this.m_topParam.m_position;
					this.m_differenceTarget = this.m_lowerParam.m_target - this.m_topParam.m_target;
					this.m_differenceApproachFlag = false;
				}
				float t = Mathf.Max(1f - this.m_interpolateRate, 0f);
				this.m_param.m_position = this.m_lowerParam.m_position - Vector3.Lerp(this.m_differencePos, Vector3.zero, t);
				this.m_param.m_target = this.m_lowerParam.m_target - Vector3.Lerp(this.m_differenceTarget, Vector3.zero, t);
				this.m_param.m_upDirection = Vector3.Lerp(this.m_param.m_upDirection, this.m_lowerParam.m_upDirection, t);
			}
			else
			{
				CameraState cameraState3 = this.m_cameraList[num];
				cameraState3.GetCameraParameter(ref this.m_topParam);
				this.m_param.m_position = Vector3.Lerp(this.m_param.m_position, this.m_topParam.m_position, this.m_interpolateRate);
				this.m_param.m_target = Vector3.Lerp(this.m_param.m_target, this.m_topParam.m_target, this.m_interpolateRate);
				this.m_param.m_upDirection = Vector3.Lerp(this.m_param.m_upDirection, this.m_topParam.m_upDirection, this.m_interpolateRate);
			}
		}
		else
		{
			CameraState cameraState4 = this.m_cameraList[num];
			cameraState4.GetCameraParameter(ref this.m_param);
		}
		base.transform.position = this.m_param.m_position;
		base.transform.LookAt(this.m_param.m_target, this.m_param.m_upDirection);
	}

	private void PushNewCamera(CameraType camType, UnityEngine.Object parameter, float interpolateTime)
	{
		CameraState cameraState = this.CreateNewCamera(camType, parameter);
		if (cameraState != null)
		{
			this.PushCamera(cameraState);
			this.StartInterpolate(true, interpolateTime);
			CameraState prevGimmickCamera = this.GetPrevGimmickCamera();
			if (prevGimmickCamera != null)
			{
				this.PopCamera(prevGimmickCamera);
			}
		}
	}

	private void PopCamera(CameraType camType, float interpolateTime)
	{
		CameraState cameraByType = this.GetCameraByType(camType);
		if (cameraByType != null)
		{
			if (cameraByType == this.GetTopCamera())
			{
				CameraState prevGimmickCamera = this.GetPrevGimmickCamera();
				if (prevGimmickCamera != null)
				{
					this.PopCamera(prevGimmickCamera);
				}
				if (!this.IsNowInterpolate() || this.m_ratePerSec >= 0f)
				{
					this.StartInterpolate(false, interpolateTime);
				}
			}
			else
			{
				this.PopCamera(cameraByType);
			}
		}
	}

	private CameraState CreateNewCamera(CameraType camType, UnityEngine.Object parameter)
	{
		CameraState result = null;
		switch (camType)
		{
		case CameraType.DEFAULT:
			result = new GameFollowCamera();
			break;
		case CameraType.LASER:
			result = new LaserCamera();
			break;
		case CameraType.JUMPBOARD:
			result = new JumpBoardCamera();
			break;
		case CameraType.CANNON:
			result = new CannonCamera();
			break;
		case CameraType.LOOP_TERRAIN:
			result = new LoopTerrainCamera();
			break;
		case CameraType.START_ACT:
			result = new StartCamera();
			break;
		}
		return result;
	}

	public CameraState GetCameraByType(CameraType type)
	{
		foreach (CameraState current in this.m_cameraList)
		{
			if (current.GetCameraType() == type)
			{
				return current;
			}
		}
		return null;
	}

	private CameraState GetTopCamera()
	{
		return this.m_cameraList[this.m_cameraList.Count - 1];
	}

	private void PushCamera(CameraState state)
	{
		state.OnEnter(this);
		this.m_cameraList.Add(state);
	}

	private void PopCamera(CameraState state)
	{
		state.OnLeave(this);
		this.m_cameraList.Remove(state);
	}

	private void StartInterpolate(bool push, float time)
	{
		this.m_interpolateTime = time;
		if (push)
		{
			this.m_interpolateRate = 0f;
			this.m_ratePerSec = 1f / time;
		}
		else
		{
			this.m_interpolateRate = 1f;
			this.m_ratePerSec = -(1f / time);
			this.m_differenceApproachFlag = true;
		}
	}

	private void OnDrawGizmos()
	{
		foreach (CameraState current in this.m_cameraList)
		{
			current.OnDrawGizmos(this);
		}
	}

	private void OnPushCamera(MsgPushCamera msg)
	{
		this.PushNewCamera(msg.m_type, msg.m_parameter, msg.m_interpolateTime);
	}

	private void OnPopCamera(MsgPopCamera msg)
	{
		this.PopCamera(msg.m_type, msg.m_interpolateTime);
	}

	private CameraState GetPrevGimmickCamera()
	{
		CameraState cameraState = null;
		foreach (CameraState current in this.m_cameraList)
		{
			CameraType cameraType = current.GetCameraType();
			if (CameraTypeData.IsGimmickCamera(cameraType))
			{
				if (cameraState != null)
				{
					return cameraState;
				}
				cameraState = current;
			}
		}
		return null;
	}

	public PlayerInformation GetPlayerInformation()
	{
		return this.m_playerInfo;
	}

	public CameraParameter GetParameter()
	{
		return this.m_param;
	}

	public Vector3 GetTargetOffset()
	{
		return this.m_defaultTargetOffset;
	}

	public Vector3 GetTargetToCamera()
	{
		return this.m_targetToCamera;
	}

	private bool IsNowInterpolate()
	{
		return this.m_interpolateTime > 0f && this.m_cameraList.Count > 1;
	}

	public void OnMsgTutorialResetForRetry(MsgTutorialResetForRetry msg)
	{
		while (this.GetTopCamera().GetCameraType() != CameraType.DEFAULT)
		{
			this.PopCamera(this.GetTopCamera());
		}
		GameFollowCamera gameFollowCamera = this.GetCameraByType(CameraType.DEFAULT) as GameFollowCamera;
		if (gameFollowCamera != null)
		{
			gameFollowCamera.OnMsgTutorialResetForRetry(this, msg);
		}
	}

	private void WindowFunction(int windowID)
	{
		string text = string.Empty;
		CameraParameter param = this.m_param;
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"POS  :",
			param.m_position.x.ToString("F2"),
			" ",
			param.m_position.y.ToString("F2"),
			" ",
			param.m_position.z.ToString("F2"),
			" \n"
		});
		text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"TARGET:",
			param.m_target.x.ToString("F2"),
			" ",
			param.m_target.y.ToString("F2"),
			" ",
			param.m_target.z.ToString("F2"),
			" \n"
		});
		text += "\n";
		for (int i = this.m_cameraList.Count - 1; i >= 0; i--)
		{
			CameraState cameraState = this.m_cameraList[i];
			text = text + cameraState.ToString() + ":\n";
			CameraParameter cameraParameter = default(CameraParameter);
			cameraState.GetCameraParameter(ref cameraParameter);
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"POS  :",
				cameraParameter.m_position.x.ToString("F2"),
				" ",
				cameraParameter.m_position.y.ToString("F2"),
				" ",
				cameraParameter.m_position.z.ToString("F2"),
				" \n"
			});
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"TARGET:",
				cameraParameter.m_target.x.ToString("F2"),
				" ",
				cameraParameter.m_target.y.ToString("F2"),
				" ",
				cameraParameter.m_target.z.ToString("F2"),
				" \n"
			});
		}
		if (this.IsNowInterpolate())
		{
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"Interpolate time:",
				this.m_interpolateTime.ToString("F2"),
				" rate:",
				this.m_interpolateRate.ToString("F2")
			});
		}
		GUIContent gUIContent = new GUIContent();
		gUIContent.text = text;
		Rect position = new Rect(10f, 20f, 270f, 280f);
		GUI.Label(position, gUIContent);
	}
}
