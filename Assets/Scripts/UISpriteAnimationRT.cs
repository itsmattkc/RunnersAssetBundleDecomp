using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite AnimationRT"), ExecuteInEditMode, RequireComponent(typeof(UISprite))]
public class UISpriteAnimationRT : MonoBehaviour
{
	[SerializeField]
	private int m_frameRate = 30;

	[SerializeField]
	private string m_namePrefix = string.Empty;

	[SerializeField]
	private bool m_loop = true;

	private UISprite mSprite;

	private float mDelta;

	private int mIndex;

	private bool mActive = true;

	private List<string> mSpriteNames = new List<string>();

	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	public int framesPerSecond
	{
		get
		{
			return this.m_frameRate;
		}
		set
		{
			this.m_frameRate = value;
		}
	}

	public string namePrefix
	{
		get
		{
			return this.m_namePrefix;
		}
		set
		{
			if (this.m_namePrefix != value)
			{
				this.m_namePrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	public bool loop
	{
		get
		{
			return this.m_loop;
		}
		set
		{
			this.m_loop = value;
		}
	}

	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	private void Start()
	{
		this.RebuildSpriteList();
	}

	private void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && (float)this.m_frameRate > 0f)
		{
			this.mDelta += Time.unscaledDeltaTime;
			float num = 1f / (float)this.m_frameRate;
			if (num < this.mDelta)
			{
				this.mDelta = ((num <= 0f) ? 0f : (this.mDelta - num));
				if (++this.mIndex >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.loop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					this.mSprite.MakePixelPerfect();
				}
			}
		}
	}

	private void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uISpriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.m_namePrefix) || uISpriteData.name.StartsWith(this.m_namePrefix))
				{
					this.mSpriteNames.Add(uISpriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	public void Reset()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			this.mSprite.MakePixelPerfect();
		}
	}
}
