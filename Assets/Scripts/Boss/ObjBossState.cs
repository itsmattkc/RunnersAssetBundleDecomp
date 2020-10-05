using Message;
using System;
using UnityEngine;

namespace Boss
{
	public class ObjBossState : MonoBehaviour
	{
		public bool m_debugDrawState;

		public bool m_debugDrawInfo;

		protected PlayerInformation m_playerInfo;

		protected LevelInformation m_levelInfo;

		private ObjBossParameter m_param;

		private ObjBossEffect m_effect;

		private ObjBossMotion m_motion;

		private bool m_hitCheck;

		private bool m_colorPowerHit;

		private bool m_chaoHit;

		private bool m_speedKeep;

		private float m_keepDistance;

		private bool m_bossDistanceEnd;

		private bool m_bossDistanceEndArea;

		private bool m_phantom;

		private bool m_hitBumper;

		private bool m_playerDead;

		private bool m_playerChange;

		private Vector3 m_prevPlayerPos = Vector3.zero;

		private float m_moveStep;

		private float m_moveAddStep;

		private bool m_clear;

		public bool ColorPowerHit
		{
			get
			{
				return this.m_colorPowerHit;
			}
			set
			{
				this.m_colorPowerHit = value;
			}
		}

		public bool ChaoHit
		{
			get
			{
				return this.m_chaoHit;
			}
			set
			{
				this.m_chaoHit = value;
			}
		}

		private void Start()
		{
			this.SetBossStateAttackOK(false);
			this.OnStart();
		}

		protected virtual void OnStart()
		{
		}

		public void Init()
		{
			if (this.m_param == null)
			{
				this.m_param = this.OnGetBossParam();
			}
			if (this.m_effect == null)
			{
				this.m_effect = this.OnGetBossEffect();
			}
			if (this.m_motion == null)
			{
				this.m_motion = this.OnGetBossMotion();
			}
			this.OnInit();
		}

		protected virtual void OnInit()
		{
		}

		protected virtual ObjBossParameter OnGetBossParam()
		{
			return null;
		}

		protected virtual ObjBossEffect OnGetBossEffect()
		{
			return null;
		}

		protected virtual ObjBossMotion OnGetBossMotion()
		{
			return null;
		}

		protected virtual void OnChangeChara()
		{
		}

		public void Setup()
		{
			this.m_playerInfo = ObjUtil.GetPlayerInformation();
			this.m_levelInfo = ObjUtil.GetLevelInformation();
			if (this.m_levelInfo != null)
			{
				this.m_levelInfo.BossEndTime = (float)this.m_param.BossDistance;
			}
			this.OnSetup();
		}

		protected virtual void OnSetup()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
			if (this.m_playerInfo != null)
			{
				if (!this.m_playerDead)
				{
					if (this.m_playerInfo.IsDead())
					{
						GameObject[] array = GameObject.FindGameObjectsWithTag("Gimmick");
						GameObject[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							GameObject gameObject = array2[i];
							gameObject.SendMessage("OnMsgNotifyDead", new MsgNotifyDead(), SendMessageOptions.DontRequireReceiver);
						}
						this.m_playerDead = true;
					}
				}
				else if (this.m_playerChange && !this.m_playerInfo.IsDead())
				{
					this.m_playerDead = false;
				}
			}
			float deltaTime = Time.deltaTime;
			this.OnFsmUpdate(deltaTime);
			this.m_hitBumper = false;
			this.UpdateMove(deltaTime);
			this.DebugDrawInfo();
		}

		protected virtual void OnFsmUpdate(float delta)
		{
		}

		public void UpdateMove(float delta)
		{
			if (this.m_speedKeep && !this.m_phantom)
			{
				base.transform.position = new Vector3(this.GetPlayerPosition().x + this.m_keepDistance, base.transform.position.y, base.transform.position.z);
			}
			else
			{
				Vector3 playerPosition = this.GetPlayerPosition();
				Vector3 prevPlayerPos = this.m_prevPlayerPos;
				if (this.m_playerDead || prevPlayerPos != playerPosition)
				{
					float keepDistance = this.m_keepDistance;
					float playerBossPositionX = this.GetPlayerBossPositionX();
					float num = keepDistance - playerBossPositionX;
					if (Mathf.Abs(num) > 0.01f && this.m_param.PlayerSpeed > this.m_param.Speed)
					{
						float num2 = num / this.m_param.PlayerSpeed;
						float num3 = this.m_param.Speed * num2;
						this.m_keepDistance = playerBossPositionX + num3;
					}
					else
					{
						float num4 = this.m_param.Speed * delta;
						this.m_keepDistance = playerBossPositionX + num4 + this.m_moveAddStep;
					}
					this.m_moveStep -= this.m_moveAddStep;
					if (this.m_moveStep < 0f)
					{
						this.m_moveStep = 0f;
						this.m_moveAddStep = 0f;
					}
				}
				else
				{
					float num5 = this.m_param.Speed * delta;
					this.m_moveStep += num5;
					this.m_moveAddStep = this.m_moveStep * 0.05f;
				}
				this.m_prevPlayerPos = playerPosition;
				base.transform.position = new Vector3(this.GetPlayerPosition().x + this.m_keepDistance, base.transform.position.y, base.transform.position.z);
			}
		}

		public void UpdateSpeedDown(float delta, float down)
		{
			this.m_speedKeep = false;
			this.m_param.Speed -= delta * down;
			if (this.m_param.Speed < this.m_param.MinSpeed)
			{
				this.m_param.Speed = this.m_param.MinSpeed;
			}
		}

		public void UpdateSpeedUp(float delta, float up)
		{
			this.m_speedKeep = false;
			this.m_param.Speed += delta * up;
		}

		public void SetSpeed(float speed)
		{
			this.m_speedKeep = false;
			this.m_param.Speed = speed;
			this.m_keepDistance = this.GetPlayerBossPositionX();
		}

		public void KeepSpeed()
		{
			this.m_speedKeep = true;
			this.m_param.Speed = this.m_param.PlayerSpeed;
			this.m_keepDistance = this.GetPlayerBossPositionX();
		}

		public void SetupMoveY(float step)
		{
			this.m_param.StepMoveY = step;
		}

		public void UpdateMoveY(float delta, float pos_y, float speed)
		{
			this.m_param.StepMoveY -= delta * this.m_param.StepMoveY * 0.5f * speed;
			if (this.m_param.StepMoveY < 0.01f)
			{
				this.m_param.StepMoveY = 0f;
			}
			Vector3 zero = Vector3.zero;
			Vector3 target = new Vector3(base.transform.position.x, pos_y, base.transform.position.z);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref zero, this.m_param.StepMoveY);
		}

		public float GetPlayerDistance()
		{
			return Mathf.Abs(this.GetPlayerBossPositionX());
		}

		public Vector3 GetPlayerPosition()
		{
			if (this.m_playerInfo != null)
			{
				return this.m_playerInfo.Position;
			}
			return Vector3.zero;
		}

		public float GetPlayerBossPositionX()
		{
			return base.transform.position.x - this.GetPlayerPosition().x;
		}

		public void DebugDrawState(string name)
		{
			if (this.m_debugDrawState)
			{
				global::Debug.Log("BossState(" + name + ")");
			}
		}

		public void SetHitCheck(bool flag)
		{
			if (this.m_hitCheck != flag)
			{
				this.m_hitCheck = flag;
				this.SetBossStateAttackOK(flag);
			}
		}

		public bool IsBossDistanceEnd()
		{
			return this.m_bossDistanceEnd;
		}

		public bool IsPlayerDead()
		{
			return this.m_playerDead;
		}

		public bool IsHitBumper()
		{
			return this.m_hitBumper;
		}

		public bool IsClear()
		{
			return this.m_clear;
		}

		public void AddDamage()
		{
			int hpDown = ObjUtil.GetChaoAbliltyValue(ChaoAbility.AGGRESSIVITY_UP_FOR_RAID_BOSS, this.m_param.BossAttackPower);
			if (this.ColorPowerHit || this.ChaoHit)
			{
				hpDown = 1;
			}
			this.SetHpDown(hpDown);
		}

		private void SetHpDown(int addDamage)
		{
			int count = this.m_param.BossHP;
			this.m_param.BossHP -= addDamage;
			if (this.m_param.BossHP < 0)
			{
				this.m_param.BossHP = 0;
			}
			else
			{
				count = addDamage;
			}
			if (this.m_param.TypeBoss != 0 && this.m_levelInfo != null)
			{
				this.m_levelInfo.AddNumBossAttack(count);
			}
			this.SetHpGauge(this.m_param.BossHP);
		}

		public void ChaoHpDown()
		{
			int bossHP = this.m_param.BossHP;
			int num = bossHP - ObjUtil.GetChaoAbliltyValue(ChaoAbility.MAP_BOSS_DAMAGE, bossHP);
			if (num > 0)
			{
				this.SetHpDown(num);
			}
		}

		public void RequestStartChaoAbility()
		{
			if (EventManager.Instance != null && !EventManager.Instance.IsRaidBossStage())
			{
				ObjUtil.RequestStartAbilityToChao(ChaoAbility.BOSS_SUPER_RING_RATE, true);
				ObjUtil.RequestStartAbilityToChao(ChaoAbility.BOSS_RED_RING_RATE, true);
			}
			BossType typeBoss = (BossType)this.m_param.TypeBoss;
			if (typeBoss != BossType.FEVER)
			{
				ObjUtil.RequestStartAbilityToChao(ChaoAbility.MAP_BOSS_DAMAGE, true);
				this.ChaoHpDown();
			}
			else
			{
				ObjUtil.RequestStartAbilityToChao(ChaoAbility.BOSS_STAGE_TIME, true);
			}
			this.m_effect.PlayChaoEffect();
		}

		public void BossEnd(bool dead)
		{
			bool flag = false;
			if (StageTutorialManager.Instance && !StageTutorialManager.Instance.IsCompletedTutorial())
			{
				flag = true;
			}
			if (flag)
			{
				GameObjectUtil.SendMessageFindGameObject("StageTutorialManager", "OnMsgTutorialEnd", new MsgTutorialEnd(), SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				if (this.m_param.TypeBoss != 0)
				{
					this.AddStockRing();
				}
				MsgBossEnd value = new MsgBossEnd(dead);
				GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnBossEnd", value, SendMessageOptions.DontRequireReceiver);
			}
		}

		public void BossClear()
		{
			this.m_clear = true;
			MsgBossClear value = new MsgBossClear();
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnBossClear", value, SendMessageOptions.DontRequireReceiver);
		}

		public void SetBossStateAttackOK(bool flag)
		{
			if (this.m_bossDistanceEndArea)
			{
				return;
			}
			if (flag && this.m_param.AfterAttack)
			{
				if (ObjUtil.RequestStartAbilityToChao(ChaoAbility.PURSUES_TO_BOSS_AFTER_ATTACK, false))
				{
					flag = false;
				}
				else
				{
					this.m_param.AfterAttack = false;
				}
			}
			bool flag2 = false;
			if (flag)
			{
				flag2 = ObjUtil.RequestStartAbilityToChao(ChaoAbility.BOSS_ATTACK, false);
			}
			else
			{
				ObjUtil.RequestEndAbilityToChao(ChaoAbility.BOSS_ATTACK);
			}
			MsgBossCheckState.State state = MsgBossCheckState.State.IDLE;
			if (flag && !flag2)
			{
				state = MsgBossCheckState.State.ATTACK_OK;
			}
			if (StageItemManager.Instance != null)
			{
				MsgBossCheckState msg = new MsgBossCheckState(state);
				StageItemManager.Instance.OnMsgBossCheckState(msg);
			}
		}

		public void UpdateBossStateAfterAttack()
		{
			this.m_param.AfterAttack = !this.m_param.AfterAttack;
		}

		public void CreateFeverRing()
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "ObjFeverRing");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjFeverRing component = gameObject2.GetComponent<ObjFeverRing>();
					if (component)
					{
						component.Setup(this.m_param.RingCount, this.m_param.SuperRingRatio, this.m_param.RedStarRingRatio, this.m_param.BronzeTimerRatio, this.m_param.SilverTimerRatio, this.m_param.GoldTimerRatio, (BossType)this.m_param.TypeBoss);
					}
				}
			}
		}

		public void CreateEventFeverRing(int playerAggressivity)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "ObjFeverRing");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					int num = EventBossParamTable.GetSuperRingDropData((BossType)this.m_param.TypeBoss, playerAggressivity);
					gameObject2.gameObject.SetActive(true);
					ObjFeverRing component = gameObject2.GetComponent<ObjFeverRing>();
					if (component)
					{
						if (this.m_param.RedStarRingRatio + num > 100)
						{
							num = 100 - this.m_param.RedStarRingRatio;
						}
						component.Setup(this.m_param.RingCount, num, this.m_param.RedStarRingRatio, this.m_param.BronzeTimerRatio, this.m_param.SilverTimerRatio, this.m_param.GoldTimerRatio, (BossType)this.m_param.TypeBoss);
					}
				}
			}
		}

		public void CreateMissile(Vector3 pos)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossMissile");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, pos, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossMissile component = gameObject2.GetComponent<ObjBossMissile>();
					if (component)
					{
						component.Setup(base.gameObject, this.m_param.MissileSpeed, (BossType)this.m_param.TypeBoss);
					}
				}
			}
		}

		public void CreateTrapBall(Vector3 colli, float attackPos, int randBall, bool bossAppear)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossTrapBall");
			if (gameObject != null)
			{
				Vector3 position = base.transform.position;
				if (!bossAppear)
				{
					position = new Vector3(this.GetPlayerPosition().x + 18f, attackPos, position.z);
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossTrapBall component = gameObject2.GetComponent<ObjBossTrapBall>();
					if (component)
					{
						bool flag = true;
						if (this.m_param.AttackBallFlag)
						{
							flag = false;
							this.m_param.AttackBallFlag = false;
						}
						else
						{
							int randomRange = ObjUtil.GetRandomRange100();
							if (randomRange >= randBall && this.m_param.AttackTrapCount < this.m_param.AttackTrapCountMax)
							{
								flag = false;
							}
						}
						BossTrapBallType type;
						if (!flag)
						{
							type = BossTrapBallType.BREAK;
							this.m_param.AttackTrapCount++;
						}
						else
						{
							type = BossTrapBallType.ATTACK;
							this.m_param.AttackBallFlag = true;
							this.m_param.AttackTrapCount = 0;
						}
						component.Setup(base.gameObject, colli, this.m_param.RotSpeed, this.m_param.AttackSpeed, type, (BossType)this.m_param.TypeBoss, bossAppear);
					}
				}
			}
		}

		public GameObject CreateBom(bool colli, float shot_speed, bool shot)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossBom");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, ObjBossUtil.GetBossHatchPos(base.gameObject), Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossBom component = gameObject2.GetComponent<ObjBossBom>();
					if (component)
					{
						component.Setup(base.gameObject, colli, this.GetShotRotation(this.m_param.ShotRotBase), shot_speed, this.m_param.AddSpeedRatio, shot);
					}
					return gameObject2;
				}
			}
			return null;
		}

		public void BlastBom(GameObject bom_obj)
		{
			if (bom_obj)
			{
				ObjBossBom component = bom_obj.GetComponent<ObjBossBom>();
				if (component)
				{
					component.Blast("ef_bo_em_dead_bom01", 5f);
				}
			}
		}

		public void ShotBom(GameObject bom_obj)
		{
			if (bom_obj)
			{
				ObjBossBom component = bom_obj.GetComponent<ObjBossBom>();
				if (component)
				{
					component.SetShot(true);
				}
			}
		}

		public void CreateBumper(bool bossAppear, float addX = 0f)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossBall");
			if (gameObject != null)
			{
				Vector3 position = Vector3.zero;
				if (!bossAppear)
				{
					position = new Vector3(this.GetPlayerPosition().x + addX, base.transform.position.y, base.transform.position.z);
				}
				else
				{
					position = ObjBossUtil.GetBossHatchPos(base.gameObject);
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossBall component = gameObject2.GetComponent<ObjBossBall>();
					if (component)
					{
						component.Setup(new ObjBossBall.SetData
						{
							obj = base.gameObject,
							bound_param = this.GetBoundParam(),
							type = BossBallType.BUMPER,
							shot_rot = this.GetShotRotation(this.m_param.ShotRotBase),
							shot_speed = this.m_param.ShotSpeed,
							attack_speed = 0f,
							firstSpeed = this.m_param.BumperFirstSpeed,
							outOfcontrol = this.m_param.BumperOutOfcontrol,
							ballSpeed = this.m_param.BallSpeed,
							bossAppear = bossAppear
						});
					}
				}
			}
		}

		public Quaternion GetShotRotation(Vector3 rot_angle)
		{
			float num = 0f;
			if (this.m_param.Speed > 0f)
			{
				num = this.m_param.Speed / this.m_param.PlayerSpeed;
			}
			float num2 = 30f * num * this.m_param.AddSpeedRatio;
			if (num2 > 60f)
			{
				num2 = 60f;
			}
			Vector3 euler = rot_angle * num2;
			return base.transform.rotation * Quaternion.FromToRotation(base.transform.up, -base.transform.up) * Quaternion.Euler(euler);
		}

		public void OpenHpGauge()
		{
			if (ObjBossUtil.IsNowLastChance(this.m_playerInfo))
			{
				return;
			}
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "HudBossHpGaugeOpen", new MsgHudBossHpGaugeOpen((BossType)this.m_param.TypeBoss, this.m_param.BossLevel, this.m_param.BossHP, this.m_param.BossHPMax), SendMessageOptions.DontRequireReceiver);
		}

		public void StartGauge()
		{
			if (ObjBossUtil.IsNowLastChance(this.m_playerInfo))
			{
				return;
			}
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "HudBossGaugeStart", new MsgHudBossGaugeStart(), SendMessageOptions.DontRequireReceiver);
		}

		public void SetHpGauge(int hp)
		{
			if (ObjBossUtil.IsNowLastChance(this.m_playerInfo))
			{
				return;
			}
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "HudBossHpGaugeSet", new MsgHudBossHpGaugeSet(this.m_param.BossHP, this.m_param.BossHPMax), SendMessageOptions.DontRequireReceiver);
		}

		public void SetClear()
		{
			if (HudCaution.Instance != null)
			{
				MsgCaution caution = new MsgCaution(HudCaution.Type.MAP_BOSS_CLEAR);
				HudCaution.Instance.SetCaution(caution);
			}
		}

		public void SetFailed()
		{
			if (HudCaution.Instance != null)
			{
				MsgCaution caution = new MsgCaution(HudCaution.Type.MAP_BOSS_FAILED);
				HudCaution.Instance.SetCaution(caution);
				ObjUtil.PlaySE("boss_failed", "SE");
			}
		}

		private void AddStockRing()
		{
			ObjUtil.SendMessageTransferRing();
		}

		public float GetBoundParam()
		{
			int randomRange = ObjUtil.GetRandomRange100();
			if (randomRange < this.m_param.BoundMaxRand)
			{
				return this.GetBoundParam(this.m_param.BoundParamMax);
			}
			return this.GetBoundParam(this.m_param.BoundParamMin);
		}

		public float GetBoundParam(float param)
		{
			return param;
		}

		public float GetAttackInterspace()
		{
			return UnityEngine.Random.Range(this.m_param.AttackInterspaceMin, this.m_param.AttackInterspaceMax);
		}

		public float GetDamageSpeedParam()
		{
			float playerDistance = this.GetPlayerDistance();
			float num = 1f - playerDistance * 0.04f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return num;
		}

		public void OnMsgBossDistanceEnd(MsgBossDistanceEnd msg)
		{
			if (msg.m_end)
			{
				this.m_bossDistanceEnd = true;
			}
			else
			{
				this.m_bossDistanceEndArea = true;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player")
				{
					bool flag = false;
					if (gameObject.name == "ChaoPartsAttackEnemy" || gameObject.name.Contains("pha_"))
					{
						flag = true;
					}
					else if (this.m_hitCheck)
					{
						flag = true;
					}
					if (flag)
					{
						MsgHitDamage value = new MsgHitDamage(base.gameObject, AttackPower.PlayerSpin);
						gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			if (this.m_hitCheck)
			{
				this.m_effect.SetHitOffset(gameObject.transform.position - base.transform.position);
			}
		}

		private void OnDamageHit(MsgHitDamage msg)
		{
			if (msg.m_sender)
			{
				GameObject gameObject = msg.m_sender.gameObject;
				if (gameObject && msg.m_attackPower > 0)
				{
					if (this.m_hitCheck)
					{
						MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
						gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
						if (gameObject.tag == "ChaoAttack" || gameObject.tag == "Chao")
						{
							this.ChaoHit = true;
						}
						if (msg.m_attackPower == 4)
						{
							this.ColorPowerHit = true;
						}
						this.OnChangeDamageState();
					}
					else if (gameObject.tag == "ChaoAttack" || gameObject.tag == "Chao" || msg.m_attackPower == 4)
					{
						MsgHitDamageSucceed value2 = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
						gameObject.SendMessage("OnDamageSucceed", value2, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}

		protected virtual void OnChangeDamageState()
		{
		}

		public void OnTransformPhantom(MsgTransformPhantom msg)
		{
			this.m_phantom = true;
		}

		public void OnReturnFromPhantom(MsgReturnFromPhantom msg)
		{
			this.m_phantom = false;
		}

		public void OnChangeCharaSucceed(MsgChangeCharaSucceed msg)
		{
			this.m_playerChange = true;
			this.OnChangeChara();
		}

		public void OnMsgPrepareContinue(MsgPrepareContinue msg)
		{
			this.m_playerChange = true;
		}

		public void OnMsgDebugDead()
		{
			if (this.m_param.BossHP > 0 && this.m_param.TypeBoss != 0 && this.m_levelInfo != null)
			{
				this.m_levelInfo.AddNumBossAttack(this.m_param.BossHP);
			}
			this.BossEnd(true);
		}

		public void OnHitBumper()
		{
			this.m_hitBumper = true;
		}

		private void DebugDrawInfo()
		{
			if (this.m_debugDrawInfo)
			{
				global::Debug.Log(string.Concat(new object[]
				{
					"BossInfo BossSpeed=",
					this.m_param.Speed,
					" PlayerSpeed=",
					this.m_param.PlayerSpeed,
					"AddSpeedRatio=",
					this.m_param.AddSpeedRatio,
					"AddSpeed=",
					this.m_param.AddSpeed
				}));
			}
		}

		public void DebugDrawInfo(string str)
		{
			if (this.m_debugDrawInfo)
			{
				global::Debug.Log("BossInfo " + str);
			}
		}
	}
}
