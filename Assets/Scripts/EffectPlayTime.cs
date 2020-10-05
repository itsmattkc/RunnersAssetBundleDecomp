using System;
using UnityEngine;

public class EffectPlayTime : MonoBehaviour
{
	private float m_passedTime;

	public float m_endTime = 1f;

	private void Update()
	{
		this.m_passedTime += Time.deltaTime;
		if (this.m_passedTime > this.m_endTime)
		{
			base.gameObject.SetActive(false);
			if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.SleepEffect(base.gameObject);
			}
		}
	}

	public void PlayEffect()
	{
		this.m_passedTime = 0f;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleSystem particleSystem = array[i];
			particleSystem.time = 0f;
			particleSystem.Play();
		}
	}
}
