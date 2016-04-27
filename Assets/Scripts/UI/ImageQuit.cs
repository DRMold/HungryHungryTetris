using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ImageQuit : MonoBehaviour {
    private void OnEnable()
    {
        GetComponent<LongPressGesture>().LongPressed += LongPressHandle;
    }

    private void OnDisable()
    {
        GetComponent<LongPressGesture>().LongPressed -= LongPressHandle;
    }

    private void LongPressHandle(object sender, System.EventArgs e)
    {
        GameMaster.TriggerEvent("QuitGame");
    }
}
