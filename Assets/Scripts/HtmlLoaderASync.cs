using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HtmlLoaderASync : HtmlLoader
{
	private sealed class _WaitLoadAsync_c__Iterator28 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW _www___0;

		internal int _PC;

		internal object _current;

		internal HtmlLoaderASync __f__this;

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
				this._www___0 = this.__f__this.GetWWW();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this._www___0.isDone)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__f__this.IsEndLoad = true;
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

	protected override void OnSetup()
	{
	}

	private void Start()
	{
		base.StartCoroutine(this.WaitLoadAsync());
	}

	private IEnumerator WaitLoadAsync()
	{
		HtmlLoaderASync._WaitLoadAsync_c__Iterator28 _WaitLoadAsync_c__Iterator = new HtmlLoaderASync._WaitLoadAsync_c__Iterator28();
		_WaitLoadAsync_c__Iterator.__f__this = this;
		return _WaitLoadAsync_c__Iterator;
	}
}
