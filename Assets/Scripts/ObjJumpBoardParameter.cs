using System;

[Serializable]
public class ObjJumpBoardParameter : SpawnableParameter
{
	public float m_succeedFirstSpeed;

	public float m_succeedAngle;

	public float m_succeedOutOfcontrol;

	public float m_missFirstSpeed;

	public float m_missAngle;

	public float m_missOutOfcontrol;

	public ObjJumpBoardParameter() : base("ObjJumpBoard")
	{
		this.m_succeedFirstSpeed = 20f;
		this.m_succeedAngle = 45f;
		this.m_succeedOutOfcontrol = 0.2f;
		this.m_missFirstSpeed = 10f;
		this.m_missAngle = 30f;
		this.m_missOutOfcontrol = 0.2f;
	}
}
