using System;
using UnityEngine;

public abstract class SettingBase : MonoBehaviour
{
	public void Setup(string anthorPath)
	{
		this.OnSetup(anthorPath);
	}

	public void PlayStart()
	{
		this.OnPlayStart();
	}

	public bool IsEndPlay()
	{
		return this.OnIsEndPlay();
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected abstract void OnSetup(string anthorPath);

	protected abstract void OnPlayStart();

	protected abstract bool OnIsEndPlay();

	protected abstract void OnUpdate();
}
