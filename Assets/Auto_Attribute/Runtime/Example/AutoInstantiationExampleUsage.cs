using UnityEngine;

public class AutoInstantiationExampleUsage : MonoBehaviour
{
    [Auto] public Collider myCollider;	//This auto will be set by the AutoReferencerOnInstantiation script

    void Awake()
    {
		// If this gameobject has a AutoReferencerOnInstantiation component, the variable myCollider will be set before this code is executed.
		// else, it will throw a NullReferenceException, since Auto only scans the scene when the application is first started.
		myCollider.isTrigger = true;	
    }

}
