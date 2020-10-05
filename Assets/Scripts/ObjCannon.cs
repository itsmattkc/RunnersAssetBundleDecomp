using Message;
using Player;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjCannon")]
public class ObjCannon : SpawnableObject
{
	private enum State
	{
		Start,
		Idle,
		Wait,
		Set,
		Shot
	}

	private const string ModelName = "obj_cmn_cannon";

	private const float m_roundParam = 5f;

	private float m_firstSpeed = 5f;

	private float m_outOfcontrol = 0.5f;

	private GameObject m_sendObject;

	private float m_moveSpeed = 0.4f;

	private float m_rotArea = 25f;

	private float m_time;

	private bool m_shot;

	private Quaternion m_centerRotation = Quaternion.identity;

	private Quaternion m_startRotation = Quaternion.identity;

	private CharacterInput m_input;

	private CameraManager m_camera;

	private ObjCannon.State m_state;

	protected override string GetModelName()
	{
		return "obj_cmn_cannon";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected virtual string GetShotEffectName()
	{
		return "ef_ob_canon_st01";
	}

	protected virtual string GetShotAnimName()
	{
		return "obj_cannon_shot";
	}

	protected virtual bool IsRoulette()
	{
		return false;
	}

	protected override void OnSpawned()
	{
		this.m_input = base.GetComponent<CharacterInput>();
		ObjUtil.StopAnimation(base.gameObject);
		this.m_centerRotation = Quaternion.Euler(0f, 0f, -45f) * base.transform.rotation;
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		switch (this.m_state)
		{
		case ObjCannon.State.Start:
			this.SetGuideline(false);
			this.m_state = ObjCannon.State.Idle;
			break;
		case ObjCannon.State.Set:
			this.UpdateStart(deltaTime);
			break;
		case ObjCannon.State.Shot:
			this.UpdateMove(deltaTime);
			break;
		}
		this.UpdateInputShot();
	}

	public void SetObjCannonParameter(ObjCannonParameter param)
	{
		this.m_firstSpeed = param.firstSpeed;
		this.m_outOfcontrol = param.outOfcontrol;
		this.m_moveSpeed = param.moveSpeed;
		this.m_rotArea = param.moveArea * 0.5f;
	}

	protected virtual Quaternion GetStartRot()
	{
		return Quaternion.Euler(0f, 0f, -this.m_rotArea) * this.m_centerRotation;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_state == ObjCannon.State.Idle)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				Quaternion rot = Quaternion.FromToRotation(base.transform.up, base.transform.right) * base.transform.rotation;
				MsgOnAbidePlayer msgOnAbidePlayer = new MsgOnAbidePlayer(base.transform.position, rot, 1f, base.gameObject);
				gameObject.SendMessage("OnAbidePlayer", msgOnAbidePlayer, SendMessageOptions.DontRequireReceiver);
				if (msgOnAbidePlayer.m_succeed)
				{
					ObjUtil.PlaySE("obj_cannon_in", "SE");
					this.m_sendObject = gameObject;
					this.m_state = ObjCannon.State.Wait;
				}
			}
		}
	}

	private void UpdateStart(float delta)
	{
		this.m_time += delta;
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.m_startRotation, this.m_time * this.m_moveSpeed * 0.5f * Time.timeScale);
		float num = Mathf.Abs(Vector3.Distance(base.transform.rotation.eulerAngles, this.m_startRotation.eulerAngles));
		if (num < 0.1f)
		{
			this.m_time = 0f;
			this.m_state = ObjCannon.State.Shot;
		}
	}

	private void UpdateMove(float delta)
	{
		if (this.m_rotArea > 0f)
		{
			this.m_time += delta;
			float num = Mathf.Cos(6.28318548f * this.m_time * this.m_moveSpeed) * -1f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, num * this.m_rotArea) * this.m_centerRotation;
		}
	}

	private void UpdateInputShot()
	{
		if (this.m_shot && this.m_input && this.m_input.IsTouched())
		{
			this.Shot();
			this.SetGuideline(false);
			this.m_shot = false;
			this.m_state = ObjCannon.State.Idle;
		}
	}

	private void Shot()
	{
		if (this.m_sendObject)
		{
			Quaternion rot = Quaternion.FromToRotation(base.transform.up, base.transform.right) * base.transform.rotation;
			if (!this.IsRoulette())
			{
				float num = rot.eulerAngles.z + 2.5f;
				if (num != 0f)
				{
					int num2 = (int)(num / 5f);
					rot = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, 5f * (float)num2);
				}
			}
			MsgOnCannonImpulse value = new MsgOnCannonImpulse(base.transform.position, rot, this.m_firstSpeed, this.m_outOfcontrol, this.IsRoulette());
			this.m_sendObject.SendMessage("OnCannonImpulse", value, SendMessageOptions.DontRequireReceiver);
			Animation componentInChildren = base.GetComponentInChildren<Animation>();
			if (componentInChildren)
			{
				componentInChildren.wrapMode = WrapMode.Once;
				componentInChildren.Play(this.GetShotAnimName());
			}
			ObjUtil.PlayEffectChild(base.gameObject, this.GetShotEffectName(), new Vector3(1f, 0f, 0f), Quaternion.Euler(new Vector3(0f, 0f, -90f)), 1f, true);
			ObjUtil.PlaySE("obj_cannon_shoot", "SE");
		}
		ObjUtil.PopCamera(CameraType.CANNON, 0.5f);
	}

	private void OnAbidePlayerLocked(MsgOnAbidePlayerLocked msg)
	{
		this.SetGuideline(true);
		this.m_startRotation = this.GetStartRot();
		this.m_state = ObjCannon.State.Set;
		this.m_shot = true;
		ObjUtil.PushCamera(CameraType.CANNON, 0.5f);
	}

	private void OnExitAbideObject(MsgOnExitAbideObject msg)
	{
		this.m_state = ObjCannon.State.Idle;
	}

	private void SetGuideline(bool on)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "obj_cmn_cannonguideline");
		if (gameObject != null)
		{
			gameObject.SetActive(on);
		}
	}
}
