using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Scripts/UI/InGame GameOver Panel")]
public class InGameOverPanel : MonoBehaviour {

    public PrevTetris playerOne;
    public PrevTetris playerTwo;
    private CanvasGroup myCanvasGroup;
    public Text headerText;
    public Image leftPanel;
    public Image rightPanel;
    public Text gameScoreLeft;
    public Text gameScoreRight;
    public Text leftPanelScore;
    public Text rightPanelScore;

    private Color winnerColor;
    private Color loserColor;

    public static bool leftLost;
    public static bool rightLost;

    void Awake()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        myCanvasGroup.alpha = 0;
        myCanvasGroup.interactable = false;
        winnerColor = Color.green;
        loserColor = Color.gray;
        leftLost = false;
        rightLost = false;
    }

    void OnEnable()
    {
        //TODO: obtain and display final scores, show winner
        Debug.Log("Show game over panel!");
        leftLost = playerOne.myGameOver;
        Debug.Log("Left loss:" + playerOne.myGameOver);
        rightLost = playerTwo.myGameOver;
        Debug.Log("Right loss:" + playerTwo.myGameOver);
        DetermineWinner();
        StartCoroutine(fadePanel());  
    }

    void OnDisable()
    {
        leftLost = false;
        rightLost = false;
    }

    IEnumerator fadePanel()
    {
        yield return null;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.unscaledDeltaTime;
            myCanvasGroup.alpha = time;
            yield return null;
        }
        myCanvasGroup.alpha = 1f;
        myCanvasGroup.interactable = true;
    }

    private void DetermineWinner()
    {
        int leftScore, rightScore;
        if (int.TryParse(gameScoreLeft.text, out leftScore) && int.TryParse(gameScoreRight.text, out rightScore))
        { 
            if (GameMaster.instance.GetTime() == 0)
            {
                Debug.Log("Time's up! Left: " + leftScore + ", Right: " + rightScore);
            
                leftPanelScore.text = leftScore.ToString();
                rightPanelScore.text = rightScore.ToString();
                if (leftScore == rightScore)
                {
                    Debug.Log("Time - Tie!");
                    headerText.text = "Time's up - it's a tie!";
                }
                else if(leftScore < rightScore)
                {
                    Debug.Log("Time - Right wins!");
                    headerText.text = "Time's up - red team wins!";
                    headerText.color = Color.red;
                    ShowRightWin();
                }
                else
                {
                    Debug.Log("Time - Left wins!");
                    headerText.text = "Time's up - blue team wins!";
                    headerText.color = Color.blue;
                    ShowLeftWin();
                }
            }
            else
            {
                Debug.Log("Someone failed!");
                if (leftLost)
                {
                    Debug.Log("Show left lost!");
                    headerText.text = "Game over - red team wins!";
                    headerText.color = Color.red;
                    ShowRightWin();
                    rightPanelScore.text = rightScore.ToString();
                    leftPanelScore.text = rightScore.ToString();
                    leftPanelScore.CrossFadeAlpha(.25f, .1f, true);
                }
                else if (rightLost)
                {
                    Debug.Log("Show right lost!");
                    headerText.text = "Game over - blue team wins!";
                    headerText.color = Color.blue;
                    ShowLeftWin();
                    leftPanelScore.text = leftScore.ToString();
                    rightPanelScore.text = rightScore.ToString(); ;
                    rightPanelScore.CrossFadeAlpha(.25f, .1f, true);
                }
                else
                {
                    Debug.LogError("No loser was determined in game over panel!");
                }
            }
        }
        else
        {
            Debug.LogError("Score values incorrectly parsed!");
        }
  
    }

    public void ShowLeftWin()
    {
        leftPanel.color = winnerColor;
        rightPanel.color = loserColor;
    }

    public void ShowRightWin()
    {
        leftPanel.color = loserColor;
        rightPanel.color = winnerColor;
    }
}
