using System;

namespace Boss
{
	public class ObjBossEggmanState : ObjBossState
	{
		private FSMSystem<ObjBossEggmanState> m_fsm;

		private STATE_ID m_initState = STATE_ID.AppearFever;

		private STATE_ID m_damageState = STATE_ID.DamageFever;

		private ObjBossEggmanParameter m_bossParam;

		private ObjBossEggmanEffect m_bossEffect;

		private ObjBossEggmanMotion m_bossMotion;

		public ObjBossEggmanParameter BossParam
		{
			get
			{
				return this.m_bossParam;
			}
		}

		public ObjBossEggmanEffect BossEffect
		{
			get
			{
				return this.m_bossEffect;
			}
		}

		public ObjBossEggmanMotion BossMotion
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
		}

		private ObjBossEggmanParameter GetBossParam()
		{
			return base.GetComponent<ObjBossEggmanParameter>();
		}

		private ObjBossEggmanEffect GetBossEffect()
		{
			return base.GetComponent<ObjBossEggmanEffect>();
		}

		private ObjBossEggmanMotion GetBossMotion()
		{
			return base.GetComponent<ObjBossEggmanMotion>();
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
		}

		protected override void OnChangeDamageState()
		{
			this.ChangeState(this.m_damageState);
		}

		private void MakeFSM()
		{
			FSMState<ObjBossEggmanState>[] array = new FSMState<ObjBossEggmanState>[]
			{
				new BossStateAppearFever(),
				new BossStateAppearMap1(),
				new BossStateAppearMap2(),
				new BossStateAppearMap2_2(),
				new BossStateAppearMap3(),
				new BossStateAttackFever(),
				new BossStateAttackMap1(),
				new BossStateAttackMap2(),
				new BossStateAttackMap3(),
				new BossStateDamageFever(),
				new BossStateDamageMap1(),
				new BossStateDamageMap2(),
				new BossStateDamageMap3(),
				new BossStatePassFever(),
				new BossStatePassFeverDistanceEnd(),
				new BossStatePassMap(),
				new BossStatePassMapDistanceEnd(),
				new BossStateDeadFever(),
				new BossStateDeadMap()
			};
			this.m_fsm = new FSMSystem<ObjBossEggmanState>();
			int num = 0;
			FSMState<ObjBossEggmanState>[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				FSMState<ObjBossEggmanState> s = array2[i];
				this.m_fsm.AddState(1 + num, s);
				num++;
			}
			base.SetSpeed(0f);
			this.m_fsm.Init(this, (int)this.m_initState);
		}

		public void ChangeState(STATE_ID state)
		{
			this.m_fsm.ChangeState(this, (int)state);
		}

		public void SetInitState(STATE_ID state)
		{
			this.m_initState = state;
		}

		public void SetDamageState(STATE_ID state)
		{
			this.m_damageState = state;
		}
	}
}
