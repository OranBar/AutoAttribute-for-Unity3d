using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auto.Utils;
using UnityEngine;

public static class AutoEx 
{
	#region Instantiate_And_AssignAutoVariables

	public static T AddComponent_Auto<T>(this GameObject go) where T : MonoBehaviour
	{
		T newComponent = go.AddComponent<T>();
		AutoAttributeManager.AutoReference(newComponent);

		return newComponent;
	}

	public static T AddComponent_Auto<T>(this MonoBehaviour mb) where T : MonoBehaviour {
		return mb.gameObject.AddComponent_Auto<T>();
	}
	#endregion
}
