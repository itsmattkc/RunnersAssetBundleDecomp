using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class DrillTrack : MonoBehaviour
{
	private class History
	{
		public Vector3 m_Position = Vector3.zero;

		public float m_UVOffset;

		public bool m_Visible = true;
	}

	private class StripMeshData
	{
		public Vector3[] m_Vertices;

		public Vector2[] m_UV;

		public Color[] m_Colors;

		public int[] m_Triangles;
	}

	private const float TRACK_RADIUS = 0.4f;

	private const float THRESHOLD = 0.3f;

	private const int HISTORY_SIZE = 100;

	public GameObject m_Target;

	public Transform m_Camera;

	private bool m_disable;

	private float m_frnotOffset;

	private CircularBuffer<DrillTrack.History> m_HistoryBuffer;

	private DrillTrack.StripMeshData m_MeshData;

	public bool Disable
	{
		get
		{
			return this.m_disable;
		}
		set
		{
			this.m_disable = value;
		}
	}

	public float FrontOffset
	{
		set
		{
			this.m_frnotOffset = value;
		}
	}

	private void Start()
	{
		this.m_HistoryBuffer = new CircularBuffer<DrillTrack.History>(100);
		this.m_MeshData = new DrillTrack.StripMeshData();
		this.m_MeshData.m_Vertices = new Vector3[400];
		this.m_MeshData.m_UV = new Vector2[400];
		this.m_MeshData.m_Colors = new Color[400];
		this.m_MeshData.m_Triangles = new int[600];
	}

	private void Update()
	{
		if (this.m_Target != null)
		{
			Vector3 vector = this.m_Target.transform.position;
			vector += this.m_Target.transform.forward * this.m_frnotOffset;
			this.AddHistory(vector);
		}
	}

	private void AddHistory(Vector3 position)
	{
		bool flag = false;
		if (this.m_HistoryBuffer.Size == 0)
		{
			flag = true;
		}
		else
		{
			DrillTrack.History at = this.m_HistoryBuffer.GetAt(this.m_HistoryBuffer.Tail);
			if (Vector3.Distance(position, at.m_Position) > 0.3f)
			{
				flag = true;
			}
		}
		if (flag)
		{
			DrillTrack.History history = new DrillTrack.History();
			history.m_Position = position;
			if (this.m_HistoryBuffer.Size > 0)
			{
				DrillTrack.History at2 = this.m_HistoryBuffer.GetAt(this.m_HistoryBuffer.Tail);
				float num = Vector3.Distance(position, at2.m_Position);
				history.m_UVOffset = at2.m_UVOffset + num / 0.8f;
			}
			if (this.m_disable)
			{
				history.m_Visible = false;
			}
			this.m_HistoryBuffer.Add(history);
			if (this.m_HistoryBuffer.Size > 1)
			{
				this.UpdateTrack();
			}
		}
	}

	private void UpdateTrack()
	{
		if (this.m_Camera == null)
		{
			return;
		}
		for (int i = 1; i < this.m_HistoryBuffer.Size; i++)
		{
			int index = (this.m_HistoryBuffer.Capacity + this.m_HistoryBuffer.Head + i - 1) % this.m_HistoryBuffer.Capacity;
			int index2 = (this.m_HistoryBuffer.Capacity + this.m_HistoryBuffer.Head + i) % this.m_HistoryBuffer.Capacity;
			DrillTrack.History at = this.m_HistoryBuffer.GetAt(index);
			DrillTrack.History at2 = this.m_HistoryBuffer.GetAt(index2);
			Vector3 position = at.m_Position;
			Vector3 position2 = at2.m_Position;
			Vector3 a = this.m_Camera.TransformDirection(Vector3.forward);
			Vector3 normalized = (position2 - position).normalized;
			Vector3 a2 = Vector3.Cross(normalized, -a);
			this.m_MeshData.m_Vertices[(i - 1) * 4] = position - a2 * 0.4f;
			this.m_MeshData.m_Vertices[(i - 1) * 4 + 1] = position + a2 * 0.4f;
			this.m_MeshData.m_Vertices[(i - 1) * 4 + 2] = position2 - a2 * 0.4f;
			this.m_MeshData.m_Vertices[(i - 1) * 4 + 3] = position2 + a2 * 0.4f;
			if (i > 1)
			{
				Vector3 vector = (this.m_MeshData.m_Vertices[(i - 2) * 4 + 2] + this.m_MeshData.m_Vertices[(i - 1) * 4]) / 2f;
				Vector3 vector2 = (this.m_MeshData.m_Vertices[(i - 2) * 4 + 3] + this.m_MeshData.m_Vertices[(i - 1) * 4 + 1]) / 2f;
				this.m_MeshData.m_Vertices[(i - 2) * 4 + 2] = vector;
				this.m_MeshData.m_Vertices[(i - 1) * 4] = vector;
				this.m_MeshData.m_Vertices[(i - 2) * 4 + 3] = vector2;
				this.m_MeshData.m_Vertices[(i - 1) * 4 + 1] = vector2;
			}
			this.m_MeshData.m_UV[(i - 1) * 4] = Vector2.right * at.m_UVOffset;
			this.m_MeshData.m_UV[(i - 1) * 4 + 1] = Vector2.up + Vector2.right * at.m_UVOffset;
			this.m_MeshData.m_UV[(i - 1) * 4 + 2] = Vector2.right * at2.m_UVOffset;
			this.m_MeshData.m_UV[(i - 1) * 4 + 3] = Vector2.up + Vector2.right * at2.m_UVOffset;
			this.m_MeshData.m_Colors[(i - 1) * 4] = ((!at.m_Visible) ? Color.clear : Color.white);
			this.m_MeshData.m_Colors[(i - 1) * 4 + 1] = ((!at.m_Visible) ? Color.clear : Color.white);
			this.m_MeshData.m_Colors[(i - 1) * 4 + 2] = ((!at2.m_Visible) ? Color.clear : Color.white);
			this.m_MeshData.m_Colors[(i - 1) * 4 + 3] = ((!at2.m_Visible) ? Color.clear : Color.white);
			this.m_MeshData.m_Triangles[(i - 1) * 6] = 0 + (i - 1) * 4;
			this.m_MeshData.m_Triangles[(i - 1) * 6 + 1] = 1 + (i - 1) * 4;
			this.m_MeshData.m_Triangles[(i - 1) * 6 + 2] = 2 + (i - 1) * 4;
			this.m_MeshData.m_Triangles[(i - 1) * 6 + 3] = 2 + (i - 1) * 4;
			this.m_MeshData.m_Triangles[(i - 1) * 6 + 4] = 1 + (i - 1) * 4;
			this.m_MeshData.m_Triangles[(i - 1) * 6 + 5] = 3 + (i - 1) * 4;
		}
		Mesh mesh = base.GetComponent<MeshFilter>().mesh;
		mesh.vertices = this.m_MeshData.m_Vertices;
		mesh.uv = this.m_MeshData.m_UV;
		mesh.colors = this.m_MeshData.m_Colors;
		mesh.triangles = this.m_MeshData.m_Triangles;
		mesh.RecalculateBounds();
	}
}
