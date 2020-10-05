using System;

public class FSMStateFactory<Context>
{
	public int stateID;

	public FSMState<Context> state;

	public FSMStateFactory(int id, FSMState<Context> st)
	{
		this.stateID = id;
		this.state = st;
	}
}
