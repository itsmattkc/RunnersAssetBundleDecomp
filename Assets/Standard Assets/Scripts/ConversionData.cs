using System;
using UnityEngine;

[Serializable]
public class ConversionData
{
	[SerializeField]
	private string url;

	public string Url
	{
		get
		{
			return this.url;
		}
		set
		{
			this.url = value;
		}
	}

	public ConversionData(string url)
	{
		this.url = url;
	}
}
