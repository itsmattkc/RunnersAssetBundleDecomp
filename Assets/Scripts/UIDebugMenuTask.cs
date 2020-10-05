using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuTask : MonoBehaviour
{
	private UIDebugMenuTask m_parent;

	private Dictionary<string, UIDebugMenuTask> m_childList;

	private bool m_isActive;

	private void Start()
	{
		this.m_childList = new Dictionary<string, UIDebugMenuTask>();
		this.m_isActive = false;
		this.OnStartFromTask();
	}

	private void OnGUI()
	{
		if (this.IsActive())
		{
			this.OnGuiFromTask();
		}
	}

	public bool IsActive()
	{
		return this.m_isActive;
	}

	public void AddChild(string childName, GameObject child)
	{
		if (this.m_childList == null)
		{
			return;
		}
		if (child == null)
		{
			return;
		}
		UIDebugMenuTask component = child.GetComponent<UIDebugMenuTask>();
		this.AddChild(childName, component);
	}

	public void AddChild(string childName, UIDebugMenuTask child)
	{
		if (this.m_childList == null)
		{
			return;
		}
		if (child == null)
		{
			return;
		}
		child.SetParent(this);
		this.m_childList.Add(childName, child);
	}

	public void TransitionFrom()
	{
		global::Debug.Log(string.Format("Transition From:{0}", this.ToString()));
		this.m_isActive = true;
		this.OnTransitionFrom();
	}

	public void TransitionToParent()
	{
		if (this.m_parent != null)
		{
			this.m_parent.TransitionFrom();
			this.TransitionTo();
		}
	}

	public void TransitionToChild(string childName)
	{
		if (this.m_childList.ContainsKey(childName))
		{
			UIDebugMenuTask uIDebugMenuTask = this.m_childList[childName];
			uIDebugMenuTask.TransitionFrom();
			this.TransitionTo();
			this.m_isActive = false;
			global::Debug.Log(string.Format("Transition to ChildMenu:{0}", childName));
		}
	}

	protected virtual void OnStartFromTask()
	{
	}

	protected virtual void OnGuiFromTask()
	{
	}

	protected virtual void OnTransitionFrom()
	{
	}

	protected virtual void OnTransitionTo()
	{
	}

	private void SetParent(UIDebugMenuTask parent)
	{
		this.m_parent = parent;
	}

	private void TransitionTo()
	{
		this.m_isActive = false;
		global::Debug.Log(string.Format("Transition To:{0}", this.ToString()));
		this.OnTransitionTo();
	}
}
