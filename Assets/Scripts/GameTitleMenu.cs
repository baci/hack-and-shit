using UnityEngine;
using System.Collections;

public class GameTitleMenu : MonoBehaviour
{
    public GameObject creditsButton;
    public GameObject loadMapButton;
    public int levelToLoad = 1;

	private void Start()
	{
        GetComponent<Touchable>().onTouchBegin += OnTouchDown;
        creditsButton.GetComponent<Touchable>().onTouchBegin += OnCreditsButtonDown;
        loadMapButton.GetComponent<Touchable>().onTouchBegin += OnLoadMapButtonDown;
	}


	private void OnTouchDown(Touch aTouch, Vector3 aHitPos)
	{
        StartCoroutine(DoFadeout(Game.State.INGAME));
	}


    private void OnCreditsButtonDown(Touch aTouch, Vector3 aHitPos)
    {
        StartCoroutine(DoFadeout(Game.State.CREDITS));
    }

    private void OnLoadMapButtonDown(Touch aTouch, Vector3 aHitPos)
    {
        Application.LoadLevel(levelToLoad);
    }

	private IEnumerator DoFadeout(Game.State aState)
	{
        iTween.ColorTo(gameObject, new Color(1, 1, 1, 0), 0.5f);
        iTween.ColorTo(creditsButton, new Color(1, 1, 1, 0), 0.5f);
        iTween.ColorTo(loadMapButton, new Color(1, 1, 1, 0), 0.5f);
		yield return new WaitForSeconds(0.5f);
        Game.Instance.ChangeState(aState);
        gameObject.SetActive(false);
        creditsButton.SetActive(false);
        loadMapButton.SetActive(false);
	}
}

