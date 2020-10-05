using System;
using UnityEngine;

public class BlockPathController : MonoBehaviour
{
	public enum PathType
	{
		SV,
		DRILL,
		LASER,
		NUM_PATH
	}

	private int m_numBlock;

	private int m_activateID;

	private bool m_nowCurrent;

	private PlayerInformation m_playerInformation;

	private PathManager m_pathManager;

	[SerializeField]
	private bool m_drawGismos;

	[SerializeField]
	private bool m_dispInfo;

	private Rect m_window;

	private PathComponent[] m_pathComponent;

	private PathEvaluator[] m_pathEvaluator;

	private static readonly string[] path_name_suffix = new string[]
	{
		"_sv",
		"_dr",
		"_ls"
	};

	public int BlockNo
	{
		get
		{
			return this.m_numBlock;
		}
	}

	public int ActivateID
	{
		get
		{
			return this.m_activateID;
		}
	}

	private void Start()
	{
		this.m_dispInfo = false;
	}

	private void Update()
	{
		if (!this.m_nowCurrent)
		{
			return;
		}
		if (this.m_pathEvaluator == null || this.m_playerInformation == null)
		{
			return;
		}
		Vector3 position = this.m_playerInformation.Position;
		position.y = 0f;
		PathEvaluator[] pathEvaluator = this.m_pathEvaluator;
		for (int i = 0; i < pathEvaluator.Length; i++)
		{
			PathEvaluator pathEvaluator2 = pathEvaluator[i];
			if (pathEvaluator2 != null)
			{
				float distance = pathEvaluator2.Distance;
				float distance2 = distance;
				pathEvaluator2.GetClosestPositionAlongSpline(position, distance - 5f, distance + 5f, out distance2);
				pathEvaluator2.Distance = distance2;
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_pathManager)
		{
			this.DestroyPathEvaluator();
			if (this.m_pathComponent != null)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_pathComponent[i] != null)
					{
						this.m_pathManager.DestroyComponent(this.m_pathComponent[i]);
					}
					this.m_pathComponent[i] = null;
				}
				this.m_pathComponent = null;
			}
			this.m_pathManager = null;
		}
	}

	public void Initialize(string stageName, int numBlock, int activateID, PathManager manager, Vector3 rootPosition)
	{
		this.m_numBlock = numBlock;
		this.m_pathManager = manager;
		this.m_activateID = activateID;
		this.m_playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
		this.m_pathComponent = new PathComponent[3];
		for (int i = 0; i < 3; i++)
		{
			string name = stageName + "Terrain" + numBlock.ToString("D2") + BlockPathController.path_name_suffix[i];
			this.m_pathComponent[i] = manager.CreatePathComponent(name, rootPosition);
			if (i > 0 && this.m_pathComponent[i] == null && this.m_pathComponent[0] != null)
			{
				Vector3 rootPosition2 = rootPosition + ((i != 1) ? new Vector3(0f, 5f, 0f) : new Vector3(0f, -2f, 0f));
				this.m_pathComponent[i] = manager.CreatePathComponent(name, rootPosition2);
			}
		}
		base.transform.position = rootPosition;
	}

	public void SetCurrent(bool value)
	{
		if (!this.IsNowCurrent() && value)
		{
			this.CreatePathEvaluator();
		}
		else if (this.IsNowCurrent() && !value)
		{
			this.m_nowCurrent = value;
		}
		this.m_nowCurrent = value;
	}

	public bool IsNowCurrent()
	{
		return this.m_nowCurrent;
	}

	public PathEvaluator GetEvaluator(BlockPathController.PathType type)
	{
		if (this.m_pathEvaluator == null)
		{
			return null;
		}
		return this.m_pathEvaluator[(int)type];
	}

	public PathComponent GetComponent(BlockPathController.PathType type)
	{
		if (this.m_pathComponent == null)
		{
			return null;
		}
		return this.m_pathComponent[(int)type];
	}

	public bool GetPNT(BlockPathController.PathType type, ref Vector3? pos, ref Vector3? nrm, ref Vector3? tan)
	{
		PathEvaluator evaluator = this.GetEvaluator(type);
		if (evaluator == null)
		{
			return false;
		}
		evaluator.GetPNT(ref pos, ref nrm, ref tan);
		return true;
	}

	private void CreatePathEvaluator()
	{
		if (this.m_pathComponent[0] != null)
		{
			this.m_pathEvaluator = new PathEvaluator[3];
			for (int i = 0; i < 3; i++)
			{
				if (!(this.m_pathComponent[i] == null))
				{
					if (this.m_pathComponent[i].IsValid())
					{
						this.m_pathEvaluator[i] = new PathEvaluator();
						this.m_pathEvaluator[i].SetPathObject(this.m_pathComponent[i]);
						if (this.m_playerInformation != null)
						{
							Vector3 position = this.m_playerInformation.Position;
							float distance = 0f;
							this.m_pathEvaluator[i].GetClosestPositionAlongSpline(position, 0f, this.m_pathEvaluator[i].GetLength(), out distance);
							this.m_pathEvaluator[i].Distance = distance;
						}
					}
				}
			}
		}
	}

	private void DestroyPathEvaluator()
	{
		if (this.m_pathEvaluator != null)
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_pathEvaluator[i] = null;
			}
			this.m_pathEvaluator = null;
		}
	}

	private void OnDrawGizmos()
	{
	}
}
