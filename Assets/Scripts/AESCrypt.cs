using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AESCrypt
{
	private static PaddingMode mPaddingMode = PaddingMode.Zeros;

	private static CipherMode mCipherMode = CipherMode.CBC;

	private static int mKeySize = 128;

	private static int mBlockSize = 256;

	public static bool HaveKey()
	{
		string kY = AESCryptKey.GetKY();
		return !kY.Equals(string.Empty);
	}

	private static RijndaelManaged _init(ref byte[] kyb, ref byte[] ivb)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Padding = AESCrypt.mPaddingMode;
		rijndaelManaged.Mode = AESCrypt.mCipherMode;
		rijndaelManaged.KeySize = AESCrypt.mKeySize;
		rijndaelManaged.BlockSize = AESCrypt.mBlockSize;
		string kY = AESCryptKey.GetKY();
		string iV = AESCryptKey.GetIV();
		kyb = Encoding.UTF8.GetBytes(kY);
		ivb = Encoding.UTF8.GetBytes(iV);
		return rijndaelManaged;
	}

	public static string Encrypt(int iDeInt)
	{
		return AESCrypt.Encrypt(iDeInt.ToString());
	}

	public static string Encrypt(float iDeFloat)
	{
		return AESCrypt.Encrypt(iDeFloat.ToString());
	}

	public static string Encrypt(string iDeText)
	{
		if (!AESCrypt.HaveKey())
		{
			return iDeText;
		}
		string result;
		try
		{
			byte[] rgbKey = null;
			byte[] rgbIV = null;
			using (RijndaelManaged rijndaelManaged = AESCrypt._init(ref rgbKey, ref rgbIV))
			{
				ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rgbKey, rgbIV);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						byte[] bytes = Encoding.UTF8.GetBytes(iDeText);
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
						byte[] inArray = memoryStream.ToArray();
						result = Convert.ToBase64String(inArray);
					}
				}
			}
		}
		catch
		{
			result = iDeText;
		}
		return result;
	}

	public static string Decrypt(string iEnText)
	{
		if (!AESCrypt.HaveKey())
		{
			return iEnText;
		}
		string result;
		try
		{
			byte[] rgbKey = null;
			byte[] rgbIV = null;
			using (RijndaelManaged rijndaelManaged = AESCrypt._init(ref rgbKey, ref rgbIV))
			{
				ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rgbKey, rgbIV);
				byte[] array = Convert.FromBase64String(iEnText);
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						byte[] array2 = new byte[array.Length];
						cryptoStream.Read(array2, 0, array2.Length);
						result = Encoding.UTF8.GetString(array2).Replace("\0", string.Empty);
					}
				}
			}
		}
		catch
		{
			result = iEnText;
		}
		return result;
	}
}
