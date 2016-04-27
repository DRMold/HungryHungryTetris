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
        GameMaster.StartListening("ShowGameOver1", ShowRightLost);
        GameMaster.StartListening("ShowGameOver2", ShowLeftLost);

    }
    void OnDisable()
    {
        GameMaster.StopListening("ShowGameOver", showGameOver);
        GameMaster.StopListening("ShowGameOver1", ShowRightLost);
        GameMaster.StopListening("ShowGameOver2", ShowLeftLost);
    }

    private void showGameOver()
    {

        gameOverPanel.SetActive(true);
		Time.timeScale = 0f;
    }

    private void ShowLeftLost()
    {
        InGameOverPanel.leftLost = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Left lost!");
    }

    private void ShowRightLost()
    {
        InGameOverPanel.rightLost = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Right lost!");
    }
}
