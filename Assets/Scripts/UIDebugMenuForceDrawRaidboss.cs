using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class UIDebugMenuForceDrawRaidboss : UIDebugMenuTask
{
	private enum TextType
	{
		EVENTID,
		SCORE,
		NUM
	}

	private delegate void NetworkRequestSuccessCallback();

	private delegate void NetworkRequestFailedCallback(ServerInterface.StatusCode statusCode);

	private sealed class _NetworkRequest_c__Iterator10 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetBase request;

		internal UIDebugMenuForceDrawRaidboss.NetworkRequestSuccessCallback successCallback;

		internal UIDebugMenuForceDrawRaidboss.NetworkRequestFailedCallback failedCallback;

		internal int _PC;

		internal object _current;

		internal NetBase ___request;

		internal UIDebugMenuForceDrawRaidboss.NetworkRequestSuccessCallback ___successCallback;

		internal UIDebugMenuForceDrawRaidboss.NetworkRequestFailedCallback ___failedCallback;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this.request.Request();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this.request.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.request.IsSucceeded())
			{
				if (this.successCallback != null)
				{
					this.successCallback();
				}
			}
			else if (this.failedCallback != null)
			{
				this.failedCallback(this.request.resultStCd);
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[2];

	private UIDebugMenuTextBox m_textBox;

	private string[] DefaultTextList = new string[]
	{
		"イベントID",
		"スコア"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 50f),
		new Rect(200f, 300f, 250f, 50f)
	};

	private NetBase m_networkRequest;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(200f, 450f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 2; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i]);
		}
		this.m_TextFields[0].text = UIDebugMenuServerDefine.DefaultRaidbossId;
		this.m_textBox = base.gameObject.AddComponent<UIDebugMenuTextBox>();
		this.m_textBox.Setup(new Rect(500f, 100f, 400f, 500f), string.Empty);
	}

	protected override void OnTransitionTo()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(false);
		}
		for (int i = 0; i < 2; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(false);
			}
		}
		if (this.m_textBox != null)
		{
			this.m_textBox.SetActive(false);
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(true);
		}
		for (int i = 0; i < 2; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(true);
			}
		}
		if (this.m_textBox != null)
		{
			this.m_textBox.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name == "Decide")
		{
			for (int i = 0; i < 2; i++)
			{
				UIDebugMenuTextField uIDebugMenuTextField = this.m_TextFields[i];
				if (!(uIDebugMenuTextField == null))
				{
					int num;
					if (!int.TryParse(uIDebugMenuTextField.text, out num))
					{
						return;
					}
				}
			}
			this.m_networkRequest = new NetDebugForceDrawRaidboss(int.Parse(this.m_TextFields[0].text), long.Parse(this.m_TextFields[1].text));
			base.StartCoroutine(this.NetworkRequest(this.m_networkRequest, new UIDebugMenuForceDrawRaidboss.NetworkRequestSuccessCallback(this.ForceDrawRaidbossEndCallback), new UIDebugMenuForceDrawRaidboss.NetworkRequestFailedCallback(this.NetworkFailedCallback)));
		}
	}

	private IEnumerator NetworkRequest(NetBase request, UIDebugMenuForceDrawRaidboss.NetworkRequestSuccessCallback successCallback, UIDebugMenuForceDrawRaidboss.NetworkRequestFailedCallback failedCallback)
	{
		UIDebugMenuForceDrawRaidboss._NetworkRequest_c__Iterator10 _NetworkRequest_c__Iterator = new UIDebugMenuForceDrawRaidboss._NetworkRequest_c__Iterator10();
		_NetworkRequest_c__Iterator.request = request;
		_NetworkRequest_c__Iterator.successCallback = successCallback;
		_NetworkRequest_c__Iterator.failedCallback = failedCallback;
		_NetworkRequest_c__Iterator.___request = request;
		_NetworkRequest_c__Iterator.___successCallback = successCallback;
		_NetworkRequest_c__Iterator.___failedCallback = failedCallback;
		return _NetworkRequest_c__Iterator;
	}

	private void ForceDrawRaidbossEndCallback()
	{
		this.m_networkRequest = new NetServerGetEventUserRaidBossList(int.Parse(this.m_TextFields[0].text));
		base.StartCoroutine(this.NetworkRequest(this.m_networkRequest, new UIDebugMenuForceDrawRaidboss.NetworkRequestSuccessCallback(this.GetEventUserRaidBossListEndCallback), new UIDebugMenuForceDrawRaidboss.NetworkRequestFailedCallback(this.NetworkFailedCallback)));
	}

	private void GetEventUserRaidBossListEndCallback()
	{
		NetServerGetEventUserRaidBossList netServerGetEventUserRaidBossList = this.m_networkRequest as NetServerGetEventUserRaidBossList;
		if (netServerGetEventUserRaidBossList == null)
		{
			return;
		}
		List<ServerEventRaidBossState> userRaidBossList = netServerGetEventUserRaidBossList.UserRaidBossList;
		if (userRaidBossList == null)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ServerEventRaidBossState current in userRaidBossList)
		{
			if (current != null)
			{
				stringBuilder.Append("BossId: " + current.Id.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  Level: " + current.Level.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  Rarity: " + current.Rarity.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  HitPoint: " + current.HitPoint.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  MaxHitPoint: " + current.MaxHitPoint.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  Status: " + current.GetStatusType().ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  EscapeAt: " + current.EscapeAt.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  EncounterName: " + current.EncounterName);
				stringBuilder.AppendLine();
				stringBuilder.Append("  Encounter: " + current.Encounter.ToString());
				stringBuilder.AppendLine();
				stringBuilder.Append("  Crowded: " + current.Crowded.ToString());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
		}
		this.m_textBox.text = stringBuilder.ToString();
	}

	private void NetworkFailedCallback(ServerInterface.StatusCode statusCode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Error!!!!!!!!!");
		stringBuilder.AppendLine();
		stringBuilder.Append("StatusCode = " + statusCode.ToString());
		stringBuilder.AppendLine();
		this.m_textBox.text = stringBuilder.ToString();
	}
}
