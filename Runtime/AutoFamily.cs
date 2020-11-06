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
 * Copyrights to Oran Bar™
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

	private bool logErrorIfMissing = true;

	private Component targetComponent;

	public AutoFamily(bool getMadIfMissing = true)
	{
		this.logErrorIfMissing = getMadIfMissing;
	}

	public bool Execute(MonoBehaviour mb, Type componentType, Action<MonoBehaviour, object> setVariable)
	{
		GameObject go = mb.gameObject;

		if(componentType.IsArray){
			return AssignArray(mb, go, componentType, setVariable);
		} else if (
			// componentType.IsArray || 
			Rhm.IsList(componentType))
		{
			// MultipleComponentAssignment(mb, go, componentType, SetVariableType);

			// return AssignList(mb, go, componentType, setVariable);
			return false;

		}
		else
		{
			setVariable(mb, GetTheSingleComponent(mb, componentType));
			return true;
		}
	}

	protected abstract object GetTheSingleComponent(MonoBehaviour mb, Type componentType);
	protected abstract string GetMethodName();
	
	private object[] GetComponentsToReference(MonoBehaviour mb, GameObject go, Type componentType){
		Type listElementType = AutoUtils.GetElementType(componentType);

		MethodInfo method = typeof(GameObject).GetMethods()
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
		//TODO: We know it's gonna be either an array or a list. We do not need to use dynamic, I think
		object[] componentsToReference = generic.Invoke(go, new object[] { true }) as object[];

		return componentsToReference;
	}

	// private bool AssignList(MonoBehaviour mb, GameObject go, Type componentType, Action<MonoBehaviour, object> setVariable)
	// {
	// 	object[] componentsToReference = GetComponentsToReference(mb, go, componentType);

	// 	if (logErrorIfMissing && componentsToReference.Length == 0){
	// 		Debug.LogError(
	// 			string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find any components <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}.</color>",
	// 				componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor)
	// 			, go);
			
	// 		return false;
	// 	}

	// 	setVariable(mb, Enumerable.ToList(componentsToReference.Select(obj => (componentType) obj  )));
	// 	return true;
	// }

	private bool AssignArray(MonoBehaviour mb, GameObject go, Type componentType, Action<MonoBehaviour, object> setVariable)
	{
		object[] componentsToReference = GetComponentsToReference(mb, go, componentType);

		if (logErrorIfMissing && componentsToReference.Length == 0)	{
			Debug.LogError(
				string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find any components <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}.</color>",
					componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor)
				, go);
			return false;
		}

		setVariable(mb, componentsToReference);
		return true;
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