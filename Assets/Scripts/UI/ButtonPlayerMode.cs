using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using TouchScript.Gestures;

[AddComponentMenu("Scripts/UI/Player Mode Button")]
public class ButtonPlayerMode : MonoBehaviour {

	public UnityEvent playerModeChanged;

	private int gameMode;
	private Button myButton;
	void Start() {
		myButton = gameObject.GetComponent<Button> ();
		if (int.TryParse (this.ToString () [0].ToString(), out gameMode)) {
			if(gameMode == 2) {
				myButton.interactable = false;
			}
		}
	}
	private void OnEnable() {
		GetComponent<PressGesture> ().Pressed += newPlayerMode;
	}

	private void OnDisable() {
		GetComponent<PressGesture> ().Pressed -= newPlayerMode;
	}

	private void newPlayerMode(object sender, System.EventArgs e) {
		switch (gameMode) {
		case 1: if (myButton.interactable) {
				playerModeChanged.Invoke(); 
			} 
			break;
		case 2: if (myButton.interactable) {
				playerModeChanged.Invoke(); 
			}
			break;
		default: Debug.Log ("Invalid player mode!"); break; 
		}
	}
}
