using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Back Button")]
public class ButtonBack : MonoBehaviour {
	public UnityEvent showMain;
	
	private void OnEnable() 
	{
		GetComponent<PressGesture> ().Pressed += backHandler;
	}
	
	private void OnDisable() 
	{
		GetComponent<PressGesture> ().Pressed -= backHandler;
	}
	
	private void backHandler(object sender, System.EventArgs e) 
	{
		Debug.Log ("Going back!");
		showMain.Invoke ();
	}
}
