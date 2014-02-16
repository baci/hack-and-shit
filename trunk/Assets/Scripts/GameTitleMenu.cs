using UnityEngine;
using System.Collections;

public class GameTitleMenu : MonoBehaviour
{
    public GameObject creditsButton;

	private void Start()
	{
        GetComponent<Touchable>().onTouchBegin += OnTouchDown;
        creditsButton.GetComponent<Touchable>().onTouchBegin += OnCreditsButtonDown;
	}


	private void OnTouchDown(Touch aTouch, Vector3 aHitPos)
	{
        StartCoroutine(DoFadeout(Game.State.INGAME));
	}


    private void OnCreditsButtonDown(Touch aTouch, Vector3 aHitPos)
    {
        StartCoroutine(DoFadeout(Game.State.CREDITS));
    }

	private IEnumerator DoFadeout(Game.State aState)
	{
        iTween.ColorTo(gameObject, new Color(1, 1, 1, 0), 0.5f);
        iTween.ColorTo(creditsButton, new Color(1, 1, 1, 0), 0.5f);
		yield return new WaitForSeconds(0.5f);
        Game.Instance.ChangeState(aState);
        gameObject.SetActive(false);
        creditsButton.SetActive(false);
	}
}

