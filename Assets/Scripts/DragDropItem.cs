using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item")]
public class DragDropItem : MonoBehaviour
{
	public GameObject prefab;

	private Transform mTrans;

	private bool mPressed;

	private int mTouchID;

	private bool mIsDragging;

	private bool mSticky;

	private Transform mParent;

	private UIRoot mRoot;

	private void UpdateTable()
	{
		UITable uITable = NGUITools.FindInParents<UITable>(base.gameObject);
		if (uITable != null)
		{
			uITable.repositionNow = true;
		}
	}

	private void Drop()
	{
		Collider collider = UICamera.lastHit.collider;
		DragDropContainer dragDropContainer = (!(collider != null)) ? null : collider.gameObject.GetComponent<DragDropContainer>();
		if (dragDropContainer != null)
		{
			this.mTrans.parent = dragDropContainer.transform;
			Vector3 localPosition = this.mTrans.localPosition;
			localPosition.z = 0f;
			this.mTrans.localPosition = localPosition;
		}
		else
		{
			this.mTrans.parent = this.mParent;
		}
		UIWidget[] componentsInChildren = base.GetComponentsInChildren<UIWidget>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].depth = componentsInChildren[i].depth - 100;
		}
		this.UpdateTable();
		NGUITools.MarkParentAsChanged(base.gameObject);
	}

	private void Awake()
	{
		this.mTrans = base.transform;
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && UICamera.currentTouchID == this.mTouchID && base.enabled)
		{
			if (!this.mIsDragging)
			{
				this.mIsDragging = true;
				this.mParent = this.mTrans.parent;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.mTrans.gameObject);
				if (DragDropRoot.root != null)
				{
					this.mTrans.parent = DragDropRoot.root;
				}
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.z = 0f;
				this.mTrans.localPosition = localPosition;
				UIWidget[] componentsInChildren = base.GetComponentsInChildren<UIWidget>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].depth = componentsInChildren[i].depth + 100;
				}
				NGUITools.MarkParentAsChanged(base.gameObject);
			}
			else
			{
				this.mTrans.localPosition += delta * this.mRoot.pixelSizeAdjustment;
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (isPressed)
			{
				if (this.mPressed)
				{
					return;
				}
				this.mPressed = true;
				this.mTouchID = UICamera.currentTouchID;
				if (!UICamera.current.stickyPress)
				{
					this.mSticky = true;
					UICamera.current.stickyPress = true;
				}
			}
			else
			{
				this.mPressed = false;
				if (this.mSticky)
				{
					this.mSticky = false;
					UICamera.current.stickyPress = false;
				}
			}
			this.mIsDragging = false;
			Collider collider = GetComponent<Collider>();
			if (collider != null)
			{
				collider.enabled = !isPressed;
			}
			if (!isPressed)
			{
				this.Drop();
			}
		}
	}
}
