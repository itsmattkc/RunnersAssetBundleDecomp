using System;
using UnityEngine;

public class RankingSimpleScroll : MonoBehaviour
{
	private UILabel m_nameLable;

	private string m_id;

	public UIToggle m_toggle;

	private UITexture m_texture;

	private SocialUserData m_userData;

	private int m_defaultImageHash;

	private void Start()
	{
		PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
		if (playerImageManager != null)
		{
			this.m_defaultImageHash = playerImageManager.GetDefaultImage().GetHashCode();
		}
		this.m_nameLable = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_username");
		this.m_toggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_ranking_top");
		this.m_texture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_tex_icon_friends");
	}

	public void SetUserData(SocialUserData user)
	{
		this.m_userData = user;
		this.m_id = user.Id;
		this.m_nameLable.text = user.Name;
	}

	public void LoadImage()
	{
		PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
		if (playerImageManager != null)
		{
			Texture2D playerImage = playerImageManager.GetPlayerImage(this.m_userData.Id, this.m_userData.Url, delegate(Texture2D _faceTexture)
			{
				if (_faceTexture.GetHashCode() != this.m_defaultImageHash && this.m_texture.mainTexture.GetHashCode() != _faceTexture.GetHashCode())
				{
					this.m_texture.mainTexture = _faceTexture;
				}
			});
			if (this.m_texture.mainTexture.GetHashCode() != playerImage.GetHashCode())
			{
				this.m_texture.mainTexture = playerImage;
			}
		}
	}

	public void OnClickRankingScroll()
	{
		RankingFriendOptionWindow rankingFriendOptionWindow = GameObjectUtil.FindGameObjectComponent<RankingFriendOptionWindow>("RankingFriendOptionWindow");
		if (rankingFriendOptionWindow != null)
		{
			rankingFriendOptionWindow.ChooseFriend(this.m_userData, this.m_toggle);
		}
	}
}
