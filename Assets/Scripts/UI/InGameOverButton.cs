using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/InGame GameOver Button")]
public class InGameOverButton : MonoBehaviour {

    private string buttonType;
    private Button myButton;

    void Awake()
    {
        buttonType = gameObject.name;
        myButton = GetComponent<Button>();
        myButton.enabled = false;
        myButton.interactable = false;
    }

    void OnEnable()
    {
        GameMaster.StartListening("ShowGameOver", overHandler);
    }

    void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandler;
        GameMaster.StopListening("ShowGameOver", overHandler);
    }
	
    private void pressHandler(object sender, System.EventArgs e)
    {
        Debug.Log("GameOver Screen button pressed");
        if (!myButton.enabled)
        {
            Debug.LogError("Pressing a game over button when not supposed to be able to!");
            return;
        }
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

    private void overHandler()
    {
        myButton.enabled = true;
        myButton.interactable = true;
        GetComponent<PressGesture>().Pressed += pressHandler;
    }
}
