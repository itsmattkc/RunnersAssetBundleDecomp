using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("CRIWARE/CRI Atom Listener")]
public class CriAtomListener : MonoBehaviour
{
	private Vector3 lastPosition;

	private static CriAtomListener _instance_k__BackingField;

	private CriAtomEx3dListener _internalListener_k__BackingField;

	public static CriAtomListener instance
	{
		get;
		private set;
	}

	public CriAtomEx3dListener internalListener
	{
		get;
		private set;
	}

	private void OnEnable()
	{
		if (CriAtomListener.instance != null)
		{
			UnityEngine.Debug.LogError("Multiple listener instances.");
		}
		CriAtomListener.instance = this;
		CriAtomPlugin.InitializeLibrary();
		this.internalListener = new CriAtomEx3dListener();
		this.lastPosition = base.transform.position;
	}

	private void OnDisable()
	{
		if (this.internalListener != null)
		{
			this.internalListener.Dispose();
			this.internalListener = null;
			CriAtomPlugin.FinalizeLibrary();
		}
		CriAtomListener.instance = null;
	}

	private void LateUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = (position - this.lastPosition) / Time.deltaTime;
		Vector3 forward = base.transform.forward;
		Vector3 up = base.transform.up;
		this.lastPosition = position;
		this.internalListener.SetPosition(position.x, position.y, position.z);
		this.internalListener.SetVelocity(vector.x, vector.y, vector.z);
		this.internalListener.SetOrientation(forward.x, forward.y, forward.z, up.x, up.y, up.z);
		this.internalListener.Update();
	}
}
