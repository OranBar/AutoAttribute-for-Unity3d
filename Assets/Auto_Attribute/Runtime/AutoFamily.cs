/* Author: Oran Bar
 * This class contains the shared code for Auto attributes that fetch arrays of multiple components of the same type
 */

using System;
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
			Rhm.IsList(componentType))
		{
			// Can't handle lists without using dynamic keyword. 
			// Arrays will have to be enough.
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
		object[] componentsToReference = generic.Invoke(go, new object[] { true }) as object[];

		return componentsToReference;
	}

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
