#define DEB

/* Author: Oran Bar
 * Summary: 
 * 
 * This class executes the code to automatically set the references of the variables with the Auto attribute.
 * The code is executed at the beginning of the scene, before Any other Awake has a chance to be executed.
 * Afterwards, all Auto variables will be assigned, and, in case of errors, [Auto] will log on the console with more info.
 
 * Don't be afraid of this little script. Apart from setting a few [Auto] variables, It's harmless. Let him live happly in your scene.
 * You'll learn to like him.
 * 
 * The Define DEB on top of the script can be commented out to remove console logs for performance.
 * 
 * Copyrights to Oran Bar™
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Auto.Utils;

[ScriptTiming(-1000)]
public class AutoAttributeManager : MonoBehaviour
{

	private void Awake()
	{
		print("[Auto]: Start Scene Sweep");
		SweeepScene();
		print("[Auto]: All Variables Referenced!");
	}

	public static void AutoReference(GameObject targetGo)
	{
		foreach(var mb in targetGo.GetComponents<MonoBehaviour>(true))
		{
			AutoReference(mb);
		}
	}

    public static void AutoReference(MonoBehaviour targetMb)
	{
		//Fields
		IEnumerable<FieldInfo> fields = GetFieldsWithAuto(targetMb);

		foreach (var field in fields)
		{
			foreach (IAutoAttribute autofind in field.GetCustomAttributes(typeof(IAutoAttribute), true))
			{
				var currentReferenceValue = field.GetValue(targetMb);
				// if (currentReferenceValue == null || currentReferenceValue.Equals(null))
				// {
					autofind.Execute(targetMb, field.FieldType, (mb, val)=>field.SetValue(mb, val));
				// }
			}
		}

		//Properties
		IEnumerable<PropertyInfo> properties = GetPropertiesWithAuto(targetMb);

		foreach (var prop in properties)
		{
			foreach (IAutoAttribute autofind in prop.GetCustomAttributes(typeof(IAutoAttribute), true))
			{
				var currentReferenceValue = prop.GetValue(targetMb, null);
				// if (currentReferenceValue == null || currentReferenceValue.Equals(null))
				// {
					autofind.Execute(targetMb, prop.PropertyType, (mb, val)=>prop.SetValue(mb, val));
				// }
			}
		}
	}

    public static void SweeepScene()
	{
#if DEB
		//Debug
		Stopwatch sw = new Stopwatch();

		sw.Start();
		//////////////////
#endif

		var activeScene = SceneManager.GetActiveScene();

		IEnumerable<MonoBehaviour> monoBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>()
			.Where(mb => mb.gameObject.scene == activeScene);


		foreach (var mb in monoBehaviours)
		{
			AutoReference(mb);
		}

#if DEB
		//Debug
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

		//Debug.Log("Elapsed "+sw.ElapsedMilliseconds+" milliseconds.");
		Debug.LogFormat("[Auto] Scan Time - {3} Milliseconds. \nAnalized {0} MonoBehaviours and {1} variables. {2}/{1} variables had [Auto]", monoBehaviours.Count(), variablesAnalized, variablesWithAuto, sw.ElapsedMilliseconds);
		/////////////////////
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