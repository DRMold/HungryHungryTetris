using UnityEngine;
using System.Collections;

public class Tetromino : MonoBehaviour {
	private Vector3 randomDirection;

	public int shape;

	// Use this for initialization
	void Start () {
		randomDirection = new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), 0.0f);
		transform.Rotate (randomDirection);
	}
	
	// Update is called once per frame
	void Update () {	
		//randomDirection = new Vector3 (Random.value, Random.value, 0.0f);
		transform.position += randomDirection * 2.0f * Time.deltaTime;
	}
}
