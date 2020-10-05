using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetFriendUserIdList
{
	private sealed class _Process_c__Iterator7A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal List<string> friendFBIdList;

		internal NetServerGetFriendUserIdList _net___1;

		internal GameObject callbackObject;

		internal List<ServerUserTransformData> _transformDataList___2;

		internal List<ServerUserTransformData>.Enumerator __s_772___3;

		internal ServerUserTransformData _data___4;

		internal MsgGetFriendUserIdListSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal List<string> ___friendFBIdList;

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
				this._monitor___0 = NetMonitor.Instance;
				if (!(this._monitor___0 != null))
				{
					goto IL_257;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D4;
			default:
				return false;
			}
			if (!this._monitor___0.IsEndPrepare())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (!this._monitor___0.IsSuccessPrepare())
			{
				goto IL_257;
			}
			this._net___1 = new NetServerGetFriendUserIdList(this.friendFBIdList);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetFriendUserIdListRetry(this.friendFBIdList, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._transformDataList___2 = ServerInterface.UserTransformDataList;
				this._transformDataList___2.Clear();
				if (this._net___1.resultTransformDataList != null)
				{
					this.__s_772___3 = this._net___1.resultTransformDataList.GetEnumerator();
					try
					{
						while (this.__s_772___3.MoveNext())
						{
							this._data___4 = this.__s_772___3.Current;
							this._transformDataList___2.Add(this._data___4);
						}
					}
					finally
					{
						((IDisposable)this.__s_772___3).Dispose();
					}
				}
				this._msg___5 = new MsgGetFriendUserIdListSucceed();
				this._msg___5.m_transformDataList = this._net___1.resultTransformDataList;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetFriendUserIdList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetFriendUserIdList_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetFriendUserIdList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_257:
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

	public static IEnumerator Process(List<string> friendFBIdList, GameObject callbackObject)
	{
		ServerGetFriendUserIdList._Process_c__Iterator7A _Process_c__Iterator7A = new ServerGetFriendUserIdList._Process_c__Iterator7A();
		_Process_c__Iterator7A.friendFBIdList = friendFBIdList;
		_Process_c__Iterator7A.callbackObject = callbackObject;
		_Process_c__Iterator7A.___friendFBIdList = friendFBIdList;
		_Process_c__Iterator7A.___callbackObject = callbackObject;
		return _Process_c__Iterator7A;
	}
}
