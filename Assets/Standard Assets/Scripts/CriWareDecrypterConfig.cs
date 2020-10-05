using System;

[Serializable]
public class CriWareDecrypterConfig
{
	public string key = string.Empty;

	public string authenticationFile = string.Empty;

	public bool enableAtomDecryption = true;

	public bool enableManaDecryption = true;
}
