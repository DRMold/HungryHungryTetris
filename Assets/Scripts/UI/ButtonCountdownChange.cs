using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Countdown Change Button")]
public class ButtonCountdownChange : MonoBehaviour {

    public bool isIncreasing;
    public Text textToChange;
    public Button otherButton;

    private Button myButton;

    private Coroutine fastChanger;

    void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
        if (MenuMaster.length > 0)
        {
            textToChange.text = MenuMaster.length + " S";
        }
        else
        {
            textToChange.text = "NONE";
        }
    }

    void checkBounded()
    {
        if ((isIncreasing && MenuMaster.timerOptionIndex >= 4) || (!isIncreasing && MenuMaster.timerOptionIndex <= 0))
        {
            myButton.interactable = false;
        }
        else
        {
            myButton.interactable = true;
        }
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressHandle;
        checkBounded();
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandle;
    }

    private void pressHandle(object sender, System.EventArgs e)
    {
        if(myButton.interactable)
        {
            if (isIncreasing)
            {
                MenuMaster.TriggerEvent("IncreaseTimer");
            }
            else
            {
                MenuMaster.TriggerEvent("DecreaseTimer");
            }
            if (MenuMaster.length > 0)
            {
                textToChange.text = MenuMaster.length + " S";
            }
            else
            {
                textToChange.text = "NONE";
            }

            checkBounded();
            otherButton.GetComponent<ButtonCountdownChange>().checkBounded();
        }
    }
}
