using GameScore;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjBigTrap")]
public class ObjBigTrap : ObjTrapBase
{
	private const string ModelName = "obj_cmn_boomboo_L";

	private ObjBigTrapParameter m_param;

	private PlayerInformation m_playerInfo;

	private bool m_start;

	private uint m_move_SEID;

	protected override string GetModelName()
	{
		return "obj_cmn_boomboo_L";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override int GetScore()
	{
		return Data.BigTrap;
	}

	private void StopSE()
	{
		if (this.m_move_SEID != 0u)
		{
			ObjUtil.StopSE((SoundManager.PlayId)this.m_move_SEID);
			this.m_move_SEID = 0u;
		}
	}

	private void Update()
	{
		if (this.m_start)
		{
			return;
		}
		if (this.m_playerInfo == null)
		{
			this.m_playerInfo = ObjUtil.GetPlayerInformation();
		}
		if (this.m_playerInfo != null && this.m_param != null)
		{
			float playerDistance = this.GetPlayerDistance();
			if (playerDistance < this.m_param.startMoveDistance)
			{
				MotorAnimalFly component = base.GetComponent<MotorAnimalFly>();
				if (component)
				{
					component.SetupParam(this.m_param.moveSpeedY, this.m_param.moveDistanceY, this.m_param.moveSpeedX, base.transform.right, 0f, false);
					if (this.m_move_SEID == 0u)
					{
						this.m_move_SEID = (uint)ObjUtil.LightPlaySE("obj_ghost_l", "SE");
					}
					this.m_start = true;
				}
			}
		}
	}

	private float GetPlayerDistance()
	{
		if (this.m_playerInfo)
		{
			Vector3 position = base.transform.position;
			return Mathf.Abs(Vector3.Distance(position, this.m_playerInfo.Position));
		}
		return 0f;
	}

	public void SetObjBigTrapParameter(ObjBigTrapParameter param)
	{
		this.m_param = param;
	}

	protected override void PlayEffect()
	{
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_com_explosion_l01", 1f, false);
	}

	protected override void TrapDamageHit()
	{
		this.StopSE();
		base.SetBroken();
	}
}
