using System;

[Serializable]
public class ObjBossZazz1Parameter : SpawnableParameter
{
	public float m_playerDistance;

	public float m_bumperFirstSpeed;

	public float m_bumperOutOfcontrol;

	public float m_bumperSpeedup;

	public float m_shotSpeed;

	public int m_LV1_distance;

	public int m_LV1_tblId;

	public float m_LV1_missileSpeed;

	public float m_LV1_missileInterspace;

	public int m_LV1_missilePos1;

	public int m_LV1_missilePos2;

	public float m_LV1_missileWaitTime;

	public int m_LV1_missileCount;

	public float m_LV1_boundParamMin;

	public float m_LV1_boundParamMax;

	public int m_LV1_boundMaxRand;

	public int m_LV1_bumperRand;

	public float m_LV1_ballSpeed;

	public float m_LV1_wispInterspace;

	public float m_LV1_bumperInterspace;

	public float m_LV1_wispSpeedMin;

	public float m_LV1_wispSpeedMax;

	public float m_LV1_wispSwingMin;

	public float m_LV1_wispSwingMax;

	public float m_LV1_wispAddXMin;

	public float m_LV1_wispAddXMax;

	public int m_LV2_distance;

	public int m_LV2_tblId;

	public float m_LV2_missileSpeed;

	public float m_LV2_missileInterspace;

	public int m_LV2_missilePos1;

	public int m_LV2_missilePos2;

	public float m_LV2_missileWaitTime;

	public int m_LV2_missileCount;

	public float m_LV2_boundParamMin;

	public float m_LV2_boundParamMax;

	public int m_LV2_boundMaxRand;

	public int m_LV2_bumperRand;

	public float m_LV2_ballSpeed;

	public float m_LV2_wispInterspace;

	public float m_LV2_bumperInterspace;

	public float m_LV2_wispSpeedMin;

	public float m_LV2_wispSpeedMax;

	public float m_LV2_wispSwingMin;

	public float m_LV2_wispSwingMax;

	public float m_LV2_wispAddXMin;

	public float m_LV2_wispAddXMax;

	public int m_LV3_distance;

	public int m_LV3_tblId;

	public float m_LV3_missileSpeed;

	public float m_LV3_missileInterspace;

	public int m_LV3_missilePos1;

	public int m_LV3_missilePos2;

	public float m_LV3_missileWaitTime;

	public int m_LV3_missileCount;

	public float m_LV3_boundParamMin;

	public float m_LV3_boundParamMax;

	public int m_LV3_boundMaxRand;

	public int m_LV3_bumperRand;

	public float m_LV3_ballSpeed;

	public float m_LV3_wispInterspace;

	public float m_LV3_bumperInterspace;

	public float m_LV3_wispSpeedMin;

	public float m_LV3_wispSpeedMax;

	public float m_LV3_wispSwingMin;

	public float m_LV3_wispSwingMax;

	public float m_LV3_wispAddXMin;

	public float m_LV3_wispAddXMax;

	public int m_LV4_distance;

	public int m_LV4_tblId;

	public float m_LV4_missileSpeed;

	public float m_LV4_missileInterspace;

	public int m_LV4_missilePos1;

	public int m_LV4_missilePos2;

	public float m_LV4_missileWaitTime;

	public int m_LV4_missileCount;

	public float m_LV4_boundParamMin;

	public float m_LV4_boundParamMax;

	public int m_LV4_boundMaxRand;

	public int m_LV4_bumperRand;

	public float m_LV4_ballSpeed;

	public float m_LV4_wispInterspace;

	public float m_LV4_bumperInterspace;

	public float m_LV4_wispSpeedMin;

	public float m_LV4_wispSpeedMax;

	public float m_LV4_wispSwingMin;

	public float m_LV4_wispSwingMax;

	public float m_LV4_wispAddXMin;

	public float m_LV4_wispAddXMax;

	public int m_LV5_distance;

	public int m_LV5_tblId;

	public float m_LV5_missileSpeed;

	public float m_LV5_missileInterspace;

	public int m_LV5_missilePos1;

	public int m_LV5_missilePos2;

	public float m_LV5_missileWaitTime;

	public int m_LV5_missileCount;

	public float m_LV5_boundParamMin;

	public float m_LV5_boundParamMax;

	public int m_LV5_boundMaxRand;

	public int m_LV5_bumperRand;

	public float m_LV5_ballSpeed;

	public float m_LV5_wispInterspace;

	public float m_LV5_bumperInterspace;

	public float m_LV5_wispSpeedMin;

	public float m_LV5_wispSpeedMax;

	public float m_LV5_wispSwingMin;

	public float m_LV5_wispSwingMax;

	public float m_LV5_wispAddXMin;

	public float m_LV5_wispAddXMax;

	public ObjBossZazz1Parameter()
	{
		this.m_playerDistance = 8.5f;
		this.m_bumperFirstSpeed = 10f;
		this.m_bumperOutOfcontrol = 0.3f;
		this.m_bumperSpeedup = 200f;
		this.m_shotSpeed = 15f;
		this.m_LV1_distance = 500;
		this.m_LV1_tblId = 0;
		this.m_LV1_missileSpeed = 6f;
		this.m_LV1_missileInterspace = 1f;
		this.m_LV1_missilePos1 = 40;
		this.m_LV1_missilePos2 = 40;
		this.m_LV1_missileWaitTime = 2f;
		this.m_LV1_missileCount = 5;
		this.m_LV1_boundParamMin = 0f;
		this.m_LV1_boundParamMax = 1.5f;
		this.m_LV1_boundMaxRand = 50;
		this.m_LV1_bumperRand = 30;
		this.m_LV1_ballSpeed = 8f;
		this.m_LV1_wispInterspace = 0.8f;
		this.m_LV1_bumperInterspace = 1.3f;
		this.m_LV1_wispSpeedMin = 0.3f;
		this.m_LV1_wispSpeedMax = 0.7f;
		this.m_LV1_wispSwingMin = 0.3f;
		this.m_LV1_wispSwingMax = 0.5f;
		this.m_LV1_wispAddXMin = -3f;
		this.m_LV1_wispAddXMax = -1f;
		this.m_LV2_distance = 700;
		this.m_LV2_tblId = 0;
		this.m_LV2_missileSpeed = 6f;
		this.m_LV2_missileInterspace = 1f;
		this.m_LV2_missilePos1 = 40;
		this.m_LV2_missilePos2 = 40;
		this.m_LV2_missileWaitTime = 2f;
		this.m_LV2_missileCount = 5;
		this.m_LV2_boundParamMin = 0f;
		this.m_LV2_boundParamMax = 1.5f;
		this.m_LV2_boundMaxRand = 50;
		this.m_LV2_bumperRand = 30;
		this.m_LV2_ballSpeed = 8f;
		this.m_LV2_wispInterspace = 0.8f;
		this.m_LV2_bumperInterspace = 1.3f;
		this.m_LV2_wispSpeedMin = 0.3f;
		this.m_LV2_wispSpeedMax = 0.7f;
		this.m_LV2_wispSwingMin = 0.3f;
		this.m_LV2_wispSwingMax = 0.5f;
		this.m_LV2_wispAddXMin = -3f;
		this.m_LV2_wispAddXMax = -1f;
		this.m_LV3_distance = 1000;
		this.m_LV3_tblId = 0;
		this.m_LV3_missileSpeed = 6f;
		this.m_LV3_missileInterspace = 1f;
		this.m_LV3_missilePos1 = 40;
		this.m_LV3_missilePos2 = 40;
		this.m_LV3_missileWaitTime = 2f;
		this.m_LV3_missileCount = 5;
		this.m_LV3_boundParamMin = 0f;
		this.m_LV3_boundParamMax = 1.5f;
		this.m_LV3_boundMaxRand = 50;
		this.m_LV3_bumperRand = 30;
		this.m_LV3_ballSpeed = 8f;
		this.m_LV3_wispInterspace = 0.8f;
		this.m_LV3_bumperInterspace = 1.3f;
		this.m_LV3_wispSpeedMin = 0.3f;
		this.m_LV3_wispSpeedMax = 0.7f;
		this.m_LV3_wispSwingMin = 0.3f;
		this.m_LV3_wispSwingMax = 0.5f;
		this.m_LV3_wispAddXMin = -3f;
		this.m_LV3_wispAddXMax = -1f;
		this.m_LV4_distance = 1300;
		this.m_LV4_tblId = 0;
		this.m_LV4_missileSpeed = 6f;
		this.m_LV4_missileInterspace = 1f;
		this.m_LV4_missilePos1 = 40;
		this.m_LV4_missilePos2 = 40;
		this.m_LV4_missileWaitTime = 2f;
		this.m_LV4_missileCount = 5;
		this.m_LV4_boundParamMin = 0f;
		this.m_LV4_boundParamMax = 1.5f;
		this.m_LV4_boundMaxRand = 50;
		this.m_LV4_bumperRand = 30;
		this.m_LV4_ballSpeed = 8f;
		this.m_LV4_wispInterspace = 0.8f;
		this.m_LV4_bumperInterspace = 1.3f;
		this.m_LV4_wispSpeedMin = 0.3f;
		this.m_LV4_wispSpeedMax = 0.7f;
		this.m_LV4_wispSwingMin = 0.3f;
		this.m_LV4_wispSwingMax = 0.5f;
		this.m_LV4_wispAddXMin = -3f;
		this.m_LV4_wispAddXMax = -1f;
		this.m_LV5_distance = 1500;
		this.m_LV5_tblId = 0;
		this.m_LV5_missileSpeed = 6f;
		this.m_LV5_missileInterspace = 1f;
		this.m_LV5_missilePos1 = 40;
		this.m_LV5_missilePos2 = 40;
		this.m_LV5_missileWaitTime = 2f;
		this.m_LV5_missileCount = 5;
		this.m_LV5_boundParamMin = 0f;
		this.m_LV5_boundParamMax = 1.5f;
		this.m_LV5_boundMaxRand = 50;
		this.m_LV5_bumperRand = 30;
		this.m_LV5_ballSpeed = 8f;
		this.m_LV5_wispInterspace = 0.8f;
		this.m_LV5_bumperInterspace = 1.3f;
		this.m_LV5_wispSpeedMin = 0.3f;
		this.m_LV5_wispSpeedMax = 0.7f;
		this.m_LV5_wispSwingMin = 0.3f;
		this.m_LV5_wispSwingMax = 0.5f;
		this.m_LV5_wispAddXMin = -3f;
		this.m_LV5_wispAddXMax = -1f;
	}
}
