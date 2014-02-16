using UnityEngine;
using System.Collections;

public class CreditsScreen : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        GetComponent<Touchable>().onTouchBegin += OnTouchDown;
	}
	
	// Update is called once per frame
	void Update () 
    {

    }

    private void OnTouchDown(Touch aTouch, Vector3 aHitPos)
    {
        StartCoroutine(DoFadeout(Game.State.TITLE));
    }

    private IEnumerator DoFadeout(Game.State aState)
    {
        iTween.ColorTo(gameObject, new Color(1, 1, 1, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        Game.Instance.ChangeState(aState);
        gameObject.SetActive(false);
    }
}
