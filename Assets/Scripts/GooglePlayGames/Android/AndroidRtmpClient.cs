using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidRtmpClient : IRealTimeMultiplayerClient
	{
		private class RoomUpdateProxy : AndroidJavaProxy
		{
			private AndroidRtmpClient mOwner;

			internal RoomUpdateProxy(AndroidRtmpClient owner) : base("com.google.android.gms.games.multiplayer.realtime.RoomUpdateListener")
			{
				this.mOwner = owner;
			}

			public void onJoinedRoom(int statusCode, AndroidJavaObject room)
			{
				this.mOwner.OnJoinedRoom(statusCode, room);
			}

			public void onLeftRoom(int statusCode, AndroidJavaObject room)
			{
				this.mOwner.OnLeftRoom(statusCode, room);
			}

			public void onRoomConnected(int statusCode, AndroidJavaObject room)
			{
				this.mOwner.OnRoomConnected(statusCode, room);
			}

			public void onRoomCreated(int statusCode, AndroidJavaObject room)
			{
				this.mOwner.OnRoomCreated(statusCode, room);
			}
		}

		private class RoomStatusUpdateProxy : AndroidJavaProxy
		{
			private AndroidRtmpClient mOwner;

			internal RoomStatusUpdateProxy(AndroidRtmpClient owner) : base("com.google.android.gms.games.multiplayer.realtime.RoomStatusUpdateListener")
			{
				this.mOwner = owner;
			}

			public void onConnectedToRoom(AndroidJavaObject room)
			{
				this.mOwner.OnConnectedToRoom(room);
			}

			public void onDisconnectedFromRoom(AndroidJavaObject room)
			{
				this.mOwner.OnDisconnectedFromRoom(room);
			}

			public void onP2PConnected(string participantId)
			{
				this.mOwner.OnP2PConnected(participantId);
			}

			public void onP2PDisconnected(string participantId)
			{
				this.mOwner.OnP2PDisconnected(participantId);
			}

			public void onPeerDeclined(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeerDeclined(room, participantIds);
			}

			public void onPeerInvitedToRoom(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeerInvitedToRoom(room, participantIds);
			}

			public void onPeerJoined(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeerJoined(room, participantIds);
			}

			public void onPeerLeft(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeerLeft(room, participantIds);
			}

			public void onPeersConnected(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeersConnected(room, participantIds);
			}

			public void onPeersDisconnected(AndroidJavaObject room, AndroidJavaObject participantIds)
			{
				this.mOwner.OnPeersDisconnected(room, participantIds);
			}

			public void onRoomAutoMatching(AndroidJavaObject room)
			{
				this.mOwner.OnRoomAutoMatching(room);
			}

			public void onRoomConnecting(AndroidJavaObject room)
			{
				this.mOwner.OnRoomConnecting(room);
			}
		}

		private class RealTimeMessageReceivedProxy : AndroidJavaProxy
		{
			private AndroidRtmpClient mOwner;

			internal RealTimeMessageReceivedProxy(AndroidRtmpClient owner) : base("com.google.android.gms.games.multiplayer.realtime.RealTimeMessageReceivedListener")
			{
				this.mOwner = owner;
			}

			public void onRealTimeMessageReceived(AndroidJavaObject message)
			{
				this.mOwner.OnRealTimeMessageReceived(message);
			}
		}

		private class SelectOpponentsProxy : AndroidJavaProxy
		{
			private AndroidRtmpClient mOwner;

			internal SelectOpponentsProxy(AndroidRtmpClient owner) : base("com.google.example.games.pluginsupport.SelectOpponentsHelperActivity$Listener")
			{
				this.mOwner = owner;
			}

			public void onSelectOpponentsResult(bool success, AndroidJavaObject opponents, bool hasAutoMatch, AndroidJavaObject autoMatchCriteria)
			{
				this.mOwner.OnSelectOpponentsResult(success, opponents, hasAutoMatch, autoMatchCriteria);
			}
		}

		private class InvitationInboxProxy : AndroidJavaProxy
		{
			private AndroidRtmpClient mOwner;

			internal InvitationInboxProxy(AndroidRtmpClient owner) : base("com.google.example.games.pluginsupport.InvitationInboxHelperActivity$Listener")
			{
				this.mOwner = owner;
			}

			public void onInvitationInboxResult(bool success, string invitationId)
			{
				this.mOwner.OnInvitationInboxResult(success, invitationId);
			}

			public void onTurnBasedMatch(AndroidJavaObject match)
			{
				Logger.e("Bug: RTMP proxy got onTurnBasedMatch(). Shouldn't happen. Ignoring.");
			}
		}

		private sealed class _CreateQuickGame_c__AnonStoreyDE
		{
			internal int minOpponents;

			internal int maxOpponents;

			internal int variant;

			internal AndroidRtmpClient __f__this;

			internal void __m__15()
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("createQuickGame", new object[]
				{
					this.__f__this.mClient.GHManager.GetApiClient(),
					this.minOpponents,
					this.maxOpponents,
					this.variant,
					new AndroidRtmpClient.RoomUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this.__f__this)
				});
			}

			internal void __m__16(bool success)
			{
				if (!success)
				{
					this.__f__this.FailRoomSetup("Failed to create game because GoogleApiClient was disconnected");
				}
			}
		}

		private sealed class _CreateWithInvitationScreen_c__AnonStoreyDF
		{
			internal int minOpponents;

			internal int maxOpponents;

			internal AndroidRtmpClient __f__this;

			internal void __m__17()
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.SelectOpponentsHelperActivity");
				this.__f__this.mLaunchedExternalActivity = true;
				@class.CallStatic("launch", new object[]
				{
					true,
					this.__f__this.mClient.GetActivity(),
					new AndroidRtmpClient.SelectOpponentsProxy(this.__f__this),
					Logger.DebugLogEnabled,
					this.minOpponents,
					this.maxOpponents
				});
			}

			internal void __m__18(bool success)
			{
				if (!success)
				{
					this.__f__this.FailRoomSetup("Failed to create game because GoogleApiClient was disconnected");
				}
			}
		}

		private sealed class _AcceptInvitation_c__AnonStoreyE0
		{
			internal string invitationId;

			internal AndroidRtmpClient __f__this;

			internal void __m__1B()
			{
				Logger.d("Accepting invite via support lib.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("accept", new object[]
				{
					this.__f__this.mClient.GHManager.GetApiClient(),
					this.invitationId,
					new AndroidRtmpClient.RoomUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this.__f__this)
				});
			}

			internal void __m__1C(bool success)
			{
				if (!success)
				{
					this.__f__this.FailRoomSetup("Failed to accept invitation because GoogleApiClient was disconnected");
				}
			}
		}

		private sealed class _SendMessage_c__AnonStoreyE1
		{
			internal bool reliable;

			internal byte[] dataToSend;

			internal string participantId;

			internal AndroidRtmpClient __f__this;

			internal void __m__1D()
			{
				if (this.__f__this.mRoom != null)
				{
					string text = this.__f__this.mRoom.Call<string>("getRoomId", new object[0]);
					if (this.reliable)
					{
						this.__f__this.mClient.GHManager.CallGmsApi<int>("games.Games", "RealTimeMultiplayer", "sendReliableMessage", new object[]
						{
							null,
							this.dataToSend,
							text,
							this.participantId
						});
					}
					else
					{
						this.__f__this.mClient.GHManager.CallGmsApi<int>("games.Games", "RealTimeMultiplayer", "sendUnreliableMessage", new object[]
						{
							this.dataToSend,
							text,
							this.participantId
						});
					}
				}
				else
				{
					Logger.w("Not sending message because real-time room was torn down.");
				}
			}
		}

		private sealed class _DeclineInvitation_c__AnonStoreyE2
		{
			internal string invitationId;

			internal AndroidRtmpClient __f__this;

			internal void __m__1F()
			{
				this.__f__this.mClient.GHManager.CallGmsApi("games.Games", "RealTimeMultiplayer", "declineInvitation", new object[]
				{
					this.invitationId
				});
			}
		}

		private sealed class _Clear_c__AnonStoreyE3
		{
			internal RealTimeMultiplayerListener listener;

			internal void __m__21()
			{
				this.listener.OnLeftRoom();
			}
		}

		private sealed class _FailRoomSetup_c__AnonStoreyE4
		{
			internal RealTimeMultiplayerListener listener;

			internal void __m__22()
			{
				this.listener.OnRoomConnected(false);
			}
		}

		private sealed class _OnRoomConnected_c__AnonStoreyE5
		{
			internal RealTimeMultiplayerListener listener;

			internal AndroidRtmpClient __f__this;

			internal void __m__23()
			{
				this.__f__this.mDeliveredRoomConnected = true;
				this.listener.OnRoomConnected(true);
			}
		}

		private sealed class _OnRealTimeMessageReceived_c__AnonStoreyE6
		{
			internal RealTimeMultiplayerListener listener;
		}

		private sealed class _OnRealTimeMessageReceived_c__AnonStoreyE7
		{
			internal bool isReliable;

			internal string senderId;

			internal byte[] messageData;

			internal AndroidRtmpClient._OnRealTimeMessageReceived_c__AnonStoreyE6 __f__ref_230;

			internal void __m__24()
			{
				this.__f__ref_230.listener.OnRealTimeMessageReceived(this.isReliable, this.senderId, this.messageData);
			}
		}

		private sealed class _OnSelectOpponentsResult_c__AnonStoreyE8
		{
			internal AndroidJavaObject opponents;

			internal bool hasAutoMatch;

			internal AndroidJavaObject autoMatchCriteria;

			internal AndroidRtmpClient __f__this;

			internal void __m__25()
			{
				Logger.d("Creating room via support lib's RtmpUtil.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("create", new object[]
				{
					this.__f__this.mClient.GHManager.GetApiClient(),
					this.opponents,
					this.__f__this.mVariant,
					(!this.hasAutoMatch) ? null : this.autoMatchCriteria,
					new AndroidRtmpClient.RoomUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this.__f__this)
				});
			}

			internal void __m__26(bool ok)
			{
				if (!ok)
				{
					this.__f__this.FailRoomSetup("GoogleApiClient lost connection");
				}
			}
		}

		private sealed class _OnInvitationInboxResult_c__AnonStoreyE9
		{
			internal string invitationId;

			internal AndroidRtmpClient __f__this;

			internal void __m__27()
			{
				Logger.d("Accepting invite from inbox via support lib.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("accept", new object[]
				{
					this.__f__this.mClient.GHManager.GetApiClient(),
					this.invitationId,
					new AndroidRtmpClient.RoomUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this.__f__this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this.__f__this)
				});
			}

			internal void __m__28(bool ok)
			{
				if (!ok)
				{
					this.__f__this.FailRoomSetup("GoogleApiClient lost connection.");
				}
			}
		}

		private sealed class _DeliverRoomSetupProgressUpdate_c__AnonStoreyEA
		{
			internal float progress;

			internal AndroidRtmpClient __f__this;

			internal void __m__29()
			{
				this.__f__this.mRtmpListener.OnRoomSetupProgress(this.progress);
			}
		}

		private AndroidClient mClient;

		private AndroidJavaObject mRoom;

		private RealTimeMultiplayerListener mRtmpListener;

		private bool mRtmpActive;

		private bool mLaunchedExternalActivity;

		private bool mDeliveredRoomConnected;

		private bool mLeaveRoomRequested;

		private int mVariant;

		private object mParticipantListsLock = new object();

		private List<Participant> mConnectedParticipants = new List<Participant>();

		private List<Participant> mAllParticipants = new List<Participant>();

		private Participant mSelf;

		private float mAccumulatedProgress;

		private float mLastReportedProgress;

		private static Action<bool> __f__am_cacheE;

		public AndroidRtmpClient(AndroidClient client)
		{
			this.mClient = client;
		}

		public void CreateQuickGame(int minOpponents, int maxOpponents, int variant, RealTimeMultiplayerListener listener)
		{
			Logger.d(string.Format("AndroidRtmpClient.CreateQuickGame, opponents={0}-{1}, variant={2}", minOpponents, maxOpponents, variant));
			if (!this.PrepareToCreateRoom("CreateQuickGame", listener))
			{
				return;
			}
			this.mRtmpListener = listener;
			this.mVariant = variant;
			this.mClient.CallClientApi("rtmp create quick game", delegate
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("createQuickGame", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					minOpponents,
					maxOpponents,
					variant,
					new AndroidRtmpClient.RoomUpdateProxy(this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this)
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					this.FailRoomSetup("Failed to create game because GoogleApiClient was disconnected");
				}
			});
		}

		public void CreateWithInvitationScreen(int minOpponents, int maxOpponents, int variant, RealTimeMultiplayerListener listener)
		{
			Logger.d(string.Format("AndroidRtmpClient.CreateWithInvitationScreen, opponents={0}-{1}, variant={2}", minOpponents, maxOpponents, variant));
			if (!this.PrepareToCreateRoom("CreateWithInvitationScreen", listener))
			{
				return;
			}
			this.mRtmpListener = listener;
			this.mVariant = variant;
			this.mClient.CallClientApi("rtmp create with invitation screen", delegate
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.SelectOpponentsHelperActivity");
				this.mLaunchedExternalActivity = true;
				@class.CallStatic("launch", new object[]
				{
					true,
					this.mClient.GetActivity(),
					new AndroidRtmpClient.SelectOpponentsProxy(this),
					Logger.DebugLogEnabled,
					minOpponents,
					maxOpponents
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					this.FailRoomSetup("Failed to create game because GoogleApiClient was disconnected");
				}
			});
		}

		public void AcceptFromInbox(RealTimeMultiplayerListener listener)
		{
			Logger.d("AndroidRtmpClient.AcceptFromInbox.");
			if (!this.PrepareToCreateRoom("AcceptFromInbox", listener))
			{
				return;
			}
			this.mRtmpListener = listener;
			this.mClient.CallClientApi("rtmp accept with inbox screen", delegate
			{
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.InvitationInboxHelperActivity");
				this.mLaunchedExternalActivity = true;
				@class.CallStatic("launch", new object[]
				{
					true,
					this.mClient.GetActivity(),
					new AndroidRtmpClient.InvitationInboxProxy(this),
					Logger.DebugLogEnabled
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					this.FailRoomSetup("Failed to accept from inbox because GoogleApiClient was disconnected");
				}
			});
		}

		public void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener)
		{
			Logger.d("AndroidRtmpClient.AcceptInvitation " + invitationId);
			if (!this.PrepareToCreateRoom("AcceptInvitation", listener))
			{
				return;
			}
			this.mRtmpListener = listener;
			this.mClient.ClearInvitationIfFromNotification(invitationId);
			this.mClient.CallClientApi("rtmp accept invitation", delegate
			{
				Logger.d("Accepting invite via support lib.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("accept", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					invitationId,
					new AndroidRtmpClient.RoomUpdateProxy(this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this)
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					this.FailRoomSetup("Failed to accept invitation because GoogleApiClient was disconnected");
				}
			});
		}

		public void SendMessage(bool reliable, string participantId, byte[] data)
		{
			this.SendMessage(reliable, participantId, data, 0, data.Length);
		}

		public void SendMessageToAll(bool reliable, byte[] data)
		{
			this.SendMessage(reliable, null, data, 0, data.Length);
		}

		public void SendMessageToAll(bool reliable, byte[] data, int offset, int length)
		{
			this.SendMessage(reliable, null, data, offset, length);
		}

		public void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length)
		{
			Logger.d(string.Format("AndroidRtmpClient.SendMessage, reliable={0}, participantId={1}, data[]={2} bytes, offset={3}, length={4}", new object[]
			{
				reliable,
				participantId,
				data.Length,
				offset,
				length
			}));
			if (!this.CheckConnectedRoom("SendMessage"))
			{
				return;
			}
			if (this.mSelf != null && this.mSelf.ParticipantId.Equals(participantId))
			{
				Logger.d("Ignoring request to send message to self, " + participantId);
				return;
			}
			byte[] dataToSend = Misc.GetSubsetBytes(data, offset, length);
			if (participantId == null)
			{
				List<Participant> connectedParticipants = this.GetConnectedParticipants();
				foreach (Participant current in connectedParticipants)
				{
					if (current.ParticipantId != null && !current.Equals(this.mSelf))
					{
						this.SendMessage(reliable, current.ParticipantId, dataToSend, 0, dataToSend.Length);
					}
				}
				return;
			}
			this.mClient.CallClientApi("send message to " + participantId, delegate
			{
				if (this.mRoom != null)
				{
					string text = this.mRoom.Call<string>("getRoomId", new object[0]);
					if (reliable)
					{
						this.mClient.GHManager.CallGmsApi<int>("games.Games", "RealTimeMultiplayer", "sendReliableMessage", new object[]
						{
							null,
							dataToSend,
							text,
							participantId
						});
					}
					else
					{
						this.mClient.GHManager.CallGmsApi<int>("games.Games", "RealTimeMultiplayer", "sendUnreliableMessage", new object[]
						{
							dataToSend,
							text,
							participantId
						});
					}
				}
				else
				{
					Logger.w("Not sending message because real-time room was torn down.");
				}
			}, null);
		}

		public List<Participant> GetConnectedParticipants()
		{
			Logger.d("AndroidRtmpClient.GetConnectedParticipants");
			if (!this.CheckConnectedRoom("GetConnectedParticipants"))
			{
				return null;
			}
			object obj = this.mParticipantListsLock;
			List<Participant> result;
			lock (obj)
			{
				result = this.mConnectedParticipants;
			}
			return result;
		}

		public Participant GetParticipant(string id)
		{
			Logger.d("AndroidRtmpClient.GetParticipant: " + id);
			if (!this.CheckConnectedRoom("GetParticipant"))
			{
				return null;
			}
			object obj = this.mParticipantListsLock;
			List<Participant> list;
			lock (obj)
			{
				list = this.mAllParticipants;
			}
			if (list == null)
			{
				Logger.e("RtmpGetParticipant called without a valid room!");
				return null;
			}
			foreach (Participant current in list)
			{
				if (current.ParticipantId.Equals(id))
				{
					return current;
				}
			}
			Logger.e("Participant not found in room! id: " + id);
			return null;
		}

		public Participant GetSelf()
		{
			Logger.d("AndroidRtmpClient.GetSelf");
			if (!this.CheckConnectedRoom("GetSelf"))
			{
				return null;
			}
			object obj = this.mParticipantListsLock;
			Participant participant;
			lock (obj)
			{
				participant = this.mSelf;
			}
			if (participant == null)
			{
				Logger.e("Call to RtmpGetSelf() can only be made when in a room. Returning null.");
			}
			return participant;
		}

		public void LeaveRoom()
		{
			Logger.d("AndroidRtmpClient.LeaveRoom");
			if (this.mRtmpActive && this.mRoom == null)
			{
				Logger.w("AndroidRtmpClient.LeaveRoom: waiting for room; deferring leave request.");
				this.mLeaveRoomRequested = true;
			}
			else
			{
				this.mClient.CallClientApi("leave room", delegate
				{
					this.Clear("LeaveRoom called");
				}, null);
			}
		}

		public void OnStop()
		{
			if (this.mLaunchedExternalActivity)
			{
				Logger.d("OnStop: EXTERNAL ACTIVITY is pending, so not clearing RTMP.");
			}
			else
			{
				this.Clear("leaving room because game is stopping.");
			}
		}

		public bool IsRoomConnected()
		{
			return this.mRoom != null && this.mDeliveredRoomConnected;
		}

		public void DeclineInvitation(string invitationId)
		{
			Logger.d("AndroidRtmpClient.DeclineInvitation " + invitationId);
			this.mClient.ClearInvitationIfFromNotification(invitationId);
			this.mClient.CallClientApi("rtmp decline invitation", delegate
			{
				this.mClient.GHManager.CallGmsApi("games.Games", "RealTimeMultiplayer", "declineInvitation", new object[]
				{
					invitationId
				});
			}, delegate(bool success)
			{
				if (!success)
				{
					Logger.w("Failed to decline invitation. GoogleApiClient was disconnected");
				}
			});
		}

		private bool PrepareToCreateRoom(string method, RealTimeMultiplayerListener listener)
		{
			if (this.mRtmpActive)
			{
				Logger.e("Cannot call " + method + " while a real-time game is active.");
				if (listener != null)
				{
					Logger.d("Notifying listener of failure to create room.");
					listener.OnRoomConnected(false);
				}
				return false;
			}
			this.mAccumulatedProgress = 0f;
			this.mLastReportedProgress = 0f;
			this.mRtmpListener = listener;
			this.mRtmpActive = true;
			return true;
		}

		private bool CheckConnectedRoom(string method)
		{
			if (this.mRoom == null || !this.mDeliveredRoomConnected)
			{
				Logger.e("Method " + method + " called without a connected room. You must create or join a room AND wait until you get the OnRoomConnected(true) callback.");
				return false;
			}
			return true;
		}

		private void Clear(string reason)
		{
			Logger.d("RtmpClear: clearing RTMP (reason: " + reason + ").");
			if (this.mRoom != null)
			{
				Logger.d("RtmpClear: Room still active, so leaving room.");
				string text = this.mRoom.Call<string>("getRoomId", new object[0]);
				Logger.d("RtmpClear: room id to leave is " + text);
				this.mClient.GHManager.CallGmsApi("games.Games", "RealTimeMultiplayer", "leave", new object[]
				{
					new NoopProxy("com.google.android.gms.games.multiplayer.realtime.RoomUpdateListener"),
					text
				});
				Logger.d("RtmpClear: left room.");
				this.mRoom = null;
			}
			else
			{
				Logger.d("RtmpClear: no room active.");
			}
			if (this.mDeliveredRoomConnected)
			{
				Logger.d("RtmpClear: looks like we must call the OnLeftRoom() callback.");
				RealTimeMultiplayerListener listener = this.mRtmpListener;
				if (listener != null)
				{
					Logger.d("Calling OnLeftRoom() callback.");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						listener.OnLeftRoom();
					});
				}
			}
			else
			{
				Logger.d("RtmpClear: no need to call OnLeftRoom() callback.");
			}
			this.mLeaveRoomRequested = false;
			this.mDeliveredRoomConnected = false;
			this.mRoom = null;
			this.mConnectedParticipants = null;
			this.mAllParticipants = null;
			this.mSelf = null;
			this.mRtmpListener = null;
			this.mVariant = 0;
			this.mRtmpActive = false;
			this.mAccumulatedProgress = 0f;
			this.mLastReportedProgress = 0f;
			this.mLaunchedExternalActivity = false;
			Logger.d("RtmpClear: RTMP cleared.");
		}

		private string[] SubtractParticipants(List<Participant> a, List<Participant> b)
		{
			List<string> list = new List<string>();
			if (a != null)
			{
				foreach (Participant current in a)
				{
					list.Add(current.ParticipantId);
				}
			}
			if (b != null)
			{
				foreach (Participant current2 in b)
				{
					if (list.Contains(current2.ParticipantId))
					{
						list.Remove(current2.ParticipantId);
					}
				}
			}
			return list.ToArray();
		}

		private void UpdateRoom()
		{
			List<AndroidJavaObject> list = new List<AndroidJavaObject>();
			Logger.d("UpdateRoom: Updating our cached data about the room.");
			string str = this.mRoom.Call<string>("getRoomId", new object[0]);
			Logger.d("UpdateRoom: room id: " + str);
			Logger.d("UpdateRoom: querying for my player ID.");
			string text = this.mClient.GHManager.CallGmsApi<string>("games.Games", "Players", "getCurrentPlayerId", new object[0]);
			Logger.d("UpdateRoom: my player ID is: " + text);
			Logger.d("UpdateRoom: querying for my participant ID in the room.");
			string text2 = this.mRoom.Call<string>("getParticipantId", new object[]
			{
				text
			});
			Logger.d("UpdateRoom: my participant ID is: " + text2);
			AndroidJavaObject androidJavaObject = this.mRoom.Call<AndroidJavaObject>("getParticipantIds", new object[0]);
			list.Add(androidJavaObject);
			int num = androidJavaObject.Call<int>("size", new object[0]);
			Logger.d("UpdateRoom: # participants: " + num);
			List<Participant> list2 = new List<Participant>();
			List<Participant> list3 = new List<Participant>();
			this.mSelf = null;
			for (int i = 0; i < num; i++)
			{
				Logger.d("UpdateRoom: querying participant #" + i);
				string text3 = androidJavaObject.Call<string>("get", new object[]
				{
					i
				});
				Logger.d(string.Concat(new object[]
				{
					"UpdateRoom: participant #",
					i,
					" has id: ",
					text3
				}));
				AndroidJavaObject androidJavaObject2 = this.mRoom.Call<AndroidJavaObject>("getParticipant", new object[]
				{
					text3
				});
				list.Add(androidJavaObject2);
				Participant participant = JavaUtil.ConvertParticipant(androidJavaObject2);
				list3.Add(participant);
				if (participant.ParticipantId.Equals(text2))
				{
					Logger.d("Participant is SELF.");
					this.mSelf = participant;
				}
				if (participant.IsConnectedToRoom)
				{
					list2.Add(participant);
				}
			}
			if (this.mSelf == null)
			{
				Logger.e("List of room participants did not include self,  participant id: " + text2 + ", player id: " + text);
				this.mSelf = new Participant("?", text2, Participant.ParticipantStatus.Unknown, new Player("?", text), false);
			}
			list2.Sort();
			list3.Sort();
			object obj = this.mParticipantListsLock;
			string[] array;
			string[] array2;
			lock (obj)
			{
				array = this.SubtractParticipants(list2, this.mConnectedParticipants);
				array2 = this.SubtractParticipants(this.mConnectedParticipants, list2);
				this.mConnectedParticipants = list2;
				this.mAllParticipants = list3;
				Logger.d("UpdateRoom: participant list now has " + this.mConnectedParticipants.Count + " participants.");
			}
			Logger.d("UpdateRoom: cleanup.");
			foreach (AndroidJavaObject current in list)
			{
				current.Dispose();
			}
			Logger.d("UpdateRoom: newly connected participants: " + array.Length);
			Logger.d("UpdateRoom: newly disconnected participants: " + array2.Length);
			if (this.mDeliveredRoomConnected)
			{
				if (array.Length > 0 && this.mRtmpListener != null)
				{
					Logger.d("UpdateRoom: calling OnPeersConnected callback");
					this.mRtmpListener.OnPeersConnected(array);
				}
				if (array2.Length > 0 && this.mRtmpListener != null)
				{
					Logger.d("UpdateRoom: calling OnPeersDisconnected callback");
					this.mRtmpListener.OnPeersDisconnected(array2);
				}
			}
			if (this.mLeaveRoomRequested)
			{
				this.Clear("deferred leave-room request");
			}
			if (!this.mDeliveredRoomConnected)
			{
				this.DeliverRoomSetupProgressUpdate();
			}
		}

		private void FailRoomSetup(string reason)
		{
			Logger.d("Failing room setup: " + reason);
			RealTimeMultiplayerListener listener = this.mRtmpListener;
			this.Clear("Room setup failed: " + reason);
			if (listener != null)
			{
				Logger.d("Invoking callback OnRoomConnected(false) to signal failure.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					listener.OnRoomConnected(false);
				});
			}
		}

		private bool CheckRtmpActive(string method)
		{
			if (!this.mRtmpActive)
			{
				Logger.d("Got call to " + method + " with RTMP inactive. Ignoring.");
				return false;
			}
			return true;
		}

		private void OnJoinedRoom(int statusCode, AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnJoinedRoom, status " + statusCode);
			if (!this.CheckRtmpActive("OnJoinedRoom"))
			{
				return;
			}
			this.mRoom = room;
			this.mAccumulatedProgress += 20f;
			if (statusCode != 0)
			{
				this.FailRoomSetup("OnJoinedRoom error code " + statusCode);
			}
		}

		private void OnLeftRoom(int statusCode, AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnLeftRoom, status " + statusCode);
			if (!this.CheckRtmpActive("OnLeftRoom"))
			{
				return;
			}
			this.Clear("Got OnLeftRoom " + statusCode);
		}

		private void OnRoomConnected(int statusCode, AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnRoomConnected, status " + statusCode);
			if (!this.CheckRtmpActive("OnRoomConnected"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
			if (statusCode != 0)
			{
				this.FailRoomSetup("OnRoomConnected error code " + statusCode);
			}
			else
			{
				Logger.d("AndroidClient.OnRoomConnected: room setup succeeded!");
				RealTimeMultiplayerListener listener = this.mRtmpListener;
				if (listener != null)
				{
					Logger.d("Invoking callback OnRoomConnected(true) to report success.");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						this.mDeliveredRoomConnected = true;
						listener.OnRoomConnected(true);
					});
				}
			}
		}

		private void OnRoomCreated(int statusCode, AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnRoomCreated, status " + statusCode);
			if (!this.CheckRtmpActive("OnRoomCreated"))
			{
				return;
			}
			this.mRoom = room;
			this.mAccumulatedProgress += 20f;
			if (statusCode != 0)
			{
				this.FailRoomSetup("OnRoomCreated error code " + statusCode);
			}
			this.UpdateRoom();
		}

		private void OnConnectedToRoom(AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnConnectedToRoom");
			if (!this.CheckRtmpActive("OnConnectedToRoom"))
			{
				return;
			}
			this.mAccumulatedProgress += 10f;
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnDisconnectedFromRoom(AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnDisconnectedFromRoom");
			if (!this.CheckRtmpActive("OnDisconnectedFromRoom"))
			{
				return;
			}
			this.Clear("Got OnDisconnectedFromRoom");
		}

		private void OnP2PConnected(string participantId)
		{
			Logger.d("AndroidClient.OnP2PConnected: " + participantId);
			if (!this.CheckRtmpActive("OnP2PConnected"))
			{
				return;
			}
			this.UpdateRoom();
		}

		private void OnP2PDisconnected(string participantId)
		{
			Logger.d("AndroidClient.OnP2PDisconnected: " + participantId);
			if (!this.CheckRtmpActive("OnP2PDisconnected"))
			{
				return;
			}
			this.UpdateRoom();
		}

		private void OnPeerDeclined(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeerDeclined");
			if (!this.CheckRtmpActive("OnPeerDeclined"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
			if (!this.mDeliveredRoomConnected)
			{
				this.FailRoomSetup("OnPeerDeclined received during setup");
			}
		}

		private void OnPeerInvitedToRoom(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeerInvitedToRoom");
			if (!this.CheckRtmpActive("OnPeerInvitedToRoom"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnPeerJoined(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeerJoined");
			if (!this.CheckRtmpActive("OnPeerJoined"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnPeerLeft(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeerLeft");
			if (!this.CheckRtmpActive("OnPeerLeft"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
			if (!this.mDeliveredRoomConnected)
			{
				this.FailRoomSetup("OnPeerLeft received during setup");
			}
		}

		private void OnPeersConnected(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeersConnected");
			if (!this.CheckRtmpActive("OnPeersConnected"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnPeersDisconnected(AndroidJavaObject room, AndroidJavaObject participantIds)
		{
			Logger.d("AndroidClient.OnPeersDisconnected.");
			if (!this.CheckRtmpActive("OnPeersDisconnected"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnRoomAutoMatching(AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnRoomAutoMatching");
			if (!this.CheckRtmpActive("OnRoomAutomatching"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnRoomConnecting(AndroidJavaObject room)
		{
			Logger.d("AndroidClient.OnRoomConnecting.");
			if (!this.CheckRtmpActive("OnRoomConnecting"))
			{
				return;
			}
			this.mRoom = room;
			this.UpdateRoom();
		}

		private void OnRealTimeMessageReceived(AndroidJavaObject message)
		{
			Logger.d("AndroidClient.OnRealTimeMessageReceived.");
			if (!this.CheckRtmpActive("OnRealTimeMessageReceived"))
			{
				return;
			}
			RealTimeMultiplayerListener listener = this.mRtmpListener;
			if (listener != null)
			{
				byte[] messageData;
				using (AndroidJavaObject androidJavaObject = message.Call<AndroidJavaObject>("getMessageData", new object[0]))
				{
					messageData = JavaUtil.ConvertByteArray(androidJavaObject);
				}
				bool isReliable = message.Call<bool>("isReliable", new object[0]);
				string senderId = message.Call<string>("getSenderParticipantId", new object[0]);
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					listener.OnRealTimeMessageReceived(isReliable, senderId, messageData);
				});
			}
			message.Dispose();
		}

		private void OnSelectOpponentsResult(bool success, AndroidJavaObject opponents, bool hasAutoMatch, AndroidJavaObject autoMatchCriteria)
		{
			Logger.d("AndroidRtmpClient.OnSelectOpponentsResult, success=" + success);
			if (!this.CheckRtmpActive("OnSelectOpponentsResult"))
			{
				return;
			}
			this.mLaunchedExternalActivity = false;
			if (!success)
			{
				Logger.w("Room setup failed because select-opponents UI failed.");
				this.FailRoomSetup("Select opponents UI failed.");
				return;
			}
			this.mClient.CallClientApi("creating room w/ select-opponents result", delegate
			{
				Logger.d("Creating room via support lib's RtmpUtil.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("create", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					opponents,
					this.mVariant,
					(!hasAutoMatch) ? null : autoMatchCriteria,
					new AndroidRtmpClient.RoomUpdateProxy(this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this)
				});
			}, delegate(bool ok)
			{
				if (!ok)
				{
					this.FailRoomSetup("GoogleApiClient lost connection");
				}
			});
		}

		private void OnInvitationInboxResult(bool success, string invitationId)
		{
			Logger.d(string.Concat(new object[]
			{
				"AndroidRtmpClient.OnInvitationInboxResult, success=",
				success,
				", invitationId=",
				invitationId
			}));
			if (!this.CheckRtmpActive("OnInvitationInboxResult"))
			{
				return;
			}
			this.mLaunchedExternalActivity = false;
			if (!success || invitationId == null || invitationId.Length == 0)
			{
				Logger.w("Failed to setup room because invitation inbox UI failed.");
				this.FailRoomSetup("Invitation inbox UI failed.");
				return;
			}
			this.mClient.ClearInvitationIfFromNotification(invitationId);
			this.mClient.CallClientApi("accept invite from inbox", delegate
			{
				Logger.d("Accepting invite from inbox via support lib.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.RtmpUtils");
				@class.CallStatic("accept", new object[]
				{
					this.mClient.GHManager.GetApiClient(),
					invitationId,
					new AndroidRtmpClient.RoomUpdateProxy(this),
					new AndroidRtmpClient.RoomStatusUpdateProxy(this),
					new AndroidRtmpClient.RealTimeMessageReceivedProxy(this)
				});
			}, delegate(bool ok)
			{
				if (!ok)
				{
					this.FailRoomSetup("GoogleApiClient lost connection.");
				}
			});
		}

		private void DeliverRoomSetupProgressUpdate()
		{
			Logger.d("AndroidRtmpClient: DeliverRoomSetupProgressUpdate");
			if (!this.mRtmpActive || this.mRoom == null || this.mDeliveredRoomConnected)
			{
				return;
			}
			float progress = this.CalcRoomSetupPercentage();
			if (progress < this.mLastReportedProgress)
			{
				progress = this.mLastReportedProgress;
			}
			else
			{
				this.mLastReportedProgress = progress;
			}
			Logger.d("room setup progress: " + progress + "%");
			if (this.mRtmpListener != null)
			{
				Logger.d("Delivering progress to callback.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mRtmpListener.OnRoomSetupProgress(progress);
				});
			}
		}

		private float CalcRoomSetupPercentage()
		{
			if (!this.mRtmpActive || this.mRoom == null)
			{
				return 0f;
			}
			if (this.mDeliveredRoomConnected)
			{
				return 100f;
			}
			float num = this.mAccumulatedProgress;
			if (num > 50f)
			{
				num = 50f;
			}
			float num2 = 100f - num;
			int num3 = (this.mAllParticipants != null) ? this.mAllParticipants.Count : 0;
			int num4 = (this.mConnectedParticipants != null) ? this.mConnectedParticipants.Count : 0;
			if (num3 == 0)
			{
				return num;
			}
			return num + num2 * ((float)num4 / (float)num3);
		}

		internal void OnSignInSucceeded()
		{
		}
	}
}
