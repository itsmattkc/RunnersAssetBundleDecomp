using System;
using System.Runtime.InteropServices;

public static class CriFsUtility
{
	public const int DefaultReadUnitSize = 1048576;

	public static CriFsLoadFileRequest LoadFile(string path, int readUnitSize = 1048576)
	{
		return CriFsServer.instance.LoadFile(null, path, null, readUnitSize);
	}

	public static CriFsLoadFileRequest LoadFile(string path, CriFsRequest.DoneDelegate doneDelegate, int readUnitSize = 1048576)
	{
		return CriFsServer.instance.LoadFile(null, path, doneDelegate, readUnitSize);
	}

	public static CriFsLoadFileRequest LoadFile(CriFsBinder binder, string path, int readUnitSize = 1048576)
	{
		return CriFsServer.instance.LoadFile(binder, path, null, readUnitSize);
	}

	public static CriFsLoadAssetBundleRequest LoadAssetBundle(string path, int readUnitSize = 1048576)
	{
		return CriFsUtility.LoadAssetBundle(null, path, readUnitSize);
	}

	public static CriFsLoadAssetBundleRequest LoadAssetBundle(CriFsBinder binder, string path, int readUnitSize = 1048576)
	{
		return CriFsServer.instance.LoadAssetBundle(binder, path, readUnitSize);
	}

	public static CriFsInstallRequest Install(string srcPath, string dstPath)
	{
		return CriFsUtility.Install(null, srcPath, dstPath, null);
	}

	public static CriFsInstallRequest Install(string srcPath, string dstPath, CriFsRequest.DoneDelegate doneDeleagate)
	{
		return CriFsUtility.Install(null, srcPath, dstPath, doneDeleagate);
	}

	public static CriFsInstallRequest Install(CriFsBinder srcBinder, string srcPath, string dstPath)
	{
		return CriFsServer.instance.Install(srcBinder, srcPath, dstPath, null);
	}

	public static CriFsInstallRequest Install(CriFsBinder srcBinder, string srcPath, string dstPath, CriFsRequest.DoneDelegate doneDeleagate)
	{
		return CriFsServer.instance.Install(srcBinder, srcPath, dstPath, doneDeleagate);
	}

	public static CriFsBindRequest BindCpk(CriFsBinder targetBinder, string srcPath)
	{
		return CriFsUtility.BindCpk(targetBinder, null, srcPath);
	}

	public static CriFsBindRequest BindCpk(CriFsBinder targetBinder, CriFsBinder srcBinder, string srcPath)
	{
		return CriFsServer.instance.BindCpk(targetBinder, srcBinder, srcPath);
	}

	public static CriFsBindRequest BindDirectory(CriFsBinder targetBinder, string srcPath)
	{
		return CriFsServer.instance.BindDirectory(targetBinder, null, srcPath);
	}

	public static CriFsBindRequest BindDirectory(CriFsBinder targetBinder, CriFsBinder srcBinder, string srcPath)
	{
		return CriFsServer.instance.BindDirectory(targetBinder, srcBinder, srcPath);
	}

	public static CriFsBindRequest BindFile(CriFsBinder targetBinder, string srcPath)
	{
		return CriFsServer.instance.BindFile(targetBinder, null, srcPath);
	}

	public static CriFsBindRequest BindFile(CriFsBinder targetBinder, CriFsBinder srcBinder, string srcPath)
	{
		return CriFsServer.instance.BindFile(targetBinder, srcBinder, srcPath);
	}

	public static void SetUserAgentString(string userAgentString)
	{
		CriFsPlugin.InitializeLibrary();
		CriFsUtility.criFsUnity_SetUserAgentString(userAgentString);
	}

	public static void SetProxyServer(string proxyPath, ushort proxyPort)
	{
		CriFsPlugin.InitializeLibrary();
		CriFsUtility.criFsUnity_SetProxyServer(proxyPath, proxyPort);
	}

	[DllImport("cri_ware_unity")]
	private static extern bool criFsUnity_SetUserAgentString(string userAgentString);

	[DllImport("cri_ware_unity")]
	private static extern bool criFsUnity_SetProxyServer(string proxyPath, ushort proxyPort);
}
