using System;
using UnityEngine;

public class ObjJumpBoardSpawner : SpawnableBehavior
{
	[SerializeField]
	private ObjJumpBoardParameter m_parameter;

	public override void SetParameters(SpawnableParameter srcParameter)
	{
		ObjJumpBoardParameter objJumpBoardParameter = srcParameter as ObjJumpBoardParameter;
		if (objJumpBoardParameter != null)
		{
			ObjJumpBoard component = base.GetComponent<ObjJumpBoard>();
			if (component)
			{
				component.SetObjJumpBoardParameter(objJumpBoardParameter);
			}
		}
	}

	public override SpawnableParameter GetParameter()
	{
		return this.m_parameter;
	}
}
