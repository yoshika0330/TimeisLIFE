using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyController : MonoBehaviour
{

    [SerializeField]
    private Sprite[] easySprite = new Sprite[2];
    [SerializeField]
    private Sprite[] normalSprite = new Sprite[2];
    [SerializeField]
    private Sprite[] hardSprite = new Sprite[2];

    [SerializeField]
    private GameObject easyObj;
    [SerializeField]
    private GameObject normalObj;
    [SerializeField]
    private GameObject hardObj;

    private Image easyImage;
    private Image normalImage;
    private Image hardImage;

    private bool changeSceneOnce = false;

    void Start()
    {
        easyImage = easyObj.GetComponent<Image>();
        normalImage = normalObj.GetComponent<Image>();
        hardImage = hardObj.GetComponent<Image>();

        easyImage.sprite = easySprite[1];
        GameController.Instance.GAMEDIFFICULTY = GameController.GameDifficulty.EASY;

    }

    void Update()
    {
        if (GameController.Instance.GAMEDIFFICULTY == GameController.GameDifficulty.EASY)
        {
            if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
            {
                GameController.Instance.GAMEDIFFICULTY = GameController.GameDifficulty.NORMAL;
                easyImage.sprite = easySprite[0];
                normalImage.sprite = normalSprite[1];
            }
        }
        else if (GameController.Instance.GAMEDIFFICULTY == GameController.GameDifficulty.NORMAL)
        {
            if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
            {
                GameController.Instance.GAMEDIFFICULTY = GameController.GameDifficulty.HARD;
                normalImage.sprite = normalSprite[0];
                hardImage.sprite = hardSprite[1];
            }
            else if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
            {
                GameController.Instance.GAMEDIFFICULTY = GameController.GameDifficulty.EASY;
                easyImage.sprite = easySprite[1];
                normalImage.sprite = normalSprite[0];
            }
        }
        else if (GameController.Instance.GAMEDIFFICULTY == GameController.GameDifficulty.HARD)
        {
            if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
            {
                GameController.Instance.GAMEDIFFICULTY = GameController.GameDifficulty.NORMAL;
                normalImage.sprite = normalSprite[1];
                hardImage.sprite = hardSprite[0];
            }
        }

        // シーンを移動させる
        if(GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
        {
            StartCoroutine(Fade.Instance.FadeOut());
            SoundManager.Instance.PlayTitlePushSE();
        }

        // フェードが終わってからシーンを読み込む
        if (Fade.Instance.Alpha >= 1 && !changeSceneOnce)
        {
            changeSceneOnce = true;
            StartCoroutine(SceneContoller.Instance.LoadScene());
        }
    }
}
