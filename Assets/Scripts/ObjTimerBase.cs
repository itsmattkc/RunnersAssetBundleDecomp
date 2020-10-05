using Message;
using System;
using UnityEngine;

public class ObjTimerBase : SpawnableObject
{
	public enum MoveType
	{
		Thorw,
		Bound,
		Still
	}

	private enum State
	{
		Jump,
		Wait,
		Drawing,
		End
	}

	private const float JUMP_END_TIME = 0.3f;

	private const float WAIT_END_TIME = 7f;

	private const float ANIMAL_SPEED = 6f;

	private const float ANIMAL_GRAVITY = -6.1f;

	private const float ADD_SPEED = 0.12f;

	private const float ADD_X = 4.2f;

	private const float ADD_Y = 3f;

	private const float FLY_SPEED = 0.5f;

	private const float FLY_DISTANCE = 1f;

	private const float FLY_ADD_X = 1f;

	private const float GROUND_DISTANCE = 3f;

	private const float HIT_CHECK_DISTANCE = 2f;

	private float m_time;

	private float m_move_speed;

	private float m_hit_length;

	private bool m_end;

	private TimerType m_timerType = TimerType.ERROR;

	private float m_startPosY;

	private ObjTimerBase.MoveType m_moveType;

	private ObjTimerBase.State m_state;

	public void SetMoveType(ObjTimerBase.MoveType type)
	{
		this.m_moveType = type;
	}

	protected virtual TimerType GetTimerType()
	{
		return TimerType.ERROR;
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	private void Start()
	{
		this.m_timerType = this.GetTimerType();
		if (StageTimeManager.Instance != null)
		{
			StageTimeManager.Instance.ReserveExtendTime(this.GetExtendPattern());
		}
		MotorAnimalFly component = base.gameObject.GetComponent<MotorAnimalFly>();
		MotorThrow component2 = base.GetComponent<MotorThrow>();
		switch (this.m_moveType)
		{
		case ObjTimerBase.MoveType.Thorw:
			this.m_startPosY = base.transform.position.y;
			this.m_move_speed = 0.12f * ObjUtil.GetPlayerAddSpeed();
			this.m_hit_length = this.GetCheckGroundHitLength();
			this.SetMotorThrowComponent();
			break;
		case ObjTimerBase.MoveType.Bound:
			if (component != null)
			{
				component.enabled = false;
			}
			break;
		case ObjTimerBase.MoveType.Still:
			if (component != null)
			{
				component.enabled = false;
			}
			if (component2 != null)
			{
				component2.enabled = false;
			}
			break;
		}
	}

	protected override void OnSpawned()
	{
	}

	protected override void OnDestroyed()
	{
		if (StageTimeManager.Instance != null)
		{
			StageTimeManager.Instance.CancelReservedExtendTime(this.GetExtendPattern());
		}
	}

	private void Update()
	{
		switch (this.m_moveType)
		{
		case ObjTimerBase.MoveType.Thorw:
			this.UpdateThorwType();
			break;
		}
	}

	private void UpdateThorwType()
	{
		float deltaTime = Time.deltaTime;
		this.m_time += deltaTime;
		switch (this.m_state)
		{
		case ObjTimerBase.State.Jump:
			if (this.m_time > 0.3f)
			{
				if (this.CheckComboChaoAbility())
				{
					this.m_time = 0f;
					this.m_state = ObjTimerBase.State.Drawing;
					this.OnDrawingRings(new MsgOnDrawingRings());
				}
				else
				{
					Vector3 zero = Vector3.zero;
					if (ObjUtil.CheckGroundHit(base.transform.position, base.transform.up, 1f, this.m_hit_length, out zero) || this.m_startPosY > base.transform.position.y)
					{
						this.SetCollider(true);
						this.EndThrowComponent();
						this.StartNextComponent();
						this.m_time = 0f;
						this.m_state = ObjTimerBase.State.Wait;
					}
					else if (this.m_time > 7f)
					{
						this.m_state = ObjTimerBase.State.End;
						UnityEngine.Object.Destroy(base.gameObject);
					}
				}
			}
			break;
		case ObjTimerBase.State.Wait:
			if (this.CheckComboChaoAbility())
			{
				this.m_time = 0f;
				this.m_state = ObjTimerBase.State.Drawing;
				this.OnDrawingRings(new MsgOnDrawingRings());
			}
			else if (this.m_time > 7f)
			{
				this.m_state = ObjTimerBase.State.End;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case ObjTimerBase.State.Drawing:
			if (this.m_time > 7f)
			{
				this.m_state = ObjTimerBase.State.End;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		}
	}

	public void SetMotorThrowComponent()
	{
		MotorThrow component = base.GetComponent<MotorThrow>();
		if (component)
		{
			component.enabled = true;
			component.SetEnd();
			component.Setup(new MotorThrow.ThrowParam
			{
				m_obj = base.gameObject,
				m_speed = 6f,
				m_gravity = -6.1f,
				m_add_x = 4.2f + this.m_move_speed,
				m_add_y = 3f + this.m_move_speed,
				m_rot_speed = 0f,
				m_up = base.transform.up,
				m_forward = base.transform.right,
				m_rot_angle = Vector3.zero
			});
		}
	}

	private bool CheckComboChaoAbility()
	{
		return StageComboManager.Instance != null && (StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ALL_OBJ) || StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_DESTROY_AND_RECOVERY));
	}

	protected float GetCheckGroundHitLength()
	{
		return 2f;
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
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player" && gameObject.tag != "ChaoAttack")
				{
					this.TakeTimer();
				}
			}
		}
	}

	private StageTimeManager.ExtendPattern GetExtendPattern()
	{
		StageTimeManager.ExtendPattern result = StageTimeManager.ExtendPattern.UNKNOWN;
		switch (this.m_timerType)
		{
		case TimerType.BRONZE:
			result = StageTimeManager.ExtendPattern.BRONZE_WATCH;
			break;
		case TimerType.SILVER:
			result = StageTimeManager.ExtendPattern.SILVER_WATCH;
			break;
		case TimerType.GOLD:
			result = StageTimeManager.ExtendPattern.GOLD_WATCH;
			break;
		}
		return result;
	}

	private int GetAtlasIndex()
	{
		int result = 0;
		switch (this.m_timerType)
		{
		case TimerType.BRONZE:
			result = 0;
			break;
		case TimerType.SILVER:
			result = 1;
			break;
		case TimerType.GOLD:
			result = 2;
			break;
		}
		return result;
	}

	private int GetAddTimer()
	{
		int result = 0;
		if (StageTimeManager.Instance != null)
		{
			result = (int)StageTimeManager.Instance.GetExtendTime(this.GetExtendPattern());
		}
		return result;
	}

	private void TakeTimer()
	{
		this.m_end = true;
		if (StageTimeManager.Instance != null)
		{
			StageTimeManager.Instance.ExtendTime(this.GetExtendPattern());
		}
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, ObjTimerUtil.GetEffectName(this.m_timerType), 1f, false);
		ObjUtil.PlaySE(ObjTimerUtil.GetSEName(this.m_timerType), "SE");
		ObjUtil.SendGetTimerIcon(this.GetAtlasIndex(), this.GetAddTimer());
		ObjUtil.AddCombo();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected void StartNextComponent()
	{
		MotorAnimalFly component = base.GetComponent<MotorAnimalFly>();
		if (component)
		{
			component.enabled = true;
			component.SetupParam(0.5f, 1f, 1f + this.m_move_speed, base.transform.right, 3f, true);
		}
	}

	protected void EndNextComponent()
	{
		MotorAnimalFly component = base.GetComponent<MotorAnimalFly>();
		if (component)
		{
			component.enabled = false;
			component.SetEnd();
		}
	}

	private void EndThrowComponent()
	{
		MotorThrow component = base.GetComponent<MotorThrow>();
		if (component)
		{
			component.enabled = false;
			component.SetEnd();
		}
	}

	private void OnDrawingRings(MsgOnDrawingRings msg)
	{
		if (this.m_state != ObjTimerBase.State.Drawing && this.m_state != ObjTimerBase.State.End)
		{
			ObjUtil.StartMagnetControl(base.gameObject);
			this.SetCollider(true);
			this.EndThrowComponent();
			this.EndNextComponent();
			this.m_time = 0f;
			this.m_state = ObjTimerBase.State.Drawing;
		}
	}

	private void OnDrawingRingsChaoAbility(MsgOnDrawingRings msg)
	{
		if (msg.m_chaoAbility == ChaoAbility.COMBO_RECOVERY_ALL_OBJ || msg.m_chaoAbility == ChaoAbility.COMBO_DESTROY_AND_RECOVERY)
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
	}

	private void SetCollider(bool on)
	{
		SphereCollider component = base.GetComponent<SphereCollider>();
		if (component != null)
		{
			component.enabled = on;
		}
	}
}
