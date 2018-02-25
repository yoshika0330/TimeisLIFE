using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public enum GameState
	{
		TITLE,
		PLAY,
		PAUSE,
        RESULT
	}
	public GameState GAMESTATE;

    public enum GameDifficulty
    {
        EASY,
        NORMAL,
        HARD
    }
    public GameDifficulty GAMEDIFFICULTY;

    public enum GameEvent
    {
        NONE,
        START,
        END
    }
    public GameEvent GAMEEVENT;

    public static GameController Instance { get; private set;}
	void Awake()
	{
		if (Instance != null) 
		{
			enabled = false;
			DestroyImmediate (this);
			return;
		}
		Instance = this;
	}

    private bool onceViewResult = false;
    public Animator resultAnimation;
    public GameObject PauseUis;

    void Start () 
	{

	}
		
	void Update () 
	{
        if (GameEvent.NONE == GAMEEVENT && GameState.PLAY == GAMESTATE || GameState.PAUSE == GAMESTATE)
        {
            PushPause();
        }

        if(PlayerController.Instance.plManager.skillTime == 0)
        {
            GAMESTATE = GameState.RESULT;
        }

        if(GameState.RESULT == GAMESTATE)
        {
            if(!onceViewResult)
            {
                onceViewResult = true;
                PlayerController.Instance.isMove = false;
                resultAnimation.SetFloat("Speed", 1f);
            }
        }

        if(GameState.PAUSE == GAMESTATE && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Triangle))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

	void PushPause()
	{
		if (Input.GetKeyDown (KeyCode.P) || GamePadManager.Instance.GetKeyDown(DS4KeyCode.Options)) 
		{
			if (Time.timeScale == 1) 
			{
				Time.timeScale = 0;
				GAMESTATE = GameState.PAUSE;
                PauseUis.SetActive(true);
			}
			else if (Time.timeScale == 0) 
			{
				Time.timeScale = 1;
				GAMESTATE = GameState.PLAY;
                PauseUis.SetActive(false);
			}	
		}
	}


}
