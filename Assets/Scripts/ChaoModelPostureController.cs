using System;
using UnityEngine;

public class ChaoModelPostureController : MonoBehaviour
{
	private GameObject m_modelObject;

	private TinyFsmBehavior m_fsmBehavior;

	private Vector3 m_velocity = Vector3.zero;

	private Quaternion m_initRotaion = Quaternion.identity;

	private Quaternion LocalRotaion
	{
		get
		{
			if (this.m_modelObject != null)
			{
				return this.m_modelObject.transform.localRotation;
			}
			return Quaternion.identity;
		}
		set
		{
			if (this.m_modelObject != null)
			{
				this.m_modelObject.transform.localRotation = value;
			}
		}
	}

	private void Start()
	{
		this.CreateTinyFsm();
	}

	private void Update()
	{
	}

	private void CreateTinyFsm()
	{
		this.m_fsmBehavior = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsmBehavior != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
			this.m_fsmBehavior.SetUp(description);
		}
	}

	private void OnDestroy()
	{
		if (this.m_fsmBehavior != null)
		{
			this.m_fsmBehavior.ShutDown();
			this.m_fsmBehavior = null;
		}
	}

	public void SetModelObject(GameObject modelObject)
	{
		this.m_modelObject = modelObject;
		this.m_initRotaion = this.LocalRotaion;
	}

	public void ChangeStateToSpin(Vector3 velocity)
	{
		this.m_velocity = velocity;
		this.ChangeState(new TinyFsmState(new EventFunction(this.StateSpin)));
	}

	public void ChangeStateToReturnIdle()
	{
		this.ChangeState(new TinyFsmState(new EventFunction(this.StateReturnToIdle)));
	}

	private void ChangeState(TinyFsmState nextState)
	{
		if (this.m_fsmBehavior != null)
		{
			this.m_fsmBehavior.ChangeState(nextState);
		}
	}

	private void AddRotation(Quaternion rot)
	{
		this.LocalRotaion *= rot;
	}

	private TinyFsmState StateIdle(TinyFsmEvent fsmEvent)
	{
		int signal = fsmEvent.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSpin(TinyFsmEvent fsmEvent)
	{
		int signal = fsmEvent.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
		{
			float getDeltaTime = fsmEvent.GetDeltaTime;
			Quaternion rot = Quaternion.AngleAxis(this.m_velocity.y * getDeltaTime, Vector3.up) * Quaternion.AngleAxis(this.m_velocity.x * getDeltaTime, Vector3.right) * Quaternion.AngleAxis(this.m_velocity.z * getDeltaTime, Vector3.forward);
			this.AddRotation(rot);
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateReturnToIdle(TinyFsmEvent fsmEvent)
	{
		int signal = fsmEvent.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_velocity.sqrMagnitude < 1.401298E-45f)
			{
				this.m_velocity = Vector3.one;
			}
			return TinyFsmState.End();
		case 4:
		{
			float getDeltaTime = fsmEvent.GetDeltaTime;
			Quaternion quaternion = this.LocalRotaion;
			quaternion = Quaternion.RotateTowards(quaternion, this.m_initRotaion, this.m_velocity.magnitude * getDeltaTime);
			this.LocalRotaion = quaternion;
			if (quaternion == this.m_initRotaion)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			}
			return TinyFsmState.End();
		}
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}
}
