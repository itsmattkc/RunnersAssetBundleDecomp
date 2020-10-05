using System;
using UnityEngine;

public class EffectShareState : MonoBehaviour
{
	public enum State
	{
		Sleep,
		Active
	}

	private EffectShareState.State m_state;

	public EffectPlayType m_effectType;

	public bool IsSleep()
	{
		return this.m_state == EffectShareState.State.Sleep;
	}

	public void SetState(EffectShareState.State state)
	{
		this.m_state = state;
	}

	private void Start()
	{
		base.enabled = false;
	}
}
