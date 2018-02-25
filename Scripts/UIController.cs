using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

	public Toggle cameraDirection_X_T;
	public Toggle cameraDirection_X_F;
	public Toggle cameraDirection_Y_T;
	public Toggle cameraDirection_Y_F;
	public Slider cameraSensitivity_X;
	public Slider cameraSensitivity_Y;

	public GameObject timeUis;

	public static UIController Instance { get; private set;}
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

	void Start () 
	{
        cameraDirection_X_T.Select();
    }

	void Update () 
	{
		DispConfig ();
	}

	void DispConfig()
	{
		switch (GameController.Instance.GAMESTATE) 
		{
		case GameController.GameState.PLAY:
			timeUis.SetActive (true);
			//--------------------------------------
			// それぞれのカメラの設定項目のenabledをfalseに
			cameraDirection_X_T.enabled = false;
			cameraDirection_X_F.enabled = false;
			cameraDirection_Y_T.enabled = false;
			cameraDirection_Y_F.enabled = false;
			cameraSensitivity_X.enabled = false;
			cameraSensitivity_Y.enabled = false;
			//--------------------------------------
			break;
		case GameController.GameState.PAUSE:
			timeUis.SetActive (false);
			//--------------------------------------
			// それぞれのカメラの設定項目のenabledをtrueに
			cameraDirection_X_T.enabled = true;
			cameraDirection_X_F.enabled = true;
			cameraDirection_Y_T.enabled = true;
			cameraDirection_Y_F.enabled = true;
			cameraSensitivity_X.enabled = true;
			cameraSensitivity_Y.enabled = true;
            //--------------------------------------
                if (EventSystem.current.currentSelectedGameObject.name == cameraDirection_X_T.name)
                {
                    if(GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_X_F.Select();
                    }
                    if(GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
                    {
                        cameraDirection_X_T.isOn = true;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == cameraDirection_X_F.name)
                {
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_Y_T.Select();
                    }
                    else if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_X_T.Select();
                    }
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
                    {
                        cameraDirection_X_F.isOn = true;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == cameraDirection_Y_T.name)
                {
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_Y_F.Select();
                    }
                    else if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_X_F.Select();
                    }
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
                    {
                        cameraDirection_Y_T.isOn = true;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == cameraDirection_Y_F.name)
                {
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraSensitivity_X.Select();
                    }
                    else if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_Y_T.Select();
                    }
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Circle))
                    {
                        cameraDirection_Y_F.isOn = true;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == cameraSensitivity_X.name)
                {
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Down))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraSensitivity_Y.Select();
                    }
                    else if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraDirection_Y_F.Select();
                    }
                    if (cameraSensitivity_X.value < 1  && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Right))
                    {
                        cameraSensitivity_X.value += 0.1f;
                    }
                    else if(cameraSensitivity_X.value > 0 && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Left))
                    {
                        cameraSensitivity_X.value -= 0.1f;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == cameraSensitivity_Y.name)
                {
                    if (GamePadManager.Instance.GetKeyDown(DS4KeyCode.Up))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        cameraSensitivity_X.Select();
                    }
                    if (cameraSensitivity_Y.value < 1f && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Right))
                    {
                        cameraSensitivity_Y.value += 0.1f;
                    }
                    else if (cameraSensitivity_Y.value > 0 && GamePadManager.Instance.GetKeyDown(DS4KeyCode.Left))
                    {
                        cameraSensitivity_Y.value -= 0.1f;
                    }
                }
                break;
		default:
			break;
		}
	}
}
