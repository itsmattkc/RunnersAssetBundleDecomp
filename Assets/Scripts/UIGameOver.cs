using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/GameMode/Stage")]
public class UIGameOver : MonoBehaviour
{
	private sealed class _waitForDrawDistance_c__Iterator20 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float time;

		internal int _PC;

		internal object _current;

		internal float ___time;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.time);
				this._PC = 1;
				return true;
			case 1u:
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const float windowWidth = 250f;

	private const float windowHeight = 400f;

	private Rect m_windowRect;

	public float m_totalDistance;

	private float m_drawDistance;

	private bool m_onButton;

	private bool m_buttonPressed;

	private GUISkin m_skin;

	private void Start()
	{
		float left = ((float)Screen.width - 250f) * 0.5f;
		float top = ((float)Screen.height - 400f) * 0.5f;
		this.m_windowRect = new Rect(left, top, 250f, 400f);
		base.StartCoroutine("waitForDrawDistance", 2f);
	}

	private void Update()
	{
		float num = this.m_totalDistance - this.m_drawDistance;
		float num2 = (float)((num <= 100f) ? 40 : 400);
		this.m_drawDistance += Time.deltaTime * num2;
		if (this.m_drawDistance >= this.m_totalDistance)
		{
			this.m_drawDistance = this.m_totalDistance;
			this.m_onButton = true;
		}
	}

	public void SetSkin(GUISkin skin)
	{
		this.m_skin = skin;
		this.m_skin.window.fontSize = 20;
		this.m_skin.window.normal.textColor = Color.white;
		this.m_skin.button.fontSize = 20;
		this.m_skin.button.normal.textColor = Color.white;
		this.m_skin.label.fontSize = 20;
		this.m_skin.label.normal.textColor = Color.white;
	}

	public void OnGUI()
	{
		GUI.backgroundColor = Color.green;
		this.m_windowRect = GUI.Window(1, this.m_windowRect, new GUI.WindowFunction(this.DoMyWindow), "Game Over", this.m_skin.window);
		if (!this.m_onButton)
		{
			return;
		}
	}

	private void DoMyWindow(int windowID)
	{
		string text = "Total Distance:" + Mathf.FloorToInt(this.m_drawDistance).ToString();
		GUIContent gUIContent = new GUIContent();
		gUIContent.text = text;
		Rect position = new Rect(5f, 40f, 180f, 40f);
		GUI.Label(position, gUIContent, this.m_skin.label);
		float left = 10f;
		float num = 100f;
		if (GUI.Button(new Rect(left, num, 230f, 40f), "Retry", this.m_skin.button) && !this.m_buttonPressed)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("s_w01StageTestmap");
			this.m_buttonPressed = true;
		}
		num += 180f;
		if (GUI.Button(new Rect(left, num, 230f, 40f), "Go Title", this.m_skin.button) && !this.m_buttonPressed)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("s_title1st");
			this.m_buttonPressed = true;
		}
	}

	private IEnumerator waitForDrawDistance(float time)
	{
		UIGameOver._waitForDrawDistance_c__Iterator20 _waitForDrawDistance_c__Iterator = new UIGameOver._waitForDrawDistance_c__Iterator20();
		_waitForDrawDistance_c__Iterator.time = time;
		_waitForDrawDistance_c__Iterator.___time = time;
		return _waitForDrawDistance_c__Iterator;
	}
}
