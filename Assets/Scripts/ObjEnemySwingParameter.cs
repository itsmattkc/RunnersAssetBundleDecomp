using System;

[Serializable]
public class ObjEnemySwingParameter : SpawnableParameter
{
	public float moveSpeed;

	public float moveDistanceX;

	public float moveDistanceY;

	public int tblID;

	public ObjEnemySwingParameter() : base("ObjEnemySwing")
	{
		this.moveSpeed = 0f;
		this.moveDistanceX = 0f;
		this.moveDistanceY = 0f;
		this.tblID = 0;
	}
}
