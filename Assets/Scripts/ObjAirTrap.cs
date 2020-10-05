using GameScore;
using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjAirTrap")]
public class ObjAirTrap : ObjTrapBase
{
	private const string ModelName = "obj_cmn_airtrap";

	private ObjAirTrapParameter m_param;

	protected override string GetModelName()
	{
		return "obj_cmn_airtrap";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override int GetScore()
	{
		return Data.AirTrap;
	}

	public override bool IsValid()
	{
		return !(StageModeManager.Instance != null) || !StageModeManager.Instance.IsQuickMode();
	}

	protected override void OnSpawned()
	{
		base.OnSpawned();
		if (StageComboManager.Instance != null && StageComboManager.Instance.IsChaoFlagStatus(StageComboManager.ChaoFlagStatus.DESTROY_AIRTRAP))
		{
			base.SetBroken();
		}
		base.enabled = false;
	}

	public void OnMsgObjectDeadChaoCombo(MsgObjectDead msg)
	{
		if (msg.m_chaoAbility == ChaoAbility.COMBO_DESTROY_AIR_TRAP)
		{
			base.SetBroken();
		}
	}

	public void SetObjAirTrapParameter(ObjAirTrapParameter param)
	{
		this.m_param = param;
		MotorSwing component = base.GetComponent<MotorSwing>();
		if (component)
		{
			component.SetParam(this.m_param.moveSpeed, this.m_param.moveDistanceX, this.m_param.moveDistanceY, base.transform.right);
		}
	}

	protected override void PlayEffect()
	{
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(EffectPlayType.AIR_TRAP, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
	}

	public override void OnRevival()
	{
		SphereCollider component = base.GetComponent<SphereCollider>();
		if (component)
		{
			component.enabled = true;
		}
		base.enabled = true;
		this.m_end = false;
		this.OnSpawned();
	}
}
