using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageBlockPathManager : MonoBehaviour
{
	private string m_pathStageName;

	private PathManager m_pathManager;

	private List<GameObject> m_blockPathController;

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.m_blockPathController != null)
		{
			foreach (GameObject current in this.m_blockPathController)
			{
				UnityEngine.Object.Destroy(current);
			}
			this.m_blockPathController = null;
		}
	}

	public void SetPathManager(PathManager manager)
	{
		this.m_pathManager = manager;
	}

	public void Setup()
	{
		this.m_blockPathController = new List<GameObject>();
	}

	public BlockPathController GetCurrentController()
	{
		foreach (GameObject current in this.m_blockPathController)
		{
			BlockPathController component = current.GetComponent<BlockPathController>();
			if (component && component.IsNowCurrent())
			{
				return component;
			}
		}
		return null;
	}

	public PathComponent GetCurentSVPath(ref float? distance)
	{
		return this.GetCurentPath(BlockPathController.PathType.SV, ref distance);
	}

	public PathComponent GetCurentDrillPath(ref float? distance)
	{
		return this.GetCurentPath(BlockPathController.PathType.DRILL, ref distance);
	}

	public PathComponent GetCurentLaserPath(ref float? distance)
	{
		return this.GetCurentPath(BlockPathController.PathType.LASER, ref distance);
	}

	private void ActivateBlock(int block, int blockActivateID, Vector3 originPoint)
	{
		GameObject gameObject = new GameObject("BlockPathController");
		gameObject.transform.parent = base.transform;
		BlockPathController blockPathController = gameObject.AddComponent<BlockPathController>();
		if (this.m_pathManager != null)
		{
			string stageName = StageTypeUtil.GetStageName(this.m_pathManager.GetSVPathName());
			blockPathController.Initialize(stageName, block, blockActivateID, this.m_pathManager, originPoint);
		}
		if (this.m_blockPathController != null)
		{
			this.m_blockPathController.Add(gameObject);
		}
	}

	private void DeactivateBlock(int activateId)
	{
		BlockPathController controllerByActivateID = this.GetControllerByActivateID(activateId);
		if (controllerByActivateID != null)
		{
			this.m_blockPathController.Remove(controllerByActivateID.gameObject);
			UnityEngine.Object.Destroy(controllerByActivateID.gameObject);
		}
	}

	private BlockPathController GetControllerByActivateID(int activateID)
	{
		foreach (GameObject current in this.m_blockPathController)
		{
			BlockPathController component = current.GetComponent<BlockPathController>();
			if (component && component.ActivateID == activateID)
			{
				return component;
			}
		}
		return null;
	}

	public PathComponent GetCurentPath(BlockPathController.PathType pathType, ref float? distance)
	{
		BlockPathController currentController = this.GetCurrentController();
		if (currentController == null)
		{
			return null;
		}
		PathEvaluator evaluator = currentController.GetEvaluator(pathType);
		if (evaluator != null && evaluator.IsValid())
		{
			if (distance.HasValue)
			{
				distance = new float?(evaluator.Distance);
			}
			return evaluator.GetPathObject();
		}
		return null;
	}

	public PathEvaluator GetCurentPathEvaluator(BlockPathController.PathType pathType)
	{
		BlockPathController currentController = this.GetCurrentController();
		if (currentController == null)
		{
			return null;
		}
		PathEvaluator evaluator = currentController.GetEvaluator(pathType);
		if (evaluator != null && evaluator.IsValid())
		{
			return evaluator;
		}
		return null;
	}

	public void OnActivateBlock(MsgActivateBlock msg)
	{
		if (msg != null)
		{
			this.ActivateBlock(msg.m_blockNo, msg.m_activateID, msg.m_originPosition);
		}
	}

	private void OnDeactivateBlock(MsgDeactivateBlock msg)
	{
		if (msg != null)
		{
			this.DeactivateBlock(msg.m_activateID);
		}
	}

	private void OnDeactivateAllBlock(MsgDeactivateAllBlock msg)
	{
		foreach (GameObject current in this.m_blockPathController)
		{
			UnityEngine.Object.Destroy(current);
		}
		this.m_blockPathController.Clear();
	}

	private void OnChangeCurerntBlock(MsgChangeCurrentBlock msg)
	{
		if (msg != null)
		{
			BlockPathController currentController = this.GetCurrentController();
			BlockPathController controllerByActivateID = this.GetControllerByActivateID(msg.m_activateID);
			if (currentController != null)
			{
				currentController.SetCurrent(false);
			}
			if (controllerByActivateID != null)
			{
				controllerByActivateID.SetCurrent(true);
			}
		}
	}
}
