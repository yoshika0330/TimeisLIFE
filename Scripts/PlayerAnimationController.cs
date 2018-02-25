using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

	private Animator animator;

	void Start () 
	{
		animator = GetComponent<Animator> ();
	}

	void Update () 
	{
		//------------------------------------------------------------------------------
		// 移動しているときに歩きor走るモーションに切り替える
		if (PlayerController.Instance.LeftStickSlope >= 0.1f) 
		{
			animator.SetFloat ("MoveSpeed", PlayerController.Instance.LeftStickSlope);
		}
		else 
		{
			animator.SetFloat ("MoveSpeed", 0);
		}

        if(!PlayerController.Instance.isMove)
        {
            animator.SetFloat("MoveSpeed", 0);
        }
		//------------------------------------------------------------------------------

		//---------------------------------------
		// ジャンプしているときにジャンプのモーションに切り替える
		if (PlayerController.Instance.PushJump) 
		{
			animator.SetBool ("Jump", true);
		}
		if(PlayerController.Instance.IsStand)
		{
			animator.SetBool ("Jump", false);
            animator.SetBool("Fall", false);
		}

        if(!PlayerController.Instance.PushJump && !PlayerController.Instance.IsStand)
        {
            animator.SetBool("Fall", true);
        }
        //---------------------------------------

        //----------------------------------------------
        // 時間を早めてるときにモーションを変える
        if(PlayerController.Instance.PushSkill)
        {
            animator.SetBool("Skill", true);
        }
        if (!PlayerController.Instance.PushSkill)
        {
            animator.SetBool("Skill", false);
        }
        //----------------------------------------------

    }
}
