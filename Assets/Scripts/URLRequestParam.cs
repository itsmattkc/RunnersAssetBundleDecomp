using System;

public class URLRequestParam
{
	private string mPropertyName;

	private string mValue;

	public string propertyName
	{
		get
		{
			return this.mPropertyName;
		}
	}

	public string value
	{
		get
		{
			return this.mValue;
		}
	}

	public URLRequestParam(string propertyName, string value)
	{
		this.mPropertyName = propertyName;
		this.mValue = value;
	}
}
