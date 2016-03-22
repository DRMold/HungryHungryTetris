using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	private int numPlayers;
	// Use this for initialization
	void Start () {
		numPlayers = 4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setNumPlayers(int num) {
		numPlayers = num;
		Debug.Log ("Number of players: " + numPlayers);
	}
}
