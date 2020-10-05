using System;

public class AESCryptKey
{
	private static string KY_DEFINE = "u-4Z~jWARVUjkNSz";

	private static string IV_DEFINE = "Zb2*_.gj/uZ)@4hG9nAN,.H6Ew4n2N5e";

	private static string KY = AESCryptKey.KY_DEFINE;

	private static string IV = AESCryptKey.IV_DEFINE;

	public static string GetKY()
	{
		return AESCryptKey.KY;
	}

	public static string GetIV()
	{
		return AESCryptKey.IV;
	}
}
