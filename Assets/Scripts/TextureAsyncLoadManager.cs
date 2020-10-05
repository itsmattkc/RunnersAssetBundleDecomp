using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureAsyncLoadManager : MonoBehaviour
{
	private class TextureInfo
	{
		private enum State
		{
			IDLE,
			LOADING,
			LOADED,
			NUM
		}

		private string m_textureName;

		private Texture m_texture;

		private List<TextureRequest> m_requestList = new List<TextureRequest>();

		private int m_removeRequestCount;

		private GameObject m_gameObject;

		private ResourceSceneLoader m_loader;

		private TextureAsyncLoadManager.TextureInfo.State m_state;

		public bool Loaded
		{
			get
			{
				return this.m_state == TextureAsyncLoadManager.TextureInfo.State.LOADED;
			}
			private set
			{
			}
		}

		public bool EnableRemove
		{
			get
			{
				return this.Loaded && this.m_removeRequestCount > 0;
			}
			private set
			{
			}
		}

		public TextureInfo(GameObject obj)
		{
			this.m_gameObject = obj;
		}

		public void RequestLoad(TextureRequest request)
		{
			if (request == null)
			{
				return;
			}
			if (this.m_gameObject == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.m_textureName))
			{
				this.m_textureName = request.GetFileName();
			}
			this.m_requestList.Add(request);
			if (this.m_state == TextureAsyncLoadManager.TextureInfo.State.LOADED)
			{
				request.LoadDone(this.m_texture);
			}
		}

		public void RequestRemove(TextureRequest request)
		{
			if (request == null)
			{
				return;
			}
			if (this.m_gameObject == null)
			{
				return;
			}
			foreach (TextureRequest current in this.m_requestList)
			{
				if (current != null)
				{
					if (current == request)
					{
						break;
					}
				}
			}
			this.m_removeRequestCount++;
		}

		public void Load()
		{
			if (this.m_state != TextureAsyncLoadManager.TextureInfo.State.IDLE)
			{
				return;
			}
			this.m_loader = this.m_gameObject.AddComponent<ResourceSceneLoader>();
			this.m_loader.AddLoadAndResourceManager(this.m_textureName, true, ResourceCategory.UI, false, false, this.m_textureName);
			this.m_state = TextureAsyncLoadManager.TextureInfo.State.LOADING;
		}

		public void Update()
		{
			switch (this.m_state)
			{
			case TextureAsyncLoadManager.TextureInfo.State.LOADING:
				if (this.m_loader != null)
				{
					if (this.m_loader.Loaded)
					{
						this.m_texture = null;
						GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, this.m_textureName);
						AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
						this.m_texture = component.m_tex;
						for (int i = 0; i < this.m_requestList.Count; i++)
						{
							TextureRequest textureRequest = this.m_requestList[i];
							if (textureRequest != null)
							{
								textureRequest.LoadDone(this.m_texture);
							}
						}
						UnityEngine.Object.Destroy(this.m_loader);
						this.m_state = TextureAsyncLoadManager.TextureInfo.State.LOADED;
					}
				}
				else
				{
					this.m_state = TextureAsyncLoadManager.TextureInfo.State.LOADED;
				}
				break;
			}
		}

		public void Remove()
		{
			ResourceManager instance = ResourceManager.Instance;
			if (instance != null)
			{
				string[] removeList = new string[]
				{
					this.m_requestList[0].GetFileName()
				};
				instance.RemoveResources(ResourceCategory.UI, removeList);
			}
		}
	}

	[SerializeField]
	private Texture m_charaDefaultTexture;

	[SerializeField]
	private Texture m_chaoDefaultTexture;

	private static TextureAsyncLoadManager m_instance;

	private Dictionary<string, TextureAsyncLoadManager.TextureInfo> m_textureList = new Dictionary<string, TextureAsyncLoadManager.TextureInfo>();

	private Queue<TextureAsyncLoadManager.TextureInfo> m_loadQueue = new Queue<TextureAsyncLoadManager.TextureInfo>();

	private bool m_DirtyFlag;

	public Texture CharaDefaultTexture
	{
		get
		{
			return this.m_charaDefaultTexture;
		}
		private set
		{
		}
	}

	public Texture ChaoDefaultTexture
	{
		get
		{
			return this.m_chaoDefaultTexture;
		}
		private set
		{
		}
	}

	public static TextureAsyncLoadManager Instance
	{
		get
		{
			return TextureAsyncLoadManager.m_instance;
		}
		private set
		{
		}
	}

	public bool IsLoaded(TextureRequest request)
	{
		if (request == null)
		{
			return false;
		}
		string fileName = request.GetFileName();
		return this.IsLoaded(fileName);
	}

	public bool IsLoaded(string fileName)
	{
		TextureAsyncLoadManager.TextureInfo textureInfo = null;
		return this.m_textureList.TryGetValue(fileName, out textureInfo) && textureInfo.Loaded;
	}

	public void Request(TextureRequest request)
	{
		if (request == null)
		{
			return;
		}
		if (!request.IsEnableLoad())
		{
			return;
		}
		string fileName = request.GetFileName();
		TextureAsyncLoadManager.TextureInfo textureInfo = null;
		if (this.m_textureList.TryGetValue(fileName, out textureInfo))
		{
			textureInfo.RequestLoad(request);
		}
		else
		{
			TextureAsyncLoadManager.TextureInfo textureInfo2 = new TextureAsyncLoadManager.TextureInfo(base.gameObject);
			textureInfo2.RequestLoad(request);
			if (this.m_loadQueue.Count <= 0)
			{
				textureInfo2.Load();
			}
			this.m_loadQueue.Enqueue(textureInfo2);
			this.m_textureList.Add(fileName, textureInfo2);
		}
		this.m_DirtyFlag = true;
	}

	public void Remove(TextureRequest request)
	{
		if (request == null)
		{
			return;
		}
		if (!request.IsEnableLoad())
		{
			return;
		}
		string fileName = request.GetFileName();
		TextureAsyncLoadManager.TextureInfo textureInfo = null;
		if (!this.m_textureList.TryGetValue(fileName, out textureInfo))
		{
			return;
		}
		textureInfo.RequestRemove(request);
	}

	private void Start()
	{
		TextureAsyncLoadManager.m_instance = this;
	}

	private void Update()
	{
		if (this.m_DirtyFlag)
		{
			UIPanel.SetDirty();
			this.m_DirtyFlag = false;
		}
		if (this.m_loadQueue.Count > 0)
		{
			TextureAsyncLoadManager.TextureInfo textureInfo = this.m_loadQueue.Peek();
			textureInfo.Update();
			if (textureInfo.Loaded)
			{
				this.m_loadQueue.Dequeue();
				if (this.m_loadQueue.Count > 0)
				{
					TextureAsyncLoadManager.TextureInfo textureInfo2 = this.m_loadQueue.Peek();
					textureInfo2.Load();
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, TextureAsyncLoadManager.TextureInfo> current in this.m_textureList)
		{
			string key = current.Key;
			TextureAsyncLoadManager.TextureInfo value = current.Value;
			if (value != null)
			{
				if (value.EnableRemove)
				{
					list.Add(key);
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (string current2 in list)
			{
				TextureAsyncLoadManager.TextureInfo textureInfo3;
				if (this.m_textureList.TryGetValue(current2, out textureInfo3))
				{
					textureInfo3.Remove();
				}
				this.m_textureList.Remove(current2);
			}
		}
	}
}
