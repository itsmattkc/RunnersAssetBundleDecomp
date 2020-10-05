using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class PlayerGetWindowUI : MonoBehaviour
{
	private sealed class _LateOpenWindow_c__Iterator43 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ServerItem serverItem;

		internal int _PC;

		internal object _current;

		internal ServerItem ___serverItem;

		internal PlayerGetWindowUI __f__this;

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
				SoundManager.SePlay("sys_window_open", "SE");
				this.__f__this.UpdateView(this.serverItem);
				ActiveAnimation.Play(this.__f__this.m_openAnimation, Direction.Forward);
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

	[SerializeField]
	public Animation m_openAnimation;

	[SerializeField]
	public UILabel m_nameLabel;

	[SerializeField]
	public UILabel m_levelLabel;

	[SerializeField]
	public UILabel m_detailsLabel;

	[SerializeField]
	public UISprite m_playerSprite;

	[SerializeField]
	public UISprite m_typeSprite;

	[SerializeField]
	public UISprite m_attrSprite;

	private GameObject m_calledGameObject;

	public static void OpenWindow(GameObject calledGameObject, ServerItem serverItem)
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "PlayerGetWindowUI");
			if (gameObject2 != null)
			{
				PlayerGetWindowUI component = gameObject2.GetComponent<PlayerGetWindowUI>();
				if (component != null)
				{
					component.OpenWindowSub(calledGameObject, serverItem);
				}
			}
		}
	}

	private void OpenWindowSub(GameObject calledGameObject, ServerItem serverItem)
	{
		this.m_calledGameObject = calledGameObject;
		base.gameObject.SetActive(true);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "player_get_window");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		base.StartCoroutine(this.LateOpenWindow(serverItem));
	}

	private IEnumerator LateOpenWindow(ServerItem serverItem)
	{
		PlayerGetWindowUI._LateOpenWindow_c__Iterator43 _LateOpenWindow_c__Iterator = new PlayerGetWindowUI._LateOpenWindow_c__Iterator43();
		_LateOpenWindow_c__Iterator.serverItem = serverItem;
		_LateOpenWindow_c__Iterator.___serverItem = serverItem;
		_LateOpenWindow_c__Iterator.__f__this = this;
		return _LateOpenWindow_c__Iterator;
	}

	private void UpdateView(ServerItem serverItem)
	{
		CharaType charaType = serverItem.charaType;
		this.m_nameLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)charaType]).text;
		this.m_levelLabel.text = TextUtility.GetTextLevel(MenuPlayerSetUtil.GetTotalLevel(charaType).ToString("D3"));
		this.m_detailsLabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_attribute_" + CharaName.Name[(int)charaType]).text;
		this.m_playerSprite.spriteName = HudUtility.MakeCharaTextureName(charaType, HudUtility.TextureType.TYPE_L);
		CharacterAttribute characterAttribute = CharacterAttribute.UNKNOWN;
		TeamAttribute teamAttribute = TeamAttribute.UNKNOWN;
		if (CharacterDataNameInfo.Instance)
		{
			CharacterDataNameInfo.Info dataByID = CharacterDataNameInfo.Instance.GetDataByID(charaType);
			if (dataByID != null)
			{
				characterAttribute = dataByID.m_attribute;
				teamAttribute = dataByID.m_teamAttribute;
			}
		}
		UISprite arg_D9_0 = this.m_typeSprite;
		string arg_D4_0 = "ui_mm_player_species_";
		int num = (int)characterAttribute;
		arg_D9_0.spriteName = arg_D4_0 + num.ToString();
		UISprite arg_F8_0 = this.m_attrSprite;
		string arg_F3_0 = "ui_mm_player_genus_";
		int num2 = (int)teamAttribute;
		arg_F8_0.spriteName = arg_F3_0 + num2.ToString();
	}

	private void OnClickOkButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public void OnFinishedCloseAnim()
	{
		if (this.m_calledGameObject != null)
		{
			this.m_calledGameObject.SendMessage("OnClosedCharaGetWindow", SendMessageOptions.DontRequireReceiver);
		}
	}
}
