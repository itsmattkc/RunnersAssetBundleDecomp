using Message;
using System;
using UnityEngine;

public class TinyFsmEvent : TinyFsmSystemEvent
{
	private enum EventDataType
	{
		NON,
		DELTATIME,
		MESSAGE,
		OBJECT,
		INTEGER
	}

	private TinyFsmEvent.EventDataType m_type;

	private int m_integer;

	private float m_float;

	private MessageBase m_message;

	private UnityEngine.Object m_object;

	public float GetDeltaTime
	{
		get
		{
			return this.m_float;
		}
	}

	public MessageBase GetMessage
	{
		get
		{
			if (this.m_type == TinyFsmEvent.EventDataType.MESSAGE)
			{
				return this.m_message;
			}
			return null;
		}
	}

	public UnityEngine.Object GetObject
	{
		get
		{
			if (this.m_type == TinyFsmEvent.EventDataType.OBJECT)
			{
				return this.m_object;
			}
			return null;
		}
	}

	public int GetInt
	{
		get
		{
			if (this.m_type == TinyFsmEvent.EventDataType.INTEGER)
			{
				return this.m_integer;
			}
			return 0;
		}
	}

	public TinyFsmEvent(int sig) : base(sig)
	{
	}

	public TinyFsmEvent(int sig, float deltaTime) : base(sig)
	{
		this.m_type = TinyFsmEvent.EventDataType.DELTATIME;
		this.m_float = deltaTime;
	}

	public TinyFsmEvent(int sig, MessageBase msg) : base(sig)
	{
		this.m_type = TinyFsmEvent.EventDataType.MESSAGE;
		this.m_message = msg;
	}

	public TinyFsmEvent(int sig, UnityEngine.Object obj) : base(sig)
	{
		this.m_type = TinyFsmEvent.EventDataType.OBJECT;
		this.m_object = obj;
	}

	public TinyFsmEvent(int sig, int integer) : base(sig)
	{
		this.m_type = TinyFsmEvent.EventDataType.OBJECT;
		this.m_integer = integer;
	}

	public static TinyFsmEvent CreateSignal(int sig)
	{
		return new TinyFsmEvent(sig);
	}

	public static TinyFsmEvent CreateSuper()
	{
		return TinyFsmEvent.CreateSignal(-1);
	}

	public static TinyFsmEvent CreateInit()
	{
		return TinyFsmEvent.CreateSignal(-2);
	}

	public static TinyFsmEvent CreateEnter()
	{
		return TinyFsmEvent.CreateSignal(-3);
	}

	public static TinyFsmEvent CreateLeave()
	{
		return TinyFsmEvent.CreateSignal(-4);
	}

	public static TinyFsmEvent CreateUserEvent(int signal)
	{
		return TinyFsmEvent.CreateSignal(signal);
	}

	public static TinyFsmEvent CreateUpdate(float deltaTime)
	{
		return new TinyFsmEvent(0, deltaTime);
	}

	public static TinyFsmEvent CreateMessage(MessageBase msg)
	{
		return new TinyFsmEvent(1, msg);
	}

	public static TinyFsmEvent CreateUserEventObject(int signal, UnityEngine.Object obj)
	{
		return new TinyFsmEvent(signal, obj);
	}

	public static TinyFsmEvent CreateUserEventInt(int signal, int integer)
	{
		return new TinyFsmEvent(signal, integer);
	}
}
