using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	private const string SimpleContainerName = "SimpleContainer";

	private Dictionary<ResourceCategory, ResourceContainerObject> m_container;

	private static ResourceManager instance;

	public static ResourceManager Instance
	{
		get
		{
			return ResourceManager.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		this.Initialize();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Initialize()
	{
		if (this.m_container == null)
		{
			this.m_container = new Dictionary<ResourceCategory, ResourceContainerObject>();
			for (ResourceCategory resourceCategory = ResourceCategory.OBJECT_RESOURCE; resourceCategory < ResourceCategory.NUM; resourceCategory++)
			{
				ResourceContainerObject resourceContainerObject = new GameObject(resourceCategory.ToString())
				{
					transform = 
					{
						parent = base.transform
					}
				}.AddComponent<ResourceContainerObject>();
				resourceContainerObject.Category = resourceCategory;
				resourceContainerObject.CreateEmptyContainer("SimpleContainer");
				this.m_container.Add(resourceCategory, resourceContainerObject);
			}
		}
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		if (this.m_container != null)
		{
			this.RemoveAllResources();
		}
		if (ResourceManager.instance == this)
		{
			ResourceManager.instance = null;
		}
	}

	public void AddCategorySceneObjects(ResourceCategory category, string containerName, GameObject resourceRootObject, bool dontDestroyOnChangeScene)
	{
		if (resourceRootObject == null)
		{
			return;
		}
		ResourceContainerObject resourceContainerObject = this.m_container[category];
		if (containerName == null)
		{
			containerName = resourceRootObject.name;
		}
		ResourceContainer resourceContainer = resourceContainerObject.GetContainer(containerName);
		if (resourceContainer == null)
		{
			resourceContainer = resourceContainerObject.CreateContainer(containerName);
			resourceContainer.DontDestroyOnChangeScene = dontDestroyOnChangeScene;
			resourceContainer.SetRootObject(resourceRootObject);
			resourceRootObject.transform.parent = resourceContainerObject.gameObject.transform;
		}
		foreach (Transform transform in resourceRootObject.transform)
		{
			if (!resourceContainer.IsExist(transform.gameObject))
			{
				resourceContainer.AddChildObject(transform.gameObject, dontDestroyOnChangeScene);
				transform.gameObject.SetActive(false);
			}
		}
	}

	public void AddCategorySceneObjectsAndSetActive(ResourceCategory category, string containerName, GameObject resourceRootObject, bool dontDestroyOnSceneChange)
	{
		this.AddCategorySceneObjects(category, containerName, resourceRootObject, dontDestroyOnSceneChange);
		this.SetContainerActive(category, containerName, true);
	}

	public GameObject GetGameObject(ResourceCategory category, string name)
	{
		return this.m_container[category].GetGameObject(name);
	}

	public GameObject GetSpawnableGameObject(string name)
	{
		ResourceCategory[] array = new ResourceCategory[]
		{
			ResourceCategory.OBJECT_PREFAB,
			ResourceCategory.ENEMY_PREFAB,
			ResourceCategory.STAGE_RESOURCE,
			ResourceCategory.EVENT_RESOURCE
		};
		ResourceCategory[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ResourceCategory category = array2[i];
			GameObject gameObject = this.GetGameObject(category, name);
			if (gameObject)
			{
				return gameObject;
			}
		}
		return null;
	}

	public bool IsExistContainer(string name)
	{
		for (ResourceCategory resourceCategory = ResourceCategory.OBJECT_RESOURCE; resourceCategory < ResourceCategory.NUM; resourceCategory++)
		{
			if (this.m_container[resourceCategory] == null)
			{
				return false;
			}
			if (this.m_container[resourceCategory].GetContainer(name) != null)
			{
				return true;
			}
		}
		return false;
	}

	private void AddObject(ResourceCategory category, GameObject addObject, bool dontDestroyOnChangeScene)
	{
		this.AddObject(category, "SimpleContainer", addObject, dontDestroyOnChangeScene);
	}

	private void AddObject(ResourceCategory category, string containerName, GameObject addObject, bool dontDestroyOnChangeScene)
	{
		if (this.m_container == null)
		{
			return;
		}
		if (category == ResourceCategory.ETC)
		{
			dontDestroyOnChangeScene = false;
		}
		ResourceContainer container = this.m_container[category].GetContainer(containerName);
		if (container != null)
		{
			container.AddChildObject(addObject, dontDestroyOnChangeScene);
		}
	}

	public void RemoveAllResources()
	{
		if (this.m_container == null)
		{
			return;
		}
		foreach (ResourceContainerObject current in this.m_container.Values)
		{
			current.RemoveAllResources();
		}
	}

	public void RemoveResourcesOnThisScene()
	{
		foreach (ResourceContainerObject current in this.m_container.Values)
		{
			current.RemoveResourcesOnThisScene();
		}
	}

	public void RemoveResources(ResourceCategory category)
	{
		this.m_container[category].RemoveAllResources();
	}

	public void RemoveResources(ResourceCategory category, string[] removeList)
	{
		this.m_container[category].RemoveResources(removeList);
	}

	public void SetContainerActive(ResourceCategory category, string name, bool value)
	{
		this.m_container[category].SetContainerActive(name, value);
	}

	public void RemoveNotActiveContainer(ResourceCategory category)
	{
		this.m_container[category].RemoveNotActiveContainer();
	}

	protected bool CheckInstance()
	{
		if (ResourceManager.instance == null)
		{
			ResourceManager.instance = this;
			this.Initialize();
			return true;
		}
		if (this == ResourceManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}
}
