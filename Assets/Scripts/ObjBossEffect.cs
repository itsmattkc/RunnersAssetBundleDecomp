using System;
using UnityEngine;

public class ObjBossEffect : MonoBehaviour
{
	private const string CHAO_SENAME = "act_boss_abnormal";

	protected Vector3 m_hit_offset = Vector3.zero;

	private uint m_chaoSEID;

	private void OnDestroy()
	{
	}

	public void SetHitOffset(Vector3 hit_offset)
	{
		this.m_hit_offset = hit_offset;
	}

	public void PlayChaoEffect()
	{
		this.OnPlayChaoEffect();
	}

	protected virtual void OnPlayChaoEffect()
	{
	}

	protected void PlayChaoEffectSE()
	{
		if (this.m_chaoSEID == 0u)
		{
			this.m_chaoSEID = (uint)ObjUtil.PlaySE("act_boss_abnormal", "SE");
		}
	}
}
