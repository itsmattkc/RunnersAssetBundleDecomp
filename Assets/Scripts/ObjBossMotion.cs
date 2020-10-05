using System;
using UnityEngine;

public class ObjBossMotion : MonoBehaviour
{
	protected class BossMotionParam
	{
		public string m_flagName;

		public int m_motionID;

		public BossMotionParam(string flagName, int motionID)
		{
			this.m_flagName = flagName;
			this.m_motionID = motionID;
		}
	}

	public bool m_debugDrawMotionInfo;

	protected Animator m_animator;

	public void Setup()
	{
		this.OnSetup();
	}

	protected virtual void OnSetup()
	{
	}
}
