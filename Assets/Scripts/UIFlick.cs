using Message;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/UIFlck")]
public class UIFlick : MonoBehaviour
{
	private const float FLICK_DISTANCE_THRESHOLD_VALUE = 10f;

	private const float FLICK_TIME_THRESHOLD_VALUE = 0.3f;

	private Vector2 m_first_touch_pos = Vector2.zero;

	private Vector2 m_base_point = Vector2.zero;

	private float m_start_time;

	private GameObject m_target_obj;

	private string m_method_name = string.Empty;

	private bool m_distance_flag;

	public float GetDragDistance()
	{
		return this.m_base_point.x - this.m_first_touch_pos.x;
	}

	public void SetCallBack(GameObject obj, string method_name)
	{
		this.m_target_obj = obj;
		this.m_method_name = method_name;
	}

	private void SendMessage(bool right_flick_flag)
	{
		if (this.m_target_obj != null)
		{
			FlickType type = (!right_flick_flag) ? FlickType.LEFT : FlickType.RIGHT;
			MsgUIFlick value = new MsgUIFlick(type);
			this.m_target_obj.SendMessage(this.m_method_name, value, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		int touchCount = UnityEngine.Input.touchCount;
		if (touchCount > 0)
		{
			Touch touch = Input.touches[0];
			Vector2 position = touch.position;
			switch (touch.phase)
			{
			case TouchPhase.Began:
				this.OnTouchBegan(position);
				break;
			case TouchPhase.Moved:
				this.OnTouchMove(position);
				break;
			case TouchPhase.Stationary:
				this.OnTouchStationary(position);
				break;
			case TouchPhase.Ended:
				this.OnTouchEnd(position);
				break;
			}
		}
	}

	private void UpdateTouchData(Vector2 position, bool first_touch_flag)
	{
		if (first_touch_flag)
		{
			this.m_first_touch_pos = position;
		}
		this.m_base_point = position;
		this.m_start_time = Time.realtimeSinceStartup;
	}

	private void OnTouchBegan(Vector2 position)
	{
		bool first_touch_flag = true;
		this.UpdateTouchData(position, first_touch_flag);
	}

	private void OnTouchMove(Vector2 position)
	{
		float num = Mathf.Abs(position.x - this.m_base_point.x);
		this.m_distance_flag = (num > 10f);
	}

	private void OnTouchStationary(Vector2 position)
	{
		if (this.m_distance_flag)
		{
			float num = Mathf.Abs(position.x - this.m_base_point.x);
			if (num < 10f)
			{
				this.m_distance_flag = false;
			}
		}
	}

	private void OnTouchEnd(Vector2 position)
	{
		if (this.m_distance_flag)
		{
			float num = Time.realtimeSinceStartup - this.m_start_time;
			if (num < 0.3f)
			{
				float num2 = position.x - this.m_base_point.x;
				bool right_flick_flag = true;
				if (num2 < 0f)
				{
					right_flick_flag = false;
					this.SendMessage(right_flick_flag);
					global::Debug.Log("Left Flick Success");
				}
				else
				{
					this.SendMessage(right_flick_flag);
					global::Debug.Log("Right Flick Success");
				}
			}
		}
	}
}
