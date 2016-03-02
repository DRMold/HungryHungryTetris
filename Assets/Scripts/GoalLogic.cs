using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {
	public string playerQueue;
	
	void OnTriggerEnter(Collider tetromino) {
		//Have control flow based off type? Or shape will be property of tetromino?
		//GameObject.FindGameObjectWithTag (playerQueue).GetComponent<PrevTetris>().shapeQueue.Enqueue (tetromino.gameObject/*.shape*/);
		Destroy (tetromino);
	}
}
