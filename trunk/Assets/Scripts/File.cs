using UnityEngine;
using System.Collections;

public class File : MonoBehaviour 
{
    public NetworkNode Target { get; set; }

    private NetworkNode mPrevNode;

	protected float timeSinceCreated = 0f;
	protected static float largestTimeSinceCreated = 0f;

	protected virtual void Start () 
    {
		GameTime.Instance.OnGameEnded += OnGameEnded;
	}

	protected virtual void OnDestroy(){
		GameTime.Instance.OnGameEnded -= OnGameEnded;
	}
	
	protected virtual void Update () 
    {
		timeSinceCreated += Time.deltaTime;
		if(timeSinceCreated > largestTimeSinceCreated) largestTimeSinceCreated = timeSinceCreated;

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

	public void OnGameEnded(){

		StartCoroutine(EndGameRoutine());
	}

	public IEnumerator EndGameRoutine(){
		float t = GameTime.Instance.totalFileDestroyTime;
		t = (timeSinceCreated/largestTimeSinceCreated) * t;

		yield return new WaitForSeconds(t);

		DestroyJuicy();
	}
}
