using UnityEngine;
using System.Collections;

public class File : MonoBehaviour 
{
    public NetworkNode Target { get; set; }

    private NetworkNode mPrevNode;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Target == null) return;
        Vector3 delta = Target.transform.position-transform.position;

        if (delta.magnitude < 1)
        {
            Target.RecieveFile(this, mPrevNode);
        }
        if (delta.magnitude < Time.deltaTime)
        {
            var oldTarget = Target;
            Target.HandleFile(this, mPrevNode);
            if (Target != oldTarget) mPrevNode = oldTarget;
        }

        delta.Normalize();

        transform.Translate(delta * Time.deltaTime);
	}

    public void DestroyJuicy()
    {
        Destroy(gameObject);
    }
}
