using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/UI/InGame GameOver Panel")]
public class InGameOverPanel : MonoBehaviour {

    private CanvasGroup myCanvasGroup;
    void Awake()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        myCanvasGroup.alpha = 0;
        myCanvasGroup.interactable = false;
    }

    void OnEnable()
    {
        //TODO: Stop game movement, obtain and display final scores, show winner
        Debug.Log("Show game over panel!");
        StartCoroutine(fadePanel());
        //GameMaster.StartListening("ShowGameOver", showPanel);
    }

    void OnDisable()
    {
        //GameMaster.StopListening("ShowGameOver", showPanel);
    }

    private void showPanel()
    {
        //TODO: Stop game movement, obtain and display final scores, show winner
        Debug.Log("Game Over!");
        StartCoroutine(fadePanel());
    }

    IEnumerator fadePanel()
    {
        yield return null;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.unscaledDeltaTime;
            myCanvasGroup.alpha = time;
            Debug.Log(time + " " + myCanvasGroup.alpha);
            yield return null;
        }
        myCanvasGroup.alpha = 1f;
        myCanvasGroup.interactable = true;
    }
}
