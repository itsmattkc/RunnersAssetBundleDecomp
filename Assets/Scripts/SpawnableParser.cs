using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class SpawnableParser : MonoBehaviour
{
	public enum Object
	{
		Object,
		ID,
		ClassName,
		ParameterName,
		RangeIn,
		RangeOut,
		Position,
		Angle,
		NUM
	}

	public enum Parameter
	{
		Parameters,
		Item,
		Name,
		Type,
		Value,
		NUM
	}

	private sealed class _CreateSetData_c__Iterator18 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal TextAsset xmlFile;

		internal string _text_data___0;

		internal XmlReader _reader___1;

		internal int _blockID___2;

		internal ResourceManager resManager;

		internal StageSpawnableParameterContainer stageDataList;

		internal int _PC;

		internal object _current;

		internal TextAsset ___xmlFile;

		internal ResourceManager ___resManager;

		internal StageSpawnableParameterContainer ___stageDataList;

		internal SpawnableParser __f__this;

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
				TimeProfiler.StartCountTime("CreateSetData:CreateSetData");
				if (!(this.xmlFile != null))
				{
					goto IL_16A;
				}
				TimeProfiler.StartCountTime("CreateSetData:AESCrypt.Decrypt");
				this._text_data___0 = AESCrypt.Decrypt(this.xmlFile.text);
				TimeProfiler.EndCountTime("CreateSetData:AESCrypt.Decrypt");
				this._reader___1 = XmlReader.Create(new StringReader(this._text_data___0));
				if (this._reader___1 == null)
				{
					goto IL_16A;
				}
				this._reader___1.Read();
				if (!this._reader___1.ReadToFollowing("Stage") || !this._reader___1.ReadToDescendant("Block"))
				{
					goto IL_15F;
				}
				break;
			case 1u:
				IL_14A:
				if (!this._reader___1.ReadToNextSibling("Block"))
				{
					goto IL_15F;
				}
				break;
			default:
				return false;
			}
			if (!this._reader___1.MoveToAttribute("ID"))
			{
				goto IL_14A;
			}
			this._blockID___2 = int.Parse(this._reader___1.Value);
			this._reader___1.MoveToElement();
			if (this._reader___1.ReadToDescendant("Layer"))
			{
				this._current = this.__f__this.StartCoroutine(this.__f__this.ReadLayer(this.resManager, this._reader___1, this._blockID___2, this.stageDataList));
				this._PC = 1;
				return true;
			}
			goto IL_14A;
			IL_15F:
			this._reader___1.Close();
			IL_16A:
			TimeProfiler.EndCountTime("CreateSetData:CreateSetData");
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

	private sealed class _ReadLayer_c__Iterator19 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal XmlReader reader;

		internal int _layerID___0;

		internal int blockID;

		internal BlockSpawnableParameterContainer _blockData___1;

		internal ResourceManager resManager;

		internal StageSpawnableParameterContainer stageDataList;

		internal int _PC;

		internal object _current;

		internal XmlReader ___reader;

		internal int ___blockID;

		internal ResourceManager ___resManager;

		internal StageSpawnableParameterContainer ___stageDataList;

		internal SpawnableParser __f__this;

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
				break;
			case 1u:
				if (!this.reader.ReadToNextSibling("Layer"))
				{
					this._PC = -1;
					return false;
				}
				break;
			default:
				return false;
			}
			if (this.reader.MoveToAttribute("ID"))
			{
				this._layerID___0 = int.Parse(this.reader.Value);
				this.reader.MoveToElement();
				if (this.reader.ReadToDescendant("Obj"))
				{
					this._blockData___1 = new BlockSpawnableParameterContainer(this.blockID, this._layerID___0);
					this.__f__this.ReadObjects(this.resManager, this.reader, this._blockData___1);
					this.stageDataList.AddData(this.blockID, this._layerID___0, this._blockData___1);
				}
			}
			this._current = null;
			this._PC = 1;
			return true;
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

	public static readonly string[] ObjectKeyTable = new string[]
	{
		"Obj",
		"ID",
		"C",
		"P",
		"I",
		"O",
		"P",
		"A"
	};

	public static readonly string[] ParameterKeyTable = new string[]
	{
		"PS",
		"It",
		"N",
		"T",
		"V"
	};

	private static Dictionary<string, int> __f__switch_map1;

	private static Dictionary<string, int> __f__switch_map2;

	private static Dictionary<string, int> __f__switch_map3;

	private static Dictionary<string, int> __f__switch_map4;

	private static Dictionary<string, int> __f__switch_map5;

	public IEnumerator CreateSetData(ResourceManager resManager, TextAsset xmlFile, StageSpawnableParameterContainer stageDataList)
	{
		SpawnableParser._CreateSetData_c__Iterator18 _CreateSetData_c__Iterator = new SpawnableParser._CreateSetData_c__Iterator18();
		_CreateSetData_c__Iterator.xmlFile = xmlFile;
		_CreateSetData_c__Iterator.resManager = resManager;
		_CreateSetData_c__Iterator.stageDataList = stageDataList;
		_CreateSetData_c__Iterator.___xmlFile = xmlFile;
		_CreateSetData_c__Iterator.___resManager = resManager;
		_CreateSetData_c__Iterator.___stageDataList = stageDataList;
		_CreateSetData_c__Iterator.__f__this = this;
		return _CreateSetData_c__Iterator;
	}

	private static Vector3 ReadVector3(XmlReader reader)
	{
		Vector3 zero = Vector3.zero;
		reader.MoveToFirstAttribute();
		do
		{
			string name = reader.Name;
			switch (name)
			{
			case "X":
				zero.x = float.Parse(reader.Value);
				break;
			case "Y":
				zero.y = float.Parse(reader.Value);
				break;
			case "Z":
				zero.z = float.Parse(reader.Value);
				break;
			}
		}
		while (reader.MoveToNextAttribute());
		reader.MoveToElement();
		return zero;
	}

	private static void ReadUserParameters(ResourceManager resManager, XmlReader reader, object o, Type t, string parameterName)
	{
		if (!(o is SpawnableParameter))
		{
			return;
		}
		if (reader.ReadToDescendant("It"))
		{
			do
			{
				reader.MoveToFirstAttribute();
				string name = null;
				string text = null;
				string text2 = null;
				do
				{
					string text3 = reader.Name;
					switch (text3)
					{
					case "N":
						name = reader.Value;
						break;
					case "V":
						text = reader.Value;
						break;
					case "T":
						text2 = reader.Value;
						break;
					}
				}
				while (reader.MoveToNextAttribute());
				reader.MoveToElement();
				FieldInfo field = t.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					string text3 = text2;
					switch (text3)
					{
					case "g":
					{
						GameObject spawnableGameObject = resManager.GetSpawnableGameObject(text);
						if (spawnableGameObject != null)
						{
							field.SetValue(o, spawnableGameObject);
						}
						break;
					}
					case "f":
					{
						float num2;
						if (float.TryParse(text, out num2))
						{
							field.SetValue(o, num2);
						}
						break;
					}
					case "i":
					{
						int num3;
						if (int.TryParse(text, out num3))
						{
							field.SetValue(o, num3);
						}
						break;
					}
					case "u":
					{
						uint num4;
						if (uint.TryParse(text, out num4))
						{
							num4 = uint.Parse(text, NumberStyles.AllowHexSpecifier);
							field.SetValue(o, num4);
						}
						break;
					}
					case "s":
					{
						string text4 = text;
						if (text4 != null)
						{
							field.SetValue(o, text4);
						}
						break;
					}
					}
				}
			}
			while (reader.ReadToNextSibling("It"));
		}
	}

	private void ReadObjects(ResourceManager resManager, XmlReader reader, BlockSpawnableParameterContainer blockData)
	{
		do
		{
			string text = null;
			uint iD = 0u;
			string text2 = null;
			int num = 20;
			int num2 = 30;
			reader.MoveToFirstAttribute();
			do
			{
				string name = reader.Name;
				switch (name)
				{
				case "C":
					text = reader.Value;
					break;
				case "ID":
					iD = uint.Parse(reader.Value, NumberStyles.AllowHexSpecifier);
					break;
				case "P":
					text2 = reader.Value;
					break;
				case "I":
					num = int.Parse(reader.Value);
					break;
				case "O":
					num2 = int.Parse(reader.Value);
					break;
				}
			}
			while (reader.MoveToNextAttribute());
			reader.MoveToElement();
			if (!string.IsNullOrEmpty(text))
			{
				Type type = null;
				object obj = null;
				SpawnableParameter spawnableParameter = null;
				if (!string.IsNullOrEmpty(text2))
				{
					type = Type.GetType(text2);
					if (type != null)
					{
						obj = Activator.CreateInstance(type);
						spawnableParameter = (obj as SpawnableParameter);
					}
					if (spawnableParameter == null)
					{
						type = null;
						obj = null;
						spawnableParameter = new SpawnableParameter();
						text2 = null;
					}
				}
				else
				{
					spawnableParameter = new SpawnableParameter();
				}
				spawnableParameter.ObjectName = text;
				spawnableParameter.ID = iD;
				spawnableParameter.RangeIn = (float)num;
				spawnableParameter.RangeOut = (float)num2;
				XmlReader xmlReader = reader.ReadSubtree();
				while (xmlReader.Read())
				{
					string name = xmlReader.Name;
					switch (name)
					{
					case "P":
						spawnableParameter.Position = SpawnableParser.ReadVector3(xmlReader);
						break;
					case "A":
					{
						Vector3 euler = SpawnableParser.ReadVector3(xmlReader);
						spawnableParameter.Rotation = Quaternion.Euler(euler);
						break;
					}
					case "PS":
						if (!string.IsNullOrEmpty(text2))
						{
							SpawnableParser.ReadUserParameters(resManager, xmlReader, obj, type, text2);
						}
						else
						{
							xmlReader.Skip();
						}
						break;
					}
				}
				xmlReader.Close();
				blockData.AddParameter(spawnableParameter);
			}
		}
		while (reader.ReadToNextSibling("Obj"));
	}

	private IEnumerator ReadLayer(ResourceManager resManager, XmlReader reader, int blockID, StageSpawnableParameterContainer stageDataList)
	{
		SpawnableParser._ReadLayer_c__Iterator19 _ReadLayer_c__Iterator = new SpawnableParser._ReadLayer_c__Iterator19();
		_ReadLayer_c__Iterator.reader = reader;
		_ReadLayer_c__Iterator.blockID = blockID;
		_ReadLayer_c__Iterator.resManager = resManager;
		_ReadLayer_c__Iterator.stageDataList = stageDataList;
		_ReadLayer_c__Iterator.___reader = reader;
		_ReadLayer_c__Iterator.___blockID = blockID;
		_ReadLayer_c__Iterator.___resManager = resManager;
		_ReadLayer_c__Iterator.___stageDataList = stageDataList;
		_ReadLayer_c__Iterator.__f__this = this;
		return _ReadLayer_c__Iterator;
	}
}
