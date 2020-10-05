using System;
using System.Collections.Generic;
using UnityEngine;

public class TickerWindow : MonoBehaviour
{
	public struct CInfo
	{
		public List<ServerTickerData> tickerList;

		public string labelName;

		public float moveSpeed;

		public float moveSpeedUp;
	}

	private enum Mode
	{
		Idle,
		Start,
		Wait1,
		SpeedUpMove,
		Wait2,
		Move
	}

	private TickerWindow.Mode m_mode;

	private TickerWindow.CInfo m_info = default(TickerWindow.CInfo);

	private UILabel m_uiLabel;

	private float m_textSize;

	private float m_startPos;

	private int m_count;

	private float m_timer;

	private void Update()
	{
		if (this.m_uiLabel != null)
		{
			switch (this.m_mode)
			{
			case TickerWindow.Mode.Start:
				this.m_timer = 1f;
				this.m_mode = TickerWindow.Mode.Wait1;
				break;
			case TickerWindow.Mode.Wait1:
				this.m_timer -= Time.deltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_mode = TickerWindow.Mode.SpeedUpMove;
				}
				break;
			case TickerWindow.Mode.SpeedUpMove:
			{
				this.UpdateMovePos(this.m_info.moveSpeedUp * Time.deltaTime * 60f);
				float x = this.m_uiLabel.transform.localPosition.x;
				float num = 24f;
				if (x < num)
				{
					this.SetResetPos(num);
					this.m_timer = 1f;
					this.m_mode = TickerWindow.Mode.Wait2;
				}
				break;
			}
			case TickerWindow.Mode.Wait2:
				this.m_timer -= Time.deltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_textSize = (float)this.m_uiLabel.width;
					this.m_mode = TickerWindow.Mode.Move;
				}
				break;
			case TickerWindow.Mode.Move:
			{
				this.UpdateMovePos(this.m_info.moveSpeed * Time.deltaTime * 60f);
				float x2 = this.m_uiLabel.transform.localPosition.x;
				float num2 = 0f - this.m_textSize;
				if (x2 < num2)
				{
					this.SetupNext();
					this.m_mode = TickerWindow.Mode.Start;
				}
				break;
			}
			}
		}
	}

	public void Setup(TickerWindow.CInfo info)
	{
		this.m_info = info;
		this.m_count = 0;
		UIPanel component = base.gameObject.GetComponent<UIPanel>();
		if (component != null)
		{
			this.m_uiLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, this.m_info.labelName);
			if (this.m_uiLabel != null && this.m_info.tickerList != null && this.m_info.tickerList.Count > 0)
			{
				this.m_uiLabel.text = this.m_info.tickerList[this.m_count].Param;
				this.m_startPos = component.clipRange.z;
				this.SetResetPos(this.m_startPos);
				this.m_mode = TickerWindow.Mode.Start;
			}
		}
	}

	private void SetupNext()
	{
		if (this.m_info.tickerList != null && this.m_uiLabel != null)
		{
			this.m_count++;
			if (this.m_count >= this.m_info.tickerList.Count)
			{
				this.m_count = 0;
			}
			this.m_uiLabel.text = this.m_info.tickerList[this.m_count].Param;
		}
		this.SetResetPos(this.m_startPos);
	}

	public void ResetWindow()
	{
		this.SetResetPos(this.m_startPos);
		this.m_mode = TickerWindow.Mode.Idle;
	}

	private void SetResetPos(float startPos)
	{
		if (this.m_uiLabel != null)
		{
			Vector3 localPosition = this.m_uiLabel.transform.localPosition;
			this.m_uiLabel.transform.localPosition = new Vector3(startPos, localPosition.y, localPosition.z);
		}
	}

	private void UpdateMovePos(float move)
	{
		if (this.m_uiLabel != null)
		{
			this.m_uiLabel.transform.localPosition -= Vector3.right * move;
		}
	}
}
