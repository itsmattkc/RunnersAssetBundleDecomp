using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerUpdateMessage
{
	private sealed class _Process_c__IteratorA0 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal List<int> messageIdList;

		internal List<int> operatorMessageIdList;

		internal NetServerUpdateMessage _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _characterState___4;

		internal List<ServerChaoState> _chaoState___5;

		internal int _chaoRoulette___6;

		internal int _itemRoulette___7;

		internal int _presentCount___8;

		internal List<ServerPresentState> _resultPresentList___9;

		internal int _i___10;

		internal ServerPresentState _presentState___11;

		internal int _missingMessageCount___12;

		internal List<int> _resultMissingMessageIdList___13;

		internal int _i___14;

		internal int _missingId___15;

		internal int _missingOperatorMessageCount___16;

		internal List<int> _resultMissingOperatorMessageIdList___17;

		internal int _i___18;

		internal int _missingId___19;

		internal MsgUpdateMesseageSucceed _msg___20;

		internal MsgServerConnctFailed _msg___21;

		internal int _PC;

		internal object _current;

		internal List<int> ___messageIdList;

		internal List<int> ___operatorMessageIdList;

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
					goto IL_4FA;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E0;
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
				goto IL_4FA;
			}
			this._net___1 = new NetServerUpdateMessage(this.messageIdList, this.operatorMessageIdList);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerUpdateMessageRetry(this.messageIdList, this.operatorMessageIdList, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._playerState___2 = ServerInterface.PlayerState;
				this._resultPlayerState___3 = this._net___1.resultPlayerState;
				if (this._resultPlayerState___3 != null)
				{
					this._resultPlayerState___3.CopyTo(this._playerState___2);
				}
				this._characterState___4 = this._net___1.resultCharacterState;
				if (this._characterState___4 != null)
				{
					this._playerState___2.SetCharacterState(this._characterState___4);
				}
				this._chaoState___5 = this._net___1.resultChaoState;
				if (this._chaoState___5 != null)
				{
					this._playerState___2.SetChaoState(this._chaoState___5);
				}
				this._chaoRoulette___6 = 0;
				this._itemRoulette___7 = 0;
				this._presentCount___8 = this._net___1.resultPresentStates;
				this._resultPresentList___9 = new List<ServerPresentState>(this._presentCount___8);
				this._i___10 = 0;
				while (this._i___10 < this._presentCount___8)
				{
					this._presentState___11 = this._net___1.GetResultPresentState(this._i___10);
					this._resultPresentList___9.Add(this._presentState___11);
					if (this._presentState___11.m_itemId == 230000)
					{
						this._chaoRoulette___6 += this._presentState___11.m_numItem;
					}
					else if (this._presentState___11.m_itemId == 240000)
					{
						this._itemRoulette___7 += this._presentState___11.m_numItem;
					}
					this._i___10++;
				}
				ServerInterface.ChaoWheelOptions.NumRouletteToken += this._chaoRoulette___6;
				ServerInterface.WheelOptions.m_numRemaining += this._itemRoulette___7;
				this._missingMessageCount___12 = this._net___1.resultMissingMessages;
				this._resultMissingMessageIdList___13 = new List<int>(this._missingMessageCount___12);
				this._i___14 = 0;
				while (this._i___14 < this._missingMessageCount___12)
				{
					this._missingId___15 = this._net___1.GetResultMissingMessageId(this._i___14);
					this._resultMissingMessageIdList___13.Add(this._missingId___15);
					this._i___14++;
				}
				this._missingOperatorMessageCount___16 = this._net___1.resultMissingOperatorMessages;
				this._resultMissingOperatorMessageIdList___17 = new List<int>(this._missingOperatorMessageCount___16);
				this._i___18 = 0;
				while (this._i___18 < this._missingOperatorMessageCount___16)
				{
					this._missingId___19 = this._net___1.GetResultMissingOperatorMessageId(this._i___18);
					this._resultMissingOperatorMessageIdList___17.Add(this._missingId___19);
					this._i___18++;
				}
				if (this.messageIdList == null)
				{
					ServerUpdateMessage.UpdateMessageList(ServerInterface.MessageList, this._resultMissingMessageIdList___13);
				}
				else
				{
					ServerUpdateMessage.UpdateMessageList(ServerInterface.MessageList, this.messageIdList, this._resultMissingMessageIdList___13);
				}
				if (this.operatorMessageIdList == null)
				{
					ServerUpdateMessage.UpdateOperatorMessageList(ServerInterface.OperatorMessageList, this._resultMissingOperatorMessageIdList___17);
				}
				else
				{
					ServerUpdateMessage.UpdateOperatorMessageList(ServerInterface.OperatorMessageList, this.operatorMessageIdList, this._resultMissingOperatorMessageIdList___17);
				}
				this._msg___20 = new MsgUpdateMesseageSucceed();
				this._msg___20.m_presentStateList = this._resultPresentList___9;
				this._msg___20.m_notRecvMessageList = this._resultMissingMessageIdList___13;
				this._msg___20.m_notRecvOperatorMessageList = this._resultMissingOperatorMessageIdList___17;
				GeneralUtil.SetPresentItemCount(this._msg___20);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___20, this.callbackObject, "ServerUpdateMessage_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUpdateMessage_Succeeded", this._msg___20, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___21 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___21, this.callbackObject, "ServerUpdateMessage_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_4FA:
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

	private const string SuccessEvent = "ServerUpdateMessage_Succeeded";

	private const string FailEvent = "ServerUpdateMessage_Failed";

	public static IEnumerator Process(List<int> messageIdList, List<int> operatorMessageIdList, GameObject callbackObject)
	{
		ServerUpdateMessage._Process_c__IteratorA0 _Process_c__IteratorA = new ServerUpdateMessage._Process_c__IteratorA0();
		_Process_c__IteratorA.messageIdList = messageIdList;
		_Process_c__IteratorA.operatorMessageIdList = operatorMessageIdList;
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___messageIdList = messageIdList;
		_Process_c__IteratorA.___operatorMessageIdList = operatorMessageIdList;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}

	private static void UpdateMessageList(List<ServerMessageEntry> msgList, List<int> missingList)
	{
		if (msgList != null && missingList != null)
		{
			List<ServerMessageEntry> list = new List<ServerMessageEntry>();
			foreach (int current in missingList)
			{
				for (int i = 0; i < msgList.Count; i++)
				{
					if (current == msgList[i].m_messageId)
					{
						ServerMessageEntry serverMessageEntry = new ServerMessageEntry();
						msgList[i].CopyTo(serverMessageEntry);
						list.Add(serverMessageEntry);
						break;
					}
				}
			}
			msgList.Clear();
			for (int j = 0; j < list.Count; j++)
			{
				msgList.Add(list[j]);
			}
		}
	}

	private static void UpdateOperatorMessageList(List<ServerOperatorMessageEntry> msgList, List<int> missingList)
	{
		if (msgList != null && missingList != null)
		{
			List<ServerOperatorMessageEntry> list = new List<ServerOperatorMessageEntry>();
			foreach (int current in missingList)
			{
				for (int i = 0; i < msgList.Count; i++)
				{
					if (current == msgList[i].m_messageId)
					{
						ServerOperatorMessageEntry serverOperatorMessageEntry = new ServerOperatorMessageEntry();
						msgList[i].CopyTo(serverOperatorMessageEntry);
						list.Add(serverOperatorMessageEntry);
						break;
					}
				}
			}
			msgList.Clear();
			for (int j = 0; j < list.Count; j++)
			{
				msgList.Add(list[j]);
			}
		}
	}

	private static void UpdateMessageList(List<ServerMessageEntry> msgList, List<int> requestList, List<int> missingList)
	{
		if (msgList != null && requestList != null && missingList != null)
		{
			foreach (int current in missingList)
			{
				for (int i = 0; i < requestList.Count; i++)
				{
					if (current == requestList[i])
					{
						requestList.Remove(requestList[i]);
						break;
					}
				}
			}
			foreach (int current2 in requestList)
			{
				for (int j = 0; j < msgList.Count; j++)
				{
					if (current2 == msgList[j].m_messageId)
					{
						msgList.Remove(msgList[j]);
						break;
					}
				}
			}
		}
	}

	private static void UpdateOperatorMessageList(List<ServerOperatorMessageEntry> msgList, List<int> requestList, List<int> missingList)
	{
		if (msgList != null && requestList != null && missingList != null)
		{
			foreach (int current in missingList)
			{
				for (int i = 0; i < requestList.Count; i++)
				{
					if (current == requestList[i])
					{
						requestList.Remove(requestList[i]);
						break;
					}
				}
			}
			foreach (int current2 in requestList)
			{
				for (int j = 0; j < msgList.Count; j++)
				{
					if (current2 == msgList[j].m_messageId)
					{
						msgList.Remove(msgList[j]);
						break;
					}
				}
			}
		}
	}
}
