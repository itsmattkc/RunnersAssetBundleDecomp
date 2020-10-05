using System;
using UnityEngine;

[Serializable]
public class UnitySysFontFPSCounter : MonoBehaviour
{
	public float updateInterval;

	private SysFontText sysFontText;

	private float accum;

	private int frames;

	private float timeleft;

	public UnitySysFontFPSCounter()
	{
		this.updateInterval = 0.5f;
	}

	public void Awake()
	{
		Application.targetFrameRate = 300;
		this.sysFontText = this.GetComponent<SysFontText>();
		this.timeleft = this.updateInterval;
	}

	public void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if (this.timeleft <= (float)0)
		{
			this.sysFontText.Text = string.Empty + (this.accum / (float)this.frames).ToString("f2");
			this.timeleft = this.updateInterval;
			this.accum = (float)0;
			this.frames = 0;
		}
	}

	public void Main()
	{
	}
}
