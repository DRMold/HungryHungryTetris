using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {
	public GameObject playerQueue;
	//to be added in inspector
	public GameObject opponentQueue;
	
	void OnTriggerEnter(Collider tetromino) {
		//Have control flow based off type? Or shape will be property of tetromino?
		int shape = tetromino.gameObject.GetComponent<Tetromino>().shape;
		//shape number does not reflect numbers properly
		if (shape != 0) {
			playerQueue.GetComponent<PrevTetris> ().AddToQueue (shape);
			Debug.Log ("GameObject Added to Queue: " + shape);
		} else {
			//temporary select powerups, will be changed to link with powerup texture / model
			int power = 0;
			Debug.Log ("Applying powerup #" + power);
			switch (power) {
			case 0:
				//Increase Fall speed
				opponentQueue.GetComponent<PrevTetris>().speedUp();
				break;
			case 1:
				//A bunch of blocks created on player's tetris playfield
				break;
			case 2:
				//reduce fall speed, gets reset by method
				playerQueue.GetComponent<PrevTetris>().slowDown();
				break;
			case 3:
				//piece cycle
				break;
			}
		}
		Destroy (tetromino.gameObject);
	}
}
