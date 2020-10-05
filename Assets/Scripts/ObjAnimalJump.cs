using System;

public class ObjAnimalJump : ObjAnimalBase
{
	private const float JUMP_SPEED = 6f;

	private const float JUMP_GRAVITY = -6.1f;

	private const float JUMP_ADD_X = 1f;

	private const float BOUND_ADD_X = 3f;

	private const float BOUND_DOWN_X = 0.2f;

	private const float BOUND_DOWN_Y = 0f;

	private const float HIT_CHECK_DISTANCE = 1f;

	protected override float GetCheckGroundHitLength()
	{
		return 1f;
	}

	protected override void StartNextComponent()
	{
		MotorAnimalJump component = base.GetComponent<MotorAnimalJump>();
		if (component)
		{
			component.enabled = true;
			MotorAnimalJump.JumpParam jumpParam = default(MotorAnimalJump.JumpParam);
			jumpParam.m_obj = base.gameObject;
			jumpParam.m_speed = 6f;
			jumpParam.m_gravity = -6.1f;
			jumpParam.m_add_x = 1f + base.GetMoveSpeed();
			jumpParam.m_up = base.transform.up;
			jumpParam.m_forward = base.transform.right;
			jumpParam.m_bound = true;
			jumpParam.m_bound_add_y = 3f;
			jumpParam.m_bound_down_x = 0.2f;
			jumpParam.m_bound_down_y = 0f;
			component.Setup(ref jumpParam);
		}
	}

	protected override void EndNextComponent()
	{
		MotorAnimalJump component = base.GetComponent<MotorAnimalJump>();
		if (component)
		{
			component.enabled = false;
			component.SetEnd();
		}
	}
}
