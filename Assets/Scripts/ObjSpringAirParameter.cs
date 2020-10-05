using System;

[Serializable]
public class ObjSpringAirParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public ObjSpringAirParameter() : base("ObjSpringAir")
	{
		this.firstSpeed = 2f;
		this.outOfcontrol = 0.5f;
	}
}
