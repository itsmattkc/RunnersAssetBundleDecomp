using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Camera"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	public enum ClickNotification
	{
		None,
		Always,
		BasedOnDelta
	}

	public class MouseOrTouch
	{
		public Vector2 pos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject current;

		public GameObject pressed;

		public GameObject dragged;

		public float clickTime;

		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		public bool touchBegan = true;

		public bool pressStarted;

		public bool dragStarted;
	}

	public enum EventType
	{
		World,
		UI
	}

	private class Highlighted
	{
		public GameObject go;

		public int counter;
	}

	private struct DepthEntry
	{
		public int depth;

		public RaycastHit hit;
	}

	public delegate void OnCustomInput();

	public static List<UICamera> list = new List<UICamera>();

	public UICamera.EventType eventType = UICamera.EventType.UI;

	public LayerMask eventReceiverMask = -1;

	public bool debug;

	public bool useMouse = true;

	public bool useTouch = true;

	public bool allowMultiTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public bool stickyPress = true;

	public bool stickyTooltip = true;

	public float tooltipDelay = 1f;

	public float mouseDragThreshold = 4f;

	public float mouseClickThreshold = 10f;

	public float touchDragThreshold = 40f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public string verticalAxisName = "Vertical";

	public string horizontalAxisName = "Horizontal";

	public KeyCode submitKey0 = KeyCode.Return;

	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	public KeyCode cancelKey0 = KeyCode.Escape;

	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	public static UICamera.OnCustomInput onCustomInput;

	public static bool showTooltips = true;

	public static Vector2 lastTouchPosition = Vector2.zero;

	public static RaycastHit lastHit;

	public static UICamera current = null;

	public static Camera currentCamera = null;

	public static int currentTouchID = -1;

	public static UICamera.MouseOrTouch currentTouch = null;

	public static bool inputHasFocus = false;

	public static GameObject genericEventHandler;

	public static GameObject fallThrough;

	private static List<UICamera.Highlighted> mHighlighted = new List<UICamera.Highlighted>();

	private static GameObject mSel = null;

	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	private static GameObject mHover;

	private static UICamera.MouseOrTouch mController = new UICamera.MouseOrTouch();

	private static float mNextEvent = 0f;

	private static Dictionary<int, UICamera.MouseOrTouch> mTouches = new Dictionary<int, UICamera.MouseOrTouch>();

	private GameObject mTooltip;

	private Camera mCam;

	private LayerMask mLayerMask;

	private float mTooltipTime;

	private bool mIsEditor;

	public static bool isDragging = false;

	public static GameObject hoveredObject;

	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	private static RaycastHit mEmpty = default(RaycastHit);

	private static Comparison<UICamera.DepthEntry> __f__am_cache34;

	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			return UICamera.mSel;
		}
		set
		{
			if (UICamera.mSel != value)
			{
				if (UICamera.mSel != null)
				{
					UICamera uICamera = UICamera.FindCameraForLayer(UICamera.mSel.layer);
					if (uICamera != null)
					{
						UICamera.current = uICamera;
						UICamera.currentCamera = uICamera.mCam;
						UICamera.Notify(UICamera.mSel, "OnSelect", false);
						if (uICamera.useController || uICamera.useKeyboard)
						{
							UICamera.Highlight(UICamera.mSel, false);
						}
						UICamera.current = null;
					}
				}
				UICamera.mSel = value;
				if (UICamera.mSel != null)
				{
					UICamera uICamera2 = UICamera.FindCameraForLayer(UICamera.mSel.layer);
					if (uICamera2 != null)
					{
						UICamera.current = uICamera2;
						UICamera.currentCamera = uICamera2.mCam;
						if (uICamera2.useController || uICamera2.useKeyboard)
						{
							UICamera.Highlight(UICamera.mSel, true);
						}
						UICamera.Notify(UICamera.mSel, "OnSelect", true);
						UICamera.current = null;
					}
				}
			}
		}
	}

	public static int touchCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.pressed != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].pressed != null)
				{
					num++;
				}
			}
			if (UICamera.mController.pressed != null)
			{
				num++;
			}
			return num;
		}
	}

	public static int dragCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.dragged != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.mController.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.Count; i++)
			{
				UICamera uICamera = UICamera.list[i];
				if (!(uICamera == null) && uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	private void OnApplicationQuit()
	{
		UICamera.mHighlighted.Clear();
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	public static bool Raycast(Vector3 inPos, out RaycastHit hit)
	{
		for (int i = 0; i < UICamera.list.Count; i++)
		{
			UICamera uICamera = UICamera.list[i];
			if (uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
			{
				UICamera.currentCamera = uICamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int layerMask = UICamera.currentCamera.cullingMask & uICamera.eventReceiverMask;
						float distance = (uICamera.rangeDistance <= 0f) ? (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane) : uICamera.rangeDistance;
						if (uICamera.eventType == UICamera.EventType.World)
						{
							if (Physics.Raycast(ray, out hit, distance, layerMask))
							{
								UICamera.hoveredObject = hit.collider.gameObject;
								return true;
							}
						}
						else if (uICamera.eventType == UICamera.EventType.UI)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, distance, layerMask);
							if (array.Length > 1)
							{
								for (int j = 0; j < array.Length; j++)
								{
									GameObject gameObject = array[j].collider.gameObject;
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									UICamera.mHit.hit = array[j];
									UICamera.mHits.Add(UICamera.mHit);
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										hit = UICamera.mHits[k].hit;
										UICamera.hoveredObject = hit.collider.gameObject;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1 && UICamera.IsVisible(ref array[0]))
							{
								hit = array[0];
								UICamera.hoveredObject = hit.collider.gameObject;
								return true;
							}
						}
					}
				}
			}
		}
		hit = UICamera.mEmpty;
		return false;
	}

	private static bool IsVisible(ref RaycastHit hit)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(hit.collider.gameObject);
		return uIPanel == null || uIPanel.IsVisible(hit.point);
	}

	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(de.hit.collider.gameObject);
		return uIPanel == null || uIPanel.IsVisible(de.hit.point);
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.Count; i++)
		{
			UICamera uICamera = UICamera.list[i];
			Camera cachedCamera = uICamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UnityEngine.Input.GetKeyDown(up))
		{
			return 1;
		}
		if (UnityEngine.Input.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UnityEngine.Input.GetKeyDown(up0) || UnityEngine.Input.GetKeyDown(up1))
		{
			return 1;
		}
		if (UnityEngine.Input.GetKeyDown(down0) || UnityEngine.Input.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (UICamera.mNextEvent < realtimeSinceStartup)
		{
			float axis2 = UnityEngine.Input.GetAxis(axis);
			if (axis2 > 0.75f)
			{
				UICamera.mNextEvent = realtimeSinceStartup + 0.25f;
				return 1;
			}
			if (axis2 < -0.75f)
			{
				UICamera.mNextEvent = realtimeSinceStartup + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static bool IsHighlighted(GameObject go)
	{
		int i = UICamera.mHighlighted.Count;
		while (i > 0)
		{
			UICamera.Highlighted highlighted = UICamera.mHighlighted[--i];
			if (highlighted.go == go)
			{
				return true;
			}
		}
		return false;
	}

	private static void Highlight(GameObject go, bool highlighted)
	{
		if (go != null)
		{
			int i = UICamera.mHighlighted.Count;
			while (i > 0)
			{
				UICamera.Highlighted highlighted2 = UICamera.mHighlighted[--i];
				if (highlighted2 == null || highlighted2.go == null)
				{
					UICamera.mHighlighted.RemoveAt(i);
				}
				else if (highlighted2.go == go)
				{
					if (highlighted)
					{
						highlighted2.counter++;
					}
					else if (--highlighted2.counter < 1)
					{
						UICamera.mHighlighted.Remove(highlighted2);
						UICamera.Notify(go, "OnHover", false);
					}
					return;
				}
			}
			if (highlighted)
			{
				UICamera.Highlighted highlighted3 = new UICamera.Highlighted();
				highlighted3.go = go;
				highlighted3.counter = 1;
				UICamera.mHighlighted.Add(highlighted3);
				UICamera.Notify(go, "OnHover", true);
			}
		}
	}

	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (go != null)
		{
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.genericEventHandler != null && UICamera.genericEventHandler != go)
			{
				UICamera.genericEventHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static UICamera.MouseOrTouch GetTouch(int id)
	{
		UICamera.MouseOrTouch mouseOrTouch = null;
		if (!UICamera.mTouches.TryGetValue(id, out mouseOrTouch))
		{
			mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.touchBegan = true;
			UICamera.mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	public static void RemoveTouch(int id)
	{
		UICamera.mTouches.Remove(id);
	}

	private void Awake()
	{
		this.cachedCamera.eventMask = 0;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.BB10Player)
		{
			this.useMouse = false;
			this.useTouch = true;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.useKeyboard = false;
				this.useController = false;
			}
		}
		else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360)
		{
			this.useMouse = false;
			this.useTouch = false;
			this.useKeyboard = false;
			this.useController = true;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			this.mIsEditor = true;
		}
		UICamera.mMouse[0].pos.x = UnityEngine.Input.mousePosition.x;
		UICamera.mMouse[0].pos.y = UnityEngine.Input.mousePosition.y;
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
		if (this.eventReceiverMask == -1)
		{
			this.eventReceiverMask = this.cachedCamera.cullingMask;
		}
	}

	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new Comparison<UICamera>(UICamera.CompareFunc));
	}

	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	private void FixedUpdate()
	{
		if (this.useMouse && Application.isPlaying && this.handlesEvents)
		{
			if (!UICamera.Raycast(UnityEngine.Input.mousePosition, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			for (int i = 0; i < 3; i++)
			{
				UICamera.mMouse[i].current = UICamera.hoveredObject;
			}
		}
	}

	private void Update()
	{
		if (!Application.isPlaying || !this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		if (this.useMouse || (this.useTouch && this.mIsEditor))
		{
			this.ProcessMouse();
		}
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if (this.useMouse && UICamera.mSel != null && ((this.cancelKey0 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.cancelKey0)) || (this.cancelKey1 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.cancelKey1))))
		{
			UICamera.selectedObject = null;
		}
		if (UICamera.mSel != null)
		{
			string text = Input.inputString;
			if (this.useKeyboard && UnityEngine.Input.GetKeyDown(KeyCode.Delete))
			{
				text += "\b";
			}
			if (text.Length > 0)
			{
				if (!this.stickyTooltip && this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.Notify(UICamera.mSel, "OnInput", text);
			}
		}
		else
		{
			UICamera.inputHasFocus = false;
		}
		if (UICamera.mSel != null)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float axis = UnityEngine.Input.GetAxis(this.scrollAxisName);
			if (axis != 0f)
			{
				UICamera.Notify(UICamera.mHover, "OnScroll", axis);
			}
			if (UICamera.showTooltips && this.mTooltipTime != 0f && (this.mTooltipTime < Time.realtimeSinceStartup || UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift)))
			{
				this.mTooltip = UICamera.mHover;
				this.ShowTooltip(true);
			}
		}
		UICamera.current = null;
	}

	public void ProcessMouse()
	{
		bool flag = this.useMouse && Time.timeScale < 0.9f;
		if (!flag)
		{
			for (int i = 0; i < 3; i++)
			{
				if (Input.GetMouseButton(i) || Input.GetMouseButtonUp(i))
				{
					flag = true;
					break;
				}
			}
		}
		UICamera.mMouse[0].pos = UnityEngine.Input.mousePosition;
		UICamera.mMouse[0].delta = UICamera.mMouse[0].pos - UICamera.lastTouchPosition;
		bool flag2 = UICamera.mMouse[0].pos != UICamera.lastTouchPosition;
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
		if (flag)
		{
			if (!UICamera.Raycast(UnityEngine.Input.mousePosition, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			UICamera.mMouse[0].current = UICamera.hoveredObject;
		}
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[j].delta = UICamera.mMouse[0].delta;
			UICamera.mMouse[j].current = UICamera.mMouse[0].current;
		}
		bool flag3 = false;
		for (int k = 0; k < 3; k++)
		{
			if (Input.GetMouseButton(k))
			{
				flag3 = true;
				break;
			}
		}
		if (flag3)
		{
			this.mTooltipTime = 0f;
		}
		else if (this.useMouse && flag2 && (!this.stickyTooltip || UICamera.mHover != UICamera.mMouse[0].current))
		{
			if (this.mTooltipTime != 0f)
			{
				this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
			}
			else if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
		}
		if (this.useMouse && !flag3 && UICamera.mHover != null && UICamera.mHover != UICamera.mMouse[0].current)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.Highlight(UICamera.mHover, false);
			UICamera.mHover = null;
		}
		if (this.useMouse)
		{
			for (int l = 0; l < 3; l++)
			{
				bool mouseButtonDown = Input.GetMouseButtonDown(l);
				bool mouseButtonUp = Input.GetMouseButtonUp(l);
				UICamera.currentTouch = UICamera.mMouse[l];
				UICamera.currentTouchID = -1 - l;
				if (mouseButtonDown)
				{
					UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				}
				else if (UICamera.currentTouch.pressed != null)
				{
					UICamera.currentCamera = UICamera.currentTouch.pressedCam;
				}
				this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			}
			UICamera.currentTouch = null;
		}
		if (this.useMouse && !flag3 && UICamera.mHover != UICamera.mMouse[0].current)
		{
			this.mTooltipTime = Time.realtimeSinceStartup + this.tooltipDelay;
			UICamera.mHover = UICamera.mMouse[0].current;
			UICamera.Highlight(UICamera.mHover, true);
		}
	}

	public void ProcessTouches()
	{
		for (int i = 0; i < UnityEngine.Input.touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : touch.fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
			bool flag = touch.phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			if (flag)
			{
				UICamera.currentTouch.delta = Vector2.zero;
			}
			else
			{
				UICamera.currentTouch.delta = touch.position - UICamera.currentTouch.pos;
			}
			UICamera.currentTouch.pos = touch.position;
			if (!UICamera.Raycast(UICamera.currentTouch.pos, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (touch.tapCount > 1)
			{
				UICamera.currentTouch.clickTime = Time.realtimeSinceStartup;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
	}

	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.mController;
		UICamera.inputHasFocus = (UICamera.mSel != null && UICamera.mSel.GetComponent<UIInput>() != null);
		bool flag = (this.submitKey0 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.submitKey0)) || (this.submitKey1 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.submitKey1));
		bool flag2 = (this.submitKey0 != KeyCode.None && UnityEngine.Input.GetKeyUp(this.submitKey0)) || (this.submitKey1 != KeyCode.None && UnityEngine.Input.GetKeyUp(this.submitKey1));
		if (flag || flag2)
		{
			UICamera.currentTouch.current = UICamera.mSel;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.current = null;
		}
		int num = 0;
		int num2 = 0;
		if (this.useKeyboard)
		{
			if (UICamera.inputHasFocus)
			{
				num += UICamera.GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
			}
			else
			{
				num += UICamera.GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
			}
		}
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				num += UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num2 += UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (num != 0)
		{
			UICamera.Notify(UICamera.mSel, "OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow);
		}
		if (num2 != 0)
		{
			UICamera.Notify(UICamera.mSel, "OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow);
		}
		if (this.useKeyboard && UnityEngine.Input.GetKeyDown(KeyCode.Tab))
		{
			UICamera.Notify(UICamera.mSel, "OnKey", KeyCode.Tab);
		}
		if (this.cancelKey0 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.cancelKey0))
		{
			UICamera.Notify(UICamera.mSel, "OnKey", KeyCode.Escape);
		}
		if (this.cancelKey1 != KeyCode.None && UnityEngine.Input.GetKeyDown(this.cancelKey1))
		{
			UICamera.Notify(UICamera.mSel, "OnKey", KeyCode.Escape);
		}
		UICamera.currentTouch = null;
	}

	public void ProcessTouch(bool pressed, bool unpressed)
	{
		bool flag = UICamera.currentTouch == UICamera.mMouse[0] || UICamera.currentTouch == UICamera.mMouse[1] || UICamera.currentTouch == UICamera.mMouse[2];
		float num = (!flag) ? this.touchDragThreshold : this.mouseDragThreshold;
		float num2 = (!flag) ? this.touchClickThreshold : this.mouseClickThreshold;
		if (pressed)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.currentTouch.pressStarted = true;
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = ((!flag) ? UICamera.ClickNotification.Always : UICamera.ClickNotification.BasedOnDelta);
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.currentTouch.pressed != UICamera.mSel)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.selectedObject = null;
			}
		}
		else
		{
			if (!this.stickyPress && !unpressed && UICamera.currentTouch.pressStarted && UICamera.currentTouch.pressed != UICamera.hoveredObject)
			{
				UICamera.isDragging = true;
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
				UICamera.currentTouch.pressed = UICamera.hoveredObject;
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
				UICamera.isDragging = false;
			}
			if (UICamera.currentTouch.pressed != null)
			{
				float magnitude = UICamera.currentTouch.delta.magnitude;
				if (magnitude != 0f)
				{
					UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
					magnitude = UICamera.currentTouch.totalDelta.magnitude;
					if (!UICamera.currentTouch.dragStarted && num < magnitude)
					{
						UICamera.currentTouch.dragStarted = true;
						UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
					}
					if (UICamera.currentTouch.dragStarted)
					{
						if (this.mTooltip != null)
						{
							this.ShowTooltip(false);
						}
						UICamera.isDragging = true;
						bool flag2 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
						UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
						UICamera.isDragging = false;
						if (flag2)
						{
							UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
						}
						else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && num2 < magnitude)
						{
							UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
						}
					}
				}
			}
		}
		if (unpressed)
		{
			UICamera.currentTouch.pressStarted = false;
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.currentTouch.pressed != null)
			{
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
				if (this.useMouse && UICamera.currentTouch.pressed == UICamera.mHover)
				{
					UICamera.Notify(UICamera.currentTouch.pressed, "OnHover", true);
				}
				if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.magnitude < num))
				{
					if (UICamera.currentTouch.pressed != UICamera.mSel)
					{
						UICamera.mSel = UICamera.currentTouch.pressed;
						UICamera.Notify(UICamera.currentTouch.pressed, "OnSelect", true);
					}
					else
					{
						UICamera.mSel = UICamera.currentTouch.pressed;
					}
					if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None)
					{
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
						if (UICamera.currentTouch.clickTime + 0.35f > realtimeSinceStartup)
						{
							UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
						}
						UICamera.currentTouch.clickTime = realtimeSinceStartup;
					}
				}
				else
				{
					UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
				}
			}
			UICamera.currentTouch.dragStarted = false;
			UICamera.currentTouch.pressed = null;
			UICamera.currentTouch.dragged = null;
		}
	}

	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		UICamera.Notify(this.mTooltip, "OnTooltip", val);
		if (!val)
		{
			this.mTooltip = null;
		}
	}
}
