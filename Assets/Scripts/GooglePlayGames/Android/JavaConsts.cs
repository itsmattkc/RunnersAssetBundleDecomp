using System;

namespace GooglePlayGames.Android
{
	internal class JavaConsts
	{
		public const int GAMEHELPER_CLIENT_ALL = 7;

		public const int STATE_HIDDEN = 2;

		public const int STATE_REVEALED = 1;

		public const int STATE_UNLOCKED = 0;

		public const int TYPE_INCREMENTAL = 1;

		public const int TYPE_STANDARD = 0;

		public const int STATUS_OK = 0;

		public const int STATUS_STALE_DATA = 3;

		public const int STATUS_NO_DATA = 4;

		public const int STATUS_DEFERRED = 5;

		public const int STATUS_KEY_NOT_FOUND = 2002;

		public const int STATUS_CONFLICT = 2000;

		public const int SDK_VARIANT = 37143;

		public const string GmsPkg = "com.google.android.gms";

		public const string ResultCallbackClass = "com.google.android.gms.common.api.ResultCallback";

		public const string RoomStatusUpdateListenerClass = "com.google.android.gms.games.multiplayer.realtime.RoomStatusUpdateListener";

		public const string RoomUpdateListenerClass = "com.google.android.gms.games.multiplayer.realtime.RoomUpdateListener";

		public const string RealTimeMessageReceivedListenerClass = "com.google.android.gms.games.multiplayer.realtime.RealTimeMessageReceivedListener";

		public const string OnInvitationReceivedListenerClass = "com.google.android.gms.games.multiplayer.OnInvitationReceivedListener";

		public const string ParticipantResultClass = "com.google.android.gms.games.multiplayer.ParticipantResult";

		public const string PluginSupportPkg = "com.google.example.games.pluginsupport";

		public const string SupportRtmpUtilsClass = "com.google.example.games.pluginsupport.RtmpUtils";

		public const string SupportTbmpUtilsClass = "com.google.example.games.pluginsupport.TbmpUtils";

		public const string SupportSelectOpponentsHelperActivity = "com.google.example.games.pluginsupport.SelectOpponentsHelperActivity";

		public const string SupportSelectOpponentsHelperActivityListener = "com.google.example.games.pluginsupport.SelectOpponentsHelperActivity$Listener";

		public const string SupportInvitationInboxHelperActivity = "com.google.example.games.pluginsupport.InvitationInboxHelperActivity";

		public const string SupportInvitationInboxHelperActivityListener = "com.google.example.games.pluginsupport.InvitationInboxHelperActivity$Listener";

		public const string SignInHelperManagerClass = "com.google.example.games.pluginsupport.SignInHelperManager";

		public const int STATUS_NOT_INVITED_YET = 0;

		public const int STATUS_INVITED = 1;

		public const int STATUS_JOINED = 2;

		public const int STATUS_DECLINED = 3;

		public const int STATUS_LEFT = 4;

		public const int STATUS_FINISHED = 5;

		public const int STATUS_UNRESPONSIVE = 6;

		public const int INVITATION_TYPE_REAL_TIME = 0;

		public const int INVITATION_TYPE_TURN_BASED = 1;

		public const int MATCH_STATUS_AUTO_MATCHING = 0;

		public const int MATCH_STATUS_ACTIVE = 1;

		public const int MATCH_STATUS_COMPLETE = 2;

		public const int MATCH_STATUS_EXPIRED = 3;

		public const int MATCH_STATUS_CANCELED = 4;

		public const int MATCH_TURN_STATUS_INVITED = 0;

		public const int MATCH_TURN_STATUS_MY_TURN = 1;

		public const int MATCH_TURN_STATUS_THEIR_TURN = 2;

		public const int MATCH_TURN_STATUS_COMPLETE = 3;

		public const int MATCH_VARIANT_ANY = -1;

		public const int MATCH_RESULT_UNINITIALIZED = -1;

		public const int PLACING_UNINITIALIZED = -1;

		public const int MATCH_RESULT_WIN = 0;

		public const int MATCH_RESULT_LOSS = 1;

		public const int MATCH_RESULT_TIE = 2;

		public const int MATCH_RESULT_NONE = 3;

		public const int MATCH_RESULT_DISCONNECT = 4;

		public const int MATCH_RESULT_DISAGREED = 5;
	}
}
