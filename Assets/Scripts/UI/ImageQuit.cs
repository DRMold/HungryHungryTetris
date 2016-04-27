using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ImageQuit : MonoBehaviour {
    private void OnEnable()
    {
        GetComponent<LongPressGesture>().LongPressed += LongPressHandle;
        GameMaster.StartListening("ShowGameOver", DestroyImage);
    }

    void Update()
    {
    }
    private void OnDisable()
    {
        GetComponent<LongPressGesture>().LongPressed -= LongPressHandle;
        GameMaster.StopListening("ShowGameOver", DestroyImage);
    }

    private void LongPressHandle(object sender, System.EventArgs e)
    {
        GameMaster.TriggerEvent("QuitGame");
    }

    private void DestroyImage()
    {
        Destroy(gameObject);
    }
}
