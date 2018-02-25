using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour {

    public GameObject title;
    public Image titleImg;
    private bool onceStart = false;
    private bool isStart = false;

	void Start ()
    {
        PlayerController.Instance.isMove = false;
	}
	
	
	void Update ()
    {
		if(GameController.Instance.GAMESTATE == GameController.GameState.TITLE )
        {
            if (!onceStart && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
            {
                onceStart = true;
                SoundManager.Instance.PlayTitlePushSE();
                //StartCoroutine(FedaTitle());
                StartCoroutine( Fade.Instance.FadeOut());
            }

            if(Fade.Instance.Alpha >= 1)
            {
                title.SetActive(false);
                StartCoroutine( Fade.Instance.FadeIn());
                isStart = true;
           }

            if (isStart && Fade.Instance.Alpha <= 0.1f)
            {
                GameController.Instance.GAMESTATE = GameController.GameState.PLAY;
                GameController.Instance.GAMEEVENT = GameController.GameEvent.START;
            }
        }
	}

    IEnumerator FedaTitle()
    {
        for(float alpha = 1; alpha >= 0; alpha -= 0.02f)
        {
            titleImg.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        PlayerController.Instance.isMove = true;
        GameController.Instance.GAMESTATE = GameController.GameState.PLAY;
    }
}
