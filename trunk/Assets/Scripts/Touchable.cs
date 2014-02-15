using UnityEngine;
using System.Collections;

public class Touchable : MonoBehaviour {

    public delegate void OnTouch(Touch aTouch);
    public delegate void OnTouchHitPos(Touch aTouch, Vector3 aHitPos);
    public OnTouchHitPos onTouchBegin;
    public OnTouch onTouchEnd;
    public OnTouch onTouchMove;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTouchDown(Touch aTouch, Vector3 aHitPos)
    {
        if (onTouchBegin != null)
            onTouchBegin(aTouch, aHitPos);
    }

    public void OnTouchMove(Touch aTouch)
    {
        if (onTouchMove != null)
            onTouchMove(aTouch);
    }

    public void OnTouchEnd(Touch aTouch)
    {
        if (onTouchEnd != null)
            onTouchEnd(aTouch);
    }
}
