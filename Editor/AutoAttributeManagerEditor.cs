using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoAttributeManagerEditor : UnityEditor.AssetModificationProcessor
{
	private static void MakeSureAutoManagerIsInScene()
	{
		var autoManagers = GameObject.FindObjectsOfType<AutoAttributeManager>();
		if (autoManagers == null || autoManagers.Length == 0)
		{
			GameObject autoGo = new GameObject("Auto_Attribute_Manager");
			autoGo.AddComponent<AutoAttributeManager>();
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
		else if (autoManagers.Length >=2)
		{
			for(int i=1; i<autoManagers.Length; i++)
			{
				GameObject.DestroyImmediate(autoManagers[i]);
			}
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
	}

	public static string[] OnWillSaveAssets(string[] paths)
	{
		MakeSureAutoManagerIsInScene();
		return paths;
	}
}
