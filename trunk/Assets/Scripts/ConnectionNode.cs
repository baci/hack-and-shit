using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionNode : NetworkNode
{
    public enum ConnectionNodeType
    {
        corner,
        straight,
        fork,
    }

	public AudioClip rotateAudio;

    public List<NetworkNode> connections = null;

    public ConnectionNodeType type;
    public int rotation = 0;

	// Use this for initialization
	protected override void Start () 
    {
		base.Start();

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
        bool passThrough = false;
        if (type == ConnectionNodeType.corner)
            passThrough = PassThrough_Corner(aFile, aFromNode);
        else if (type == ConnectionNodeType.straight)
            passThrough = PassThrough_Straight(aFile, aFromNode);
        else if (type == ConnectionNodeType.fork)
            passThrough = PassThrough_Fork(aFile, aFromNode);

        if (!passThrough)
            aFile.DestroyJuicy(false);
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
        ConnectionDir connectionDir = ConnectionDir.down;
        if (type == ConnectionNodeType.corner)
            connectionDir = GetConnection_Corner(aFile, aFromNode);
        else if (type == ConnectionNodeType.straight)
            connectionDir = GetConnection_Straight(aFile, aFromNode);
        else if (type == ConnectionNodeType.fork)
            connectionDir = GetConnection_Fork(aFile, aFromNode);

        aFile.Target = connections[(int)connectionDir];

        if (aFile is Virus && (aFile as Virus).DirectionReversed)
        {
            NetworkNode tmp = aFile.Target;
            aFile.Target = aFromNode;
            aFromNode = tmp;
        }

        if (aFile.Target == null)
        {
            if (connectionDir == ConnectionDir.down) aFile.TargetDeathPos = transform.position + Vector3.down * 2;
            if (connectionDir == ConnectionDir.up) aFile.TargetDeathPos = transform.position + Vector3.up * 2;
            if (connectionDir == ConnectionDir.right) aFile.TargetDeathPos = transform.position + Vector3.right * 2;
            if (connectionDir == ConnectionDir.left) aFile.TargetDeathPos = transform.position + Vector3.left * 2;
        }
    }

    public void SetRotation(int aRot, bool aInstant = false)
    {
        Debug.Log("rotation " + (aRot*90));
        rotation = aRot;

        if (aInstant)
            transform.rotation = Quaternion.Euler(0, 0, aRot * 90);
        else{
			Hashtable args = new Hashtable(){
				{"rotation", new Vector3(0, 0, rotation * 90)},
				{"time", 0.3f},
				{"easetype", "easeInOutCubic"},
				{"delay", 0.05f}
			};
            iTween.RotateTo(gameObject, args);
			args = new Hashtable(){
				{"scale", Vector3.one*1.2f},
				{"time", 0.1f},
				{"easetype", "easeOutCubic"}
			};
			iTween.ScaleTo(gameObject, args);
			args = new Hashtable(){
				{"scale", Vector3.one},
				{"time", 0.1f},
				{"delay", 0.3f},
				{"easetype", "easeInCubic"}
			};
			iTween.ScaleTo(gameObject, args);

			AudioController.instance.PlaySfx(rotateAudio);
		}
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

    // -----------------------------
    ///////////////////////////////
    // SHAPES
    ///////////////////////////////
    // -----------------------------

    private bool PassThrough_Corner(File aFile, NetworkNode aFromNode)
    {
        if (rotation == 0 && aFromNode != connections[(int)ConnectionDir.right] &&
            aFromNode != connections[(int)ConnectionDir.down])
            return false;

        else if (rotation == 1 && aFromNode != connections[(int)ConnectionDir.left] &&
            aFromNode != connections[(int)ConnectionDir.down])
            return false;

        else if (rotation == 2 && aFromNode != connections[(int)ConnectionDir.left] &&
            aFromNode != connections[(int)ConnectionDir.up])
            return false;

        else if (rotation == 3 && aFromNode != connections[(int)ConnectionDir.right] &&
            aFromNode != connections[(int)ConnectionDir.up])
            return false;

        return true;
    }

    private ConnectionDir GetConnection_Corner(File aFile, NetworkNode aFromNode)
    {
        switch (rotation)
        {
            case 0:
				if (aFromNode == connections[(int)ConnectionDir.right]) return ConnectionDir.down;
                else return ConnectionDir.right;
            case 1:
                if (aFromNode == connections[(int)ConnectionDir.down]) return ConnectionDir.left;
                else return ConnectionDir.down;
            case 2:
                if (aFromNode == connections[(int)ConnectionDir.left]) return ConnectionDir.up;
                else return ConnectionDir.left;
            default:
                if (aFromNode == connections[(int)ConnectionDir.up]) return ConnectionDir.right;
                else return ConnectionDir.up;
        }
    }

    private bool PassThrough_Straight(File aFile, NetworkNode aFromNode)
    {
        if ((rotation == 0 || rotation == 2) && (aFromNode == connections[(int)ConnectionDir.left] ||
            aFromNode == connections[(int)ConnectionDir.right]))
            return false;

        else if ((rotation == 1 || rotation == 3) && (aFromNode == connections[(int)ConnectionDir.up] ||
            aFromNode == connections[(int)ConnectionDir.down]))
            return false;

        return true;
    }

    private ConnectionDir GetConnection_Straight(File aFile, NetworkNode aFromNode)
    {
        if (rotation == 0 || rotation == 2)
        {
            if (aFromNode == connections[(int)ConnectionDir.up]) return ConnectionDir.down;
            else return ConnectionDir.up;
        }
        else
        {
            if (aFromNode == connections[(int)ConnectionDir.left]) return ConnectionDir.right;
            else return ConnectionDir.left;
        }
    }

    private bool PassThrough_Fork(File aFile, NetworkNode aFromNode)
    {
        if (aFromNode == null)
            return true;

        if (rotation == 0 && aFromNode != connections[(int)ConnectionDir.up])
            return true;

        else if (rotation == 1 && aFromNode != connections[(int)ConnectionDir.right])
            return true;

        else if (rotation == 2 && aFromNode != connections[(int)ConnectionDir.down])
            return true;

        else if (rotation == 3 && aFromNode != connections[(int)ConnectionDir.left])
            return true;

        return false;
    }

    private ConnectionDir GetConnection_Fork(File aFile, NetworkNode aFromNode)
    {
        if (rotation == 0 || rotation == 2)
        {
            if (aFromNode == connections[(int)ConnectionDir.left]) return ConnectionDir.right;
            else if (aFromNode == connections[(int)ConnectionDir.right]) return ConnectionDir.left;
            else
            {
                int rand = Random.Range(0, 2);
                if (rand == 0) return ConnectionDir.right;
                else           return ConnectionDir.left;
            }
        }
        else
        {
            if (aFromNode == connections[(int)ConnectionDir.up]) return ConnectionDir.down;
            else if (aFromNode == connections[(int)ConnectionDir.down]) return ConnectionDir.up;
            else
            {
                int rand = Random.Range(0, 2);
                if (rand == 0) return ConnectionDir.up;
                else           return ConnectionDir.down;
            }
        }
    }
}
