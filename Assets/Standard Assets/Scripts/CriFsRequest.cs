using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CriFsRequest : IDisposable
{
	public delegate void DoneDelegate(CriFsRequest request);

	private sealed class _CheckDone_c__Iterator0 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal CriFsRequest __f__this;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.__f__this.isDone)
			{
				this._current = null;
				this._PC = 1;
				return true;
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

	private CriFsRequest.DoneDelegate _doneDelegate_k__BackingField;

	private bool _isDone_k__BackingField;

	private string _error_k__BackingField;

	private bool _isDisposed_k__BackingField;

	public CriFsRequest.DoneDelegate doneDelegate
	{
		get;
		protected set;
	}

	public bool isDone
	{
		get;
		private set;
	}

	public string error
	{
		get;
		protected set;
	}

	public bool isDisposed
	{
		get;
		protected set;
	}

	public virtual void Stop()
	{
	}

	public YieldInstruction WaitForDone(MonoBehaviour mb)
	{
		return mb.StartCoroutine(this.CheckDone());
	}

	public virtual void Update()
	{
	}

	protected void Done()
	{
		this.isDone = true;
		if (this.doneDelegate != null)
		{
			this.doneDelegate(this);
		}
	}

	private IEnumerator CheckDone()
	{
		CriFsRequest._CheckDone_c__Iterator0 _CheckDone_c__Iterator = new CriFsRequest._CheckDone_c__Iterator0();
		_CheckDone_c__Iterator.__f__this = this;
		return _CheckDone_c__Iterator;
	}

	public virtual void Dispose()
	{
	}

	~CriFsRequest()
	{
		this.Dispose();
	}
}
