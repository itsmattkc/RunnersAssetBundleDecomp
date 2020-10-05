using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/TinyFsm")]
public class TinyFsm
{
	private TinyFsmState m_cur;

	private TinyFsmState m_src;

	private bool m_hierarchical;

	public TinyFsm()
	{
		this.m_src = TinyFsmState.Top();
		this.m_hierarchical = false;
	}

	public void Initialize(MonoBehaviour context, TinyFsmState state, bool hierarchical)
	{
		this.m_hierarchical = hierarchical;
		if (this.m_hierarchical)
		{
			List<TinyFsmState> list = new List<TinyFsmState>();
			TinyFsmState tinyFsmState = state;
			while (!tinyFsmState.IsTop())
			{
				list.Add(tinyFsmState);
				tinyFsmState = TinyFsm.Super(context, tinyFsmState);
			}
			list.Reverse();
			foreach (TinyFsmState current in list)
			{
				TinyFsm.Enter(current);
			}
			this.m_cur = state;
		}
		else
		{
			TinyFsm.Enter(state);
			this.m_cur = state;
		}
	}

	public void Shutdown(MonoBehaviour context)
	{
		if (this.m_hierarchical)
		{
			TinyFsmState tinyFsmState = this.m_cur;
			while (!tinyFsmState.IsTop())
			{
				TinyFsm.Leave(tinyFsmState);
				tinyFsmState = TinyFsm.Super(context, tinyFsmState);
			}
			this.m_cur.Clear();
			this.m_src.Clear();
		}
		else
		{
			TinyFsm.Leave(this.m_cur);
			this.m_cur.Clear();
		}
	}

	public void Dispatch(MonoBehaviour context, TinyFsmEvent e)
	{
		if (this.m_hierarchical)
		{
			this.m_src = this.m_cur;
			while (!this.m_src.IsTop() && !this.m_src.IsEnd())
			{
				TinyFsmState tinyFsmState = TinyFsm.Trigger(context, this.m_src, e);
				if (tinyFsmState.IsEnd() || !tinyFsmState.IsValid())
				{
					return;
				}
				this.m_src = TinyFsm.Super(context, this.m_src);
			}
		}
		else
		{
			this.m_cur.Call(e);
		}
	}

	public bool ChangeState(MonoBehaviour context, TinyFsmState state)
	{
		if (this.m_cur == state)
		{
			return false;
		}
		if (this.m_hierarchical)
		{
			for (TinyFsmState tinyFsmState = this.m_cur; tinyFsmState != this.m_src; tinyFsmState = TinyFsm.Super(context, tinyFsmState))
			{
				TinyFsm.Leave(tinyFsmState);
			}
			if (this.m_src == state)
			{
				TinyFsm.Leave(this.m_src);
				TinyFsm.Enter(state);
			}
			else
			{
				List<TinyFsmState> list = new List<TinyFsmState>();
				TinyFsmState tinyFsmState2 = this.m_src;
				while (!tinyFsmState2.IsTop())
				{
					list.Add(tinyFsmState2);
					tinyFsmState2 = TinyFsm.Super(context, tinyFsmState2);
				}
				List<TinyFsmState> list2 = new List<TinyFsmState>();
				TinyFsmState tinyFsmState3 = state;
				while (!tinyFsmState3.IsTop())
				{
					list2.Add(tinyFsmState3);
					tinyFsmState3 = TinyFsm.Super(context, tinyFsmState3);
				}
				list.Reverse();
				list2.Reverse();
				IEnumerator<TinyFsmState> enumerator = list.GetEnumerator();
				IEnumerator<TinyFsmState> enumerator2 = list2.GetEnumerator();
				while (enumerator.Current != list[0] && enumerator2.Current != list2[0] && enumerator.Current == enumerator2.Current)
				{
					enumerator.MoveNext();
					enumerator2.MoveNext();
				}
				list.Reverse();
				foreach (TinyFsmState current in list)
				{
					TinyFsm.Leave(current);
					if (current == enumerator.Current)
					{
						break;
					}
				}
				while (enumerator2.Current != list2[0])
				{
					TinyFsm.Enter(enumerator2.Current);
					enumerator2.MoveNext();
				}
				this.m_cur = state;
			}
		}
		else
		{
			TinyFsm.Leave(this.m_cur);
			TinyFsm.Enter(state);
			this.m_cur = state;
		}
		return true;
	}

	public TinyFsmState GetCurrentState()
	{
		return this.m_cur;
	}

	private static TinyFsmState Trigger(MonoBehaviour context, TinyFsmState state, TinyFsmEvent e)
	{
		return state.Call(e);
	}

	private static TinyFsmState Super(MonoBehaviour context, TinyFsmState state)
	{
		return TinyFsm.Trigger(context, state, TinyFsmEvent.CreateSuper());
	}

	private static void Init(TinyFsmState state)
	{
		state.Call(TinyFsmEvent.CreateInit());
	}

	private static void Enter(TinyFsmState state)
	{
		state.Call(TinyFsmEvent.CreateEnter());
	}

	private static void Leave(TinyFsmState state)
	{
		state.Call(TinyFsmEvent.CreateLeave());
	}
}
