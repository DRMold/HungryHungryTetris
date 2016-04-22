using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {
	public GameObject playerQueue;
	//to be added in inspector
	public GameObject opponentQueue;
	
	void OnTriggerEnter(Collider tetromino) {
		// shape      -        int
		//   s                  0
		//   i                  1
		//   o                  2
		//   j                  3
		//   t                  4
		//   l                  5
		//   z                  6
		// ^^^ don't know if above is correct rn
		//   FFS                7
		//   SFS                8
		int shape = tetromino.gameObject.GetComponent<Tetromino>().shape;
		Debug.Log ("Got shape " + shape);
		if (shape < 7) {
			playerQueue.GetComponent<PrevTetris> ().AddToQueue (shape);
			Debug.Log ("GameObject Added to Queue: " + shape);
		} else {
			int power = shape-7;
			Debug.Log ("Applying powerup #" + power);
			switch (power) {
			case 0:
				//Increase Fall speed
				opponentQueue.GetComponent<PrevTetris>().speedUp();
				break;
			case 1:
				//reduce fall speed, gets reset by method
				playerQueue.GetComponent<PrevTetris>().slowDown();
				break;
			case 2:
				//A bunch of blocks created on player's tetris playfield
				break;
			case 3:
				//piece cycle
				break;
			}
		}
		Destroy (tetromino.gameObject);
	}
}
