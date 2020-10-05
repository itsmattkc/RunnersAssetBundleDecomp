using System;
using UnityEngine;

public class HtmlLoaderSync : HtmlLoader
{
	protected override void OnSetup()
	{
		WWW wWW;
		do
		{
			wWW = base.GetWWW();
		}
		while (!wWW.isDone);
		base.IsEndLoad = true;
	}
}
