/* Author: Oran Bar
 * Summary: 
 * 
 * This class executes the code to automatically set the references of the variables with the Auto attribute.
 * The code is executed at the beginning of the scene, 500 milliseconds before other Awake calls. (This is done using the ScriptTiming attribute, and can be changed manually)
 * Afterwards, all Auto variables will be assigned, and, in case of errors, [Auto] will log on the console with more info.
 
 * AutoAttributeManager will sneak into your scene upon saving it. 
 * Don't be afraid of this little script. Apart from setting a few [Auto] variables, It's harmless. 
 * Let him live happly in your scene. You'll learn to like him.
 * 
 * If the #define DEB on top of this script is uncommented, Auto will log data about its performance in the console.
 * 
 * Copyrights to Oran Bar™
 */


#define DEB
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Auto.Utils;


[Auto.Utils.ScriptTiming(-500)]	
public class AutoAttributeManager : MonoBehaviour
{

	private void Awake()
	{
		SweeepScene();
	}

	public static void AutoReference(GameObject targetGo)
	{
		AutoReference(targetGo, out int succ, out int fail);
	}

	public static void AutoReference(MonoBehaviour mb)
	{
		AutoReference(mb, out int succ, out int fail);
	}

	public static void AutoReference(GameObject targetGo, out int successfullyAssigments, out int failedAssignments)
	{
		successfullyAssigments = 0;
		failedAssignments = 0;

		foreach(var mb in targetGo.GetComponents<MonoBehaviour>(true))
		{
			AutoReference(mb, out int successes, out int failures);
			successfullyAssigments += successes;
			failedAssignments += failures;
		}
	}

    public static void AutoReference(MonoBehaviour targetMb, out int successfullyAssigments, out int failedAssignments)
	{
		successfullyAssigments = 0;
		failedAssignments = 0;

		//Fields
		IEnumerable<FieldInfo> fields = GetFieldsWithAuto(targetMb);

		foreach (var field in fields)
		{
			foreach (IAutoAttribute autoAttribute in field.GetCustomAttributes(typeof(IAutoAttribute), true))
			{
				// var currentReferenceValue = field.GetValue(targetMb);
				bool result = autoAttribute.Execute(targetMb, field.FieldType, (mb, val) => field.SetValue(mb, val));
				if(result){
					successfullyAssigments++;
				}else{
					failedAssignments++;
				}
			}
		}

		//Properties
		IEnumerable<PropertyInfo> properties = GetPropertiesWithAuto(targetMb);

		foreach (var prop in properties)
		{
			foreach (IAutoAttribute autofind in prop.GetCustomAttributes(typeof(IAutoAttribute), true))
			{
				// var currentReferenceValue = prop.GetValue(targetMb, null);
				bool result = autofind.Execute(targetMb, prop.PropertyType, (mb, val) => prop.SetValue(mb, val));
				if(result){
					successfullyAssigments++;
				}else{
					failedAssignments++;
				}
			}
		}
	}

    public static void SweeepScene()
	{
#if DEB
		Stopwatch sw = new Stopwatch();

		sw.Start();
#endif
		int autoVarialbesAssigned_count = 0;
		int autoVarialbesNotAssigned_count = 0;
	
		var activeScene = SceneManager.GetActiveScene();

		IEnumerable<MonoBehaviour> monoBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>()
			.Where(mb => mb.gameObject.scene == activeScene);


		foreach (var mb in monoBehaviours)
		{
			AutoReference(mb, out int succ, out int fail);
			autoVarialbesAssigned_count += succ;
			autoVarialbesNotAssigned_count += fail;
		}

#if DEB
		sw.Stop();

		int variablesAnalized = monoBehaviours
			.Select(mb => mb.GetType())
			.Aggregate(0, (agg, mbType) => 
				agg = agg + mbType.GetFields().Count() + mbType.GetProperties().Count()
			);

		int variablesWithAuto = monoBehaviours
			.Aggregate(0, (agg, mb) => 
				agg = agg + GetFieldsWithAuto(mb).Count() + GetPropertiesWithAuto(mb).Count()
			);

		string result_color = (autoVarialbesNotAssigned_count > 0) ? "red" : "green";
		UnityEngine.Debug.LogFormat("[Auto] Assigned <color={5}><b>{4}/{2}</b></color> [Auto*] variables in <color=#cc3300><b>{3} Milliseconds </b></color> - Analized {0} MonoBehaviours and {1} variables", 
			monoBehaviours.Count(), variablesAnalized, variablesWithAuto, sw.ElapsedMilliseconds, autoVarialbesAssigned_count, autoVarialbesAssigned_count+autoVarialbesNotAssigned_count, result_color );
#endif
	}

	private static IEnumerable<FieldInfo> GetFieldsWithAuto(MonoBehaviour mb)
	{
		ReflectionHelperMethods rhm = new ReflectionHelperMethods();

		return mb.GetType()
			.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
			.Where(prop => prop.FieldType.IsClass)
			.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoParentAttribute))
			)
			.Concat(
				rhm.GetNonPublicFieldsInBaseClasses(mb.GetType())
				.Where(prop => prop.FieldType.IsClass)
				.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoParentAttribute))
				)
			); 
	}

	private static IEnumerable<PropertyInfo> GetPropertiesWithAuto(MonoBehaviour mb)
	{
		ReflectionHelperMethods rhm = new ReflectionHelperMethods();

		return mb.GetType()
			.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
			.Where(prop => prop.PropertyType.IsClass)
			.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
					Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
					Attribute.IsDefined(prop, typeof(AutoParentAttribute))
			)
			.Concat(
				rhm.GetNonPublicPropertiesInBaseClasses(mb.GetType())
				.Where(prop => prop.PropertyType.IsClass)
				.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
						Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
						Attribute.IsDefined(prop, typeof(AutoParentAttribute))
				)
			);
	}
}