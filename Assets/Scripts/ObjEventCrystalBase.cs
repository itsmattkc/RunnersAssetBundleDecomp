using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjEventCrystalBase : SpawnableObject
{
	private PlayerInformation m_playerInfo;

	private bool m_end;

	private int DRILL_UP_COUNT = 3;

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.EVENT_RESOURCE;
	}

	protected override void OnSpawned()
	{
		base.enabled = false;
		this.m_playerInfo = ObjUtil.GetPlayerInformation();
		this.CheckActiveComboChaoAbility();
	}

	private EventCtystalType GetCtystalType()
	{
		return this.GetOriginalType();
	}

	protected virtual int GetAddCount()
	{
		return 0;
	}

	protected virtual EventCtystalType GetOriginalType()
	{
		return EventCtystalType.SMALL;
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

	private void TakeCrystal()
	{
		this.m_end = true;
		CtystalParam ctystalParam = ObjEventCrystalData.GetCtystalParam(this.GetCtystalType());
		if (ctystalParam != null)
		{
			ObjUtil.SendMessageAddSpecialCrystal(this.GetSrytalCount());
			ObjEventCrystalBase.SetChaoAbliltyScoreEffect(this.m_playerInfo, ctystalParam, base.gameObject);
			if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.PlayEffect((!ctystalParam.m_big) ? EffectPlayType.CRYSTAL_C : EffectPlayType.CRYSTAL_BIG_C, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
			}
			ObjUtil.LightPlaySE(ctystalParam.m_se, "SE");
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

	private int GetSrytalCount()
	{
		if (this.m_playerInfo != null && this.m_playerInfo.PhantomType == PhantomType.DRILL)
		{
			return this.GetAddCount() * this.DRILL_UP_COUNT;
		}
		return this.GetAddCount();
	}

	public static void SetChaoAbliltyScoreEffect(PlayerInformation playerInfo, CtystalParam ctystalParam, GameObject obj)
	{
		if (ctystalParam != null)
		{
			List<ChaoAbility> abilityList = new List<ChaoAbility>();
			ObjUtil.GetChaoAbliltyPhantomFlag(playerInfo, ref abilityList);
			int chaoAbliltyScore = ObjUtil.GetChaoAbliltyScore(abilityList, ctystalParam.m_score);
			ObjUtil.SendMessageAddScore(chaoAbliltyScore);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(3, chaoAbliltyScore));
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
