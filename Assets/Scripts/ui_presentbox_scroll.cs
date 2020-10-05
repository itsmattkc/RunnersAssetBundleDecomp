using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ui_presentbox_scroll : MonoBehaviour
{
	private sealed class _SetUITexture_c__AnonStoreyFC
	{
		internal UITexture uiTex;

		internal void __m__68(Texture2D _faceTexture)
		{
			this.uiTex.mainTexture = _faceTexture;
		}
	}

	[SerializeField]
	private UIToggle m_toggle;

	[SerializeField]
	private GameObject m_imgChara;

	[SerializeField]
	private GameObject m_imgChao;

	[SerializeField]
	private GameObject m_imgItem;

	[SerializeField]
	private GameObject m_imgTex;

	[SerializeField]
	private UILabel m_infoLabel;

	[SerializeField]
	private UILabel m_itemNameLabel;

	[SerializeField]
	private UILabel m_receivedTimeLabel;

	private PresentBoxUI.PresentInfo m_persentInfo;

	private SocialInterface m_socialInterface;

	private string m_friendFBId = string.Empty;

	private bool m_check_flag;

	private bool m_se_skip_flag;

	private void Start()
	{
		base.enabled = false;
		if (this.m_toggle != null)
		{
			EventDelegate.Add(this.m_toggle.onChange, new EventDelegate.Callback(this.OnChangeToggle));
		}
	}

	private void OnDestroy()
	{
		this.ResetTextureData();
	}

	public void UpdateView(PresentBoxUI.PresentInfo info)
	{
		this.m_persentInfo = info;
		this.m_friendFBId = string.Empty;
		if (this.m_persentInfo != null)
		{
			if (this.CheckTextureDisplay())
			{
				this.SetUITexture();
			}
			else
			{
				this.SetUISprite();
			}
			if (this.m_itemNameLabel != null)
			{
				string itemName = PresentBoxUtility.GetItemName(this.m_persentInfo.serverItem);
				this.m_itemNameLabel.text = itemName + " × " + this.m_persentInfo.itemNum.ToString();
			}
			if (this.m_infoLabel != null)
			{
				if (this.m_persentInfo.operatorFlag)
				{
					this.m_infoLabel.text = this.m_persentInfo.infoText;
				}
				else
				{
					this.m_infoLabel.text = PresentBoxUtility.GetItemInfo(this.m_persentInfo);
				}
			}
			if (this.m_receivedTimeLabel != null)
			{
				this.m_receivedTimeLabel.text = PresentBoxUtility.GetReceivedTime(this.m_persentInfo.expireTime);
			}
			this.SetCheckFlag(this.m_persentInfo.checkFlag);
		}
	}

	public void ResetTextureData()
	{
		if (this.m_imgTex != null)
		{
			UITexture component = this.m_imgTex.GetComponent<UITexture>();
			if (component != null && component.mainTexture != null)
			{
				component.mainTexture = null;
			}
		}
	}

	public bool IsCheck()
	{
		return this.m_check_flag;
	}

	private void SetCheckFlag(bool check_flag)
	{
		if (this.m_toggle != null)
		{
			this.m_toggle.value = check_flag;
		}
		if (this.m_persentInfo != null)
		{
			this.m_persentInfo.checkFlag = check_flag;
		}
		this.m_check_flag = check_flag;
		this.m_se_skip_flag = true;
	}

	private void SetUITexture()
	{
		if (this.m_persentInfo != null)
		{
			this.SetUISprite(this.m_imgChara, false, string.Empty);
			this.SetUISprite(this.m_imgItem, false, string.Empty);
			if (this.m_imgChao != null)
			{
				this.m_imgChao.SetActive(false);
			}
			if (this.m_imgTex != null)
			{
				this.m_imgTex.SetActive(true);
				UITexture uiTex = this.m_imgTex.GetComponent<UITexture>();
				if (uiTex != null)
				{
					uiTex.enabled = true;
					PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
					if (playerImageManager != null)
					{
						uiTex.mainTexture = playerImageManager.GetPlayerImage(this.m_friendFBId, string.Empty, delegate(Texture2D _faceTexture)
						{
							uiTex.mainTexture = _faceTexture;
						});
					}
				}
			}
		}
	}

	private void SetUISprite()
	{
		if (this.m_imgTex != null)
		{
			this.m_imgTex.SetActive(false);
		}
		if (this.m_persentInfo != null)
		{
			if (this.m_persentInfo.serverItem.idType == ServerItem.IdType.CHARA)
			{
				CharaType charaType = this.m_persentInfo.serverItem.charaType;
				string arg_70_0 = "ui_tex_player_set_";
				int num = (int)charaType;
				string spriteName = arg_70_0 + num.ToString("00") + "_" + CharaName.PrefixName[(int)charaType];
				this.SetUISprite(this.m_imgChara, true, spriteName);
				this.SetUISprite(this.m_imgItem, false, string.Empty);
				if (this.m_imgChao != null)
				{
					this.m_imgChao.SetActive(false);
				}
			}
			else if (this.m_persentInfo.serverItem.idType == ServerItem.IdType.CHAO)
			{
				this.SetUISprite(this.m_imgChara, false, string.Empty);
				this.SetUISprite(this.m_imgChao, true, "ui_tex_chao_" + this.m_persentInfo.serverItem.chaoId.ToString("D4"));
				this.SetUISprite(this.m_imgItem, false, string.Empty);
				if (this.m_imgChao != null)
				{
					this.m_imgChao.SetActive(true);
					UITexture component = this.m_imgChao.GetComponent<UITexture>();
					int chaoId = this.m_persentInfo.serverItem.chaoId;
					ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(component, null, true);
					ChaoTextureManager.Instance.GetTexture(chaoId, info);
				}
			}
			else
			{
				this.SetUISprite(this.m_imgChara, false, string.Empty);
				this.SetUISprite(this.m_imgItem, true, PresentBoxUtility.GetItemSpriteName(this.m_persentInfo.serverItem));
				if (this.m_imgChao != null)
				{
					this.m_imgChao.SetActive(false);
				}
			}
		}
	}

	private void SetUISprite(GameObject obj, bool on, string spriteName = "")
	{
		if (obj != null)
		{
			obj.SetActive(on);
			if (on)
			{
				UISprite component = obj.GetComponent<UISprite>();
				if (component != null)
				{
					component.spriteName = spriteName;
				}
			}
		}
	}

	private void SetSocialInterface()
	{
		if (this.m_socialInterface == null)
		{
			this.m_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		}
	}

	private bool CheckTextureDisplay()
	{
		if (this.m_persentInfo != null && this.m_persentInfo.messageType == ServerMessageEntry.MessageType.SendEnergy)
		{
			this.SetSocialInterface();
			if (this.m_socialInterface != null)
			{
				SocialUserData socialUserDataFromGameId = SocialInterface.GetSocialUserDataFromGameId(this.m_socialInterface.FriendList, this.m_persentInfo.fromId);
				if (socialUserDataFromGameId != null)
				{
					this.m_friendFBId = socialUserDataFromGameId.Id;
					return !socialUserDataFromGameId.IsSilhouette;
				}
			}
		}
		return false;
	}

	private void OnChangeToggle()
	{
		if (this.m_toggle != null)
		{
			this.m_check_flag = this.m_toggle.value;
			if (this.m_persentInfo != null)
			{
				this.m_persentInfo.checkFlag = this.m_check_flag;
			}
		}
		if (!this.m_se_skip_flag)
		{
			if (this.m_check_flag)
			{
				SoundManager.SePlay("sys_menu_decide", "SE");
			}
			else
			{
				SoundManager.SePlay("sys_window_close", "SE");
			}
		}
		this.m_se_skip_flag = false;
	}
}
