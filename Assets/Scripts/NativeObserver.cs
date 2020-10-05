using Message;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NativeObserver : MonoBehaviour
{
	public enum IAPResult
	{
		ProductsRequestCompleted,
		ProductsRequestInvalid,
		PaymentTransactionPurchasing,
		PaymentTransactionPurchased,
		PaymentTransactionFailed,
		PaymentTransactionRestored
	}

	public enum FailStatus
	{
		Disable,
		AppStoreFailed,
		ServerFailed,
		Deferred
	}

	public delegate void IAPDelegate(NativeObserver.IAPResult result);

	public delegate void PurchaseSuccessCallback(int scValue);

	public delegate void PurchaseFailedCallback(NativeObserver.FailStatus status);

	private sealed class _GetPriceAsync_c__IteratorBB : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _index___0;

		internal List<string> productList;

		internal string _productName___1;

		internal DateTime _startTime___2;

		internal string _price___3;

		internal DateTime _currentTime___4;

		internal TimeSpan _timeSpan___5;

		internal int _PC;

		internal object _current;

		internal List<string> ___productList;

		internal NativeObserver __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._index___0 = 0;
				goto IL_EE;
			case 1u:
				break;
			default:
				return false;
			}
			IL_5F:
			this._price___3 = this.__f__this.GetProductPrice(this._productName___1);
			if (string.IsNullOrEmpty(this._price___3))
			{
				this._currentTime___4 = DateTime.Now;
				this._timeSpan___5 = this._currentTime___4 - this._startTime___2;
				if ((float)this._timeSpan___5.Seconds < 10f)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
			}
			IL_E0:
			this._index___0++;
			IL_EE:
			if (this._index___0 >= this.productList.Count)
			{
				this.__f__this.productInfoEnable = true;
				if (this.__f__this.iapDelegate != null)
				{
					this.__f__this.iapDelegate(NativeObserver.IAPResult.ProductsRequestCompleted);
				}
				this._PC = -1;
			}
			else
			{
				this._productName___1 = this.productList[this._index___0];
				if (this._productName___1 == null)
				{
					goto IL_E0;
				}
				this._startTime___2 = DateTime.Now;
				goto IL_5F;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private NativeObserver.IAPDelegate iapDelegate;

	private bool isIAPurchasing;

	private NativeObserver.PurchaseSuccessCallback purchaseSuccessCallback;

	private NativeObserver.PurchaseFailedCallback purchaseFailedCallback;

	private Action purchaseCancelCallback;

	private bool initialized;

	private bool productInfoEnable;

	private string consumingProductId = string.Empty;

	private string m_processingReceipt = string.Empty;

	private static NativeObserver instance;

	private static bool _IsBusy_k__BackingField;

	public static int ProductCount
	{
		get
		{
			return ServerInterface.RedStarItemList.Count;
		}
	}

	public static NativeObserver Instance
	{
		get
		{
			if (NativeObserver.instance != null && !NativeObserver.instance.initialized)
			{
				NativeObserver.instance.StartInAppPurchase();
			}
			return NativeObserver.instance;
		}
		private set
		{
		}
	}

	public static bool IsBusy
	{
		get;
		set;
	}

	public string GetProductName(int productIndex)
	{
		if (productIndex >= NativeObserver.ProductCount)
		{
			return null;
		}
		return ServerInterface.RedStarItemList[productIndex].m_productId;
	}

	private void Awake()
	{
		if (NativeObserver.instance == null)
		{
			NativeObserver.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public string GetProductPrice(string productId)
	{
		return Binding.Instance.GetProductInfoPrice(productId);
	}

	public void StartInAppPurchase()
	{
		Binding.Instance.CreateInAppPurchase("Observer");
		this.initialized = true;
	}

	public void CheckCurrentTransaction()
	{
		this.Log("NativeObserver : CheckCurrentTransaction");
		Binding.Instance.FinishTransaction(string.Empty);
		string[] array = this.ReciptGet();
		if (array != null)
		{
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (text != null)
				{
					global::Debug.Log("receipts=" + text);
				}
			}
		}
		if (array != null && array.Length > 0)
		{
			this._StartBuyFlow(array[0]);
		}
	}

	public void RequestProductsInfo(List<string> productList, NativeObserver.IAPDelegate del)
	{
		if (this.productInfoEnable)
		{
			if (del != null)
			{
				del(NativeObserver.IAPResult.ProductsRequestCompleted);
			}
			return;
		}
		this.iapDelegate = del;
		if (this.isIAPurchasing)
		{
			if (this.iapDelegate != null)
			{
				this.iapDelegate(NativeObserver.IAPResult.PaymentTransactionFailed);
			}
			return;
		}
		base.StartCoroutine(this.GetPriceAsync(productList));
	}

	public void ResetIapDelegate()
	{
		this.iapDelegate = null;
	}

	public void BuyProduct(string productId, NativeObserver.PurchaseSuccessCallback successCallback, NativeObserver.PurchaseFailedCallback failCallback, Action cancelCallback)
	{
		if (this.isIAPurchasing)
		{
			if (failCallback != null)
			{
				failCallback(NativeObserver.FailStatus.Disable);
			}
			return;
		}
		NetMonitor netMonitor = NetMonitor.Instance;
		if (netMonitor != null)
		{
			netMonitor.StartMonitor(null);
		}
		Binding.Instance.BuyProduct(productId);
		this.purchaseSuccessCallback = successCallback;
		this.purchaseFailedCallback = failCallback;
		this.purchaseCancelCallback = cancelCallback;
	}

	public void OnBeforePurchaseFinishedSuccess(string message)
	{
		this.Log("NativeObserver : OnBeforePurchaseFinishedSuccess :" + message);
		this.ReciptPush(message);
	}

	public void OnPurchaseFinishedSuccess(string message)
	{
		this.Log("NativeObserver : OnPurchaseFinishedSuccess :" + message);
		NetMonitor netMonitor = NetMonitor.Instance;
		if (netMonitor != null)
		{
			netMonitor.EndMonitorForward(null, null, null);
			netMonitor.EndMonitorBackward();
		}
		this._StartBuyFlow(message);
	}

	private void _StartBuyFlow(string message)
	{
		string[] array = message.Split(new char[]
		{
			','
		});
		if (array == null || array.Length < 3)
		{
			global::Debug.Log("NativeObserver._StartBuyFlow:no Reciept or invalid Reciept");
			if (this.purchaseCancelCallback != null)
			{
				this.purchaseCancelCallback();
			}
			this.ClearCallback();
			this.OnStopBusy();
		}
		else
		{
			global::Debug.Log("NativeObserver._StartBuyFlow:Retry send reciept");
			this.ExecCpChargeBuyCommit(array[0], WWW.UnEscapeURL(array[1]), WWW.UnEscapeURL(array[2]), message);
			this.OnStopBusy();
		}
	}

	public void OnPurchaseFinishedCancel(string message)
	{
		this.Log("NativeObserver : OnPurchaseFinishedCancel :" + message);
		if (this.purchaseCancelCallback != null)
		{
			this.purchaseCancelCallback();
		}
		this.ClearCallback();
		this.OnStopBusy();
		NetMonitor netMonitor = NetMonitor.Instance;
		if (netMonitor != null)
		{
			netMonitor.EndMonitorForward(null, null, null);
			netMonitor.EndMonitorBackward();
		}
	}

	public void OnPurchaseFinishedFailed(string message)
	{
		this.Log("NativeObserver : OnPurchaseFinishedFailed :" + message);
		if (this.purchaseFailedCallback != null)
		{
			this.purchaseFailedCallback(NativeObserver.FailStatus.AppStoreFailed);
		}
		this.ClearCallback();
		this.OnStopBusy();
		NetMonitor netMonitor = NetMonitor.Instance;
		if (netMonitor != null)
		{
			netMonitor.EndMonitorForward(null, null, null);
			netMonitor.EndMonitorBackward();
		}
	}

	private void ExecCpChargeBuyCommit(string productId, string json, string sign, string message)
	{
		this.Log(string.Format("NativeObserver : ExecCpChargeBuyCommit :{0} , {1}, {2}", productId, json, sign));
		this.m_processingReceipt = message;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			this.consumingProductId = productId;
			loggedInServerInterface.RequestServerBuyAndroid(json, sign, base.gameObject);
		}
	}

	private void OnStartBusy()
	{
		global::Debug.Log("NativeObserver : StartBusy");
		this.isIAPurchasing = true;
	}

	private void OnStopBusy()
	{
		global::Debug.Log("NativeObserver : StopBusy");
		this.isIAPurchasing = false;
	}

	private void Log(string log)
	{
		global::Debug.Log(log);
	}

	private void ClearCallback()
	{
		this.purchaseSuccessCallback = null;
		this.purchaseFailedCallback = null;
		this.purchaseCancelCallback = null;
	}

	private void ReciptPush(string recipt)
	{
		SystemSaveManager systemSaveManager = SystemSaveManager.Instance;
		if (systemSaveManager != null)
		{
			SystemData systemdata = systemSaveManager.GetSystemdata();
			if (systemdata != null)
			{
				string purchasedRecipt = systemdata.purchasedRecipt;
				if (string.IsNullOrEmpty(purchasedRecipt))
				{
					systemdata.purchasedRecipt = recipt;
				}
				else
				{
					systemdata.purchasedRecipt = purchasedRecipt + "@" + recipt;
				}
				systemSaveManager.SaveSystemData();
				global::Debug.Log("NativeObserver.ReciptPush:" + systemdata.purchasedRecipt);
			}
		}
	}

	private string[] ReciptGet()
	{
		SystemSaveManager systemSaveManager = SystemSaveManager.Instance;
		if (systemSaveManager != null)
		{
			SystemData systemdata = systemSaveManager.GetSystemdata();
			if (systemdata != null)
			{
				string purchasedRecipt = systemdata.purchasedRecipt;
				global::Debug.Log("NativeObserver ReciptGet: " + purchasedRecipt);
				if (!string.IsNullOrEmpty(purchasedRecipt))
				{
					return purchasedRecipt.Split(new char[]
					{
						'@'
					});
				}
			}
		}
		return null;
	}

	private void ReciptDelete(string recipt)
	{
		SystemSaveManager systemSaveManager = SystemSaveManager.Instance;
		if (systemSaveManager != null)
		{
			SystemData systemdata = systemSaveManager.GetSystemdata();
			if (systemdata != null)
			{
				string purchasedRecipt = systemdata.purchasedRecipt;
				if (string.IsNullOrEmpty(purchasedRecipt))
				{
					return;
				}
				string[] array = purchasedRecipt.Split(new char[]
				{
					'@'
				});
				string text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					if (!(array[i] == recipt))
					{
						if (text.Length > 0)
						{
							text += "@";
						}
						text += array[i];
					}
				}
				if (text.Length > 0)
				{
					systemdata.purchasedRecipt = text;
					systemSaveManager.SaveSystemData();
				}
				else
				{
					systemdata.purchasedRecipt = text;
					systemSaveManager.SaveSystemData();
				}
			}
		}
	}

	private IEnumerator GetPriceAsync(List<string> productList)
	{
		NativeObserver._GetPriceAsync_c__IteratorBB _GetPriceAsync_c__IteratorBB = new NativeObserver._GetPriceAsync_c__IteratorBB();
		_GetPriceAsync_c__IteratorBB.productList = productList;
		_GetPriceAsync_c__IteratorBB.___productList = productList;
		_GetPriceAsync_c__IteratorBB.__f__this = this;
		return _GetPriceAsync_c__IteratorBB;
	}

	private void ServerBuyAndroid_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.ReciptDelete(this.m_processingReceipt);
		int rsrType = 0;
		for (int i = 0; i < NativeObserver.ProductCount; i++)
		{
			string productName = this.GetProductName(i);
			if (productName == this.consumingProductId)
			{
				rsrType = i;
				break;
			}
		}
		FoxManager.SendLtvPointBuyRSR(rsrType);
		this.consumingProductId = string.Empty;
		if (this.purchaseSuccessCallback != null)
		{
			this.purchaseSuccessCallback(0);
		}
	}

	private void ServerBuyAndroid_Failed(MsgServerConnctFailed msg)
	{
		if (msg.m_status == ServerInterface.StatusCode.AlreadyProcessedReceipt)
		{
			int rsrType = 0;
			for (int i = 0; i < NativeObserver.ProductCount; i++)
			{
				string productName = this.GetProductName(i);
				if (productName == this.consumingProductId)
				{
					rsrType = i;
					break;
				}
			}
			FoxManager.SendLtvPointBuyRSR(rsrType);
			this.ReciptDelete(this.m_processingReceipt);
		}
		if (this.purchaseFailedCallback != null)
		{
			this.purchaseFailedCallback(NativeObserver.FailStatus.ServerFailed);
		}
	}
}
