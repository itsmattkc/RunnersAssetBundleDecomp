using Message;
using System;
using UnityEngine;

public class ObjBossBall : MonoBehaviour
{
	public struct SetData
	{
		public GameObject obj;

		public float bound_param;

		public BossBallType type;

		public Quaternion shot_rot;

		public float shot_speed;

		public float attack_speed;

		public float firstSpeed;

		public float outOfcontrol;

		public float ballSpeed;

		public bool bossAppear;
	}

	private enum State
	{
		Idle,
		Start,
		Down,
		Bound,
		Attack
	}

	private const ResourceCategory MODEL_CATEGORY = ResourceCategory.OBJECT_RESOURCE;

	private const float END_TIME = 5f;

	private const float BALL_GRAVITY = -6.1f;

	private const float ATTACK_ROT_SPEED = 10f;

	private static readonly string[] MODEL_FILES = new string[]
	{
		"obj_boss_ironball",
		"obj_boss_thornball",
		"obj_boss_bumper"
	};

	private ObjBossBall.State m_state;

	private float m_time;

	private float m_bound_param;

	private BossBallType m_type;

	private GameObject m_boss_obj;

	private GameObject m_model_obj;

	private MotorShot m_motor_cmp;

	private Quaternion m_shot_rotation = Quaternion.identity;

	private float m_shot_speed;

	private float m_attack_speed;

	private float m_add_speed = 1f;

	private float m_firstSpeed;

	private float m_outOfcontrol;

	private bool m_playerDead;

	private float m_ballSpeed;

	private bool m_bossAppear;

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		switch (this.m_state)
		{
		case ObjBossBall.State.Start:
			if (this.m_bossAppear)
			{
				ObjBossUtil.SetupBallAppear(this.m_boss_obj, base.gameObject);
			}
			this.m_state = ObjBossBall.State.Down;
			break;
		case ObjBossBall.State.Down:
			if (this.m_boss_obj)
			{
				if (this.m_bossAppear)
				{
					if (ObjBossUtil.UpdateBallAppear(deltaTime, this.m_boss_obj, base.gameObject, this.m_add_speed))
					{
						ObjBossUtil.PlayShotEffect(this.m_boss_obj);
						ObjBossUtil.PlayShotSE();
						this.MotorShot();
						this.m_time = 0f;
						this.m_state = ObjBossBall.State.Bound;
					}
				}
				else
				{
					this.MotorShot();
					this.m_time = 0f;
					this.m_state = ObjBossBall.State.Bound;
				}
			}
			break;
		case ObjBossBall.State.Bound:
			this.m_time += deltaTime;
			if (this.m_time > 5f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case ObjBossBall.State.Attack:
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
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_com_explosion_s01", 1f, false);
		ObjUtil.PlaySE("obj_brk", "SE");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Setup(ObjBossBall.SetData setData)
	{
		this.CreateModel(setData.type);
		this.m_state = ObjBossBall.State.Start;
		this.m_boss_obj = setData.obj;
		this.m_bound_param = setData.bound_param;
		this.m_type = setData.type;
		this.m_shot_rotation = setData.shot_rot;
		this.m_shot_speed = setData.shot_speed;
		this.m_attack_speed = setData.attack_speed;
		this.m_firstSpeed = setData.firstSpeed;
		this.m_outOfcontrol = setData.outOfcontrol;
		this.m_ballSpeed = setData.ballSpeed;
		this.m_bossAppear = setData.bossAppear;
		if (this.m_bossAppear)
		{
			ObjUtil.SetModelVisible(base.gameObject, false);
		}
	}

	public void MotorShot()
	{
		this.m_motor_cmp = base.GetComponent<MotorShot>();
		if (this.m_motor_cmp)
		{
			MotorShot.ShotParam shotParam = new MotorShot.ShotParam();
			shotParam.m_obj = base.gameObject;
			shotParam.m_gravity = -6.1f;
			shotParam.m_rot_speed = 0f;
			shotParam.m_rot_downspeed = 0f;
			shotParam.m_rot_angle = Vector3.zero;
			shotParam.m_shot_rotation = ObjBossUtil.GetShotRotation(this.m_shot_rotation, this.m_playerDead);
			shotParam.m_shot_time = 1f;
			shotParam.m_shot_speed = this.m_shot_speed;
			shotParam.m_shot_downspeed = 0f;
			shotParam.m_bound = true;
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component)
			{
				shotParam.m_bound_pos_y = component.radius;
			}
			shotParam.m_bound_add_y = Mathf.Max(this.m_bound_param, 0f);
			shotParam.m_bound_down_x = 0f;
			shotParam.m_bound_down_y = 0.01f;
			shotParam.m_after_speed = this.m_ballSpeed;
			shotParam.m_after_add_x = 0f;
			shotParam.m_after_up = base.transform.up;
			shotParam.m_after_forward = base.transform.right;
			this.m_motor_cmp.Setup(shotParam);
		}
	}

	private void StartAttack()
	{
		this.m_time = 0f;
		this.m_state = ObjBossBall.State.Attack;
		ObjUtil.PlaySE("boss_counterattack", "SE");
		if (this.m_motor_cmp)
		{
			this.m_motor_cmp.SetEnd();
		}
	}

	private void HitAttack(GameObject obj)
	{
		if (obj)
		{
			AttackPower myPower = ObjBossBall.GetMyPower(this.m_type);
			MsgHitDamage value = new MsgHitDamage(base.gameObject, myPower);
			obj.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_type == BossBallType.BUMPER)
		{
			MsgOnSpringImpulse msgOnSpringImpulse = new MsgOnSpringImpulse(base.transform.position, base.transform.rotation, this.m_firstSpeed, this.m_outOfcontrol);
			other.gameObject.SendMessage("OnSpringImpulse", msgOnSpringImpulse, SendMessageOptions.DontRequireReceiver);
			if (this.m_boss_obj != null)
			{
				this.m_boss_obj.SendMessage("OnHitBumper", SendMessageOptions.DontRequireReceiver);
			}
			if (msgOnSpringImpulse.m_succeed)
			{
				if (this.m_model_obj != null)
				{
					Animation componentInChildren = this.m_model_obj.GetComponentInChildren<Animation>();
					if (componentInChildren)
					{
						componentInChildren.wrapMode = WrapMode.Once;
						componentInChildren.Play("obj_boss_bumper_bounce");
					}
				}
				ObjUtil.PlaySE("obj_spring", "SE");
			}
		}
		else if (other)
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
		if (this.m_type == BossBallType.BUMPER)
		{
			return;
		}
		if (ObjBossBall.IsAttackPower(this.m_type, msg.m_attackPower) && msg.m_sender)
		{
			GameObject gameObject = msg.m_sender.gameObject;
			if (gameObject)
			{
				MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
				gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
				if (ObjBossBall.IsBrokenPower(this.m_type, msg.m_attackPower) || gameObject.name == "ChaoPartsAttackEnemy")
				{
					this.SetBroken();
				}
				else
				{
					this.StartAttack();
				}
			}
		}
	}

	private static bool IsAttackPower(BossBallType type, int plPower)
	{
		AttackPower attackPower = AttackPower.PlayerSpin;
		if (type == BossBallType.TRAP)
		{
			attackPower = AttackPower.PlayerColorPower;
		}
		return plPower >= (int)attackPower;
	}

	private static bool IsBrokenPower(BossBallType type, int plPower)
	{
		if (type == BossBallType.ATTACK)
		{
			if (plPower == 5)
			{
				return false;
			}
		}
		return plPower >= 4;
	}

	private static AttackPower GetMyPower(BossBallType type)
	{
		AttackPower result = AttackPower.PlayerSpin;
		if (type == BossBallType.TRAP)
		{
			result = AttackPower.PlayerColorPower;
		}
		return result;
	}

	private void CreateModel(BossBallType type)
	{
		if (type < (BossBallType)ObjBossBall.MODEL_FILES.Length)
		{
			string name = ObjBossBall.MODEL_FILES[(int)type];
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_RESOURCE, name);
			if (gameObject)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject2)
				{
					ObjUtil.StopAnimation(gameObject2);
					gameObject2.gameObject.SetActive(true);
					gameObject2.transform.parent = base.gameObject.transform;
					gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
					this.m_model_obj = gameObject2;
				}
			}
		}
	}

	public void OnMsgNotifyDead(MsgNotifyDead msg)
	{
		this.m_playerDead = true;
	}
}
