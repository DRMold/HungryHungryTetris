using UnityEngine;
using System.Collections;

public class Tetromino : MonoBehaviour {
	private Vector3 randomDirection;
	private Rigidbody rb;
	private float randomSpeed;

	public int shape;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		randomDirection = new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), 0.0f);
		randomSpeed = Random.value * 300.0f;
		//transform.Rotate (randomDirection);

		rb.AddForce (randomDirection * randomSpeed);
	}

	public void ReverseDirection() {
		randomSpeed *= -1;
		// rb.AddForce (randomDirection * randomSpeed * 100, ForceMode.Impulse);
	}
}
