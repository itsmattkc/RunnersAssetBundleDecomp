using Message;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SettingPartsInviteButton : MonoBehaviour
{
	public delegate void ButtonPressedCallback(SocialUserData friendData);

	private sealed class _Setup_c__AnonStoreyFF
	{
		internal UITexture texture;

		internal void __m__70(Texture2D _faceTexture)
		{
			this.texture.mainTexture = _faceTexture;
		}
	}

	private SocialUserData m_friendData;

	private SettingPartsInviteButton.ButtonPressedCallback m_callback;

	public void Setup(SocialUserData friendData, SettingPartsInviteButton.ButtonPressedCallback callback)
	{
		if (friendData == null)
		{
			return;
		}
		this.m_friendData = friendData;
		this.m_callback = callback;
		UITexture texture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_icon_friends");
		if (texture != null)
		{
			PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
			texture.mainTexture = playerImageManager.GetPlayerImage(this.m_friendData.Id, string.Empty, delegate(Texture2D _faceTexture)
			{
				texture.mainTexture = _faceTexture;
			});
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_friend_name");
		if (uILabel != null)
		{
			uILabel.text = this.m_friendData.Name;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_invite");
		if (gameObject != null)
		{
			UIButtonMessage uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnClickButton";
		}
	}

	private void OnClickButton()
	{
		if (this.m_callback != null)
		{
			this.m_callback(this.m_friendData);
		}
	}

	private void InviteFriendEndCallback(MsgSocialNormalResponse msg)
	{
	}
}
