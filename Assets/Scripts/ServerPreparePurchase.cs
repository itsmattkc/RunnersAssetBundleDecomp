using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerPreparePurchase
{
	private sealed class _Process_c__IteratorB4 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int itemId;

		internal NetServerPreparePurchase _net___0;

		internal GameObject callbackObject;

		internal MsgServerConnctFailed _msg___1;

		internal int _PC;

		internal object _current;

		internal int ___itemId;

		internal GameObject ___callbackObject;

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
				this._net___0 = new NetServerPreparePurchase(this.itemId);
				this._net___0.Request();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._net___0.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._net___0.IsSucceeded())
			{
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerPreparePurchase_Succeeded", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___1 = new MsgServerConnctFailed(this._net___0.resultStCd);
				this.callbackObject.SendMessage("ServerPreparePurchase_Failed", this._msg___1, SendMessageOptions.DontRequireReceiver);
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

	public static IEnumerator Process(int itemId, GameObject callbackObject)
	{
		ServerPreparePurchase._Process_c__IteratorB4 _Process_c__IteratorB = new ServerPreparePurchase._Process_c__IteratorB4();
		_Process_c__IteratorB.itemId = itemId;
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___itemId = itemId;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
