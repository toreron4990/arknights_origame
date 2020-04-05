using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChange : MonoBehaviour
{
    private Animator animator;
    private GameObject refObj;
    private PlayerMoveController pmController;
    private AnimatorStateInfo animInfo;


    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        refObj = transform.root.gameObject;
        pmController = refObj.GetComponent<PlayerMoveController>();
    }

    bool animFlag = false;

    // Update is called once per frame
    void Update()
    {
        //自身の速度が0以外(静止状態でない)の場合
        if (pmController.key != 0 || !pmController.isGround)
        {
            //アニメーションを Run に設定
            animator.Play("Run");
        }

        animInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animInfo.fullPathHash == Animator.StringToHash("Base Layer.Run"))
        {
            if (pmController.key == 0 && pmController.isGround)
            {
                animator.Play("Idle");
                animFlag = false;
            }
        }
        else
        {
            if (pmController.key == 0 && pmController.isGround && !animFlag)
            {
                animator.Play("Idle");
                animFlag = false;
            }
        }

       

        if (Input.GetButton("Square") && pmController.isGround)
        {
            animator.Play("Attack_N");
            animFlag = true;
        }
    }

    public void AnimFlagFalse()
    {
        animFlag = false;
    }
}
