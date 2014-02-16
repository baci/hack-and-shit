using UnityEngine;
using System.Collections;

public class ChangeAllTheShits : MonoBehaviour {

	public float changeToSpeed = -5f;
	public float changeToLifetime = 1.8f;

	[ContextMenu("Update Values")]
	void UpdateVals(){
		ParticleSystem ps = GetComponent<ParticleSystem>();

		ps.startSpeed = changeToSpeed;
		ps.startLifetime = changeToLifetime;
	}
}
