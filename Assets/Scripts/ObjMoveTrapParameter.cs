using System;

[Serializable]
public class ObjMoveTrapParameter : SpawnableParameter
{
	public float moveSpeed;

	public float moveDistance;

	public float startMoveDistance;

	public ObjMoveTrapParameter() : base("ObjMoveTrap")
	{
		this.moveSpeed = 3f;
		this.moveDistance = 20f;
		this.startMoveDistance = 20f;
	}
}
