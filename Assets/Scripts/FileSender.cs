using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSender : MonoBehaviour 
{
    public GameObject filePrefab;

    public List<NetworkNode> connections;

    float mFileSendTime = 1.0f;

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
            GameObject file = Instantiate(filePrefab) as GameObject;
            file.GetComponent<File>().Target = connections[Random.Range(0, connections.Count)];
            file.transform.position = transform.position;
            mFileSendTime = 0.5f;
        }
	}
}
