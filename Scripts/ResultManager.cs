using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {

    public PlayerManager plManager;

    public Text timeText;
    public GameObject timeUis;

    public Image[] starImg;
    public Sprite[] starSprite;

    public Image gameEndImg;
    public Sprite[] gameEndSprite;
   
    private float alpha;

    [SerializeField]
    private int rank_S = 20;
    [SerializeField]
    private int rank_A = 15;
    [SerializeField]
    private int rank_B = 10;
    [SerializeField]
    private int rank_C = 5;
    [SerializeField]
    private int rank_D = 1;

    private bool changeSceneOnce = false;
    private bool isEnd = false;

    void Start ()
    {
        
	}
	

	void Update ()
    {

        if(GameController.Instance.GAMESTATE== GameController.GameState.RESULT)
        {
            Evaluation();
            timeText.text = (plManager.skillTime).ToString() + ":00";
            timeUis.SetActive(false);
        }

        if(isEnd)
        {
            if(GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            Evaluation();
            timeText.text = (plManager.skillTime).ToString() + ":00";
        }
	}
    
    void Evaluation()
    {
        if(plManager.skillTime >= rank_S)
        {
            for(int i = 0; i < 5; i++)
            {
                changeStar(i, 0);
            }
        }
        else if (plManager.skillTime >= rank_A)
        {
           for(int i = 0; i < 4;i++)
           {
                changeStar(i, 0);
           }
            changeStar(4, 1);
        }
        else if (plManager.skillTime >= rank_B)
        {
            for (int i = 0; i < 3; i++)
            {
                changeStar(i, 0);
            }
            for (int i = 3; i < 5; i++)
            {
                changeStar(i, 1);
            }
        }
        else if (plManager.skillTime >= rank_C)
        {
            for (int i = 0; i < 2; i++)
            {
                changeStar(i, 0);
            }
            for (int i = 2; i < 5; i++)
            {
                changeStar(i, 1);
            }
        }
        else if (plManager.skillTime >= rank_D)
        {
            for (int i = 1; i < 5; i++)
            {
                changeStar(i, 1);
            }
            changeStar(0, 0);
        }
        else if(plManager.skillTime == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                changeStar(i, 1);
            }
            gameEndImg.sprite = gameEndSprite[1];
        }
    }

    void changeStar(int imgNum,int spriteNum)
    {
        starImg[imgNum].sprite = starSprite[spriteNum];
    }

    void EndAnimation()
    {
        isEnd = true;
    }

}
