using System;
using UnityEngine;

namespace Player
{
	public class PhantomDrillUtil
	{
		private const string truckName = "drill_truck";

		private const float truckOffset = 0.1f;

		public const string EffectName = "ef_ph_spin_lp01";

		public const string SEName = "phantom_drill_quick";

		public static GameObject ChangeVisualOnEnter(CharacterState context)
		{
			string name = CharacterDefs.PhantomBodyName[1];
			StateUtil.EnableChildObject(context, context.BodyModelName, false);
			StateUtil.EnableChildObject(context, name, true);
			StateUtil.EnablePlayerCollision(context, false);
			SoundManager.SePlay("phantom_drill_quick", "SE");
			GameObject gameObject = StateUtil.CreateEffect(context, "ef_ph_spin_lp01", false);
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(context.gameObject, name);
			if (gameObject != null && gameObject2 != null)
			{
				gameObject.transform.parent = gameObject2.transform;
			}
			StateUtil.SetShadowActive(context, false);
			return gameObject;
		}

		public static void ChangeVisualOnLeave(CharacterState context, GameObject effect)
		{
			StateUtil.EnableChildObject(context, context.BodyModelName, true);
			StateUtil.EnableChildObject(context, CharacterDefs.PhantomBodyName[1], false);
			StateUtil.EnablePlayerCollision(context, true);
			if (effect != null)
			{
				UnityEngine.Object.Destroy(effect);
			}
			SoundManager.SeStop("phantom_drill_quick", "SE");
			StateUtil.SetShadowActive(context, true);
		}

		public static GameObject CreateTruck(CharacterState context)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(context.gameObject, "drill_truck");
			if (gameObject)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.SetActive(true);
					gameObject2.GetComponent<DrillTrack>().FrontOffset = 0.1f;
					return gameObject2;
				}
			}
			return null;
		}

		public static void DestroyTruck(GameObject truck)
		{
			if (truck != null)
			{
				UnityEngine.Object.Destroy(truck);
			}
		}

		public static bool CheckTruckDraw(CharacterState context, GameObject truck)
		{
			if (truck == null)
			{
				return false;
			}
			GameObject gameObject = GameObject.Find("GameMainCamera");
			if (!(gameObject != null))
			{
				return false;
			}
			Vector3 position = gameObject.transform.position;
			Vector3 position2 = context.Position;
			Vector3 direction = Vector3.Normalize(position2 - position);
			Ray ray = new Ray(position, direction);
			int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			layerMask = -1 - (1 << LayerMask.NameToLayer("Player"));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, Vector3.Distance(position2, position), layerMask))
			{
				truck.GetComponent<DrillTrack>().Disable = false;
				return true;
			}
			truck.GetComponent<DrillTrack>().Disable = true;
			return false;
		}

		public static void CheckAndCreateFogEffect(CharacterState context, bool nowInDirt, Vector3 prevPosition)
		{
			Vector3 a = context.Position;
			Vector3 vector = context.Position - prevPosition;
			if (!nowInDirt)
			{
				vector -= vector;
				a = prevPosition;
			}
			Vector3 vector2 = Vector3.Normalize(vector);
			Ray ray = new Ray(a - vector2 * 0.5f, vector2);
			int layerMask = -1 - (1 << LayerMask.NameToLayer("Player"));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, vector.magnitude + 1f, layerMask))
			{
				GameObject gameObject = StateUtil.CreateEffect(context, "ef_ph_spin_fog01", true);
				if (gameObject != null)
				{
					gameObject.transform.parent = null;
					gameObject.transform.position = raycastHit.point;
					gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
				}
			}
			if (nowInDirt)
			{
				SoundManager.SePlay("phantom_drill_in", "SE");
			}
			else
			{
				SoundManager.SePlay("phantom_drill_out", "SE");
			}
		}
	}
}
