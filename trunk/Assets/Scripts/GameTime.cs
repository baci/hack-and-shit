using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public static GameTime Instance;

	public System.Action OnGameEnded;

	public GameObject fadeTexture;
	public TextMesh winningText;

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

		switch(bestPlayer)
		{
		case -1:
			winningText.text = "no matter who vince...";
			winningText.color = Color.white;
			break;
		case 0:
			winningText.text = "Google wins!";
			winningText.color = Color.magenta;
			break;
		case 1:
			winningText.text = "NSA wins!";
			winningText.color = Color.green;
			break;
		case 2:
			winningText.text = "Facebook wins!";
			winningText.color = Color.cyan;
			break;
		case 3: 
			winningText.text = "Microsoft wins!";
			winningText.color = Color.yellow;
			break;
		}

		iTween.ColorTo(fadeTexture, new Color(0,0,0,0.8f), totalFileDestroyTime);
		iTween.ColorTo(Game.Instance.winningSprite.gameObject, new Color(1,1,1,1), totalFileDestroyTime);
		//iTween.ColorTo(winningText.gameObject, new Color(1,1,1,1), totalFileDestroyTime);

		yield return new WaitForSeconds(totalFileDestroyTime);

		Game.Instance.ChangeState(Game.State.ENDGAME);

		yield return new WaitForSeconds(5f);

		Application.LoadLevel(0);
	}
}
