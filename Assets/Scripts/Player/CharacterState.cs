using App;
using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterState : MonoBehaviour
	{
		private PlayerInformation m_information;

		private CameraManager m_camera;

		private CharaType m_charaType = CharaType.UNKNOWN;

		private bool m_subPlayer;

		public CharacterInput m_input;

		private CharacterMovement m_movement;

		private FSMSystem<CharacterState> m_fsm;

		private Animator m_bodyAnimator;

		private string m_bodyName;

		private string m_charaName;

		private string m_suffixName;

		private Bitset32 m_status;

		private Bitset32 m_visibleStatus;

		private Bitset32 m_options;

		private StateEnteringParameter m_enteringParam;

		private CharacterBlinkTimer m_blinkTimer;

		private AttackPower m_attack;

		private DefensePower m_defense;

		private uint m_attackAttribute;

		private PlayerSpeed m_nowSpeedLevel;

		private PhantomType m_nowPhantomType = PhantomType.NONE;

		private CharacterAttribute m_attribute;

		private TeamAttribute m_teamAttribute;

		[SerializeField]
		private float m_defaultSpeed = 8f;

		[SerializeField]
		private bool m_notLoadCharaParameter = true;

		private PlayingCharacterType m_playingCharacterType;

		private StageBlockPathManager m_blockPathManager;

		private CharacterContainer m_characterContainer;

		private LevelInformation m_levelInformation;

		private StageScoreManager m_scoreManager;

		private bool m_nowOnDestroy;

		private int m_numAirAction;

		private int m_numEnableJump = 1;

		public float m_hitWallTimer;

		private ItemType m_changePhantomCancel = ItemType.UNKNOWN;

		private WispBoostLevel m_wispBoostLevel = WispBoostLevel.NONE;

		private string m_wispBoostEffect = string.Empty;

		public bool m_isEdit;

		public bool m_notDeadNoRing;

		public bool m_noCrushDead;

		public bool m_notDropRing;

		private bool m_isAlreadySetupModel;

		public PhantomType NowPhantomType
		{
			get
			{
				return this.m_nowPhantomType;
			}
			set
			{
				this.m_nowPhantomType = value;
			}
		}

		public CharacterMovement Movement
		{
			get
			{
				return this.m_movement;
			}
		}

		public string BodyModelName
		{
			get
			{
				return this.m_bodyName;
			}
			set
			{
				this.m_bodyName = value;
			}
		}

		public string CharacterName
		{
			get
			{
				return this.m_charaName;
			}
			set
			{
				this.m_charaName = value;
			}
		}

		public string SuffixName
		{
			get
			{
				return this.m_suffixName;
			}
			set
			{
				this.m_suffixName = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				return base.transform.position;
			}
		}

		public CharacterParameterData Parameter
		{
			get
			{
				return base.GetComponent<CharacterParameter>().GetData();
			}
		}

		public float DefaultSpeed
		{
			get
			{
				return this.m_defaultSpeed;
			}
		}

		public int NumAirAction
		{
			get
			{
				return this.m_numAirAction;
			}
		}

		public int NumEnableJump
		{
			get
			{
				return this.m_numEnableJump;
			}
		}

		public CharaType charaType
		{
			get
			{
				return this.m_charaType;
			}
		}

		public WispBoostLevel BossBoostLevel
		{
			get
			{
				return this.m_wispBoostLevel;
			}
		}

		public string BossBoostEffect
		{
			get
			{
				return this.m_wispBoostEffect;
			}
		}

		public void SetPlayingType(PlayingCharacterType type)
		{
			this.m_playingCharacterType = type;
		}

		public void SetupModelsAndParameter()
		{
			if (this.m_isAlreadySetupModel)
			{
				return;
			}
			PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
			ResourceManager instance = ResourceManager.Instance;
			if (playerInformation != null && instance != null)
			{
				string text = (this.m_playingCharacterType != PlayingCharacterType.MAIN) ? playerInformation.SubCharacterName : playerInformation.MainCharacterName;
				if (!this.m_notLoadCharaParameter)
				{
					string name = text + "Parameter";
					GameObject gameObject = instance.GetGameObject(ResourceCategory.PLAYER_COMMON, name);
					if (gameObject != null)
					{
						CharacterParameter component = gameObject.GetComponent<CharacterParameter>();
						CharacterParameter component2 = base.GetComponent<CharacterParameter>();
						if (component2 != null && component != null)
						{
							component2.CopyData(component.GetData());
						}
					}
				}
				if (CharacterDataNameInfo.Instance)
				{
					CharacterDataNameInfo.Info dataByName = CharacterDataNameInfo.Instance.GetDataByName(text);
					if (dataByName != null)
					{
						this.m_charaType = dataByName.m_ID;
						this.m_attribute = dataByName.m_attribute;
						this.m_teamAttribute = dataByName.m_teamAttribute;
						this.m_options.Reset();
						this.SetOption(Option.BigSize, dataByName.BigSize);
						this.SetOption(Option.HighSpeedExEffect, dataByName.HighSpeedEffect);
						this.SetOption(Option.ThirdJump, dataByName.ThirdJump);
						this.SuffixName = dataByName.m_hud_suffix;
					}
				}
				if (base.name != null)
				{
					string text2 = "chr_" + text;
					text2 = text2.ToLower();
					GameObject gameObject2 = this.CreateChildModelObject(instance.GetGameObject(ResourceCategory.CHARA_MODEL, text2), true);
					if (gameObject2)
					{
						CharacterState.OffAnimatorRootAnimation(gameObject2);
					}
					this.BodyModelName = text2;
				}
				this.SetupAnimation();
				string[] phantomBodyName = CharacterDefs.PhantomBodyName;
				for (int i = 0; i < phantomBodyName.Length; i++)
				{
					string name2 = phantomBodyName[i];
					GameObject gameObject3 = this.CreateChildModelObject(instance.GetGameObject(ResourceCategory.PLAYER_COMMON, name2), false);
					if (gameObject3)
					{
						CharacterState.OffAnimatorRootAnimation(gameObject3);
					}
					Collider[] componentsInChildren = gameObject3.GetComponentsInChildren<Collider>(true);
					Collider[] array = componentsInChildren;
					for (int j = 0; j < array.Length; j++)
					{
						Collider collider = array[j];
						if (collider.gameObject.layer == LayerMask.NameToLayer("Magnet"))
						{
							collider.gameObject.AddComponent<CharacterMagnetPhantom>();
						}
						if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
						{
							collider.gameObject.AddComponent<CharacterPhantomCollision>();
						}
					}
				}
				GameObject gameObject4 = this.CreateChildObject(instance.GetGameObject(ResourceCategory.PLAYER_COMMON, "drill_truck"), false);
				if (gameObject4 != null)
				{
					this.SetupDrill(gameObject4);
				}
				this.CreateChildObject(instance.GetGameObject(ResourceCategory.COMMON_EFFECT, "ef_ph_laser_lp01"), false);
				if (text != null)
				{
					string effectName = "ef_pl_" + text.ToLower() + "_boost01";
					string spinDashSEName = CharaSEUtil.GetSpinDashSEName(this.m_charaType);
					this.CreateLoopEffectBehavior("CharacterBoost", effectName, spinDashSEName, ResourceCategory.CHARA_EFFECT);
					string effectName2 = "ef_pl_" + text.ToLower() + "_jump01";
					GameObject gameobj = this.CreateLoopEffectBehavior("CharacterSpinAttack", effectName2, null, ResourceCategory.CHARA_EFFECT);
					StateUtil.SetObjectLocalPositionToCenter(this, gameobj);
					if (this.IsExHighSpeedEffect())
					{
						string effectName3 = "ef_pl_" + text.ToLower() + "_infinityrun01";
						GameObject gameobj2 = this.CreateLoopEffectBehavior("CharacterBoostEx", effectName3, null, ResourceCategory.CHARA_EFFECT);
						StateUtil.SetObjectLocalPositionToCenter(this, gameobj2);
					}
					if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
					{
						for (int k = 0; k < 3; k++)
						{
							string str = (k + 1).ToString();
							string effectName4 = "ef_raid_speedup_lv" + str + "_atk01";
							GameObject gameObject5 = this.CreateLoopEffectBehavior("CharacterSpinAttackLv" + str, effectName4, null, ResourceCategory.EVENT_RESOURCE);
							if (gameObject5 != null)
							{
								StateUtil.SetObjectLocalPositionToCenter(this, gameObject5);
								if (k == 2)
								{
									CharacterMagnet partsComponentAlways = StateUtil.GetPartsComponentAlways<CharacterMagnet>(this, "CharacterMagnet");
									if (partsComponentAlways != null)
									{
										GameObject gameObject6 = UnityEngine.Object.Instantiate(partsComponentAlways.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
										if (gameObject6 != null)
										{
											gameObject6.name = "CharacterMagnetBossBoost";
											gameObject6.transform.parent = gameObject5.transform;
											gameObject6.transform.localPosition = partsComponentAlways.gameObject.transform.localPosition;
											gameObject6.transform.localRotation = partsComponentAlways.gameObject.transform.localRotation;
											gameObject6.transform.localScale = partsComponentAlways.gameObject.transform.localScale;
											SphereCollider component3 = gameObject6.GetComponent<SphereCollider>();
											if (component3 != null)
											{
												component3.radius = 1.5f;
											}
											gameObject6.SetActive(true);
										}
									}
								}
							}
						}
					}
					this.CharacterName = text.ToLower();
				}
				if (this.IsBigSize())
				{
					CharacterMagnet partsComponentAlways2 = StateUtil.GetPartsComponentAlways<CharacterMagnet>(this, "CharacterMagnet");
					if (partsComponentAlways2 != null)
					{
						partsComponentAlways2.IsBigSize = true;
					}
					CharacterMagnet partsComponentAlways3 = StateUtil.GetPartsComponentAlways<CharacterMagnet>(this, "CharacterMagnetBossBoost");
					if (partsComponentAlways3 != null)
					{
						partsComponentAlways3.IsBigSize = true;
					}
					CharacterInvincible partsComponentAlways4 = StateUtil.GetPartsComponentAlways<CharacterInvincible>(this, "CharacterInvincible");
					if (partsComponentAlways4 != null)
					{
						partsComponentAlways4.IsBigSize = true;
					}
					CharacterBarrier partsComponentAlways5 = StateUtil.GetPartsComponentAlways<CharacterBarrier>(this, "CharacterBarrier");
					if (partsComponentAlways5 != null)
					{
						partsComponentAlways5.IsBigSize = true;
					}
				}
			}
			this.m_isAlreadySetupModel = true;
		}

		private GameObject CreateChildModelObject(GameObject srcObject, bool active)
		{
			if (srcObject != null)
			{
				Vector3 localPosition = srcObject.transform.localPosition;
				Quaternion localRotation = srcObject.transform.localRotation;
				GameObject gameObject = UnityEngine.Object.Instantiate(srcObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.parent = base.transform;
					gameObject.SetActive(active);
					gameObject.transform.localPosition = localPosition;
					gameObject.transform.localRotation = localRotation;
					gameObject.name = srcObject.name;
					return gameObject;
				}
			}
			return null;
		}

		private static void OffAnimatorRootAnimation(GameObject modelObject)
		{
			if (modelObject == null)
			{
				return;
			}
			Animator component = modelObject.GetComponent<Animator>();
			if (component != null)
			{
				component.applyRootMotion = false;
			}
		}

		private GameObject CreateChildObject(GameObject srcObject, bool active)
		{
			if (srcObject != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(srcObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject)
				{
					gameObject.SetActive(active);
					gameObject.transform.parent = base.transform;
					gameObject.name = srcObject.name;
					return gameObject;
				}
			}
			return null;
		}

		private void SetupDrill(GameObject drillObject)
		{
			DrillTrack drillTrack = drillObject.AddComponent<DrillTrack>();
			if (drillTrack != null)
			{
				drillTrack.m_Target = base.gameObject;
				GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
				if (gameObject != null)
				{
					drillTrack.m_Camera = gameObject.transform;
				}
			}
		}

		private void SetupAnimation()
		{
			GameObject gameObject = base.transform.FindChild(this.m_bodyName).gameObject;
			if (gameObject)
			{
				this.m_bodyAnimator = gameObject.GetComponent<Animator>();
			}
		}

		private GameObject CreateLoopEffectBehavior(string objectName, string effectName, string sename, ResourceCategory category)
		{
			GameObject gameObject = new GameObject(objectName);
			if (gameObject != null)
			{
				CharacterLoopEffect characterLoopEffect = gameObject.AddComponent<CharacterLoopEffect>();
				if (characterLoopEffect != null)
				{
					characterLoopEffect.Setup(effectName, category);
					characterLoopEffect.SetSE(sename);
					gameObject.transform.position = base.transform.position;
					gameObject.transform.rotation = base.transform.rotation;
					gameObject.transform.parent = base.transform;
					gameObject.SetActive(false);
					return gameObject;
				}
				UnityEngine.Object.Destroy(gameObject);
			}
			return null;
		}

		public void Start()
		{
			this.SetupOnStart();
		}

		private void SetupOnStart()
		{
			if (this.m_information == null)
			{
				GameObject gameObject = GameObject.Find("PlayerInformation");
				this.m_information = gameObject.GetComponent<PlayerInformation>();
			}
			if (this.m_blockPathManager == null)
			{
				this.m_blockPathManager = GameObjectUtil.FindGameObjectComponent<StageBlockPathManager>("StageBlockManager");
			}
			if (this.m_characterContainer == null)
			{
				this.m_characterContainer = GameObjectUtil.FindGameObjectComponent<CharacterContainer>("CharacterContainer");
			}
			if (this.m_camera == null)
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
				this.m_camera = gameObject2.GetComponent<CameraManager>();
			}
			if (this.m_levelInformation == null)
			{
				this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
			}
			if (this.m_scoreManager == null)
			{
				this.m_scoreManager = StageScoreManager.Instance;
			}
			if (this.m_information != null)
			{
				this.m_nowSpeedLevel = this.m_information.SpeedLevel;
				this.m_information.SetPlayerAttribute(this.m_attribute, this.m_teamAttribute, this.m_playingCharacterType);
			}
			if (!this.m_isAlreadySetupModel)
			{
				this.SetupModelsAndParameter();
			}
			if (this.m_input == null)
			{
				this.m_input = base.GetComponent<CharacterInput>();
			}
			if (this.m_movement == null)
			{
				this.m_movement = base.GetComponent<CharacterMovement>();
			}
			this.SetSpeedLevel(this.m_nowSpeedLevel);
			StateUtil.NowLanding(this, false);
			this.MakeFSM();
			this.SetupChildObjectOnStart();
			this.Movement.SetupOnStart();
			this.m_input.CreateHistory();
		}

		private void SetupChildObjectOnStart()
		{
			foreach (Transform transform in base.gameObject.transform)
			{
				transform.gameObject.SetActive(false);
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "ShadowProjector");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, this.BodyModelName);
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
			}
			this.m_hitWallTimer = 0f;
		}

		private void OnDestroy()
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_nowOnDestroy = true;
				this.m_fsm.CurrentState.Leave(this);
				this.m_fsm = null;
			}
		}

		private void Update()
		{
			if (App.Math.NearZero(Time.deltaTime, 1E-06f))
			{
				return;
			}
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.Step(this, Time.deltaTime);
			}
			this.UpdateInfomations();
			this.CheckHitWallTimerDirty();
			this.CheckFallingDead();
			this.UpdateTransformInformations();
		}

		private void MakeFSM()
		{
			if (this.m_fsm == null)
			{
				this.m_fsm = new FSMSystem<CharacterState>();
				FSMStateFactory<CharacterState>[] commonFSMTable = CharacterState.GetCommonFSMTable();
				FSMStateFactory<CharacterState>[] array = commonFSMTable;
				for (int i = 0; i < array.Length; i++)
				{
					FSMStateFactory<CharacterState> stateFactory = array[i];
					this.m_fsm.AddState(stateFactory);
				}
				this.SetAttributeState();
				if (!this.m_isEdit)
				{
					this.m_fsm.Init(this, 2);
				}
				else
				{
					this.m_fsm.Init(this, 1);
				}
			}
		}

		private void SetAttributeState()
		{
			switch (this.m_attribute)
			{
			case CharacterAttribute.SPEED:
				this.m_fsm.AddState(4, new StateDoubleJump());
				this.m_numEnableJump = 2;
				break;
			case CharacterAttribute.FLY:
				this.m_fsm.AddState(4, new StateFly());
				this.m_numEnableJump = 1;
				break;
			case CharacterAttribute.POWER:
				this.m_fsm.AddState(4, new StateGride());
				this.m_numEnableJump = 1;
				break;
			}
		}

		public void ChangeState(STATE_ID state)
		{
			this.m_fsm.ChangeState(this, (int)state);
			this.m_enteringParam = null;
			this.SetStatus(Status.NowLanding, false);
		}

		public void ChangeMovement(MOVESTATE_ID state)
		{
			if (this.m_movement)
			{
				this.m_movement.ChangeState(state);
			}
		}

		public T CreateEnteringParameter<T>() where T : StateEnteringParameter, new()
		{
			this.m_enteringParam = Activator.CreateInstance<T>();
			return (T)((object)this.m_enteringParam);
		}

		public T GetEnteringParameter<T>() where T : StateEnteringParameter
		{
			if (this.m_enteringParam == null)
			{
				return (T)((object)null);
			}
			if (this.m_enteringParam is T)
			{
				return (T)((object)this.m_enteringParam);
			}
			return (T)((object)null);
		}

		public void SetVisibleBlink(bool value)
		{
			this.m_visibleStatus.Set(0, value);
			this.SetRenderEnable(!this.m_visibleStatus.Any());
		}

		public void SetModelNotDraw(bool value)
		{
			this.m_visibleStatus.Set(1, value);
			this.SetRenderEnable(!this.m_visibleStatus.Any());
			if (this.m_bodyName == "chr_omega")
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "chr_omega");
				if (gameObject != null)
				{
					GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Booster_R");
					if (gameObject2 != null)
					{
						gameObject2.SetActive(!value);
					}
					GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject, "Booster_L");
					if (gameObject3 != null)
					{
						gameObject3.SetActive(!value);
					}
				}
			}
		}

		private void SetRenderEnable(bool value)
		{
			Component[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			Component[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)array[i];
				skinnedMeshRenderer.enabled = value;
			}
		}

		public void StartDamageBlink()
		{
			if (this.m_blinkTimer == null)
			{
				this.m_blinkTimer = base.gameObject.AddComponent<CharacterBlinkTimer>();
			}
			this.m_blinkTimer.Setup(this, this.Parameter.m_damageInvincibleTime);
		}

		private void UpdateTransformInformations()
		{
			if (this.m_information == null)
			{
				return;
			}
			this.m_information.SetTransform(base.transform);
			this.m_information.SetVelocity(this.m_movement.Velocity);
			this.m_information.SetFrontSpeed(this.m_movement.GetForwardVelocityScalar());
			this.m_information.SetHorzAndVertVelocity(this.m_movement.HorzVelocity, this.m_movement.VertVelocity);
		}

		private void UpdateInfomations()
		{
			if (this.m_information == null)
			{
				return;
			}
			this.UpdateTransformInformations();
			Vector3 displacement = this.m_movement.GetDisplacement();
			if (this.m_information.IsMovementUpdated())
			{
				float nowDistance;
				if (this.m_movement.IsOnGround())
				{
					nowDistance = Mathf.Max(0f, Vector3.Dot(displacement, this.m_movement.GetForwardDir()));
				}
				else
				{
					nowDistance = Mathf.Max(0f, Vector3.Dot(displacement, CharacterDefs.BaseFrontTangent));
				}
				this.m_information.AddTotalDistance(nowDistance);
			}
			this.m_information.SetDistanceToGround(this.m_movement.DistanceToGround);
			this.m_information.SetGravityDirection(this.m_movement.GetGravityDir());
			this.m_information.SetUpDirection(this.m_movement.GroundUpDirection);
			this.m_information.SetDefautlSpeed(this.DefaultSpeed);
			this.m_information.SetDead(this.IsDead());
			this.m_information.SetDamaged(this.IsDamaged());
			this.m_information.SetOnGround(this.m_movement.IsOnGround());
			this.m_information.SetEnableCharaChange(this.IsEnableCharaChange(false));
			this.m_information.SetParaloop(this.IsParaloop());
			this.m_information.SetPhantomType(this.m_nowPhantomType);
			this.m_information.SetLastChance(this.IsNowLastChance());
			StageBlockPathManager blockPathManager = this.m_blockPathManager;
			if (blockPathManager != null)
			{
				Vector3 position = this.Position;
				position.y = 0f;
				Vector3? vector = new Vector3?(position);
				Vector3? vector2 = new Vector3?(base.transform.up);
				Vector3? vector3 = null;
				PathEvaluator curentPathEvaluator = blockPathManager.GetCurentPathEvaluator(BlockPathController.PathType.SV);
				if (curentPathEvaluator != null)
				{
					float distance = curentPathEvaluator.Distance;
					curentPathEvaluator.GetClosestPositionAlongSpline(vector.Value, distance - 10f, distance + 10f, out distance);
					curentPathEvaluator.GetPNT(distance, ref vector, ref vector2, ref vector3);
					this.m_information.SetSideViewPath(vector.Value, vector2.Value);
				}
			}
		}

		private void CheckHitWallTimerDirty()
		{
			if (!this.TestStatus(Status.HitWallTimerDirty))
			{
				this.m_hitWallTimer = 0f;
			}
			else
			{
				this.SetStatus(Status.HitWallTimerDirty, false);
			}
		}

		private void CheckFallingDead()
		{
			if (this.IsDead() || this.IsNowLastChance() || this.IsHold())
			{
				return;
			}
			if (base.transform.position.y < -100f)
			{
				this.ChangeState(STATE_ID.FallingDead);
			}
		}

		public void SetStatus(Status st, bool value)
		{
			this.m_status.Set((int)st, value);
		}

		public bool TestStatus(Status st)
		{
			return this.m_status.Test((int)st);
		}

		public void SetOption(Option op, bool value)
		{
			this.m_options.Set((int)op, value);
		}

		public bool TestOption(Option op)
		{
			return this.m_options.Test((int)op);
		}

		public bool IsDead()
		{
			return this.TestStatus(Status.Dead);
		}

		public bool IsDamaged()
		{
			return this.TestStatus(Status.Damaged);
		}

		public bool IsParaloop()
		{
			return this.TestStatus(Status.Paraloop);
		}

		public bool IsHold()
		{
			return this.TestStatus(Status.Hold);
		}

		public bool IsBigSize()
		{
			return this.TestOption(Option.BigSize);
		}

		public bool IsExHighSpeedEffect()
		{
			return this.TestOption(Option.HighSpeedExEffect);
		}

		public bool IsThirdJump()
		{
			return this.TestOption(Option.ThirdJump);
		}

		public bool IsNowLastChance()
		{
			return this.TestStatus(Status.LastChance);
		}

		public bool IsOnDestroy()
		{
			return this.m_nowOnDestroy;
		}

		public bool IsNowPhantom()
		{
			return this.m_nowPhantomType != PhantomType.NONE;
		}

		public CharacterAttribute GetCharacterAttribute()
		{
			return this.m_attribute;
		}

		public void SetSpeedLevel(PlayerSpeed speed)
		{
			switch (speed)
			{
			case PlayerSpeed.LEVEL_1:
				this.m_defaultSpeed = this.Parameter.m_level1Speed;
				break;
			case PlayerSpeed.LEVEL_2:
				this.m_defaultSpeed = this.Parameter.m_level2Speed;
				break;
			case PlayerSpeed.LEVEL_3:
				this.m_defaultSpeed = this.Parameter.m_level3Speed;
				break;
			}
		}

		public T GetMovementState<T>() where T : FSMState<CharacterMovement>
		{
			return this.Movement.GetCurrentState<T>();
		}

		public Animator GetAnimator()
		{
			return this.m_bodyAnimator;
		}

		public void OnAttack(AttackPower attack, DefensePower defense)
		{
			this.m_attack = attack;
			this.m_defense = defense;
		}

		public void OffAttack()
		{
			this.m_attack = AttackPower.PlayerNormal;
			this.m_defense = DefensePower.PlayerNormal;
			this.m_attackAttribute = 0u;
		}

		public void OnAttackAttribute(AttackAttribute attribute)
		{
			this.m_attackAttribute |= (uint)attribute;
		}

		public void SetLastChance(bool value)
		{
			this.SetStatus(Status.LastChance, value);
		}

		public void SetNotCharaChange(bool value)
		{
			this.SetStatus(Status.NotCharaChange, value);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeCharaButton", new MsgChangeCharaButton(!value, false), SendMessageOptions.DontRequireReceiver);
		}

		public void SetNotUseItem(bool value)
		{
			MsgItemButtonEnable value2 = new MsgItemButtonEnable(!value);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnItemEnable", value2, SendMessageOptions.DontRequireReceiver);
		}

		public bool IsEnableCharaChange(bool changeByMiss)
		{
			return !this.TestStatus(Status.NotCharaChange) && (!this.IsDead() || changeByMiss);
		}

		public StageBlockPathManager GetStagePathManager()
		{
			return this.m_blockPathManager;
		}

		public CharacterContainer GetCharacterContainer()
		{
			return this.m_characterContainer;
		}

		public CameraManager GetCamera()
		{
			return this.m_camera;
		}

		public LevelInformation GetLevelInformation()
		{
			return this.m_levelInformation;
		}

		public PlayerInformation GetPlayerInformation()
		{
			return this.m_information;
		}

		public StageScoreManager GetStageScoreManager()
		{
			return this.m_scoreManager;
		}

		public void ActiveCharacter(bool jump, bool damageBlink, Vector3 position, Quaternion rotation)
		{
			this.SetupOnStart();
			if (damageBlink)
			{
				this.StartDamageBlink();
			}
			this.WarpAndCheckOverlap(position, rotation);
			if (jump)
			{
				this.ClearAirAction();
				this.GetAnimator().CrossFade("SpringJump", 0.01f);
				JumpSpringParameter jumpSpringParameter = this.CreateEnteringParameter<JumpSpringParameter>();
				jumpSpringParameter.Set(base.transform.position, base.transform.rotation, 7f, 0.3f);
				this.ChangeState(STATE_ID.SpringJump);
			}
			else
			{
				this.m_numAirAction = 99;
				this.GetAnimator().Play("Fall");
				this.Movement.Velocity = new Vector3(0f, 2f, 0f);
				this.ChangeState(STATE_ID.Fall);
			}
			if (this.m_information.NumRings == 0)
			{
				StateUtil.SetEmergency(this, true);
			}
		}

		private void WarpAndCheckOverlap(Vector3 pos, Quaternion rotation)
		{
			CapsuleCollider component = base.GetComponent<CapsuleCollider>();
			this.Movement.ResetPosition(pos);
			this.Movement.ResetRotation(rotation);
			float num = Mathf.Max(component.height, component.radius) + 0.2f;
			Vector3 position = pos + component.center.y * base.transform.up;
			int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			Collider[] array = Physics.OverlapSphere(position, num, layerMask);
			if (array.Length > 0)
			{
				Vector3 a = -this.Movement.GetGravityDir();
				float num2 = 5f;
				Vector3 a2 = pos + a * num2;
				num = component.radius;
				float d = component.height * 0.5f - num;
				Vector3 a3 = a2 + base.transform.TransformDirection(component.center);
				Vector3 point = a3 - a * d;
				Vector3 point2 = a3 + a * d;
				Vector3 vector = -a;
				RaycastHit raycastHit;
				if (Physics.CapsuleCast(point, point2, num, vector, out raycastHit, num2, layerMask))
				{
					Vector3 b = vector * (raycastHit.distance - 0.02f);
					pos = a2 + b;
					this.Movement.ResetPosition(pos);
					global::Debug.Log(string.Concat(new object[]
					{
						"WarpAndCheckOverlap CapsuleCast Hit:",
						pos.x,
						" ",
						pos.y,
						" ",
						pos.z
					}));
				}
			}
		}

		public void DeactiveCharacter()
		{
			StateUtil.DeactiveCombo(this, true);
			StateUtil.DeactiveInvincible(this);
			StateUtil.DeactiveBarrier(this);
			StateUtil.DeactiveMagetObject(this);
			StateUtil.DeactiveTrampoline(this);
			bool flag = this.TestStatus(Status.Emergency);
			if (flag)
			{
				StateUtil.SetEmergency(this, false);
			}
			this.m_status.Reset();
		}

		public void AddAirAction()
		{
			this.m_numAirAction++;
		}

		public void ClearAirAction()
		{
			this.m_numAirAction = 0;
		}

		public void SetAirAction(int num)
		{
			this.m_numAirAction = num;
		}

		public CharacterLoopEffect GetSpinAttackEffect()
		{
			string text = "CharacterSpinAttack";
			if (this.m_wispBoostLevel != WispBoostLevel.NONE)
			{
				text = text + "Lv" + ((int)(this.m_wispBoostLevel + 1)).ToString();
			}
			return GameObjectUtil.FindChildGameObjectComponent<CharacterLoopEffect>(base.gameObject, text);
		}

		public void SetBoostLevel(WispBoostLevel wispBoostLevel, string effect)
		{
			this.m_wispBoostLevel = wispBoostLevel;
			this.m_wispBoostEffect = effect;
		}

		public void SetChangePhantomCancel(ItemType itemType)
		{
			this.m_changePhantomCancel = itemType;
		}

		public ItemType GetChangePhantomCancel()
		{
			return this.m_changePhantomCancel;
		}

		public void OnAddRings(int numRing)
		{
			this.m_information.AddNumRings(numRing);
			ObjUtil.AddCombo();
			StateUtil.SetEmergency(this, false);
		}

		private void OnMsgTutorialGetRingNum(MsgTutorialGetRingNum msg)
		{
			msg.m_ring = this.m_information.NumRings;
		}

		private void OnMsgTutorialResetForRetry(MsgTutorialResetForRetry msg)
		{
			this.m_information.SetNumRings(msg.m_ring);
			if (msg.m_blink)
			{
				StateUtil.SetEmergency(this, msg.m_ring == 0);
			}
		}

		private void OnResetRingsForCheckPoint(MsgPlayerTransferRing msg)
		{
			if (msg.m_hud)
			{
				StateUtil.SetEmergency(this, true);
			}
		}

		private void OnResetRingsForContinue()
		{
			StateUtil.SetEmergency(this, false);
		}

		private void OnDebugAddDistance(int distance)
		{
		}

		private void OnDebugWarpPlayer(Vector3 pos)
		{
		}

		private bool IsStompHitObject(Collider other)
		{
			float vertVelocityScalar = this.Movement.GetVertVelocityScalar();
			if (vertVelocityScalar > -1f)
			{
				return false;
			}
			Vector3 b = other.transform.position;
			CapsuleCollider capsuleCollider = other as CapsuleCollider;
			if (capsuleCollider != null)
			{
				b = capsuleCollider.transform.TransformPoint(capsuleCollider.center);
			}
			else
			{
				SphereCollider sphereCollider = other as SphereCollider;
				if (sphereCollider != null)
				{
					b = sphereCollider.transform.TransformPoint(sphereCollider.center);
				}
				else
				{
					BoxCollider boxCollider = other as BoxCollider;
					if (boxCollider != null)
					{
						b = boxCollider.transform.TransformPoint(boxCollider.center);
					}
				}
			}
			Vector3 vector = base.transform.TransformPoint(StateUtil.GetBodyCenterPosition(this)) - b;
			return vector.sqrMagnitude > 1.401298E-45f && Vector3.Dot(vector.normalized, base.transform.up) > Mathf.Cos(0.0174532924f * this.Parameter.m_enableStompDec);
		}

		public void OnTriggerEnter(Collider other)
		{
			AttackPower attackPower = this.m_attack;
			uint num = this.m_attackAttribute;
			if (StateUtil.IsInvincibleActive(this))
			{
				attackPower = AttackPower.PlayerInvincible;
				num |= 16u;
			}
			else if (attackPower == AttackPower.PlayerStomp && this.IsStompHitObject(other))
			{
				attackPower = AttackPower.PlayerSpin;
				if (this.m_attribute == CharacterAttribute.POWER)
				{
					num |= 8u;
				}
			}
			MsgHitDamage msgHitDamage = new MsgHitDamage(base.gameObject, attackPower);
			msgHitDamage.m_attackAttribute = num;
			GameObjectUtil.SendDelayedMessageToGameObject(other.gameObject, "OnDamageHit", msgHitDamage);
		}

		public void OnDefrayRing()
		{
			if (this.m_information.NumRings == 0)
			{
				StateUtil.SetEmergency(this, true);
			}
		}

		public void OnDamageHit(MsgHitDamage msg)
		{
			if (!this.IsDamaged() && !this.IsDead() && !this.IsNowLastChance() && !this.IsHold())
			{
				this.m_notDropRing = false;
				if (StateUtil.IsInvincibleActive(this))
				{
					return;
				}
				int num = (int)this.m_defense;
				Collider component = msg.m_sender.GetComponent<Collider>();
				if (component != null && this.IsStompHitObject(component))
				{
					num = 2;
				}
				if (msg.m_attackPower > num)
				{
					CharacterBarrier barrier = StateUtil.GetBarrier(this);
					if (barrier != null)
					{
						barrier.Damaged();
						this.StartDamageBlink();
						return;
					}
					Vector3 bodyCenterPosition = StateUtil.GetBodyCenterPosition(this);
					Vector3 position = base.transform.TransformPoint(bodyCenterPosition);
					StateUtil.CreateEffect(this, position, base.transform.rotation, "ef_pl_damage_com01", true);
					if (this.m_information.NumRings == 0)
					{
						this.ChangeState(STATE_ID.Dead);
						return;
					}
					if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.GUARD_DROP_RING))
					{
						float chaoAbilityValue = StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.GUARD_DROP_RING);
						float num2 = UnityEngine.Random.Range(0f, 99.9f);
						if (chaoAbilityValue >= num2)
						{
							this.m_notDropRing = true;
							ObjUtil.RequestStartAbilityToChao(ChaoAbility.GUARD_DROP_RING, false);
						}
					}
					if (!this.m_notDropRing)
					{
						ObjUtil.CreateLostRing(base.transform.position, base.transform.rotation, this.m_information.NumRings);
						this.m_information.LostRings();
						StateUtil.SetEmergency(this, true);
					}
					ObjUtil.RequestStartAbilityToChao(ChaoAbility.DAMAGE_TRAMPOLINE, false);
					ObjUtil.RequestStartAbilityToChao(ChaoAbility.DAMAGE_DESTROY_ALL, false);
					this.m_levelInformation.Missed = true;
					if (!this.IsNowPhantom())
					{
						this.ChangeState(STATE_ID.Damage);
					}
					return;
				}
			}
		}

		private void OnDamageSucceed(MsgHitDamageSucceed msg)
		{
			if (this.m_fsm != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		public void OnAttackGuard(MsgAttackGuard msg)
		{
			if (this.m_fsm != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		public void OnFallingDead()
		{
			if (this.NowPhantomType == PhantomType.DRILL)
			{
				return;
			}
			if (!this.IsDead() && !this.IsNowLastChance() && !this.IsHold())
			{
				this.ChangeState(STATE_ID.FallingDead);
			}
		}

		private void OnSpringImpulse(MsgOnSpringImpulse msg)
		{
			if (!this.IsDead() && !this.IsNowPhantom() && !this.IsNowLastChance() && !this.IsHold())
			{
				JumpSpringParameter jumpSpringParameter = this.CreateEnteringParameter<JumpSpringParameter>();
				jumpSpringParameter.Set(msg.m_position, msg.m_rotation, msg.m_firstSpeed, msg.m_outOfControl);
				this.ChangeState(STATE_ID.SpringJump);
				msg.m_succeed = true;
			}
		}

		private void OnDashRingImpulse(MsgOnDashRingImpulse msg)
		{
			if (!this.IsDead() && !this.IsNowPhantom() && !this.IsNowLastChance() && !this.IsHold())
			{
				JumpSpringParameter jumpSpringParameter = this.CreateEnteringParameter<JumpSpringParameter>();
				jumpSpringParameter.Set(msg.m_position, msg.m_rotation, msg.m_firstSpeed, msg.m_outOfControl);
				this.ChangeState(STATE_ID.DashRing);
				msg.m_succeed = true;
			}
		}

		private void OnCannonImpulse(MsgOnCannonImpulse msg)
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		private void OnAbidePlayer(MsgOnAbidePlayer msg)
		{
			if (!this.IsDead() && !this.IsNowPhantom() && !this.IsNowLastChance())
			{
				CannonReachParameter cannonReachParameter = this.CreateEnteringParameter<CannonReachParameter>();
				cannonReachParameter.Set(msg.m_position, msg.m_rotation, msg.m_height, msg.m_abideObject);
				this.ChangeState(STATE_ID.ReachCannon);
				msg.m_succeed = true;
			}
		}

		private void OnJumpBoardHit(MsgOnJumpBoardHit msg)
		{
			if (!this.IsDead() && !this.IsNowPhantom() && !this.IsNowLastChance() && !this.IsHold())
			{
				CharacterCheckTrickJump characterCheckTrickJump = base.GetComponent<CharacterCheckTrickJump>();
				if (characterCheckTrickJump == null)
				{
					characterCheckTrickJump = base.gameObject.AddComponent<CharacterCheckTrickJump>();
				}
				if (characterCheckTrickJump != null)
				{
					characterCheckTrickJump.Reset();
				}
			}
		}

		private void OnJumpBoardJump(MsgOnJumpBoardJump msg)
		{
			if (!this.IsDead() && !this.IsNowPhantom() && !this.IsNowLastChance() && !this.IsHold())
			{
				bool flag = false;
				CharacterCheckTrickJump component = base.GetComponent<CharacterCheckTrickJump>();
				if (component != null)
				{
					flag = component.IsTouched;
					UnityEngine.Object.Destroy(component);
				}
				TrickJumpParameter trickJumpParameter = this.CreateEnteringParameter<TrickJumpParameter>();
				if (flag)
				{
					trickJumpParameter.Set(msg.m_position, msg.m_succeedRotation, msg.m_succeedFirstSpeed, msg.m_succeedOutOfcontrol, msg.m_succeedRotation, msg.m_succeedFirstSpeed, msg.m_succeedOutOfcontrol, flag);
				}
				else
				{
					trickJumpParameter.Set(msg.m_position, msg.m_missRotation, msg.m_missFirstSpeed, msg.m_missOutOfcontrol, msg.m_succeedRotation, msg.m_succeedFirstSpeed, msg.m_succeedOutOfcontrol, flag);
				}
				this.ChangeState(STATE_ID.TrickJump);
				msg.m_succeed = true;
			}
		}

		private void OnUpSpeedLevel(MsgUpSpeedLevel msg)
		{
			this.m_nowSpeedLevel = msg.m_level;
			this.SetSpeedLevel(this.m_nowSpeedLevel);
		}

		private void OnRunLoopPath(MsgRunLoopPath msg)
		{
			if (this.m_fsm != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		private void OnUseItem(MsgUseItem msg)
		{
			if (this.IsDead() || this.IsNowLastChance())
			{
				return;
			}
			switch (msg.m_itemType)
			{
			case ItemType.INVINCIBLE:
				if (!this.IsNowPhantom())
				{
					StateUtil.ActiveInvincible(this, msg.m_time);
				}
				break;
			case ItemType.BARRIER:
				StateUtil.ActiveBarrier(this);
				if (this.IsNowPhantom())
				{
					StateUtil.SetNotDrawBarrierEffect(this, true);
				}
				break;
			case ItemType.MAGNET:
				StateUtil.ActiveMagnetObject(this, msg.m_time);
				break;
			case ItemType.TRAMPOLINE:
				StateUtil.ActiveTrampoline(this, msg.m_time);
				break;
			case ItemType.COMBO:
				StateUtil.ActiveCombo(this, msg.m_time);
				break;
			case ItemType.LASER:
				if (this.NowPhantomType == PhantomType.NONE)
				{
					StateUtil.ChangeStateToChangePhantom(this, PhantomType.LASER, msg.m_time);
					return;
				}
				break;
			case ItemType.DRILL:
				if (this.NowPhantomType == PhantomType.NONE)
				{
					StateUtil.ChangeStateToChangePhantom(this, PhantomType.DRILL, msg.m_time);
					return;
				}
				break;
			case ItemType.ASTEROID:
				if (this.NowPhantomType == PhantomType.NONE)
				{
					StateUtil.ChangeStateToChangePhantom(this, PhantomType.ASTEROID, msg.m_time);
					return;
				}
				break;
			}
		}

		private void OnInvalidateItem(MsgInvalidateItem msg)
		{
			if (this.IsDead())
			{
				return;
			}
			if (this.IsNowLastChance())
			{
				if (msg.m_itemType == ItemType.COMBO)
				{
					StateUtil.DeactiveCombo(this, true);
				}
				return;
			}
			switch (msg.m_itemType)
			{
			case ItemType.INVINCIBLE:
				StateUtil.DeactiveInvincible(this);
				break;
			case ItemType.MAGNET:
				StateUtil.DeactiveMagetObject(this);
				break;
			case ItemType.TRAMPOLINE:
				StateUtil.DeactiveTrampoline(this);
				break;
			case ItemType.COMBO:
				StateUtil.DeactiveCombo(this, true);
				break;
			case ItemType.LASER:
				if (this.m_fsm != null && this.m_fsm.CurrentState != null)
				{
					StateUtil.SetChangePhantomCancel(this, msg.m_itemType);
					this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
				}
				break;
			case ItemType.DRILL:
				if (this.m_fsm != null && this.m_fsm.CurrentState != null)
				{
					StateUtil.SetChangePhantomCancel(this, msg.m_itemType);
					this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
				}
				break;
			case ItemType.ASTEROID:
				if (this.m_fsm != null && this.m_fsm.CurrentState != null)
				{
					StateUtil.SetChangePhantomCancel(this, msg.m_itemType);
					this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
				}
				break;
			}
		}

		private void WarpPosition(Vector3 pos, Quaternion rotation, bool hold)
		{
			Vector3 gravityDir = this.Movement.GetGravityDir();
			float distance = 0.2f;
			RaycastHit raycastHit;
			if (Physics.Raycast(pos, gravityDir, out raycastHit, distance))
			{
				pos = raycastHit.point + raycastHit.normal * 0.01f;
			}
			this.Movement.ResetPosition(pos);
			this.Movement.ResetRotation(rotation);
			if (this.m_information)
			{
				this.m_information.SetTransform(base.transform);
			}
			if (hold && this.m_fsm != null && this.m_fsm.CurrentStateID != (StateID)1)
			{
				this.ChangeState(STATE_ID.Hold);
			}
		}

		private void OnMsgStageReplace(MsgStageReplace msg)
		{
			Vector3 vector = msg.m_position;
			Vector3 b = new Vector3(0.5f, 0.2f, 0f);
			vector += b;
			this.WarpPosition(vector, msg.m_rotation, true);
		}

		private void OnMsgWarpPlayer(MsgWarpPlayer msg)
		{
			this.WarpPosition(msg.m_position, msg.m_rotation, msg.m_hold);
		}

		private void OnMsgStageRestart(MsgStageRestart msg)
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		private void OnMsgPLHold(MsgPLHold msg)
		{
			this.ChangeState(STATE_ID.Hold);
		}

		private void OnMsgPLReleaseHold(MsgPLReleaseHold msg)
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		private void OnPauseItemOnBoss(MsgPauseItemOnBoss msg)
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.DispatchMessage(this, msg.ID, msg);
			}
		}

		private void OnMsgExitStage(MsgExitStage msg)
		{
			base.enabled = false;
			this.Movement.enabled = false;
			StateUtil.DeactiveCombo(this, true);
		}

		private void OnMsgAbilityEffectStart(MsgAbilityEffectStart msg)
		{
			if (msg.m_loop)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, msg.m_effectName);
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			GameObject gameObject2 = StateUtil.CreateEffect(this, msg.m_effectName, true, ResourceCategory.CHAO_MODEL);
			if (gameObject2 != null && msg.m_center)
			{
				StateUtil.SetObjectLocalPositionToCenter(this, gameObject2);
			}
		}

		private void OnMsgAbilityEffectEnd(MsgAbilityEffectEnd msg)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, msg.m_effectName);
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		private void OnBossBoostLevel(MsgBossBoostLevel msg)
		{
			this.SetBoostLevel(msg.m_wispBoostLevel, msg.m_wispBoostEffect);
		}

		public static FSMStateFactory<CharacterState>[] GetCommonFSMTable()
		{
			return new FSMStateFactory<CharacterState>[]
			{
				new FSMStateFactory<CharacterState>(1, new StateEdit()),
				new FSMStateFactory<CharacterState>(2, new StateRun()),
				new FSMStateFactory<CharacterState>(3, new StateJump()),
				new FSMStateFactory<CharacterState>(8, new StateFall()),
				new FSMStateFactory<CharacterState>(9, new StateDamage()),
				new FSMStateFactory<CharacterState>(5, new StateSpringJump()),
				new FSMStateFactory<CharacterState>(6, new StateDashRing()),
				new FSMStateFactory<CharacterState>(10, new StateFallingDead()),
				new FSMStateFactory<CharacterState>(11, new StateDead()),
				new FSMStateFactory<CharacterState>(7, new StateAfterSpinAttack()),
				new FSMStateFactory<CharacterState>(12, new StateRunLoop()),
				new FSMStateFactory<CharacterState>(13, new StateChangePhantom()),
				new FSMStateFactory<CharacterState>(14, new StateReturnFromPhantom()),
				new FSMStateFactory<CharacterState>(15, new StatePhantomLaser()),
				new FSMStateFactory<CharacterState>(16, new StatePhantomLaserBoss()),
				new FSMStateFactory<CharacterState>(17, new StatePhantomDrill()),
				new FSMStateFactory<CharacterState>(18, new StatePhantomDrillBoss()),
				new FSMStateFactory<CharacterState>(19, new StatePhantomAsteroid()),
				new FSMStateFactory<CharacterState>(20, new StatePhantomAsteroidBoss()),
				new FSMStateFactory<CharacterState>(21, new StateReachCannon()),
				new FSMStateFactory<CharacterState>(22, new StateLaunchCannon()),
				new FSMStateFactory<CharacterState>(23, new StateHold()),
				new FSMStateFactory<CharacterState>(24, new StateTrickJump()),
				new FSMStateFactory<CharacterState>(25, new StateStumble()),
				new FSMStateFactory<CharacterState>(26, new StateDoubleJump()),
				new FSMStateFactory<CharacterState>(27, new StateThirdJump()),
				new FSMStateFactory<CharacterState>(28, new StateLastChance())
			};
		}
	}
}
