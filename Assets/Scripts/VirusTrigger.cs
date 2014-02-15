using UnityEngine;
using System.Collections;

public class VirusTrigger : MonoBehaviour {

	public GameObject virusPrefab;

	public float spawnInterval;
	public float rndSpawnIntervalFactor;

	private bool running = true;

	public void StartTriggering () {
		StartCoroutine(DoSpawnViruses());

		GameTime.Instance.OnGameEnded += OnGameEnded;
	}
	
	private IEnumerator DoSpawnViruses()
	{
		while(running)
		{
			yield return new WaitForSeconds(Random.Range(spawnInterval/rndSpawnIntervalFactor, 
			                                             spawnInterval*rndSpawnIntervalFactor));

			GameObject file = Instantiate(virusPrefab) as GameObject;
			file.GetComponent<Virus>().Target = GetComponent<FileSender>().connections[Random.Range(0, 
			                                                 GetComponent<FileSender>().connections.Count)];
			file.transform.position = transform.position;
		}
	}

	private void OnGameEnded()
	{
		running = false;
		StopAllCoroutines();
	}
}
