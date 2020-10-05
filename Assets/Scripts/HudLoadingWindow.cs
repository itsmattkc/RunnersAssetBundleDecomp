using AnimationOrTween;
using System;
using UnityEngine;

public class HudLoadingWindow : MonoBehaviour
{
	private enum EventSignal
	{
		SIG_PLAYSTART = 100,
		SIG_PLAYEND
	}

	public const float TEXTURE_IMAGE_SCALE = 0.859375f;

	private TinyFsmBehavior m_fsm;

	private float m_lastClick;

	private float m_parcentage;

	[SerializeField]
	private float m_charaDisplayTime = 10f;

	private float m_currentDisplayTime;

	private HudLoadingCharaInfo m_charaInfo;

	[SerializeField]
	private Texture2D defultImage;

	public void PlayStart()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			UIPanel component = base.gameObject.GetComponent<UIPanel>();
			if (component != null)
			{
				component.alpha = 1f;
			}
			this.m_fsm.Dispatch(signal);
		}
		this.m_charaInfo.enabled = true;
	}

	public void SetLoadingPercentage(float parcentage)
	{
		if (parcentage > 100f)
		{
			parcentage = 100f;
		}
		this.m_parcentage = parcentage;
	}

	public void PlayEnd()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void Start()
	{
		this.m_lastClick = 0f;
		this.m_fsm = base.gameObject.AddComponent<TinyFsmBehavior>();
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateSetup));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		this.m_charaInfo = base.gameObject.AddComponent<HudLoadingCharaInfo>();
		this.m_charaInfo.enabled = false;
	}

	private void OnDestroy()
	{
		if (this.m_charaInfo != null)
		{
			UnityEngine.Object.Destroy(this.m_charaInfo.gameObject);
			this.m_charaInfo = null;
		}
	}

	private void Update()
	{
	}

	private TinyFsmState StateSetup(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_lastClick = 0f;
			return TinyFsmState.End();
		case 4:
			base.gameObject.SetActive(false);
			this.SetCharaExplainActive(false);
			this.SetLoadingBarParcentage(this.m_parcentage);
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
		{
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			base.gameObject.SetActive(true);
			Animation component = base.gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation.Play(component, Direction.Forward);
			}
			this.m_currentDisplayTime = this.m_charaDisplayTime;
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateLoading)));
			return TinyFsmState.End();
		}
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateLoading(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
		{
			IL_23:
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			this.SetLoadingBarParcentage(100f);
			Animation component = base.gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation.Play(component, Direction.Reverse);
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			return TinyFsmState.End();
		}
		case 4:
			this.SetLoadingBarParcentage(this.m_parcentage);
			this.m_currentDisplayTime += Time.deltaTime;
			if (this.m_currentDisplayTime >= this.m_charaDisplayTime)
			{
				this.m_currentDisplayTime = this.m_charaDisplayTime;
				bool flag = this.ChangeCharaExplain();
				if (flag)
				{
					this.m_currentDisplayTime = 0f;
				}
			}
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private void SetLoadingBarParcentage(float parcentage)
	{
		UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(base.gameObject, "Progress_Bar");
		if (uISlider != null)
		{
			uISlider.value = parcentage / 100f;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_progress");
		if (uILabel != null)
		{
			uILabel.text = ((int)parcentage).ToString() + "%";
		}
	}

	private bool ChangeCharaExplain()
	{
		if (this.m_charaInfo == null)
		{
			return false;
		}
		if (!this.m_charaInfo.IsReady())
		{
			return false;
		}
		this.SetCharaExplainActive(true);
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_event_tex");
		if (uITexture != null)
		{
			Texture2D charaPicture = this.m_charaInfo.GetCharaPicture();
			if (charaPicture != null)
			{
				bool flag = false;
				if (charaPicture.width < 150 || charaPicture.height < 150)
				{
					flag = true;
				}
				if (!flag)
				{
					uITexture.mainTexture = charaPicture;
					uITexture.width = (int)((float)uITexture.mainTexture.width * 0.859375f);
					uITexture.height = (int)((float)uITexture.mainTexture.height * 0.859375f);
				}
				else
				{
					uITexture.mainTexture = this.defultImage;
					uITexture.width = 220;
					uITexture.height = 220;
				}
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_tex_flame");
				if (uITexture != null)
				{
					uISprite.width = uITexture.width + 4;
					uISprite.height = uITexture.height + 6;
				}
			}
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_name");
		if (uILabel != null)
		{
			uILabel.text = this.m_charaInfo.GetCharaName();
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_help");
		if (uILabel2 != null)
		{
			uILabel2.text = this.m_charaInfo.GetCharaExplain();
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption_sub");
		if (uILabel3 != null)
		{
			uILabel3.text = this.m_charaInfo.GetCharaExplainCaption();
		}
		this.m_charaInfo.GoNext();
		return true;
	}

	private void SetCharaExplainActive(bool isActive)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_event_tex");
		if (gameObject != null)
		{
			gameObject.SetActive(isActive);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_name");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(isActive);
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_help");
		if (gameObject3 != null)
		{
			gameObject3.SetActive(isActive);
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_caption_sub");
		if (gameObject4 != null)
		{
			gameObject4.SetActive(isActive);
		}
	}

	public void OnPressBg()
	{
		if (Time.realtimeSinceStartup >= this.m_lastClick + 0.05f && this.m_currentDisplayTime > 0.5f)
		{
			this.m_currentDisplayTime = this.m_charaDisplayTime;
			this.m_lastClick = Time.realtimeSinceStartup;
		}
	}
}
