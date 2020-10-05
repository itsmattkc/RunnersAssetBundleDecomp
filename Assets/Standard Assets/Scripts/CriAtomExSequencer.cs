using System;
using System.Runtime.InteropServices;

public static class CriAtomExSequencer
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void EventCbFunc(string eventParamsString);

	public static void SetEventCallback(CriAtomExSequencer.EventCbFunc func, string separator = "\t")
	{
		CriAtom.SetEventCallback(func, separator);
	}
}
