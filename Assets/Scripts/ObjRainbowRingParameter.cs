using System;

[Serializable]
public class ObjRainbowRingParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public ObjRainbowRingParameter() : base("ObjRainbowRing")
	{
		this.firstSpeed = 20f;
		this.outOfcontrol = 0.5f;
	}
}
