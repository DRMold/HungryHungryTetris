using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Start Play Button")]
public class ButtonStart : MonoBehaviour {
	public UnityEvent showReady;
	
	private void OnEnable() 
	{
		GetComponent<PressGesture> ().Pressed += readyHandler;
	}
	
	private void OnDisable() 
	{
		GetComponent<PressGesture> ().Pressed -= readyHandler;
	}
	
	private void readyHandler(object sender, System.EventArgs e) 
	{
        //TODO: FIX ready screen!
        showReady.Invoke ();
        //GameMaster.TriggerEvent("AllPlayersReady");
	}
}
