using AnimationOrTween;
using DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttentionChaoWindow : MonoBehaviour
{
	private enum BUTTON_ACT
	{
		CLOSE,
		NONE
	}

	private enum Mode
	{
		Idle,
		Wait,
		End
	}

	private const int MAX_CHAO = 3;

	private List<int> m_attentionChaoIds;

	private AttentionChaoWindow.Mode m_mode;

	private Animation m_animation;

	private bool m_close;

	private List<ChaoData> m_chaoData;

	private GameObject[] m_chaoWindow;

	public void Setup(List<int> chaoIds)
	{
		this.m_animation = base.GetComponentInChildren<Animation>();
		if (this.m_animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_cmn_window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_open", "SE");
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_caption");
		UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(base.gameObject, "blinder");
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "blinder");
		if (uIPlayAnimation != null && uIButtonMessage != null)
		{
			uIPlayAnimation.enabled = false;
			uIButtonMessage.enabled = false;
		}
		this.m_chaoWindow = new GameObject[3];
		for (int i = 0; i < 3; i++)
		{
			this.m_chaoWindow[i] = GameObjectUtil.FindChildGameObject(base.gameObject, "chao_window_" + i);
			this.m_chaoWindow[i].SetActive(false);
		}
		if (uILabel != null)
		{
			uILabel.text = "今週の目玉";
			uILabel.text = ObjectUtility.SetColorString(uILabel.text, 0, 0, 0);
		}
		if (chaoIds != null)
		{
			this.m_attentionChaoIds = chaoIds;
			int count = this.m_attentionChaoIds.Count;
			if (count > 0)
			{
				this.m_chaoData = ChaoTable.GetChaoData(this.m_attentionChaoIds);
				this.SetChao(0);
			}
		}
		this.m_mode = AttentionChaoWindow.Mode.Wait;
	}

	private bool SetChao(int offset)
	{
		bool result = false;
		if (this.m_chaoData != null && this.m_chaoData.Count > 0 && offset >= 0 && this.m_chaoWindow != null)
		{
			int chaoLevel = ChaoTable.ChaoMaxLevel();
			for (int i = 0; i < 3; i++)
			{
				int num = i + 3 * offset;
				if (num < this.m_chaoData.Count)
				{
					this.m_chaoWindow[i].SetActive(true);
					ChaoData chaoData = this.m_chaoData[num];
					if (chaoData != null)
					{
						UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_chaoWindow[i].gameObject, "Lbl_name");
						UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_chaoWindow[i].gameObject, "Lbl_text");
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_chaoWindow[i].gameObject, "Img_bg");
						UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_chaoWindow[i].gameObject, "Tex_chao");
						if (uILabel != null)
						{
							uILabel.text = chaoData.nameTwolines;
							uILabel.text = ObjectUtility.SetColorString(uILabel.text, 0, 0, 0);
						}
						if (uILabel2 != null)
						{
							uILabel2.text = chaoData.GetDetailsLevel(chaoLevel);
							uILabel2.text = ObjectUtility.SetColorString(uILabel2.text, 0, 0, 0);
						}
						if (uISprite != null)
						{
							switch (chaoData.rarity)
							{
							case ChaoData.Rarity.NORMAL:
								uISprite.spriteName = "ui_tex_chao_bg_0";
								break;
							case ChaoData.Rarity.RARE:
								uISprite.spriteName = "ui_tex_chao_bg_1";
								break;
							case ChaoData.Rarity.SRARE:
								uISprite.spriteName = "ui_tex_chao_bg_2";
								break;
							}
						}
						if (uITexture != null)
						{
							ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
							ChaoTextureManager.Instance.GetTexture(chaoData.id, info);
						}
					}
				}
				else
				{
					this.m_chaoWindow[i].SetActive(false);
				}
			}
		}
		return result;
	}

	public bool IsEnd()
	{
		return this.m_mode != AttentionChaoWindow.Mode.Wait;
	}

	public void OnClickNoButton()
	{
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	public void OnClickNoBgButton()
	{
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		UIPlayAnimation component = gameObject.GetComponent<UIPlayAnimation>();
		if (component != null)
		{
			EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			component.Play(true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			this.m_mode = AttentionChaoWindow.Mode.End;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static AttentionChaoWindow Create(List<int> chaoIds)
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "AttentionChaoWindowUI(Clone)");
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			GameObject original = Resources.Load("Prefabs/UI/AttentionChaoWindowUI") as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject2.SetActive(true);
			gameObject2.transform.parent = cameraUIObject.transform;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			AttentionChaoWindow attentionChaoWindow = null;
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject2, "attention_window");
			if (gameObject3 != null)
			{
				attentionChaoWindow = gameObject3.AddComponent<AttentionChaoWindow>();
				if (attentionChaoWindow != null)
				{
					attentionChaoWindow.Setup(chaoIds);
				}
			}
			return attentionChaoWindow;
		}
		return null;
	}
}
