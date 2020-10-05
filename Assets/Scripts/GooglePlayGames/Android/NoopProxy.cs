using GooglePlayGames.OurUtils;
using System;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class NoopProxy : AndroidJavaProxy
	{
		private string mInterfaceClass;

		internal NoopProxy(string javaInterfaceClass) : base(javaInterfaceClass)
		{
			this.mInterfaceClass = javaInterfaceClass;
		}

		public override AndroidJavaObject Invoke(string methodName, object[] args)
		{
			Logger.d("NoopProxy for " + this.mInterfaceClass + " got call to " + methodName);
			return null;
		}

		public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
		{
			Logger.d("NoopProxy for " + this.mInterfaceClass + " got call to " + methodName);
			return null;
		}
	}
}
