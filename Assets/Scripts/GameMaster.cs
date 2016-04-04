using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    public bool fadeIn, fadeOut;
    public float transitionTime = 1.0f;
    public Image fadeImg;
    float time = 0f;

    private int numPlayers;
    private AudioSource gameAudio;
    private GUITexture fadeTexture;

    private Dictionary<string, UnityEvent> eventDictionary;
    private static GameMaster gameMaster;
    public static GameMaster instance
    {
        get
        {
            if (!gameMaster)
            {
                gameMaster = FindObjectOfType(typeof(GameMaster)) as GameMaster;
                if (!gameMaster)
                {
                    Debug.LogError("There needs to be a GameMaster script on a GameObject.");
                }
                else
                {
                    gameMaster.initialize();
                }
            }
            return gameMaster;
        }
    }


    void initialize()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
        if (fadeIn)
        {
            fadeTexture = GetComponent<GUITexture>();
            fadeTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
            //if (fadeImg != null )
            //{
            //    fadeImg.gameObject.SetActive(true);
            //    fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1.0f);
            //}
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
        if (gameMaster == null) return;
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
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        numPlayers = 4;
		gameAudio = GetComponent<AudioSource>();
        gameAudio.loop = true;
		gameAudio.Play();
		//DontDestroyOnLoad(this.gameObject);
    }

    void Update() {

    }

    private void OnEnable()
    {
        if (fadeIn)
        {
            StartCoroutine(StartScene());
        }
        GameMaster.StartListening("AllPlayersReady", startGame);
        GameMaster.StartListening("GameRestart", startGame);
        GameMaster.StartListening("ShowMenu", showMenu);
        GameMaster.StartListening("MusicVolumeChange", musicVolumeChange);
    }

    private void OnDisable()
    {
        GameMaster.StopListening("AllPlayersReady", startGame);
        GameMaster.StopListening("GameRestart", startGame);
        GameMaster.StopListening("ShowMenu", showMenu);
        GameMaster.StopListening("MusicVolumeChange", musicVolumeChange);
    }

    public void LoadScene(string level)
    {
        StartCoroutine(EndScene(level));
    }

    IEnumerator StartScene()
    {
        Debug.Log("Starting scene");
        yield return null;
        FadeToClear();
        if(fadeTexture.color.a <= 0.05f)
        {
            fadeTexture.color = Color.clear;
            fadeTexture.enabled = false;
            Debug.Log("Scene started");
        }
        //while(time >= 0.0f)
        //{
        //    fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
        //    time -= Time.deltaTime * (1.0f / transitionTime);
        //    yield return null;
        //}
        //fadeImg.gameObject.SetActive(false);
    }

    IEnumerator EndScene(string nextScene)
    {
        //fadeImg.gameObject.SetActive(true);
        yield return null;
        fadeTexture.enabled = true;
        FadeToBlack();
        if (fadeTexture.color.a <= 0.95f)
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
        //while (time <= 1.0f)
        //{
        //    fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
        //    time += Time.deltaTime * (1.0f / transitionTime);
        //    yield return null;
        //}
        StartCoroutine(StartScene());
    }

    private void FadeToClear()
    {
        fadeTexture.color = Color.Lerp(fadeTexture.color, Color.clear, transitionTime * Time.deltaTime);
    }

    private void FadeToBlack()
    {
        fadeTexture.color = Color.Lerp(fadeTexture.color, Color.black, transitionTime * Time.deltaTime);
    }

    public void startGame()
    {
        //TODO: Load options and modes
        Debug.Log("Starting game");
        instance.LoadScene("Vertical_Slice_POC");
    }

    public void showMenu()
    {
        Debug.Log("Showing Menu");
        instance.LoadScene("MainMenu");
    }

	public void setNumPlayers(int num) {
		numPlayers = num;
		Debug.Log ("Number of players: " + numPlayers);
	}

    private void musicVolumeChange()
    {
        gameAudio.volume = MenuMaster.musicVolume / 100f;
    }
}
