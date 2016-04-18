using UnityEngine;
using System.Collections;

public class InGameCanvasManager : MonoBehaviour {
    public GameObject gameOverPanel;
	public GameMaster gameMasterScript;

	void Awake()
    {
        gameOverPanel.SetActive(false);
    }
	
	void Start()
	{
		//gameMasterScript = GameObject.FindWithTag("GameMaster").GetComponent("GameMaster.cs") as GameMaster;
	}

    void Update()
    {
        //TODO: Trigger event in actual game
        if (GameMaster.instance.GetTime() == 0f)
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
