using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSender : MonoBehaviour 
{
    public GameObject filePrefab;
    public GameObject virusPrefab;

    public List<NetworkNode> connections;

    public float fileSentInterval=1;
    float mFileSendTime = 1;
    public int virusProbability = 10;

	public bool sendFiles = true; 

	// Use this for initialization
	void Start () 
    {
		GameTime.Instance.OnGameEnded += OnGameEnded;
	}

	void OnDestroy(){
		GameTime.Instance.OnGameEnded -= OnGameEnded;
	}

	void OnGameEnded(){
		sendFiles = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(!sendFiles) return;

        mFileSendTime -= Time.deltaTime;

        if (mFileSendTime <= 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                GameObject file = Instantiate(Random.Range(0, virusProbability)==0 ? virusPrefab : filePrefab) as GameObject;
                file.GetComponent<File>().Target = connections[i];
                file.transform.position = transform.position;
                mFileSendTime = fileSentInterval;
            }
        }
	}
}
