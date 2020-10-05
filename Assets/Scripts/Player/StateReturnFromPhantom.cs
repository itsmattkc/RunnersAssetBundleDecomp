using System;

namespace Player
{
	public class StateReturnFromPhantom : FSMState<CharacterState>
	{
		private string m_animTriggerName;

		private PhantomType m_phantomType;

		public override void Enter(CharacterState context)
		{
			StateUtil.ResetVelocity(context);
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, true);
			context.OnAttack(AttackPower.PlayerColorPower, DefensePower.PlayerColorPower);
			this.m_phantomType = PhantomType.NONE;
			this.m_animTriggerName = null;
			ChangePhantomParameter enteringParameter = context.GetEnteringParameter<ChangePhantomParameter>();
			if (enteringParameter != null)
			{
				this.m_phantomType = enteringParameter.ChangeType;
				switch (this.m_phantomType)
				{
				case PhantomType.LASER:
					this.m_animTriggerName = "Laser";
					break;
				case PhantomType.DRILL:
					this.m_animTriggerName = "Drill";
					break;
				case PhantomType.ASTEROID:
					this.m_animTriggerName = "Asteroid";
					break;
				}
			}
			context.GetAnimator().CrossFade(this.m_animTriggerName, 0.1f);
			SoundManager.SePlay("phantom_change", "SE");
			StateUtil.SetPhantomQuickTimerPause(true);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			this.m_animTriggerName = null;
			StateUtil.SetPhantomQuickTimerPause(false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			string animName = "End" + this.m_animTriggerName;
			if (StateUtil.IsAnimationEnd(context, animName))
			{
				context.ChangeState(STATE_ID.Fall);
				return;
			}
		}
	}
}
