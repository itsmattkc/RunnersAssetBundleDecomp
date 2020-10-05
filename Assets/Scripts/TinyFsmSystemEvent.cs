using System;

public class TinyFsmSystemEvent
{
	private int m_sig;

	public int Signal
	{
		get
		{
			return this.m_sig;
		}
	}

	protected TinyFsmSystemEvent(int sig)
	{
		this.m_sig = sig;
	}
}
