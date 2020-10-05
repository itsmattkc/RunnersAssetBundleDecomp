using System;
using System.Collections.Generic;
using UnityEngine;

public class AtlasIntermediary : MonoBehaviour
{
	[SerializeField]
	private UIAtlas[] atlasList;

	private Dictionary<string, UIAtlas> m_atlasList;

	private static AtlasIntermediary m_instance;

	public static AtlasIntermediary instance
	{
		get
		{
			return AtlasIntermediary.m_instance;
		}
	}

	public bool isInit
	{
		get
		{
			bool result = false;
			if (this.m_atlasList != null && this.m_atlasList.Count > 0)
			{
				result = true;
			}
			return result;
		}
	}

	public void Awake()
	{
		this.SetInstance();
	}

	private void OnDestroy()
	{
		if (AtlasIntermediary.m_instance == this)
		{
			AtlasIntermediary.m_instance = null;
		}
	}

	private void SetInstance()
	{
		if (AtlasIntermediary.m_instance == null)
		{
			AtlasIntermediary.m_instance = this;
			this.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Init()
	{
		if (this.m_atlasList == null)
		{
			this.m_atlasList = new Dictionary<string, UIAtlas>();
			UIAtlas[] array = this.atlasList;
			for (int i = 0; i < array.Length; i++)
			{
				UIAtlas uIAtlas = array[i];
				this.m_atlasList.Add(uIAtlas.name, uIAtlas);
			}
		}
	}

	public UIAtlas GetAtlas(string atlasName)
	{
		UIAtlas result = null;
		if (!this.isInit)
		{
			this.Init();
		}
		if (this.m_atlasList.ContainsKey(atlasName))
		{
			result = this.m_atlasList[atlasName];
		}
		return result;
	}

	public UIAtlas GetAtlasServerItemId(int serverItemId)
	{
		UIAtlas result = null;
		ServerItem serverItem = new ServerItem((ServerItem.Id)serverItemId);
		string idTypeAtlasName = ServerItem.GetIdTypeAtlasName(serverItem.idType);
		if (idTypeAtlasName != null && idTypeAtlasName != string.Empty)
		{
			result = this.GetAtlas(idTypeAtlasName);
		}
		return result;
	}

	public UIAtlas GetAtlasItemIdType(ServerItem.IdType idType)
	{
		UIAtlas result = null;
		string idTypeAtlasName = ServerItem.GetIdTypeAtlasName(idType);
		if (idTypeAtlasName != null && idTypeAtlasName != string.Empty)
		{
			result = this.GetAtlas(idTypeAtlasName);
		}
		return result;
	}

	public static List<string> GetSpriteNameList(UIAtlas atlas)
	{
		List<string> list = null;
		if (atlas != null)
		{
			list = new List<string>();
			List<UISpriteData> spriteList = atlas.spriteList;
			foreach (UISpriteData current in spriteList)
			{
				list.Add(current.name);
			}
		}
		return list;
	}

	public static UISpriteData GetSpriteData(UIAtlas atlas, string spriteName)
	{
		UISpriteData result = null;
		if (atlas != null)
		{
			List<UISpriteData> spriteList = atlas.spriteList;
			foreach (UISpriteData current in spriteList)
			{
				if (current.name == spriteName)
				{
					result = current;
					break;
				}
			}
		}
		return result;
	}

	public bool SetSprite(ref UISprite target, ServerItem itemData, float scale = 1f)
	{
		bool result = false;
		if (target != null)
		{
			UIAtlas atlasItemIdType = this.GetAtlasItemIdType(itemData.idType);
			if (atlasItemIdType != null)
			{
				string serverItemSpriteName = itemData.serverItemSpriteName;
				if (serverItemSpriteName != null && serverItemSpriteName != string.Empty)
				{
					List<string> spriteNameList = AtlasIntermediary.GetSpriteNameList(atlasItemIdType);
					foreach (string current in spriteNameList)
					{
						if (current == serverItemSpriteName)
						{
							UISpriteData spriteData = AtlasIntermediary.GetSpriteData(atlasItemIdType, serverItemSpriteName);
							target.atlas = atlasItemIdType;
							target.spriteName = serverItemSpriteName;
							if (scale >= 0f)
							{
								int num = spriteData.paddingLeft + spriteData.paddingRight;
								int num2 = spriteData.paddingTop + spriteData.paddingBottom;
								target.width = (int)((float)(spriteData.width + num) * scale);
								target.height = (int)((float)(spriteData.height + num2) * scale);
							}
							result = true;
							break;
						}
					}
				}
			}
		}
		return result;
	}
}
