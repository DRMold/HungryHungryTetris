using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextCountdown : MonoBehaviour {
    private Text myText;
    private int counter;
    private Coroutine countingFunction;

    void Start() {
        myText = GetComponent<Text>();
        counter = 3;
    }

    private void OnEnable()
    {
        MenuMaster.StartListening("CountdownInterrupted", interruptHandler);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);
        while (counter > 0)
        {
            yield return new WaitForSeconds(1f);
            counter--;
            Debug.Log(counter);
            myText.text = counter.ToString();
        }
        Debug.Log("Countdown finished!");
    }
    private void OnDisable()
    {
        StopCoroutine(StartCountdown());
        MenuMaster.StopListening("CountdownInterrupted", interruptHandler);
        Debug.Log("Disabled countdown.");
        counter = 3;
    }

    private void interruptHandler()
    {
        StopCoroutine(StartCountdown());
        counter = 3;
        myText.text = counter.ToString();
        Debug.Log("Countdown interrupted!");
        this.enabled = false;
    }
}
