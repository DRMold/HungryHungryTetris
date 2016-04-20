using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Text Score Right")]
public class TextScoreRight : MonoBehaviour {

    private Text scoreValue;
    void Awake()
    {
        scoreValue = GetComponent<Text>();
    }
    void OnEnable()
    {
        GameMaster.StartListening("UpdateRightScore", UpdateScore);
    }
    void OnDisable()
    {
        GameMaster.StopListening("UpdateRightScore", UpdateScore);
    }

    void UpdateScore()
    {
        //TODO: Get left Score!
        //scoreValue.text = GameMaster.instance.getRightScore();
    }
}
