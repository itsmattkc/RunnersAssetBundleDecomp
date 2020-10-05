using System;
using UnityEngine;

public interface ISysFontTexturable
{
	string Text
	{
		get;
		set;
	}

	string AppleFontName
	{
		get;
		set;
	}

	string AndroidFontName
	{
		get;
		set;
	}

	string FontName
	{
		get;
		set;
	}

	int FontSize
	{
		get;
		set;
	}

	bool IsBold
	{
		get;
		set;
	}

	bool IsItalic
	{
		get;
		set;
	}

	SysFont.Alignment Alignment
	{
		get;
		set;
	}

	bool IsMultiLine
	{
		get;
		set;
	}

	int MaxWidthPixels
	{
		get;
		set;
	}

	int MaxHeightPixels
	{
		get;
		set;
	}

	int WidthPixels
	{
		get;
	}

	int HeightPixels
	{
		get;
	}

	int TextWidthPixels
	{
		get;
	}

	int TextHeightPixels
	{
		get;
	}

	Texture2D Texture
	{
		get;
	}
}
