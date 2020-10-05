using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerImageManager : MonoBehaviour
{
	private class PlayerImageData
	{
		private sealed class _DoFetchPlayerImages_c__IteratorBA : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal PlayerImageManager.PlayerImageData playerImageData;

			internal WWW _www___0;

			internal int _countCallback___1;

			internal int _i___2;

			internal Action<Texture2D> _callback___3;

			internal int _PC;

			internal object _current;

			internal PlayerImageManager.PlayerImageData ___playerImageData;

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
					this._www___0 = new WWW(this.playerImageData.PlayerImageUrl);
					this._current = this._www___0;
					this._PC = 1;
					return true;
				case 1u:
					if (this._www___0.texture != null)
					{
						this.playerImageData.PlayerImage = this._www___0.texture;
					}
					this.playerImageData.IsLoading = false;
					this.playerImageData.IsLoaded = true;
					this._countCallback___1 = this.playerImageData.CallbackList.Count;
					if (0 < this._countCallback___1)
					{
						this._i___2 = 0;
						while (this._i___2 < this._countCallback___1)
						{
							this._callback___3 = this.playerImageData.CallbackList[this._i___2];
							if (this._callback___3 != null)
							{
								this._callback___3(this.playerImageData.PlayerImage);
							}
							this._i___2++;
						}
					}
					this._www___0.Dispose();
					this._www___0 = null;
					this._PC = -1;
					break;
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

		private bool _IsLoading_k__BackingField;

		private bool _IsLoaded_k__BackingField;

		private string _PlayerId_k__BackingField;

		private string _PlayerImageUrl_k__BackingField;

		private Texture2D _PlayerImage_k__BackingField;

		private List<Action<Texture2D>> _CallbackList_k__BackingField;

		public bool IsLoading
		{
			get;
			set;
		}

		public bool IsLoaded
		{
			get;
			set;
		}

		public string PlayerId
		{
			get;
			set;
		}

		public string PlayerImageUrl
		{
			get;
			set;
		}

		public Texture2D PlayerImage
		{
			get;
			set;
		}

		public List<Action<Texture2D>> CallbackList
		{
			get;
			set;
		}

		public PlayerImageData()
		{
			this.IsLoading = false;
			this.IsLoaded = false;
			this.PlayerId = string.Empty;
			this.PlayerImageUrl = string.Empty;
			this.PlayerImage = null;
			this.CallbackList = new List<Action<Texture2D>>(10);
		}

		public static IEnumerator DoFetchPlayerImages(PlayerImageManager.PlayerImageData playerImageData)
		{
			PlayerImageManager.PlayerImageData._DoFetchPlayerImages_c__IteratorBA _DoFetchPlayerImages_c__IteratorBA = new PlayerImageManager.PlayerImageData._DoFetchPlayerImages_c__IteratorBA();
			_DoFetchPlayerImages_c__IteratorBA.playerImageData = playerImageData;
			_DoFetchPlayerImages_c__IteratorBA.___playerImageData = playerImageData;
			return _DoFetchPlayerImages_c__IteratorBA;
		}
	}

	private static PlayerImageManager mInstance;

	[SerializeField]
	private readonly int mMaxStorageCount = -1;

	[SerializeField]
	private Texture2D mDefaultPlayerImage;

	private Dictionary<string, PlayerImageManager.PlayerImageData> mPlayerImageDataDic;

	private Queue<PlayerImageManager.PlayerImageData> mPlayerImageQueue = new Queue<PlayerImageManager.PlayerImageData>();

	public static PlayerImageManager Instance
	{
		get
		{
			return PlayerImageManager.mInstance;
		}
	}

	private int MaxStorageCount
	{
		get
		{
			return this.mMaxStorageCount;
		}
	}

	private bool IsExistStorageLimit
	{
		get
		{
			return 0 < this.mMaxStorageCount;
		}
	}

	private void Awake()
	{
		if (PlayerImageManager.mInstance == null)
		{
			PlayerImageManager.mInstance = this;
			if (this.IsExistStorageLimit)
			{
				this.mPlayerImageDataDic = new Dictionary<string, PlayerImageManager.PlayerImageData>(this.MaxStorageCount);
			}
			else
			{
				this.mPlayerImageDataDic = new Dictionary<string, PlayerImageManager.PlayerImageData>();
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public bool IsLoaded(string playerId)
	{
		PlayerImageManager.PlayerImageData playerImageData;
		return this.mPlayerImageDataDic.TryGetValue(playerId, out playerImageData) && playerImageData.IsLoaded;
	}

	public bool IsLoading(string playerId)
	{
		PlayerImageManager.PlayerImageData playerImageData;
		return this.mPlayerImageDataDic.TryGetValue(playerId, out playerImageData) && playerImageData.IsLoading;
	}

	public bool Load(string playerId, string url, Action<Texture2D> callback)
	{
		bool result = false;
		if (playerId != null && string.Empty != playerId)
		{
			PlayerImageManager.PlayerImageData playerImageData;
			if (!this.mPlayerImageDataDic.TryGetValue(playerId, out playerImageData))
			{
				if (url != null && string.Empty != url)
				{
					playerImageData = new PlayerImageManager.PlayerImageData();
					playerImageData.PlayerId = playerId;
					playerImageData.PlayerImageUrl = url;
					playerImageData.PlayerImage = this.mDefaultPlayerImage;
					playerImageData.IsLoading = false;
					playerImageData.IsLoaded = false;
					if (callback != null)
					{
						playerImageData.CallbackList.Add(callback);
					}
					this.mPlayerImageDataDic.Add(playerId, playerImageData);
					this.mPlayerImageQueue.Enqueue(playerImageData);
					result = true;
				}
			}
			else if (!playerImageData.IsLoaded)
			{
				if (callback != null)
				{
					playerImageData.CallbackList.Add(callback);
				}
			}
			else
			{
				if (callback != null)
				{
					callback(playerImageData.PlayerImage);
				}
				result = true;
			}
		}
		return result;
	}

	public void Dispose(string playerId, bool is_removeList = true)
	{
		PlayerImageManager.PlayerImageData playerImageData;
		if (this.mPlayerImageDataDic.TryGetValue(playerId, out playerImageData))
		{
			if (playerImageData == null)
			{
				return;
			}
			if (playerImageData.CallbackList == null)
			{
				return;
			}
			foreach (Action<Texture2D> current in playerImageData.CallbackList)
			{
				if (current != null)
				{
					current(this.mDefaultPlayerImage);
				}
			}
			if (is_removeList)
			{
				this.mPlayerImageDataDic.Remove(playerId);
			}
			playerImageData.CallbackList.Clear();
		}
	}

	private void Update()
	{
		if (this.mPlayerImageQueue.Count <= 0)
		{
			return;
		}
		PlayerImageManager.PlayerImageData playerImageData = this.mPlayerImageQueue.Peek();
		if (playerImageData.IsLoaded)
		{
			this.mPlayerImageQueue.Dequeue();
		}
		else if (!playerImageData.IsLoading)
		{
			playerImageData.IsLoading = true;
			base.StartCoroutine(PlayerImageManager.PlayerImageData.DoFetchPlayerImages(playerImageData));
		}
	}

	public Texture2D GetPlayerImage(string playerId)
	{
		return this.GetPlayerImage(playerId, string.Empty, null);
	}

	public Texture2D GetPlayerImage(string playerId, string url, Action<Texture2D> callback)
	{
		if (this.Load(playerId, url, callback))
		{
			PlayerImageManager.PlayerImageData playerImageData = this.mPlayerImageDataDic[playerId];
			return playerImageData.PlayerImage;
		}
		return this.mDefaultPlayerImage;
	}

	public Texture2D GetDefaultImage()
	{
		return this.mDefaultPlayerImage;
	}

	public void ClearPlayerImage(string playerId, bool is_removeList = true)
	{
		this.Dispose(playerId, is_removeList);
	}

	public void ClearAllPlayerImage()
	{
		foreach (KeyValuePair<string, PlayerImageManager.PlayerImageData> current in this.mPlayerImageDataDic)
		{
			string playerId = current.Value.PlayerId;
			this.ClearPlayerImage(playerId, false);
		}
		this.mPlayerImageDataDic.Clear();
	}

	public static Texture2D GetPlayerDefaultImage()
	{
		Texture2D result = null;
		if (PlayerImageManager.mInstance != null)
		{
			result = PlayerImageManager.mInstance.GetDefaultImage();
		}
		return result;
	}
}
