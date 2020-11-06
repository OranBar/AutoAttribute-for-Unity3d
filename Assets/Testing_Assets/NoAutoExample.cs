//#define [GetComponent] [Auto]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAutoExample : MonoBehaviour {

	
	private Collider col; 
	private Rigidbody rb;
	private Button button;

	void Awake () {
		col = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		button = GetComponentInChildren<Button>();
	}

	void Start(){
		//Do stuff with our variables
	}
	
}
