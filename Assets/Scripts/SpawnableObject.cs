using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Level")]
public abstract class SpawnableObject : MonoBehaviour
{
	[SerializeField]
	private StockObjectType m_stockObjectType = StockObjectType.UNKNOWN;

	private SpawnableInfo m_spawnableInfo;

	private bool m_sleep;

	private bool m_share;

	public bool Sleep
	{
		get
		{
			return this.m_sleep;
		}
		set
		{
			this.m_sleep = value;
		}
	}

	public bool Share
	{
		get
		{
			return this.m_share;
		}
		set
		{
			this.m_share = value;
		}
	}

	private void Start()
	{
		this.Spawn();
	}

	private void Spawn()
	{
		if (!this.IsSpawnedByManager())
		{
			SpawnableBehavior component = base.GetComponent<SpawnableBehavior>();
			if (component)
			{
				component.SetParameters(component.GetParameter());
			}
		}
		this.OnSpawned();
	}

	public GameObject AttachModelObject()
	{
		string modelName = this.GetModelName();
		if (modelName != null)
		{
			ResourceCategory modelCategory = this.GetModelCategory();
			GameObject gameObject = ResourceManager.Instance.GetGameObject(modelCategory, modelName);
			if (gameObject)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.SetActive(true);
					gameObject2.transform.parent = base.transform;
					gameObject2.transform.localPosition = Vector3.zero;
					gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
					return gameObject2;
				}
			}
		}
		return null;
	}

	protected GameObject AttachObject(ResourceCategory category, string objectName)
	{
		return this.AttachObject(category, objectName, Vector3.zero, Quaternion.identity);
	}

	protected GameObject AttachObject(ResourceCategory category, string objectName, Vector3 localPosition, Quaternion localRotation)
	{
		if (this.IsValid() && objectName != null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(category, objectName);
			if (gameObject)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject2)
				{
					gameObject2.SetActive(true);
					gameObject2.transform.parent = base.transform;
					gameObject2.transform.localPosition = localPosition;
					gameObject2.transform.localRotation = localRotation;
					return gameObject2;
				}
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		this.OnDestroyed();
		ObjectSpawnManager manager = this.GetManager();
		if (this.m_spawnableInfo != null && manager != null)
		{
			manager.DetachObject(this.m_spawnableInfo);
		}
		this.m_spawnableInfo = null;
	}

	protected void SetSleep(GameObject obj)
	{
		ObjectSpawnManager manager = this.GetManager();
		if (manager != null)
		{
			SpawnableObject component = obj.GetComponent<SpawnableObject>();
			if (component.IsStockObject())
			{
				manager.SleepSpawnableObject(component);
			}
		}
	}

	public void SetSleep()
	{
		ObjectSpawnManager manager = this.GetManager();
		if (manager != null && this.IsStockObject())
		{
			manager.SleepSpawnableObject(this);
		}
	}

	protected abstract void OnSpawned();

	public virtual void OnCreate()
	{
	}

	public virtual void OnRevival()
	{
	}

	protected virtual void OnDestroyed()
	{
	}

	public void AttachSpawnableInfo(SpawnableInfo info)
	{
		this.m_spawnableInfo = info;
	}

	public bool IsSpawnedByManager()
	{
		return this.m_spawnableInfo != null;
	}

	protected ObjectSpawnManager GetManager()
	{
		if (this.m_spawnableInfo != null)
		{
			return this.m_spawnableInfo.Manager;
		}
		return null;
	}

	protected virtual string GetModelName()
	{
		return null;
	}

	protected virtual ResourceCategory GetModelCategory()
	{
		return ResourceCategory.UNKNOWN;
	}

	protected virtual bool isStatic()
	{
		return false;
	}

	public virtual bool IsValid()
	{
		return true;
	}

	protected void SetOnlyOneObject()
	{
		if (this.m_spawnableInfo != null)
		{
			this.m_spawnableInfo.AttributeOnlyOne = true;
			ObjectSpawnManager manager = this.GetManager();
			if (manager)
			{
				manager.RegisterOnlyOneObject(this.m_spawnableInfo);
			}
		}
	}

	protected void SetNotRageout(bool value)
	{
		if (this.m_spawnableInfo != null)
		{
			this.m_spawnableInfo.NotRangeOut = value;
		}
	}

	public StockObjectType GetStockObjectType()
	{
		return this.m_stockObjectType;
	}

	public bool IsStockObject()
	{
		return this.m_stockObjectType != StockObjectType.UNKNOWN;
	}
}
