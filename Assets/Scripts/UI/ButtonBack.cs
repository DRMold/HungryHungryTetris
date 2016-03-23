using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Back Button")]
public class ButtonBack : MonoBehaviour {
	public UnityEvent showMain;

    private Button myButton;
    private Image buttonAlpha;
    public Text backText;
    void Start()
    {
        myButton = GetComponent<Button>();
        buttonAlpha = myButton.image;
    }

	private void OnEnable() 
	{
		GetComponent<PressGesture> ().Pressed += backHandler;
        MenuMaster.StartListening("AllPlayersReady", readyHandler);
        MenuMaster.StartListening("AllPlayersNotReady", unreadyHandler);
    }
	
	private void OnDisable() 
	{
		GetComponent<PressGesture> ().Pressed -= backHandler;
        MenuMaster.StopListening("AllPlayersReady", readyHandler);
        MenuMaster.StopListening("AllPlayersNotReady", unreadyHandler);
    }
	
	private void backHandler(object sender, System.EventArgs e) 
	{
		showMain.Invoke ();
	}

    private void readyHandler()
    {
        myButton.interactable = false;
        Color c = buttonAlpha.color;
        c.a = 0f;
        buttonAlpha.color = c;
        backText.enabled = false;
    }

    private void unreadyHandler()
    {
        myButton.interactable = true;
        Color c = buttonAlpha.color;
        c.a = 1f;
        buttonAlpha.color = c;
        backText.enabled = true;
    }
}
