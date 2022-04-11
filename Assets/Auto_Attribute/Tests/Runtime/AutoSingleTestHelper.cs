using System.Collections;
using UnityEngine;

public class AutoSingleTestHelper : MonoBehaviour {

	[Auto] public MeshRenderer pub_meshRenderer;
	[Auto] private MeshRenderer priv_meshRenderer;
	public MeshRenderer PrivMeshRenderer => priv_meshRenderer;

	// [Auto] public MeshRenderer[] pub_meshRenderer_list;
	// [Auto] private MeshRenderer[] priv_meshRenderer_list;
	
}


