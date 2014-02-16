using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
    public static Game Instance;

    public int[] playerScores = { 0, 0, 0, 0 };
    public GUIText[] playerScoreText;

    public Vector2 BoardSize = new Vector2(9, 5);

    public GameObject corner90prefab;
    public GameObject straightPrefab;
    public GameObject forkPrefab;

	public GameTitleMenu titleMenu;
	public GameObject winningSprite;

	public FileSender fileSender;
	public VirusTrigger virusTrigger;

	public enum State
	{
		TITLE,
		INGAME,
		ENDGAME
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
        playerScoreText[0].text = "" + playerScores[0];
        playerScoreText[1].text = "" + playerScores[1];
        playerScoreText[2].text = "" + playerScores[2];
        playerScoreText[3].text = "" + playerScores[3];
	}

	public void ChangeState(State newState)
	{
		switch(newState)
		{
		case State.ENDGAME:
			titleMenu.gameObject.SetActive(false);
			if(winningSprite)
				winningSprite.SetActive(true);
			break;
		case State.INGAME:
			titleMenu.gameObject.SetActive(false);
			if(winningSprite)
				winningSprite.SetActive(false);
			fileSender.sendFiles = true;
			break;
		case State.TITLE:
			titleMenu.gameObject.SetActive(true);
			titleMenu.gameObject.renderer.material.color = new Color(1,1,1,1);
			if(winningSprite)
				winningSprite.SetActive(false);
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
