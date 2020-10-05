using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjCrystalBase : SpawnableObject
{
	private PlayerInformation m_playerInfo;

	private CtystalType m_type = CtystalType.NONE;

	private bool m_end;

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override string GetModelName()
	{
		if (this.m_type == CtystalType.NONE)
		{
			this.m_type = this.GetCtystalModelType();
		}
		CtystalParam crystalParam = ObjCrystalData.GetCrystalParam(this.m_type);
		if (crystalParam != null)
		{
			return crystalParam.m_model;
		}
		return string.Empty;
	}

	protected override void OnSpawned()
	{
		base.enabled = false;
		this.m_playerInfo = ObjUtil.GetPlayerInformation();
		if (this.m_type == CtystalType.NONE)
		{
			this.m_type = this.GetCtystalModelType();
		}
		bool flag = ObjCrystalData.IsBig(this.GetOriginalType());
		bool flag2 = ObjCrystalData.IsBig(this.m_type);
		if (flag2 && flag != flag2)
		{
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component != null)
			{
				component.center = new Vector3(component.center.x, component.center.y + 0.15f, component.center.z);
				component.radius += 0.1f;
			}
		}
		this.CheckActiveComboChaoAbility();
	}

	private CtystalType GetCtystalModelType()
	{
		return ObjCrystalUtil.GetCrystalModelType(this.GetOriginalType());
	}

	protected virtual CtystalType GetOriginalType()
	{
		return CtystalType.SMALL_A;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_end)
		{
			return;
		}
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player")
				{
					if (gameObject.tag != "ChaoAttack")
					{
						this.TakeCrystal();
					}
				}
				else if (a == "HitCrystal" && gameObject.tag == "Chao")
				{
					this.TakeCrystal();
				}
			}
		}
	}

	private void OnDrawingRings(MsgOnDrawingRings msg)
	{
		ObjUtil.StartMagnetControl(base.gameObject);
	}

	private void OnDrawingRingsToChao(MsgOnDrawingRings msg)
	{
		if (msg != null)
		{
			ObjUtil.StartMagnetControl(base.gameObject, msg.m_target);
		}
	}

	private void OnDrawingRingsChaoAbility(MsgOnDrawingRings msg)
	{
		if (msg.m_chaoAbility == ChaoAbility.COMBO_RECOVERY_ALL_OBJ || msg.m_chaoAbility == ChaoAbility.COMBO_DESTROY_AND_RECOVERY)
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
	}

	private EffectPlayType GetEffectType()
	{
		switch (this.m_type)
		{
		case CtystalType.SMALL_A:
			return EffectPlayType.CRYSTAL_A;
		case CtystalType.SMALL_B:
			return EffectPlayType.CRYSTAL_B;
		case CtystalType.SMALL_C:
			return EffectPlayType.CRYSTAL_C;
		case CtystalType.BIG_A:
			return EffectPlayType.CRYSTAL_BIG_A;
		case CtystalType.BIG_B:
			return EffectPlayType.CRYSTAL_BIG_B;
		case CtystalType.BIG_C:
			return EffectPlayType.CRYSTAL_BIG_C;
		default:
			return EffectPlayType.CRYSTAL_A;
		}
	}

	private void TakeCrystal()
	{
		this.m_end = true;
		CtystalParam crystalParam = ObjCrystalData.GetCrystalParam(this.m_type);
		if (crystalParam != null)
		{
			ObjCrystalBase.SetChaoAbliltyScoreEffect(this.m_playerInfo, crystalParam, base.gameObject);
			if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.PlayEffect(this.GetEffectType(), ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
			}
			ObjUtil.LightPlaySE(crystalParam.m_se, "SE");
			HudTutorial.SendActionTutorial(HudTutorial.Id.ACTION_1);
			if (base.Share)
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public static void SetChaoAbliltyScoreEffect(PlayerInformation playerInfo, CtystalParam ctystalParam, GameObject obj)
	{
		if (ctystalParam != null)
		{
			List<ChaoAbility> abilityList = new List<ChaoAbility>();
			ObjUtil.GetChaoAbliltyPhantomFlag(playerInfo, ref abilityList);
			int chaoAbliltyScore = ObjUtil.GetChaoAbliltyScore(abilityList, ctystalParam.m_score);
			ObjUtil.SendMessageAddScore(chaoAbliltyScore);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(2, chaoAbliltyScore));
			ObjUtil.AddCombo();
		}
	}

	public void CheckActiveComboChaoAbility()
	{
		if (StageComboManager.Instance != null && (StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ALL_OBJ) || StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_DESTROY_AND_RECOVERY)))
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
	}

	public override void OnRevival()
	{
		MagnetControl component = base.GetComponent<MagnetControl>();
		if (component != null)
		{
			component.Reset();
		}
		SphereCollider component2 = base.GetComponent<SphereCollider>();
		if (component2)
		{
			component2.enabled = true;
		}
		BoxCollider component3 = base.GetComponent<BoxCollider>();
		if (component3)
		{
			component3.enabled = true;
		}
		this.CheckActiveComboChaoAbility();
		this.m_end = false;
	}
}
