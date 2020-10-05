using Message;
using System;
using UnityEngine;

namespace Chao
{
	public class StageChao : MonoBehaviour
	{
		private void Start()
		{
			base.enabled = false;
		}

		private void OnMsgExitStage(MsgExitStage msg)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
