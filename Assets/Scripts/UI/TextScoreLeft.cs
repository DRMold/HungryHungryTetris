using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Text Score Left")]
public class TextScoreLeft : MonoBehaviour {

    private Text scoreValue;
    void Awake()
    {
        scoreValue = GetComponent<Text>();
    }
    void OnEnable()
    {
        GameMaster.StartListening("UpdateLeftScore", UpdateScore);
    }
    void OnDisable()
    {
        GameMaster.StopListening("UpdateLeftScore", UpdateScore);
    }

    void UpdateScore()
    {
        //TODO: Get left Score!
        //scoreValue.text = GameMaster.instance.getLeftScore();
    }
}
