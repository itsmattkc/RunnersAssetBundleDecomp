using System;
using UnityEngine;

public class ui_ranking_reward : MonoBehaviour
{
	public const int ADD_HEGHT = 48;

	public const int INIT_LINE = 3;

	public const float OPEN_SPEED = 0.5f;

	public const float CLOSE_SPEED = 0.25f;

	[SerializeField]
	private UISprite m_bg;

	[SerializeField]
	private BoxCollider m_collider;

	[SerializeField]
	private UILabel m_label;

	[SerializeField]
	private UISprite m_icon;

	[SerializeField]
	private UIDragPanelContents m_dragPanelContents;

	private float m_move;

	private UIDraggablePanel m_parent;

	private UITable m_table;

	private void Start()
	{
		base.enabled = false;
	}

	private void Update()
	{
		if (this.m_parent == null)
		{
			this.m_parent = base.gameObject.transform.parent.GetComponent<UIDraggablePanel>();
			if (this.m_parent == null)
			{
				this.m_parent = base.gameObject.transform.parent.transform.parent.GetComponent<UIDraggablePanel>();
				this.m_dragPanelContents.draggablePanel = this.m_parent;
			}
			else
			{
				this.m_dragPanelContents.draggablePanel = this.m_parent;
			}
		}
		if (this.m_table == null)
		{
			this.m_table = base.gameObject.transform.parent.GetComponent<UITable>();
			if (this.m_table == null)
			{
				this.m_table = base.gameObject.transform.parent.transform.parent.GetComponent<UITable>();
			}
		}
		if (this.m_move > 0f)
		{
			this.m_move -= Time.deltaTime;
			if (this.m_move <= 0f)
			{
				if (this.m_table != null)
				{
					this.m_table.repositionNow = true;
				}
				this.m_move = 0f;
			}
		}
		if (this.m_collider.size.y - 48f != (float)this.m_label.height)
		{
			float num = (float)this.m_label.height * 1.2f + 48f;
			this.m_collider.size = new Vector3(this.m_collider.size.x, num, this.m_collider.size.z);
			this.m_bg.height = (int)num;
		}
	}

	private void OnClickBg()
	{
		global::Debug.Log("OnClickBg m_icon:" + (this.m_icon != null));
	}
}
