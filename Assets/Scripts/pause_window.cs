using AnimationOrTween;
using System;
using UnityEngine;

public class pause_window : MonoBehaviour
{
	public static string INANIM_NAME = "ui_pause_intro_Anim";

	public static string OUTANIM1_NAME = "ui_pause_outro_Anim";

	public static string OUTANIM2_NAME = "ui_pause_outro_title_Anim";

	private UIPlayAnimation m_backAnim;

	private UIPlayAnimation m_continueAnim;

	private UIImageButton m_imageButtonBack;

	private void Start()
	{
		if (ServerInterface.LoggedInServerInterface != null)
		{
			this.m_imageButtonBack = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_back_mainmenu");
			if (this.m_imageButtonBack != null)
			{
				this.m_imageButtonBack.isEnabled = true;
			}
		}
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>(true);
		Component[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = (Renderer)array[i];
			renderer.enabled = false;
		}
		componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
		Component[] array2 = componentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			MeshRenderer meshRenderer = (MeshRenderer)array2[j];
			meshRenderer.enabled = false;
		}
	}

	private void OnMsgNotifyStartPause()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_continue");
		if (gameObject != null)
		{
			this.m_continueAnim = gameObject.GetComponent<UIPlayAnimation>();
			if (this.m_continueAnim != null && this.m_continueAnim.onFinished.Count == 0)
			{
				EventDelegate.Add(this.m_continueAnim.onFinished, new EventDelegate.Callback(this.OnFinishedContinueAnimationCallback), true);
			}
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_back_mainmenu");
		if (gameObject2 != null)
		{
			this.m_backAnim = gameObject2.GetComponent<UIPlayAnimation>();
			if (this.m_backAnim != null)
			{
				this.m_backAnim.enabled = false;
			}
		}
	}

	private void OnFinishedContinueAnimationCallback()
	{
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnFinishedContinueAnimation", null, SendMessageOptions.DontRequireReceiver);
		GameObject gameObject = base.transform.FindChild("pause_Anim").gameObject;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	private void OnContinueAnimation()
	{
		if (this.m_continueAnim != null)
		{
			this.m_continueAnim.enabled = true;
			Animation target = this.m_continueAnim.target;
			if (target != null)
			{
				target.Rewind(pause_window.OUTANIM1_NAME);
				ActiveAnimation activeAnimation = ActiveAnimation.Play(target, pause_window.OUTANIM1_NAME, Direction.Forward, true);
				if (activeAnimation != null && activeAnimation.onFinished.Count == 0)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedContinueAnimationCallback), true);
				}
			}
		}
	}

	private void OnBackMainMenuAnimation()
	{
		if (this.m_backAnim != null)
		{
			this.m_backAnim.enabled = true;
			Animation target = this.m_backAnim.target;
			if (target != null)
			{
				target.Rewind(pause_window.OUTANIM2_NAME);
				ActiveAnimation.Play(target, pause_window.OUTANIM2_NAME, Direction.Forward, true);
			}
		}
	}

	private void OnSetFirstTutorial()
	{
		if (this.m_imageButtonBack != null)
		{
			this.m_imageButtonBack.isEnabled = false;
		}
	}
}
