using System;

public class Clipboard
{
	private static string m_oldText;

	public static string text
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(Clipboard.m_oldText))
				{
					Clipboard.m_oldText = value;
				}
				else
				{
					Clipboard.m_oldText = value;
				}
			}
			if (Binding.Instance != null)
			{
				Binding.Instance.SetClipBoard(value);
			}
		}
	}

	public static string oldText
	{
		get
		{
			return Clipboard.m_oldText;
		}
	}
}
