using Boss;
using System;
using UnityEngine;

public class ObjBossEggmanEffect : ObjBossEffect
{
	public enum BoostType
	{
		Normal,
		Attack
	}

	private const float HITEFFECT_AREA = 0.5f;

	private static Vector3 APPEAR_EFFECT_OFFSET = new Vector3(-1f, 0.5f, 0f);

	private static Vector3 BOOST_EFFECT_ROT = new Vector3(-90f, 0f, 0f);

	private GameObject m_sweat_effect;

	private GameObject m_boost_effectL;

	private GameObject m_boost_effectR;

	private int m_boostType = -1;

	private void OnDestroy()
	{
		this.PlaySweatEffectEnd();
		this.DestroyBoostEffect();
	}

	public void PlayHitEffect()
	{
		if (this.m_hit_offset.y > 0.5f)
		{
			this.m_hit_offset.y = Mathf.Min(this.m_hit_offset.y, 0.5f);
		}
		else
		{
			this.m_hit_offset.y = Mathf.Max(this.m_hit_offset.y, -0.5f);
		}
		ObjUtil.PlayEffect("ef_bo_em_damage01", base.transform.position + new Vector3(0f, this.m_hit_offset.y, 0f), Quaternion.identity, 1f, false);
	}

	public void PlayFoundEffect()
	{
		ObjUtil.PlayEffectChild(base.gameObject, "ef_bo_em_found01", ObjBossEggmanEffect.APPEAR_EFFECT_OFFSET, Quaternion.identity, 2f, true);
	}

	public void PlaySweatEffectStart()
	{
		this.PlaySweatEffectEnd();
		this.m_sweat_effect = ObjUtil.PlayEffectChild(base.gameObject, "ef_bo_em_sweat01", Vector3.zero, Quaternion.identity, false);
	}

	public void PlaySweatEffectEnd()
	{
		if (this.m_sweat_effect)
		{
			UnityEngine.Object.Destroy(this.m_sweat_effect);
			this.m_sweat_effect = null;
		}
	}

	public void PlayEscapeEffect(ObjBossEggmanState context)
	{
		ObjUtil.PlayEffectChild(base.gameObject, "ef_bo_em_blackfog01", Vector3.zero, Quaternion.identity, 5f, true);
	}

	public void PlayBoostEffect(ObjBossEggmanEffect.BoostType type)
	{
		if (this.m_boostType != (int)type)
		{
			this.DestroyBoostEffect();
			if (type != ObjBossEggmanEffect.BoostType.Normal)
			{
				if (type == ObjBossEggmanEffect.BoostType.Attack)
				{
					this.PlayBoostEffect("ef_bo_em_vernier_l01");
				}
			}
			else
			{
				this.PlayBoostEffect("ef_bo_em_vernier_s01");
			}
			this.m_boostType = (int)type;
		}
	}

	public void DestroyBoostEffect()
	{
		if (this.m_boost_effectL)
		{
			UnityEngine.Object.Destroy(this.m_boost_effectL);
			this.m_boost_effectL = null;
		}
		if (this.m_boost_effectR)
		{
			UnityEngine.Object.Destroy(this.m_boost_effectR);
			this.m_boost_effectR = null;
		}
	}

	private void PlayBoostEffect(string effectName)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Booster_L");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Booster_R");
		if (gameObject && gameObject2)
		{
			Quaternion local_rot = Quaternion.Euler(ObjBossEggmanEffect.BOOST_EFFECT_ROT);
			this.m_boost_effectL = ObjUtil.PlayEffectChild(gameObject, effectName, Vector3.zero, local_rot, true);
			this.m_boost_effectR = ObjUtil.PlayEffectChild(gameObject2, effectName, Vector3.zero, local_rot, true);
		}
	}

	protected override void OnPlayChaoEffect()
	{
		StageAbilityManager instance = StageAbilityManager.Instance;
		if (instance == null)
		{
			return;
		}
		if (instance.HasChaoAbility(ChaoAbility.BOSS_SUPER_RING_RATE))
		{
			base.PlayChaoEffectSE();
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_beam_atk_ht_sr02", Vector3.zero, -1f, false);
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_beam_atk_lp_sr02", Vector3.zero, -1f, false);
		}
		if (instance.HasChaoAbility(ChaoAbility.BOSS_RED_RING_RATE))
		{
			base.PlayChaoEffectSE();
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_beam_atk_ht_sr01", Vector3.zero, -1f, false);
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_beam_atk_lp_sr01", Vector3.zero, -1f, false);
		}
		if (instance.HasChaoAbility(ChaoAbility.BOSS_STAGE_TIME))
		{
			base.PlayChaoEffectSE();
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_magic_atk_ht_sr01", Vector3.zero, -1f, false);
			ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_magic_atk_lp_sr01", Vector3.zero, -1f, false);
		}
	}
}
