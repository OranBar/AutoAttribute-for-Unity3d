/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's component. 
 * It basically acts as an automatic GetComponentInChildren on Awake.
 * Using [AutoChildren(true)], the behaviour can be extendend to act like an AddOrGetComponent: The component will be added if it is not found, instead of an error being thrown.
 * 
 * usage example
 * 
 * public class Foo
 * {
 *		[AutoChildren] public BoxCollier myBoxCollier;	//This assigns the variable to the BoxColider attached on your object
 *		
 *		//Methods...
 * }
 * 
 * Copyrights to Oran Bar™
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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