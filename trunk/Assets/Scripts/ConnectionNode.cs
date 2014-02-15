using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionNode : NetworkNode 
{
    public List<NetworkNode> connections = null;

    public int rotation = 0;

	// Use this for initialization
	void Start () 
    {
        GetComponent<Touchable>().onTouchBegin += OnTouchBegin;
        GetComponent<Touchable>().onTouchMove += OnTouchMove;
        GetComponent<Touchable>().onTouchEnd += OnTouchEnd;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (connections.Count == 0)
        {
            AddConnection(Vector3.up);
            AddConnection(Vector3.down);
            AddConnection(Vector3.right);
            AddConnection(Vector3.left);
        }
	}

    private void AddConnection(Vector3 aDir)
    {
        RaycastHit info;
        Ray ray = new Ray(transform.position + aDir * 0.5f, aDir);
        if (Physics.Raycast(ray, out info) && info.transform.GetComponent<NetworkNode>() != null)
        {
            connections.Add(info.transform.GetComponent<NetworkNode>());
        }
        else
        {
            connections.Add(null);
        }
    }

    override public void RecieveFile(File aFile, NetworkNode aFromNode)
    {
        bool bDestroy = false;
        if (rotation == 0 && aFromNode != connections[(int)ConnectionDir.right] && 
            aFromNode != connections[(int)ConnectionDir.down])
            bDestroy = true;

        else if (rotation == 1 && aFromNode != connections[(int)ConnectionDir.left] && 
            aFromNode != connections[(int)ConnectionDir.down])
            bDestroy = true;

        else if (rotation == 2 && aFromNode != connections[(int)ConnectionDir.left] &&
            aFromNode != connections[(int)ConnectionDir.up])
            bDestroy = true;

        else if (rotation == 3 && aFromNode != connections[(int)ConnectionDir.right] &&
            aFromNode != connections[(int)ConnectionDir.up])
            bDestroy = true;

        if (bDestroy)
            aFile.DestroyJuicy();
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
        switch (rotation)
        {
            case 0:
                if (aFromNode == connections[(int)ConnectionDir.right]) aFile.Target = connections[(int)ConnectionDir.down];
                else aFile.Target = connections[(int)ConnectionDir.right];
                break;
            case 1:
                if (aFromNode == connections[(int)ConnectionDir.down]) aFile.Target = connections[(int)ConnectionDir.left];
                else aFile.Target = connections[(int)ConnectionDir.down];
                break;
            case 2:
                if (aFromNode == connections[(int)ConnectionDir.left]) aFile.Target = connections[(int)ConnectionDir.up];
                else aFile.Target = connections[(int)ConnectionDir.left];
                break;
            case 3:
                if (aFromNode == connections[(int)ConnectionDir.up]) aFile.Target = connections[(int)ConnectionDir.right];
                else aFile.Target = connections[(int)ConnectionDir.up];
                break;
        }
    }

    public void SetRotation(int aRot)
    {
        Debug.Log("rotation " + rotation);
        rotation = aRot;
        iTween.RotateTo(gameObject, new Vector3(0,0,rotation * 90), 0.3f);
    }

    Vector3 touchPoint;
    public void OnTouchBegin(Touch aTouch, Vector3 aHitPos)
    {
        var pos = (Camera.main.ScreenToWorldPoint(aTouch.position));
        pos.z = 0;
        touchPoint = pos;

        int newRot = rotation + 1;
        if (newRot > 3) newRot = 0;
        SetRotation(newRot);
    }

    public void OnTouchMove(Touch aTouch)
    {
        //var pos = (Camera.main.ScreenToWorldPoint(aTouch.position));
        //pos.z = 0;

        //Vector3 prevRel = touchPoint - transform.position;
        //Vector3 newRel = pos - transform.position;
        //float angle = Vector3.Angle(prevRel, newRel);

        ////Debug.Log("Delta: " + angle);

        //transform.Rotate(new Vector3(0, 0, angle));

        //touchPoint = aTouch.position;
    }

    public void OnTouchEnd(Touch aTouch)
    {
        //var pos = (Camera.main.ScreenToWorldPoint(aTouch.position));
        //pos.z = 0;

        //Vector3 prevRel = touchPoint - transform.position;
        //Vector3 newRel = pos - transform.position;
        //float angle = Vector3.Angle(newRel, prevRel);

        //Debug.Log("Delta: " + angle);

        ////transform.Rotate(new Vector3(0, 0, angle));

        ////touchPoint = aTouch.position;
    }
}
