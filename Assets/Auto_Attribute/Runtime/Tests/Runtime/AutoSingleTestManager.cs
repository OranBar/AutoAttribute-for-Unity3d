using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSingleTestManager : MonoBehaviour
{
	public AutoSingleTestHelper activeWithAuto, inactiveWithAuto;
	public AutoSingleTestHelper_Child activeWithAuto_inherited, inactiveWithAuto_inherited;
	

	public void Awake() {
		Debug.Assert(activeWithAuto.pub_meshRenderer != null, "Auto on active go, public component not working");
		Debug.Assert(activeWithAuto.PrivMeshRenderer != null, "Auto on active go, private component not working");

		Debug.Assert(inactiveWithAuto.pub_meshRenderer != null, "Auto on inactive go, public component not working");
		Debug.Assert(inactiveWithAuto.PrivMeshRenderer != null, "Auto on inactive go, private component not working");
		// Debug.Assert(pub_meshRenderer_list != null, "Auto on public component list not working");
		// Debug.Assert(priv_meshRenderer_list != null, "Auto on private component list not working");
		Debug.Assert(activeWithAuto_inherited.pub_meshRenderer != null, "Auto inherited on active go, public component not working");
		Debug.Assert(activeWithAuto_inherited.PrivMeshRenderer != null, "Auto inherited on active go, private component not working");

		Debug.Assert(inactiveWithAuto_inherited.pub_meshRenderer != null, "Auto inherited on inactive go, public component not working");
		Debug.Assert(inactiveWithAuto_inherited.PrivMeshRenderer != null, "Auto inherited on inactive go, private component not working");
	}
}
