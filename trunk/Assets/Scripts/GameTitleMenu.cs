using UnityEngine;
using System.Collections;

public class GameTitleMenu : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Touchable>().onTouchBegin += OnTouchDown;
	}


	private void OnTouchDown(Touch aTouch, Vector3 aHitPos)
	{
		GetComponent<Touchable>().onTouchBegin -= OnTouchDown;
		StartCoroutine(DoFadeout());
	}

	private IEnumerator DoFadeout()
	{
		iTween.ColorTo(gameObject, new Color(1,1,1,0), 0.5f);
		yield return new WaitForSeconds(0.5f);
		Game.Instance.state = Game.State.INGAME;
		gameObject.SetActive(false);
	}
}

