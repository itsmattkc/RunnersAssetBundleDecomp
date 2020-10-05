using System;

[Serializable]
public class ObjRouletteCannonParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public float moveSpeed;

	public float angle;

	public ObjRouletteCannonParameter() : base("ObjRouletteCannon")
	{
		this.firstSpeed = 10f;
		this.outOfcontrol = 0.5f;
		this.moveSpeed = 1f;
		this.angle = 60f;
	}
}
