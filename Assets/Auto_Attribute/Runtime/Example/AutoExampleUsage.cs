using UnityEngine;


public class AutoExampleUsage : MonoBehaviour {

	#region Single component Getters
	[Auto] public AutoExampleUsage mySelf; //Same as GetComponent<TestTesta>() done in Awake 
	[AutoParent] public AudioSource audioSource;	//Same as GetComponentInParent<AudioSource>() done in Awake
	[AutoChildren] public Collider firstCollider;	//Same as GetComponentInChildren<Collider>() done in Awake
	#endregion
	
	#region Multiple component Getters
	[AutoChildren] public Collider[] allChildrenColliders_Arr; //Same as GetComponentsInChildren<Collider>() done in Awake 
	#endregion
	
	#region Optional Parameters - Logging
	[Auto(true)] private CanvasGroup canvas1;	//Passing true as parameter will log an error if the component was not found. This is the default, and builds will be halted with Auto is unsuccessful
	[Auto(false)] public CanvasGroup canvas2;		//Passing false as parameter will log a warning if the component was not found. Builds will not be halted by Auto
	#endregion

	void Awake () {
		firstCollider.isTrigger = true;	//The variable is already referenced when awake starts

	}

	
	
}
