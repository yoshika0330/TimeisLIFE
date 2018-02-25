using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform m_Transform;
    Camera m_Camera;

	[SerializeField]
	private float rotateSpeed = 60;
    
    Vector3 lerpedPosition;
	float maxUp = 80;
    float maxDown = 350;
    int moveCount = 3;

    void Start ()
    {
        m_Camera = Camera.main;
        m_Transform = GetComponent<Transform>();
        lerpedPosition = m_Transform.position;
		m_Camera.transform.position = (lerpedPosition + new Vector3 (0, 2, -5));
    }
	
	void Update ()
    {
		m_Camera.transform.position += transform.position - lerpedPosition;
		lerpedPosition = transform.position;

		// プレイヤー操作でスキルボタンが押されていなかったら
		if (!PlayerController.Instance.PushSkill && GameController.Instance.GAMEEVENT == GameController.GameEvent.NONE && GameController.Instance.GAMESTATE == GameController.GameState.PLAY)
		{
			// 左右の動き
			if (GamePadManager.Instance.GetAxisRaw (DS4Axis.RightStickX) < 0) 
			{
				//--------------------------------------------------------------------------------------
				// 設定によって動かす向きを変える
				if (UIController.Instance.cameraDirection_X_T.isOn) 
				{
					// 右移動
					Move (Vector3.up, rotateSpeed, UIController.Instance.cameraSensitivity_X.value);
				}
				else 
				{
					// 左移動
					Move (Vector3.up, -rotateSpeed, UIController.Instance.cameraSensitivity_X.value);
				}
				//--------------------------------------------------------------------------------------
			} 
			else if (GamePadManager.Instance.GetAxisRaw (DS4Axis.RightStickX) > 0) 
			{
				//--------------------------------------------------------------------------------------
				// 設定によって動かす向きを変える
				if (UIController.Instance.cameraDirection_X_T.isOn) 
				{
					// 左移動
					Move (Vector3.up, -rotateSpeed, UIController.Instance.cameraSensitivity_X.value);
				}
				else 
				{
					// 右移動
					Move (Vector3.up, rotateSpeed, UIController.Instance.cameraSensitivity_X.value);
				}
				//--------------------------------------------------------------------------------------
			}

            var moveRange = m_Camera.transform.localEulerAngles.x <= maxUp || m_Camera.transform.localEulerAngles.x >= maxDown;

            // 上下の動き
            if (GamePadManager.Instance.GetAxisRaw (DS4Axis.RightStickY) > 0) 
			{
                //----------------------------------------------------------------------------------------------------
                // 設定によって動かす向きを変える
                if (UIController.Instance.cameraDirection_Y_T.isOn && moveRange)
				{
					// 上移動
					Move (m_Camera.transform.right, rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
				} 
				else if(UIController.Instance.cameraDirection_Y_F.isOn && moveRange)
				{
					// 下移動
					Move (m_Camera.transform.right, -rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
				}
				//----------------------------------------------------------------------------------------------------
			} 
			else if (GamePadManager.Instance.GetAxisRaw (DS4Axis.RightStickY) < 0 ) 
			{
				//----------------------------------------------------------------------------------------------------
				// 設定によって動かす向きを変える
				if (UIController.Instance.cameraDirection_Y_T.isOn && moveRange) 
				{
					// 下移動
					Move (m_Camera.transform.right, -rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
				} 
				else if(UIController.Instance.cameraDirection_Y_F.isOn && moveRange)
				{
					// 上移動
					Move (m_Camera.transform.right, rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
				}
				//----------------------------------------------------------------------------------------------------
			}

            //-------------------------------------------------------------------------------------------------------------
            // カメラの位置補正
            if (m_Camera.transform.localEulerAngles.x > maxUp && m_Camera.transform.localEulerAngles.x < maxUp + 10)
            {
                while (m_Camera.transform.localEulerAngles.x > maxUp)
                {
                    Move(m_Camera.transform.right, -rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
                }
            }
            if(m_Camera.transform.localEulerAngles.x > maxDown - 10 && m_Camera.transform.localEulerAngles.x < maxDown)
            {
                while(m_Camera.transform.localEulerAngles.x < maxDown)
                {
                    Move(m_Camera.transform.right, rotateSpeed, UIController.Instance.cameraSensitivity_Y.value);
                }
            }
            //-------------------------------------------------------------------------------------------------------------

            // カメラの向いてる方向を取得
            Vector3 cameraForward = Vector3.Scale (m_Camera.transform.forward, new Vector3 (1, 0, 1)).normalized;

			if (GameController.Instance.GAMESTATE == GameController.GameState.PLAY) 
			{
				// カメラを近づける
				if (GamePadManager.Instance.GetKeyDown (DS4KeyCode.R1) && moveCount > 0)
				{
                    moveCount--;
					m_Camera.transform.position += cameraForward + transform.forward * 3 * Time.deltaTime;
				} 
			// カメラを遠ざける
			else if (GamePadManager.Instance.GetKeyDown (DS4KeyCode.L1) && moveCount < 6) 
				{
                    moveCount++;
					m_Camera.transform.position -= cameraForward + transform.forward * 3 * Time.deltaTime;
				}
			}
		}

		m_Camera.transform.LookAt (m_Transform.position + new Vector3(0,0.7f,0));
        
    }

	void Move(Vector3 moveAxis , float moveSpeed, float sensitivity)
	{
		m_Camera.transform.RotateAround (transform.position, moveAxis, moveSpeed * sensitivity * Time.deltaTime);
	}
    
}
