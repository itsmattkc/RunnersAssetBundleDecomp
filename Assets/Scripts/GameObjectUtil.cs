using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class GameObjectUtil
{
	private sealed class _FindChildGameObjectsEnumerable_c__Iterator22 : IDisposable, IEnumerator, IEnumerable, IEnumerable<GameObject>, IEnumerator<GameObject>
	{
		internal int _i___0;

		internal GameObject parent;

		internal GameObject _child___1;

		internal string name;

		internal IEnumerable<GameObject> _gos___2;

		internal IEnumerator<GameObject> __s_422___3;

		internal GameObject _go___4;

		internal int _PC;

		internal GameObject _current;

		internal GameObject ___parent;

		internal string ___name;

		GameObject IEnumerator<GameObject>.Current
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.System.Collections.Generic.IEnumerable_UnityEngine.GameObject_.GetEnumerator();
		}

		IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
		{
			if (Interlocked.CompareExchange(ref this._PC, 0, -2) == -2)
			{
				return this;
			}
			GameObjectUtil._FindChildGameObjectsEnumerable_c__Iterator22 _FindChildGameObjectsEnumerable_c__Iterator = new GameObjectUtil._FindChildGameObjectsEnumerable_c__Iterator22();
			_FindChildGameObjectsEnumerable_c__Iterator.parent = this.___parent;
			_FindChildGameObjectsEnumerable_c__Iterator.name = this.___name;
			return _FindChildGameObjectsEnumerable_c__Iterator;
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0u:
				this._i___0 = 0;
				goto IL_12A;
			case 1u:
				IL_87:
				this._gos___2 = GameObjectUtil.FindChildGameObjectsEnumerable(this._child___1, this.name);
				this.__s_422___3 = this._gos___2.GetEnumerator();
				num = 4294967293u;
				break;
			case 2u:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				}
				if (this.__s_422___3.MoveNext())
				{
					this._go___4 = this.__s_422___3.Current;
					this._current = this._go___4;
					this._PC = 2;
					flag = true;
					return true;
				}
			}
			finally
			{
				if (!flag)
				{
					if (this.__s_422___3 != null)
					{
						this.__s_422___3.Dispose();
					}
				}
			}
			this._i___0++;
			IL_12A:
			if (this._i___0 >= this.parent.transform.childCount)
			{
				this._PC = -1;
			}
			else
			{
				this._child___1 = this.parent.transform.GetChild(this._i___0).gameObject;
				if (this._child___1.name == this.name)
				{
					this._current = this._child___1;
					this._PC = 1;
					return true;
				}
				goto IL_87;
			}
			return false;
		}

		public void Dispose()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 2u:
				try
				{
				}
				finally
				{
					if (this.__s_422___3 != null)
					{
						this.__s_422___3.Dispose();
					}
				}
				break;
			}
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static List<T> FindChildGameObjectsComponents<T>(GameObject parent, string name) where T : Component
	{
		List<T> list = new List<T>();
		if (parent != null)
		{
			IEnumerable<GameObject> enumerable = GameObjectUtil.FindChildGameObjectsEnumerable(parent, name);
			foreach (GameObject current in enumerable)
			{
				list.Add(current.GetComponent<T>());
			}
		}
		return list;
	}

	public static List<GameObject> FindChildGameObjects(GameObject parent, string name)
	{
		List<GameObject> list = new List<GameObject>();
		if (parent != null)
		{
			IEnumerable<GameObject> enumerable = GameObjectUtil.FindChildGameObjectsEnumerable(parent, name);
			foreach (GameObject current in enumerable)
			{
				list.Add(current);
			}
		}
		return list;
	}

	private static IEnumerable<GameObject> FindChildGameObjectsEnumerable(GameObject parent, string name)
	{
		GameObjectUtil._FindChildGameObjectsEnumerable_c__Iterator22 _FindChildGameObjectsEnumerable_c__Iterator = new GameObjectUtil._FindChildGameObjectsEnumerable_c__Iterator22();
		_FindChildGameObjectsEnumerable_c__Iterator.parent = parent;
		_FindChildGameObjectsEnumerable_c__Iterator.name = name;
		_FindChildGameObjectsEnumerable_c__Iterator.___parent = parent;
		_FindChildGameObjectsEnumerable_c__Iterator.___name = name;
		GameObjectUtil._FindChildGameObjectsEnumerable_c__Iterator22 expr_23 = _FindChildGameObjectsEnumerable_c__Iterator;
		expr_23._PC = -2;
		return expr_23;
	}

	public static GameObject FindChildGameObject(GameObject parent, string name)
	{
		Transform transform = parent.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject gameObject = transform.GetChild(i).gameObject;
			if (gameObject.name == name)
			{
				return gameObject;
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, name);
			if (gameObject2 != null)
			{
				return gameObject2;
			}
		}
		return null;
	}

	public static T FindChildGameObjectComponent<T>(GameObject parent, string name) where T : Component
	{
		if (parent == null)
		{
			return (T)((object)null);
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(parent, name);
		if (gameObject == null)
		{
			return (T)((object)null);
		}
		return gameObject.GetComponent<T>();
	}

	public static T FindGameObjectComponent<T>(string name) where T : Component
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject == null)
		{
			return (T)((object)null);
		}
		return gameObject.GetComponent<T>();
	}

	public static T FindGameObjectComponentWithTag<T>(string tagName, string name) where T : Component
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			if (gameObject.name == name)
			{
				return gameObject.GetComponent<T>();
			}
		}
		return (T)((object)null);
	}

	public static GameObject FindGameObjectWithTag(string tagName, string name)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			if (gameObject.name == name)
			{
				return gameObject;
			}
		}
		return null;
	}

	public static GameObject FindParentGameObject(GameObject gameObject, string name)
	{
		while (gameObject != null)
		{
			GameObject gameObject2 = null;
			Transform parent = gameObject.transform.parent;
			if (parent != null)
			{
				gameObject2 = parent.gameObject;
				if (gameObject2 != null && gameObject2.name == name)
				{
					return gameObject2;
				}
			}
			gameObject = gameObject2;
		}
		return null;
	}

	public static GameObject SendMessageFindGameObject(string objectName, string methodName, object value, SendMessageOptions options)
	{
		GameObject gameObject = GameObject.Find(objectName);
		if (gameObject != null)
		{
			gameObject.SendMessage(methodName, value, options);
		}
		return gameObject;
	}

	public static GameObject SendMessageFindGameObjectWithTag(string tagName, string objectName, string methodName, object value, SendMessageOptions options)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			if (gameObject.name == objectName)
			{
				gameObject.SendMessage(methodName, value, options);
				return gameObject;
			}
		}
		return null;
	}

	public static void SendMessageToTagObjects(string tagName, string methodName, object value, SendMessageOptions options)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			gameObject.SendMessage(methodName, value, options);
		}
	}

	public static void SendDelayedMessageFindGameObject(string objectName, string methodName, object value)
	{
		if (DelayedMessageManager.Instance != null)
		{
			DelayedMessageManager.Instance.AddDelayedMessage(objectName, methodName, value);
		}
	}

	public static void SendDelayedMessageToGameObject(GameObject gameObject, string methodName, object value)
	{
		if (DelayedMessageManager.Instance != null)
		{
			DelayedMessageManager.Instance.AddDelayedMessage(gameObject, methodName, value);
		}
	}

	public static void SendDelayedMessageToTagObjects(string tagName, string methodName, object value)
	{
		if (DelayedMessageManager.Instance != null)
		{
			DelayedMessageManager.Instance.AddDelayedMessageToTag(tagName, methodName, value);
		}
	}
}
