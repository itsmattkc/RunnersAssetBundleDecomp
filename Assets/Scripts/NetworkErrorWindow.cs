using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetworkErrorWindow : MonoBehaviour
{
	public enum ButtonType
	{
		Ok,
		YesNo,
		HomePage,
		Repayment,
		TextOnly
	}

	public struct CInfo
	{
		public delegate void FinishedCloseDelegate();

		public string name;

		public NetworkErrorWindow.ButtonType buttonType;

		public string anchor_path;

		public GameObject parentGameObject;

		public string caption;

		public string message;

		public NetworkErrorWindow.CInfo.FinishedCloseDelegate finishedCloseDelegate;

		public bool disableButton;

		public bool isPlayErrorSe;
	}

	private struct Pressed
	{
		public bool m_isButtonPressed;

		public bool m_isOkButtonPressed;

		public bool m_isYesButtonPressed;

		public bool m_isNoButtonPressed;
	}

	private sealed class _OnFinishedCloseAnimCoroutine_c__Iterator58 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float _waitStartTime___0;

		internal float _currentTime___1;

		internal float _elapseTime___2;

		internal int _PC;

		internal object _current;

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
				this._waitStartTime___0 = Time.unscaledTime;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._currentTime___1 = Time.unscaledTime;
			this._elapseTime___2 = this._currentTime___1 - this._waitStartTime___0;
			if (this._elapseTime___2 < 0.5f)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			NetworkErrorWindow.m_resultPressed = NetworkErrorWindow.m_pressed;
			if (NetworkErrorWindow.m_info.finishedCloseDelegate != null)
			{
				NetworkErrorWindow.m_info.finishedCloseDelegate();
			}
			NetworkErrorWindow.DestroyWindow();
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

	private const char FORM_FEED_CODE = '\f';

	private static GameObject m_windowPrefab;

	private static GameObject m_windowObject;

	private static NetworkErrorWindow.CInfo m_info;

	private static bool m_disableButton;

	private static bool m_created;

	private static UILabel m_captionLabel;

	private static NetworkErrorWindow.Pressed m_pressed;

	private static NetworkErrorWindow.Pressed m_resultPressed;

	private static string[] m_messages;

	private static int m_messageCount;

	public static NetworkErrorWindow.CInfo Info
	{
		get
		{
			return NetworkErrorWindow.m_info;
		}
	}

	public static bool Created
	{
		get
		{
			return NetworkErrorWindow.m_created;
		}
	}

	public static bool IsButtonPressed
	{
		get
		{
			return NetworkErrorWindow.m_resultPressed.m_isButtonPressed;
		}
	}

	public static bool IsOkButtonPressed
	{
		get
		{
			return NetworkErrorWindow.m_resultPressed.m_isOkButtonPressed;
		}
	}

	public static bool IsYesButtonPressed
	{
		get
		{
			return NetworkErrorWindow.m_resultPressed.m_isYesButtonPressed;
		}
	}

	public static bool IsNoButtonPressed
	{
		get
		{
			return NetworkErrorWindow.m_resultPressed.m_isNoButtonPressed;
		}
	}

	private void Start()
	{
		NetworkErrorWindow.m_windowObject = base.gameObject;
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		Transform parent;
		if (NetworkErrorWindow.m_info.parentGameObject != null)
		{
			parent = GameObjectUtil.FindChildGameObject(NetworkErrorWindow.m_info.parentGameObject, "Anchor_5_MC").transform;
		}
		else if (NetworkErrorWindow.m_info.anchor_path != null)
		{
			parent = gameObject.transform.Find(NetworkErrorWindow.m_info.anchor_path);
		}
		else
		{
			parent = gameObject.transform.Find("Camera/Anchor_5_MC");
		}
		Vector3 localPosition = base.transform.localPosition;
		Vector3 localScale = base.transform.localScale;
		base.transform.parent = parent;
		base.transform.localPosition = localPosition;
		base.transform.localScale = localScale;
		NetworkErrorWindow.SetDisableButton(NetworkErrorWindow.m_disableButton);
		GameObject gameObject2 = NetworkErrorWindow.m_windowObject.transform.Find("window/Lbl_caption").gameObject;
		NetworkErrorWindow.m_captionLabel = gameObject2.GetComponent<UILabel>();
		NetworkErrorWindow.m_captionLabel.text = NetworkErrorWindow.m_info.caption;
		GameObject gameObject3 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/textView/ScrollView/Lbl_body").gameObject;
		string str = "window/pattern_btn_use/textView/";
		Transform transform = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use");
		Transform transform2 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/textView");
		Transform transform3 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/pattern_0");
		Transform transform4 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/pattern_1");
		Transform transform5 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/pattern_2");
		Transform transform6 = NetworkErrorWindow.m_windowObject.transform.Find("window/pattern_btn_use/pattern_3");
		transform.gameObject.SetActive(true);
		transform2.gameObject.SetActive(true);
		switch (NetworkErrorWindow.m_info.buttonType)
		{
		case NetworkErrorWindow.ButtonType.Ok:
			transform3.gameObject.SetActive(true);
			transform4.gameObject.SetActive(false);
			transform5.gameObject.SetActive(false);
			transform6.gameObject.SetActive(false);
			break;
		case NetworkErrorWindow.ButtonType.YesNo:
			transform3.gameObject.SetActive(false);
			transform4.gameObject.SetActive(true);
			transform5.gameObject.SetActive(false);
			transform6.gameObject.SetActive(false);
			break;
		case NetworkErrorWindow.ButtonType.HomePage:
			transform3.gameObject.SetActive(false);
			transform4.gameObject.SetActive(false);
			transform5.gameObject.SetActive(true);
			transform6.gameObject.SetActive(false);
			break;
		case NetworkErrorWindow.ButtonType.Repayment:
			transform3.gameObject.SetActive(false);
			transform4.gameObject.SetActive(false);
			transform5.gameObject.SetActive(false);
			transform6.gameObject.SetActive(true);
			break;
		case NetworkErrorWindow.ButtonType.TextOnly:
			transform3.gameObject.SetActive(false);
			transform4.gameObject.SetActive(false);
			transform5.gameObject.SetActive(false);
			transform6.gameObject.SetActive(false);
			break;
		}
		NetworkErrorWindow.m_messages = null;
		NetworkErrorWindow.m_messageCount = 0;
		if (NetworkErrorWindow.m_info.message != null)
		{
			NetworkErrorWindow.m_messages = NetworkErrorWindow.m_info.message.Split(new char[]
			{
				'\f'
			});
			UILabel component = gameObject3.GetComponent<UILabel>();
			component.text = ((NetworkErrorWindow.m_messages.Length < 1) ? null : NetworkErrorWindow.m_messages[NetworkErrorWindow.m_messageCount++]);
			GameObject gameObject4 = NetworkErrorWindow.m_windowObject.transform.Find(str + "ScrollView").gameObject;
			UIPanel component2 = gameObject4.GetComponent<UIPanel>();
			float w = component2.clipRange.w;
			float num = component.printedSize.y * component.transform.localScale.y;
			if (num <= w)
			{
				BoxCollider component3 = NetworkErrorWindow.m_windowObject.transform.Find(str + "ScrollOutline").GetComponent<BoxCollider>();
				component3.enabled = false;
			}
		}
		if (NetworkErrorWindow.m_info.isPlayErrorSe)
		{
			SoundManager.SePlay("sys_error", "SE");
		}
	}

	public static GameObject Create(NetworkErrorWindow.CInfo info)
	{
		NetworkErrorWindow.SetUIEffect(false);
		NetworkErrorWindow.m_info = info;
		NetworkErrorWindow.m_disableButton = info.disableButton;
		NetworkErrorWindow.m_pressed.m_isButtonPressed = false;
		NetworkErrorWindow.m_pressed.m_isOkButtonPressed = false;
		NetworkErrorWindow.m_pressed.m_isYesButtonPressed = false;
		NetworkErrorWindow.m_pressed.m_isNoButtonPressed = false;
		NetworkErrorWindow.m_resultPressed = NetworkErrorWindow.m_pressed;
		NetworkErrorWindow.m_created = true;
		if (NetworkErrorWindow.m_windowPrefab == null)
		{
			NetworkErrorWindow.m_windowPrefab = (Resources.Load("Prefabs/UI/NetworkErrorWindow") as GameObject);
			GameObject gameObject = GameObjectUtil.FindChildGameObject(NetworkErrorWindow.m_windowPrefab, "pattern_0");
			if (gameObject != null)
			{
				UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(gameObject, "Btn_ok");
				if (uIPlayAnimation != null)
				{
					uIPlayAnimation.enabled = false;
				}
			}
		}
		if (NetworkErrorWindow.m_windowObject != null)
		{
			return null;
		}
		NetworkErrorWindow.m_windowObject = (UnityEngine.Object.Instantiate(NetworkErrorWindow.m_windowPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		NetworkErrorWindow.m_windowObject.SetActive(true);
		SoundManager.SePlay("sys_window_open", "SE");
		Animation component = NetworkErrorWindow.m_windowObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation.Play(component, Direction.Forward);
		}
		return NetworkErrorWindow.m_windowObject;
	}

	public static bool Close()
	{
		NetworkErrorWindow.m_info = default(NetworkErrorWindow.CInfo);
		NetworkErrorWindow.m_pressed = default(NetworkErrorWindow.Pressed);
		NetworkErrorWindow.m_resultPressed = default(NetworkErrorWindow.Pressed);
		NetworkErrorWindow.m_created = false;
		if (NetworkErrorWindow.m_windowObject != null)
		{
			SoundManager.SePlay("sys_window_close", "SE");
			NetworkErrorWindow.DestroyWindow();
			return true;
		}
		return false;
	}

	public static void ResetButton()
	{
		NetworkErrorWindow.m_pressed = default(NetworkErrorWindow.Pressed);
		NetworkErrorWindow.m_resultPressed = default(NetworkErrorWindow.Pressed);
	}

	public static bool IsCreated(string name)
	{
		return NetworkErrorWindow.Info.name == name;
	}

	private void OnClickOkButton()
	{
		if (!NetworkErrorWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			NetworkErrorWindow.m_pressed.m_isOkButtonPressed = true;
			NetworkErrorWindow.m_pressed.m_isButtonPressed = true;
			Animation component = NetworkErrorWindow.m_windowObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedCloseAnim), true);
			}
		}
	}

	private void OnClickRepaymetButton()
	{
		if (!NetworkErrorWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			NetworkErrorWindow.m_pressed.m_isButtonPressed = true;
			NetworkErrorWindow.m_resultPressed.m_isButtonPressed = true;
		}
	}

	private void OnClickYesButton()
	{
		if (!NetworkErrorWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			NetworkErrorWindow.m_pressed.m_isYesButtonPressed = true;
			NetworkErrorWindow.m_pressed.m_isButtonPressed = true;
		}
	}

	private void OnClickNoButton()
	{
		if (!NetworkErrorWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_window_close", "SE");
			NetworkErrorWindow.m_pressed.m_isNoButtonPressed = true;
			NetworkErrorWindow.m_pressed.m_isButtonPressed = true;
		}
	}

	public void OnFinishedCloseAnim()
	{
		base.StartCoroutine(this.OnFinishedCloseAnimCoroutine());
	}

	private IEnumerator OnFinishedCloseAnimCoroutine()
	{
		return new NetworkErrorWindow._OnFinishedCloseAnimCoroutine_c__Iterator58();
	}

	private static void DestroyWindow()
	{
		if (NetworkErrorWindow.m_windowObject != null)
		{
			UnityEngine.Object.Destroy(NetworkErrorWindow.m_windowObject);
			NetworkErrorWindow.m_windowObject = null;
		}
		NetworkErrorWindow.SetUIEffect(true);
	}

	public static void SetDisableButton(bool disableButton)
	{
		NetworkErrorWindow.m_disableButton = disableButton;
		if (NetworkErrorWindow.m_windowObject != null)
		{
			UIButton[] componentsInChildren = NetworkErrorWindow.m_windowObject.GetComponentsInChildren<UIButton>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UIButton uIButton = componentsInChildren[i];
				uIButton.isEnabled = !NetworkErrorWindow.m_disableButton;
			}
			UIImageButton[] componentsInChildren2 = NetworkErrorWindow.m_windowObject.GetComponentsInChildren<UIImageButton>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				UIImageButton uIImageButton = componentsInChildren2[j];
				uIImageButton.isEnabled = !NetworkErrorWindow.m_disableButton;
			}
		}
	}

	private static void SetUIEffect(bool flag)
	{
		if (UIEffectManager.Instance != null)
		{
			UIEffectManager.Instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, flag);
		}
	}

	private static void SendButtonMessage(string patternName, string btnName)
	{
		if (NetworkErrorWindow.m_windowObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(NetworkErrorWindow.m_windowObject, patternName);
			if (gameObject != null)
			{
				UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(gameObject, btnName);
				if (uIButtonMessage != null)
				{
					uIButtonMessage.SendMessage("OnClick");
				}
			}
		}
	}

	public static void OnClickPlatformBackButton()
	{
		if (NetworkErrorWindow.m_created)
		{
			switch (NetworkErrorWindow.m_info.buttonType)
			{
			case NetworkErrorWindow.ButtonType.Ok:
				NetworkErrorWindow.SendButtonMessage("pattern_0", "Btn_ok");
				break;
			case NetworkErrorWindow.ButtonType.YesNo:
				NetworkErrorWindow.SendButtonMessage("pattern_1", "Btn_no");
				break;
			case NetworkErrorWindow.ButtonType.HomePage:
				NetworkErrorWindow.SendButtonMessage("pattern_2", "Btn_no");
				break;
			}
		}
	}
}
