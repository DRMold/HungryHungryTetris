using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {
	public GameObject playerQueue;
	
	void OnTriggerEnter(Collider tetromino) {
		//Have control flow based off type? Or shape will be property of tetromino?
		int shape = tetromino.gameObject.GetComponent<Tetromino>().shape;
		playerQueue.GetComponent<PrevTetris>().AddToQueue (shape);
		Debug.Log ("GameObject Added to Queue: "+shape);
		Destroy (tetromino.gameObject);
	}
}
