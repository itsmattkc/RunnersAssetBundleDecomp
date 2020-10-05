using System;
using UnityEngine;

public class BindingAndroid : BindingPlugin
{
	public const string PackageNameRelease = "com.sega.sonicrunners";

	private AndroidJavaObject Billing;

	private AndroidJavaObject GCM;

	private AndroidJavaObject Env;

	public BindingAndroid()
	{
		this.Billing = new AndroidJavaObject(BindingAndroid.GetPackageName() + ".billing.BillingManager", new object[0]);
		this.GCM = new AndroidJavaObject(BindingAndroid.GetPackageName() + ".gcm.GCMManager", new object[0]);
		this.Env = new AndroidJavaObject(BindingAndroid.GetPackageName() + ".EnvManager", new object[0]);
	}

	public override void Initialize()
	{
	}

	public override void Review(string defaultComment)
	{
	}

	public override void CreateInAppPurchase(string delegator)
	{
	}

	public override void ClearProductsIdentifier()
	{
	}

	public override void AddProductsIdentifier(string productsId)
	{
	}

	public override void RequestProductsInfo()
	{
	}

	public override string GetProductInfoPrice(string productsId)
	{
		if (this.Billing == null)
		{
			return null;
		}
		return this.Billing.Call<string>("getProductPrice", new object[]
		{
			productsId
		});
	}

	public override string GetProductInfoTitle(string productsId)
	{
		return null;
	}

	public override void BuyProduct(string productsId)
	{
		if (this.Billing != null)
		{
			this.Billing.Call("startPurchase", new object[]
			{
				productsId
			});
		}
	}

	public override bool CanMakePayments()
	{
		return this.Billing != null && this.Billing.Call<bool>("canMakePayments", new object[0]);
	}

	public override string GetPurchasedTransaction()
	{
		return null;
	}

	public override string GetProductIdentifier(string transactionId)
	{
		return null;
	}

	public override string GetTransactionReceipt(string transactionId)
	{
		return null;
	}

	public override void FinishTransaction(string transactionId)
	{
		AndroidJavaClass androidJavaClass = null;
		AndroidJavaObject androidJavaObject = null;
		androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		try
		{
			if (androidJavaClass != null)
			{
				androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (androidJavaObject != null)
				{
					if (this.Billing != null)
					{
						androidJavaObject.Call("runOnUiThread", new object[]
						{
							new AndroidJavaRunnable(delegate
							{
								this.Billing.Call("consumePurchase", new object[0]);
							})
						});
					}
					androidJavaObject.Dispose();
				}
				androidJavaClass.Dispose();
			}
		}
		catch (Exception var_2_63)
		{
			if (androidJavaClass != null)
			{
				androidJavaClass.Dispose();
			}
			if (androidJavaObject != null)
			{
				androidJavaObject.Dispose();
			}
		}
	}

	public override void ResetPaymentQueueDelegate()
	{
	}

	public override void ClearIconBadgeNumber()
	{
	}

	public override string GetNoticeRegistrationId()
	{
		if (this.GCM != null)
		{
			return this.GCM.Call<string>("getGcmRegistrationId", new object[0]);
		}
		return null;
	}

	public override void RegistPnote(string guid)
	{
		if (this.GCM != null)
		{
			this.GCM.Call("registPnote", new object[]
			{
				guid
			});
		}
	}

	public override void UnregistPnote()
	{
		if (this.GCM != null)
		{
			this.GCM.Call("unregistPnote", new object[0]);
		}
	}

	public override void SendMessagePnote(string message, string sender, string reciever, string launchOption)
	{
		if (this.GCM != null)
		{
			this.GCM.Call("sendMessage", new object[]
			{
				message,
				sender,
				reciever,
				launchOption
			});
		}
	}

	public override void RegistTagsPnote(string tags, string guid)
	{
		if (this.GCM != null)
		{
			this.GCM.Call("registTags", new object[]
			{
				tags,
				guid
			});
		}
	}

	public override string GetUrlSchemeStr()
	{
		if (this.Env != null)
		{
			return this.Env.Call<string>("getAtomScheme", new object[0]);
		}
		return null;
	}

	public override void ClearUrlSchemeStr()
	{
		if (this.Env != null)
		{
			this.Env.Call("clearAtomScheme", new object[0]);
		}
	}

	public override string GetPnoteLaunchString()
	{
		if (this.GCM != null)
		{
			return this.GCM.Call<string>("getLaunchString", new object[0]);
		}
		return null;
	}

	public static string GetPackageName()
	{
		return "com.sega.sonicrunners";
	}

	public override void GetSystemProxy(out string host, out ushort port)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.lang.System"))
		{
			host = androidJavaClass.CallStatic<string>("getProperty", new object[]
			{
				"http.proxyHost"
			});
			port = Convert.ToUInt16(androidJavaClass.CallStatic<string>("getProperty", new object[]
			{
				"http.proxyPort"
			}));
			androidJavaClass.Dispose();
		}
	}

	public override void SetClipBoard(string text)
	{
		if (this.GCM != null)
		{
			this.GCM.Call("setTextClipBoard", new object[]
			{
				text
			});
		}
	}
}
