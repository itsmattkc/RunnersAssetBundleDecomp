using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class HudNetworkConnect : MonoBehaviour
{
	public enum DisplayType
	{
		ALL,
		NO_BG,
		ONLY_ICON,
		LOADING
	}

	private sealed class _OnWaitDisplayAnchor9_c__Iterator2B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _count___0;

		internal int _PC;

		internal object _current;

		internal HudNetworkConnect __f__this;

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
				this._count___0 = 1;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 > 0)
			{
				this._count___0--;
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.__f__this.m_imgRing != null)
			{
				this.__f__this.m_imgRing.enabled = true;
			}
			if (this.__f__this.m_imgBg != null)
			{
				this.__f__this.m_imgBg.enabled = true;
			}
			if (this.__f__this.m_labelConnectSdw != null)
			{
				this.__f__this.m_labelConnectSdw.enabled = true;
			}
			if (this.__f__this.m_labelConnect != null)
			{
				this.__f__this.m_labelConnect.enabled = true;
			}
			this._PC = -1;
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

	private sealed class _OnPlayEnd_c__Iterator2C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal Animation _animation___0;

		internal ActiveAnimation _anim___1;

		internal GameObject _anchor9___2;

		internal int _PC;

		internal object _current;

		internal HudNetworkConnect __f__this;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this.__f__this.m_isPlaying)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this._animation___0 = null;
			this._anim___1 = null;
			if (this.__f__this.m_object != null)
			{
				this._anchor9___2 = GameObjectUtil.FindChildGameObject(this.__f__this.m_object, "Anchor_9_BR");
				if (this._anchor9___2 != null)
				{
					this._animation___0 = GameObjectUtil.FindChildGameObjectComponent<Animation>(this._anchor9___2, "Animation");
					if (this._animation___0 != null)
					{
						this._anim___1 = ActiveAnimation.Play(this._animation___0, Direction.Reverse, true);
					}
				}
			}
			if (this._anim___1 != null)
			{
				EventDelegate.Add(this._anim___1.onFinished, new EventDelegate.Callback(this.__f__this.OutAnimFinishCallback), true);
			}
			else
			{
				this.__f__this.OutAnimFinishCallback();
			}
			this.__f__this.enabled = false;
			this._PC = -1;
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

	private GameObject m_object;

	private UISprite m_imgRing;

	private UISprite m_imgBg;

	private UILabel m_labelConnect;

	private UILabel m_labelConnectSdw;

	private bool m_isPlaying;

	private int m_refCount;

	private void Start()
	{
		this.Setup();
		base.enabled = false;
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		global::Debug.Log("HudNetworkConnct is Destroyed");
	}

	public GameObject Setup()
	{
		if (this.m_object != null)
		{
			return this.m_object;
		}
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			this.m_object = GameObjectUtil.FindChildGameObject(cameraUIObject, "ConnectAlertUI");
		}
		if (this.m_object != null)
		{
			this.m_imgRing = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_object, "img_ring");
			this.m_imgBg = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_object, "img_bg");
			this.m_labelConnect = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_object, "Lbl_conect_condition");
			if (this.m_imgRing != null)
			{
				this.m_imgRing.enabled = false;
			}
			if (this.m_imgBg != null)
			{
				this.m_imgBg.enabled = false;
			}
			if (this.m_labelConnect != null)
			{
				this.m_labelConnect.enabled = false;
				this.m_labelConnectSdw = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_labelConnect.gameObject, "Lbl_conect_condition_sh");
			}
			if (this.m_labelConnectSdw != null)
			{
				this.m_labelConnectSdw.enabled = false;
			}
		}
		return this.m_object;
	}

	public void PlayStart(HudNetworkConnect.DisplayType displayType)
	{
		if (this.m_refCount > 0)
		{
			return;
		}
		this.m_refCount++;
		base.enabled = true;
		if (this.m_object == null)
		{
			return;
		}
		this.m_object.SetActive(true);
		this.m_isPlaying = true;
		string text = string.Empty;
		if (displayType == HudNetworkConnect.DisplayType.LOADING)
		{
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "MainMenuLoading", "Lbl_conect_condition").text;
		}
		else
		{
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "common", "ui_Lbl_connect").text;
		}
		if (this.m_labelConnect != null)
		{
			this.m_labelConnect.text = text;
			if (this.m_labelConnectSdw != null)
			{
				this.m_labelConnectSdw.text = text;
			}
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_object, "window_bg");
		BoxCollider boxCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.m_object, "window_bg_colider");
		Animation animation = null;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_object, "Anchor_9_BR");
		if (gameObject != null)
		{
			animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject, "Animation");
		}
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, Direction.Forward, true);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimFinishCallback), true);
			}
			else
			{
				this.InAnimFinishCallback();
			}
		}
		switch (displayType)
		{
		case HudNetworkConnect.DisplayType.ALL:
			if (uISprite != null)
			{
				uISprite.enabled = true;
			}
			if (boxCollider != null)
			{
				boxCollider.enabled = true;
			}
			break;
		case HudNetworkConnect.DisplayType.NO_BG:
			if (uISprite != null)
			{
				uISprite.enabled = false;
			}
			if (boxCollider != null)
			{
				boxCollider.enabled = true;
			}
			break;
		case HudNetworkConnect.DisplayType.ONLY_ICON:
			if (uISprite != null)
			{
				uISprite.enabled = false;
			}
			if (boxCollider != null)
			{
				boxCollider.enabled = false;
			}
			break;
		}
		base.StartCoroutine(this.OnWaitDisplayAnchor9());
	}

	private IEnumerator OnWaitDisplayAnchor9()
	{
		HudNetworkConnect._OnWaitDisplayAnchor9_c__Iterator2B _OnWaitDisplayAnchor9_c__Iterator2B = new HudNetworkConnect._OnWaitDisplayAnchor9_c__Iterator2B();
		_OnWaitDisplayAnchor9_c__Iterator2B.__f__this = this;
		return _OnWaitDisplayAnchor9_c__Iterator2B;
	}

	public void PlayEnd()
	{
		this.m_refCount--;
		if (this.m_refCount > 0)
		{
			return;
		}
		this.m_refCount = 0;
		base.StartCoroutine(this.OnPlayEnd());
	}

	private IEnumerator OnPlayEnd()
	{
		HudNetworkConnect._OnPlayEnd_c__Iterator2C _OnPlayEnd_c__Iterator2C = new HudNetworkConnect._OnPlayEnd_c__Iterator2C();
		_OnPlayEnd_c__Iterator2C.__f__this = this;
		return _OnPlayEnd_c__Iterator2C;
	}

	private void InAnimFinishCallback()
	{
		this.m_isPlaying = false;
	}

	private void OutAnimFinishCallback()
	{
		if (this.m_object != null)
		{
			this.m_object.SetActive(false);
			this.m_object = null;
		}
	}
}
