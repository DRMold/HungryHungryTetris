using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {
	
	public GameObject playerQueue;

	void OnTriggerEnter(Collider tetromino) {
		//Have control flow based off type? Or shape will be property of tetromino?
		int shape = tetromino.gameObject.GetComponent<Tetromino>().shape;
		if (shape != 7) {
			playerQueue.GetComponent<PrevTetris> ().AddToQueue (shape);
			Debug.Log ("GameObject Added to Queue: " + shape);
			Destroy (tetromino.gameObject);
		} else {
			//temporary select powerups, will be changed to link with powerup texture / model
			playerQueue.GetComponent<PrevTetris> ().PowerBar = true;
			/*
			int power = Random.Range(0,4);
			Debug.Log ("Applying powerup #" + power);
			switch (power) {
			case 0:
				//Increase Fall speed
				break;
			case 1:
				//A bunch of blocks created on player's tetris playfield
				playerQueue.GetComponent<PrevTetris> ().PowerBar = true;
				break;
			case 2:
				//reduce fall speed
				break;
			case 3:
				//piece cycle
				break;
			}
			*/
		}
	}
}
