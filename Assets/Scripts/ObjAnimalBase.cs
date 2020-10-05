using Message;
using System;
using UnityEngine;

public class ObjAnimalBase : MonoBehaviour
{
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

	public static string EFFECT_NAME = "ef_ob_get_animal01";

	private ObjAnimalBase.State m_state;

	private float m_time;

	private float m_move_speed;

	private float m_hit_length;

	private int m_addCount = 1;

	private StageComboManager m_stageComboManager;

	private bool m_end;

	private bool m_share;

	private bool m_sleep;

	private AnimalType m_animalType = AnimalType.ERROR;

	public bool Sleep
	{
		set
		{
			this.m_sleep = value;
		}
	}

	public bool IsSleep()
	{
		return this.m_share && this.m_sleep;
	}

	private void Start()
	{
		this.m_move_speed = 0.12f * ObjUtil.GetPlayerAddSpeed();
		this.m_hit_length = this.GetCheckGroundHitLength();
		this.SetMotorThrowComponent();
		this.m_stageComboManager = StageComboManager.Instance;
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		this.m_time += deltaTime;
		switch (this.m_state)
		{
		case ObjAnimalBase.State.Jump:
			if (this.m_time > 0.3f)
			{
				if (this.UpdateCheckComboChaoAbility())
				{
					this.m_time = 0f;
					this.m_state = ObjAnimalBase.State.Drawing;
				}
				else
				{
					Vector3 zero = Vector3.zero;
					if (ObjUtil.CheckGroundHit(base.transform.position, base.transform.up, 1f, this.m_hit_length, out zero))
					{
						this.SetCollider(true);
						this.EndThrowComponent();
						this.StartNextComponent();
						this.m_time = 0f;
						this.m_state = ObjAnimalBase.State.Wait;
					}
					else if (this.m_time > 7f)
					{
						this.m_state = ObjAnimalBase.State.End;
						this.SleepOrDestroy();
					}
				}
			}
			break;
		case ObjAnimalBase.State.Wait:
			if (this.UpdateCheckComboChaoAbility())
			{
				this.m_time = 0f;
				this.m_state = ObjAnimalBase.State.Drawing;
			}
			else if (this.m_time > 7f)
			{
				this.m_state = ObjAnimalBase.State.End;
				this.SleepOrDestroy();
			}
			break;
		case ObjAnimalBase.State.Drawing:
			if (this.m_time > 7f)
			{
				this.m_state = ObjAnimalBase.State.End;
				this.SleepOrDestroy();
			}
			break;
		}
	}

	protected virtual float GetCheckGroundHitLength()
	{
		return 1f;
	}

	protected virtual void StartNextComponent()
	{
	}

	protected virtual void EndNextComponent()
	{
	}

	protected float GetMoveSpeed()
	{
		return this.m_move_speed;
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

	public void OnRevival()
	{
		base.enabled = true;
		this.m_end = false;
		this.m_state = ObjAnimalBase.State.Jump;
		this.m_time = 0f;
		this.m_move_speed = 0.12f * ObjUtil.GetPlayerAddSpeed();
		this.SetMotorThrowComponent();
	}

	public void SetShareState(AnimalType animalType)
	{
		this.m_share = true;
		this.m_sleep = true;
		this.m_animalType = animalType;
	}

	private void SleepOrDestroy()
	{
		if (this.m_share)
		{
			base.gameObject.SetActive(false);
			if (AnimalResourceManager.Instance != null)
			{
				AnimalResourceManager.Instance.SetSleep(this.m_animalType, base.gameObject);
			}
			this.EndThrowComponent();
			this.EndNextComponent();
			MagnetControl component = base.gameObject.GetComponent<MagnetControl>();
			if (component != null)
			{
				component.Reset();
			}
			this.SetCollider(false);
			this.m_state = ObjAnimalBase.State.End;
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
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player" || a == "Chao")
				{
					this.TakeAnimal();
				}
				else if (a == "Magnet")
				{
				}
			}
		}
	}

	private void OnDrawingRings(MsgOnDrawingRings msg)
	{
		if (this.m_state != ObjAnimalBase.State.Drawing && this.m_state != ObjAnimalBase.State.End)
		{
			ObjUtil.StartMagnetControl(base.gameObject);
			this.SetCollider(true);
			this.EndThrowComponent();
			this.EndNextComponent();
			this.m_time = 0f;
			this.m_state = ObjAnimalBase.State.Drawing;
		}
	}

	public void OnDestroyAnimal()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void TakeAnimal()
	{
		this.m_end = true;
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(EffectPlayType.ANIMAL, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
		ObjUtil.SendMessageAddAnimal(this.m_addCount);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(7, this.m_addCount));
		ObjUtil.LightPlaySE("obj_animal_get", "SE");
		ObjUtil.AddCombo();
		this.SleepOrDestroy();
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

	public static void DestroyAnimalEffect()
	{
		string name = ObjAnimalBase.EFFECT_NAME + "(Clone)";
		GameObject gameObject = GameObject.Find(name);
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private bool UpdateCheckComboChaoAbility()
	{
		if (this.m_stageComboManager != null && (this.m_stageComboManager.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ANIMAL) || this.m_stageComboManager.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ALL_OBJ) || this.m_stageComboManager.IsActiveComboChaoAbility(ChaoAbility.COMBO_DESTROY_AND_RECOVERY)))
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
			return true;
		}
		return false;
	}

	public void SetAnimalAddCount(int addCount)
	{
		this.m_addCount = addCount;
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
