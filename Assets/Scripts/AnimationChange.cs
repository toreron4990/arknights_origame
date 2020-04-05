using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChange : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private PlayerMoveController pmController;

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //自身の速度が0以外(静止状態でない)の場合
        if (pmController.key != 0 || !pmController.isGround)
        {
            //アニメーションを Run に設定
            animator.Play("Run");
        }

        if (pmController.key == 0 && pmController.isGround)
        {
            animator.Play("Idle");
        }
    }
}
