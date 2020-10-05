using System;

public class SocialResult
{
	private bool m_isError;

	private int m_resultId;

	private string m_result;

	public bool IsError
	{
		get
		{
			return this.m_isError;
		}
		set
		{
			this.m_isError = value;
		}
	}

	public int ResultId
	{
		get
		{
			return this.m_resultId;
		}
		set
		{
			this.m_resultId = value;
		}
	}

	public string Result
	{
		get
		{
			return this.m_result;
		}
		set
		{
			this.m_result = value;
		}
	}
}
