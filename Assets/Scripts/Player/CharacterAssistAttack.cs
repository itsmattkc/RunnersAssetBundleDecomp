using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterAssistAttack : MonoBehaviour
	{
		private enum Mode
		{
			Homing,
			ForceHoming,
			Up,
			Down
		}

		private const float FirstUpSpeed = 5f;

		private const float AttackUpSpeed = 10f;

		private const float LimitTime = 0.8f;

		private const float ForcedHomingTime = 0.2f;

		private const float GraityAcc = 35f;

		private const float TargetLimitOffset = 1f;

		private const float LimitForcedHomingTime = 2f;

		private const float FirstSpeedRate = 4f;

		private string m_name;

		private bool m_hitDamage;

		private float m_firstSpeed;

		private Vector3 m_velocity;

		private CharacterAssistAttack.Mode m_mode;

		private float m_timer;

		private GameObject m_targetObject;

		private Vector3 m_targetPosition;

		private Camera m_mainCamera;

		private Animator m_animation;

		private void Start()
		{
			if (this.m_animation != null)
			{
				this.m_animation.SetBool("Jump", true);
			}
		}

		public void Setup(string name, Vector3 playerPos, float speed)
		{
			this.m_name = name;
			GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
			if (gameObject != null)
			{
				Camera component = gameObject.GetComponent<Camera>();
				if (component != null)
				{
					Vector3 position = component.WorldToScreenPoint(playerPos);
					position = new Vector3(0f, component.pixelHeight * 0.5f, position.z);
					Vector3 position2 = component.ScreenToWorldPoint(position);
					base.transform.position = position2;
					base.transform.rotation = Quaternion.FromToRotation(Vector3.forward, CharacterDefs.BaseFrontTangent);
					this.m_mainCamera = component;
				}
			}
			string text = "chr_" + this.m_name;
			text = text.ToLower();
			GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.CHARA_MODEL, text);
			GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, base.transform.position, base.transform.rotation) as GameObject;
			if (gameObject3 != null)
			{
				Vector3 localPosition = gameObject2.transform.localPosition;
				Quaternion localRotation = gameObject2.transform.localRotation;
				gameObject3.transform.parent = base.transform;
				gameObject3.SetActive(true);
				gameObject3.transform.localPosition = localPosition;
				gameObject3.transform.localRotation = localRotation;
				this.m_animation = gameObject3.GetComponent<Animator>();
			}
			MsgBossInfo msgBossInfo = new MsgBossInfo();
			GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgBossInfo", msgBossInfo, SendMessageOptions.DontRequireReceiver);
			if (msgBossInfo.m_succeed)
			{
				this.m_targetObject = msgBossInfo.m_boss;
				this.m_targetPosition = msgBossInfo.m_position;
			}
			this.m_firstSpeed = speed * 4f;
			this.m_velocity = this.m_firstSpeed * base.transform.forward + 5f * Vector3.up;
		}

		private void Update()
		{
			switch (this.m_mode)
			{
			case CharacterAssistAttack.Mode.Homing:
				this.UpdateHoming(Time.deltaTime);
				break;
			case CharacterAssistAttack.Mode.ForceHoming:
				this.UpdateForcedHoming(Time.deltaTime);
				break;
			case CharacterAssistAttack.Mode.Up:
				this.UpdateUp(Time.deltaTime);
				break;
			case CharacterAssistAttack.Mode.Down:
				this.UpdateDown(Time.deltaTime);
				break;
			}
		}

		private void UpdateHoming(float deltaTime)
		{
			this.m_timer += deltaTime;
			this.UpdateTarget();
			if (this.m_targetObject == null)
			{
				base.transform.position += this.m_velocity * deltaTime;
				this.GoDown();
				return;
			}
			float firstSpeed = this.m_firstSpeed;
			this.MoveHoming(firstSpeed, deltaTime);
			if (this.m_timer > 0.8f)
			{
				this.m_mode = CharacterAssistAttack.Mode.ForceHoming;
				this.m_timer = 0f;
			}
		}

		private void UpdateForcedHoming(float deltaTime)
		{
			this.m_timer += deltaTime;
			if (!this.UpdateTarget())
			{
				base.transform.position += this.m_velocity * deltaTime;
				this.GoDown();
				return;
			}
			float magnitude = (this.m_targetPosition - base.transform.position).magnitude;
			float speed = magnitude / 0.2f;
			this.MoveHoming(speed, deltaTime);
			if (this.m_timer > 2f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}

		private void UpdateUp(float deltaTime)
		{
			base.transform.position += this.m_velocity * deltaTime;
			this.m_timer += deltaTime;
			if (this.m_timer > 1f || this.IsOutsideOfCamera(base.transform.position))
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}

		private void UpdateDown(float deltaTime)
		{
			this.m_velocity += -Vector3.up * 35f * deltaTime;
			base.transform.position += this.m_velocity * deltaTime;
			this.m_timer += deltaTime;
			if (this.m_timer > 1f || this.IsOutsideOfCamera(base.transform.position))
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}

		private void GoUp()
		{
			this.m_mode = CharacterAssistAttack.Mode.Up;
			this.m_velocity = base.transform.forward * 4f + 10f * Vector3.up;
			this.m_timer = 0f;
		}

		private void GoDown()
		{
			this.m_mode = CharacterAssistAttack.Mode.Down;
			this.m_timer = 0f;
		}

		private void MoveHoming(float speed, float deltaTime)
		{
			Vector3 vector = this.m_targetPosition - base.transform.position;
			float magnitude = vector.magnitude;
			Vector3 normalized = vector.normalized;
			if (magnitude < speed * deltaTime)
			{
				base.transform.position = this.m_targetPosition;
			}
			else
			{
				this.m_velocity = normalized * speed;
				base.transform.position += this.m_velocity * deltaTime;
			}
		}

		private bool UpdateTarget()
		{
			if (this.m_targetObject != null)
			{
				MsgBossInfo msgBossInfo = new MsgBossInfo();
				this.m_targetObject.SendMessage("OnMsgBossInfo", msgBossInfo);
				if (msgBossInfo.m_succeed)
				{
					Vector3 position = msgBossInfo.m_position;
					if (position.x + 1f > base.transform.position.x && !this.IsOutsideOfCamera(position))
					{
						this.m_targetPosition = position;
						return true;
					}
				}
			}
			return false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject != this.m_targetObject)
			{
				return;
			}
			AttackPower attack = AttackPower.PlayerInvincible;
			MsgHitDamage value = new MsgHitDamage(base.gameObject, attack);
			other.gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
			this.GoUp();
		}

		private bool IsOutsideOfCamera(Vector3 position)
		{
			if (this.m_mainCamera != null)
			{
				Vector3 vector = this.m_mainCamera.WorldToViewportPoint(position);
				return vector.x < -0.3f || vector.x > 1.3f || vector.y < -0.3f || vector.y > 1.3f;
			}
			return true;
		}
	}
}
