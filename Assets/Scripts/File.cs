using UnityEngine;
using System.Collections;

public class File : MonoBehaviour 
{
    public NetworkNode Target { get; set; }

    private NetworkNode mPrevNode;

	protected virtual void Start () 
    {
	
	}
	
	protected virtual void Update () 
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

    public virtual void DestroyJuicy()
    {
        Destroy(gameObject);
    }
}
