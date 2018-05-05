/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's component. 
 * It acts as the equivalent of a GetComponent call done in Awake.
 * Components that Auto has not been able to find are logged as errors in the console. 
 * Using [Auto(true)], Auto will log warnings as opposed to errors. 
 * This is important because, allowing Auto to log error will result in builds being halted whenever one of the [Auto] variables assignments was unsuccessful.
 * 
 * Usage example:
 * 
 * public class Foo
 * {
 *		[Auto] public BoxCollier myBoxCollier;	//This assigns the variable to the BoxColider attached on your object
 *		[Auto(true)] public Camera myCamera;	//since we passed true as an argument, if the camera is not found, Auto will log a warning as opposed to an error, and won't halt the build.
 *		
 *		//[...]
 * }
 * 
 */

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutoAttribute : Attribute, IAutoAttribute {

	private const string MonoBehaviourNameColor = "green";	//Changeme

	private bool logMissingAsError = true;

	private Component targetComponent;

	public AutoAttribute(bool logMissingAsError = true)
	{
		this.logMissingAsError = logMissingAsError;
	}

	/// <Summary>
	///	Executes the call to fetch the component and assign it to the variable with [Auto*]
	/// </Summary>
	public bool Execute(MonoBehaviour mb, Type componentType, Action<MonoBehaviour, object> SetVariableType)
	{
		GameObject go = mb.gameObject;

		Component componentToReference = go.GetComponent(componentType);
		if (componentToReference == null)
		{
			LogMissingComponent(mb, componentType, go);
			
			return false;
		}

		SetVariableType(mb, componentToReference);
		
		return true;
	}

	
	private void LogMissingComponent(MonoBehaviour mb, Type componentType, GameObject go){
		string errorMessage = string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}</color>",
						componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor);

		if (logMissingAsError)
		{
			//Logging an error during PostProcessScene halts the build.
			Debug.LogError(errorMessage, mb);
		}
		else
		{
			Debug.LogWarning("<color=red>" + errorMessage + "</color>", mb);
		}
	}
}