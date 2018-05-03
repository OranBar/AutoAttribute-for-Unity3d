/* Author: Oran Bar
 * Summary: This attribute automatically assigns a class variable to one of the gameobject's component. 
 * It basically acts as an automatic GetComponent on Awake.
 * Using [Auto(true)], the behaviour can be extendend to act like an AddOrGetComponent: The component will be added if it is not found, instead of an error being thrown.
 * 
 * usage example
 * 
 * public class Foo
 * {
 *		[Auto] public BoxCollier myBoxCollier;	//This assigns the variable to the BoxColider attached on your object
 *		[Auto(true)] public Camera myCamera;	//If this GameObject has no camera, since we wrote (true), the component will be automayically added and referenced.
 *		
 *		//Methods...
 * }
 * 
 * Copyrights to Oran Bar™
 */

using System;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutoAttribute : Attribute, IAutoAttribute {

	private const string MonoBehaviourNameColor = "green";

	private bool autoAdd = false;
	private bool logErrorIfMissing = true;

	private Component targetComponent;

	public AutoAttribute(bool autoAdd)
	{
		this.autoAdd = autoAdd;
		this.logErrorIfMissing = true;
	}

	public AutoAttribute(bool autoAdd = false, bool getMadIfMissing = true)
	{
		this.autoAdd = autoAdd;
		this.logErrorIfMissing = getMadIfMissing;
	}

	public void Execute(MonoBehaviour mb, Type componentType, Action<MonoBehaviour, object> SetVariableType)
	{
		GameObject go = mb.gameObject;

		Component componentToReference = go.GetComponent(componentType);
		if (componentToReference == null)
		{
			if (autoAdd)
			{
				Debug.LogWarning(string.Format("[Auto]: <color={3}><b>{1}</b></color> automatically added component <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{1}</color>",
					componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor)
					, go);
				componentToReference = mb.gameObject.AddComponent(componentType);
			}
			else
			{
				if (logErrorIfMissing)
				{
					Debug.LogError(
						string.Format("[Auto]: <color={3}><b>{1}</b></color> couldn't find <color=#cc3300><b>{0}</b></color> on <color=#e68a00>{2}</color>",
							componentType.Name, mb.GetType().Name, go.name, MonoBehaviourNameColor)
						, go);
				}
				return;
			}
		}

		SetVariableType(mb, componentToReference);
	}
	
}