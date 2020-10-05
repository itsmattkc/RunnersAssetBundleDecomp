using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	public int updateOrder;

	public float speed = 10f;

	public bool ignoreTimeScale;

	private Transform mTrans;

	private Quaternion mRelative;

	private Quaternion mAbsolute;

	private void Start()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
		if (this.ignoreTimeScale)
		{
			UpdateManager.AddCoroutine(this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
		}
		else
		{
			UpdateManager.AddLateUpdate(this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
		}
	}

	private void CoroutineUpdate(float delta)
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, delta * this.speed);
			this.mTrans.rotation = this.mAbsolute;
		}
	}
}
