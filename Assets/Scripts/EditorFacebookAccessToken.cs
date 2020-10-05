using Facebook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorFacebookAccessToken : MonoBehaviour
{
	private sealed class _Start_c__Iterator3 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string _downloadUrl___0;

		internal WWW _www___1;

		internal int _PC;

		internal object _current;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (!(EditorFacebookAccessToken.fbSkin != null))
				{
					this._downloadUrl___0 = IntegratedPluginCanvasLocation.FbSkinUrl;
					this._www___1 = new WWW(this._downloadUrl___0);
					this._current = this._www___1;
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				if (this._www___1.error != null)
				{
					FbDebug.Error("Could not find the Facebook Skin: " + this._www___1.error);
				}
				else
				{
					EditorFacebookAccessToken.fbSkin = (this._www___1.assetBundle.mainAsset as GUISkin);
					this._www___1.assetBundle.Unload(false);
					this._PC = -1;
				}
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const float windowWidth = 592f;

	private float windowHeight = 200f;

	private string accessToken = string.Empty;

	private bool isLoggingIn;

	private static GUISkin fbSkin;

	private GUIStyle greyButton;

	private IEnumerator Start()
	{
		return new EditorFacebookAccessToken._Start_c__Iterator3();
	}

	private void OnGUI()
	{
		float top = (float)(Screen.height / 2) - this.windowHeight / 2f;
		float left = (float)(Screen.width / 2) - 296f;
		if (EditorFacebookAccessToken.fbSkin != null)
		{
			GUI.skin = EditorFacebookAccessToken.fbSkin;
			this.greyButton = EditorFacebookAccessToken.fbSkin.GetStyle("greyButton");
		}
		else
		{
			this.greyButton = GUI.skin.button;
		}
		GUI.ModalWindow(this.GetHashCode(), new Rect(left, top, 592f, this.windowHeight), new GUI.WindowFunction(this.OnGUIDialog), "Unity Editor Facebook Login");
	}

	private void OnGUIDialog(int windowId)
	{
		GUI.enabled = !this.isLoggingIn;
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.Label("User Access Token:", new GUILayoutOption[0]);
		GUILayout.EndVertical();
		this.accessToken = GUILayout.TextField(this.accessToken, GUI.skin.textArea, new GUILayoutOption[]
		{
			GUILayout.MinWidth(400f)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("Find Access Token", new GUILayoutOption[0]))
		{
			Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
		}
		GUILayout.FlexibleSpace();
		GUIContent content = new GUIContent("Login");
		Rect rect = GUILayoutUtility.GetRect(content, GUI.skin.button);
		if (GUI.Button(rect, content))
		{
			EditorFacebook component = FBComponentFactory.GetComponent<EditorFacebook>(IfNotExist.AddNew);
			component.AccessToken = this.accessToken;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["batch"] = "[{\"method\":\"GET\", \"relative_url\":\"me?fields=id\"},{\"method\":\"GET\", \"relative_url\":\"app?fields=id\"}]";
			dictionary["method"] = "POST";
			dictionary["access_token"] = this.accessToken;
			FB.API("/", HttpMethod.GET, new FacebookDelegate(component.MockLoginCallback), dictionary);
			this.isLoggingIn = true;
		}
		GUI.enabled = true;
		GUIContent content2 = new GUIContent("Cancel");
		Rect rect2 = GUILayoutUtility.GetRect(content2, this.greyButton);
		if (GUI.Button(rect2, content2, this.greyButton))
		{
			FBComponentFactory.GetComponent<EditorFacebook>(IfNotExist.AddNew).MockCancelledLoginCallback();
			UnityEngine.Object.Destroy(this);
		}
		GUILayout.EndHorizontal();
		if (Event.current.type == EventType.Repaint)
		{
			this.windowHeight = rect2.y + rect2.height + (float)GUI.skin.window.padding.bottom;
		}
	}
}
