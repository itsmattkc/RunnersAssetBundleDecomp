using GameScore;
using System;
using Tutorial;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjMoveTrap")]
public class ObjMoveTrap : ObjTrapBase
{
	private enum ModelType
	{
		Missile,
		Boo,
		NUM,
		NONE
	}

	private class ModelParam
	{
		public string m_modelName;

		public string m_seName;

		public string m_effectName;

		public ModelParam(string modelName, string seName, string effectName)
		{
			this.m_modelName = modelName;
			this.m_seName = seName;
			this.m_effectName = effectName;
		}
	}

	private static readonly ObjMoveTrap.ModelParam[] MODEL_PARAMS = new ObjMoveTrap.ModelParam[]
	{
		new ObjMoveTrap.ModelParam("obj_cmn_movetrap", "obj_missile_shoot", "ef_com_explosion_m01"),
		new ObjMoveTrap.ModelParam("obj_cmn_boomboo", "obj_ghost_s", "ef_com_explosion_m01")
	};

	private ObjMoveTrapParameter m_param;

	private LevelInformation m_levelInformation;

	private ObjMoveTrap.ModelParam m_modelParam;

	protected override string GetModelName()
	{
		this.SetupParam();
		return this.m_modelParam.m_modelName;
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override int GetScore()
	{
		return Data.MoveTrap;
	}

	public override bool IsValid()
	{
		return !(StageModeManager.Instance != null) || !StageModeManager.Instance.IsQuickMode();
	}

	protected override void OnSpawned()
	{
		if (this.IsCreateCheck())
		{
			base.enabled = false;
			this.SetupParam();
			base.OnSpawned();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetObjMoveTrapParameter(ObjMoveTrapParameter param)
	{
		if (this.IsCreateCheck())
		{
			this.SetupParam();
			this.m_param = param;
			MotorConstant component = base.GetComponent<MotorConstant>();
			if (component)
			{
				component.SetParam(this.m_param.moveSpeed, this.m_param.moveDistance, this.m_param.startMoveDistance, -base.transform.right, "SE", this.m_modelParam.m_seName);
			}
		}
	}

	protected override void PlayEffect()
	{
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, this.m_modelParam.m_effectName, 1f, false);
	}

	protected override void TrapDamageHit()
	{
		base.SetBroken();
	}

	private bool IsTutorialCheck()
	{
		return StageTutorialManager.Instance != null && !StageTutorialManager.Instance.IsCompletedTutorial();
	}

	private bool IsCreateCheck()
	{
		if (!ObjUtil.IsUseTemporarySet() && StageTutorialManager.Instance != null)
		{
			if (StageTutorialManager.Instance.IsCompletedTutorial())
			{
				return true;
			}
			EventID currentEventID = StageTutorialManager.Instance.CurrentEventID;
			if (currentEventID != EventID.DAMAGE)
			{
				return false;
			}
		}
		return true;
	}

	private void SetupParam()
	{
		if (this.m_levelInformation == null)
		{
			this.m_levelInformation = ObjUtil.GetLevelInformation();
		}
		if (this.m_modelParam == null)
		{
			this.m_modelParam = ObjMoveTrap.MODEL_PARAMS[0];
			if (this.m_levelInformation != null && !this.IsTutorialCheck())
			{
				int randomRange = ObjUtil.GetRandomRange100();
				if (randomRange < this.m_levelInformation.MoveTrapBooRand)
				{
					this.m_modelParam = ObjMoveTrap.MODEL_PARAMS[1];
				}
			}
		}
	}
}
