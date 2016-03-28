using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TouchScript.Gestures;

[AddComponentMenu("Scripts/UI/Game Mode Button")]
public class ButtonGameMode : MonoBehaviour {

    private string gameMode;
    private Button myButton;
    void Start() {
        myButton = gameObject.GetComponent<Button>();
        myButton.interactable = false;
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += newPlayerMode;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= newPlayerMode;
    }

	private void newPlayerMode(object sender, System.EventArgs e)
    {
        Debug.Log("TODO: Change game mode!");
        myButton.interactable = false;
    }
}
