using Message;
using System;
using UnityEngine;

public class ObjBossTrapBall : MonoBehaviour
{
	private class BossTrapBallModelTypeParam
	{
		public string m_modelName1;

		public string m_modelName2;

		public ResourceCategory m_resCategory;

		public string m_effectName;

		public string m_modelNameLeft;

		public string m_modelNameRight;

		public string m_modelNameTop;

		public string m_modelNameUnder;

		public BossTrapBallModelTypeParam(string model1, string model2, ResourceCategory resCategory, string effect, string modelL, string modelR, string modelT, string modelU)
		{
			this.m_modelName1 = model1;
			this.m_modelName2 = model2;
			this.m_resCategory = resCategory;
			this.m_effectName = effect;
			this.m_modelNameLeft = modelL;
			this.m_modelNameRight = modelR;
			this.m_modelNameTop = modelT;
			this.m_modelNameUnder = modelU;
		}
	}

	private enum State
	{
		Idle,
		Start,
		Down,
		Wait,
		Attack
	}

	private const float END_TIME = 5f;

	private const float ATTACK_ROT_SPEED = 25f;

	private const int COLLI_NUM = 2;

	private static Vector3 TRAP_TYPE_BALLROT = new Vector3(0f, 0f, 1f);

	private ObjBossTrapBall.State m_state;

	private float m_time;

	private GameObject m_boss_obj;

	private GameObject m_model_obj;

	private GameObject[] m_colli_obj;

	private GameObject[] m_colli_model_obj;

	private Vector3 m_colli = Vector3.zero;

	private float m_rot_speed;

	private float m_attack_speed;

	private float m_add_speed = 3f;

	private BossTrapBallType m_type;

	private bool m_bossAppear;

	private ObjBossTrapBall.BossTrapBallModelTypeParam m_typeParam = new ObjBossTrapBall.BossTrapBallModelTypeParam("obj_boss_movetrap", string.Empty, ResourceCategory.OBJECT_RESOURCE, "ef_com_explosion_m01", string.Empty, string.Empty, string.Empty, string.Empty);

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		switch (this.m_state)
		{
		case ObjBossTrapBall.State.Start:
			if (this.m_bossAppear)
			{
				ObjBossUtil.SetupBallAppear(this.m_boss_obj, base.gameObject);
			}
			this.m_state = ObjBossTrapBall.State.Down;
			break;
		case ObjBossTrapBall.State.Down:
			if (this.m_boss_obj)
			{
				if (this.m_bossAppear)
				{
					if (ObjBossUtil.UpdateBallAppear(deltaTime, this.m_boss_obj, base.gameObject, this.m_add_speed))
					{
						ObjBossUtil.PlayShotEffect(this.m_boss_obj);
						ObjBossUtil.PlayShotSE();
						this.Shot(this.m_colli);
						this.m_time = 0f;
						this.m_state = ObjBossTrapBall.State.Wait;
					}
				}
				else
				{
					this.Shot(this.m_colli);
					this.m_time = 0f;
					this.m_state = ObjBossTrapBall.State.Wait;
				}
			}
			break;
		case ObjBossTrapBall.State.Wait:
			if (this.m_colli_model_obj != null)
			{
				for (int i = 0; i < this.m_colli_model_obj.Length; i++)
				{
					if (this.m_colli_model_obj[i] != null)
					{
						ObjBossUtil.UpdateBallRot(deltaTime, this.m_colli_model_obj[i], ObjBossTrapBall.TRAP_TYPE_BALLROT, this.m_rot_speed);
					}
				}
			}
			this.m_time += deltaTime;
			if (this.m_time > 5f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case ObjBossTrapBall.State.Attack:
			if (this.m_boss_obj)
			{
				this.m_time += deltaTime;
				ObjBossUtil.UpdateBallAttack(this.m_boss_obj, base.gameObject, this.m_time, this.m_attack_speed);
				float num = Mathf.Abs(Vector3.Distance(base.transform.position, this.m_boss_obj.transform.position));
				if (num < 0.1f)
				{
					this.HitAttack(this.m_boss_obj);
					this.SetBroken();
				}
			}
			break;
		}
	}

	public void OnMsgObjectDead(MsgObjectDead msg)
	{
		if (base.enabled)
		{
			this.SetBroken();
		}
	}

	private void SetBroken()
	{
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, this.m_typeParam.m_effectName, 1f, false);
		ObjUtil.PlaySE("obj_brk", "SE");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Setup(GameObject obj, Vector3 colli, float rot_speed, float attack_speed, BossTrapBallType type, BossType bossType, bool bossAppear)
	{
		this.m_boss_obj = obj;
		this.m_colli = colli;
		this.m_rot_speed = rot_speed;
		this.m_attack_speed = attack_speed;
		this.m_type = type;
		this.m_bossAppear = bossAppear;
		if (BossTypeUtil.GetBossCharaType(bossType) != BossCharaType.EGGMAN)
		{
			this.m_typeParam.m_modelName1 = EventBossObjectTable.GetItemData(EventBossObjectTableItem.Obj2_ModelName);
			this.m_typeParam.m_resCategory = ResourceCategory.EVENT_RESOURCE;
			this.m_typeParam.m_effectName = EventBossObjectTable.GetItemData(EventBossObjectTableItem.Obj2_EffectName);
		}
		this.m_typeParam.m_modelName2 = this.m_typeParam.m_modelName1 + "brk";
		this.m_typeParam.m_modelNameLeft = this.m_typeParam.m_modelName1 + "_left";
		this.m_typeParam.m_modelNameRight = this.m_typeParam.m_modelName1 + "_right";
		this.m_typeParam.m_modelNameTop = this.m_typeParam.m_modelName1 + "_top";
		this.m_typeParam.m_modelNameUnder = this.m_typeParam.m_modelName1 + "_under";
		this.CreateModel(this.m_type);
		if (this.m_bossAppear)
		{
			ObjUtil.SetModelVisible(base.gameObject, false);
		}
		this.m_time = 0f;
		this.m_state = ObjBossTrapBall.State.Start;
	}

	public void Shot(Vector3 colli)
	{
		if (this.m_model_obj)
		{
			Animator componentInChildren = base.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				if (colli.x > 0f)
				{
					componentInChildren.Play(this.m_typeParam.m_modelName1 + "_width");
				}
				else
				{
					componentInChildren.Play(this.m_typeParam.m_modelName1 + "_length");
				}
			}
		}
		if (this.m_colli_obj == null)
		{
			this.m_colli_obj = new GameObject[2];
		}
		if (this.m_colli_model_obj == null)
		{
			this.m_colli_model_obj = new GameObject[2];
		}
		for (int i = 0; i < 2; i++)
		{
			if (colli.x > 0f)
			{
				string name = (i != 0) ? this.m_typeParam.m_modelNameRight : this.m_typeParam.m_modelNameLeft;
				this.m_colli_obj[i] = this.CreateCollision(name);
				this.m_colli_model_obj[i] = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			}
			else
			{
				string name2 = (i != 0) ? this.m_typeParam.m_modelNameUnder : this.m_typeParam.m_modelNameTop;
				this.m_colli_obj[i] = this.CreateCollision(name2);
				this.m_colli_model_obj[i] = GameObjectUtil.FindChildGameObject(base.gameObject, name2);
			}
		}
	}

	private void StartAttack()
	{
		if (this.m_colli_obj != null)
		{
			for (int i = 0; i < this.m_colli_obj.Length; i++)
			{
				if (this.m_colli_obj[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_colli_obj[i].gameObject);
				}
			}
		}
		ObjUtil.PlaySE("boss_counterattack", "SE");
		this.m_time = 0f;
		this.m_rot_speed = 25f;
		this.m_state = ObjBossTrapBall.State.Attack;
	}

	private void HitAttack(GameObject obj)
	{
		if (obj)
		{
			MsgHitDamage value = new MsgHitDamage(base.gameObject, AttackPower.PlayerSpin);
			obj.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				this.HitAttack(gameObject);
			}
		}
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (msg.m_attackPower > 0 && msg.m_sender)
		{
			GameObject gameObject = msg.m_sender.gameObject;
			if (gameObject)
			{
				MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
				gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
				if (ObjBossTrapBall.IsBrokenPower(this.m_type, msg.m_attackPower) || gameObject.name == "ChaoPartsAttackEnemy")
				{
					this.SetBroken();
				}
				else if (this.m_type == BossTrapBallType.ATTACK)
				{
					this.StartAttack();
				}
			}
		}
	}

	private static bool IsBrokenPower(BossTrapBallType type, int plPower)
	{
		if (type != BossTrapBallType.ATTACK)
		{
			if (type == BossTrapBallType.BREAK)
			{
				return true;
			}
		}
		else
		{
			if (plPower == 5)
			{
				return false;
			}
			if (plPower >= 4)
			{
				return true;
			}
		}
		return false;
	}

	private void CreateModel(BossTrapBallType type)
	{
		string name = (type != BossTrapBallType.ATTACK) ? this.m_typeParam.m_modelName2 : this.m_typeParam.m_modelName1;
		GameObject gameObject = ResourceManager.Instance.GetGameObject(this.m_typeParam.m_resCategory, name);
		if (gameObject)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
			if (gameObject2)
			{
				gameObject2.gameObject.SetActive(true);
				gameObject2.transform.parent = base.gameObject.transform;
				gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
				this.m_model_obj = gameObject2;
			}
		}
	}

	private GameObject CreateCollision(string name)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
		if (gameObject)
		{
			GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossTrapBallCollision");
			if (gameObject2)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				if (gameObject3)
				{
					gameObject3.gameObject.SetActive(true);
					gameObject3.transform.parent = gameObject.transform;
					return gameObject3;
				}
			}
		}
		return null;
	}
}
