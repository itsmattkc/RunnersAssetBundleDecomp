using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("CRIWARE/Error Handler")]
public class CriWareErrorHandler : MonoBehaviour
{
	public bool enableDebugPrintOnTerminal;

	public bool dontDestroyOnLoad = true;

	private static int initializationCount;

	private static string _errorMessage_k__BackingField;

	public static string errorMessage
	{
		get;
		set;
	}

	private void Awake()
	{
		CriWareErrorHandler.initializationCount++;
		if (CriWareErrorHandler.initializationCount != 1)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		CriWareErrorHandler.criWareUnity_Initialize();
		CriWareErrorHandler.criWareUnity_ControlLogOutput(this.enableDebugPrintOnTerminal);
		if (this.dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		}
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.enableDebugPrintOnTerminal)
		{
			CriWareErrorHandler.OutputErrorMessage();
		}
	}

	private void OnDestroy()
	{
		CriWareErrorHandler.initializationCount--;
		if (CriWareErrorHandler.initializationCount != 0)
		{
			return;
		}
		CriWareErrorHandler.criWareUnity_Finalize();
	}

	private static void OutputErrorMessage()
	{
		IntPtr intPtr = CriWareErrorHandler.criWareUnity_GetFirstError();
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		string text = Marshal.PtrToStringAnsi(intPtr);
		if (text != string.Empty)
		{
			CriWareErrorHandler.OutputLog(text);
			CriWareErrorHandler.criWareUnity_ResetError();
		}
		if (CriWareErrorHandler.errorMessage == null)
		{
			CriWareErrorHandler.errorMessage = string.Copy(text);
		}
	}

	private static void OutputLog(string errmsg)
	{
		if (errmsg == null)
		{
			return;
		}
		if (errmsg.StartsWith("E"))
		{
			UnityEngine.Debug.LogError("[CRIWARE] Error:" + errmsg);
		}
		else if (errmsg.StartsWith("W"))
		{
			UnityEngine.Debug.LogWarning("[CRIWARE] Warning:" + errmsg);
		}
		else
		{
			UnityEngine.Debug.Log("[CRIWARE]" + errmsg);
		}
	}

	[DllImport("cri_ware_unity")]
	private static extern void criWareUnity_Initialize();

	[DllImport("cri_ware_unity")]
	private static extern void criWareUnity_Finalize();

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criWareUnity_GetFirstError();

	[DllImport("cri_ware_unity")]
	private static extern void criWareUnity_ResetError();

	[DllImport("cri_ware_unity")]
	private static extern void criWareUnity_ControlLogOutput(bool sw);
}
