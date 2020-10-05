using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjTrapBase : SpawnableObject
{
	protected bool m_end;

	protected override void OnSpawned()
	{
	}

	public void OnMsgObjectDead(MsgObjectDead msg)
	{
		if (!this.m_end)
		{
			this.SetBroken();
		}
	}

	protected virtual int GetScore()
	{
		return 0;
	}

	protected virtual void PlayEffect()
	{
	}

	protected virtual void TrapDamageHit()
	{
		PlayerInformation playerInformation = ObjUtil.GetPlayerInformation();
		if (playerInformation != null && playerInformation.IsNowLastChance())
		{
			return;
		}
		ObjUtil.LightPlaySE("obj_needle_damage", "SE");
	}

	private void SetPlayerBroken(uint attribute_state)
	{
		int num = this.GetScore();
		List<ChaoAbility> abilityList = new List<ChaoAbility>();
		ObjUtil.GetChaoAbliltyPhantomFlag(attribute_state, ref abilityList);
		num = ObjUtil.GetChaoAndEnemyScore(abilityList, num);
		ObjUtil.SendMessageAddScore(num);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(1, num));
		this.SetBroken();
	}

	protected void SetBroken()
	{
		if (this.m_end)
		{
			return;
		}
		this.m_end = true;
		this.PlayEffect();
		ObjUtil.LightPlaySE("obj_brk", "SE");
		if (base.Share)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
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
				MsgHitDamage value = new MsgHitDamage(base.gameObject, AttackPower.PlayerColorPower);
				gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (this.m_end)
		{
			return;
		}
		if (msg.m_attackPower >= 4)
		{
			if (msg.m_sender)
			{
				GameObject gameObject = msg.m_sender.gameObject;
				if (gameObject)
				{
					MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
					gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
					this.SetPlayerBroken(msg.m_attackAttribute);
					ObjUtil.CreateBrokenBonus(base.gameObject, gameObject, msg.m_attackAttribute);
				}
			}
		}
		else
		{
			this.TrapDamageHit();
		}
	}
}
