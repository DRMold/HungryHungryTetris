using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Menu Master")]
public class MenuMaster : MonoBehaviour
{
    public GameObject mainPanel, readyPanel, optionsPanel;
    public Text countDownText;

    private int readyPlayerCount = 0;

    public static int musicVolume = 100;
	
	private static List<float> timerOptions = new List<float>();
	public static int timerOptionIndex = 0;
	public static float length = -1f;

    private Dictionary<string, UnityEvent> eventDictionary;
    private static MenuMaster menuMaster;
    public static MenuMaster instance
    {
        get
        {
            if (!menuMaster)
            {
                menuMaster = FindObjectOfType(typeof(MenuMaster)) as MenuMaster;
            }
            if (!menuMaster)
            {
                Debug.LogError("There needs to be a MenuMaster script on a GameObject.");
            }
            else
            {
                menuMaster.initialize();
            }
            return menuMaster;
        }
    }


    void initialize()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
        //timerOptionIndex = 0;
        //length = -1f;
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (menuMaster == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }


    void Start()
    {
		timerOptions.Add(500f);
		timerOptions.Add(300f);
		timerOptions.Add(180f);
		timerOptions.Add(60f);
		timerOptions.Add(-1f);
        timerOptions.Sort();
        mainPanel.SetActive(true);
        readyPanel.SetActive(false);
        optionsPanel.SetActive(false);
        countDownText.enabled = false;
    }

    void OnEnable()
    {
        MenuMaster.StartListening("PlayerReady", addPlayerReady);
        MenuMaster.StartListening("PlayerNotReady", removePlayerReady);
        MenuMaster.StartListening("DecreaseMusicVolume", decreaseMusicVolume);
        MenuMaster.StartListening("IncreaseMusicVolume", increaseMusicVolume);
        MenuMaster.StartListening("FastDecreaseMusicVolume", fastDecreaseMusicVolume);
        MenuMaster.StartListening("FastIncreaseMusicVolume", fastIncreaseMusicVolume);
		MenuMaster.StartListening("DecreaseTimer", DecreaseTimer);
		MenuMaster.StartListening("IncreaseTimer", IncreaseTimer);
    }
    void OnDisable()
    {
        MenuMaster.StopListening("PlayerReady", addPlayerReady);
        MenuMaster.StopListening("PlayerNotReady", removePlayerReady);
        MenuMaster.StopListening("DecreaseMusicVolume", decreaseMusicVolume);
        MenuMaster.StopListening("IncreaseMusicVolume", increaseMusicVolume);
        MenuMaster.StopListening("FastDecreaseMusicVolume", fastDecreaseMusicVolume);
        MenuMaster.StopListening("FastIncreaseMusicVolume", fastIncreaseMusicVolume);
		MenuMaster.StopListening("DecreaseTimer", DecreaseTimer);
		MenuMaster.StopListening("IncreaseTimer", IncreaseTimer);
    }

    void addPlayerReady()
    {
        readyPlayerCount++;
        Debug.Log("New player added! Count: " + readyPlayerCount);
        if(readyPlayerCount >= 4) //TODO: Check for game mode as well!
        {
            MenuMaster.TriggerEvent("AllPlayersReady");
            countDownText.enabled = true;
            Debug.Log(countDownText.enabled + " is countdown enabled?");
            //GameMaster.TriggerEvent("AllPlayersReady");
        }
    }

    void removePlayerReady()
    {
        readyPlayerCount--;
        Debug.Log("Player removed! Count: " + readyPlayerCount);
        countDownText.enabled = false;
        MenuMaster.TriggerEvent("CountdownInterrupted");
        MenuMaster.TriggerEvent("AllPlayersNotReady");
    }

	void IncreaseTimer()
	{
        timerOptionIndex++;
        if (timerOptionIndex > 4)
			timerOptionIndex = 4;
		length = timerOptions[timerOptionIndex];
    }
	
	void DecreaseTimer()
	{
        timerOptionIndex--;
        if (timerOptionIndex < 0)
			timerOptionIndex = 0;
		length = timerOptions[timerOptionIndex];
    }
	
    void decreaseMusicVolume()
    {
        musicVolume--;
        if (musicVolume < 0)
        {
            musicVolume = 0;
        }
        GameMaster.TriggerEvent("MusicVolumeChange");
    }

    void increaseMusicVolume()
    {
        musicVolume++;
        if (musicVolume > 100)
        {
            musicVolume = 100;
        }
        GameMaster.TriggerEvent("MusicVolumeChange");
    }

    void fastDecreaseMusicVolume()
    {
        musicVolume -= 5;
        if (musicVolume < 0)
        {
            musicVolume = 0;
        }
        GameMaster.TriggerEvent("MusicVolumeChange");
    }

    void fastIncreaseMusicVolume()
    {
        musicVolume += 5;
        if (musicVolume > 100)
        {
            musicVolume = 100;
        }
        GameMaster.TriggerEvent("MusicVolumeChange");
    }
    public void showMainMenu() {
        mainPanel.SetActive (true);
		readyPanel.SetActive (false);
		optionsPanel.SetActive (false);
    }

	public void showReadyMenu() {
		mainPanel.SetActive (false);
		readyPanel.SetActive (true);
		optionsPanel.SetActive (false);
	}

	public void showOptionsMenu() {
		mainPanel.SetActive (false);
		readyPanel.SetActive (false);
		optionsPanel.SetActive (true);
    }
}

