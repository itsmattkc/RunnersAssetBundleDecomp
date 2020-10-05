using System;

[Serializable]
public class ObjDashRingParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public ObjDashRingParameter() : base("ObjDashRing")
	{
		this.firstSpeed = 8f;
		this.outOfcontrol = 0.5f;
	}
}
