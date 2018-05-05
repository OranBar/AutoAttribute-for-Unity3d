/* Author: Oran Bar
 * Summary: If the instantiated object has this script prior to its instantiation, auto will reference all variables of attached components correctly.
 * The alsoReferenceChildren boolean will determine if the referencing has to be done recursively to all its children, or only on this gameobject.
 */

using UnityEngine;
using Auto.Utils;

[ScriptTiming(-500)]
public class AutoReferencerOnInstantiation : MonoBehaviour {

    public bool alsoReferenceChildren = true;

	void Awake() 
    {
        AutoAttributeManager.AutoReference(this.gameObject);

        if(alsoReferenceChildren)
        {
            foreach(Transform child in this.transform)
            {
                AutoAttributeManager.AutoReference(child.gameObject);
            }
        }
	}

    private void RecursivelyReferenceChildren(GameObject go)
    {
        AutoAttributeManager.AutoReference(go);

        foreach(Transform child in this.transform)
        {
            RecursivelyReferenceChildren(child.gameObject);  
        }
    }

}
