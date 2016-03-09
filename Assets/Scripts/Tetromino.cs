using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{ public float xMin, xMax, yMin, yMax; }

public class Tetromino : MonoBehaviour {
	private Vector3 randomDirection;
	private Rigidbody rb;
	private float randomSpeed;

	public int shape;
	public Boundary boundary;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		randomDirection = new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), 0.0f);
		randomSpeed = Random.value * 300.0f;
		//transform.Rotate (randomDirection);

		rb.AddForce (randomDirection * randomSpeed);
	}

	void Update () {	
		// randomDirection = randomDirection * 2.0f * Time.deltaTime;
		//randomDirection = new Vector3 (Random.value, Random.value, 0.0f);
//        transform.position = new Vector3 (
//			Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
//			Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax),
//			0.0f
//		)s;
	}

	void FixedUpdate() {
		//rb.AddForce (randomDirection * randomSpeed, ForceMode.Acceleration);
//		if (rb.velocity.magnitude == 0) {
//			rb.AddForce (randomDirection * -randomSpeed);
//		}
	}

	public void ReverseDirection() {
		randomSpeed *= -1;
		// rb.AddForce (randomDirection * randomSpeed * 100, ForceMode.Impulse);
	}
}
