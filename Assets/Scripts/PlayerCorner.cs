using UnityEngine;
using System.Collections;

public class PlayerCorner : MonoBehaviour 
{
    public int playerIndex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddScore(int aScore)
    {
        Game.Instance.playerScores[playerIndex] += aScore;
    }
}
