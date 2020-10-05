using DataTable;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class PresentBoxUI : MonoBehaviour
{
	public class PresentInfo
	{
		public int messageId = -1;

		public int itemId = -1;

		public int itemNum;

		public int expireTime;

		public ServerMessageEntry.MessageType messageType = ServerMessageEntry.MessageType.Unknown;

		public string name = string.Empty;

		public string infoText = string.Empty;

		public string fromId = string.Empty;

		public ServerItem serverItem;

		public Texture charaTex;

		public bool operatorFlag;

		public bool checkFlag = true;

		public PresentInfo(ServerMessageEntry msg)
		{
			if (msg != null)
			{
				this.messageId = msg.m_messageId;
				this.messageType = msg.m_messageType;
				this.itemId = msg.m_presentState.m_itemId;
				this.itemNum = msg.m_presentState.m_numItem;
				this.expireTime = msg.m_expireTiem;
				this.name = msg.m_name;
				this.fromId = msg.m_fromId;
				this.serverItem = new ServerItem((ServerItem.Id)this.itemId);
			}
		}

		public PresentInfo(ServerOperatorMessageEntry msg)
		{
			if (msg != null)
			{
				this.messageId = msg.m_messageId;
				this.itemId = msg.m_presentState.m_itemId;
				this.itemNum = msg.m_presentState.m_numItem;
				this.expireTime = msg.m_expireTiem;
				this.infoText = msg.m_content;
				this.serverItem = new ServerItem((ServerItem.Id)this.itemId);
				this.operatorFlag = true;
			}
		}
	}

	private sealed class _ShowOnlyOutOfDataResult_c__Iterator4F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal PresentBoxUI __f__this;

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
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "presentbox",
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextUtility.GetCommonText("PresentBox", "out_of_date_caption"),
					message = TextUtility.GetCommonText("PresentBox", "out_of_date_text"),
					parentGameObject = this.__f__this.gameObject
				});
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!GeneralWindow.IsButtonPressed)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			GeneralWindow.Close();
			this.__f__this.m_outOfDataFlag = false;
			this._PC = -1;
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

	private sealed class _ShowResult_c__Iterator50 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal CharaType _charaType___0;

		internal bool _charaFlag___1;

		internal bool _chaoFlag___2;

		internal bool _missFlag___3;

		internal MsgUpdateMesseageSucceed msg;

		internal List<ServerPresentState>.Enumerator __s_616___4;

		internal ServerPresentState _state___5;

		internal ServerItem _serverItem___6;

		internal GameObject _uiRoot___7;

		internal ServerPlayerState _playerState___8;

		internal ServerCharacterState _charaState___9;

		internal PlayerGetPartsOverlap _playerGet___10;

		internal ChaoGetPartsBase _partsBase___11;

		internal bool _tutorial___12;

		internal bool _disabledEqip___13;

		internal List<int> _chaoIdList___14;

		internal List<ServerPresentState>.Enumerator __s_617___15;

		internal ServerPresentState _state___16;

		internal ServerItem _serverItem___17;

		internal GameObject _uiRoot___18;

		internal ChaoData _data___19;

		internal ChaoGetPartsBase _partsBase___20;

		internal ChaoGetPartsNormal _normal___21;

		internal ChaoGetPartsRare _rare___22;

		internal bool _tutorial___23;

		internal bool _disabledEqip___24;

		internal int _PC;

		internal object _current;

		internal MsgUpdateMesseageSucceed ___msg;

		internal PresentBoxUI __f__this;

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
			bool flag = false;
			switch (num)
			{
			case 0u:
				this._charaType___0 = CharaType.UNKNOWN;
				this._charaFlag___1 = false;
				this._chaoFlag___2 = false;
				this._missFlag___3 = false;
				if (this.msg != null)
				{
					this._missFlag___3 = (this.msg.m_notRecvMessageList.Count + this.msg.m_notRecvOperatorMessageList.Count > 0);
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "presentbox",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextUtility.GetCommonText("PresentBox", "present_box"),
						message = PresentBoxUtility.GetPresetTextList(this.msg.m_presentStateList),
						parentGameObject = this.__f__this.gameObject
					});
				}
				else
				{
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "presentbox",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextUtility.GetCommonText("PresentBox", "present_box"),
						message = TextUtility.GetCommonText("PresentBox", "not_receive_message"),
						parentGameObject = this.__f__this.gameObject
					});
				}
				break;
			case 1u:
				break;
			case 2u:
				goto IL_20D;
			case 3u:
				IL_2A7:
				if (GeneralWindow.IsButtonPressed)
				{
					GeneralWindow.Close();
					goto IL_2B7;
				}
				this._current = null;
				this._PC = 3;
				return true;
			case 4u:
			case 5u:
				Block_9:
				try
				{
					switch (num)
					{
					case 4u:
						IL_428:
						if (!this.__f__this.m_playerMergeWindow.IsPlayEnd)
						{
							this._current = null;
							this._PC = 4;
							flag = true;
							return true;
						}
						this.__f__this.m_playerMergeWindow = null;
						break;
					case 5u:
						IL_54E:
						if (!this.__f__this.m_charaGetWindow.IsPlayEnd)
						{
							this._current = null;
							this._PC = 5;
							flag = true;
							return true;
						}
						this.__f__this.m_charaGetWindow = null;
						break;
					}
					IL_56F:
					while (this.__s_616___4.MoveNext())
					{
						this._state___5 = this.__s_616___4.Current;
						this._serverItem___6 = new ServerItem((ServerItem.Id)this._state___5.m_itemId);
						if (this._serverItem___6.idType == ServerItem.IdType.CHARA && this._serverItem___6.charaType != CharaType.UNKNOWN)
						{
							this._charaFlag___1 = true;
							this._charaType___0 = this._serverItem___6.charaType;
							this._uiRoot___7 = GameObject.Find("UI Root (2D)");
							this._playerState___8 = ServerInterface.PlayerState;
							if (this._playerState___8 != null)
							{
								this._charaState___9 = this._playerState___8.CharacterState(this._charaType___0);
								if (this._charaState___9 != null && this._charaState___9.star > 0)
								{
									if (this.__f__this.m_playerMergeWindow == null)
									{
										this.__f__this.m_playerMergeWindow = GameObjectUtil.FindChildGameObjectComponent<PlayerMergeWindow>(this._uiRoot___7, "player_merge_Window");
									}
									if (this.__f__this.m_playerMergeWindow != null)
									{
										this.__f__this.m_playerMergeWindow.PlayStart(this._state___5.m_itemId, RouletteUtility.AchievementType.PlayerGet);
										goto IL_428;
									}
								}
								else
								{
									if (this.__f__this.m_charaGetWindow == null)
									{
										this.__f__this.m_charaGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(this._uiRoot___7, "ro_PlayerGetWindowUI");
									}
									if (this.__f__this.m_charaGetWindow != null)
									{
										this._playerGet___10 = this.__f__this.m_charaGetWindow.gameObject.GetComponent<PlayerGetPartsOverlap>();
										if (this._playerGet___10 == null)
										{
											this._playerGet___10 = this.__f__this.m_charaGetWindow.gameObject.AddComponent<PlayerGetPartsOverlap>();
										}
										this._playerGet___10.Init(this._state___5.m_itemId, 100, 0, null, PlayerGetPartsOverlap.IntroType.NO_EGG);
										this._partsBase___11 = this._playerGet___10;
										this._tutorial___12 = false;
										this._disabledEqip___13 = true;
										this.__f__this.m_charaGetWindow.PlayStart(this._partsBase___11, this._tutorial___12, this._disabledEqip___13, RouletteUtility.AchievementType.NONE);
										goto IL_54E;
									}
								}
							}
						}
					}
					goto IL_599;
					goto IL_56F;
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)this.__s_616___4).Dispose();
					}
				}
				IL_599:
				this._chaoIdList___14 = new List<int>();
				this.__s_617___15 = this.msg.m_presentStateList.GetEnumerator();
				num = 4294967293u;
				goto Block_10;
			case 6u:
			case 7u:
				goto IL_5BD;
			case 8u:
				IL_96D:
				if (AchievementManager.IsRequestEnd())
				{
					goto IL_977;
				}
				this._current = null;
				this._PC = 8;
				return true;
			case 9u:
				IL_9FE:
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						this._chaoFlag___2 = false;
						MenuPlayerSetUtil.SetMarkCharaPage(this._charaType___0);
						HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHARA_MAIN, true);
					}
					GeneralWindow.Close();
					goto IL_A31;
				}
				this._current = null;
				this._PC = 9;
				return true;
			case 10u:
				IL_AB8:
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHAO, true);
					}
					GeneralWindow.Close();
					goto IL_AD9;
				}
				this._current = null;
				this._PC = 10;
				return true;
			default:
				return false;
			}
			if (!GeneralWindow.IsButtonPressed)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			GeneralWindow.Close();
			if (!this._missFlag___3)
			{
				goto IL_21D;
			}
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "presentbox",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("PresentBox", "present_box"),
				message = TextUtility.GetCommonText("PresentBox", "miss_message"),
				parentGameObject = this.__f__this.gameObject
			});
			IL_20D:
			if (!GeneralWindow.IsButtonPressed)
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			GeneralWindow.Close();
			IL_21D:
			if (this.__f__this.m_outOfDataFlag)
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "presentbox",
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextUtility.GetCommonText("PresentBox", "out_of_date_caption"),
					message = TextUtility.GetCommonText("PresentBox", "out_of_date_text"),
					parentGameObject = this.__f__this.gameObject
				});
				goto IL_2A7;
			}
			IL_2B7:
			if (this.msg != null)
			{
				this.__s_616___4 = this.msg.m_presentStateList.GetEnumerator();
				num = 4294967293u;
				goto Block_9;
			}
			goto IL_93A;
			Block_10:
			try
			{
				IL_5BD:
				switch (num)
				{
				case 6u:
					IL_70E:
					if (!this.__f__this.m_chaoMergeWindow.IsPlayEnd)
					{
						this._current = null;
						this._PC = 6;
						flag = true;
						return true;
					}
					this.__f__this.m_chaoMergeWindow = null;
					break;
				case 7u:
					IL_8EF:
					if (!this.__f__this.m_chaoGetWindow.IsPlayEnd)
					{
						this._current = null;
						this._PC = 7;
						flag = true;
						return true;
					}
					this.__f__this.m_chaoGetWindow = null;
					break;
				}
				while (this.__s_617___15.MoveNext())
				{
					this._state___16 = this.__s_617___15.Current;
					this._serverItem___17 = new ServerItem((ServerItem.Id)this._state___16.m_itemId);
					if (this._serverItem___17.idType == ServerItem.IdType.CHAO)
					{
						if (!this._chaoIdList___14.Contains(this._serverItem___17.chaoId))
						{
							this._chaoIdList___14.Add(this._serverItem___17.chaoId);
							this._uiRoot___18 = GameObject.Find("UI Root (2D)");
							this._data___19 = ChaoTable.GetChaoData(this._serverItem___17.chaoId);
							if (this._data___19 != null)
							{
								this._chaoFlag___2 = true;
								this._partsBase___20 = null;
								if (this._data___19.level > 0)
								{
									if (this.__f__this.m_chaoMergeWindow == null)
									{
										this.__f__this.m_chaoMergeWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoMergeWindow>(this._uiRoot___18, "chao_merge_Window");
									}
									this.__f__this.m_chaoMergeWindow.PlayStart(this._state___16.m_itemId, this._data___19.level, (int)this._data___19.rarity, RouletteUtility.AchievementType.NONE);
									goto IL_70E;
								}
								if (this._data___19.rarity == ChaoData.Rarity.NORMAL || this._data___19.rarity == ChaoData.Rarity.SRARE)
								{
									if (this.__f__this.m_chaoGetWindow == null)
									{
										this.__f__this.m_chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(this._uiRoot___18, "chao_get_Window");
									}
									this._normal___21 = this.__f__this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsNormal>();
									if (this._normal___21 == null)
									{
										this._normal___21 = this.__f__this.m_chaoGetWindow.gameObject.AddComponent<ChaoGetPartsNormal>();
									}
									this._normal___21.Init(this._state___16.m_itemId, (int)this._data___19.rarity);
									this._partsBase___20 = this._normal___21;
								}
								else
								{
									if (this.__f__this.m_chaoGetWindow == null)
									{
										this.__f__this.m_chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(this._uiRoot___18, "chao_rare_get_Window");
									}
									this._rare___22 = this.__f__this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsRare>();
									if (this._rare___22 == null)
									{
										this._rare___22 = this.__f__this.m_chaoGetWindow.gameObject.AddComponent<ChaoGetPartsRare>();
									}
									this._rare___22.Init(this._state___16.m_itemId, (int)this._data___19.rarity);
									this._partsBase___20 = this._rare___22;
								}
								this._tutorial___23 = false;
								this._disabledEqip___24 = true;
								this.__f__this.m_chaoGetWindow.PlayStart(this._partsBase___20, this._tutorial___23, this._disabledEqip___24, RouletteUtility.AchievementType.NONE);
								goto IL_8EF;
							}
						}
					}
				}
			}
			finally
			{
				if (!flag)
				{
					((IDisposable)this.__s_617___15).Dispose();
				}
			}
			IL_93A:
			if (this._charaFlag___1 || this._chaoFlag___2)
			{
				AchievementManager.RequestUpdate();
				goto IL_96D;
			}
			IL_977:
			if (this._charaFlag___1)
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "presentbox",
					buttonType = GeneralWindow.ButtonType.YesNo,
					caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_verification"),
					message = TextUtility.GetCommonText("PresentBox", "player_set_text"),
					parentGameObject = this.__f__this.gameObject
				});
				goto IL_9FE;
			}
			IL_A31:
			if (this._chaoFlag___2)
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "presentbox",
					buttonType = GeneralWindow.ButtonType.YesNo,
					caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_verification"),
					message = TextUtility.GetCommonText("PresentBox", "chao_set_text"),
					parentGameObject = this.__f__this.gameObject
				});
				goto IL_AB8;
			}
			IL_AD9:
			this.__f__this.m_outOfDataFlag = false;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 4u:
			case 5u:
				try
				{
				}
				finally
				{
					((IDisposable)this.__s_616___4).Dispose();
				}
				break;
			case 6u:
			case 7u:
				try
				{
				}
				finally
				{
					((IDisposable)this.__s_617___15).Dispose();
				}
				break;
			}
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const int DISPLAY_MAX_ITEM_COUNT = 10;

	[SerializeField]
	private UIRectItemStorage m_itemStorage;

	[SerializeField]
	private UIScrollBar m_scrollBar;

	[SerializeField]
	private UILabel m_recieveAllLabel;

	[SerializeField]
	private UILabel m_recieveSelectLabel;

	[SerializeField]
	private UILabel m_infoLabel;

	[SerializeField]
	private UILabel m_nextPageLabel;

	[SerializeField]
	private UILabel m_prevPageLabel;

	[SerializeField]
	private UILabel m_numPageLabel;

	[SerializeField]
	private GameObject m_recieveAllBtnObj;

	[SerializeField]
	private GameObject m_recieveSelectBtnObj;

	[SerializeField]
	private GameObject m_nextPageBtnObj;

	[SerializeField]
	private GameObject m_prevPageBtnObj;

	private int m_currentPageNum;

	private int m_totalPageCount;

	private bool m_init_flag;

	private bool m_outOfDataFlag;

	private bool m_setUp;

	private List<PresentBoxUI.PresentInfo> m_presentInfoList = new List<PresentBoxUI.PresentInfo>();

	private Dictionary<int, int> m_pageItemCountDic = new Dictionary<int, int>();

	private ChaoGetWindow m_charaGetWindow;

	private ChaoGetWindow m_chaoGetWindow;

	private ChaoMergeWindow m_chaoMergeWindow;

	private PlayerMergeWindow m_playerMergeWindow;

	public bool IsEndSetup
	{
		get
		{
			return this.m_setUp;
		}
	}

	private void Start()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface == null)
		{
			ServerInterface.DebugInit();
		}
		this.Initialize();
		base.enabled = false;
	}

	private void Update()
	{
	}

	private void Initialize()
	{
		if (!this.m_init_flag)
		{
			this.SetUIButtonMessage(this.m_recieveAllBtnObj, "OnClickedRecieveAllBtn");
			this.SetUIButtonMessage(this.m_recieveSelectBtnObj, "OnClickedRecieveSelectBtn");
			this.SetUIButtonMessage(this.m_nextPageBtnObj, "OnClickedNextPageBtn");
			this.SetUIButtonMessage(this.m_prevPageBtnObj, "OnClickedPrevPageBtnBtn");
			this.SetTextLabel(this.m_infoLabel, "information", null, null);
			this.SetTextLabel(this.m_recieveAllLabel, "recieve_all_item", null, null);
			this.SetTextLabel(this.m_recieveSelectLabel, "recieve_select_item", null, null);
			this.SetTextLabel(this.m_prevPageLabel, "prev_page", "{MAILE_COUNT}", 10.ToString());
			this.SetTextLabel(this.m_nextPageLabel, "next_page", "{MAILE_COUNT}", 10.ToString());
			this.m_init_flag = true;
		}
	}

	private void SetTextLabel(UILabel uiLabel, string cellId, string tagString, string replaceString)
	{
		if (uiLabel != null)
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PresentBox", cellId);
			if (text != null)
			{
				text.ReplaceTag(tagString, replaceString);
				uiLabel.text = text.text;
			}
		}
	}

	private void SetUIButtonMessage(GameObject obj, string callBackName)
	{
		if (obj != null)
		{
			UIButtonMessage component = obj.GetComponent<UIButtonMessage>();
			if (component == null)
			{
				obj.AddComponent<UIButtonMessage>();
				component = obj.GetComponent<UIButtonMessage>();
			}
			if (component != null)
			{
				component.enabled = true;
				component.trigger = UIButtonMessage.Trigger.OnClick;
				component.target = base.gameObject;
				component.functionName = callBackName;
			}
		}
	}

	private void UpdatePage()
	{
		this.UpdateRectItemStorage();
		this.UpdateText();
		bool flag = this.m_totalPageCount > 1;
		this.SetEnableImageButton(this.m_nextPageBtnObj, flag);
		this.SetEnableImageButton(this.m_prevPageBtnObj, flag);
		bool flag2 = this.m_totalPageCount > 0;
		this.SetEnableImageButton(this.m_recieveAllBtnObj, flag2);
		this.SetEnableImageButton(this.m_recieveSelectBtnObj, flag2);
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.value = 0f;
		}
	}

	private void SetEnableImageButton(GameObject obj, bool flag)
	{
		if (obj != null)
		{
			UIImageButton component = obj.GetComponent<UIImageButton>();
			if (component != null)
			{
				component.isEnabled = flag;
			}
		}
	}

	private void UpdateText()
	{
		if (this.m_numPageLabel != null)
		{
			if (this.m_totalPageCount == 0)
			{
				this.m_numPageLabel.text = "0/0";
			}
			else
			{
				this.m_numPageLabel.text = (1 + this.m_currentPageNum).ToString() + "/" + this.m_totalPageCount.ToString();
			}
		}
	}

	private void UpdateRectItemStorage()
	{
		if (this.m_itemStorage != null && this.m_presentInfoList != null)
		{
			if (this.m_totalPageCount == 0)
			{
				this.m_itemStorage.maxItemCount = 0;
				this.m_itemStorage.maxRows = 0;
				this.m_currentPageNum = 0;
				this.m_itemStorage.Restart();
			}
			else
			{
				this.m_currentPageNum = Mathf.Min(this.m_currentPageNum, this.m_totalPageCount - 1);
				this.m_currentPageNum = Mathf.Max(this.m_currentPageNum, 0);
				int num = Mathf.Clamp(this.m_pageItemCountDic[this.m_currentPageNum], 0, 10);
				this.m_itemStorage.maxItemCount = num;
				this.m_itemStorage.maxRows = num;
				this.m_itemStorage.Restart();
				ui_presentbox_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_presentbox_scroll>(true);
				int num2 = componentsInChildren.Length;
				for (int i = 0; i < num; i++)
				{
					if (i < num2)
					{
						int index = i + this.m_currentPageNum * 10;
						componentsInChildren[i].UpdateView(this.m_presentInfoList[index]);
					}
				}
			}
		}
	}

	private void SetPresentBoxInfo()
	{
		if (this.m_presentInfoList != null)
		{
			this.m_presentInfoList.Clear();
			this.m_pageItemCountDic.Clear();
			if (ServerInterface.MessageList != null)
			{
				List<ServerMessageEntry> messageList = ServerInterface.MessageList;
				foreach (ServerMessageEntry current in messageList)
				{
					if (PresentBoxUtility.IsWithinTimeLimit(current.m_expireTiem))
					{
						PresentBoxUI.PresentInfo item = new PresentBoxUI.PresentInfo(current);
						this.m_presentInfoList.Add(item);
					}
				}
			}
			if (ServerInterface.OperatorMessageList != null)
			{
				List<ServerOperatorMessageEntry> operatorMessageList = ServerInterface.OperatorMessageList;
				foreach (ServerOperatorMessageEntry current2 in operatorMessageList)
				{
					if (PresentBoxUtility.IsWithinTimeLimit(current2.m_expireTiem))
					{
						PresentBoxUI.PresentInfo item2 = new PresentBoxUI.PresentInfo(current2);
						this.m_presentInfoList.Add(item2);
					}
				}
			}
			this.m_totalPageCount = this.m_presentInfoList.Count / 10;
			int num = this.m_presentInfoList.Count % 10;
			if (num > 0)
			{
				this.m_totalPageCount++;
			}
			for (int i = 0; i < this.m_totalPageCount; i++)
			{
				if (i == this.m_totalPageCount - 1)
				{
					if (num == 0)
					{
						this.m_pageItemCountDic.Add(i, 10);
					}
					else
					{
						this.m_pageItemCountDic.Add(i, num);
					}
				}
				else
				{
					this.m_pageItemCountDic.Add(i, 10);
				}
			}
		}
	}

	private void OnStartPresentBox()
	{
		this.m_outOfDataFlag = false;
		this.Initialize();
		this.SetPresentBoxInfo();
		this.UpdatePage();
		this.m_setUp = true;
	}

	private void OnEndPresentBox()
	{
		if (this.m_itemStorage != null)
		{
			this.m_itemStorage.maxItemCount = 0;
			this.m_itemStorage.maxRows = 0;
			this.m_itemStorage.Restart();
			this.m_presentInfoList.Clear();
			this.m_pageItemCountDic.Clear();
			GameObject gameObject = GameObject.Find("PresentBoxTextures");
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_setUp = false;
	}

	private void OnClickedRecieveAllBtn()
	{
		if (this.m_totalPageCount > 0)
		{
			int num = 0;
			if (ServerInterface.MessageList != null)
			{
				foreach (PresentBoxUI.PresentInfo current in this.m_presentInfoList)
				{
					if (PresentBoxUtility.IsWithinTimeLimit(current.expireTime))
					{
						num++;
					}
					else
					{
						this.m_outOfDataFlag = true;
					}
				}
			}
			if (num > 0)
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerUpdateMessage(null, null, base.gameObject);
				}
				BackKeyManager.InvalidFlag = true;
			}
			else if (this.m_outOfDataFlag)
			{
				base.StartCoroutine(this.ShowOnlyOutOfDataResult());
			}
			SoundManager.SePlay("sys_roulette_itemget", "SE");
		}
	}

	private void OnClickedRecieveSelectBtn()
	{
		if (this.m_totalPageCount > 0)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			if (this.m_itemStorage != null && list != null && list2 != null)
			{
				ui_presentbox_scroll[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_presentbox_scroll>(true);
				int num = componentsInChildren.Length;
				for (int i = 0; i < num; i++)
				{
					if (componentsInChildren[i].IsCheck())
					{
						int index = i + this.m_currentPageNum * 10;
						int messageId = this.m_presentInfoList[index].messageId;
						if (PresentBoxUtility.IsWithinTimeLimit(this.m_presentInfoList[index].expireTime))
						{
							if (this.m_presentInfoList[index].operatorFlag)
							{
								list2.Add(messageId);
							}
							else
							{
								list.Add(messageId);
							}
						}
						else
						{
							this.m_outOfDataFlag = true;
						}
					}
				}
				int num2 = list.Count + list2.Count;
				if (num2 > 0)
				{
					ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
					if (loggedInServerInterface != null)
					{
						loggedInServerInterface.RequestServerUpdateMessage(list, list2, base.gameObject);
					}
					BackKeyManager.InvalidFlag = true;
				}
				else if (this.m_outOfDataFlag)
				{
					base.StartCoroutine(this.ShowOnlyOutOfDataResult());
				}
			}
			SoundManager.SePlay("sys_roulette_itemget", "SE");
		}
	}

	private void OnClickedNextPageBtn()
	{
		SoundManager.SePlay("sys_page_skip", "SE");
		if (this.m_totalPageCount > 1)
		{
			this.m_currentPageNum++;
			if (this.m_totalPageCount == this.m_currentPageNum)
			{
				this.m_currentPageNum = 0;
			}
			this.UpdatePage();
		}
	}

	private void OnClickedPrevPageBtnBtn()
	{
		SoundManager.SePlay("sys_page_skip", "SE");
		if (this.m_totalPageCount > 1)
		{
			if (this.m_currentPageNum > 0)
			{
				this.m_currentPageNum--;
			}
			else
			{
				this.m_currentPageNum = this.m_totalPageCount - 1;
			}
			this.UpdatePage();
		}
	}

	private void ServerUpdateMessage_Succeeded(MsgUpdateMesseageSucceed msg)
	{
		BackKeyManager.InvalidFlag = false;
		base.StartCoroutine(this.ShowResult(msg));
		this.SetPresentBoxInfo();
		this.UpdatePage();
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void ServerUpdateMessage_Failed()
	{
		BackKeyManager.InvalidFlag = false;
	}

	public IEnumerator ShowOnlyOutOfDataResult()
	{
		PresentBoxUI._ShowOnlyOutOfDataResult_c__Iterator4F _ShowOnlyOutOfDataResult_c__Iterator4F = new PresentBoxUI._ShowOnlyOutOfDataResult_c__Iterator4F();
		_ShowOnlyOutOfDataResult_c__Iterator4F.__f__this = this;
		return _ShowOnlyOutOfDataResult_c__Iterator4F;
	}

	public IEnumerator ShowResult(MsgUpdateMesseageSucceed msg)
	{
		PresentBoxUI._ShowResult_c__Iterator50 _ShowResult_c__Iterator = new PresentBoxUI._ShowResult_c__Iterator50();
		_ShowResult_c__Iterator.msg = msg;
		_ShowResult_c__Iterator.___msg = msg;
		_ShowResult_c__Iterator.__f__this = this;
		return _ShowResult_c__Iterator;
	}
}
