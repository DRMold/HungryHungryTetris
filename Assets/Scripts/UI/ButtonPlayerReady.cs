using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Utils;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Ready Player Button")]
public class ButtonPlayerReady : MonoBehaviour {

    private Button myButton;
    //private Image myButtonImage;
    private Text buttonText;
    private ColorBlock buttonColors, selectedButtonColors;
    private bool isReady;
    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        //myButtonImage = gameObject.GetComponent<Image>();
        buttonText = gameObject.GetComponentInChildren<Text>();
        selectedButtonColors = buttonColors = myButton.colors;
        buttonColors.highlightedColor = buttonColors.normalColor;
        selectedButtonColors.normalColor = buttonColors.pressedColor;
        selectedButtonColors.highlightedColor = buttonColors.pressedColor;
        selectedButtonColors.pressedColor = buttonColors.pressedColor;
        //myButton.colors = Color.red;
        isReady = false;
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressHandle;
    }
    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandle;
    }

    private void pressHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button pressed. Colors before: " + buttonColors.normalColor);
        if(!isReady)
        {
            myButton.colors = selectedButtonColors;
            buttonText.text = "Ready";
            Debug.Log("Normal colors now: " + myButton.colors.normalColor);
            MenuMaster.TriggerEvent("PlayerReady");
            isReady = true;
        }
        else
        {
            Debug.Log("Ready button now not ready.");
            myButton.colors = buttonColors;
            buttonText.text = "Not Ready";
            Debug.Log("Normal colors now: " + myButton.colors.normalColor);
            MenuMaster.TriggerEvent("PlayerNotReady");
            isReady = false;
        }
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
