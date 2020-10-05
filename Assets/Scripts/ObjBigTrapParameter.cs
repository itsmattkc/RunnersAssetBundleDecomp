using System;

[Serializable]
public class ObjBigTrapParameter : SpawnableParameter
{
	public float moveSpeedX;

	public float moveSpeedY;

	public float moveDistanceY;

	public float startMoveDistance;

	public ObjBigTrapParameter() : base("ObjBigTrap")
	{
		this.moveSpeedX = -1f;
		this.moveSpeedY = 0.5f;
		this.moveDistanceY = 1f;
		this.startMoveDistance = 20f;
	}
}
