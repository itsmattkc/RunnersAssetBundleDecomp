using System;
using UnityEngine;

public class ObjEnemyConstant : ObjEnemyBase
{
	public void SetObjEnemyConstantParameter(ObjEnemyConstantParameter param)
	{
		if (param != null)
		{
			base.SetupEnemy((uint)param.tblID, param.moveSpeed);
			MotorConstant component = base.GetComponent<MotorConstant>();
			if (component)
			{
				component.SetParam(param.moveSpeed, param.moveDistance, param.startMoveDistance, this.GetConstantAngle(), string.Empty, string.Empty);
			}
		}
	}

	protected virtual Vector3 GetConstantAngle()
	{
		return base.transform.forward;
	}
}
