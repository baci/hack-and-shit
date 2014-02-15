using UnityEngine;
using System.Collections;

public class PlayerCornerNode : NetworkNode {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void RecieveFile(File aFile, NetworkNode aFromNode)
    {
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
        Destroy(aFile.gameObject);
        transform.parent.GetComponent<PlayerCorner>().AddScore(1);
    }
}
