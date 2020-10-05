using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuFacebook : UIDebugMenuTask
{
	private enum MenuType
	{
		FEED,
		DELETE_CUSTOM_DATA,
		LOGOUT,
		CREATE_TEST_USER,
		CREATE_MASTER_USER,
		NUM
	}

	private List<string> MenuObjName = new List<string>
	{
		"Feed",
		"DeleteCustomData",
		"Logout",
		"CreateTestUser",
		"CreateMasterUser"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 150f, 50f),
		new Rect(400f, 200f, 150f, 50f),
		new Rect(600f, 200f, 150f, 50f),
		new Rect(200f, 300f, 150f, 50f),
		new Rect(200f, 570f, 150f, 50f)
	};

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuTextField m_testUserCountField;

	private UIDebugMenuTextField m_masterUserIdField;

	private UIDebugMenuTextField m_masterUserAccessTokenField;

	private FacebookTestUserCreater m_testUserCreate;

	private FacebookMasterUserCreater m_masterUserCreate;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		for (int i = 0; i < 5; i++)
		{
			string name = this.MenuObjName[i];
			GameObject x = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (!(x == null))
			{
				this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
			}
		}
		this.m_testUserCountField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_testUserCountField.Setup(new Rect(200f, 370f, 350f, 50f), "作成するテストユーザー数");
		this.m_testUserCountField.text = "2";
		this.m_masterUserIdField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_masterUserIdField.Setup(new Rect(200f, 450f, 350f, 50f), "全ユーザーと友達にしたいユーザーID");
		this.m_masterUserIdField.text = string.Empty;
		this.m_masterUserAccessTokenField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_masterUserAccessTokenField.Setup(new Rect(200f, 520f, 350f, 50f), "全ユーザーと友達にしたいユーザーのアクセストークン");
		this.m_masterUserAccessTokenField.text = string.Empty;
	}

	protected override void OnTransitionTo()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(false);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_testUserCountField != null)
		{
			this.m_testUserCountField.SetActive(false);
		}
		if (this.m_masterUserIdField != null)
		{
			this.m_masterUserIdField.SetActive(false);
		}
		if (this.m_masterUserAccessTokenField != null)
		{
			this.m_masterUserAccessTokenField.SetActive(false);
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(true);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_testUserCountField != null)
		{
			this.m_testUserCountField.SetActive(true);
		}
		if (this.m_masterUserIdField != null)
		{
			this.m_masterUserIdField.SetActive(true);
		}
		if (this.m_masterUserAccessTokenField != null)
		{
			this.m_masterUserAccessTokenField.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name == "CreateTestUser")
		{
			if (this.m_testUserCountField != null)
			{
				int createCount = int.Parse(this.m_testUserCountField.text);
				if (this.m_testUserCreate == null)
				{
					this.m_testUserCreate = base.gameObject.AddComponent<FacebookTestUserCreater>();
				}
				this.m_testUserCreate.Create(createCount);
			}
		}
		else if (name == "CreateMasterUser")
		{
			if (this.m_masterUserIdField != null && this.m_masterUserAccessTokenField != null)
			{
				if (this.m_masterUserCreate == null)
				{
					this.m_masterUserCreate = base.gameObject.AddComponent<FacebookMasterUserCreater>();
				}
				string text = this.m_masterUserIdField.text;
				string text2 = this.m_masterUserAccessTokenField.text;
				this.m_masterUserCreate.AttachFriend(text, text2);
			}
		}
		else
		{
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface == null)
			{
				return;
			}
			if (name.Contains("Feed"))
			{
				socialInterface.Feed("デバッグ投稿", "デバッグ投稿です", base.gameObject);
			}
			else if (name.Contains("DeleteCustomData"))
			{
				socialInterface.Login(base.gameObject);
			}
			else if (name.Contains("Logout"))
			{
				socialInterface.Logout();
			}
		}
	}
}
