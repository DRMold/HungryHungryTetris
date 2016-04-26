using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextCountdown : MonoBehaviour {
    private Text myText;
    private int counter;
    private Coroutine countingFunction;

    void Awake() {
        myText = GetComponent<Text>();
        counter = 4;
        myText.text = "";
    }

    private void OnEnable()
    {
        MenuMaster.StartListening("CountdownInterrupted", interruptHandler);
        MenuMaster.StartListening("AllPlayersReady", ReadyHandle);
    }
    
    public void ReadyHandle()
    {
        countingFunction = StartCoroutine(StartCountdown());
    }
    IEnumerator StartCountdown()
    {
        myText.text = counter.ToString();
        yield return new WaitForSeconds(1f);
        while (counter > 0)
        {
            yield return new WaitForSeconds(1f);
            counter--;
            Debug.Log(counter);
            myText.text = counter.ToString();
        }
        Debug.Log("Countdown finished!");
        GameMaster.TriggerEvent("AllPlayersReady");
    }
    private void OnDisable()
    {
        if(countingFunction != null)
            StopCoroutine(countingFunction);
        MenuMaster.StopListening("CountdownInterrupted", interruptHandler);
        MenuMaster.StopListening("AllPlayersReady", ReadyHandle);
        Debug.Log("Disabled countdown.");
        counter = 4;
        if(myText != null)
            myText.text = "";
    }

    private void interruptHandler()
    {
        if(countingFunction != null)
            StopCoroutine(countingFunction);
        counter = 4;
        myText.text = "";
        Debug.Log("Countdown interrupted!");
        //this.enabled = false;
    }
}
