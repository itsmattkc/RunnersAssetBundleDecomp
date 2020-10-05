using GameScore;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjFloor")]
public class ObjAirFloor : SpawnableObject
{
	private GameObject m_modelObject;

	private static readonly string[] FLOOR_TYPENAME = new string[]
	{
		"1m",
		"2m",
		"3m"
	};

	private static readonly float[] FLOOR_TYPESIZE = new float[]
	{
		1f,
		2f,
		3f
	};

	private static readonly string[] FLOOR_EFFNAME = new string[]
	{
		"ef_com_explosion_m01",
		"ef_com_explosion_m01",
		"ef_com_explosion_l01"
	};

	private static Vector3 COLLI_CENTER = new Vector3(0f, -0.15f, 0f);

	private static Vector3 COLLI_SIZE = new Vector3(0f, 0.4f, 3f);

	private int m_type_index;

	private bool m_end;

	public GameObject ModelObject
	{
		get
		{
			return this.m_modelObject;
		}
		set
		{
			if (this.m_modelObject == null)
			{
				this.m_modelObject = value;
				this.m_modelObject.SetActive(true);
				this.m_modelObject.isStatic = true;
			}
		}
	}

	protected override void OnSpawned()
	{
		base.enabled = false;
	}

	public void Setup(string name)
	{
		for (int i = 0; i < ObjAirFloor.FLOOR_TYPENAME.Length; i++)
		{
			if (name.IndexOf(ObjAirFloor.FLOOR_TYPENAME[i]) != -1)
			{
				this.m_type_index = i;
				break;
			}
		}
		if (this.m_type_index < ObjAirFloor.FLOOR_TYPESIZE.Length)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			if (component)
			{
				component.center = ObjAirFloor.COLLI_CENTER;
				component.size = new Vector3(ObjAirFloor.FLOOR_TYPESIZE[this.m_type_index], ObjAirFloor.COLLI_SIZE.y, ObjAirFloor.COLLI_SIZE.z);
			}
		}
	}

	public void OnMsgStepObjectDead(MsgObjectDead msg)
	{
		if (this.m_end)
		{
			return;
		}
		GameObject gameObject = GameObject.FindWithTag("Player");
		if (gameObject != null)
		{
			ObjUtil.CreateBrokenBonusForChaoAbiilty(base.gameObject, gameObject);
		}
		this.SetBroken();
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (this.m_end)
		{
			return;
		}
		if (ObjUtil.IsAttackAttribute(msg.m_attackAttribute, AttackAttribute.Invincible))
		{
			return;
		}
		if (msg.m_attackPower >= 4 && msg.m_sender)
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

	private void SetPlayerBroken(uint attribute_state)
	{
		if (this.m_end)
		{
			return;
		}
		int num = Data.AirFloor;
		List<ChaoAbility> abilityList = new List<ChaoAbility>();
		ObjUtil.GetChaoAbliltyPhantomFlag(attribute_state, ref abilityList);
		num = ObjUtil.GetChaoAndEnemyScore(abilityList, num);
		ObjUtil.SendMessageAddScore(num);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(1, num));
		this.SetBroken();
	}

	private void SetBroken()
	{
		if (this.m_end)
		{
			return;
		}
		this.m_end = true;
		if (this.m_type_index < ObjAirFloor.FLOOR_EFFNAME.Length)
		{
			ObjUtil.PlayEffectCollisionCenter(base.gameObject, ObjAirFloor.FLOOR_EFFNAME[this.m_type_index], 1f, false);
		}
		ObjUtil.LightPlaySE("obj_brk", "SE");
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
