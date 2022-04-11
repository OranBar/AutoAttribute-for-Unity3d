using System.Collections;
using UnityEngine;


public class AutoSingleTestHelper : MonoBehaviour {

	[Auto] public MeshRenderer pub_meshRenderer;
	[Auto] private MeshRenderer priv_meshRenderer;
	[AutoChildren] public MeshRenderer[] pub_colldier_children;
	[AutoChildren] private MeshRenderer[] priv_colldier_children;

	[AutoParent] public AudioSource[] pub_audio_children;
	[AutoParent] private AudioSource[] priv_audio_children;
	public MeshRenderer PrivMeshRenderer => priv_meshRenderer;
	public AudioSource[] PrivAudioChildren => priv_audio_children;
	
}


