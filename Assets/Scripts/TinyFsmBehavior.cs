using App;
using App.Utility;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Components/TinyFSM")]
public class TinyFsmBehavior : MonoBehaviour
{
	public class Description
	{
		public MonoBehaviour controlBehavior;

		private TinyFsmState _initState_k__BackingField;

		private bool _hierarchical_k__BackingField;

		private bool _onFixedUpdate_k__BackingField;

		private bool _ignoreDeltaTime_k__BackingField;

		public TinyFsmState initState
		{
			get;
			set;
		}

		public bool hierarchical
		{
			get;
			set;
		}

		public bool onFixedUpdate
		{
			get;
			set;
		}

		public bool ignoreDeltaTime
		{
			get;
			set;
		}

		public Description(MonoBehaviour control)
		{
			this.controlBehavior = control;
			this.initState = TinyFsmState.Top();
			this.hierarchical = false;
			this.onFixedUpdate = false;
			this.ignoreDeltaTime = false;
		}
	}

	private enum StatusFlag
	{
		STATUS_END_SETUP,
		STATUS_ENABLE_UPDATE,
		STATUS_ON_CHANGE_STATE,
		STATUS_HIERARCHICAL,
		STATUS_FIXEDUPDATE,
		STATUS_IGNORE_DELTATIME,
		STATUS_SHUTDOWN
	}

	private TinyFsm m_fsm;

	private MonoBehaviour m_controlBehavior;

	private Bitset32 m_statusFlags;

	public bool NowShutDown
	{
		get
		{
			return this.m_statusFlags.Test(6);
		}
	}

	public void SetUp(TinyFsmBehavior.Description desc)
	{
		if (this.m_fsm == null)
		{
			this.m_fsm = new TinyFsm();
			this.m_controlBehavior = desc.controlBehavior;
			this.m_fsm.Initialize(this.m_controlBehavior, desc.initState, desc.hierarchical);
			this.m_statusFlags.Set(4, desc.onFixedUpdate);
			this.m_statusFlags.Set(5, desc.ignoreDeltaTime);
			this.m_statusFlags.Set(0, true);
			this.m_statusFlags.Set(1, true);
		}
	}

	public void ShutDown()
	{
		this.m_statusFlags.Set(6, true);
		if (this.m_controlBehavior != null && this.m_fsm != null)
		{
			this.m_fsm.Shutdown(this.m_controlBehavior);
		}
		this.m_fsm = null;
		this.m_controlBehavior = null;
	}

	private void Update()
	{
		if (App.Math.NearZero(Time.deltaTime, 1E-06f) && !this.m_statusFlags.Test(5))
		{
			return;
		}
		if (this.m_statusFlags.Test(4))
		{
			return;
		}
		this.UpdateImpl(Time.deltaTime);
	}

	private void FixedUpdate()
	{
		if (!this.m_statusFlags.Test(4))
		{
			return;
		}
		this.UpdateImpl(Time.deltaTime);
	}

	private void OnDestroy()
	{
		this.ShutDown();
	}

	public bool ChangeState(TinyFsmState state)
	{
		if (this.m_fsm == null || this.m_controlBehavior == null)
		{
			return false;
		}
		bool result = false;
		if (!this.m_statusFlags.Test(2))
		{
			this.m_statusFlags.Set(2);
			result = this.m_fsm.ChangeState(this.m_controlBehavior, state);
			this.m_statusFlags.Reset(2);
		}
		return result;
	}

	public TinyFsmState GetCurrentState()
	{
		if (this.m_fsm != null)
		{
			return this.m_fsm.GetCurrentState();
		}
		return TinyFsmState.Top();
	}

	private void SetEnableUpdate(bool isEnableUpdate)
	{
		this.m_statusFlags.Set(1, isEnableUpdate);
	}

	private void UpdateImpl(float deltaTime)
	{
		if (this.m_fsm == null || this.m_controlBehavior == null)
		{
			return;
		}
		if (this.m_statusFlags.Test(1) && this.m_statusFlags.Test(0))
		{
			this.m_fsm.Dispatch(this.m_controlBehavior, TinyFsmEvent.CreateUpdate(deltaTime));
		}
	}

	public void Dispatch(TinyFsmEvent signal)
	{
		if (this.m_fsm == null || this.m_controlBehavior == null)
		{
			return;
		}
		if (signal.Signal < 1)
		{
			global::Debug.Log("Cannot Dispatch Signal ID is not for User.\n");
		}
		this.m_fsm.Dispatch(this.m_controlBehavior, signal);
	}
}
