using System;
using UnityEngine;

public class Binding
{
	private static BindingPlugin bindingInstance = null;

	public static readonly Binding Instance = new Binding();

	private Binding()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			Binding.bindingInstance = new BindingAndroid();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		}
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.Initialize();
		}
	}

	public void Review(string defaultComment)
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.Review(defaultComment);
	}

	public void CreateInAppPurchase(string delegator)
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.CreateInAppPurchase(delegator);
	}

	public void ClearProductsIdentifier()
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.ClearProductsIdentifier();
	}

	public void AddProductsIdentifier(string productsId)
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.AddProductsIdentifier(productsId);
	}

	public void RequestProductsInfo()
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.RequestProductsInfo();
	}

	public string GetProductInfoPrice(string productsId)
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetProductInfoPrice(productsId);
	}

	public string GetProductInfoTitle(string productsId)
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetProductInfoTitle(productsId);
	}

	public void BuyProduct(string productsId)
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.BuyProduct(productsId);
	}

	public bool CanMakePayments()
	{
		return Binding.bindingInstance != null && Binding.bindingInstance.CanMakePayments();
	}

	public string GetPurchasedTransaction()
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetPurchasedTransaction();
	}

	public string GetProductIdentifier(string transactionId)
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetProductIdentifier(transactionId);
	}

	public string GetTransactionReceipt(string transactionId)
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetTransactionReceipt(transactionId);
	}

	public void FinishTransaction(string transactionId)
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.FinishTransaction(transactionId);
	}

	public void ResetPaymentQueueDelegate()
	{
		if (Binding.bindingInstance == null)
		{
			return;
		}
		Binding.bindingInstance.ResetPaymentQueueDelegate();
	}

	public string GetNoticeRegistrationId()
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetNoticeRegistrationId();
	}

	public string GetUrlSchemeStr()
	{
		if (Binding.bindingInstance == null)
		{
			return null;
		}
		return Binding.bindingInstance.GetUrlSchemeStr();
	}

	public void ClearUrlSchemeStr()
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.ClearUrlSchemeStr();
		}
	}

	public void RegistPnote(string guid)
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.RegistPnote(guid);
		}
	}

	public void UnregistPnote()
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.UnregistPnote();
		}
	}

	public void SendMessagePnote(string message, string sender, string reciever, string launchOption)
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.SendMessagePnote(message, sender, reciever, launchOption);
		}
	}

	public void RegistTagsPnote(string tags, string guid)
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.RegistTagsPnote(tags, guid);
		}
	}

	public string GetPnoteLaunchString()
	{
		if (Binding.bindingInstance != null)
		{
			return Binding.bindingInstance.GetPnoteLaunchString();
		}
		return null;
	}

	public void ClearIconBadgeNumber()
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.ClearIconBadgeNumber();
		}
	}

	public void GetSystemProxy(out string host, out ushort port)
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.GetSystemProxy(out host, out port);
		}
		else
		{
			host = null;
			port = 0;
		}
	}

	public void SetClipBoard(string text)
	{
		if (Binding.bindingInstance != null)
		{
			Binding.bindingInstance.SetClipBoard(text);
		}
	}
}
