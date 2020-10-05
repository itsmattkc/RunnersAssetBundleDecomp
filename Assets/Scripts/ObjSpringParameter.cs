using System;

[Serializable]
public class ObjSpringParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public ObjSpringParameter() : base("ObjSpring")
	{
		this.firstSpeed = 2f;
		this.outOfcontrol = 0.5f;
	}
}
