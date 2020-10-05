using System;
using UnityEngine;

public class ObjBossEventBossMotion : ObjBossMotion
{
	private static readonly ObjBossMotion.BossMotionParam[] MOTION_DATA = new ObjBossMotion.BossMotionParam[]
	{
		new ObjBossMotion.BossMotionParam("Appear", 6),
		new ObjBossMotion.BossMotionParam("Pass", 6),
		new ObjBossMotion.BossMotionParam("Escape", 6),
		new ObjBossMotion.BossMotionParam("Damage", 4),
		new ObjBossMotion.BossMotionParam("Attack", 3)
	};

	protected override void OnSetup()
	{
	}

	public void SetMotion(EventBossMotion id, bool flag = true)
	{
		if (this.m_animator == null)
		{
			this.m_animator = base.GetComponentInChildren<Animator>();
		}
		if (this.m_animator && (ulong)id < (ulong)((long)ObjBossEventBossMotion.MOTION_DATA.Length))
		{
			string flagName = ObjBossEventBossMotion.MOTION_DATA[(int)((UIntPtr)id)].m_flagName;
			if (flagName != string.Empty)
			{
				this.m_animator.SetBool(flagName, flag);
				if (this.m_debugDrawMotionInfo)
				{
					global::Debug.Log(string.Concat(new object[]
					{
						"SetMotion ",
						flagName,
						" flag=",
						flag
					}));
				}
				if (flag)
				{
					EventBossMotion motionID = (EventBossMotion)ObjBossEventBossMotion.MOTION_DATA[(int)((UIntPtr)id)].m_motionID;
					if (motionID != EventBossMotion.NONE)
					{
						this.SetMotion(motionID, false);
					}
				}
			}
		}
	}
}
