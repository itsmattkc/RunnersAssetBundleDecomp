using App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class InformationImageManager : MonoBehaviour
{
	private class ImageData
	{
		private sealed class _DoFetchImages_c__Iterator5A : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal InformationImageManager.ImageData imageData;

			internal WWW _www___0;

			internal int _countCallback___1;

			internal int _index___2;

			internal Action<Texture2D> _callback___3;

			internal int _PC;

			internal object _current;

			internal InformationImageManager.ImageData ___imageData;

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
					this._www___0 = new WWW(this.imageData.ImageUrl);
					this._current = this._www___0;
					this._PC = 1;
					return true;
				case 1u:
					this.imageData.IsLoading = false;
					this.imageData.IsLoaded = true;
					if (string.IsNullOrEmpty(this._www___0.error) && this._www___0.texture != null)
					{
						this.imageData.Image = this._www___0.texture;
						this._countCallback___1 = this.imageData.CallbackList.Count;
						if (this._countCallback___1 > 0)
						{
							this._index___2 = 0;
							while (this._index___2 < this._countCallback___1)
							{
								this._callback___3 = this.imageData.CallbackList[this._index___2];
								if (this._callback___3 != null)
								{
									this._callback___3(this.imageData.Image);
								}
								this._index___2++;
							}
						}
						InformationImageManager.SaveImageData(this._www___0, this.imageData.Image, this.imageData.ImagePath, this.imageData.ImageEtagPath);
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

		private sealed class _DoFetchCashImages_c__Iterator5B : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal Dictionary<string, string> _headers___0;

			internal string lastModified;

			internal string etag;

			internal InformationImageManager.ImageData imageData;

			internal WWW _www___1;

			internal int _countCallback___2;

			internal int _index___3;

			internal Action<Texture2D> _callback___4;

			internal int _PC;

			internal object _current;

			internal string ___lastModified;

			internal string ___etag;

			internal InformationImageManager.ImageData ___imageData;

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
					this._headers___0 = new Dictionary<string, string>();
					this._headers___0["If-Modified-Since"] = this.lastModified;
					this._headers___0["ETAG"] = this.etag;
					this._www___1 = new WWW(this.imageData.ImageUrl, null, this._headers___0);
					this._current = this._www___1;
					this._PC = 1;
					return true;
				case 1u:
					if (string.IsNullOrEmpty(this._www___1.error) && this._www___1.texture != null && this._www___1.size > 0)
					{
						this.imageData.IsLoading = false;
						this.imageData.IsLoaded = true;
						this.imageData.Image = this._www___1.texture;
						this._countCallback___2 = this.imageData.CallbackList.Count;
						if (this._countCallback___2 > 0)
						{
							this._index___3 = 0;
							while (this._index___3 < this._countCallback___2)
							{
								this._callback___4 = this.imageData.CallbackList[this._index___3];
								if (this._callback___4 != null)
								{
									this._callback___4(this.imageData.Image);
								}
								this._index___3++;
							}
						}
						InformationImageManager.SaveImageData(this._www___1, this.imageData.Image, this.imageData.ImagePath, this.imageData.ImageEtagPath);
					}
					else
					{
						global::Debug.Log("DoFetchCashImages new jpeg is non!!");
					}
					this._www___1.Dispose();
					this._www___1 = null;
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

		private string _ImageId_k__BackingField;

		private string _ImageUrl_k__BackingField;

		private string _ImagePath_k__BackingField;

		private string _ImageEtagPath_k__BackingField;

		private Texture2D _Image_k__BackingField;

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

		public string ImageId
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public string ImagePath
		{
			get;
			set;
		}

		public string ImageEtagPath
		{
			get;
			set;
		}

		public Texture2D Image
		{
			get;
			set;
		}

		public List<Action<Texture2D>> CallbackList
		{
			get;
			set;
		}

		public ImageData()
		{
			this.IsLoading = false;
			this.IsLoaded = false;
			this.ImageId = string.Empty;
			this.ImageUrl = string.Empty;
			this.ImagePath = string.Empty;
			this.Image = null;
			this.CallbackList = new List<Action<Texture2D>>(10);
		}

		public static IEnumerator DoFetchImages(InformationImageManager.ImageData imageData)
		{
			InformationImageManager.ImageData._DoFetchImages_c__Iterator5A _DoFetchImages_c__Iterator5A = new InformationImageManager.ImageData._DoFetchImages_c__Iterator5A();
			_DoFetchImages_c__Iterator5A.imageData = imageData;
			_DoFetchImages_c__Iterator5A.___imageData = imageData;
			return _DoFetchImages_c__Iterator5A;
		}

		public static IEnumerator DoFetchCashImages(InformationImageManager.ImageData imageData, string etag, string lastModified)
		{
			InformationImageManager.ImageData._DoFetchCashImages_c__Iterator5B _DoFetchCashImages_c__Iterator5B = new InformationImageManager.ImageData._DoFetchCashImages_c__Iterator5B();
			_DoFetchCashImages_c__Iterator5B.lastModified = lastModified;
			_DoFetchCashImages_c__Iterator5B.etag = etag;
			_DoFetchCashImages_c__Iterator5B.imageData = imageData;
			_DoFetchCashImages_c__Iterator5B.___lastModified = lastModified;
			_DoFetchCashImages_c__Iterator5B.___etag = etag;
			_DoFetchCashImages_c__Iterator5B.___imageData = imageData;
			return _DoFetchCashImages_c__Iterator5B;
		}
	}

	private sealed class _LoadCashImage_c__Iterator59 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal InformationImageManager.ImageData imageData;

		internal FileStream _fs___0;

		internal BinaryReader _reader___1;

		internal string _text___2;

		internal string[] _delimiter___3;

		internal string[] _word___4;

		internal string _etag___5;

		internal string _lastModified___6;

		internal List<Action<Texture2D>>.Enumerator __s_751___7;

		internal Action<Texture2D> _callback___8;

		internal int _PC;

		internal object _current;

		internal InformationImageManager.ImageData ___imageData;

		internal InformationImageManager __f__this;

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
				if (File.Exists(this.imageData.ImageEtagPath))
				{
					this._fs___0 = new FileStream(this.imageData.ImageEtagPath, FileMode.Open);
					this._reader___1 = new BinaryReader(this._fs___0);
					this._text___2 = this._reader___1.ReadString();
					this._delimiter___3 = new string[]
					{
						"@@@"
					};
					this._word___4 = this._text___2.Split(this._delimiter___3, StringSplitOptions.None);
					this._etag___5 = this._word___4[0];
					this._lastModified___6 = this._word___4[1];
					this._reader___1.Close();
					this._current = this.__f__this.StartCoroutine(InformationImageManager.ImageData.DoFetchCashImages(this.imageData, this._etag___5, this._lastModified___6));
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.imageData.IsLoaded)
			{
				this.__f__this.LoadCashImage(this.imageData.ImagePath, ref this.imageData);
				this.imageData.IsLoading = false;
				this.imageData.IsLoaded = true;
				this.__s_751___7 = this.imageData.CallbackList.GetEnumerator();
				try
				{
					while (this.__s_751___7.MoveNext())
					{
						this._callback___8 = this.__s_751___7.Current;
						if (this._callback___8 != null)
						{
							this._callback___8(this.imageData.Image);
						}
					}
				}
				finally
				{
					((IDisposable)this.__s_751___7).Dispose();
				}
			}
			this._PC = -1;
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

	private const string BANNER_PREFIX = "ad_";

	private const string IMAGE_FOLDER = "/infoImage/";

	private const string IMAGE_EXTENSION = ".jpg";

	private const string PNG_EXTENSION = ".png";

	private const string IMAGE_ETG_EXTENSION = ".etag";

	private static InformationImageManager m_instance;

	private Dictionary<string, InformationImageManager.ImageData> m_imageDataDic;

	public static InformationImageManager Instance
	{
		get
		{
			return InformationImageManager.m_instance;
		}
	}

	private void Awake()
	{
		if (InformationImageManager.m_instance == null)
		{
			InformationImageManager.m_instance = this;
			this.m_imageDataDic = new Dictionary<string, InformationImageManager.ImageData>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		string savePath = this.getSavePath();
		if (!Directory.Exists(savePath))
		{
			Directory.CreateDirectory(savePath);
		}
		string etagFloderPath = this.GetEtagFloderPath();
		if (!Directory.Exists(etagFloderPath))
		{
			Directory.CreateDirectory(etagFloderPath);
		}
	}

	private string getSavePath()
	{
		return Application.persistentDataPath + "/infoImage/";
	}

	private IEnumerator LoadCashImage(InformationImageManager.ImageData imageData)
	{
		InformationImageManager._LoadCashImage_c__Iterator59 _LoadCashImage_c__Iterator = new InformationImageManager._LoadCashImage_c__Iterator59();
		_LoadCashImage_c__Iterator.imageData = imageData;
		_LoadCashImage_c__Iterator.___imageData = imageData;
		_LoadCashImage_c__Iterator.__f__this = this;
		return _LoadCashImage_c__Iterator;
	}

	private void LoadCashImage(string filePath, ref InformationImageManager.ImageData imageData)
	{
		if (imageData != null)
		{
			imageData.Image = new Texture2D(0, 0, TextureFormat.ARGB32, false);
			imageData.Image.LoadImage(this.LoadImageData(filePath));
		}
	}

	private byte[] LoadImageData(string path)
	{
		FileStream input = new FileStream(path, FileMode.Open);
		BinaryReader binaryReader = new BinaryReader(input);
		byte[] result = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
		binaryReader.Close();
		return result;
	}

	private static void SaveImageData(WWW www, Texture2D texture, string path, string etagPath)
	{
		if (texture != null)
		{
			FileStream output = new FileStream(path, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(output);
			binaryWriter.Write(texture.EncodeToPNG());
			binaryWriter.Close();
		}
		if (www != null && www.responseHeaders.ContainsKey("ETAG") && www.responseHeaders.ContainsKey("LAST-MODIFIED"))
		{
			FileStream output2 = new FileStream(etagPath, FileMode.Create);
			BinaryWriter binaryWriter2 = new BinaryWriter(output2);
			binaryWriter2.Write(www.responseHeaders["ETAG"] + "@@@" + www.responseHeaders["LAST-MODIFIED"]);
			binaryWriter2.Close();
		}
	}

	public void DeleteImageData(string imageId)
	{
		string str = this.getSavePath() + "ad_" + imageId;
		string str2 = this.getSavePath() + imageId;
		string str3 = this.GetEtagFloderPath() + imageId;
		string str4 = this.GetEtagFloderPath() + "ad_" + imageId;
		for (int i = 0; i <= 10; i++)
		{
			string str5 = "_" + TextUtility.GetSuffix((Env.Language)i);
			string path = str + str5 + ".jpg";
			string path2 = str2 + str5 + ".jpg";
			string path3 = str + str5 + ".png";
			string path4 = str2 + str5 + ".png";
			string path5 = str4 + str5 + ".etag";
			string path6 = str3 + str5 + ".etag";
			if (this.ExistsFile(path))
			{
				File.Delete(path);
			}
			if (this.ExistsFile(path2))
			{
				File.Delete(path2);
			}
			if (this.ExistsFile(path5))
			{
				File.Delete(path5);
			}
			if (this.ExistsFile(path6))
			{
				File.Delete(path6);
			}
			if (this.ExistsFile(path3))
			{
				File.Delete(path3);
			}
			if (this.ExistsFile(path4))
			{
				File.Delete(path4);
			}
		}
	}

	private bool ExistsFile(string path)
	{
		return !string.IsNullOrEmpty(path) && File.Exists(path);
	}

	private string GetCashedFilePath(string imageId)
	{
		if (!string.IsNullOrEmpty(imageId))
		{
			string suffixe = TextUtility.GetSuffixe();
			return string.Concat(new string[]
			{
				this.getSavePath(),
				imageId,
				"_",
				suffixe,
				".jpg"
			});
		}
		return null;
	}

	private string GetCashedEtagFilePath(string imageId)
	{
		if (!string.IsNullOrEmpty(imageId))
		{
			string suffixe = TextUtility.GetSuffixe();
			return string.Concat(new string[]
			{
				this.GetEtagFloderPath(),
				imageId,
				"_",
				suffixe,
				".etag"
			});
		}
		return null;
	}

	private string GetEtagFloderPath()
	{
		return this.getSavePath() + "etag/";
	}

	private string GetServerFileURL(string imageId)
	{
		if (!string.IsNullOrEmpty(imageId))
		{
			string text = "_" + TextUtility.GetSuffix(Env.language);
			return string.Concat(new string[]
			{
				NetBaseUtil.InformationServerURL,
				"pictures/infoImage/",
				imageId,
				text,
				".jpg"
			});
		}
		return null;
	}

	private string GetBannerName(string imageId)
	{
		return "ad_" + imageId;
	}

	public bool IsLoaded(string imageId)
	{
		InformationImageManager.ImageData imageData;
		return this.m_imageDataDic.TryGetValue(imageId, out imageData) && imageData.IsLoaded;
	}

	public bool IsLoading(string imageId)
	{
		InformationImageManager.ImageData imageData;
		return this.m_imageDataDic.TryGetValue(imageId, out imageData) && imageData.IsLoading;
	}

	public bool Load(string imageId, bool bannerFlag, Action<Texture2D> callback)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(imageId))
		{
			if (bannerFlag)
			{
				imageId = this.GetBannerName(imageId);
			}
			InformationImageManager.ImageData imageData = null;
			if (this.m_imageDataDic.TryGetValue(imageId, out imageData))
			{
				if (!imageData.IsLoaded)
				{
					if (callback != null)
					{
						imageData.CallbackList.Add(callback);
					}
				}
				else
				{
					if (callback != null)
					{
						callback(imageData.Image);
					}
					result = true;
				}
			}
			else
			{
				imageData = new InformationImageManager.ImageData();
				imageData.ImageId = imageId;
				imageData.ImageUrl = this.GetServerFileURL(imageId);
				imageData.ImagePath = this.GetCashedFilePath(imageId);
				imageData.ImageEtagPath = this.GetCashedEtagFilePath(imageId);
				if (callback != null)
				{
					imageData.CallbackList.Add(callback);
				}
				imageData.IsLoading = true;
				imageData.IsLoaded = false;
				if (this.ExistsFile(imageData.ImagePath) && this.ExistsFile(imageData.ImageEtagPath))
				{
					this.m_imageDataDic.Add(imageId, imageData);
					base.StartCoroutine(this.LoadCashImage(imageData));
					result = true;
				}
				else
				{
					if (callback != null)
					{
						imageData.CallbackList.Add(callback);
					}
					this.m_imageDataDic.Add(imageId, imageData);
					base.StartCoroutine(InformationImageManager.ImageData.DoFetchImages(imageData));
					result = true;
				}
			}
		}
		return result;
	}

	public void ClearWinowImage()
	{
		if (this.m_imageDataDic.Count > 0)
		{
			Dictionary<string, InformationImageManager.ImageData>.KeyCollection keys = this.m_imageDataDic.Keys;
			List<string> list = new List<string>();
			foreach (string current in keys)
			{
				if (current.IndexOf("ad_") < 0)
				{
					list.Add(current);
				}
			}
			foreach (string current2 in list)
			{
				if (this.m_imageDataDic.ContainsKey(current2))
				{
					this.m_imageDataDic.Remove(current2);
				}
			}
		}
	}

	public void ResetImage()
	{
		this.m_imageDataDic.Clear();
	}

	public void DeleteImageFiles()
	{
		this.ResetImage();
		string savePath = this.getSavePath();
		if (Directory.Exists(savePath))
		{
			string[] files = Directory.GetFiles(savePath, "*.png", SearchOption.AllDirectories);
			if (files != null && files.Length > 0)
			{
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					if (this.ExistsFile(path))
					{
						File.Delete(path);
					}
				}
			}
			string[] files2 = Directory.GetFiles(savePath, "*.jpg", SearchOption.AllDirectories);
			if (files2 != null && files2.Length > 0)
			{
				string[] array2 = files2;
				for (int j = 0; j < array2.Length; j++)
				{
					string path2 = array2[j];
					if (this.ExistsFile(path2))
					{
						File.Delete(path2);
					}
				}
			}
			string etagFloderPath = this.GetEtagFloderPath();
			if (Directory.Exists(etagFloderPath))
			{
				string[] files3 = Directory.GetFiles(etagFloderPath, "*.etag", SearchOption.AllDirectories);
				if (files3 != null && files3.Length > 0)
				{
					string[] array3 = files3;
					for (int k = 0; k < array3.Length; k++)
					{
						string path3 = array3[k];
						if (this.ExistsFile(path3))
						{
							File.Delete(path3);
						}
					}
				}
			}
		}
	}

	public Texture2D GetImage(string imageId, bool bannerFlag, Action<Texture2D> callback)
	{
		if (this.Load(imageId, bannerFlag, callback))
		{
			InformationImageManager.ImageData imageData = this.m_imageDataDic[imageId];
			return imageData.Image;
		}
		return null;
	}
}
