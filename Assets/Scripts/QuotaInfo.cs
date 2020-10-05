using System;
using UnityEngine;

public class QuotaInfo
{
	private enum State
	{
		IDLE,
		IN,
		BONUS,
		WAIT,
		END
	}

	private string m_caption;

	private string m_quotaString;

	private int m_serverRewardId;

	private string m_reward;

	private bool m_isCleared;

	private GameObject m_quotaPlate;

	private Animation m_animation;

	private string m_animClipName;

	private bool m_isPlayEnd;

	private static readonly float WAIT_TIME = 0.5f;

	private float m_timer;

	private QuotaInfo.State m_state;

	public GameObject QuotaPlate
	{
		get
		{
			return this.m_quotaPlate;
		}
	}

	public string AnimClipName
	{
		get
		{
			return this.m_animClipName;
		}
	}

	public QuotaInfo(string caption, string quotaString, int serverRewardId, string reward, bool isCleared)
	{
		this.m_caption = caption;
		this.m_quotaString = quotaString;
		this.m_serverRewardId = serverRewardId;
		this.m_reward = reward;
		this.m_isCleared = isCleared;
	}

	public void Setup(GameObject quotaPlate, Animation animation, string animClipName)
	{
		this.m_quotaPlate = quotaPlate;
		this.m_animation = animation;
		this.m_animClipName = animClipName;
	}

	public void SetupDisplay()
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)this.m_serverRewardId);
		bool flag = false;
		if (serverItem.idType == ServerItem.IdType.CHAO)
		{
			flag = true;
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_quotaPlate, "img_item_0");
		if (uISprite != null)
		{
			uISprite.gameObject.SetActive(!flag);
			uISprite.spriteName = PresentBoxUtility.GetItemSpriteName(serverItem);
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_quotaPlate, "texture_chao_0");
		if (uISprite2 != null)
		{
			uISprite2.gameObject.SetActive(false);
		}
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_quotaPlate, "texture_chao_1");
		if (uITexture != null)
		{
			uITexture.gameObject.SetActive(flag);
			if (flag && ChaoTextureManager.Instance != null)
			{
				uITexture.alpha = 1f;
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
				ChaoTextureManager.Instance.GetTexture(serverItem.chaoId, info);
			}
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_quotaPlate, "Lbl_event_object_total_num");
		if (uILabel != null)
		{
			uILabel.text = this.m_quotaString;
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_quotaPlate, "Lbl_itemname");
		if (uILabel2 != null)
		{
			uILabel2.text = this.m_reward;
			UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_quotaPlate, "Lbl_itemname_sh");
			if (uILabel3 != null)
			{
				uILabel3.text = this.m_reward;
			}
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_quotaPlate, "Lbl_word_event_object_total");
		if (uILabel4 != null)
		{
			uILabel4.text = this.m_caption;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_quotaPlate, "get_icon_Anim");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	public void PlayStart()
	{
		this.m_state = QuotaInfo.State.IN;
		this.m_isPlayEnd = false;
		this.m_timer = 0f;
		this.SetupDisplay();
		if (string.IsNullOrEmpty(this.m_animClipName))
		{
			return;
		}
		if (this.m_animation == null)
		{
			return;
		}
		ActiveAnimation component = this.m_animation.gameObject.GetComponent<ActiveAnimation>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		this.m_animation.enabled = true;
		this.m_animation.Rewind();
		this.m_animation.Play(this.m_animClipName);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_quotaPlate, "get_icon_Anim");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	public bool IsPlayEnd()
	{
		return this.m_isPlayEnd;
	}

	public virtual void Update()
	{
		switch (this.m_state)
		{
		case QuotaInfo.State.IN:
			if (this.m_animation != null && !this.m_animation.IsPlaying(this.m_animClipName))
			{
				if (this.m_isCleared)
				{
					Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_quotaPlate, "get_icon_Anim");
					if (animation != null)
					{
						animation.gameObject.SetActive(true);
						animation.Rewind();
						animation.Play("ui_Event_mission_getin_Anim");
						SoundManager.SePlay("sys_result_decide", "SE");
					}
					this.m_state = QuotaInfo.State.BONUS;
				}
				else
				{
					this.m_state = QuotaInfo.State.WAIT;
				}
			}
			break;
		case QuotaInfo.State.BONUS:
		{
			Animation animation2 = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_quotaPlate, "get_icon_Anim");
			if (animation2 != null && !animation2.IsPlaying("ui_Event_mission_getin_Anim"))
			{
				this.m_state = QuotaInfo.State.WAIT;
			}
			break;
		}
		case QuotaInfo.State.WAIT:
			this.m_timer += Time.deltaTime;
			if (this.m_timer >= QuotaInfo.WAIT_TIME)
			{
				this.m_state = QuotaInfo.State.END;
			}
			break;
		case QuotaInfo.State.END:
			this.m_isPlayEnd = true;
			this.m_state = QuotaInfo.State.IDLE;
			break;
		}
	}
}
