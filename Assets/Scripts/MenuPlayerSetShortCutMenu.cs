using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MenuPlayerSetShortCutMenu : MonoBehaviour
{
	public delegate void ShortCutCallback(CharaType charaType);

	private sealed class _OnSetupCoroutine_c__Iterator4B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal List<GameObject> _buttonObjectList___0;

		internal int _openedCharaCount___1;

		internal int _buttonIndex___2;

		internal int _charaIndex___3;

		internal CharaType _charaType___4;

		internal GameObject _buttonObject___5;

		internal MenuPlayerSetShortCutButton _button___6;

		internal bool _isLocked___7;

		internal UIRectItemStorage _itemStorage___8;

		internal UIPanel _panel___9;

		internal GameObject _scrollButtonRoot___10;

		internal GameObject _parentObject___11;

		internal GameObject _leftButtonObject___12;

		internal UIButtonMessage _buttonMessage___13;

		internal GameObject _rightButtonObject___14;

		internal UIButtonMessage _buttonMessage___15;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSetShortCutMenu __f__this;

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
				this._buttonObjectList___0 = null;
				this._openedCharaCount___1 = MenuPlayerSetUtil.GetOpenedCharaCount();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._buttonObjectList___0 = GameObjectUtil.FindChildGameObjects(this.__f__this.gameObject, "ui_player_set_pager_scroll(Clone)");
			if (this._buttonObjectList___0 == null || this._buttonObjectList___0.Count < this._openedCharaCount___1)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this._buttonIndex___2 = 0;
			this._charaIndex___3 = 0;
			while (this._charaIndex___3 < 29)
			{
				if (this._buttonIndex___2 >= this._buttonObjectList___0.Count)
				{
					break;
				}
				this._charaType___4 = (CharaType)this._charaIndex___3;
				if (MenuPlayerSetUtil.IsOpenedCharacter(this._charaType___4))
				{
					this._buttonObject___5 = this._buttonObjectList___0[this._buttonIndex___2];
					this._buttonIndex___2++;
					if (!(this._buttonObject___5 == null))
					{
						this._button___6 = this._buttonObject___5.AddComponent<MenuPlayerSetShortCutButton>();
						this._isLocked___7 = (SaveDataManager.Instance.CharaData.Status[this._charaIndex___3] == 0);
						this._button___6.Setup(this._charaType___4, this._isLocked___7);
						this._button___6.SetCallback(new MenuPlayerSetShortCutButton.ButtonClickedCallback(this.__f__this.ButtonClickedCallback));
						this.__f__this.m_buttons.Add(this._button___6);
					}
				}
				this._charaIndex___3++;
			}
			this._itemStorage___8 = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.__f__this.gameObject, "slot");
			if (this._itemStorage___8 != null)
			{
				this.__f__this.m_buttonSpace = (float)this._itemStorage___8.spacing_x;
				this._panel___9 = this.__f__this.gameObject.GetComponent<UIPanel>();
				if (this._panel___9 != null)
				{
					this.__f__this.m_displayIconCount = (int)(this._panel___9.clipRange.z / this.__f__this.m_buttonSpace);
				}
			}
			this._scrollButtonRoot___10 = null;
			this._parentObject___11 = this.__f__this.gameObject.transform.parent.gameObject;
			this._scrollButtonRoot___10 = GameObjectUtil.FindChildGameObject(this._parentObject___11, "player_set_scroll_other");
			this._leftButtonObject___12 = GameObjectUtil.FindChildGameObject(this._scrollButtonRoot___10, "Btn_icon_arrow_lt");
			if (this._leftButtonObject___12 != null)
			{
				this._buttonMessage___13 = this._leftButtonObject___12.GetComponent<UIButtonMessage>();
				if (this._buttonMessage___13 == null)
				{
					this._buttonMessage___13 = this._leftButtonObject___12.AddComponent<UIButtonMessage>();
				}
				this._buttonMessage___13.target = this.__f__this.gameObject;
				this._buttonMessage___13.functionName = "LeftButtonClickedCallback";
			}
			this._rightButtonObject___14 = GameObjectUtil.FindChildGameObject(this._scrollButtonRoot___10, "Btn_icon_arrow_rt");
			if (this._rightButtonObject___14 != null)
			{
				this._buttonMessage___15 = this._rightButtonObject___14.GetComponent<UIButtonMessage>();
				if (this._buttonMessage___15 == null)
				{
					this._buttonMessage___15 = this._rightButtonObject___14.AddComponent<UIButtonMessage>();
				}
				this._buttonMessage___15.target = this.__f__this.gameObject;
				this._buttonMessage___15.functionName = "RightButtonClickedCallback";
			}
			this.__f__this.m_isEndSetup = true;
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

	private sealed class _OnSetActiveCharacter_c__Iterator4C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal CharaType charaType;

		internal int _pageIndex___0;

		internal int _maxPage___1;

		internal bool isSetup;

		internal UIDraggablePanel _draggablePanel___2;

		internal int _moveDistance___3;

		internal UIDraggablePanel _draggablePanel___4;

		internal int _moveDistance___5;

		internal GameObject _scrollButtonRoot___6;

		internal GameObject _parentObject___7;

		internal BoxCollider _boxCollider___8;

		internal BoxCollider _boxCollider___9;

		internal BoxCollider _boxColliderL___10;

		internal BoxCollider _boxColliderR___11;

		internal int _PC;

		internal object _current;

		internal CharaType ___charaType;

		internal bool ___isSetup;

		internal MenuPlayerSetShortCutMenu __f__this;

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
				this._pageIndex___0 = MenuPlayerSetUtil.GetPageIndexFromCharaType(this.charaType);
				this._maxPage___1 = MenuPlayerSetUtil.GetOpenedCharaCount();
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				if (this.isSetup)
				{
					this._draggablePanel___2 = this.__f__this.gameObject.GetComponent<UIDraggablePanel>();
					if (this._draggablePanel___2 != null)
					{
						this._moveDistance___3 = -(int)this.__f__this.m_buttonSpace * (this.__f__this.m_displayIconCount / 2);
						this._draggablePanel___2.MoveRelative(new Vector3((float)(-(float)this._moveDistance___3), 0f, 0f));
					}
				}
				else
				{
					this._draggablePanel___4 = this.__f__this.gameObject.GetComponent<UIDraggablePanel>();
					if (this._draggablePanel___4 != null)
					{
						this._moveDistance___5 = -(int)this.__f__this.m_buttonSpace * (this.__f__this.m_displayIconCount / 2);
						this._moveDistance___5 += (int)this.__f__this.m_buttonSpace * this._pageIndex___0;
						this._draggablePanel___4.ResetPosition();
						this._draggablePanel___4.MoveRelative(new Vector3((float)(-(float)this._moveDistance___5), 0f, 0f));
					}
				}
				this._scrollButtonRoot___6 = null;
				this._parentObject___7 = this.__f__this.gameObject.transform.parent.gameObject;
				this._scrollButtonRoot___6 = GameObjectUtil.FindChildGameObject(this._parentObject___7, "player_set_scroll_other");
				if (this._pageIndex___0 == 0)
				{
					this._boxCollider___8 = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this._scrollButtonRoot___6, "Btn_icon_arrow_lt");
					if (this._boxCollider___8 != null)
					{
						this._boxCollider___8.isTrigger = true;
					}
				}
				else if (this._pageIndex___0 >= this._maxPage___1 - 1)
				{
					this._boxCollider___9 = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this._scrollButtonRoot___6, "Btn_icon_arrow_rt");
					if (this._boxCollider___9 != null)
					{
						this._boxCollider___9.isTrigger = true;
					}
				}
				else
				{
					this._boxColliderL___10 = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this._scrollButtonRoot___6, "Btn_icon_arrow_lt");
					if (this._boxColliderL___10 != null)
					{
						this._boxColliderL___10.isTrigger = false;
					}
					this._boxColliderR___11 = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this._scrollButtonRoot___6, "Btn_icon_arrow_rt");
					if (this._boxColliderR___11 != null)
					{
						this._boxColliderR___11.isTrigger = false;
					}
				}
				if (this.__f__this.m_prevActivePageIndex >= 0)
				{
					this.__f__this.m_buttons[this.__f__this.m_prevActivePageIndex].SetButtonActive(false);
				}
				this.__f__this.m_buttons[this._pageIndex___0].SetButtonActive(true);
				this.__f__this.m_prevActivePageIndex = this._pageIndex___0;
				this._PC = -1;
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

	private List<MenuPlayerSetShortCutButton> m_buttons = new List<MenuPlayerSetShortCutButton>();

	private MenuPlayerSetShortCutMenu.ShortCutCallback m_callback;

	private bool m_isEndSetup;

	private int m_prevActivePageIndex = -1;

	private float m_buttonSpace = 1f;

	private int m_displayIconCount = 1;

	public bool IsEndSetup
	{
		get
		{
			return this.m_isEndSetup;
		}
		private set
		{
		}
	}

	private void OnEnable()
	{
		if (!this.m_isEndSetup)
		{
			return;
		}
		foreach (MenuPlayerSetShortCutButton current in this.m_buttons)
		{
			if (!(current == null))
			{
				CharaType chara = current.Chara;
				if (SaveDataManager.Instance.CharaData.Status[(int)chara] != 0)
				{
					current.SetIconLock(false);
				}
			}
		}
	}

	public void Setup(MenuPlayerSetShortCutMenu.ShortCutCallback callback)
	{
		this.m_callback = callback;
		base.StartCoroutine(this.OnSetupCoroutine());
	}

	private IEnumerator OnSetupCoroutine()
	{
		MenuPlayerSetShortCutMenu._OnSetupCoroutine_c__Iterator4B _OnSetupCoroutine_c__Iterator4B = new MenuPlayerSetShortCutMenu._OnSetupCoroutine_c__Iterator4B();
		_OnSetupCoroutine_c__Iterator4B.__f__this = this;
		return _OnSetupCoroutine_c__Iterator4B;
	}

	public void SetActiveCharacter(CharaType charaType, bool isSetup)
	{
		base.StartCoroutine(this.OnSetActiveCharacter(charaType, isSetup));
	}

	public IEnumerator OnSetActiveCharacter(CharaType charaType, bool isSetup)
	{
		MenuPlayerSetShortCutMenu._OnSetActiveCharacter_c__Iterator4C _OnSetActiveCharacter_c__Iterator4C = new MenuPlayerSetShortCutMenu._OnSetActiveCharacter_c__Iterator4C();
		_OnSetActiveCharacter_c__Iterator4C.charaType = charaType;
		_OnSetActiveCharacter_c__Iterator4C.isSetup = isSetup;
		_OnSetActiveCharacter_c__Iterator4C.___charaType = charaType;
		_OnSetActiveCharacter_c__Iterator4C.___isSetup = isSetup;
		_OnSetActiveCharacter_c__Iterator4C.__f__this = this;
		return _OnSetActiveCharacter_c__Iterator4C;
	}

	public void UnclockCharacter(CharaType charaType)
	{
		int pageIndexFromCharaType = MenuPlayerSetUtil.GetPageIndexFromCharaType(charaType);
		MenuPlayerSetShortCutButton menuPlayerSetShortCutButton = this.m_buttons[pageIndexFromCharaType];
		if (menuPlayerSetShortCutButton == null)
		{
			return;
		}
		menuPlayerSetShortCutButton.SetIconLock(false);
		menuPlayerSetShortCutButton.SetButtonActive(true);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void ButtonClickedCallback(CharaType charaType)
	{
		if (this.m_callback != null)
		{
			this.m_callback(charaType);
		}
	}

	private void LeftButtonClickedCallback()
	{
		int num = this.m_prevActivePageIndex - 1;
		if (num <= 0)
		{
			num = 0;
		}
		CharaType charaTypeFromPageIndex = MenuPlayerSetUtil.GetCharaTypeFromPageIndex(num);
		this.ButtonClickedCallback(charaTypeFromPageIndex);
	}

	private void RightButtonClickedCallback()
	{
		int num = this.m_prevActivePageIndex + 1;
		int openedCharaCount = MenuPlayerSetUtil.GetOpenedCharaCount();
		if (num >= openedCharaCount - 1)
		{
			num = openedCharaCount - 1;
		}
		CharaType charaTypeFromPageIndex = MenuPlayerSetUtil.GetCharaTypeFromPageIndex(num);
		this.ButtonClickedCallback(charaTypeFromPageIndex);
	}
}
