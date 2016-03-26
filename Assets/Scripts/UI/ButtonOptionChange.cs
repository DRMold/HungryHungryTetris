using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Options Change Button")]
public class ButtonOptionChange : MonoBehaviour {

    public bool isIncreasing;
    public Text textToChange;
    public Button otherButton;
    public float holdTime = 1f;

    private Button myButton;

    private Coroutine fastChanger;

    void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
    }

    void checkBounded()
    {
        if ((isIncreasing && MenuMaster.musicVolume >= 100) || (!isIncreasing && MenuMaster.musicVolume <= 0))
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
        GetComponent<LongPressGesture>().LongPressed += longPressHandle;
        GetComponent<ReleaseGesture>().Released += releaseHandle;
        checkBounded();
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandle;
        GetComponent<LongPressGesture>().LongPressed -= longPressHandle;
        GetComponent<ReleaseGesture>().Released -= releaseHandle;
    }

    private void pressHandle(object sender, System.EventArgs e)
    {
        //TODO: Determine which event to trigger with multiple options
        if (isIncreasing)
        {
            MenuMaster.TriggerEvent("IncreaseMusicVolume"); 
        }
        else
        {
            MenuMaster.TriggerEvent("DecreaseMusicVolume");
        }
        textToChange.text = MenuMaster.musicVolume.ToString();

        checkBounded();
        otherButton.GetComponent<ButtonOptionChange>().checkBounded();
    }

    private void longPressHandle(object sender, System.EventArgs e)
    {
        if (MenuMaster.musicVolume > 0 && MenuMaster.musicVolume < 100)
        {
            fastChanger = StartCoroutine(fastChange());
        }
    }

    //TODO: extend for multiple options
    IEnumerator fastChange()
    {
        while (MenuMaster.musicVolume > 0 && MenuMaster.musicVolume < 100)
        {
            if (isIncreasing)
            {
                MenuMaster.TriggerEvent("FastIncreaseMusicVolume");
            }
            else
            {
                MenuMaster.TriggerEvent("FastDecreaseMusicVolume");
            }
            Debug.Log("LongPress: " + MenuMaster.musicVolume);
            textToChange.text = MenuMaster.musicVolume.ToString();
            checkBounded();
            otherButton.GetComponent<ButtonOptionChange>().checkBounded();
            yield return new WaitForSeconds(holdTime);
        }
    }

    private void releaseHandle(object sender, System.EventArgs e)
    {
        if (fastChanger != null)
        {
            StopCoroutine(fastChanger);
        }
    }
}
