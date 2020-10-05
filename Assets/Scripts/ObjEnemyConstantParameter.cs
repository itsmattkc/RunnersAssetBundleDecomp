using System;

[Serializable]
public class ObjEnemyConstantParameter : SpawnableParameter
{
	public float moveSpeed;

	public float moveDistance;

	public float startMoveDistance;

	public int tblID;

	public ObjEnemyConstantParameter() : base("ObjEnemyConstant")
	{
		this.moveSpeed = 0f;
		this.moveDistance = 0f;
		this.startMoveDistance = 0f;
		this.tblID = 0;
	}
}
