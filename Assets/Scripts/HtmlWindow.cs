using System;
using UnityEngine;

public class HtmlWindow : MonoBehaviour
{
	private enum EventSignal
	{
		SIG_PLAYSTART = 100
	}

	private GameObject m_parserObject;

	private bool m_isSetup;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_isSetup)
		{
			HtmlParser component = this.m_parserObject.GetComponent<HtmlParser>();
			if (component == null)
			{
				return;
			}
			if (component.IsEndParse)
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = "利用規約",
					message = component.ParsedString
				});
				this.m_isSetup = false;
			}
		}
	}

	private void PlayStartSync()
	{
		string urlString = HtmlParserFactory.GetUrlString("http://puyopuyoquest.sega-net.com/rule/index.html");
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = "利用規約",
			message = urlString
		});
		this.m_isSetup = false;
	}

	private void PlayStartASync()
	{
		this.m_parserObject = HtmlParserFactory.Create("http://puyopuyoquest.sega-net.com/rule/index.html", HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
		this.m_isSetup = true;
	}
}
