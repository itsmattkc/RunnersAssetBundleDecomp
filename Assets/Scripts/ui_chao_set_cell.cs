using DataTable;
using System;
using Text;
using UnityEngine;

public class ui_chao_set_cell : MonoBehaviour
{
	private const float IMAGE_LOAD_DELAY = 0.02f;

	private static float s_lastLoadTime = -1f;

	private static bool s_loadLock = true;

	[SerializeField]
	private UISprite m_setSprite;

	[SerializeField]
	private UISprite m_chaoRankSprite;

	[SerializeField]
	private UILabel m_chaoLevelLabel;

	[SerializeField]
	private UISprite m_chaoTypeSprite;

	[SerializeField]
	private UISprite m_bonusTypeSprite;

	[SerializeField]
	private UILabel m_bonusLabel;

	[SerializeField]
	private GameObject m_disabledSprite;

	[SerializeField]
	private UIButtonScale m_buttonScale;

	private ChaoData m_chaoData;

	private bool m_isLoad;

	private bool m_isLoadCmp;

	private bool m_isDraw;

	private bool m_isDrawChao;

	private float m_loadingTime;

	private float m_drawDelay = -1f;

	private float m_checkDelay;

	private int m_chaoId = -1;

	private UITexture m_chaoTex;

	private Texture m_chaoTextureData;

	private UISprite m_chaoDefault;

	private UIPanel m_parentPanel;

	public static void ResetLastLoadTime()
	{
		ui_chao_set_cell.s_lastLoadTime = 0f;
		ui_chao_set_cell.s_loadLock = true;
	}

	private void Start()
	{
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.transform.root.gameObject, "chao_set_window");
		UIPlayAnimation component = base.gameObject.GetComponent<UIPlayAnimation>();
		if (animation != null && component != null)
		{
			component.target = animation;
		}
	}

	private void Update()
	{
		if (this.m_chaoId >= 0 && !ui_chao_set_cell.s_loadLock)
		{
			if (!this.m_isLoadCmp)
			{
				if (this.m_chaoDefault != null && !this.m_chaoDefault.gameObject.activeSelf && this.m_chaoTex != null && this.m_chaoTex.alpha <= 0f)
				{
					this.m_chaoDefault.gameObject.SetActive(true);
				}
				if (this.m_isLoad)
				{
					this.m_loadingTime += Time.deltaTime;
					if (this.m_loadingTime > 1f)
					{
						ChaoTextureManager.CallbackInfo.LoadFinishCallback callback = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoLoadFinishCallback);
						ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(null, callback, false);
						ChaoTextureManager.Instance.GetTexture(this.m_chaoId, info);
						this.m_loadingTime = 0.001f;
					}
				}
			}
			if (!this.m_isDrawChao)
			{
				if (this.m_isLoad && this.m_chaoTextureData != null)
				{
					if (this.IsDisp())
					{
						this.m_drawDelay = 0.02f;
					}
					else
					{
						this.m_drawDelay = 0.04f;
					}
					this.m_isDrawChao = true;
					if (this.m_chaoDefault != null && !this.m_chaoDefault.gameObject.activeSelf)
					{
						this.m_chaoDefault.gameObject.SetActive(true);
					}
				}
				else if (!this.m_isLoad && this.IsDraw())
				{
					float num = 0.05f;
					if (!this.IsDisp())
					{
						num = 0.3f;
					}
					if (ui_chao_set_cell.s_lastLoadTime <= 0f)
					{
						if (ui_chao_set_cell.s_lastLoadTime < 0f)
						{
							ui_chao_set_cell.s_lastLoadTime = Time.realtimeSinceStartup + 1f;
						}
						else
						{
							ui_chao_set_cell.s_lastLoadTime = Time.realtimeSinceStartup + 0.5f;
						}
					}
					else if (ui_chao_set_cell.s_lastLoadTime + num < Time.realtimeSinceStartup)
					{
						ChaoTextureManager.CallbackInfo.LoadFinishCallback callback2 = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoLoadFinishCallback);
						ChaoTextureManager.CallbackInfo info2 = new ChaoTextureManager.CallbackInfo(null, callback2, false);
						ChaoTextureManager.Instance.GetTexture(this.m_chaoId, info2);
						this.m_loadingTime = 0f;
						this.m_isLoad = true;
						this.m_isLoadCmp = false;
						ui_chao_set_cell.s_lastLoadTime = Time.realtimeSinceStartup;
					}
				}
			}
			else if (this.m_drawDelay > 0f)
			{
				this.m_drawDelay -= Time.deltaTime;
				if (this.m_drawDelay <= 0f)
				{
					if (GeneralUtil.CheckChaoTexture(this.m_chaoTextureData, this.m_chaoData.id))
					{
						this.m_drawDelay = -1f;
						this.m_loadingTime = -1f;
						this.m_isLoadCmp = true;
						this.m_checkDelay = 1f;
						this.m_chaoTex.mainTexture = this.m_chaoTextureData;
					}
					else
					{
						ChaoTextureManager.CallbackInfo.LoadFinishCallback callback3 = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoReloadFinishCallback);
						ChaoTextureManager.CallbackInfo info3 = new ChaoTextureManager.CallbackInfo(null, callback3, false);
						ChaoTextureManager.Instance.GetTexture(this.m_chaoData.id, info3);
					}
				}
			}
		}
		if (this.m_isLoadCmp && this.m_checkDelay > 0f)
		{
			this.m_checkDelay -= Time.deltaTime;
			if (this.m_checkDelay <= 0f)
			{
				this.m_checkDelay = 0f;
				if (!GeneralUtil.CheckChaoTexture(this.m_chaoTex, this.m_chaoData.id))
				{
					ChaoTextureManager.CallbackInfo.LoadFinishCallback callback4 = new ChaoTextureManager.CallbackInfo.LoadFinishCallback(this.ChaoReloadFinishCallback);
					ChaoTextureManager.CallbackInfo info4 = new ChaoTextureManager.CallbackInfo(null, callback4, false);
					ChaoTextureManager.Instance.GetTexture(this.m_chaoData.id, info4);
				}
				else
				{
					this.m_chaoTex.alpha = 1f;
					if (this.m_chaoDefault != null)
					{
						this.m_chaoDefault.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private bool IsDraw()
	{
		bool result = false;
		if (this.m_parentPanel != null)
		{
			if (!this.m_isDraw)
			{
				float num = this.m_parentPanel.transform.localPosition.y * -1f;
				float w = this.m_parentPanel.clipRange.w;
				float num2 = num - w;
				float y = base.gameObject.transform.localPosition.y;
				if (y > num2)
				{
					this.m_isDraw = true;
				}
			}
			result = this.m_isDraw;
		}
		return result;
	}

	private bool IsDisp()
	{
		bool result = false;
		if (this.m_parentPanel != null && this.m_isDraw)
		{
			float num = this.m_parentPanel.transform.localPosition.y * -1f;
			float num2 = this.m_parentPanel.clipRange.w * 1.2f;
			float num3 = num - num2;
			float y = base.gameObject.transform.localPosition.y;
			if (y > num3 && y < num3 + num2)
			{
				result = true;
			}
		}
		return result;
	}

	private void UpdateView(ChaoData chaoData, int mainChaoId, int subChaoId, UIPanel parentPanel)
	{
		ui_chao_set_cell.s_loadLock = false;
		this.m_isDraw = false;
		this.m_drawDelay = -1f;
		this.m_isDrawChao = false;
		this.m_chaoId = -1;
		this.m_loadingTime = 0f;
		this.m_isLoad = false;
		this.m_isLoadCmp = false;
		this.m_chaoTextureData = null;
		this.m_parentPanel = parentPanel;
		if (chaoData != null)
		{
			this.m_chaoId = chaoData.id;
		}
		ChaoTextureManager instance = ChaoTextureManager.Instance;
		Texture texture = null;
		if (instance != null)
		{
			texture = instance.GetLoadedTexture(chaoData.id);
		}
		if (this.m_chaoTex == null)
		{
			this.m_chaoTex = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_chao_tex");
		}
		this.m_chaoDefault = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_chao_default");
		if (this.m_chaoDefault != null)
		{
			this.m_chaoDefault.gameObject.SetActive(true);
		}
		if (texture != null)
		{
			this.m_drawDelay = 0.005f;
			this.m_chaoTextureData = texture;
			this.m_isLoad = true;
			this.m_isDraw = true;
			this.m_isDrawChao = true;
		}
		if (chaoData != null)
		{
			this.m_chaoRankSprite.spriteName = "ui_chao_set_bg_s_" + (int)chaoData.rarity;
		}
		if (chaoData != null && chaoData.IsValidate)
		{
			this.m_chaoData = chaoData;
			this.m_setSprite.spriteName = ((chaoData.id != mainChaoId) ? ((chaoData.id != subChaoId) ? null : "ui_chao_set_cursor_1") : "ui_chao_set_cursor_0");
			this.m_chaoTex.color = Color.white;
			this.m_chaoDefault.color = Color.white;
			this.m_chaoLevelLabel.text = TextUtility.GetTextLevel(chaoData.level.ToString());
			string str = this.m_chaoData.charaAtribute.ToString().ToLower();
			this.m_chaoTypeSprite.spriteName = "ui_chao_set_type_icon_" + str;
			if (this.m_bonusTypeSprite != null)
			{
				this.m_bonusTypeSprite.gameObject.SetActive(false);
			}
			this.m_bonusLabel.enabled = false;
			this.m_disabledSprite.SetActive(false);
			this.m_buttonScale.enabled = true;
		}
		else if (chaoData != null && !chaoData.IsValidate)
		{
			this.m_chaoData = chaoData;
			this.m_setSprite.spriteName = null;
			this.m_chaoTex.color = Color.black;
			this.m_chaoDefault.color = Color.black;
			this.m_chaoLevelLabel.text = string.Empty;
			string str2 = this.m_chaoData.charaAtribute.ToString().ToLower();
			this.m_chaoTypeSprite.spriteName = "ui_chao_set_type_icon_" + str2;
			if (this.m_bonusTypeSprite != null)
			{
				this.m_bonusTypeSprite.gameObject.SetActive(false);
			}
			this.m_bonusLabel.enabled = false;
			this.m_disabledSprite.SetActive(true);
			this.m_buttonScale.enabled = true;
		}
		else
		{
			this.m_chaoData = null;
			this.m_setSprite.spriteName = null;
			this.m_chaoTex.color = Color.black;
			this.m_chaoDefault.color = Color.black;
			this.m_chaoLevelLabel.text = string.Empty;
			this.m_chaoTypeSprite.spriteName = null;
			this.m_bonusTypeSprite.spriteName = null;
			this.m_bonusLabel.text = string.Empty;
			this.m_disabledSprite.SetActive(true);
			this.m_buttonScale.enabled = false;
		}
		this.m_chaoTex.alpha = 0.001f;
	}

	private void ChaoLoadFinishCallback(Texture tex)
	{
		this.m_chaoTextureData = tex;
	}

	private void ChaoReloadFinishCallback(Texture tex)
	{
		this.m_chaoTextureData = tex;
		this.m_chaoTex.mainTexture = this.m_chaoTextureData;
		this.m_drawDelay = -1f;
		this.m_loadingTime = -1f;
		this.m_isLoadCmp = true;
		this.m_checkDelay = 1f;
	}

	public void UpdateView(int id, int mainChaoId, int subChaoId, UIPanel parentPanel)
	{
		this.UpdateView(ChaoTable.GetChaoData(id), mainChaoId, subChaoId, parentPanel);
	}

	private void OnClick()
	{
		if (this.m_chaoData == null)
		{
			return;
		}
		ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
		if (window != null)
		{
			ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(this.m_chaoData);
			if (!this.m_chaoData.IsValidate)
			{
				chaoInfo.level = 0;
				chaoInfo.detail = this.m_chaoData.GetDetailLevelPlusSP(0);
				chaoInfo.name = TextManager.GetText(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON, "Name", "name_question").text;
				chaoInfo.onMask = true;
				window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
			}
			else
			{
				window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WITH_BUTTON);
			}
		}
	}
}
