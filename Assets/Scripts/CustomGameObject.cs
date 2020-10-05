using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomGameObject : MonoBehaviour
{
	public delegate void UpdateCustom(string updateName, float timeRate);

	public delegate void Callback(string callbackName);

	private float m_gameObjTime;

	private float m_gameObjTimeRate = 1f;

	private float m_gameObjSleepTime;

	private bool m_updateStdLast;

	private bool m_updateCusOnly = true;

	private Dictionary<string, CustomGameObject.UpdateCustom> m_updateDelegateList;

	private Dictionary<string, float> m_updateDelegateListTime;

	private Dictionary<string, float> m_updateDelegateListCurrentTime;

	private Dictionary<string, CustomGameObject.Callback> m_callbackDelegateList;

	private Dictionary<string, float> m_callbackDelegateListCurrentTime;

	public float gameObjectTime
	{
		get
		{
			return this.m_gameObjTime;
		}
	}

	public float gameObjectTimeRate
	{
		get
		{
			return this.m_gameObjTimeRate;
		}
		protected set
		{
			this.m_gameObjTimeRate = value;
		}
	}

	public float gameObjectSleepTime
	{
		get
		{
			return this.m_gameObjSleepTime;
		}
		protected set
		{
			this.m_gameObjSleepTime = value;
		}
	}

	public bool isSleep
	{
		get
		{
			return this.m_gameObjSleepTime > 0f;
		}
	}

	protected bool updateStdLast
	{
		get
		{
			return this.m_updateStdLast;
		}
		set
		{
			this.m_updateStdLast = value;
		}
	}

	protected bool updateCusOnly
	{
		get
		{
			return this.m_updateCusOnly;
		}
		set
		{
			this.m_updateCusOnly = value;
		}
	}

	private void Start()
	{
		this.m_gameObjTime = 0f;
	}

	private void Update()
	{
		float num = Time.deltaTime;
		if (this.m_gameObjSleepTime <= 0f)
		{
			num *= this.m_gameObjTimeRate;
			if (!this.m_updateStdLast)
			{
				this.UpdateStd(num, this.m_gameObjTimeRate);
			}
			this.UpdateCustoms(num, this.m_gameObjTimeRate);
			if (this.m_updateStdLast)
			{
				this.UpdateStd(num, this.m_gameObjTimeRate);
			}
			this.m_gameObjTime += num;
		}
		else
		{
			this.m_gameObjSleepTime -= num;
			if (this.m_gameObjSleepTime <= 0f)
			{
				this.m_gameObjSleepTime = 0f;
			}
		}
	}

	private void UpdateCustoms(float deltaTime, float timeRate)
	{
		if (this.m_updateDelegateList != null)
		{
			int num = 0;
			Dictionary<string, CustomGameObject.UpdateCustom>.KeyCollection keys = this.m_updateDelegateList.Keys;
			foreach (string current in keys)
			{
				if (this.m_updateDelegateListCurrentTime[current] <= 0f)
				{
					if (!this.m_updateCusOnly || num == 0)
					{
						this.m_updateDelegateList[current](current, timeRate);
						this.m_updateDelegateListCurrentTime[current] = this.m_updateDelegateListTime[current];
						num++;
					}
				}
				else
				{
					Dictionary<string, float> updateDelegateListCurrentTime;
					Dictionary<string, float> expr_8E = updateDelegateListCurrentTime = this.m_updateDelegateListCurrentTime;
					string key;
					string expr_92 = key = current;
					float num2 = updateDelegateListCurrentTime[key];
					expr_8E[expr_92] = num2 - deltaTime;
					if (this.m_updateDelegateListCurrentTime[current] <= 0f)
					{
						this.m_updateDelegateListCurrentTime[current] = 0f;
					}
				}
			}
		}
		if (this.m_callbackDelegateList != null)
		{
			Dictionary<string, CustomGameObject.Callback>.KeyCollection keys2 = this.m_callbackDelegateList.Keys;
			List<string> list = null;
			foreach (string current2 in keys2)
			{
				if (this.m_callbackDelegateListCurrentTime[current2] <= 0f)
				{
					this.m_callbackDelegateList[current2](current2);
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(current2);
				}
				else
				{
					Dictionary<string, float> callbackDelegateListCurrentTime;
					Dictionary<string, float> expr_16C = callbackDelegateListCurrentTime = this.m_callbackDelegateListCurrentTime;
					string key;
					string expr_171 = key = current2;
					float num2 = callbackDelegateListCurrentTime[key];
					expr_16C[expr_171] = num2 - deltaTime;
					if (this.m_callbackDelegateListCurrentTime[current2] <= 0f)
					{
						this.m_callbackDelegateListCurrentTime[current2] = 0f;
					}
				}
			}
			if (list != null)
			{
				foreach (string current3 in list)
				{
					this.RemoveCallback(current3);
				}
			}
		}
	}

	protected virtual void UpdateStd(float deltaTime, float timeRate)
	{
	}

	protected bool AddUpdateCustom(CustomGameObject.UpdateCustom upd, string updName, float updTime)
	{
		bool result = false;
		if (!this.IsUpdateCustom(updName))
		{
			if (this.m_updateDelegateList == null)
			{
				this.m_updateDelegateList = new Dictionary<string, CustomGameObject.UpdateCustom>();
			}
			if (this.m_updateDelegateListTime == null)
			{
				this.m_updateDelegateListTime = new Dictionary<string, float>();
			}
			if (this.m_updateDelegateListCurrentTime == null)
			{
				this.m_updateDelegateListCurrentTime = new Dictionary<string, float>();
			}
			this.m_updateDelegateList.Add(updName, upd);
			this.m_updateDelegateListTime.Add(updName, updTime);
			this.m_updateDelegateListCurrentTime.Add(updName, updTime);
			result = true;
		}
		return result;
	}

	protected bool SetUpdateCustom(string updName, float updTime)
	{
		bool result = false;
		if (this.IsUpdateCustom(updName))
		{
			this.m_updateDelegateListTime[updName] = updTime;
			this.m_updateDelegateListCurrentTime[updName] = updTime;
			result = true;
		}
		return result;
	}

	protected bool RemoveUpdateCustom(string updName = null)
	{
		bool result = false;
		if (string.IsNullOrEmpty(updName))
		{
			if (this.m_updateDelegateList != null)
			{
				this.m_updateDelegateList.Clear();
			}
			if (this.m_updateDelegateListTime != null)
			{
				this.m_updateDelegateListTime.Clear();
			}
			if (this.m_updateDelegateListCurrentTime != null)
			{
				this.m_updateDelegateListCurrentTime.Clear();
			}
			this.m_updateDelegateList = null;
			this.m_updateDelegateListTime = null;
			this.m_updateDelegateListCurrentTime = null;
			result = true;
		}
		else if (this.IsUpdateCustom(updName))
		{
			this.m_updateDelegateList.Remove(updName);
			this.m_updateDelegateListTime.Remove(updName);
			this.m_updateDelegateListCurrentTime.Remove(updName);
			if (this.m_updateDelegateList.Count <= 0)
			{
				this.m_updateDelegateList = null;
				this.m_updateDelegateListTime = null;
				this.m_updateDelegateListCurrentTime = null;
			}
			result = true;
		}
		return result;
	}

	protected bool IsUpdateCustom(string updName)
	{
		bool result = false;
		if (this.m_updateDelegateList != null)
		{
			result = this.m_updateDelegateList.ContainsKey(updName);
		}
		return result;
	}

	protected bool AddCallback(CustomGameObject.Callback call, string callName, float callTime)
	{
		bool result = false;
		if (!this.IsCallback(callName))
		{
			if (this.m_callbackDelegateList == null)
			{
				this.m_callbackDelegateList = new Dictionary<string, CustomGameObject.Callback>();
			}
			if (this.m_callbackDelegateListCurrentTime == null)
			{
				this.m_callbackDelegateListCurrentTime = new Dictionary<string, float>();
			}
			this.m_callbackDelegateList.Add(callName, call);
			this.m_callbackDelegateListCurrentTime.Add(callName, callTime);
			result = true;
		}
		return result;
	}

	protected bool RemoveCallback(string callName = null)
	{
		bool result = false;
		if (string.IsNullOrEmpty(callName))
		{
			if (this.m_callbackDelegateList != null)
			{
				this.m_callbackDelegateList.Clear();
			}
			if (this.m_callbackDelegateListCurrentTime != null)
			{
				this.m_callbackDelegateListCurrentTime.Clear();
			}
			this.m_callbackDelegateList = null;
			this.m_callbackDelegateListCurrentTime = null;
			result = true;
		}
		else if (this.IsCallback(callName))
		{
			this.m_callbackDelegateList.Remove(callName);
			this.m_callbackDelegateListCurrentTime.Remove(callName);
			result = true;
		}
		return result;
	}

	protected int RemoveCallbackPartialMatch(string callName = null)
	{
		int num = 0;
		if (this.m_callbackDelegateList != null && this.m_callbackDelegateList.Count > 0)
		{
			Dictionary<string, CustomGameObject.Callback>.KeyCollection keys = this.m_callbackDelegateList.Keys;
			List<string> list = new List<string>();
			foreach (string current in keys)
			{
				if (current.IndexOf(callName) != -1 && this.IsCallback(current))
				{
					list.Add(current);
				}
			}
			foreach (string current2 in list)
			{
				this.m_callbackDelegateList.Remove(current2);
				this.m_callbackDelegateListCurrentTime.Remove(current2);
				num++;
			}
		}
		return num;
	}

	protected bool IsCallback(string callName)
	{
		bool result = false;
		if (this.m_callbackDelegateList != null)
		{
			result = this.m_callbackDelegateList.ContainsKey(callName);
		}
		return result;
	}

	public void ResetGameObjTime()
	{
		this.m_gameObjTime = 0f;
	}
}
