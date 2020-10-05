using App.Utility;
using System;
using UnityEngine;

public abstract class WindowBase : MonoBehaviour
{
	public class BackButtonMessage
	{
		public enum Flags
		{
			STAY_SEQUENCE
		}

		private Bitset32 m_flags;

		public void StaySequence()
		{
			this.SetFlag(WindowBase.BackButtonMessage.Flags.STAY_SEQUENCE, true);
		}

		public bool IsFlag(WindowBase.BackButtonMessage.Flags flag)
		{
			return this.m_flags.Test((int)flag);
		}

		private void SetFlag(WindowBase.BackButtonMessage.Flags flag, bool value)
		{
			this.m_flags.Set((int)flag, value);
		}
	}

	public void Destroy()
	{
		this.RemoveBackKeyCallBack();
	}

	private void Awake()
	{
		this.EntryBackKeyCallBack();
	}

	public abstract void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg);

	public void EntryBackKeyCallBack()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	public void RemoveBackKeyCallBack()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}
}
