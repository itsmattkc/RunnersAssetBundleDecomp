using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class AtlasListTrace : MonoBehaviour
{
	private class SpriteInfo
	{
		private UISprite m_sprite;

		private UIAtlas m_atlas;

		public UISprite sprite
		{
			get
			{
				return this.m_sprite;
			}
			private set
			{
			}
		}

		public UIAtlas atlas
		{
			get
			{
				return this.m_atlas;
			}
			private set
			{
			}
		}

		public SpriteInfo(UISprite sprite, UIAtlas atlas)
		{
			this.m_sprite = sprite;
			this.m_atlas = atlas;
		}
	}

	private sealed class _ProcessCoroutine_c__IteratorC : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _rootObject___0;

		internal List<AtlasListTrace.SpriteInfo> _spriteInfoList___1;

		internal StringBuilder _log___2;

		internal List<AtlasListTrace.SpriteInfo>.Enumerator __s_173___3;

		internal AtlasListTrace.SpriteInfo _info___4;

		internal UISprite _sprite___5;

		internal UIAtlas _atlas___6;

		internal int _PC;

		internal object _current;

		internal AtlasListTrace __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
				this._current = null;
				this._PC = 3;
				return true;
			case 3u:
				this._current = null;
				this._PC = 4;
				return true;
			case 4u:
				this._current = null;
				this._PC = 5;
				return true;
			case 5u:
				this._rootObject___0 = this.__f__this.gameObject;
				if (!(this._rootObject___0 == null))
				{
					this._spriteInfoList___1 = new List<AtlasListTrace.SpriteInfo>();
					this.__f__this.SearchSpriteInfoList(this._rootObject___0, ref this._spriteInfoList___1);
					if (this._spriteInfoList___1.Count > 0)
					{
						this._log___2 = new StringBuilder();
						this._log___2.AppendLine("-----" + this._rootObject___0.name + "'s AtlasList-----");
						this.__s_173___3 = this._spriteInfoList___1.GetEnumerator();
						try
						{
							while (this.__s_173___3.MoveNext())
							{
								this._info___4 = this.__s_173___3.Current;
								if (this._info___4 != null)
								{
									this._sprite___5 = this._info___4.sprite;
									if (!(this._sprite___5 == null))
									{
										this._atlas___6 = this._info___4.atlas;
										if (!(this._atlas___6 == null))
										{
											this._log___2.AppendLine(string.Concat(new string[]
											{
												"[",
												this._atlas___6.name,
												"] is fount from [",
												this._sprite___5.name,
												"]"
											}));
										}
									}
								}
							}
						}
						finally
						{
							((IDisposable)this.__s_173___3).Dispose();
						}
						global::Debug.Log(this._log___2.ToString());
					}
					this._PC = -1;
				}
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public bool m_showAll;

	private void Start()
	{
		base.StartCoroutine(this.ProcessCoroutine());
	}

	private IEnumerator ProcessCoroutine()
	{
		AtlasListTrace._ProcessCoroutine_c__IteratorC _ProcessCoroutine_c__IteratorC = new AtlasListTrace._ProcessCoroutine_c__IteratorC();
		_ProcessCoroutine_c__IteratorC.__f__this = this;
		return _ProcessCoroutine_c__IteratorC;
	}

	private void Update()
	{
	}

	private void SearchSpriteInfoList(GameObject parentObject, ref List<AtlasListTrace.SpriteInfo> spriteInfoList)
	{
		if (parentObject == null)
		{
			return;
		}
		if (spriteInfoList == null)
		{
			return;
		}
		int childCount = parentObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = parentObject.transform.GetChild(i).gameObject;
			if (!(gameObject == null))
			{
				this.SearchSpriteInfoList(gameObject, ref spriteInfoList);
				UISprite uISprite = null;
				UIAtlas uIAtlas = null;
				uISprite = gameObject.GetComponent<UISprite>();
				if (!(uISprite == null))
				{
					uIAtlas = uISprite.atlas;
					if (!(uIAtlas == null))
					{
						bool flag = false;
						foreach (AtlasListTrace.SpriteInfo current in spriteInfoList)
						{
							if (current != null)
							{
								if (current.atlas.name == uIAtlas.name)
								{
									flag = true;
								}
							}
						}
						if (!flag || this.m_showAll)
						{
							AtlasListTrace.SpriteInfo item = new AtlasListTrace.SpriteInfo(uISprite, uIAtlas);
							spriteInfoList.Add(item);
						}
					}
				}
			}
		}
	}
}
