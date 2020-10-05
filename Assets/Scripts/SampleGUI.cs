using NoahUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SampleGUI : MonoBehaviour
{
	private sealed class _DelayConnection_c__IteratorCD : IDisposable, IEnumerator, IEnumerator<object>
	{
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
				Noah.Instance.Suspend();
				this._current = new WaitForSeconds(0.5f);
				this._PC = 1;
				return true;
			case 1u:
				Noah.Instance.Resume();
				Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key);
				this._PC = -1;
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

	private sealed class _DelayConnectionAndCommit_c__IteratorCE : IDisposable, IEnumerator, IEnumerator<object>
	{
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
				Noah.Instance.Suspend();
				this._current = new WaitForSeconds(0.5f);
				this._PC = 1;
				return true;
			case 1u:
				Noah.Instance.Resume();
				Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
				this._PC = -1;
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

	public GameObject cube;

	public int selGridInt;

	public string[] selStrings;

	private ScreenOrientation prevOrientation;

	public static float scrollStartPositionY;

	public static Vector2 scrollPosition;

	public static Vector2 scrollLogPosition;

	public float scrollDistance;

	public static string logText = string.Empty;

	private void Start()
	{
		Application.targetFrameRate = 30;
		this.prevOrientation = Screen.orientation;
		global::Debug.Log("Screen.orientation = " + Screen.orientation);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKey("escape"))
		{
			Application.Quit();
		}
		if (this.prevOrientation != Screen.orientation)
		{
			this.prevOrientation = Screen.orientation;
			Noah.Instance.CloseBanner();
			Noah.Instance.CloseOffer();
			Noah.Instance.CloseShop();
		}
		if (UnityEngine.Input.touchCount != 0)
		{
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Began)
			{
				SampleGUI.scrollStartPositionY = (float)Screen.height - touch.position.y;
			}
			if (SampleGUI.scrollStartPositionY < (float)Screen.height * 0.7f)
			{
				if (touch.phase == TouchPhase.Moved)
				{
					SampleGUI.scrollPosition.y = SampleGUI.scrollPosition.y + touch.deltaPosition.y;
					this.scrollDistance += Mathf.Abs(touch.deltaPosition.y);
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				SampleGUI.scrollLogPosition.y = SampleGUI.scrollLogPosition.y + touch.deltaPosition.y;
				this.scrollDistance += Mathf.Abs(touch.deltaPosition.y);
			}
		}
		else
		{
			this.scrollDistance = 0f;
		}
	}

	private bool IsSelect()
	{
		return this.scrollDistance < 40f * ((float)Screen.height / 480f);
	}

	public static void AddLogText(string text)
	{
		string text2 = SampleGUI.logText;
		SampleGUI.logText = string.Concat(new string[]
		{
			text2,
			DateTime.Now.ToString("yy/MM/dd HH:mm:ss.ff"),
			" : ",
			text,
			"\n\n"
		});
		SampleGUI.scrollLogPosition.y = 1000000f;
	}

	private void OnGUI()
	{
		float height = 40f * ((float)Screen.height / 480f);
		SampleGUI.scrollPosition = GUILayout.BeginScrollView(SampleGUI.scrollPosition, new GUILayoutOption[]
		{
			GUILayout.Width((float)(Screen.width - 10)),
			GUILayout.Height((float)Screen.height * 0.7f)
		});
		GUILayout.Label(string.Concat(new string[]
		{
			NoahHandler.app_name,
			"\n",
			NoahHandler.consumer_key,
			"\n",
			NoahHandler.secret_key,
			"\n",
			NoahHandler.action_id
		}), new GUILayoutOption[0]);
		if (GUILayout.Button("0. setting", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("appinfo");
		}
		if (GUILayout.Button("1-1. connect", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			base.StartCoroutine("DelayConnection");
		}
		if (GUILayout.Button("1-2. connect & commit offer", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			base.StartCoroutine("DelayConnectionAndCommit");
		}
		if (GUILayout.Button("1-3. commit", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Commit(NoahHandler.action_id);
		}
		if (GUILayout.Button("1-4. setGUID", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.SetGUID("guid");
		}
		if (GUILayout.Button("2-1. show banner (standard size)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeStandard);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeStandard, 0f, (float)Screen.height - bannerSize.y);
		}
		if (GUILayout.Button("2-2. show banner (wide size)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize2 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeWide);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeWide, 0f, (float)Screen.height - bannerSize2.y);
		}
		if (GUILayout.Button("2-3. show banner (large portrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.ShowBannerView(Noah.BannerSize.Size224x336, 0f, 0f);
		}
		if (GUILayout.Button("2-4. show banner (large landscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.ShowBannerView(Noah.BannerSize.Size336x224, 0f, 0f);
		}
		if (GUILayout.Button("2-5. show banner (standard/center)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Vector2 bannerSize3 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeStandardFillParentWidth);
				Noah.Instance.ShowBannerView(Noah.BannerSize.SizeStandardFillParentWidth, 0f, (float)Screen.height - bannerSize3.y);
			}
			else
			{
				SampleGUI.AddLogText("SizeStandard2xFillParentWidth is only supported on Android");
			}
		}
		if (GUILayout.Button("2-6. show banner (wide/center)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Vector2 bannerSize4 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeWideFillParentWidth);
				Noah.Instance.ShowBannerView(Noah.BannerSize.SizeWideFillParentWidth, 0f, (float)Screen.height - bannerSize4.y);
			}
			else
			{
				SampleGUI.AddLogText("SizeStandard2xFillParentWidth is only supported on Android");
			}
		}
		if (GUILayout.Button("2-7. show banner (standard x2 size)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize5 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeStandard2x);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeStandard2x, 0f, (float)Screen.height - bannerSize5.y);
		}
		if (GUILayout.Button("2-8. show banner (wide x2 size)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize6 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeWide2x);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeWide2x, 0f, (float)Screen.height - bannerSize6.y);
		}
		if (GUILayout.Button("2-9. show banner (large portrait x2)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.ShowBannerView(Noah.BannerSize.Size448x672, 0f, 0f);
		}
		if (GUILayout.Button("2-10. show banner (large landscape x2)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.ShowBannerView(Noah.BannerSize.Size672x448, 0f, 0f);
		}
		if (GUILayout.Button("2-11. show banner (standard/center x2)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Vector2 bannerSize7 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeStandard2xFillParentWidth);
				Noah.Instance.ShowBannerView(Noah.BannerSize.SizeStandard2xFillParentWidth, 0f, (float)Screen.height - bannerSize7.y);
			}
			else
			{
				SampleGUI.AddLogText("SizeStandard2xFillParentWidth is only supported on Android");
			}
		}
		if (GUILayout.Button("2-12. show banner (wide/center x2)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Vector2 bannerSize8 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeWide2xFillParentWidth);
				Noah.Instance.ShowBannerView(Noah.BannerSize.SizeWide2xFillParentWidth, 0f, (float)Screen.height - bannerSize8.y);
			}
			else
			{
				SampleGUI.AddLogText("SizeStandard2xFillParentWidth is only supported on Android");
			}
		}
		if (GUILayout.Button("2-13. close banner", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.CloseBanner();
		}
		if (GUILayout.Button("2-14. set banner effect (downward)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.SetBannerEffect(Noah.BannerEffect.EffectDown);
		}
		if (GUILayout.Button("2-15. set banner effect (upward)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.SetBannerEffect(Noah.BannerEffect.EffectUp);
		}
		if (GUILayout.Button("2-16. show banner (standard, tag)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize9 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeStandard);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeStandard, 0f, (float)Screen.height - bannerSize9.y, "debug,standard");
		}
		if (GUILayout.Button("2-17. show banner (wide, tag)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 bannerSize10 = Noah.Instance.GetBannerSize(Noah.BannerSize.SizeWide);
			Noah.Instance.ShowBannerView(Noah.BannerSize.SizeWide, 0f, (float)Screen.height - bannerSize10.y, "debug,wide");
		}
		if (GUILayout.Button("3-10. open banner wall (portrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.Portrait);
		}
		if (GUILayout.Button("3-11. open banner wall (landscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.Landscape);
		}
		if (GUILayout.Button("3-12. open banner wall (portrait with tag)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.Portrait, false, "debug,wall,portrait");
		}
		if (GUILayout.Button("3-13 open banner wall (landscape with tag)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.Landscape, false, "debug,wall,standard");
		}
		if (GUILayout.Button("3-14. open banner wall (ReversePortrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.ReversePortrait, false);
		}
		if (GUILayout.Button("3-15 open banner wall (ReverseLandscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.ReverseLandscape, false);
		}
		if (GUILayout.Button("3-16. open banner wall (SensorPortrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.SensorPortrait, false);
		}
		if (GUILayout.Button("3-17 open banner wall (SensorLandscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.SensorLandscape, false);
		}
		if (GUILayout.Button("3-18 open banner wall (Sensor)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.BannerWall(Noah.Orientation.Sensor, false);
		}
		if (GUILayout.Button("4-3 show offer image (white)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			this.cube.renderer.material.mainTexture = Noah.Instance.GetOfferBitmap(Noah.OfferButtonImage.White);
		}
		if (GUILayout.Button("4-4 show offer image (Black)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			this.cube.renderer.material.mainTexture = Noah.Instance.GetOfferBitmap(Noah.OfferButtonImage.Black);
		}
		if (GUILayout.Button("4-8 open offer list (portrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.Portrait);
		}
		if (GUILayout.Button("4-9 open offer list (Landscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.Landscape);
		}
		if (GUILayout.Button("4-10 open offer list portrait with tag", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.Portrait, "debug,offer");
		}
		if (GUILayout.Button("4-11 open offer list  landscape with tag", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.Landscape, "debug,offer");
		}
		if (GUILayout.Button("4-12 open offer list (portrait reverse)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.ReversePortrait);
		}
		if (GUILayout.Button("4-13 open offer list (landscape reverse)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.ReverseLandscape);
		}
		if (GUILayout.Button("4-14 open offer list (sensor portrait)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.SensorPortrait);
		}
		if (GUILayout.Button("4-15 open offer list (sensor landscape)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.SensorLandscape);
		}
		if (GUILayout.Button("4-16 open offer list (sensor 4-way)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Offer("test", Noah.Orientation.Sensor);
		}
		if (GUILayout.Button("6-1 show review dialog", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Review();
		}
		if (GUILayout.Button("6-2 show review dialog with tag", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.Review("debug,revielw");
		}
		if (GUILayout.Button("7-1 show number of rewards", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Vector2 rewardSize = Noah.Instance.GetRewardSize();
			Noah.Instance.ShowRewardView(0f, (float)Screen.height - rewardSize.y);
		}
		if (GUILayout.Button("7-2. set rewaerd display effect (downward)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.SetRewardEffect(Noah.RewardEffect.EffectDown);
		}
		if (GUILayout.Button("7-3. set rewaerd display effect (upward)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			Noah.Instance.SetRewardEffect(Noah.RewardEffect.EffectUp);
		}
		if (GUILayout.Button("7-4. show new offer badge image(red)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			this.cube.renderer.material.mainTexture = Noah.Instance.GetNewBadge(Noah.BadgeType.Red);
		}
		if (GUILayout.Button("7-5. show new offer badge image(blue)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			this.cube.renderer.material.mainTexture = Noah.Instance.GetNewBadge(Noah.BadgeType.Blue);
		}
		if (GUILayout.Button("7-6. show new offer badge image(grey)", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			this.cube.renderer.material.mainTexture = Noah.Instance.GetNewBadge(Noah.BadgeType.Gray);
		}
		if (GUILayout.Button("8-1. check new offer", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			string message = "new offer = " + Noah.Instance.HasNewOffer();
			if (Application.platform == RuntimePlatform.Android)
			{
				NoahAndroid.Toast(message);
			}
		}
		if (GUILayout.Button("98. show debug info", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			global::Debug.Log("button " + this.selGridInt + "clicked");
			string text = string.Concat(new object[]
			{
				"version = ",
				Noah.Instance.GetVersion(),
				"\nconsumer_key = ",
				NoahHandler.consumer_key,
				"\nsecret_key = ",
				NoahHandler.secret_key,
				"\naction_id = ",
				NoahHandler.action_id,
				"\ndebug flag = ",
				NoahHandler.noahDebug,
				"\nnoah id = ",
				Noah.Instance.GetNoahID(),
				"\ncheck token = ",
				Noah.Instance.GetCheckToken(),
				"\nbanner flag = ",
				Noah.Instance.GetBannerFlag(),
				"\nbanner wall flag = ",
				Noah.Instance.GetBannerWallFlag(),
				"\noffer flag = ",
				Noah.Instance.GetOfferFlag(),
				"\nshop flag = ",
				Noah.Instance.GetShopFlag(),
				"\nreview flag = ",
				Noah.Instance.GetReviewFlag(),
				"\nreward flag = ",
				Noah.Instance.GetRewardFlag(),
				"\nnew reward = ",
				Noah.Instance.HasNewOffer(),
				"\n"
			});
			text += "Dialog info: ";
			ArrayList alertMessage = Noah.Instance.GetAlertMessage();
			int num = 0;
			while (alertMessage != null && num < alertMessage.Count)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Dialog ",
					num + 1,
					"\n"
				});
				Hashtable hashtable = (Hashtable)alertMessage[num];
				text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"title:",
					(string)hashtable["title"],
					"\nmessage:",
					(string)hashtable["msg"],
					"\nbutton:",
					(string)hashtable["btn"],
					"\n"
				});
				num++;
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				NoahAndroid.Toast(text);
			}
		}
		if (GUILayout.Button("99. clear user data", new GUILayoutOption[]
		{
			GUILayout.Height(height)
		}) && this.IsSelect())
		{
			SampleGUI.AddLogText("clear user data...");
			Noah.Instance.Delete();
		}
		GUILayout.EndScrollView();
		SampleGUI.scrollLogPosition = GUILayout.BeginScrollView(SampleGUI.scrollLogPosition, new GUILayoutOption[]
		{
			GUILayout.Width((float)(Screen.width - 10)),
			GUILayout.Height((float)Screen.height * 0.3f)
		});
		GUILayout.Label(SampleGUI.logText, new GUILayoutOption[]
		{
			GUILayout.Width((float)(Screen.width - 40))
		});
		GUILayout.EndScrollView();
	}

	public IEnumerator DelayConnection()
	{
		return new SampleGUI._DelayConnection_c__IteratorCD();
	}

	public IEnumerator DelayConnectionAndCommit()
	{
		return new SampleGUI._DelayConnectionAndCommit_c__IteratorCE();
	}
}
