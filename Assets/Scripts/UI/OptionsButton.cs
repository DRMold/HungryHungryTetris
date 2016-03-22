using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

[AddComponentMenu("Scripts/UI/Options Button")]
public class OptionsButton : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnEnable() 
	{
		GetComponent<PressGesture> ().Pressed += optionsHandler;
	}
	
	private void OnDisable() 
	{
		GetComponent<PressGesture> ().Pressed -= optionsHandler;
	}
	
	private void optionsHandler(object sender, System.EventArgs e) 
	{
		Debug.Log ("Options button pressed!");
	}
}
