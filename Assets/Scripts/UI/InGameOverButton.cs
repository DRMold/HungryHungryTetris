using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

[AddComponentMenu("Scripts/UI/InGame GameOver Button")]
public class InGameOverButton : MonoBehaviour {

    private string buttonType;

	void Start ()
    {
        buttonType = gameObject.name;
	}

    void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressHandler;
    }

    void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandler;
    }
	
    private void pressHandler(object sender, System.EventArgs e)
    {
        Debug.Log("GameOver Screen button pressed");
        if (buttonType.Equals("RestartButton"))
        {
            GameMaster.TriggerEvent("GameRestart");
        }
        else if (buttonType.Equals("MenuButton"))
        {
            GameMaster.TriggerEvent("ShowMenu");
        }
        else
        {
            Debug.LogError("Game Over Button Press: Invalid name!");
        }
    }
}
