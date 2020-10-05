using System;
using System.Collections.Generic;

public class FSMSystem<Context>
{
	private List<FSMStateFactory<Context>> m_states;

	private int m_currentStateID;

	private FSMState<Context> m_currentState;

	public StateID CurrentStateID
	{
		get
		{
			return (StateID)this.m_currentStateID;
		}
	}

	public FSMState<Context> CurrentState
	{
		get
		{
			return this.m_currentState;
		}
	}

	public FSMSystem()
	{
		this.m_states = new List<FSMStateFactory<Context>>();
	}

	public void AddState(int stateID, FSMState<Context> s)
	{
		FSMStateFactory<Context> stateFactory = new FSMStateFactory<Context>(stateID, s);
		this.AddState(stateFactory);
	}

	public void AddState(FSMStateFactory<Context> stateFactory)
	{
		if (this.CurrentState != null)
		{
			UnityEngine.Debug.LogError("FSM ERROR: Impossible to add state " + stateFactory.stateID.ToString() + ". State is already Initialized");
		}
		if (stateFactory.state == null)
		{
			UnityEngine.Debug.LogError("FSM ERROR: Null reference is not allowed");
		}
		if (this.GetStateFactory(stateFactory.stateID) != null)
		{
			UnityEngine.Debug.LogError("FSM ERROR: Impossible to add state " + stateFactory.stateID.ToString() + " because state has already been added");
			return;
		}
		this.m_states.Add(stateFactory);
	}

	public void Init(Context context, int id)
	{
		FSMStateFactory<Context> stateFactory = this.GetStateFactory(id);
		if (stateFactory != null)
		{
			this.m_currentState = stateFactory.state;
			this.m_currentStateID = stateFactory.stateID;
			this.m_currentState.Enter(context);
		}
		else
		{
			UnityEngine.Debug.LogError("FSM ERROR: Impossible to Init " + id.ToString() + ". State is not Found");
		}
	}

	private FSMStateFactory<Context> GetStateFactory(int id)
	{
		for (int i = 0; i < this.m_states.Count; i++)
		{
			if (this.m_states[i].stateID == id)
			{
				return this.m_states[i];
			}
		}
		return null;
	}

	private FSMState<Context> GetState(int id)
	{
		for (int i = 0; i < this.m_states.Count; i++)
		{
			if (this.m_states[i].stateID == id)
			{
				return this.m_states[i].state;
			}
		}
		return null;
	}

	public void ReplaceState(int stateID, FSMState<Context> s)
	{
		if (this.CurrentState != null)
		{
			UnityEngine.Debug.LogError("FSM ERROR: Impossible to replace state " + stateID.ToString() + ". State is already Initialized");
		}
		if (s == null)
		{
			UnityEngine.Debug.LogError("FSM ERROR: Null reference is not allowed");
		}
		FSMStateFactory<Context> stateFactory = this.GetStateFactory(stateID);
		if (stateFactory != null)
		{
			stateFactory.state = s;
		}
		else
		{
			FSMStateFactory<Context> item = new FSMStateFactory<Context>(stateID, s);
			this.m_states.Add(item);
		}
	}

	public void ChangeState(Context context, int stateID)
	{
		FSMStateFactory<Context> stateFactory = this.GetStateFactory(stateID);
		if (stateFactory != null)
		{
			this.m_currentState.Leave(context);
			this.m_currentState = stateFactory.state;
			this.m_currentStateID = stateFactory.stateID;
			this.m_currentState.Enter(context);
		}
	}
}
