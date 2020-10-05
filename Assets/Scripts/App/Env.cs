using System;

namespace App
{
	public class Env
	{
		public enum Region
		{
			JAPAN,
			WORLDWIDE
		}

		public enum Language
		{
			JAPANESE,
			ENGLISH,
			CHINESE_ZHJ,
			CHINESE_ZH,
			KOREAN,
			FRENCH,
			GERMAN,
			SPANISH,
			PORTUGUESE,
			ITALIAN,
			RUSSIAN
		}

		public enum ActionServerType
		{
			LOCAL1,
			LOCAL2,
			LOCAL3,
			LOCAL4,
			LOCAL5,
			DEVELOP,
			DEVELOP2,
			DEVELOP3,
			STAGING,
			RELEASE,
			APPLICATION
		}

		public const bool isDebug = false;

		public const bool isDebugFont = false;

		public const bool forDevelop = false;

		private static readonly bool m_useAssetBundle = true;

		private static readonly bool m_releaseApplication;

		private static Env.Region mRegion;

		private static Env.Language mLanguage;

		private static Env.ActionServerType mActionServerType = Env.ActionServerType.RELEASE;

		public static bool useAssetBundle
		{
			get
			{
				return Env.m_useAssetBundle;
			}
		}

		public static bool isReleaseApplication
		{
			get
			{
				return Env.m_releaseApplication;
			}
		}

		public static Env.Region region
		{
			get
			{
				return Env.mRegion;
			}
			set
			{
				Env.mRegion = value;
			}
		}

		public static Env.Language language
		{
			get
			{
				return Env.mLanguage;
			}
			set
			{
				Env.mLanguage = value;
			}
		}

		public static Env.ActionServerType actionServerType
		{
			get
			{
				return Env.mActionServerType;
			}
			set
			{
				Env.mActionServerType = value;
			}
		}
	}
}
