using App;
using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterInput : MonoBehaviour
	{
		private enum TouchedStatus
		{
			touched,
			hold,
			released
		}

		private enum Status
		{
			STATUS_STICKENABLED,
			STATUS_DISABLE
		}

		private struct History
		{
			public Bitset32 touchedStatus;

			public float time;
		}

		private Bitset32 m_status;

		private Bitset32 m_touchedStatus;

		private Vector3 m_stick;

		private byte m_historyIndex;

		private CharacterInput.History[] m_history;

		private LevelInformation m_levelInformation;

		private void Start()
		{
			this.m_status.Set(0, true);
			this.m_stick = Vector3.zero;
			if (this.m_levelInformation == null)
			{
				this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
			}
		}

		private void Update()
		{
			if (App.Math.NearZero(Time.deltaTime, 1E-06f) || App.Math.NearZero(Time.timeScale, 1E-06f))
			{
				return;
			}
			if (this.m_levelInformation.RequestPause)
			{
				return;
			}
			if (this.m_levelInformation.RequestEqitpItem)
			{
				return;
			}
			if (this.m_levelInformation.RequestCharaChange)
			{
				return;
			}
			this.m_touchedStatus.Reset();
			bool flag = !this.m_status.Test(1);
			if (flag)
			{
				if (this.m_status.Test(0))
				{
					float axis = UnityEngine.Input.GetAxis("Vertical");
					float axis2 = UnityEngine.Input.GetAxis("Horizontal");
					Vector3 stick = Camera.main.transform.right * axis2 + Camera.main.transform.up * axis;
					this.m_stick = stick;
				}
				else
				{
					this.m_stick = Vector3.zero;
				}
				if (Input.GetMouseButtonDown(0) || (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began))
				{
					this.m_touchedStatus.Set(0, true);
					this.m_touchedStatus.Set(1, true);
				}
				else if (Input.GetMouseButton(0) || (UnityEngine.Input.touchCount > 0 && (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved || UnityEngine.Input.GetTouch(0).phase == TouchPhase.Stationary)))
				{
					this.m_touchedStatus.Set(1, true);
				}
				if (Input.GetMouseButtonUp(0) || (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended))
				{
					this.m_touchedStatus.Set(2, true);
				}
			}
			if (this.m_history != null)
			{
				this.m_historyIndex += 1;
				this.m_history[(int)this.m_historyIndex].touchedStatus = this.m_touchedStatus;
				this.m_history[(int)this.m_historyIndex].time = Time.deltaTime;
			}
		}

		public Vector3 GetStick()
		{
			return this.m_stick;
		}

		public bool IsTouched()
		{
			return this.m_touchedStatus.Test(0);
		}

		public bool IsHold()
		{
			return this.m_touchedStatus.Test(1);
		}

		public bool IsReleased()
		{
			return this.m_touchedStatus.Test(2);
		}

		public void CreateHistory()
		{
			this.m_history = new CharacterInput.History[256];
			this.m_historyIndex = 0;
		}

		public bool IsTouchedLastSecond(float lastSecond)
		{
			float num = 0f;
			byte historyIndex = this.m_historyIndex;
			while (num < lastSecond)
			{
				if (this.m_history[(int)historyIndex].touchedStatus.Test(0))
				{
					return true;
				}
				if (this.m_history[(int)historyIndex].time <= 0f)
				{
					break;
				}
				num += this.m_history[(int)historyIndex].time;
			}
			return false;
		}

		private void OnInputDisable(MsgDisableInput msg)
		{
			this.m_status.Set(1, msg.m_disable);
		}
	}
}
