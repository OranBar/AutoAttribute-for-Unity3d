using UnityEngine;

public class Example_InstantiateObjectWithAutos : MonoBehaviour
{
	public GameObject prefabWithComponentsUsingAuto;

    void Start()
    {
		GameObject.Instantiate(prefabWithComponentsUsingAuto);
    }

    
}
