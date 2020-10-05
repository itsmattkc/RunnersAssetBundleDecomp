using System;

[Serializable]
public class ObjAirTrapParameter : SpawnableParameter
{
	public float moveSpeed;

	public float moveDistanceX;

	public float moveDistanceY;

	public ObjAirTrapParameter() : base("ObjAirTrap")
	{
		this.moveSpeed = 0f;
		this.moveDistanceX = 0f;
		this.moveDistanceY = 0f;
	}
}
