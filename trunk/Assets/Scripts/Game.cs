using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
    public static Game Instance;

    public int[] playerScores = { 0, 0, 0, 0 };
    public TextMesh[] playerScoreText;

	public Vector2 BoardSize = new Vector2(9, 5);

    public GameObject corner90prefab;
    public GameObject straightPrefab;
    public GameObject forkPrefab;

    public GameTitleMenu titleMenu;
    public CreditsScreen creditsScreen;
	public GameObject winningSprite;

	public FileSender fileSender;

	public enum State
	{
		TITLE,
		INGAME,
		ENDGAME,
        CREDITS,
	}
	private State state;
	public State GameState {get{return state;}}

	// Use this for initialization
	void Awake () {
        Instance = this;
		ChangeState(State.TITLE);
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < playerScoreText.Length; i++)
        {
            playerScoreText[i].text = "" + playerScores[i];
        }
	}

	public void ChangeState(State newState)
	{
		switch(newState)
		{
		case State.ENDGAME:
			break;
		case State.INGAME:
			fileSender.sendFiles = true;
            foreach (var text in playerScoreText) text.gameObject.SetActive(true);
			break;
		case State.TITLE:
			titleMenu.gameObject.SetActive(true);
            titleMenu.creditsButton.SetActive(true);
            titleMenu.loadMapButton.SetActive(true);
			iTween.ColorTo(titleMenu.gameObject, new Color(1,1,1,1), 0.5f);
            iTween.ColorTo(titleMenu.creditsButton, new Color(1,1,1,1), 0.5f);
            iTween.ColorTo(titleMenu.loadMapButton, new Color(1,1,1,1), 0.5f);
			fileSender.sendFiles = false;
            break;
        case State.CREDITS:
            creditsScreen.gameObject.SetActive(true);
            creditsScreen.gameObject.renderer.material.color = new Color(1, 1, 1, 1);
            fileSender.sendFiles = false;
            break;
		}
		state = newState;
	}

	public int GetWinner(){
		int result = 0;
		int best = -1;
		for(int i = 0; i < playerScores.Length; i++){
			if(playerScores[i]> result){
				best = i;
				result = playerScores[i];
			} else if (playerScores[i] == result) {
				best = -1;
			}
		}

		return best;
	}
}
