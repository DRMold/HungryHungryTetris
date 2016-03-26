using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Options Change Button")]
public class ButtonOptionChange : MonoBehaviour {

    public bool isIncreasing;
    public Text textToChange;
    public UnityEvent hitBounds, notBounded;
	
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
        if(isIncreasing)
        {
            MenuMaster.TriggerEvent("IncreaseMusicVolume"); 
        }
        else
        {
            MenuMaster.TriggerEvent("DecreaseMusicVolume");
        }
        textToChange.text = MenuMaster.musicVolume.ToString();
        
        //Works but does not update button state
        if ((isIncreasing && MenuMaster.musicVolume >= 100) || (!isIncreasing && MenuMaster.musicVolume <= 0))
        {
            hitBounds.Invoke();
        }
        else
        {
            notBounded.Invoke();
        }
    }
}
