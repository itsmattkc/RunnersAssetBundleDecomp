using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer
{
	private bool m_active = true;

	private ResourceInfo m_rootResource;

	private Dictionary<string, ResourceInfo> m_resources;

	private string m_name;

	public string Name
	{
		get
		{
			return this.m_name;
		}
	}

	public ResourceCategory Category
	{
		get
		{
			return this.m_rootResource.Category;
		}
	}

	public bool Active
	{
		get
		{
			return this.m_active;
		}
		set
		{
			this.m_active = value;
		}
	}

	public bool DontDestroyOnChangeScene
	{
		get
		{
			return this.m_rootResource != null && this.m_rootResource.DontDestroyOnChangeScene;
		}
		set
		{
			if (this.m_rootResource != null)
			{
				this.m_rootResource.DontDestroyOnChangeScene = value;
			}
		}
	}

	public ResourceContainer(ResourceCategory category, string name)
	{
		this.m_rootResource = new ResourceInfo(category);
		this.m_resources = new Dictionary<string, ResourceInfo>();
		this.m_name = name;
	}

	public bool SetRootObject(GameObject srcObject)
	{
		if (this.m_rootResource.ResObject == null)
		{
			this.m_rootResource.ResObject = srcObject;
			return true;
		}
		return false;
	}

	public bool SetRootObject(ResourceInfo resInfo)
	{
		if (this.m_rootResource.ResObject == null)
		{
			resInfo.CopyTo(this.m_rootResource);
			return true;
		}
		return false;
	}

	public void AddChildObject(GameObject srcObject, bool dontDestoryOnChangeScene)
	{
		ResourceInfo resourceInfo = new ResourceInfo(this.m_rootResource.Category);
		resourceInfo.ResObject = srcObject;
		resourceInfo.PathName = this.m_rootResource.PathName;
		resourceInfo.DontDestroyOnChangeScene = dontDestoryOnChangeScene;
		this.m_resources.Add(srcObject.name, resourceInfo);
	}

	public bool IsExist(GameObject gameObject)
	{
		return this.m_resources.ContainsKey(gameObject.name);
	}

	public GameObject GetObject(string objectName)
	{
		if (this.m_rootResource.ResObject != null && this.m_rootResource.ResObject.name == objectName)
		{
			return this.m_rootResource.ResObject;
		}
		ResourceInfo resourceInfo;
		this.m_resources.TryGetValue(objectName, out resourceInfo);
		if (resourceInfo != null)
		{
			return resourceInfo.ResObject;
		}
		return null;
	}

	public void RemoveAllResources()
	{
		foreach (ResourceInfo current in this.m_resources.Values)
		{
			this.DestroyObject(current);
		}
		this.m_resources.Clear();
		if (this.m_rootResource != null && this.m_rootResource.ResObject != null)
		{
			UnityEngine.Object.Destroy(this.m_rootResource.ResObject);
			this.m_rootResource = null;
		}
	}

	public void RemoveResourcesOnThisScene()
	{
		List<string> list = new List<string>();
		foreach (ResourceInfo current in this.m_resources.Values)
		{
			if (!current.DontDestroyOnChangeScene)
			{
				string name = current.ResObject.name;
				this.DestroyObject(current);
				list.Add(name);
			}
		}
		foreach (string current2 in list)
		{
			this.m_resources.Remove(current2);
		}
	}

	public void RemoveResources(string[] names)
	{
		for (int i = 0; i < names.Length; i++)
		{
			string text = names[i];
			ResourceInfo resourceInfo;
			this.m_resources.TryGetValue(text, out resourceInfo);
			if (resourceInfo != null)
			{
				this.DestroyObject(resourceInfo);
				this.m_resources.Remove(text);
			}
			if (this.m_rootResource.ResObject != null && this.m_rootResource.ResObject.name == text)
			{
				this.DestroyObject(this.m_rootResource);
				break;
			}
		}
	}

	private void DestroyObject(ResourceInfo info)
	{
		UnityEngine.Object.Destroy(info.ResObject);
	}
}
