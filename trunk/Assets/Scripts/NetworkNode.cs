using UnityEngine;
using System.Collections;

public abstract class NetworkNode : MonoBehaviour 
{
    protected enum ConnectionDir
    {
        up,
        down,
        left,
        right,
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    abstract public void RecieveFile(File aFile, NetworkNode aFromNode);
    abstract public void HandleFile(File aFile, NetworkNode aFromNode);
}
