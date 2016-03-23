using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Ready Player Button")]
public class ButtonPlayerReady : MonoBehaviour {


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
        Debug.Log("Ready button pressed.");
        MenuMaster.TriggerEvent("playerReady");
    }

    private void longPressHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button long pressed.");
    }

    private void releaseHandle(object sender, System.EventArgs e)
    {
        Debug.Log("Ready button released.");
        MenuMaster.TriggerEvent("playerNotReady");
    }
}
