using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSender : MonoBehaviour 
{
    public GameObject filePrefab;

    public List<NetworkNode> connections;

    float mFileSendTime = 1.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
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
