using System;

[Serializable]
public class ObjCannonParameter : SpawnableParameter
{
	public float firstSpeed;

	public float outOfcontrol;

	public float moveSpeed;

	public float moveArea;

	public ObjCannonParameter() : base("ObjCannon")
	{
		this.firstSpeed = 10f;
		this.outOfcontrol = 0.5f;
		this.moveSpeed = 0.4f;
		this.moveArea = 50f;
	}
}
