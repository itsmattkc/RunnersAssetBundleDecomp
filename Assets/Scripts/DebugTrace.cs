using System;

public class DebugTrace
{
	private string m_text;

	public string text
	{
		get
		{
			return this.m_text;
		}
		private set
		{
		}
	}

	public DebugTrace(string trace)
	{
		this.m_text = trace;
	}
}
