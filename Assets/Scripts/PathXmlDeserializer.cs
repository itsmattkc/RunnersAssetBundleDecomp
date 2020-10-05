using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class PathXmlDeserializer
{
	private sealed class _CreatePathObjectData_c__Iterator1C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal XmlDocument _doc___0;

		internal TextAsset xmlData;

		internal XmlNodeList _pathObjDataNode___1;

		internal IEnumerator __s_315___2;

		internal XmlNode _rootNode___3;

		internal ResPathObjectData _pathObj___4;

		internal int _indexAt___5;

		internal int _value___6;

		internal int _value___7;

		internal XmlNode _distance___8;

		internal XmlNode _position___9;

		internal XmlNode _normal___10;

		internal XmlNode _tangent___11;

		internal XmlNode _vertNode___12;

		internal Vector3 _vec___13;

		internal XmlNode _minNode___14;

		internal XmlNode _item___15;

		internal XmlNode _maxNode___16;

		internal Dictionary<string, ResPathObjectData> dictonary;

		internal int _PC;

		internal object _current;

		internal TextAsset ___xmlData;

		internal Dictionary<string, ResPathObjectData> ___dictonary;

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
				this._doc___0 = new XmlDocument();
				this._doc___0.LoadXml(this.xmlData.text);
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._pathObjDataNode___1 = this._doc___0.GetElementsByTagName("ResPathObjectData");
				this.__s_315___2 = this._pathObjDataNode___1.GetEnumerator();
				try
				{
					while (this.__s_315___2.MoveNext())
					{
						this._rootNode___3 = (XmlNode)this.__s_315___2.Current;
						this._pathObj___4 = new ResPathObjectData();
						this._pathObj___4.name = this._rootNode___3.Attributes["Name"].Value;
						this._indexAt___5 = this._pathObj___4.name.IndexOf("@");
						if (this._indexAt___5 >= 0)
						{
							this._pathObj___4.name = this._pathObj___4.name.Substring(0, this._indexAt___5);
						}
						this._pathObj___4.name = this._pathObj___4.name.ToLower();
						this._value___6 = int.Parse(this._rootNode___3.Attributes["PlaybackType"].Value);
						this._pathObj___4.playbackType = (byte)this._value___6;
						this._value___7 = int.Parse(this._rootNode___3.Attributes["Flags"].Value);
						this._pathObj___4.flags = (byte)this._value___7;
						this._pathObj___4.numKeys = ushort.Parse(this._rootNode___3.Attributes["NumKeys"].Value);
						this._pathObj___4.length = float.Parse(this._rootNode___3.Attributes["Length"].Value);
						this._pathObj___4.distance = new float[(int)this._pathObj___4.numKeys];
						this._pathObj___4.position = new Vector3[(int)this._pathObj___4.numKeys];
						this._pathObj___4.normal = new Vector3[(int)this._pathObj___4.numKeys];
						this._pathObj___4.tangent = new Vector3[(int)this._pathObj___4.numKeys];
						this._distance___8 = this._rootNode___3.SelectSingleNode("Distance");
						PathXmlDeserializer.ParseFloatArray(this._distance___8, this._pathObj___4.distance, (int)this._pathObj___4.numKeys);
						this._position___9 = this._rootNode___3.SelectSingleNode("Position");
						PathXmlDeserializer.ParseVector3Array(this._position___9, this._pathObj___4.position, (int)this._pathObj___4.numKeys);
						this._normal___10 = this._rootNode___3.SelectSingleNode("Normal");
						PathXmlDeserializer.ParseVector3Array(this._normal___10, this._pathObj___4.normal, (int)this._pathObj___4.numKeys);
						this._tangent___11 = this._rootNode___3.SelectSingleNode("Tangent");
						PathXmlDeserializer.ParseVector3Array(this._tangent___11, this._pathObj___4.tangent, (int)this._pathObj___4.numKeys);
						this._vertNode___12 = this._rootNode___3.SelectSingleNode("Vertices");
						this._pathObj___4.numVertices = uint.Parse(this._vertNode___12.Attributes["value"].Value);
						if (this._pathObj___4.numVertices > 0u)
						{
							this._pathObj___4.vertices = new Vector3[this._pathObj___4.numVertices];
							PathXmlDeserializer.ParseVector3Array(this._vertNode___12, this._pathObj___4.vertices, (int)this._pathObj___4.numVertices);
						}
						this._vec___13 = default(Vector3);
						this._minNode___14 = this._rootNode___3.SelectSingleNode("Min");
						this._item___15 = this._minNode___14.SelectSingleNode("item");
						PathXmlDeserializer.ParseVector3(this._item___15, ref this._vec___13);
						this._pathObj___4.min = this._vec___13;
						this._vec___13 = default(Vector3);
						this._maxNode___16 = this._rootNode___3.SelectSingleNode("Max");
						this._item___15 = this._maxNode___16.SelectSingleNode("item");
						PathXmlDeserializer.ParseVector3(this._item___15, ref this._vec___13);
						this._pathObj___4.max = this._vec___13;
						this._pathObj___4.uid = (ulong)uint.Parse(this._rootNode___3.Attributes["UniqueID"].Value);
						this.dictonary.Add(this._pathObj___4.name, this._pathObj___4);
					}
				}
				finally
				{
					IDisposable disposable = this.__s_315___2 as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				this._current = null;
				this._PC = 2;
				return true;
			case 2u:
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

	public static IEnumerator CreatePathObjectData(TextAsset xmlData, Dictionary<string, ResPathObjectData> dictonary)
	{
		PathXmlDeserializer._CreatePathObjectData_c__Iterator1C _CreatePathObjectData_c__Iterator1C = new PathXmlDeserializer._CreatePathObjectData_c__Iterator1C();
		_CreatePathObjectData_c__Iterator1C.xmlData = xmlData;
		_CreatePathObjectData_c__Iterator1C.dictonary = dictonary;
		_CreatePathObjectData_c__Iterator1C.___xmlData = xmlData;
		_CreatePathObjectData_c__Iterator1C.___dictonary = dictonary;
		return _CreatePathObjectData_c__Iterator1C;
	}

	private static void ParseFloatArray(XmlNode node, float[] array, int numOfArray)
	{
		if (node != null)
		{
			XmlNodeList xmlNodeList = node.SelectNodes("item");
			if (xmlNodeList.Count == numOfArray && xmlNodeList.Count > 0)
			{
				int num = 0;
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					float.TryParse(xmlNode.InnerText, out array[num]);
					num++;
				}
			}
			else
			{
				global::Debug.Log("Array Num is not Equal keyNum");
			}
		}
	}

	private static void ParseVector3Array(XmlNode node, Vector3[] array, int numOfArray)
	{
		if (node != null)
		{
			XmlNodeList xmlNodeList = node.SelectNodes("item");
			if (xmlNodeList.Count == numOfArray && xmlNodeList.Count > 0)
			{
				int num = 0;
				foreach (XmlNode node2 in xmlNodeList)
				{
					PathXmlDeserializer.ParseVector3(node2, ref array[num]);
					num++;
				}
			}
			else
			{
				global::Debug.Log("Array Num is not Equal keyNum");
			}
		}
	}

	private static void ParseVector3(XmlNode node, ref Vector3 vec)
	{
		if (node != null)
		{
			float.TryParse(node.Attributes["X"].Value, out vec.x);
			float.TryParse(node.Attributes["Y"].Value, out vec.y);
			float.TryParse(node.Attributes["Z"].Value, out vec.z);
		}
	}
}
