﻿/* Author: Oran Bar
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
using Auto.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


[Auto.Utils.ScriptTiming(-500)]	
public class AutoAttributeManager : MonoBehaviour
{

	private List<MonoBehaviour> monoBehavioursInSceneWithAuto = new List<MonoBehaviour>();

	private void Awake()
	{
		SweeepScene();
	}

	public static void AutoReference(GameObject targetGo)
	{
		AutoReference(targetGo, out _, out _);
	}

	public static void AutoReference(MonoBehaviour mb)
	{
		AutoReference(mb, out _, out _);
	}

	public static void AutoReference(GameObject targetGo, out int successfulAssigments, out int failedAssignments)
	{
		successfulAssigments = 0;
		failedAssignments = 0;

		foreach(var mb in targetGo.GetComponents<MonoBehaviour>(true))
		{
			AutoReference(mb, out int successes, out int failures);
			successfulAssigments += successes;
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
				bool result = autofind.Execute(targetMb, prop.PropertyType, (mb, val) => prop.SetValue(mb, val));
				if(result){
					successfullyAssigments++;
				}else{
					failedAssignments++;
				}
			}
		}
	}

    public void SweeepScene()
	{
#if DEB
		Stopwatch sw = new Stopwatch();

		sw.Start();
#endif
		IEnumerable<MonoBehaviour> monoBehaviours = null;
		if(monoBehavioursInSceneWithAuto?.Any() != true){
			//Fallback if, for some reason, the monobehaviours were not previously cached
			monoBehaviours = GetAllMonobehavioursWithAuto();
		} else {
			monoBehaviours = monoBehavioursInSceneWithAuto;
		}

		int autoVarialbesAssigned_count = 0;
		int autoVarialbesNotAssigned_count = 0;

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
			monoBehavioursInSceneWithAuto.Count(), variablesAnalized, variablesWithAuto, sw.ElapsedMilliseconds, autoVarialbesAssigned_count, autoVarialbesAssigned_count+autoVarialbesNotAssigned_count, result_color );
#endif
	}

	// public void CacheMonobehavioursWithAuto(){
	// 	var start = Time.time;
	// 	monoBehavioursInSceneWithAuto = GetAllMonobehavioursWithAuto().ToList();
	// 	UnityEngine.Debug.Log($"Cached {monoBehavioursInSceneWithAuto.Count} MonoBehaviours in {Time.time - start} mills");
	// }

	private IEnumerable<MonoBehaviour> GetAllMonobehavioursWithAuto(){
		
		IEnumerable<MonoBehaviour> monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>(true)
				.Where(mb => mb.gameObject.scene == this.gameObject.scene);

		monoBehaviours = monoBehaviours.Where(mb => GetFieldsWithAuto(mb).Count() + GetPropertiesWithAuto(mb).Count() > 0);

		return monoBehaviours;
	}

	private static IEnumerable<FieldInfo> GetFieldsWithAuto(MonoBehaviour mb)
	{
		ReflectionHelperMethods rhm = new ReflectionHelperMethods();

		return mb.GetType()
			.GetFields(BindingFlags.Instance | BindingFlags.Public)
			.Where(prop => prop.FieldType.IsPrimitive == false)
			.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
								Attribute.IsDefined(prop, typeof(AutoParentAttribute))
			)
			.Concat(
				rhm.GetNonPublicFieldsInBaseClasses(mb.GetType())
				.Where(prop => prop.FieldType.IsPrimitive == false)
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
			.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(prop => prop.PropertyType.IsPrimitive == false)
			.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
					Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
					Attribute.IsDefined(prop, typeof(AutoParentAttribute))
			)
			.Concat(
				rhm.GetNonPublicPropertiesInBaseClasses(mb.GetType())
				.Where(prop => prop.PropertyType.IsPrimitive == false)
				.Where(prop => Attribute.IsDefined(prop, typeof(AutoAttribute)) ||
						Attribute.IsDefined(prop, typeof(AutoChildrenAttribute)) ||
						Attribute.IsDefined(prop, typeof(AutoParentAttribute))
				)
			);
	}
}