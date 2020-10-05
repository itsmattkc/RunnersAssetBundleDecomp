using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	private sealed class _ParseAndCreateDatas_c__Iterator1B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal TerrainXmlData terrainData;

		internal TextAsset _asset___0;

		internal int _PC;

		internal object _current;

		internal TerrainXmlData ___terrainData;

		internal PathManager __f__this;

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
				if (!(this.terrainData != null))
				{
					goto IL_127;
				}
				this._asset___0 = this.terrainData.LoopPath;
				if (this._asset___0 != null && this._asset___0.text != null)
				{
					this._current = this.__f__this.StartCoroutine(PathXmlDeserializer.CreatePathObjectData(this._asset___0, this.__f__this.m_pathList));
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				break;
			case 2u:
				goto IL_127;
			default:
				return false;
			}
			this._asset___0 = this.terrainData.SideViewPath;
			if (this._asset___0 != null)
			{
				this.__f__this.m_svPathName = this._asset___0.name;
			}
			if (this._asset___0 != null && this._asset___0.text != null)
			{
				this._current = this.__f__this.StartCoroutine(PathXmlDeserializer.CreatePathObjectData(this._asset___0, this.__f__this.m_pathList));
				this._PC = 2;
				return true;
			}
			IL_127:
			this.__f__this.SetupEnd = true;
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

	private Dictionary<string, ResPathObjectData> m_pathList;

	private uint m_idCounter;

	public bool m_drawGismos;

	private string m_svPathName = string.Empty;

	private bool _SetupEnd_k__BackingField;

	public bool SetupEnd
	{
		get;
		private set;
	}

	public Dictionary<string, ResPathObjectData> PathList
	{
		get
		{
			return this.m_pathList;
		}
	}

	private void Start()
	{
		if (this.m_pathList == null)
		{
			this.m_pathList = new Dictionary<string, ResPathObjectData>();
		}
	}

	public void CreatePathObjectData()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (this.m_pathList == null)
		{
			this.m_pathList = new Dictionary<string, ResPathObjectData>();
		}
		GameObject gameObject = instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, TerrainXmlData.DataAssetName);
		if (gameObject)
		{
			TerrainXmlData component = gameObject.GetComponent<TerrainXmlData>();
			base.StartCoroutine(this.ParseAndCreateDatas(component));
		}
	}

	public PathComponent CreatePathComponent(string name, Vector3 rootPosition)
	{
		ResPathObject resPathObject = this.CreatePathObject(name, rootPosition);
		if (resPathObject != null)
		{
			GameObject gameObject = new GameObject("PathComponentObject");
			PathComponent pathComponent = gameObject.AddComponent<PathComponent>();
			if (pathComponent)
			{
				gameObject.transform.position = rootPosition;
				gameObject.transform.parent = base.transform;
				pathComponent.SetManager(this);
				pathComponent.SetObject(resPathObject);
				pathComponent.SetID(this.m_idCounter);
				this.m_idCounter += 1u;
				return pathComponent;
			}
		}
		return null;
	}

	public PathComponent GetPathComponent(string name)
	{
		PathComponent[] componentsInChildren = base.GetComponentsInChildren<PathComponent>();
		PathComponent[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			PathComponent pathComponent = array[i];
			if (pathComponent.GetName() == name)
			{
				return pathComponent;
			}
		}
		return null;
	}

	public PathComponent GetPathComponent(uint id)
	{
		PathComponent[] componentsInChildren = base.GetComponentsInChildren<PathComponent>();
		PathComponent[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			PathComponent pathComponent = array[i];
			if (pathComponent.GetID() == id)
			{
				return pathComponent;
			}
		}
		return null;
	}

	private ResPathObject CreatePathObject(string name, Vector3 rootPosition)
	{
		if (this.m_pathList == null)
		{
			return null;
		}
		string key = name.ToLower();
		ResPathObjectData resPathObjectData;
		this.m_pathList.TryGetValue(key, out resPathObjectData);
		if (resPathObjectData != null)
		{
			return new ResPathObject(resPathObjectData, rootPosition);
		}
		return null;
	}

	public void DestroyComponent(string pathname)
	{
		PathComponent pathComponent = this.GetPathComponent(pathname);
		this.DestroyComponent(pathComponent);
	}

	public void DestroyComponent(PathComponent component)
	{
		if (component != null)
		{
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	public string GetSVPathName()
	{
		return this.m_svPathName;
	}

	private IEnumerator ParseAndCreateDatas(TerrainXmlData terrainData)
	{
		PathManager._ParseAndCreateDatas_c__Iterator1B _ParseAndCreateDatas_c__Iterator1B = new PathManager._ParseAndCreateDatas_c__Iterator1B();
		_ParseAndCreateDatas_c__Iterator1B.terrainData = terrainData;
		_ParseAndCreateDatas_c__Iterator1B.___terrainData = terrainData;
		_ParseAndCreateDatas_c__Iterator1B.__f__this = this;
		return _ParseAndCreateDatas_c__Iterator1B;
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
