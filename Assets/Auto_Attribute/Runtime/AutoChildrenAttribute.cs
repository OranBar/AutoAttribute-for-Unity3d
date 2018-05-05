/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's components; if nothing is found, it will continue to look for it going down the scene hiearchy (children). 
 * It acts as the equivalent of a GetComponentInChildren call done in Awake.
 * Components that Auto has not been able to find are logged as errors in the console. 
 * Using [Auto(true)], Auto will log warnings as opposed to errors. 
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
public class AutoChildrenAttribute : AutoFamily
{
	protected override string GetMethodName()
	{
		return "GetComponentsInChildren";
	}

	protected override object GetTheSingleComponent(MonoBehaviour mb, Type componentType)
	{
		return mb.GetComponentInChildren(componentType, true);
	}
}