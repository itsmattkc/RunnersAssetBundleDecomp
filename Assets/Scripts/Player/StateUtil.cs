using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateUtil
	{
		private const float RunEffectTime = 2f;

		private const string MagnetObjectName = "CharacterMagnet";

		private const string ChaoAbilityMagnetObjectName = "CharacterMagnetChaoAbility";

		private const string InvincibleObjectName = "CharacterInvincible";

		private const string BarrierObjectName = "CharacterBarrier";

		private const string ComboObjectName = "CharacterCombo";

		private const string TrampoilneObjectName = "CharacterTrampoline";

		private const int NumAirActionNothing = 0;

		private const int NumAirActionJump = 1;

		public static bool IsAnimationEnd(CharacterState context)
		{
			return context.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f;
		}

		public static bool IsAnimationEnd(CharacterState context, string animName)
		{
			if (StateUtil.IsAnimationInTransition(context))
			{
				return false;
			}
			string name = "Base Layer." + animName;
			AnimatorStateInfo currentAnimatorStateInfo = context.GetAnimator().GetCurrentAnimatorStateInfo(0);
			return currentAnimatorStateInfo.IsName(name) && currentAnimatorStateInfo.normalizedTime > 0.98f;
		}

		public static bool IsAnimationInTransition(CharacterState context)
		{
			return context.GetAnimator().IsInTransition(0);
		}

		public static bool IsCurrentAnimation(CharacterState context, string animName, bool notTransition)
		{
			if (notTransition && StateUtil.IsAnimationInTransition(context))
			{
				return false;
			}
			string name = "Base Layer." + animName;
			return context.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(name);
		}

		public static bool SetRunningAnimationSpeed(CharacterState context, ref float animationSpeed)
		{
			float spinDashSpeed = context.Parameter.m_spinDashSpeed;
			float magnitude = context.Movement.HorzVelocity.magnitude;
			bool result = false;
			float num;
			if (magnitude > spinDashSpeed)
			{
				num = 1f;
				result = true;
			}
			else
			{
				num = Mathf.Clamp(magnitude / spinDashSpeed, 0f, 1f) * 0.9f;
			}
			context.GetAnimator().SetFloat("RunSpeed", num);
			animationSpeed = num;
			return result;
		}

		public static GameObject CreateEffect(MonoBehaviour context, string effectname, bool recreate)
		{
			return StateUtil.CreateEffect(context, context.gameObject, effectname, recreate, ResourceCategory.COMMON_EFFECT);
		}

		public static GameObject CreateEffect(MonoBehaviour context, string effectname, bool recreate, ResourceCategory category)
		{
			return StateUtil.CreateEffect(context, context.gameObject, effectname, recreate, category);
		}

		public static GameObject CreateEffect(MonoBehaviour context, GameObject parentObject, string effectname, bool recreate, ResourceCategory category)
		{
			if (parentObject == null)
			{
				parentObject = context.gameObject;
			}
			if (!recreate)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
				if (gameObject != null)
				{
					gameObject.SetActive(true);
					return gameObject;
				}
			}
			if (ResourceManager.Instance)
			{
				GameObject gameObject2 = ResourceManager.Instance.GetGameObject(category, effectname);
				if (gameObject2)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, parentObject.transform.position, parentObject.transform.rotation) as GameObject;
					if (gameObject3 != null)
					{
						gameObject3.name = effectname;
						gameObject3.SetActive(true);
						gameObject3.transform.parent = parentObject.transform;
						return gameObject3;
					}
				}
			}
			return null;
		}

		public static GameObject CreateEffectOnTransform(MonoBehaviour context, Transform transform, string effectname, bool recreate)
		{
			return StateUtil.CreateEffectOnTransform(context, transform, effectname, recreate, ResourceCategory.COMMON_EFFECT);
		}

		public static GameObject CreateEffectOnTransform(MonoBehaviour context, Transform transform, string effectname, bool recreate, ResourceCategory category)
		{
			if (!recreate)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
				if (gameObject != null)
				{
					gameObject.SetActive(true);
					return gameObject;
				}
			}
			if (ResourceManager.Instance)
			{
				GameObject gameObject2 = ResourceManager.Instance.GetGameObject(category, effectname);
				if (gameObject2)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, transform.position, transform.rotation) as GameObject;
					if (gameObject3 != null)
					{
						gameObject3.name = effectname;
						gameObject3.SetActive(true);
						gameObject3.transform.parent = context.transform;
						return gameObject3;
					}
				}
			}
			return null;
		}

		public static GameObject CreateEffect(MonoBehaviour context, Vector3 position, Quaternion rotation, string effectname, bool recreate)
		{
			return StateUtil.CreateEffect(context, position, rotation, effectname, recreate, ResourceCategory.COMMON_EFFECT);
		}

		public static GameObject CreateEffect(MonoBehaviour context, Vector3 position, Quaternion rotation, string effectname, bool recreate, ResourceCategory category)
		{
			if (!recreate)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
				if (gameObject != null)
				{
					gameObject.SetActive(true);
					return gameObject;
				}
			}
			if (ResourceManager.Instance)
			{
				GameObject gameObject2 = ResourceManager.Instance.GetGameObject(category, effectname);
				if (gameObject2)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, position, rotation) as GameObject;
					if (gameObject3 != null)
					{
						gameObject3.name = effectname;
						gameObject3.SetActive(true);
						return gameObject3;
					}
				}
			}
			return null;
		}

		public static void DestroyEffect(MonoBehaviour context, string effectname)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		public static void StopEffect(MonoBehaviour context, string effectname, float destroyTime)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
			if (gameObject != null)
			{
				StateUtil.DestroyParticle(gameObject, destroyTime);
			}
		}

		public static void SetActiveEffect(MonoBehaviour context, string effectname, bool value)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, effectname);
			if (gameObject != null)
			{
				gameObject.SetActive(value);
			}
		}

		public static void CreateJumpEffect(MonoBehaviour context)
		{
			if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.PlayEffect(EffectPlayType.JUMP, context.transform.position, context.transform.rotation);
			}
		}

		public static void CreateLandingEffect(CharacterState context)
		{
			if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.PlayEffect(EffectPlayType.LAND, context.transform.position, context.transform.rotation);
			}
		}

		public static void CreateRunEffect(CharacterState context, float animSpeed)
		{
			if (context.GetLevelInformation().LightMode && context.BossBoostLevel == WispBoostLevel.NONE)
			{
				return;
			}
			if (context.BossBoostLevel != WispBoostLevel.NONE)
			{
				ResourceCategory category = ResourceCategory.EVENT_RESOURCE;
				string bossBoostEffect = context.BossBoostEffect;
				StateUtil.CreateEffect(context, context.transform.position, context.transform.rotation, bossBoostEffect, true, category);
			}
			else if (StageEffectManager.Instance != null)
			{
				StageEffectManager.Instance.PlayEffect((animSpeed < 0.6f) ? EffectPlayType.RUN : EffectPlayType.FAST_RUN, context.transform.position, context.transform.rotation);
			}
		}

		public static void CreateStartBoostEffect(CharacterState context)
		{
			if (context.CharacterName != null)
			{
				string effectname = "ef_pl_" + context.CharacterName + "_boost_st01";
				StateUtil.CreateEffect(context, context.transform.position, context.transform.rotation, effectname, true, ResourceCategory.CHARA_EFFECT);
			}
		}

		public static void Create2ndJumpEffect(CharacterState context)
		{
			CharaSEUtil.Play2ndJumpSE(context.charaType);
			string effectname = "ef_pl_" + context.CharacterName + "_2stepjump01";
			GameObject gameobj = StateUtil.CreateEffect(context, effectname, true, ResourceCategory.CHARA_EFFECT);
			StateUtil.SetObjectLocalPositionToCenter(context, gameobj);
		}

		public static void CheckAndCreateRunEffect(CharacterState context, ref float effectTimer, float speed, float animationSpeed, float deltaTime)
		{
			effectTimer -= speed * deltaTime;
			if (effectTimer < 0f)
			{
				StateUtil.CreateRunEffect(context, animationSpeed);
				effectTimer = 2f;
			}
		}

		public static void DestroyParticle(GameObject effect, float destroyTime)
		{
			if (effect != null)
			{
				ParticleSystem[] componentsInChildren = effect.transform.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ParticleSystem particleSystem = componentsInChildren[i];
					particleSystem.Stop();
				}
				UnityEngine.Object.Destroy(effect, destroyTime);
			}
		}

		public static void StopParticle(GameObject effect)
		{
			if (effect != null)
			{
				ParticleSystem[] componentsInChildren = effect.transform.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ParticleSystem particleSystem = componentsInChildren[i];
					particleSystem.Stop();
				}
			}
		}

		public static void PlayParticle(GameObject effect)
		{
			if (effect != null)
			{
				ParticleSystem[] componentsInChildren = effect.transform.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ParticleSystem particleSystem = componentsInChildren[i];
					particleSystem.Play();
				}
			}
		}

		public static bool ChangeAfterSpinattack(CharacterState context, int messageId, MessageBase msg)
		{
			if (messageId != 16385)
			{
				if (messageId != 16386)
				{
					return false;
				}
				context.ChangeState(STATE_ID.AfterSpinAttack);
				return true;
			}
			else
			{
				if (StateUtil.IsThroughBreakable(context))
				{
					return true;
				}
				if (!context.TestStatus(Status.InvincibleByChao))
				{
					context.ChangeState(STATE_ID.AfterSpinAttack);
				}
				return true;
			}
		}

		public static void SendMessageToTerminateItem(ItemType item)
		{
			if (StageItemManager.Instance != null)
			{
				StageItemManager.Instance.OnTerminateItem(new MsgTerminateItem(item));
			}
		}

		public static MsgBossInfo GetBossInfo(GameObject bossObject)
		{
			MsgBossInfo msgBossInfo = new MsgBossInfo();
			if (bossObject == null)
			{
				GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgBossInfo", msgBossInfo, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				bossObject.SendMessage("OnMsgBossInfo", msgBossInfo);
			}
			return msgBossInfo;
		}

		public static void SetAirMovementToRotateGround(CharacterState context, bool value)
		{
			CharacterMovement movement = context.Movement;
			if (movement)
			{
				CharacterMoveAir currentState = movement.GetCurrentState<CharacterMoveAir>();
				if (currentState != null)
				{
					currentState.SetRotateToGround(value);
				}
			}
		}

		public static void SetTargetMovement(CharacterState context, Vector3 position, Quaternion rotation, float time)
		{
			CharacterMovement movement = context.Movement;
			if (movement)
			{
				CharacterMoveTarget currentState = movement.GetCurrentState<CharacterMoveTarget>();
				if (currentState != null)
				{
					currentState.SetTarget(context.Movement, position, rotation, time);
				}
			}
		}

		public static void ThroughBreakable(CharacterState context, bool value)
		{
			CharacterMovement movement = context.Movement;
			if (movement)
			{
				movement.ThroughBreakable = value;
			}
		}

		public static bool IsThroughBreakable(CharacterState context)
		{
			CharacterMovement movement = context.Movement;
			return movement && movement.ThroughBreakable;
		}

		public static void ActiveMagnetObject(CharacterState context, float time)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterMagnet");
			if (gameObject != null)
			{
				CharacterMagnet component = gameObject.GetComponent<CharacterMagnet>();
				if (component != null)
				{
					component.SetEnable();
					component.SetTime(time);
					component.SetDefaultRadiusAndOffset();
				}
			}
		}

		public static void DeactiveMagetObject(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterMagnet");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterMagnet component = gameObject.GetComponent<CharacterMagnet>();
				if (component != null)
				{
					component.SetDisable();
				}
			}
		}

		public static void ActiveBarrier(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterBarrier");
			if (gameObject != null)
			{
				CharacterBarrier component = gameObject.GetComponent<CharacterBarrier>();
				component.SetEnable();
			}
		}

		public static void DeactiveBarrier(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterBarrier");
			if (gameObject != null && gameObject.activeSelf)
			{
				CharacterBarrier component = gameObject.GetComponent<CharacterBarrier>();
				component.SetDisable();
			}
		}

		public static bool IsBarrierActive(CharacterState context)
		{
			return StateUtil.IsPartsActive(context, "CharacterBarrier");
		}

		public static CharacterBarrier GetBarrier(CharacterState context)
		{
			return StateUtil.GetPartsComponent<CharacterBarrier>(context, "CharacterBarrier");
		}

		public static void ActiveInvincible(CharacterState context, float time)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterInvincible");
			if (gameObject != null)
			{
				CharacterInvincible component = gameObject.GetComponent<CharacterInvincible>();
				if (component != null)
				{
					component.SetActive(time);
				}
			}
		}

		public static void DeactiveInvincible(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterInvincible");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterInvincible component = gameObject.GetComponent<CharacterInvincible>();
				if (component != null)
				{
					component.SetDisable();
				}
			}
		}

		public static bool IsInvincibleActive(CharacterState context)
		{
			return StateUtil.IsPartsActive(context, "CharacterInvincible") || context.TestStatus(Status.InvincibleByChao);
		}

		private static bool IsPartsActive(CharacterState context, string name)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, name);
			return gameObject != null && gameObject.activeInHierarchy;
		}

		public static void ActiveCombo(CharacterState context, float time)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterCombo");
			if (gameObject != null)
			{
				CharacterCombo component = gameObject.GetComponent<CharacterCombo>();
				if (component != null)
				{
					component.SetEnable();
					component.SetTime(time);
				}
			}
		}

		public static void DeactiveCombo(CharacterState context, bool immediate)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterCombo");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterCombo component = gameObject.GetComponent<CharacterCombo>();
				if (component != null)
				{
					if (immediate)
					{
						component.SetDisable();
					}
					else
					{
						component.RequestEnd();
					}
				}
			}
		}

		public static void ActiveTrampoline(CharacterState context, float time)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterTrampoline");
			if (gameObject != null)
			{
				CharacterTrampoline component = gameObject.GetComponent<CharacterTrampoline>();
				if (component != null)
				{
					component.SetEnable();
					component.SetTime(time);
				}
			}
		}

		public static void DeactiveTrampoline(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterTrampoline");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterTrampoline component = gameObject.GetComponent<CharacterTrampoline>();
				if (component != null)
				{
					component.SetDisable();
				}
			}
		}

		public static T GetPartsComponent<T>(CharacterState context, string componentName) where T : Component
		{
			T result = GameObjectUtil.FindChildGameObjectComponent<T>(context.gameObject, componentName);
			if (result.gameObject.activeInHierarchy)
			{
				return result;
			}
			return (T)((object)null);
		}

		public static T GetPartsComponentAlways<T>(CharacterState context, string componentName) where T : Component
		{
			return GameObjectUtil.FindChildGameObjectComponent<T>(context.gameObject, componentName);
		}

		public static Vector3 GetBodyCenterPosition(MonoBehaviour context)
		{
			CapsuleCollider component;
			if (context.transform.parent == null || context.gameObject.tag == "Player")
			{
				component = context.gameObject.GetComponent<CapsuleCollider>();
			}
			else
			{
				component = context.gameObject.transform.parent.GetComponent<CapsuleCollider>();
			}
			if (component != null)
			{
				return component.center;
			}
			return Vector3.zero;
		}

		public static void SetObjectLocalPositionToCenter(MonoBehaviour context, GameObject gameobj)
		{
			Vector3 bodyCenterPosition = StateUtil.GetBodyCenterPosition(context);
			if (gameobj != null)
			{
				gameobj.transform.localPosition = bodyCenterPosition;
			}
		}

		public static void SetShadowActive(CharacterState context, bool value)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "ShadowProjector");
			if (gameObject != null)
			{
				gameObject.SetActive(value);
			}
		}

		public static void SetNotDrawBarrierEffect(CharacterState context, bool value)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterBarrier");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterBarrier component = gameObject.GetComponent<CharacterBarrier>();
				component.SetNotDraw(value);
			}
		}

		public static void SetNotDrawItemEffect(CharacterState context, bool value)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterInvincible");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterInvincible component = gameObject.GetComponent<CharacterInvincible>();
				component.SetNotDraw(value);
			}
			StateUtil.SetNotDrawBarrierEffect(context, value);
		}

		public static void ActiveChaoAbilityMagnetObject(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterMagnetChaoAbility");
			if (gameObject != null)
			{
				CharacterMagnet component = gameObject.GetComponent<CharacterMagnet>();
				if (component != null)
				{
					component.SetEnable();
					component.SetDefaultRadiusAndOffset();
				}
			}
		}

		public static void DeactiveChaoAbilityMagetObject(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "CharacterMagnetChaoAbility");
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				CharacterMagnet component = gameObject.GetComponent<CharacterMagnet>();
				if (component != null)
				{
					component.SetDisable();
				}
			}
		}

		public static void SetChangePhantomCancel(CharacterState context, ItemType itemType)
		{
			context.SetChangePhantomCancel(itemType);
		}

		public static PathEvaluator GetStagePathEvaluator(CharacterState context, BlockPathController.PathType patytype)
		{
			StageBlockPathManager stagePathManager = context.GetStagePathManager();
			if (stagePathManager != null)
			{
				PathEvaluator curentPathEvaluator = stagePathManager.GetCurentPathEvaluator(patytype);
				if (curentPathEvaluator != null)
				{
					return curentPathEvaluator;
				}
			}
			return null;
		}

		public static float GetForwardSpeedAir(CharacterState context, float targetSpeed, float deltaTime)
		{
			float num = context.Movement.GetForwardVelocityScalar();
			if (num < targetSpeed)
			{
				num = Mathf.Max(num + context.Parameter.m_airForwardAccel * deltaTime, targetSpeed);
			}
			else
			{
				num = Mathf.Min(num - context.Parameter.m_airForwardAccel * deltaTime, targetSpeed);
			}
			return num;
		}

		public static void ResetVelocity(CharacterState context)
		{
			context.Movement.Velocity = Vector3.zero;
		}

		public static void SetVelocityForwardRun(CharacterState context, bool setHorz)
		{
			Vector3 vector = context.Movement.GetForwardDir() * context.DefaultSpeed;
			if (setHorz)
			{
				context.Movement.HorzVelocity = vector;
			}
			else
			{
				context.Movement.Velocity = vector;
			}
		}

		public static void SetRotation(CharacterState context, Vector3 up)
		{
			context.transform.rotation = Quaternion.FromToRotation(context.transform.up, up) * context.transform.rotation;
		}

		public static void SetRotation(CharacterState context, Vector3 up, Vector3 front)
		{
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(front, up);
			context.transform.rotation = identity;
		}

		public static void SetRotateOnGround(CharacterState context)
		{
			CharacterMovement component = context.GetComponent<CharacterMovement>();
			HitInfo hitInfo;
			if (component.GetGroundInfo(out hitInfo))
			{
				MovementUtil.RotateByCollision(context.transform, context.GetComponent<CapsuleCollider>(), hitInfo.info.normal);
			}
		}

		public static void SetRotationOnGravityUp(CharacterState context)
		{
			MovementUtil.RotateByCollision(context.transform, context.GetComponent<CapsuleCollider>(), -context.Movement.GetGravityDir());
		}

		public static void GetBaseGroundPosition(CharacterState context, ref Vector3 pos)
		{
			Vector3 worldPosition = new Vector3(pos.x, 0f, pos.z);
			StageBlockPathManager stagePathManager = context.GetStagePathManager();
			if (stagePathManager != null)
			{
				PathEvaluator curentPathEvaluator = stagePathManager.GetCurentPathEvaluator(BlockPathController.PathType.SV);
				if (curentPathEvaluator != null)
				{
					worldPosition = curentPathEvaluator.GetWorldPosition();
				}
			}
			pos = worldPosition;
		}

		public static bool CheckOverlap(CapsuleCollider collider, Vector3 pos, Vector3 upDir, int layerMask)
		{
			float d = collider.height * 0.5f - collider.radius;
			float num = 0.01f;
			float radius = collider.radius - num;
			Vector3 a = pos + upDir * collider.height * 0.5f;
			Vector3 start = a - upDir * d;
			Vector3 end = a + upDir * d;
			return Physics.CheckCapsule(start, end, radius, layerMask);
		}

		public static bool CheckOverlapSphere(Vector3 pos, Vector3 upDir, float radius, int layerMask)
		{
			Vector3 position = pos + upDir * radius;
			return Physics.CheckSphere(position, radius, layerMask);
		}

		public static bool CapsuleCast(CapsuleCollider collider, Vector3 pos, Vector3 upDir, int layerMask, Vector3 direction, float distance, ref Vector3 outPos, bool noskin)
		{
			float d = collider.height * 0.5f - collider.radius;
			float num = (!noskin) ? 0.01f : 0f;
			float radius = collider.radius - num;
			Vector3 a = pos + upDir * collider.height * 0.5f;
			Vector3 point = a - upDir * d;
			Vector3 point2 = a + upDir * d;
			RaycastHit raycastHit;
			if (Physics.CapsuleCast(point, point2, radius, direction, out raycastHit, distance, layerMask))
			{
				num = 0.01f;
				Vector3 vector = pos + direction * (raycastHit.distance - num);
				outPos = vector;
				return true;
			}
			outPos = pos + direction * distance;
			return false;
		}

		public static void EnableChildObject(CharacterState context, string name, bool value)
		{
			if (name == null)
			{
				return;
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, name);
			if (gameObject != null)
			{
				gameObject.SetActive(value);
			}
		}

		public static void EnablePlayerCollision(CharacterState context, bool value)
		{
			CapsuleCollider component = context.GetComponent<CapsuleCollider>();
			if (component == null)
			{
				return;
			}
			component.enabled = value;
		}

		public static bool CheckDeadByHitWall(CharacterState context, float deltaTime)
		{
			HitInfo hitInfo;
			if (context.Movement.GetHitInfo(CharacterMovement.HitType.Front, out hitInfo))
			{
				float num = Vector3.Dot(context.Movement.GetDisplacement(), context.Movement.GetForwardDir());
				float vertVelocityScalar = context.Movement.GetVertVelocityScalar();
				float num2 = (!context.Movement.IsOnGround()) ? context.Parameter.m_hitWallUpSpeedAir : context.Parameter.m_hitWallUpSpeedGround;
				float num3 = Vector3.Dot(context.Movement.GetForwardDir(), hitInfo.info.normal);
				if (num3 < -0.94f && num < 0.5f * deltaTime && vertVelocityScalar < num2)
				{
					return true;
				}
			}
			return false;
		}

		public static bool CheckHitWallAndGoDeadOrStumble(CharacterState context, float deltaTime, ref STATE_ID state)
		{
			if (!StateUtil.CheckDeadByHitWall(context, deltaTime))
			{
				return false;
			}
			Vector3 origin = context.Position + -context.Movement.GetGravityDir() * context.Parameter.m_goStumbleMaxHeight;
			Vector3 baseFrontTangent = CharacterDefs.BaseFrontTangent;
			int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			Ray ray = new Ray(origin, baseFrontTangent);
			RaycastHit raycastHit;
			if (!Physics.Raycast(ray, out raycastHit, 1.5f, layerMask))
			{
				state = STATE_ID.Stumble;
				return true;
			}
			StateUtil.AddHitWallTimer(context, deltaTime);
			if (context.m_hitWallTimer > context.Parameter.m_hitWallDeadTime)
			{
				state = STATE_ID.Dead;
				return true;
			}
			return false;
		}

		private static bool IsPrecedeInputTouch(CharacterState context, float precedeSec)
		{
			return context.m_input.IsTouchedLastSecond(precedeSec);
		}

		public static bool ChangeToJumpStateIfPrecedeInputTouch(CharacterState context, float precedeSec, bool rotateOnGround)
		{
			if (StateUtil.IsPrecedeInputTouch(context, precedeSec))
			{
				context.ClearAirAction();
				StateUtil.NowLanding(context, rotateOnGround);
				context.ChangeState(STATE_ID.Jump);
				return true;
			}
			return false;
		}

		public static void ChangeStateToChangePhantom(CharacterState context, PhantomType phantom, float time)
		{
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP);
			context.NowPhantomType = phantom;
			ChangePhantomParameter changePhantomParameter = context.CreateEnteringParameter<ChangePhantomParameter>();
			changePhantomParameter.Set(phantom, time);
			context.ChangeState(STATE_ID.ChangePhantom);
		}

		public static void ReturnFromPhantomAndChangeState(CharacterState context, PhantomType nowPhantom, bool hitBoss)
		{
			context.NowPhantomType = PhantomType.NONE;
			string effectname = null;
			switch (nowPhantom)
			{
			case PhantomType.LASER:
				effectname = "ef_pl_change_laser01";
				break;
			case PhantomType.DRILL:
				effectname = "ef_pl_change_drill01";
				break;
			case PhantomType.ASTEROID:
				effectname = "ef_pl_change_asteroid01";
				break;
			}
			GameObject gameobj = StateUtil.CreateEffect(context, effectname, true);
			StateUtil.SetObjectLocalPositionToCenter(context, gameobj);
			context.SetNotCharaChange(false);
			if (!hitBoss)
			{
				context.SetNotUseItem(false);
			}
			if (hitBoss)
			{
				context.ChangeState(STATE_ID.AfterSpinAttack);
			}
			else
			{
				context.ChangeState(STATE_ID.Fall);
			}
		}

		public static void SendMessageTransformPhantom(CharacterState context, PhantomType phantom)
		{
			MsgTransformPhantom msgTransformPhantom = new MsgTransformPhantom(phantom);
			if (msgTransformPhantom != null)
			{
				GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnTransformPhantom", msgTransformPhantom, SendMessageOptions.DontRequireReceiver);
				if (StageItemManager.Instance != null)
				{
					StageItemManager.Instance.OnTransformPhantom(msgTransformPhantom);
				}
				GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnTransformPhantom", msgTransformPhantom);
				GameObjectUtil.SendDelayedMessageToTagObjects("Boss", "OnTransformPhantom", msgTransformPhantom);
				ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE_TIMER, -1f);
			}
		}

		public static void SendMessageReturnFromPhantom(CharacterState context, PhantomType phantom)
		{
			if (context.IsOnDestroy())
			{
				return;
			}
			context.NowPhantomType = PhantomType.NONE;
			MsgReturnFromPhantom msgReturnFromPhantom = new MsgReturnFromPhantom();
			if (msgReturnFromPhantom != null)
			{
				GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnReturnFromPhantom", msgReturnFromPhantom, SendMessageOptions.DontRequireReceiver);
				if (StageItemManager.Instance != null)
				{
					StageItemManager.Instance.OnReturnFromPhantom(msgReturnFromPhantom);
				}
				GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnReturnFromPhantom", msgReturnFromPhantom);
				GameObjectUtil.SendDelayedMessageToTagObjects("Boss", "OnReturnFromPhantom", msgReturnFromPhantom);
				if (context.GetLevelInformation().NowBoss)
				{
					ObjUtil.SendMessageOnBossObjectDead();
				}
				else
				{
					ObjUtil.SendMessageOnObjectDead();
				}
				ObjUtil.SendMessageAppearTrampoline();
				ObjUtil.PauseCombo(MsgPauseComboTimer.State.PLAY_SET, 3f);
			}
			ItemType item = ItemType.UNKNOWN;
			switch (phantom)
			{
			case PhantomType.LASER:
				item = ItemType.LASER;
				break;
			case PhantomType.DRILL:
				item = ItemType.DRILL;
				break;
			case PhantomType.ASTEROID:
				item = ItemType.ASTEROID;
				break;
			}
			StateUtil.SendMessageToTerminateItem(item);
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.COME_IN);
		}

		public static bool CheckCharaChangeOnDieAndSendMessage(CharacterState context)
		{
			StateUtil.SendMessageToTerminateItem(ItemType.LASER);
			StateUtil.SendMessageToTerminateItem(ItemType.DRILL);
			StateUtil.SendMessageToTerminateItem(ItemType.ASTEROID);
			StageAbilityManager instance = StageAbilityManager.Instance;
			CharacterContainer characterContainer = context.GetCharacterContainer();
			if (characterContainer != null)
			{
				if (characterContainer.IsEnableRecovery() && (float)ObjUtil.GetRandomRange100() < instance.GetChaoAbilityValue(ChaoAbility.RECOVERY_FROM_FAILURE))
				{
					characterContainer.PrepareRecovery(context.GetLevelInformation().NowBoss);
					return false;
				}
				if (characterContainer.IsEnableChangeCharacter())
				{
					MsgChangeChara msgChangeChara = new MsgChangeChara();
					msgChangeChara.m_changeByMiss = true;
					characterContainer.SendMessage("OnMsgChangeChara", msgChangeChara);
					if (msgChangeChara.m_succeed)
					{
						return false;
					}
				}
			}
			if (context.GetLevelInformation().NowTutorial)
			{
				return false;
			}
			if (instance.HasChaoAbility(ChaoAbility.LAST_CHANCE) && !context.GetLevelInformation().NowBoss && !context.GetLevelInformation().NowFeverBoss)
			{
				context.ChangeState(STATE_ID.LastChance);
				return false;
			}
			MsgNotifyDead value = new MsgNotifyDead();
			GameObject gameObject = GameObject.Find("GameModeStage");
			if (gameObject)
			{
				gameObject.SendMessage("OnMsgNotifyDead", value, SendMessageOptions.DontRequireReceiver);
			}
			GameObjectUtil.SendDelayedMessageToTagObjects("Boss", "OnMsgNotifyDead", value);
			return true;
		}

		public static bool CheckAndChangeStateToAirAttack(CharacterState context, bool checkAfterActionJump, bool resetUpDirection)
		{
			STATE_ID sTATE_ID = STATE_ID.Non;
			if (StateUtil.GetNextStateToAirAttack(context, ref sTATE_ID, checkAfterActionJump))
			{
				if (sTATE_ID == STATE_ID.Jump)
				{
					JumpParameter jumpParameter = context.CreateEnteringParameter<JumpParameter>();
					jumpParameter.Set(true);
					if (context.IsThirdJump())
					{
						sTATE_ID = STATE_ID.ThirdJump;
					}
				}
				if (resetUpDirection)
				{
					MovementUtil.RotateByCollision(context.transform, context.GetComponent<CapsuleCollider>(), -context.Movement.GetGravityDir());
				}
				context.ChangeState(sTATE_ID);
				return true;
			}
			return false;
		}

		public static bool GetNextStateToAirAttack(CharacterState context, ref STATE_ID id, bool checkAfterActionJump)
		{
			if (context.NumAirAction == 0)
			{
				id = STATE_ID.Jump;
				return true;
			}
			if (context.NumAirAction < context.NumEnableJump)
			{
				id = STATE_ID.DoubleJump;
				return true;
			}
			if (context.NumAirAction == context.NumEnableJump)
			{
				id = STATE_ID.AirAttackAction;
				return true;
			}
			if (checkAfterActionJump && context.NumAirAction == context.NumEnableJump + 1 && context.GetCharacterAttribute() == CharacterAttribute.POWER)
			{
				id = STATE_ID.Jump;
				return true;
			}
			return false;
		}

		public static void NowLanding(CharacterState context, bool rotateOnGround)
		{
			if (rotateOnGround)
			{
				StateUtil.SetRotateOnGround(context);
			}
			StateUtil.CreateLandingEffect(context);
			context.SetStatus(Status.NowLanding, true);
		}

		public static void SetOnBoost(CharacterState context, CharacterLoopEffect characterboost, bool value)
		{
			if (value)
			{
				context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ChaoAbility.SPIN_DASH_MAGNET);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						StateUtil.ActiveChaoAbilityMagnetObject(context);
						instance.RequestPlayChaoEffect(ChaoAbility.SPIN_DASH_MAGNET);
					}
				}
			}
			else
			{
				context.OffAttack();
				StateUtil.DeactiveChaoAbilityMagetObject(context);
			}
			if (characterboost != null)
			{
				characterboost.SetValid(value);
			}
		}

		public static void SetOnBoostEx(CharacterState context, CharacterLoopEffect characterBoostEx, bool value)
		{
			if (characterBoostEx != null)
			{
				characterBoostEx.SetValid(value);
			}
		}

		public static void Dead(CharacterState context)
		{
			context.SetStatus(Status.Dead, true);
		}

		public static void AddHitWallTimer(CharacterState context, float deltaTime)
		{
			context.m_hitWallTimer += deltaTime;
			context.SetStatus(Status.HitWallTimerDirty, true);
		}

		public static void SetEmergency(CharacterState context, bool value)
		{
			bool flag = context.TestStatus(Status.Emergency);
			if (value && !flag)
			{
				if (context.GetLevelInformation().BossStage && context.GetLevelInformation().BossDestroy)
				{
					return;
				}
				HudCaution.Instance.SetCaution(new MsgCaution(HudCaution.Type.NO_RING));
			}
			else if (!value && flag)
			{
				HudCaution.Instance.SetInvisibleCaution(new MsgCaution(HudCaution.Type.NO_RING));
			}
			context.SetStatus(Status.Emergency, value);
		}

		public static void SetAttackAttributePowerIfPowerType(CharacterState context, bool value)
		{
			if (context.GetCharacterAttribute() == CharacterAttribute.POWER)
			{
				if (value)
				{
					context.OnAttackAttribute(AttackAttribute.Power);
				}
				else
				{
					context.OnAttackAttribute(AttackAttribute.Power);
				}
			}
		}

		public static void SetDashRingMagnet(CharacterState context, bool onFlag)
		{
			if (onFlag)
			{
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ChaoAbility.DASH_RING_MAGNET);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						StateUtil.ActiveChaoAbilityMagnetObject(context);
						instance.RequestPlayChaoEffect(ChaoAbility.DASH_RING_MAGNET);
					}
				}
			}
			else
			{
				StateUtil.DeactiveChaoAbilityMagetObject(context);
			}
		}

		public static void SetCannonMagnet(CharacterState context, bool onFlag)
		{
			if (onFlag)
			{
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ChaoAbility.CANNON_MAGNET);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						StateUtil.ActiveChaoAbilityMagnetObject(context);
						instance.RequestPlayChaoEffect(ChaoAbility.CANNON_MAGNET);
					}
				}
			}
			else
			{
				StateUtil.DeactiveChaoAbilityMagetObject(context);
			}
		}

		public static void SetJumpRampMagnet(CharacterState context, bool onFlag)
		{
			if (onFlag)
			{
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ChaoAbility.JUMP_RAMP_MAGNET);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						StateUtil.ActiveChaoAbilityMagnetObject(context);
						instance.RequestPlayChaoEffect(ChaoAbility.JUMP_RAMP_MAGNET);
					}
				}
			}
			else
			{
				StateUtil.DeactiveChaoAbilityMagetObject(context);
			}
		}

		public static void SetSpecialtyJumpMagnet(CharacterState context, CharacterAttribute attri, ChaoAbility ability, bool onFlag)
		{
			if (attri != context.GetCharacterAttribute())
			{
				return;
			}
			if (context.IsNowPhantom())
			{
				return;
			}
			if (onFlag)
			{
				if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ability))
				{
					float chaoAbilityValue = StageAbilityManager.Instance.GetChaoAbilityValue(ability);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						StateUtil.ActiveChaoAbilityMagnetObject(context);
						ObjUtil.RequestStartAbilityToChao(ability, false);
					}
				}
			}
			else
			{
				StateUtil.DeactiveChaoAbilityMagetObject(context);
			}
		}

		public static void SetSpecialtyJumpDestroyEnemy(ChaoAbility ability)
		{
			if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ability))
			{
				float chaoAbilityValue = StageAbilityManager.Instance.GetChaoAbilityValue(ability);
				float num = UnityEngine.Random.Range(0f, 99.9f);
				if (chaoAbilityValue >= num)
				{
					ObjUtil.RequestStartAbilityToChao(ability, false);
				}
			}
		}

		public static void SetPhantomMagnetColliderRange(CharacterState context, PhantomType phantom)
		{
			if (phantom < PhantomType.NUM_MAX)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, CharacterDefs.PhantomBodyName[(int)phantom]);
				if (gameObject != null)
				{
					CharacterMagnetPhantom characterMagnetPhantom = GameObjectUtil.FindChildGameObjectComponent<CharacterMagnetPhantom>(gameObject, "MagnetCollision");
					if (characterMagnetPhantom != null)
					{
						characterMagnetPhantom.SetDefaultRadiusAndOffset();
					}
				}
			}
		}

		public static void SetPhantomQuickTimerPause(bool pauseFlag)
		{
			if (StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode() && StageTimeManager.Instance != null)
			{
				StageTimeManager.Instance.PhantomPause(pauseFlag);
			}
		}
	}
}
