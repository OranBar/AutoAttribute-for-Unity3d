using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auto.Utils;

public static class AutoEx 
{
	#region Instantiate_And_AssignAutoVariables
	public static GameObject Instantiate_And_AssignAutoVariables<T>(this T mb, GameObject original, Vector3 position, Quaternion rotation) where T: MonoBehaviour {
		var newObj = GameObject.Instantiate(original);
		
		AutoAttributeManager.AutoReference(newObj);
		
		return newObj;
	}

	public static GameObject Instantiate_And_AssignAutoVariables<T>(this T mb, GameObject original) where T: MonoBehaviour {
		var newObj = GameObject.Instantiate(original);
		
		AutoAttributeManager.AutoReference(newObj);
		Debug.Log("AutoAssignment Complete");
		
		return newObj;
	}

	public static GameObject Instantiate_And_AssignAutoVariables<T>(this T mb, GameObject original, Transform parent) where T: MonoBehaviour {
		var newObj = GameObject.Instantiate(original, parent);
		
		AutoAttributeManager.AutoReference(newObj);
		
		return newObj;
	}

	public static GameObject Instantiate_And_AssignAutoVariables<T>(this T mb, GameObject original, Vector3 position, Quaternion rotation, Transform parent) where T: MonoBehaviour {
		var newObj = GameObject.Instantiate(original, position, rotation, parent);
		
		AutoAttributeManager.AutoReference(newObj);
		
		return newObj;
	}
	
	public static GameObject Instantiate_And_AssignAutoVariables<T>(this T mb, GameObject original, Transform parent, bool instantiateInWorldSpace) where T: MonoBehaviour {
		var newObj = GameObject.Instantiate(original, parent, instantiateInWorldSpace);
		
		AutoAttributeManager.AutoReference(newObj);
		
		return newObj;
	}
	#endregion
}
