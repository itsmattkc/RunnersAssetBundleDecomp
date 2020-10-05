using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceContainerObject : MonoBehaviour
{
	private Dictionary<string, ResourceContainer> m_resContainer = new Dictionary<string, ResourceContainer>();

	private ResourceCategory _Category_k__BackingField;

	public ResourceCategory Category
	{
		get;
		set;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public ResourceContainer GetContainer(string name)
	{
		ResourceContainer result = null;
		this.m_resContainer.TryGetValue(name, out result);
		return result;
	}

	public ResourceContainer CreateContainer(string name)
	{
		ResourceContainer resourceContainer = new ResourceContainer(this.Category, name);
		this.m_resContainer.Add(name, resourceContainer);
		return resourceContainer;
	}

	public ResourceContainer CreateEmptyContainer(string name)
	{
		ResourceContainer resourceContainer = new ResourceContainer(this.Category, name);
		this.m_resContainer.Add(name, resourceContainer);
		GameObject gameObject = new GameObject(name);
		resourceContainer.SetRootObject(gameObject);
		resourceContainer.DontDestroyOnChangeScene = true;
		gameObject.transform.parent = base.transform;
		return resourceContainer;
	}

	public GameObject GetGameObject(string name)
	{
		foreach (ResourceContainer current in this.m_resContainer.Values)
		{
			if (current.Active)
			{
				GameObject @object = current.GetObject(name);
				if (@object != null)
				{
					return @object;
				}
			}
		}
		return null;
	}

	public void RemoveAllResources()
	{
		foreach (ResourceContainer current in this.m_resContainer.Values)
		{
			current.RemoveAllResources();
		}
		this.m_resContainer.Clear();
	}

	public void RemoveResourcesOnThisScene()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, ResourceContainer> current in this.m_resContainer)
		{
			if (!current.Value.DontDestroyOnChangeScene)
			{
				current.Value.RemoveAllResources();
				list.Add(current.Key);
			}
			else
			{
				current.Value.RemoveResourcesOnThisScene();
			}
		}
		foreach (string current2 in list)
		{
			this.m_resContainer.Remove(current2);
		}
	}

	public void RemoveResources(string[] removeList)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, ResourceContainer> current in this.m_resContainer)
		{
			string key = current.Key;
			ResourceContainer value = current.Value;
			if (value.Active)
			{
				value.RemoveResources(removeList);
				for (int i = 0; i < removeList.Length; i++)
				{
					string text = removeList[i];
					if (!string.IsNullOrEmpty(text))
					{
						if (value.Name == text)
						{
							list.Add(key);
							break;
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (string current2 in list)
			{
				if (!string.IsNullOrEmpty(current2))
				{
					this.m_resContainer.Remove(current2);
				}
			}
		}
	}

	public void SetContainerActive(string name, bool value)
	{
		ResourceContainer container = this.GetContainer(name);
		if (container != null)
		{
			container.Active = value;
		}
	}

	public void RemoveNotActiveContainer()
	{
		List<string> list = new List<string>();
		foreach (ResourceContainer current in this.m_resContainer.Values)
		{
			if (!current.Active)
			{
				list.Add(current.Name);
			}
		}
		foreach (string current2 in list)
		{
			this.RemoveContainer(current2);
		}
	}

	private void RemoveContainer(string name)
	{
		ResourceContainer container = this.GetContainer(name);
		if (container != null)
		{
			container.RemoveAllResources();
			this.m_resContainer.Remove(name);
		}
	}
}
