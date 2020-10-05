using Message;
using System;
using UnityEngine;

public class ObjBossBom : MonoBehaviour
{
	private enum State
	{
		Idle,
		Start,
		Down,
		Bom,
		Wait
	}

	private const string MODEL_NAME = "obj_boss_bomb";

	private const ResourceCategory MODEL_CATEGORY = ResourceCategory.OBJECT_RESOURCE;

	private const float BOM_END_TIME = 0.1f;

	private const float WAIT_END_TIME = 5f;

	private const float BALL_GRAVITY = -6.1f;

	private ObjBossBom.State m_state;

	private float m_time;

	private bool m_hit;

	private GameObject m_boss_obj;

	private Quaternion m_shot_rotation = Quaternion.identity;

	private float m_shot_speed;

	private float m_bom_pos_y;

	private float m_wait_time;

	private string m_blast_effect_name = string.Empty;

	private float m_blast_destroy_time;

	private float m_add_speed = 1f;

	private bool m_shot;

	private bool m_playerDead;

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		switch (this.m_state)
		{
		case ObjBossBom.State.Start:
			ObjBossUtil.SetupBallAppear(this.m_boss_obj, base.gameObject);
			this.m_state = ObjBossBom.State.Down;
			break;
		case ObjBossBom.State.Down:
			if (this.m_boss_obj && ObjBossUtil.UpdateBallAppear(deltaTime, this.m_boss_obj, base.gameObject, this.m_add_speed) && this.m_shot)
			{
				ObjBossUtil.PlayShotEffect(this.m_boss_obj);
				ObjBossUtil.PlayShotSE();
				this.MotorShot();
				this.m_wait_time = 5f;
				this.m_state = ObjBossBom.State.Wait;
			}
			break;
		case ObjBossBom.State.Bom:
			ObjUtil.PlayEffectCollisionCenter(base.gameObject, this.m_blast_effect_name, this.m_blast_destroy_time, true);
			ObjUtil.PlaySE("obj_common_exp", "SE");
			this.m_time = 0f;
			this.m_wait_time = 0.1f;
			this.m_state = ObjBossBom.State.Wait;
			break;
		case ObjBossBom.State.Wait:
			this.m_time += deltaTime;
			if (this.m_time > this.m_wait_time)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		}
	}

	public void OnMsgObjectDead(MsgObjectDead msg)
	{
		if (base.enabled && this.m_hit)
		{
			this.SetBroken();
		}
	}

	private void SetBroken()
	{
		this.Blast("ef_bo_em_bom01", 2f);
	}

	public void Setup(GameObject obj, bool colli, Quaternion shot_rot, float shot_speed, float add_speed, bool shot)
	{
		this.CreateModel();
		ObjUtil.SetModelVisible(base.gameObject, false);
		this.m_hit = colli;
		this.m_boss_obj = obj;
		this.m_shot_rotation = shot_rot;
		this.m_shot_speed = shot_speed;
		this.m_add_speed = add_speed;
		this.m_shot = shot;
		this.m_state = ObjBossBom.State.Start;
	}

	public void Blast(string name, float destroy_time)
	{
		this.m_blast_effect_name = name;
		this.m_blast_destroy_time = destroy_time;
		this.m_state = ObjBossBom.State.Bom;
	}

	public void SetShot(bool shot)
	{
		this.m_shot = shot;
	}

	public void MotorShot()
	{
		MotorShot component = base.GetComponent<MotorShot>();
		if (component)
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
			SphereCollider component2 = base.GetComponent<SphereCollider>();
			if (component2)
			{
				this.m_bom_pos_y = component2.radius;
			}
			shotParam.m_bound_pos_y = this.m_bom_pos_y;
			shotParam.m_bound_add_y = 0f;
			component.Setup(shotParam);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other && this.m_hit)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				MsgHitDamage value = new MsgHitDamage(base.gameObject, AttackPower.PlayerColorPower);
				gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
				this.SetBroken();
			}
		}
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (this.m_hit && msg.m_attackPower >= 4 && msg.m_sender)
		{
			GameObject gameObject = msg.m_sender.gameObject;
			if (gameObject)
			{
				this.SetBroken();
			}
		}
	}

	private void CreateModel()
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_RESOURCE, "obj_boss_bomb");
		if (gameObject)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
			if (gameObject2)
			{
				gameObject2.gameObject.SetActive(true);
				gameObject2.transform.parent = base.gameObject.transform;
				gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
		}
	}

	public void OnMsgNotifyDead(MsgNotifyDead msg)
	{
		this.m_playerDead = true;
	}
}
