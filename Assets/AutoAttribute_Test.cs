using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterface
{

}

public class AutoAttribute_Test : MonoBehaviour, IInterface
{
	// [Auto] private AutoAttribute_Test myself;
	[Auto] private IInterface myself_asInterface;

	public bool spawn = false;

	void Start()
	{
		// Debug.Assert(myself != null);
		Debug.Assert(myself_asInterface != null);

		myself_asInterface = gameObject.GetComponent<IInterface>();
		Debug.Assert(myself_asInterface != null);

		if(spawn == false){
			return;
		}
		var componentAdded = this.AddComponent_Auto<AutoAttribute_Test>();
		Debug.Assert(componentAdded.myself_asInterface != null);

	}
}