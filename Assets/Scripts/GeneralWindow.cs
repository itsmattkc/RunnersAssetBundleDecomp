using AnimationOrTween;
using System;
using System.Diagnostics;
using UnityEngine;

public class GeneralWindow : WindowBase
{
	public enum ButtonType
	{
		None,
		Ok,
		YesNo,
		ShopCancel,
		TweetCancel,
		Close,
		TweetOk,
		OkNextSkip,
		OkNextSkipAllSkip
	}

	public struct CInfo
	{
		public struct Event
		{
			public class FaceWindow
			{
				public Texture texture;

				public string name;

				public EffectType effectType;

				public AnimType animType;

				public ReverseType reverseType;

				public ShowingType showingType;
			}

			public GeneralWindow.CInfo.Event.FaceWindow[] faceWindows;

			public ArrowType arrowType;

			public string bgmCueName;

			public string seCueName;

			public string message;
		}

		public delegate void FinishedCloseDelegate();

		public bool isImgView;

		public string imgTextureName;

		public string name;

		public GeneralWindow.ButtonType buttonType;

		public string caption;

		public string anchor_path;

		public GameObject parentGameObject;

		public bool disableButton;

		public string message;

		public string imageName;

		public string imageCount;

		public GeneralWindow.CInfo.Event[] events;

		public GeneralWindow.CInfo.FinishedCloseDelegate finishedCloseDelegate;

		public bool isPlayErrorSe;

		public bool isNotPlaybackDefaultBgm;

		public bool isActiveImageView;

		public bool isSpecialEvent;
	}

	private struct Pressed
	{
		public bool m_isButtonPressed;

		public bool m_isOkButtonPressed;

		public bool m_isYesButtonPressed;

		public bool m_isNoButtonPressed;

		public bool m_isAllSkipButtonPressed;
	}

	[Serializable]
	private class FaceWindowUI
	{
		[SerializeField]
		public string m_windowKey;

		[SerializeField]
		public GameObject m_faceWindowGameObject;

		[SerializeField]
		public UITexture m_faceTexture;

		[SerializeField]
		public GameObject m_namePlateGameObject;

		[SerializeField]
		public UILabel m_nameLabel;

		[SerializeField]
		public GameObject m_balloonArrow;

		[SerializeField]
		public GameObject m_disableFilter;

		[SerializeField]
		public Animation m_fadeAnimation;

		[SerializeField]
		public Animation m_vibrateAnimation;

		[SerializeField]
		public Animation[] m_faceAnimations = new Animation[2];
	}

	private const char FORM_FEED_CODE = '\f';

	private static GameObject m_generalWindowPrefab;

	private static GameObject m_generalWindowObject;

	private static GeneralWindow.CInfo m_info;

	private static bool m_disableButton;

	private static UILabel m_captionLabel;

	private static UILabel m_imageCountLabel;

	private static GeneralWindow.Pressed m_pressed;

	private static GeneralWindow.Pressed m_resultPressed;

	private static bool m_isChangedBgm;

	private static bool m_created;

	private static bool m_isOpend;

	private static bool m_playedCloseSE;

	private static int m_eventCount;

	private static string[] m_messages;

	private static int m_messageCount;

	private static UILabel m_messageLabel;

	[SerializeField]
	private GameObject[] m_textViews = new GameObject[2];

	[SerializeField]
	private GameObject m_imgView;

	[SerializeField]
	private GameObject m_imgItem;

	[SerializeField]
	private GameObject m_imgChao;

	[SerializeField]
	private UILabel m_imgName;

	[SerializeField]
	private UILabel m_imgCount;

	[SerializeField]
	private GameObject m_imgDecoEff;

	[SerializeField]
	private GameObject[] m_eventViews = new GameObject[2];

	[SerializeField]
	private UILabel[] m_eventTexts = new UILabel[2];

	[SerializeField]
	private GameObject m_eventOkButton;

	[SerializeField]
	private GameObject m_eventNextButton;

	[SerializeField]
	private GameObject m_eventAllSkipButton;

	[SerializeField]
	private GameObject m_spEventOkButton;

	[SerializeField]
	private GameObject m_spEventNextButton;

	[SerializeField]
	private UIScrollBar m_eventScrollBar;

	private static float m_createdTime;

	[SerializeField]
	private GeneralWindow.FaceWindowUI[] m_singleFaceWindowUis = new GeneralWindow.FaceWindowUI[1];

	[SerializeField]
	private GeneralWindow.FaceWindowUI[] m_twinFaceWindowUis = new GeneralWindow.FaceWindowUI[2];

	[SerializeField]
	public UITexture m_eventWindowBgTexture;

	public static GeneralWindow.CInfo Info
	{
		get
		{
			return GeneralWindow.m_info;
		}
	}

	public static bool Created
	{
		get
		{
			return GeneralWindow.m_created;
		}
	}

	public static bool IsButtonPressed
	{
		get
		{
			return GeneralWindow.m_resultPressed.m_isButtonPressed;
		}
	}

	public static bool IsOkButtonPressed
	{
		get
		{
			return GeneralWindow.m_resultPressed.m_isOkButtonPressed;
		}
	}

	public static bool IsYesButtonPressed
	{
		get
		{
			return GeneralWindow.m_resultPressed.m_isYesButtonPressed;
		}
	}

	public static bool IsNoButtonPressed
	{
		get
		{
			return GeneralWindow.m_resultPressed.m_isNoButtonPressed;
		}
	}

	public static bool IsAllSkipButtonPressed
	{
		get
		{
			return GeneralWindow.m_resultPressed.m_isAllSkipButtonPressed;
		}
	}

	private void Start()
	{
		GeneralWindow.m_generalWindowObject = base.gameObject;
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		Transform parent;
		if (GeneralWindow.m_info.parentGameObject != null)
		{
			parent = GameObjectUtil.FindChildGameObject(GeneralWindow.m_info.parentGameObject, "Anchor_5_MC").transform;
		}
		else if (GeneralWindow.m_info.anchor_path != null)
		{
			parent = gameObject.transform.Find(GeneralWindow.m_info.anchor_path);
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
		GeneralWindow.SetDisableButton(GeneralWindow.m_disableButton);
		GameObject gameObject2 = GeneralWindow.m_generalWindowObject.transform.Find("window/Lbl_caption").gameObject;
		GeneralWindow.m_captionLabel = gameObject2.GetComponent<UILabel>();
		GeneralWindow.m_captionLabel.text = GeneralWindow.m_info.caption;
		GeneralWindow.m_imageCountLabel = this.m_imgCount;
		GameObject gameObject3 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/textView/ScrollView/Lbl_body").gameObject;
		string str = "window/pattern_btn_use/textView/";
		Transform transform = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_less");
		Transform transform2 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use");
		Transform transform3 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_0");
		Transform transform4 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_1");
		Transform transform5 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_2");
		Transform transform6 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_3");
		Transform transform7 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_4");
		Transform transform8 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_5");
		Transform transform9 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_6");
		Transform transform10 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_use/pattern_7");
		transform.gameObject.SetActive(false);
		transform2.gameObject.SetActive(false);
		transform3.gameObject.SetActive(false);
		transform4.gameObject.SetActive(false);
		transform5.gameObject.SetActive(false);
		transform6.gameObject.SetActive(false);
		transform7.gameObject.SetActive(false);
		transform8.gameObject.SetActive(false);
		transform9.gameObject.SetActive(false);
		transform10.gameObject.SetActive(false);
		bool flag = false;
		if (RegionManager.Instance != null)
		{
			flag = RegionManager.Instance.IsUseSNS();
		}
		switch (GeneralWindow.m_info.buttonType)
		{
		case GeneralWindow.ButtonType.None:
			transform.gameObject.SetActive(true);
			gameObject3 = GeneralWindow.m_generalWindowObject.transform.Find("window/pattern_btn_less/bl_textView/bl_ScrollView/bl_Lbl_body").gameObject;
			str = "window/pattern_btn_less/bl_textView/bl_";
			break;
		case GeneralWindow.ButtonType.Ok:
			transform2.gameObject.SetActive(true);
			transform3.gameObject.SetActive(true);
			break;
		case GeneralWindow.ButtonType.YesNo:
			transform2.gameObject.SetActive(true);
			transform4.gameObject.SetActive(true);
			if (GeneralWindow.m_info.name == "FacebookLogin" && !flag)
			{
				UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(transform4.gameObject, "Btn_yes");
				if (uIImageButton != null)
				{
					uIImageButton.isEnabled = false;
				}
			}
			break;
		case GeneralWindow.ButtonType.ShopCancel:
			transform2.gameObject.SetActive(true);
			transform5.gameObject.SetActive(true);
			break;
		case GeneralWindow.ButtonType.TweetCancel:
			transform2.gameObject.SetActive(true);
			transform6.gameObject.SetActive(true);
			if (!flag)
			{
				UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(transform6.gameObject, "Btn_post");
				if (uIImageButton2 != null)
				{
					uIImageButton2.isEnabled = false;
				}
			}
			break;
		case GeneralWindow.ButtonType.Close:
			transform2.gameObject.SetActive(true);
			transform7.gameObject.SetActive(true);
			break;
		case GeneralWindow.ButtonType.TweetOk:
			transform2.gameObject.SetActive(true);
			transform8.gameObject.SetActive(true);
			if (!flag)
			{
				UIImageButton uIImageButton3 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(transform8.gameObject, "Btn_post");
				if (uIImageButton3 != null)
				{
					uIImageButton3.isEnabled = false;
				}
			}
			break;
		case GeneralWindow.ButtonType.OkNextSkip:
			transform2.gameObject.SetActive(true);
			transform9.gameObject.SetActive(true);
			break;
		case GeneralWindow.ButtonType.OkNextSkipAllSkip:
			transform2.gameObject.SetActive(true);
			transform10.gameObject.SetActive(true);
			break;
		}
		int num = (GeneralWindow.m_info.buttonType != GeneralWindow.ButtonType.None) ? 1 : 0;
		GeneralWindow.m_isChangedBgm = false;
		GeneralWindow.m_playedCloseSE = false;
		GeneralWindow.m_eventCount = 0;
		GeneralWindow.m_messages = null;
		GeneralWindow.m_messageCount = 0;
		GeneralWindow.m_messageLabel = null;
		this.m_textViews[num].SetActive(GeneralWindow.m_info.message != null);
		if (GeneralWindow.m_info.message != null)
		{
			GeneralWindow.m_messages = GeneralWindow.m_info.message.Split(new char[]
			{
				'\f'
			});
			UILabel component = gameObject3.GetComponent<UILabel>();
			component.text = ((GeneralWindow.m_messages.Length < 1) ? null : GeneralWindow.m_messages[GeneralWindow.m_messageCount++]);
			GeneralWindow.m_messageLabel = component;
			GameObject gameObject4 = GeneralWindow.m_generalWindowObject.transform.Find(str + "ScrollView").gameObject;
			UIPanel component2 = gameObject4.GetComponent<UIPanel>();
			float w = component2.clipRange.w;
			float num2 = component.printedSize.y * component.transform.localScale.y;
			if (num2 <= w)
			{
				BoxCollider component3 = GeneralWindow.m_generalWindowObject.transform.Find(str + "ScrollOutline").GetComponent<BoxCollider>();
				component3.enabled = false;
			}
		}
		bool isActiveImageView = GeneralWindow.m_info.isActiveImageView;
		this.m_imgView.SetActive(isActiveImageView);
		if (isActiveImageView)
		{
			this.m_imgItem.SetActive(false);
			this.m_imgChao.SetActive(false);
			this.m_imgDecoEff.SetActive(false);
			this.m_imgName.text = GeneralWindow.m_info.imageName;
			this.m_imgCount.text = GeneralWindow.m_info.imageCount;
		}
		bool flag2 = GeneralWindow.m_info.events != null;
		this.m_eventViews[num].SetActive(flag2);
		if (flag2)
		{
			GeneralWindow.m_messageLabel = this.m_eventTexts[num];
			this.NextEvent();
			this.SetOkNextButton();
		}
		bool isImgView = GeneralWindow.m_info.isImgView;
		this.m_imgView.SetActive(isImgView);
		if (isImgView)
		{
			this.m_imgItem.SetActive(false);
			this.m_imgChao.SetActive(false);
			this.m_imgDecoEff.SetActive(false);
			this.m_textViews[num].SetActive(false);
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_imgView, "Lbl_explan");
			if (uILabel != null)
			{
				uILabel.text = GeneralWindow.m_info.message;
			}
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_imgView, "img_tutorial_other_character");
			if (uITexture != null)
			{
				GameObject gameObject5 = GameObject.Find(GeneralWindow.m_info.imgTextureName);
				if (gameObject5 != null)
				{
					AssetBundleTexture component4 = gameObject5.GetComponent<AssetBundleTexture>();
					if (component4 != null)
					{
						uITexture.mainTexture = component4.m_tex;
					}
				}
			}
		}
		if (GeneralWindow.m_info.isPlayErrorSe)
		{
			SoundManager.SePlay("sys_error", "SE");
		}
	}

	private void Update()
	{
		if (GeneralWindow.m_createdTime < 2f)
		{
			float deltaTime = Time.deltaTime;
			if (GeneralWindow.m_createdTime < 1f && GeneralWindow.m_createdTime + deltaTime >= 1f)
			{
				UILabel[] componentsInChildren = base.gameObject.GetComponentsInChildren<UILabel>();
				if (componentsInChildren != null && componentsInChildren.Length > 0)
				{
					UILabel[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						UILabel uILabel = array[i];
						uILabel.gameObject.SendMessage("Start");
					}
					global::Debug.Log("GeneralWindow UILabel restart " + componentsInChildren.Length + " !");
				}
			}
			else if (GeneralWindow.m_createdTime < 2f && GeneralWindow.m_createdTime + deltaTime >= 2f)
			{
				UILabel[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<UILabel>();
				if (componentsInChildren2 != null && componentsInChildren2.Length > 0)
				{
					UILabel[] array2 = componentsInChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						UILabel uILabel2 = array2[j];
						uILabel2.gameObject.SendMessage("Start");
					}
					global::Debug.Log("GeneralWindow UILabel restart " + componentsInChildren2.Length + " !!");
				}
			}
			GeneralWindow.m_createdTime += deltaTime;
		}
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private void SendButtonMessage(string patternName, string btnName)
	{
		Transform transform = GeneralWindow.m_generalWindowObject.transform.Find(patternName);
		if (transform != null && transform.gameObject.activeSelf)
		{
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(transform.gameObject, btnName);
			if (uIButtonMessage != null)
			{
				uIButtonMessage.SendMessage("OnClick");
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (GeneralWindow.m_isOpend)
		{
			switch (GeneralWindow.m_info.buttonType)
			{
			case GeneralWindow.ButtonType.Ok:
				this.SendButtonMessage("window/pattern_btn_use/pattern_0", "Btn_ok");
				break;
			case GeneralWindow.ButtonType.YesNo:
				this.SendButtonMessage("window/pattern_btn_use/pattern_1", "Btn_no");
				break;
			case GeneralWindow.ButtonType.ShopCancel:
				this.SendButtonMessage("window/pattern_btn_use/pattern_2", "Btn_cancel");
				break;
			case GeneralWindow.ButtonType.TweetCancel:
				this.SendButtonMessage("window/pattern_btn_use/pattern_3", "Btn_ok");
				break;
			case GeneralWindow.ButtonType.Close:
				this.SendButtonMessage("window/pattern_btn_use/pattern_4", "Btn_close");
				break;
			case GeneralWindow.ButtonType.TweetOk:
				this.SendButtonMessage("window/pattern_btn_use/pattern_5", "Btn_ok");
				break;
			case GeneralWindow.ButtonType.OkNextSkip:
				this.SendButtonMessage("window/pattern_btn_use/pattern_6", "Btn_skip");
				break;
			case GeneralWindow.ButtonType.OkNextSkipAllSkip:
				this.SendButtonMessage("window/pattern_btn_use/pattern_7", "Btn_skip");
				break;
			}
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
	}

	private bool NextEvent()
	{
		if (GeneralWindow.m_info.events != null && GeneralWindow.m_eventCount < GeneralWindow.m_info.events.Length)
		{
			GeneralWindow.CInfo.Event @event = GeneralWindow.m_info.events[GeneralWindow.m_eventCount++];
			if (!string.IsNullOrEmpty(@event.bgmCueName))
			{
				if (WindowBodyData.IsBgmStop(@event.bgmCueName))
				{
					SoundManager.BgmStop();
				}
				else if (@event.bgmCueName.Contains(","))
				{
					string[] array = @event.bgmCueName.Split(new char[]
					{
						','
					});
					if (array != null && array.Length > 1)
					{
						SoundManager.BgmPlay(array[0], array[1], false);
					}
				}
				else
				{
					SoundManager.BgmPlay(@event.bgmCueName, "BGM", false);
				}
				GeneralWindow.m_isChangedBgm = true;
			}
			if (!string.IsNullOrEmpty(@event.seCueName))
			{
				SoundManager.SePlay(@event.seCueName, "SE");
			}
			GeneralWindow.FaceWindowUI[][] array2 = new GeneralWindow.FaceWindowUI[][]
			{
				this.m_singleFaceWindowUis,
				this.m_twinFaceWindowUis
			};
			for (int i = 0; i < array2.Length; i++)
			{
				GeneralWindow.FaceWindowUI[] array3 = array2[i];
				GeneralWindow.FaceWindowUI[] array4 = array3;
				for (int j = 0; j < array4.Length; j++)
				{
					GeneralWindow.FaceWindowUI faceWindowUI = array4[j];
					faceWindowUI.m_faceWindowGameObject.SetActive(false);
					faceWindowUI.m_namePlateGameObject.SetActive(false);
					faceWindowUI.m_balloonArrow.SetActive(false);
				}
			}
			Texture texture = (!GeneralWindow.m_info.isSpecialEvent) ? MileageMapUtility.GetWindowBGTexture() : EventUtility.GetBGTexture();
			if (texture != null)
			{
				this.m_eventWindowBgTexture.mainTexture = texture;
			}
			switch (@event.arrowType)
			{
			case ArrowType.MIDDLE:
				this.m_singleFaceWindowUis[0].m_balloonArrow.SetActive(true);
				break;
			case ArrowType.RIGHT:
				this.m_twinFaceWindowUis[1].m_balloonArrow.SetActive(true);
				break;
			case ArrowType.LEFT:
				this.m_twinFaceWindowUis[0].m_balloonArrow.SetActive(true);
				break;
			case ArrowType.TWO_SIDES:
				this.m_twinFaceWindowUis[0].m_balloonArrow.SetActive(true);
				this.m_twinFaceWindowUis[1].m_balloonArrow.SetActive(true);
				break;
			}
			if (@event.faceWindows != null)
			{
				GeneralWindow.FaceWindowUI[] array5 = (@event.faceWindows.Length != 1) ? this.m_twinFaceWindowUis : this.m_singleFaceWindowUis;
				for (int k = 0; k < @event.faceWindows.Length; k++)
				{
					GeneralWindow.CInfo.Event.FaceWindow faceWindow = @event.faceWindows[k];
					GeneralWindow.FaceWindowUI faceWindowUI2 = array5[k];
					Animation[] faceAnimations = faceWindowUI2.m_faceAnimations;
					for (int l = 0; l < faceAnimations.Length; l++)
					{
						Animation animation = faceAnimations[l];
						animation.gameObject.SetActive(false);
					}
					switch (faceWindow.showingType)
					{
					case ShowingType.NORMAL:
						faceWindowUI2.m_faceWindowGameObject.SetActive(true);
						faceWindowUI2.m_disableFilter.SetActive(false);
						break;
					case ShowingType.DARK:
						faceWindowUI2.m_faceWindowGameObject.SetActive(true);
						faceWindowUI2.m_disableFilter.SetActive(true);
						break;
					case ShowingType.HIDE:
						faceWindowUI2.m_faceWindowGameObject.SetActive(false);
						faceWindowUI2.m_disableFilter.SetActive(false);
						break;
					}
					faceWindowUI2.m_namePlateGameObject.SetActive(!string.IsNullOrEmpty(faceWindow.name));
					faceWindowUI2.m_nameLabel.text = faceWindow.name;
					faceWindowUI2.m_faceTexture.mainTexture = faceWindow.texture;
					Rect uvRect = faceWindowUI2.m_faceTexture.uvRect;
					ReverseType reverseType = faceWindow.reverseType;
					if (reverseType != ReverseType.MIRROR)
					{
						uvRect.width = Mathf.Abs(uvRect.width);
					}
					else
					{
						uvRect.width = -Mathf.Abs(uvRect.width);
					}
					faceWindowUI2.m_faceTexture.uvRect = uvRect;
					EffectType effectType = faceWindow.effectType;
					if (effectType == EffectType.BANG || effectType == EffectType.BLAST)
					{
						GameObject gameObject = faceWindowUI2.m_faceAnimations[faceWindow.effectType - EffectType.BANG].gameObject;
						gameObject.SetActive(false);
						gameObject.SetActive(true);
					}
					string str = "_vibe_skip_Anim";
					string str2 = "_intro_skip_Anim";
					switch (faceWindow.animType)
					{
					case AnimType.VIBRATION:
						str = "_vibe_Anim";
						break;
					case AnimType.FADE_IN:
						str2 = "_intro_Anim";
						break;
					case AnimType.FADE_OUT:
						str2 = "_outro_Anim";
						break;
					}
					ActiveAnimation.Play(faceWindowUI2.m_vibrateAnimation, "ui_gn_window_event_tex_" + faceWindowUI2.m_windowKey + str, Direction.Forward);
					ActiveAnimation.Play(faceWindowUI2.m_fadeAnimation, "ui_gn_window_event_tex_" + faceWindowUI2.m_windowKey + str2, Direction.Forward);
				}
			}
			GeneralWindow.m_messageCount = 0;
			GeneralWindow.m_messages = @event.message.Split(new char[]
			{
				'\f'
			});
			this.EventNextMessage();
			return true;
		}
		return false;
	}

	private bool EventNextMessage()
	{
		if (GeneralWindow.m_messages != null && GeneralWindow.m_messageCount < GeneralWindow.m_messages.Length)
		{
			GeneralWindow.m_messageLabel.text = GeneralWindow.m_messages[GeneralWindow.m_messageCount++];
			this.m_eventScrollBar.value = 0f;
			return true;
		}
		return false;
	}

	private void SetOkNextButton()
	{
		bool flag = (GeneralWindow.m_info.events != null && GeneralWindow.m_eventCount < GeneralWindow.m_info.events.Length) || (GeneralWindow.m_messages != null && GeneralWindow.m_messageCount < GeneralWindow.m_messages.Length);
		if (GeneralWindow.m_info.buttonType == GeneralWindow.ButtonType.OkNextSkip)
		{
			this.m_spEventOkButton.SetActive(!flag);
			this.m_spEventNextButton.SetActive(flag);
		}
		else if (GeneralWindow.m_info.buttonType == GeneralWindow.ButtonType.OkNextSkipAllSkip)
		{
			this.m_eventOkButton.SetActive(!flag);
			this.m_eventNextButton.SetActive(flag);
			this.m_eventAllSkipButton.SetActive(flag);
		}
	}

	public static GameObject Create(GeneralWindow.CInfo info)
	{
		GeneralWindow.SetUIEffect(false);
		GeneralWindow.m_info = info;
		GeneralWindow.m_disableButton = info.disableButton;
		GeneralWindow.m_pressed.m_isButtonPressed = false;
		GeneralWindow.m_pressed.m_isOkButtonPressed = false;
		GeneralWindow.m_pressed.m_isYesButtonPressed = false;
		GeneralWindow.m_pressed.m_isNoButtonPressed = false;
		GeneralWindow.m_pressed.m_isAllSkipButtonPressed = false;
		GeneralWindow.m_resultPressed = GeneralWindow.m_pressed;
		GeneralWindow.m_created = true;
		GeneralWindow.m_isOpend = true;
		GeneralWindow.m_isChangedBgm = false;
		GeneralWindow.m_playedCloseSE = false;
		GeneralWindow.m_createdTime = 0f;
		if (GeneralWindow.m_generalWindowPrefab == null)
		{
			GeneralWindow.m_generalWindowPrefab = (Resources.Load("Prefabs/UI/GeneralWindow") as GameObject);
		}
		if (GeneralWindow.m_generalWindowObject != null)
		{
			return null;
		}
		GeneralWindow.m_generalWindowObject = (UnityEngine.Object.Instantiate(GeneralWindow.m_generalWindowPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		GeneralWindow.m_generalWindowObject.SetActive(true);
		GeneralWindow.ResetScrollBar();
		SoundManager.SePlay("sys_window_open", "SE");
		Animation component = GeneralWindow.m_generalWindowObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation.Play(component, Direction.Forward);
		}
		return GeneralWindow.m_generalWindowObject;
	}

	public static bool Close()
	{
		bool playedCloseSE = GeneralWindow.m_playedCloseSE;
		GeneralWindow.m_info = default(GeneralWindow.CInfo);
		GeneralWindow.m_pressed = default(GeneralWindow.Pressed);
		GeneralWindow.m_resultPressed = default(GeneralWindow.Pressed);
		GeneralWindow.m_created = false;
		GeneralWindow.m_isOpend = false;
		GeneralWindow.m_playedCloseSE = false;
		if (GeneralWindow.m_generalWindowObject != null)
		{
			if (!playedCloseSE)
			{
				SoundManager.SePlay("sys_window_close", "SE");
			}
			GeneralWindow.DestroyWindow();
			return true;
		}
		return false;
	}

	public static bool IsCreated(string name)
	{
		return GeneralWindow.Info.name == name;
	}

	private void OnClickOkButton()
	{
		if (!GeneralWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			GeneralWindow.m_pressed.m_isOkButtonPressed = true;
			GeneralWindow.m_pressed.m_isButtonPressed = true;
		}
		GeneralWindow.m_isOpend = false;
	}

	private void OnClickYesButton()
	{
		if (!GeneralWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			GeneralWindow.m_pressed.m_isYesButtonPressed = true;
			GeneralWindow.m_pressed.m_isButtonPressed = true;
		}
		GeneralWindow.m_isOpend = false;
	}

	private void OnClickNoButton()
	{
		if (!GeneralWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_window_close", "SE");
			GeneralWindow.m_pressed.m_isNoButtonPressed = true;
			GeneralWindow.m_pressed.m_isButtonPressed = true;
			GeneralWindow.m_playedCloseSE = true;
		}
		GeneralWindow.m_isOpend = false;
	}

	private void OnClickNextButton()
	{
		if (this.EventNextMessage() || this.NextEvent())
		{
			SoundManager.SePlay("sys_page_skip", "SE");
		}
		this.SetOkNextButton();
	}

	private void OnClickSkipButton()
	{
		if (!GeneralWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_window_close", "SE");
			GeneralWindow.m_pressed.m_isNoButtonPressed = true;
			GeneralWindow.m_pressed.m_isButtonPressed = true;
			GeneralWindow.m_playedCloseSE = true;
		}
		GeneralWindow.m_isOpend = false;
	}

	private void OnClickAllSkipButton()
	{
		if (!GeneralWindow.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_window_close", "SE");
			GeneralWindow.m_pressed.m_isButtonPressed = true;
			GeneralWindow.m_pressed.m_isAllSkipButtonPressed = true;
			GeneralWindow.m_playedCloseSE = true;
		}
		GeneralWindow.m_isOpend = false;
	}

	public void OnFinishedCloseAnim()
	{
		GeneralWindow.m_resultPressed = GeneralWindow.m_pressed;
		GeneralWindow.m_created = false;
		if (GeneralWindow.m_info.finishedCloseDelegate != null)
		{
			GeneralWindow.m_info.finishedCloseDelegate();
		}
		GeneralWindow.DestroyWindow();
		if (GeneralWindow.m_isChangedBgm)
		{
			if (GeneralWindow.m_info.isNotPlaybackDefaultBgm)
			{
				SoundManager.BgmStop();
			}
			else if (GeneralWindow.m_info.isSpecialEvent)
			{
				string data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.EventTop_BgmName);
				if (string.IsNullOrEmpty(data))
				{
					HudMenuUtility.ChangeMainMenuBGM();
				}
				else
				{
					string cueSheetName = "BGM_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
					SoundManager.BgmChange(data, cueSheetName);
				}
			}
			else
			{
				HudMenuUtility.ChangeMainMenuBGM();
			}
		}
	}

	private static void DestroyWindow()
	{
		if (GeneralWindow.m_generalWindowObject != null)
		{
			UnityEngine.Object.Destroy(GeneralWindow.m_generalWindowObject);
			GeneralWindow.m_generalWindowObject = null;
		}
		GeneralWindow.SetUIEffect(true);
	}

	public static void SetCaption(string caption)
	{
		GeneralWindow.m_captionLabel.text = caption;
	}

	public static void SetImageCount(string text)
	{
		GeneralWindow.m_imageCountLabel.text = text;
	}

	public static void SetDisableButton(bool disableButton)
	{
		GeneralWindow.m_disableButton = disableButton;
		if (GeneralWindow.m_generalWindowObject != null)
		{
			UIButton[] componentsInChildren = GeneralWindow.m_generalWindowObject.GetComponentsInChildren<UIButton>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UIButton uIButton = componentsInChildren[i];
				uIButton.isEnabled = !GeneralWindow.m_disableButton;
			}
			UIImageButton[] componentsInChildren2 = GeneralWindow.m_generalWindowObject.GetComponentsInChildren<UIImageButton>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				UIImageButton uIImageButton = componentsInChildren2[j];
				uIImageButton.isEnabled = !GeneralWindow.m_disableButton;
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

	private static void ResetScrollBar()
	{
		if (GeneralWindow.m_generalWindowObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(GeneralWindow.m_generalWindowObject, "textView");
			if (gameObject != null)
			{
				UIScrollBar uIScrollBar = GameObjectUtil.FindChildGameObjectComponent<UIScrollBar>(gameObject, "Scroll_Bar");
				if (uIScrollBar != null)
				{
					uIScrollBar.value = 0f;
				}
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(GeneralWindow.m_generalWindowObject, "bl_textView");
			if (gameObject2 != null)
			{
				UIScrollBar uIScrollBar2 = GameObjectUtil.FindChildGameObjectComponent<UIScrollBar>(gameObject2, "bl_Scroll_Bar");
				if (uIScrollBar2 != null)
				{
					uIScrollBar2.value = 0f;
				}
			}
		}
	}

	public static Texture2D GetDummyTexture(int index)
	{
		Color[] array = new Color[]
		{
			Color.red,
			Color.blue,
			Color.green,
			Color.yellow,
			Color.white,
			Color.cyan,
			Color.gray,
			Color.magenta
		};
		Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);
		texture2D.SetPixel(0, 0, Color.black);
		texture2D.SetPixel(1, 0, Color.black);
		checked
		{
			texture2D.SetPixel(0, 1, array[(int)((IntPtr)(unchecked((ulong)index % (ulong)((long)array.Length))))]);
			texture2D.SetPixel(1, 1, array[(int)((IntPtr)(unchecked((ulong)index % (ulong)((long)array.Length))))]);
			texture2D.Apply();
			return texture2D;
		}
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
