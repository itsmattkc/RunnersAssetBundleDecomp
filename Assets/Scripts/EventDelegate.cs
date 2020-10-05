using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventDelegate
{
	public delegate void Callback();

	[SerializeField]
	private MonoBehaviour mTarget;

	[SerializeField]
	private string mMethodName;

	public bool oneShot;

	private EventDelegate.Callback mCachedCallback;

	private static int s_Hash = "EventDelegate".GetHashCode();

	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
		}
	}

	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
		}
	}

	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName);
		}
	}

	public bool isEnabled
	{
		get
		{
			return this.mTarget != null && this.mTarget.enabled;
		}
	}

	public EventDelegate()
	{
	}

	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			return this.mTarget == callback.Target && string.Equals(this.mMethodName, callback.Method.Name);
		}
		if (obj is EventDelegate)
		{
			EventDelegate eventDelegate = obj as EventDelegate;
			return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	private EventDelegate.Callback Get()
	{
		if (this.mCachedCallback == null || this.mCachedCallback.Target != this.mTarget || this.mCachedCallback.Method.Name != this.mMethodName)
		{
			if (!(this.mTarget != null) || string.IsNullOrEmpty(this.mMethodName))
			{
				return null;
			}
			this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
		}
		return this.mCachedCallback;
	}

	private void Set(EventDelegate.Callback call)
	{
		if (call == null || call.Method == null)
		{
			this.mTarget = null;
			this.mMethodName = null;
			this.mCachedCallback = null;
		}
		else
		{
			this.mTarget = (call.Target as MonoBehaviour);
			this.mMethodName = call.Method.Name;
		}
	}

	public void Set(MonoBehaviour target, string methodName)
	{
		this.mTarget = target;
		this.mMethodName = methodName;
		this.mCachedCallback = null;
	}

	public bool Execute()
	{
		EventDelegate.Callback callback = this.Get();
		if (callback != null)
		{
			callback();
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.methodName))
		{
			string text = this.mTarget.GetType().ToString();
			int num = text.LastIndexOf('.');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			return text + "." + this.methodName;
		}
		return null;
	}

	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					eventDelegate.Execute();
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	public static void Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(new EventDelegate(callback));
		}
	}

	public static void Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		EventDelegate.Add(list, callback, false);
	}

	public static void Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return;
				}
				i++;
			}
			list.Add(new EventDelegate(callback)
			{
				oneShot = oneShot
			});
		}
		else
		{
			global::Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, false);
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			list.Add(new EventDelegate(ev.target, ev.methodName)
			{
				oneShot = oneShot
			});
		}
		else
		{
			global::Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}
}
