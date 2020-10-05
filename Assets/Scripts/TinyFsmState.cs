using System;

public class TinyFsmState
{
	private const int IDENTIFIER_INVALID = 0;

	private const int IDENTIFIER_VALID = 1;

	private const int IDENTIFIER_TOP = 2;

	private const int IDENTIFIER_END = 3;

	private uint m_identifier;

	private EventFunction m_delegate;

	public TinyFsmState(EventFunction delegator)
	{
		this.m_identifier = 1u;
		this.m_delegate = delegator;
	}

	public TinyFsmState(int identifier)
	{
		this.m_identifier = (uint)identifier;
	}

	public static TinyFsmState Top()
	{
		return new TinyFsmState(2);
	}

	public static TinyFsmState End()
	{
		return new TinyFsmState(3);
	}

	public bool IsValid()
	{
		return this.m_identifier != 0u;
	}

	public bool IsTop()
	{
		return this.m_identifier == 2u;
	}

	public bool IsEnd()
	{
		return this.m_identifier == 3u;
	}

	public void Clear()
	{
		this.m_delegate = null;
	}

	public TinyFsmState Call(TinyFsmEvent e)
	{
		if (this.m_delegate != null)
		{
			return this.m_delegate(e);
		}
		return null;
	}
}
