using UnityEngine;
using System.Collections;

public class BoundaryLogic : MonoBehaviour {

	void OnCollisionExit(Collision tetromino) {
		Debug.Log ("Tetromino Leaving play area");
		//tetromino.collider.GetComponent<Tetromino> ().ReverseDirection ();
	}
}
