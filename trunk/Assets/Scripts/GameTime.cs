using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public static GameTime Instance;

	public System.Action OnGameEnded;

	public AudioClip endGame;

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

		AudioController.instance.PlaySfx(endGame);

		int bestScore = Game.Instance.playerScores[bestPlayer];

		switch(bestPlayer)
		{
		case -1:
			winningText.text =  "No monopoly on data was estabilished.";
			winningText.color = Color.white;
			break;
		case 0:
			winningText.text = bestScore + " billion lives were analyzed by your algorythms!";
			winningText.color = Color.cyan;
			break;
		case 1:
			winningText.text = "The wereabouts of " + bestScore + " billion people are in your database!";
			winningText.color = Color.yellow;
			break;
		case 2:
			winningText.text = "Over " + bestScore*100 + " embassies were spied upon!";
			winningText.color = Color.green;
			break;
		case 3: 
			winningText.text = bestScore + " billion targeted ads sold!";
			winningText.color = Color.magenta;
			break;
		}
		winningText.color = new Color(winningText.color.r,winningText.color.g,winningText.color.b,0);
		iTween.ColorTo(fadeTexture, new Color(0,0,0,0.8f), totalFileDestroyTime);
		iTween.ColorTo(Game.Instance.winningSprite.gameObject, new Color(1,1,1,1), totalFileDestroyTime);
		/*iTween.ColorTo(winningText.gameObject, new Color(winningText.color.r,winningText.color.g,winningText.color.b,1), 
		               totalFileDestroyTime);*/

		yield return new WaitForSeconds(totalFileDestroyTime);

		winningText.color = new Color(winningText.color.r,winningText.color.g,winningText.color.b,1);

		Game.Instance.ChangeState(Game.State.ENDGAME);

		yield return new WaitForSeconds(5f);

		Application.LoadLevel(0);
	}
}
