using GameScore;
using Message;
using System;
using UnityEngine;

public class ObjTrap : ObjTrapBase
{
	private const string ModelName = "obj_cmn_trap";

	protected override string GetModelName()
	{
		return "obj_cmn_trap";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override int GetScore()
	{
		return Data.Trap;
	}

	protected override void OnSpawned()
	{
		base.enabled = false;
		base.OnSpawned();
		if (StageComboManager.Instance != null && StageComboManager.Instance.IsChaoFlagStatus(StageComboManager.ChaoFlagStatus.DESTROY_TRAP))
		{
			base.SetBroken();
		}
	}

	public void OnMsgObjectDeadChaoCombo(MsgObjectDead msg)
	{
		if (msg.m_chaoAbility == ChaoAbility.COMBO_DESTROY_TRAP)
		{
			base.SetBroken();
		}
	}

	protected override void PlayEffect()
	{
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(EffectPlayType.TRAP, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
	}

	public override void OnRevival()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		if (component)
		{
			component.enabled = true;
		}
		base.enabled = true;
		this.m_end = false;
		this.OnSpawned();
	}
}
