using System;

[Serializable]
public class ObjBossEggmanFeverParameter : SpawnableParameter
{
	public int m_hp;

	public int m_distance;

	public int m_tblId;

	public float m_downSpeed;

	public float m_attackInterspaceMin;

	public float m_attackInterspaceMax;

	public float m_boundParamMin;

	public float m_boundParamMax;

	public int m_boundMaxRand;

	public float m_shotSpeed;

	public float m_bumperFirstSpeed;

	public float m_bumperOutOfcontrol;

	public float m_bumperSpeedup;

	public float m_ballSpeed;

	public ObjBossEggmanFeverParameter() : base("ObjBossEggmanFever")
	{
		this.m_hp = 3;
		this.m_distance = 500;
		this.m_tblId = 0;
		this.m_downSpeed = 1f;
		this.m_attackInterspaceMin = 1f;
		this.m_attackInterspaceMax = 2f;
		this.m_boundParamMin = 0f;
		this.m_boundParamMax = 1.5f;
		this.m_boundMaxRand = 50;
		this.m_shotSpeed = 15f;
		this.m_bumperFirstSpeed = 10f;
		this.m_bumperOutOfcontrol = 0.3f;
		this.m_bumperSpeedup = 100f;
		this.m_ballSpeed = 8f;
	}
}
