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

	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () 
    {
        playerScoreText[0].text = "" + playerScores[0];
        playerScoreText[1].text = "" + playerScores[1];
        playerScoreText[2].text = "" + playerScores[2];
        playerScoreText[3].text = "" + playerScores[3];
	}
}
