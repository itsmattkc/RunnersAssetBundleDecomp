using System;

public class ObjEnemySwing : ObjEnemyBase
{
	protected override void OnSpawned()
	{
		base.OnSpawned();
	}

	public void SetObjEnemySwingParameter(ObjEnemySwingParameter param)
	{
		if (param != null)
		{
			base.SetupEnemy((uint)param.tblID, 0f);
			MotorSwing component = base.GetComponent<MotorSwing>();
			if (component)
			{
				component.SetParam(param.moveSpeed, param.moveDistanceX, param.moveDistanceY, base.transform.forward);
			}
		}
	}
}
