using UnityEngine;
using UnityEngine.UI;

public class NoAutoExample : MonoBehaviour {
	
	private Collider col; 
	private Rigidbody rb;
	private Button button;

	// With Auto, this whole Method is not needed.
	void Awake () {
		col = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		button = GetComponentInChildren<Button>();
	}
	//----------------------------------------------

	void Start(){
		//Do stuff with our variables
	}
	
}
