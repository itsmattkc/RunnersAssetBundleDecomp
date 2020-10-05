using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Enemy/ObjEnmBeetonMove")]
public class ObjEnmBeetonMove : ObjEnemyConstant
{
	protected override ObjEnemyUtil.EnemyType GetOriginalType()
	{
		return ObjEnemyUtil.EnemyType.NORMAL;
	}

	protected override string[] GetModelFiles()
	{
		return ObjEnmBeetonData.GetModelFiles();
	}

	protected override ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnmBeetonData.GetEffectSize();
	}
}
