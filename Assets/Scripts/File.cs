using UnityEngine;
using System.Collections;

public class File : MonoBehaviour 
{
    public NetworkNode Target { get; set; }

    protected NetworkNode mPrevNode;

	protected float timeSinceCreated = 0f;
	protected NetworkNode curNode;
	protected static float largestTimeSinceCreated = 0f;
	
	public AudioClip scoreSfx;
	public AudioClip destroySfx;
	public GameObject scoreParticles;
	public GameObject destroyParticles;


	protected virtual void Start () 
    {
	
		GameTime.Instance.OnGameEnded += OnGameEnded;
	}

	protected virtual void OnDestroy(){
		GameTime.Instance.OnGameEnded -= OnGameEnded;
	}

	//this exists so the update is overriden with no issues
	//needed for endgame effect
	protected virtual void LateUpdate(){
		timeSinceCreated += Time.deltaTime;
		if(timeSinceCreated > largestTimeSinceCreated) largestTimeSinceCreated = timeSinceCreated;
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

    public virtual void DestroyJuicy(bool willGivePoints = true)
	{
		GameObject part;
		if(willGivePoints){
			AudioController.instance.PlaySfx(scoreSfx);
			if(scoreParticles){
				part = Instantiate(scoreParticles) as GameObject;
				part.transform.position = transform.position;
				Destroy(part, 5f);
			}
		} else {
			AudioController.instance.PlaySfx(destroySfx);
			if(destroyParticles){
				part = Instantiate(destroyParticles) as GameObject;
				part.transform.position = transform.position;
				Destroy(part, 5f);
			}
		}
        Destroy(gameObject);
    }

	public void OnGameEnded(){

		StartCoroutine(EndGameRoutine());
	}

	public IEnumerator EndGameRoutine(){
		float t = GameTime.Instance.totalFileDestroyTime;
		t = (timeSinceCreated/largestTimeSinceCreated) * t;

		yield return new WaitForSeconds(t);

		DestroyJuicy(false);
	}
}
