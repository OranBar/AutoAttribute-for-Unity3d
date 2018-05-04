/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's component. 
 * It basically acts as an automatic GetComponentInParent on Awake.
 * Using [AutoChildren(true)], the behaviour can be extendend to act like an AddOrGetComponent: The component will be added if it is not found, instead of an error being thrown.
 * 
 * usage example
 * 
 * public class Foo
 * {
 *		[AutoParent] public BoxCollier myBoxCollier;	//This assigns the variable to the BoxColider attached on your object
 *		
 *		//Methods...
 * }
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Auto.Utils;

public abstract class AutoFamily : Attribute, IAutoAttribute
{
	private const string MonoBehaviourNameColor = "green";
	private static ReflectionHelperMethods Rhm = new ReflectionHelperMethods();

	private bool haltBuildIfNull = true;

	private Component targetComponent;

	public AutoFamily(bool getMadIfMissing = true)
	{
		this.haltBuildIfNull = getMadIfMissing;
	}

	public void Execute(MonoBehaviour mb, Type componentType, Action<MonoBehaviour, object> SetVariableType)
	{
		GameObject go = mb.gameObject;

		if (componentType.IsArray || Rhm.IsList(componentType))
		{
			MultipleComponentAssignment(mb, go, componentType, SetVariableType);
		}
		else
		{
			SetVariableType(mb, GetTheSingleComponent(mb, componentType));
		}
	}

	protected abstract object GetTheSingleComponent(MonoBehaviour mb, Type componentType);
	protected abstract string GetMethodName();
	
	private void MultipleComponentAssignment(MonoBehaviour mb, GameObject go, Type componentType, Action<MonoBehaviour, object> SetVariable)
	{
		Type listElementType = AutoUtils.GetElementType(componentType);

		MethodInfo method = typeof(GameObject).GetMethods()
			//The next line would be similar to .Where(m => m.Name == "GetComponentsInChildren/InParent")
			.First(m =>
			{
				bool result = true;
				result = result && m.Name == GetMethodName();
				result = result && m.IsGenericMethod;
				result = result && m.GetParameters().Length == 1;
				result = result && m.GetParameters()[0].ParameterType == typeof(bool);
				return result;
			});
		//we want to pass true as arg, to get from inactive objs too
		MethodInfo generic = method.MakeGenericMethod(listElementType);
		dynamic componentsToReference = generic.Invoke(go, new object[] { true });

		if (componentsToReference.Length == 0)
		{
			string errorMessage = string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find any components <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}.</color>",
						componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor);

			if (haltBuildIfNull)
			{
				//Logging an error during PostProcessScene halts the build.
				Debug.LogError(errorMessage);
			}
			else
			{
				Debug.Log("<color=red>"+errorMessage+"</color>");
			}
			return;

		}
			
		if (componentType.IsArray)
		{
			SetVariable(mb, componentsToReference);
		}
		else if (Rhm.IsList(componentType))
		{
			SetVariable(mb, Enumerable.ToList(componentsToReference));
		}
	}

	
}

public static class AutoUtils
{

	internal static Type GetElementType(Type seqType)
	{
		Type ienum = FindIEnumerable(seqType);
		if (ienum == null) return seqType;
		return ienum.GetGenericArguments()[0];
	}
	private static Type FindIEnumerable(Type seqType)
	{
		if (seqType == null || seqType == typeof(string))
			return null;
		if (seqType.IsArray)
			return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
		if (seqType.IsGenericType)
		{
			foreach (Type arg in seqType.GetGenericArguments())
			{
				Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
				if (ienum.IsAssignableFrom(seqType))
				{
					return ienum;
				}
			}
		}
		Type[] ifaces = seqType.GetInterfaces();
		if (ifaces != null && ifaces.Length > 0)
		{
			foreach (Type iface in ifaces)
			{
				Type ienum = FindIEnumerable(iface);
				if (ienum != null) return ienum;
			}
		}
		if (seqType.BaseType != null && seqType.BaseType != typeof(object))
		{
			return FindIEnumerable(seqType.BaseType);
		}
		return null;
	}

}