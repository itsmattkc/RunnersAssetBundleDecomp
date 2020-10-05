using System;
using UnityEngine;

public class FBSettings : ScriptableObject
{
	private const string facebookSettingsAssetName = "FacebookSettings";

	private const string facebookSettingsPath = "Facebook/Resources";

	private const string facebookSettingsAssetExtension = ".asset";

	private static FBSettings instance;

	[SerializeField]
	private int selectedAppIndex;

	[SerializeField]
	private string[] appIds = new string[]
	{
		"0"
	};

	[SerializeField]
	private string[] appLabels = new string[]
	{
		"App Name"
	};

	[SerializeField]
	private bool cookie = true;

	[SerializeField]
	private bool logging = true;

	[SerializeField]
	private bool status = true;

	[SerializeField]
	private bool xfbml;

	[SerializeField]
	private bool frictionlessRequests = true;

	[SerializeField]
	private string iosURLSuffix = string.Empty;

	private static FBSettings Instance
	{
		get
		{
			if (FBSettings.instance == null)
			{
				FBSettings.instance = (Resources.Load("FacebookSettings") as FBSettings);
				if (FBSettings.instance == null)
				{
					FBSettings.instance = ScriptableObject.CreateInstance<FBSettings>();
				}
			}
			return FBSettings.instance;
		}
	}

	public int SelectedAppIndex
	{
		get
		{
			return this.selectedAppIndex;
		}
	}

	public string[] AppIds
	{
		get
		{
			return this.appIds;
		}
		set
		{
			if (this.appIds != value)
			{
				this.appIds = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public string[] AppLabels
	{
		get
		{
			return this.appLabels;
		}
		set
		{
			if (this.appLabels != value)
			{
				this.appLabels = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static string[] AllAppIds
	{
		get
		{
			return FBSettings.Instance.AppIds;
		}
	}

	public static string AppId
	{
		get
		{
			return FBSettings.Instance.AppIds[FBSettings.Instance.SelectedAppIndex];
		}
	}

	public static bool IsValidAppId
	{
		get
		{
			return FBSettings.AppId != null && FBSettings.AppId.Length > 0 && !FBSettings.AppId.Equals("0");
		}
	}

	public static bool Cookie
	{
		get
		{
			return FBSettings.Instance.cookie;
		}
		set
		{
			if (FBSettings.Instance.cookie != value)
			{
				FBSettings.Instance.cookie = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static bool Logging
	{
		get
		{
			return FBSettings.Instance.logging;
		}
		set
		{
			if (FBSettings.Instance.logging != value)
			{
				FBSettings.Instance.logging = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static bool Status
	{
		get
		{
			return FBSettings.Instance.status;
		}
		set
		{
			if (FBSettings.Instance.status != value)
			{
				FBSettings.Instance.status = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static bool Xfbml
	{
		get
		{
			return FBSettings.Instance.xfbml;
		}
		set
		{
			if (FBSettings.Instance.xfbml != value)
			{
				FBSettings.Instance.xfbml = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static string IosURLSuffix
	{
		get
		{
			return FBSettings.Instance.iosURLSuffix;
		}
		set
		{
			if (FBSettings.Instance.iosURLSuffix != value)
			{
				FBSettings.Instance.iosURLSuffix = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public static string ChannelUrl
	{
		get
		{
			return "/channel.html";
		}
	}

	public static bool FrictionlessRequests
	{
		get
		{
			return FBSettings.Instance.frictionlessRequests;
		}
		set
		{
			if (FBSettings.Instance.frictionlessRequests != value)
			{
				FBSettings.Instance.frictionlessRequests = value;
				FBSettings.DirtyEditor();
			}
		}
	}

	public void SetAppIndex(int index)
	{
		if (this.selectedAppIndex != index)
		{
			this.selectedAppIndex = index;
			FBSettings.DirtyEditor();
		}
	}

	public void SetAppId(int index, string value)
	{
		if (this.appIds[index] != value)
		{
			this.appIds[index] = value;
			FBSettings.DirtyEditor();
		}
	}

	public void SetAppLabel(int index, string value)
	{
		if (this.appLabels[index] != value)
		{
			this.AppLabels[index] = value;
			FBSettings.DirtyEditor();
		}
	}

	private static void DirtyEditor()
	{
	}
}
