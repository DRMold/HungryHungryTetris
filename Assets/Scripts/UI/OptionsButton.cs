using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Options Button")]
public class OptionsButton : MonoBehaviour {
	public UnityEvent showOptions;

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
		showOptions.Invoke ();
	}
}
