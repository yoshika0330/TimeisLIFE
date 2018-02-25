using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerManager plManager;
    public float objForward = 2.0f;

    private GameObjHighlight gameObjHighlight;

    private Rigidbody rb;

    public GameObject obj;
    public GameObject timeEffect;

    private Vector2 leftStick;
    private Vector2 rightStick;
    private Vector2 moveLeftStick;

    private int startTime;
    public int timeToReduce = 20;

    private float titleWaitTime;
    private float elapsedTime;
    private float checkPos;
    private float rightStickMove;
    private float timeDecreaseCount = 0;
    private float r_StickRotationCount;
    public float R_StickRotationCount
    {
        get { return r_StickRotationCount; }
    }
    private float leftStickSlope;
    public float LeftStickSlope
    {
        get { return leftStickSlope; }
    }

    private bool isStand = false;
    public bool IsStand
    {
           get { return isStand; }
    }
    [HideInInspector]
	public bool isMove = true;
	private bool firstPosCheck = false;
	private bool timeDecreaseOnce = false;
	private bool pushSkill = false;
	public bool PushSkill
	{
		get { return pushSkill;}
	}

	private bool pushJump = false;
	public bool PushJump
	{
		get { return pushJump;}
	}
	private bool isResetGOHighlight = false;
	public bool IsResetGOHighlight
	{
		set { this.isResetGOHighlight = value; }
	}
    private bool onceWalkSe = false;
    private bool onceRunSe = false;
    private bool onceGetStartTime = false;
    private bool onceDeleteTime = false;

    private GameObject mHoldingGO;
    private bool mIsStopRaycast = false;

	public static PlayerController Instance { get;private set;}
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
		rb = GetComponent<Rigidbody> ();
		plManager.skillTime = 72;
	}

	void Update () 
	{

        elapsedTime += Time.deltaTime;

		CheckInput ();
		CheckDistanceGimmick ();
        CheckGround();
        TimeDecreases();

	}

	void FixedUpdate()
	{
		if (isMove) 
		{
			Move ();
		}
        else
        {
            SoundManager.Instance.StopMoveSE();
        }
	}

    void TimeDecreases()
    {
        if (GameController.GameState.TITLE == GameController.Instance.GAMESTATE)
        {
            titleWaitTime += Time.deltaTime;
        }
        if (GameController.GameState.PLAY == GameController.Instance.GAMESTATE)
        {
            if (!onceGetStartTime)
            {
                onceGetStartTime = true;
                startTime = (int)titleWaitTime;
            }

            if (((int)elapsedTime - startTime) >= timeToReduce && ((int)elapsedTime - startTime) % timeToReduce == 0 && !onceDeleteTime)
            {
                onceDeleteTime = true;
                plManager.skillTime--;
            }

            else if (((int)elapsedTime - startTime) % timeToReduce >= 1)
            {
                onceDeleteTime = false;
            }
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.25f, -transform.up, out hit,0.3f))
        {
            if (hit.collider.tag == "Ground")
            {
                isStand = true;
            }
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.up * 0.25f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.5f, 0) + -transform.up * (0.3f), 0.25f);
    }

    void Move()
	{
		Vector3 cameraForward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;

		if (leftStick != Vector2.zero) 
		{
            if (isStand)
            {
                if (!onceWalkSe)
                {
                    onceWalkSe = true;
                    SoundManager.Instance.PlayWalkSE();
                }

                if (leftStickSlope >= 0.4f && !onceRunSe)
                {
                    onceRunSe = true;
                    SoundManager.Instance.PlayRunSE();
                }

                //----------------------------------------------------------------------------------------------------------
                // 移動中に歩く速度が変わったらその速度によって移動時の音を変える
                if (leftStickSlope < 0.4f && SoundManager.Instance.PlayerMoveSource.clip == SoundManager.Instance.runSe)
                {
                    SoundManager.Instance.PlayWalkSE();
                }
                else if(leftStickSlope >= 0.4f && SoundManager.Instance.PlayerMoveSource.clip == SoundManager.Instance.walkSe)
                {
                    SoundManager.Instance.PlayRunSE();
                }
                //----------------------------------------------------------------------------------------------------------
            }

            // キャラの向いている方向が違う方向を向こうとしたら
            if (moveLeftStick != leftStick) 
			{
				moveLeftStick = leftStick;
				//var _direction = new Vector3 (leftStick.x, 0, leftStick.y);
				// カメラの向きを基準にキャラの向きを変える
				var _direction = cameraForward * leftStick.y + Camera.main.transform.right * leftStick.x;
				transform.localRotation = Quaternion.LookRotation (_direction);
			}
			// 左のスティックの合計の傾きを取得
			leftStickSlope = Mathf.Abs (leftStick.x) + Mathf.Abs (leftStick.y);
			// 想定以上の大きさになった場合の回避
			if (leftStickSlope > 1) 
			{
				leftStickSlope = 1;
			}
			// 左スティックの傾き具合によって移動速度を変化
			transform.position += transform.forward * plManager.moveSpeed * leftStickSlope * Time.deltaTime;
		}
		else 
		{
			leftStickSlope = 0;
            if (onceWalkSe || onceRunSe)
            {
                onceWalkSe = false;
                onceRunSe = false;
                SoundManager.Instance.StopMoveSE();
            }
		}

        Jump();
		
	}

    void Jump()
    {
        // キャラをジャンプさせる
        if (isStand && pushJump)
        {
            isStand = false;
            rb.AddForce(Vector3.up * plManager.jumpPow, ForceMode.VelocityChange);

            //　移動時のSEを止める
            SoundManager.Instance.StopMoveSE();
            //-------------------
            // フラグの初期化
            onceWalkSe = false;
            onceRunSe = false;
            //-------------------
        }
    }

	// ボタン入力の確認
	void CheckInput()
	{
		// 左スティックの傾きをそれぞれ取得
		leftStick.x = GamePadManager.Instance.GetAxisRaw (DS4Axis.LeftStickX);
		leftStick.y = GamePadManager.Instance.GetAxisRaw (DS4Axis.LeftStickY);
		// 右スティックの傾きをそれぞれ取得
		rightStick.x = GamePadManager.Instance.GetAxisRaw(DS4Axis.RightStickX);
		rightStick.y = GamePadManager.Instance.GetAxisRaw(DS4Axis.RightStickY);

		// コントロラーのXかスペースキーが押されたらpushJumpをtrueにする
		if (GamePadManager.Instance.GetKeyDown (DS4KeyCode.Cross) || Input.GetKeyDown(KeyCode.Space)) 
		{
			pushJump = true;
		}
		// 初期化
		else 
		{
			pushJump = false;
		}

        if (GameController.Instance.GAMESTATE == GameController.GameState.PLAY && GameController.Instance.GAMEEVENT == GameController.GameEvent.NONE)
        {
            // コントロラーのL2か左のShiftが押されたらpushSkillをtrueにする
            if (GamePadManager.Instance.GetKey(DS4KeyCode.L2) || Input.GetKey(KeyCode.LeftShift))
            {
                if (gameObjHighlight != null && gameObjHighlight.enabled)
                {
                    pushSkill = true;
                    // プレイヤーが動かないようにする
                    isMove = false;
                    GetR_StickRotate();
                    timeEffect.SetActive(true);
                    if (Input.GetKey(KeyCode.Return))
                    {
                        r_StickRotationCount += 1;
                        Debug.Log(r_StickRotationCount);
                        plManager.skillTime--;
                    }
                }
            }
            // 初期化
            if (GamePadManager.Instance.GetKeyUp(DS4KeyCode.L2))
            {
                pushSkill = false;
                isMove = true;
                checkPos = 0;
                firstPosCheck = false;
                rightStickMove = 0;
                r_StickRotationCount = 0;
                timeDecreaseOnce = false;
                timeEffect.SetActive(false);
            }
        }
			
	}	

	// 右スティックの回転を取得する処理
	void GetR_StickRotate()
	{
		float rightStickRotation;
		// 右スティックの入力があったら
		if (rightStick != Vector2.zero) 
		{
			// スティックの傾きをオブジェの回転に変更する
			var r_direction = new Vector3 (rightStick.x, 0, rightStick.y);
			obj.transform.localRotation = Quaternion.LookRotation (r_direction);
			// オブジェの傾きからスティックの傾きを取得
			rightStickRotation = obj.transform.localEulerAngles.y;
			// 最初に入力があった時一回だけ呼ばれる
			if (!firstPosCheck) 
			{
				firstPosCheck = true;
				// 最初のスティックの傾きを取得
				checkPos = rightStickRotation;
			}

			// スティックの傾きがcheckPosを超えたら
			if (rightStickRotation > checkPos) 
			{
				// 大きすぎる移動が起きないようにする（バグ防止）
				if ((rightStickRotation - checkPos) < 300)
				{
					// chekPosから動いた分、移動量として取得
					rightStickMove += (rightStickRotation - checkPos);
					// 動いた位置を次のcheckPosとする
					checkPos = rightStickRotation;
				}
			}

			// 傾きが360を越えると0に戻ってしまうのでその時の特別な処理
			else if (checkPos >= 320 && rightStickRotation <= 5) 
			{
				if ((rightStickRotation + 360) > checkPos) 
				{
					rightStickMove += ((rightStickRotation+360) - checkPos);
					checkPos = rightStickRotation;
				}
			}
				
			if (rightStickMove / 360 >= 1) 
			{
				// 回転数を取得
				r_StickRotationCount = Mathf.Floor ((rightStickMove / 360));
				// 右スティックを一回転させたらスキルタイムを減らす
				// 一回だけ呼ばれるように
				if (!timeDecreaseOnce) 
				{
					timeDecreaseOnce = true;
					// 減らした時の回転数を保持
					timeDecreaseCount = r_StickRotationCount;
					plManager.skillTime--;
				}
				// 保持していた回転数と更新された回転数を比較して
				if (r_StickRotationCount > timeDecreaseCount) 
				{
					// 初期化
					timeDecreaseOnce = false;
				}
			}
		}
	}

	// ギミックの近くに居るかを判定する処理
	void CheckDistanceGimmick()
	{
        if (!mIsStopRaycast)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hit, plManager.gimmickDistance))
            {
				if (hit.collider.GetComponent<GameObjHighlight> () != null &&
				                hit.collider.GetComponent<GameObjHighlight> ().enabled != false) 
				{
					gameObjHighlight = hit.collider.GetComponent<GameObjHighlight> ();
					gameObjHighlight.SetHighlightMaterial ();

					// もし左クリックしたらタグをチェックして動けるようになってます。
					if (Input.GetMouseButton (0) && hit.collider.tag == "Movable") {
						mHoldingGO = hit.collider.gameObject;
						mIsStopRaycast = true;
					}
				} 
            }
			else 
			{
				if (!pushSkill && gameObjHighlight != null) 
				{
					gameObjHighlight.SetDefaultMaterial ();
					gameObjHighlight = null;
				}
			}
        }
        else
        {
            gameObjHighlight = null;
            if (Input.GetMouseButtonUp(0)) mIsStopRaycast = false;

            // 物はプレーヤーの前にします。
            Vector3 tempPos = transform.position + (transform.forward * objForward);
            mHoldingGO.transform.position = tempPos;
        }
        
	}

	void OnCollisionExit(Collision collision)
	{
		switch (collision.gameObject.tag) 
		{
		case "Ground":
			isStand = false;
            SoundManager.Instance.StopMoveSE();
            //-------------------
            // フラグの初期化
            onceWalkSe = false;
            onceRunSe = false;
            //-------------------
                break;
		default:
			break;
		}
	}
	// ----------------------------------------------
		
}
