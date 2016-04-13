using UnityEngine;
using System.Collections;

public class InGameCanvasManager : MonoBehaviour {
    public GameObject gameOverPanel;

	void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        //TODO: Trigger event in actual game
        if (GameMaster.GetTime() = 0f)
        {
            GameMaster.TriggerEvent("ShowGameOver");
        }
    }

    void OnEnable()
    {
        GameMaster.StartListening("ShowGameOver", showGameOver);
    }
    void OnDisable()
    {
        GameMaster.StopListening("ShowGameOver", showGameOver);
    }

    private void showGameOver()
    {
        gameOverPanel.SetActive(true);
		Time.timeScale = 0f;
    }
}
