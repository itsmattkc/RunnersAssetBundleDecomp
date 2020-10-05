using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceInfo
{
	private GameObject _ResObject_k__BackingField;

	private bool _DontDestroyOnChangeScene_k__BackingField;

	private ResourceCategory _Category_k__BackingField;

	private string _PathName_k__BackingField;

	private bool _AssetBundle_k__BackingField;

	private bool _Cashed_k__BackingField;

	public GameObject ResObject
	{
		get;
		set;
	}

	public bool DontDestroyOnChangeScene
	{
		get;
		set;
	}

	public ResourceCategory Category
	{
		get;
		set;
	}

	public string PathName
	{
		get;
		set;
	}

	public bool AssetBundle
	{
		get;
		set;
	}

	public bool Cashed
	{
		get;
		set;
	}

	public ResourceInfo(ResourceCategory category)
	{
		this.Category = category;
	}

	public void CopyTo(ResourceInfo dest)
	{
		dest.ResObject = this.ResObject;
		dest.DontDestroyOnChangeScene = this.DontDestroyOnChangeScene;
		dest.Category = this.Category;
		dest.PathName = this.PathName;
		dest.AssetBundle = this.AssetBundle;
		dest.Cashed = this.Cashed;
	}
}
