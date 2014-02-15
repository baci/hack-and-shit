using UnityEngine;
using System.Collections;

public class VirusTrigger : MonoBehaviour {

	public Virus virusPrefab;

	public float spawnInterval;
	public float rndSpawnIntervalFactor;

	private void Start () {
		StartCoroutine(DoSpawnViruses());
	}
	
	private IEnumerator DoSpawnViruses()
	{
		while(true)
		{
			yield return new WaitForSeconds(Random.Range(spawnInterval/rndSpawnIntervalFactor, 
			                                             spawnInterval*rndSpawnIntervalFactor));

			GameObject file = Instantiate(virusPrefab) as GameObject;
			file.GetComponent<Virus>().Target = GetComponent<FileSender>().connections[Random.Range(0, 
			                                                 GetComponent<FileSender>().connections.Count)];
			file.transform.position = transform.position;
		}
	}
}
