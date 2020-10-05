using System;
using UnityEngine;

public class ScreenGuide : MonoBehaviour
{
	private float m_screenWidth;

	private float m_screenHeight;

	private Rect[] m_rect;

	private void Start()
	{
		this.m_screenWidth = (float)Screen.width;
		this.m_screenHeight = (float)Screen.height;
		this.m_rect = new Rect[3];
		float num = this.m_screenWidth / this.m_screenHeight;
		Vector2 vector = new Vector2(this.m_screenWidth / 2f, this.m_screenHeight / 2f);
		Vector2[] array = new Vector2[3];
		array[0].Set(16f, 9f);
		array[1].Set(3f, 2f);
		array[2].Set(4f, 3f);
		for (int i = 0; i < 3; i++)
		{
			float num2 = array[i].x / array[i].y;
			if (num.Equals(num2))
			{
				this.m_rect[i].Set(0f, 0f, this.m_screenWidth, this.m_screenHeight);
			}
			else if (num > num2)
			{
				float num3 = this.m_screenHeight / array[i].y * array[i].x;
				this.m_rect[i].Set(vector.x - num3 / 2f, 0f, num3, this.m_screenHeight);
			}
			else
			{
				float num4 = this.m_screenWidth / array[i].x * array[i].y;
				this.m_rect[i].Set(0f, vector.y - num4 / 2f, this.m_screenWidth, num4);
			}
		}
	}

	private void OnGUI()
	{
		Color[] array = new Color[]
		{
			Color.red,
			Color.green,
			Color.white
		};
		for (int i = 0; i < 3; i++)
		{
			this.DrawRectangle(this.m_rect[i], array[i]);
		}
	}

	private void DrawRectangle(Rect baseRect, Color color)
	{
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, color);
		texture2D.Apply();
		float num = 2f;
		GUI.skin.box.normal.background = texture2D;
		Rect position = new Rect(0f, 0f, 0f, 0f);
		position.Set(baseRect.x, baseRect.y - num / 2f, baseRect.width, num);
		GUI.Box(position, GUIContent.none);
		position.Set(baseRect.x, baseRect.y + baseRect.height - num / 2f, baseRect.width, num);
		GUI.Box(position, GUIContent.none);
		position.Set(baseRect.x - num / 2f, baseRect.y, num, baseRect.height);
		GUI.Box(position, GUIContent.none);
		position.Set(baseRect.x + baseRect.width - num / 2f, baseRect.y, num, baseRect.height);
		GUI.Box(position, GUIContent.none);
	}
}
