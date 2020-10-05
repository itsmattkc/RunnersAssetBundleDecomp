using System;
using UnityEngine;

public class ObjBossEggmanMotion : ObjBossMotion
{
	private static readonly ObjBossMotion.BossMotionParam[] MOTION_DATA = new ObjBossMotion.BossMotionParam[]
	{
		new ObjBossMotion.BossMotionParam("Appear", 11),
		new ObjBossMotion.BossMotionParam("BomStart", 11),
		new ObjBossMotion.BossMotionParam("MissileStart", 9),
		new ObjBossMotion.BossMotionParam("MoveR", 11),
		new ObjBossMotion.BossMotionParam("Notice", 11),
		new ObjBossMotion.BossMotionParam("Pass", 11),
		new ObjBossMotion.BossMotionParam("Escape", 11),
		new ObjBossMotion.BossMotionParam("EscapeStart", 11),
		new ObjBossMotion.BossMotionParam("Damage", 2),
		new ObjBossMotion.BossMotionParam("Attack", 2)
	};

	protected override void OnSetup()
	{
	}

	public void SetMotion(BossMotion id, bool flag = true)
	{
		if (this.m_animator == null)
		{
			this.m_animator = base.GetComponentInChildren<Animator>();
		}
		if (this.m_animator && (ulong)id < (ulong)((long)ObjBossEggmanMotion.MOTION_DATA.Length))
		{
			string flagName = ObjBossEggmanMotion.MOTION_DATA[(int)((UIntPtr)id)].m_flagName;
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
					BossMotion motionID = (BossMotion)ObjBossEggmanMotion.MOTION_DATA[(int)((UIntPtr)id)].m_motionID;
					if (motionID != BossMotion.NONE)
					{
						this.SetMotion(motionID, false);
					}
				}
			}
		}
	}
}
