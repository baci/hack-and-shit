using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public static GameTime Instance;

	public System.Action OnGameEnded;

	private Game game;
	public float totalTime = 60f;
	public float timePassed;

	public float totalFileDestroyTime = 1f;
	private bool endedAlready = false;

	void Awake(){
		Instance = this;
	}


	void Start(){
		game = Game.Instance;
	}

	void Update(){
		if(game.GameState == Game.State.INGAME)
		{
			timePassed += Time.deltaTime;
			if(timePassed >= totalTime && !endedAlready){
				endedAlready = true;
				StartCoroutine(EndGame());
			}
		}

        if (timePassed >= (totalTime + totalFileDestroyTime +1.0f) && Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Application.LoadLevel(0);
                    return;
                }
            }
        }
	}

	public float GetTimePercentage(){
		return timePassed/totalTime;
	}

	private IEnumerator EndGame(){
		print ("wait for it...");
		int bestPlayer = game.GetWinner();

		if(OnGameEnded != null) OnGameEnded();

        iTween.CameraFadeAdd();
        iTween.CameraFadeTo(0.4f, totalFileDestroyTime*0.5f);

		yield return new WaitForSeconds(totalFileDestroyTime);

		Game.Instance.ChangeState(Game.State.ENDGAME);

		print ("Player " + bestPlayer + " won!");

		yield return new WaitForSeconds(5f);

		Application.LoadLevel(0);
	}
}
