using Message;
using System;
using UnityEngine;

namespace Boss
{
	public class ObjBossEventBossState : ObjBossState
	{
		private const float WISPSTART_TIME = 0f;

		private const float WISP_POSY_MIN = 0.5f;

		private const float WISP_POSY_MAX = 3f;

		private const float WISP_POSX = 15f;

		private bool m_damageWispCancel = true;

		private FSMSystem<ObjBossEventBossState> m_fsm;

		private EVENTBOSS_STATE_ID m_initState = EVENTBOSS_STATE_ID.AppearEvent1;

		private EVENTBOSS_STATE_ID m_damageState = EVENTBOSS_STATE_ID.DamageEvent1;

		private ObjBossEventBossParameter m_bossParam;

		private ObjBossEventBossEffect m_bossEffect;

		private ObjBossEventBossMotion m_bossMotion;

		private float m_wispTime;

		private float m_wispTimeMax;

		private WispBoostLevel m_currentBoostLevel = WispBoostLevel.NONE;

		private static readonly string[] BOOST_SE_NAME = new string[]
		{
			"rb_boost_1",
			"rb_boost_2",
			"rb_boost_3"
		};

		public ObjBossEventBossParameter BossParam
		{
			get
			{
				return this.m_bossParam;
			}
		}

		public ObjBossEventBossEffect BossEffect
		{
			get
			{
				return this.m_bossEffect;
			}
		}

		public ObjBossEventBossMotion BossMotion
		{
			get
			{
				return this.m_bossMotion;
			}
		}

		protected override void OnStart()
		{
			this.OnInit();
		}

		protected override void OnInit()
		{
			if (this.m_bossParam == null)
			{
				this.m_bossParam = this.GetBossParam();
			}
			if (this.m_bossEffect == null)
			{
				this.m_bossEffect = this.GetBossEffect();
			}
			if (this.m_bossMotion == null)
			{
				this.m_bossMotion = this.GetBossMotion();
			}
			this.m_wispTime = 0f;
			this.m_wispTimeMax = 0f;
			this.m_currentBoostLevel = WispBoostLevel.NONE;
		}

		private ObjBossEventBossParameter GetBossParam()
		{
			return base.GetComponent<ObjBossEventBossParameter>();
		}

		private ObjBossEventBossEffect GetBossEffect()
		{
			return base.GetComponent<ObjBossEventBossEffect>();
		}

		private ObjBossEventBossMotion GetBossMotion()
		{
			return base.GetComponent<ObjBossEventBossMotion>();
		}

		protected override ObjBossParameter OnGetBossParam()
		{
			return this.GetBossParam();
		}

		protected override ObjBossEffect OnGetBossEffect()
		{
			return this.GetBossEffect();
		}

		protected override ObjBossMotion OnGetBossMotion()
		{
			return this.GetBossMotion();
		}

		protected override void OnChangeChara()
		{
			this.ResetWisp();
		}

		protected override void OnSetup()
		{
			this.m_bossParam.Setup();
			this.m_bossMotion.Setup();
			this.MakeFSM();
		}

		private void OnDestroy()
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.Leave(this);
			}
		}

		protected override void OnFsmUpdate(float delta)
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.Step(this, delta);
			}
			this.UpdateWisp(delta);
		}

		protected override void OnChangeDamageState()
		{
			this.ChangeState(this.m_damageState);
		}

		private void MakeFSM()
		{
			FSMState<ObjBossEventBossState>[] array = new FSMState<ObjBossEventBossState>[]
			{
				new BossStateAppearEvent1(),
				new BossStateAppearEvent2(),
				new BossStateAppearEvent1_2(),
				new BossStateAppearEvent2_2(),
				new BossStateAttackEvent1(),
				new BossStateAttackEvent2(),
				new BossStateDamageEvent1(),
				new BossStateDamageEvent2(),
				new BossStatePassEvent(),
				new BossStatePassEventDistanceEnd(),
				new BossStateDeadEvent()
			};
			this.m_fsm = new FSMSystem<ObjBossEventBossState>();
			int num = 0;
			FSMState<ObjBossEventBossState>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				FSMState<ObjBossEventBossState> s = array2[i];
				this.m_fsm.AddState(1 + num, s);
				num++;
			}
			base.SetSpeed(0f);
			this.m_fsm.Init(this, (int)this.m_initState);
		}

		public void ChangeState(EVENTBOSS_STATE_ID state)
		{
			this.m_fsm.ChangeState(this, (int)state);
		}

		public void SetInitState(EVENTBOSS_STATE_ID state)
		{
			this.m_initState = state;
		}

		public void SetDamageState(EVENTBOSS_STATE_ID state)
		{
			this.m_damageState = state;
		}

		private void OnGetWisp()
		{
			int num = (int)this.m_bossParam.BoostLevel;
			float num2 = this.m_bossParam.BoostRatio + this.m_bossParam.BoostRatioAdd;
			if ((double)num2 >= 1.0)
			{
				num2 = 1f;
				if (num < 2)
				{
					num++;
				}
			}
			this.SetBoostLevel((WispBoostLevel)num, num2);
		}

		private void ResetWisp()
		{
			if (this.m_bossParam.BoostRatio > 0f)
			{
				this.m_bossParam.BoostRatio = 0f;
				this.m_bossParam.BoostLevel = WispBoostLevel.NONE;
				this.SetBoostLevel(this.m_bossParam.BoostLevel, this.m_bossParam.BoostRatio);
			}
		}

		public int GetDropRingAggressivity()
		{
			int result;
			if (base.ColorPowerHit || base.ChaoHit)
			{
				result = 1;
			}
			else
			{
				result = ObjUtil.GetChaoAbliltyValue(ChaoAbility.AGGRESSIVITY_UP_FOR_RAID_BOSS, (int)this.m_bossParam.ChallengeValue);
			}
			return result;
		}

		private void BoostMeter()
		{
			MsgCaution caution = new MsgCaution(HudCaution.Type.WISPBOOST, this.m_bossParam);
			HudCaution.Instance.SetCaution(caution);
		}

		private void UpdateWisp(float delta)
		{
			this.m_wispTime += delta;
			if (this.m_wispTime > this.m_wispTimeMax)
			{
				this.m_wispTime = 0f;
				this.m_wispTimeMax = this.m_bossParam.WispInterspace;
				float y = UnityEngine.Random.Range(0.5f, 3f);
				Vector3 pos = new Vector3(base.GetPlayerPosition().x + 15f, y, base.transform.position.z);
				this.CreateWisp(pos);
			}
			if (this.m_bossParam.BoostRatio > 0f)
			{
				this.m_bossParam.BoostRatio -= delta * this.m_bossParam.BoostRatioDown;
				if (this.m_bossParam.BoostRatio <= 0f)
				{
					this.m_bossParam.BoostRatio = 0f;
					this.m_bossParam.BoostLevel = WispBoostLevel.NONE;
					this.SetBoostLevel(this.m_bossParam.BoostLevel, this.m_bossParam.BoostRatio);
				}
			}
		}

		private void CreateWisp(Vector3 pos)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "ObjBossWisp");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, pos, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossWisp component = gameObject2.GetComponent<ObjBossWisp>();
					if (component)
					{
						float speed = UnityEngine.Random.Range(this.m_bossParam.WispSpeedMin, this.m_bossParam.WispSpeedMax);
						float num = UnityEngine.Random.Range(this.m_bossParam.WispSwingMin, this.m_bossParam.WispSwingMax);
						float addX = UnityEngine.Random.Range(this.m_bossParam.WispAddXMin, this.m_bossParam.WispAddXMax);
						float num2 = pos.y - num;
						if (num2 < 0f)
						{
							num = pos.y;
						}
						component.Setup(base.gameObject, speed, num, addX);
					}
				}
			}
		}

		private void SetBoostLevel(WispBoostLevel level, float ratio)
		{
			bool flag = false;
			if (this.m_currentBoostLevel != level)
			{
				flag = true;
				this.m_currentBoostLevel = level;
			}
			this.m_bossParam.BoostLevel = level;
			this.m_bossParam.BoostRatio = ratio;
			if (flag)
			{
				if (level == WispBoostLevel.NONE)
				{
					this.m_bossParam.BossAttackPower = 1;
				}
				else
				{
					this.m_bossParam.BossAttackPower = this.m_bossParam.GetBoostAttackParam(level);
					ObjUtil.PlayEventSE(ObjBossEventBossState.GetBoostSE(level), EventManager.EventType.RAID_BOSS);
				}
				this.BoostMeter();
				this.SendPlayerBoostLevel(level);
			}
		}

		private void SendPlayerBoostLevel(WispBoostLevel level)
		{
			string boostEffect = ObjBossEventBossEffect.GetBoostEffect(level);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnBossBoostLevel", new MsgBossBoostLevel(level, boostEffect), SendMessageOptions.DontRequireReceiver);
		}

		private static string GetBoostSE(WispBoostLevel level)
		{
			if ((ulong)level < (ulong)((long)ObjBossEventBossState.BOOST_SE_NAME.Length))
			{
				return ObjBossEventBossState.BOOST_SE_NAME[(int)level];
			}
			return string.Empty;
		}

		private void OnPlayerDamage(MsgBossPlayerDamage msg)
		{
			bool flag = this.m_damageWispCancel;
			if (msg.m_dead)
			{
				flag = true;
			}
			if (flag && this.m_bossParam.BoostLevel != WispBoostLevel.NONE)
			{
				this.m_bossParam.BoostRatio = 0f;
				this.m_bossParam.BoostLevel = WispBoostLevel.NONE;
				this.SetBoostLevel(this.m_bossParam.BoostLevel, this.m_bossParam.BoostRatio);
			}
		}
	}
}
