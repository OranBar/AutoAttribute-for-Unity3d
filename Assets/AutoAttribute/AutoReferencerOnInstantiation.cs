using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auto.Utils;

[ScriptTiming(-990)]
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
