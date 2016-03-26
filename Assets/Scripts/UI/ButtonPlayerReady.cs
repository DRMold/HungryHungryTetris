using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Utils;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Ready Player Button")]
public class ButtonPlayerReady : MonoBehaviour {

    private Button myButton;
    private Text buttonText;
    private ColorBlock buttonColors, selectedButtonColors;

    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        buttonText = gameObject.GetComponentInChildren<Text>();
        selectedButtonColors = buttonColors = myButton.colors;
        selectedButtonColors.normalColor = buttonColors.pressedColor;
        Debug.Log(buttonText.text);
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressHandle;
        GetComponent<LongPressGesture>().LongPressed += longPressHandle;
        GetComponent<ReleaseGesture>().Released += releaseHandle;
    }
    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandle;
        GetComponent<LongPressGesture>().LongPressed -= longPressHandle;
        GetComponent<ReleaseGesture>().Released -= releaseHandle;
    }

    private void pressHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button pressed. Colors before: " + buttonColors.normalColor);
        myButton.colors = selectedButtonColors;
        buttonText.text = "Ready";
        Debug.Log("Normal colors now: " + myButton.colors.normalColor);
        MenuMaster.TriggerEvent("PlayerReady");
    }

    private void longPressHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button long pressed."); 
    }

    private void releaseHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button released. Colors before: " + buttonColors.normalColor);
        MenuMaster.TriggerEvent("PlayerNotReady");
        myButton.colors = buttonColors;
        buttonText.text = "Not Ready";
        Debug.Log("Normal colors now: " + myButton.colors.normalColor);
    }
}
