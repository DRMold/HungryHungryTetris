using UnityEngine;
using System.Collections;
using TouchScript.Gestures;


public class Tetromino : MonoBehaviour {
	//For handling touch
	private float startTime;
	private Vector2 startPos;
	private bool couldBeSwiped;
	private float comfortZone;
	//Modify these two for fine tuning gameplay
	public float minSwipeDist;
	public float maxSwipeTime = 2.0f;

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

    private void OnEnable() {
		// Subscribe to Flick Gesture
		GetComponent<FlickGesture>().Flicked += flickedHandler;
	}
	
	private void OnDisable() {
		GetComponent<FlickGesture>().Flicked -= flickedHandler;
	}
	
	private void flickedHandler(object sender, System.EventArgs e) {
		var gesture = sender as FlickGesture;
		
		float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
		
		Vector3 wp1 = new Vector3(gesture.PreviousScreenPosition.x,
								gesture.PreviousScreenPosition.y,
								distanceFromCamera);
		wp1 = Camera.main.ScreenToWorldPoint(wp1);
		Vector3 wp2 = new Vector3(gesture.ScreenPosition.x, gesture.ScreenPosition.y, distanceFromCamera);
		wp2 = Camera.main.ScreenToWorldPoint(wp2);
		
		Vector3 velocity = (wp2 - wp1)/gesture.FlickTime;
		
		rb.AddForce(velocity, ForceMode.VelocityChange);
	}
}
