﻿using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[InitializeOnLoad]
public class AutoAttributeManagerEditor : UnityEditor.AssetModificationProcessor
{
	static AutoAttributeManagerEditor(){
	}

	private static void MakeSureAutoManagerIsInScene()
	{
		var autoManagers = GameObject.FindObjectsOfType<AutoAttributeManager>(true);
		bool noAutoAttributeManager_inScene = autoManagers == null || autoManagers.Length == 0;
		if (noAutoAttributeManager_inScene) {
			InstantiateAutoAttributeManager_InScene(); 
		}
		else if (autoManagers.Length >=2)
		{
			autoManagers.Skip(1).ToList().ForEach(DestroyAutoAttributeManager);
		}
	}
	
	private static void DestroyAutoAttributeManager(AutoAttributeManager autoAttributeManager)
	{
		GameObject.DestroyImmediate(autoAttributeManager);
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	private static void InstantiateAutoAttributeManager_InScene() {
		GameObject autoGo = new GameObject("Auto_Attribute_Manager");
		autoGo.AddComponent<AutoAttributeManager>();
		//Make scene dirty, to notify it has changed following the creation of the AutoAttributeManager
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); 
	}

	public static string[] OnWillSaveAssets(string[] paths)
	{
		MakeSureAutoManagerIsInScene();
		var autoManager = GameObject.FindObjectOfType<AutoAttributeManager>();
		autoManager.CacheMonobehavioursWithAuto();
		return paths;
	}
}
