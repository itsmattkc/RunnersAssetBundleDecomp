using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Panel Contents"), ExecuteInEditMode]
public class UIDragPanelContents : MonoBehaviour
{
	public UIDraggablePanel draggablePanel;

	private void Start()
	{
		if (this.draggablePanel == null)
		{
			this.draggablePanel = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggablePanel != null)
		{
			this.draggablePanel.Press(pressed);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggablePanel != null)
		{
			this.draggablePanel.Drag();
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggablePanel != null)
		{
			this.draggablePanel.Scroll(delta);
		}
	}
}
