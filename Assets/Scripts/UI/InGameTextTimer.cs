using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/In Game Text Timer")]
public class InGameTextTimer : MonoBehaviour {

    private Text timerText;
    private float timeRemaining;
    void Awake()
    {
        timerText = GetComponent<Text>();
    }

    void Start()
    {
        timeRemaining = GameMaster.instance.GetTime();
    }

    void Update()
    {
        timeRemaining = GameMaster.instance.GetTime();
        if (timeRemaining < 0)
        {
            timerText.text = "-";
        }
        else
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }   
    }
}
