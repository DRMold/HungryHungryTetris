using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Scripts/UI/Menu Master")]
public class MenuMaster : MonoBehaviour
{
    public GameObject mainPanel, readyPanel, optionsPanel;

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
        mainPanel.SetActive(true);
        readyPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    void OnEnable()
    {
        MenuMaster.StartListening("playerReady", addPlayerReady);
        MenuMaster.StartListening("playerNotReady", removePlayerReady);
    }
    void OnDisable()
    {
        MenuMaster.StopListening("playerReady", addPlayerReady);
        MenuMaster.StopListening("playerNotReady", removePlayerReady);
    }

    void addPlayerReady()
    {
        Debug.Log("New player added!");
    }
    void removePlayerReady()
    {
        Debug.Log("Player removed!");
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

