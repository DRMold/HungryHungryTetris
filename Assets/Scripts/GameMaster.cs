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
    float time = 500f;
	bool countingDown = false;

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
	
	void OnLevelWasLoaded(int level)
	{
        if (level == 1)
        {
            time = MenuMaster.length;
            InitTimer();
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
        instance.time = -1f;
    }
    void Start()
    {
		//countingDown = false;
        numPlayers = 4;
		gameAudio = GetComponent<AudioSource>();
        gameAudio.loop = true;
		gameAudio.Play();
		//DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
		if (countingDown)
		{
			time -= 1.0f * Time.deltaTime;
			if (time < 0f)
				time = 0.0f;
		}
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
		GameMaster.StartListening("TimerChange", SetTimer);
        GameMaster.StartListening("QuitGame", ExitGame);
    }

    private void OnDisable()
    {
        GameMaster.StopListening("AllPlayersReady", startGame);
        GameMaster.StopListening("GameRestart", startGame);
        GameMaster.StopListening("ShowMenu", showMenu);
        GameMaster.StopListening("MusicVolumeChange", musicVolumeChange);
		GameMaster.StopListening("TimerChange", SetTimer);
        GameMaster.StopListening("QuitGame", ExitGame);
    }

    public void LoadScene(string level)
    {
        StartCoroutine(EndScene(level));
    }

    IEnumerator StartScene()
    {
        Debug.Log("Starting scene");
        yield return null;
        Time.timeScale = 1.0f;
    }

    IEnumerator EndScene(string nextScene)
    {
        yield return null;
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        StartCoroutine(StartScene());
    }

    private void FadeToClear()
    {
        fadeTexture.color = Color.Lerp(fadeTexture.color, Color.clear, transitionTime);
    }

    private void FadeToBlack()
    {
        fadeTexture.color = Color.Lerp(fadeTexture.color, Color.black, transitionTime);
    }

    public void startGame()
    {
        //TODO: Load options and modes
        Debug.Log("Starting game");
        instance.LoadScene("Vertical_Slice_POC");
    }
	
	void SetTimer()
	{
		time = MenuMaster.length;
		Debug.Log("Timer set to: " + time);
	}
	
	void InitTimer() 
	{ 
		if (time > 0)
			countingDown = true; 
	}
	
	public float GetTime()
	{ return time; }

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

    public void ExitGame()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneNum + " - current scene");
        if(sceneNum == 0)
        {
            Debug.Log("Close game!");
            Application.Quit();
        }
        else if (sceneNum == 1)
        {
            Debug.Log("Show menu!");
            GameMaster.TriggerEvent("ShowMenu");
        } 
        else
        {
            Debug.LogError("Invalid scene number when exiting game!");
            GameMaster.TriggerEvent("ShowMenu");
        }
    }
}
