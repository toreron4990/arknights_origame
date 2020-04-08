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

    //通常攻撃関係変数
    private bool attackAnimFlag = false;
    private string attackName;

    //スキル関係変数
    private bool skillAnimFlag = false;
    private float skillChargeCount = 0.0f;
    private float skillChargeCountMax = 2.0f;
    private string skillName;



    void Update()
    {
        //スキルチャージ時間を超えた場合
        if (skillChargeCount >= skillChargeCountMax)
        {

        }
        //一度でもチャージが始まっていれば
        else if(skillChargeCount != 0)
        {
            //時間を追加
            skillChargeCount += Time.deltaTime;
        }
        //スキルチャージが始まっていない時(スキルが発動していない時)
        else
        {
            //通常攻撃ボタンが押された時 かつ　攻撃フラグが立っていなければ
            if (Input.GetButtonDown("Square") && attackAnimFlag == false)
            {
                //X軸入力がある時
                if (Input.GetAxis("X axis") == 1 || Input.GetAxis("X axis") == -1)
                {
                    StartAttackAnimation("Attack_N", "Attack_N");
                }
                //X軸入力がない時
                else
                {
                    //Y軸入力が上
                    if (Input.GetAxis("Y axis") == 1)
                    {
                        StartAttackAnimation("Attack_N", "Attack_N");
                    }
                    //Y軸入力が下
                    else if (Input.GetAxis("Y axis") == -1)
                    {
                        StartAttackAnimation("Attack_N", "Attack_N");
                    }
                    //いずれの入力もないニュートラルの時
                    else
                    {
                        StartAttackAnimation("Attack_N","Attack_N");
                    }
                }

            }

            //スキルボタンが押された時
            if (Input.GetButton("Circle"))
            {
                //時間を追加
                skillChargeCount += Time.deltaTime;
                //通常攻撃アニメのフラグを解除(スキルで通常攻撃をキャンセル可能)
                attackAnimFlag = false;
            }

        }

        //攻撃またはスキル実行中じゃないなら
        if (!attackAnimFlag && !skillAnimFlag)
        {
            pmController.canMove = true;
            //移動入力があるなら
            if (pmController.key != 0)
            {
                animator.Play("Run");
            }
            //入力がない時
            else
            {
                animator.Play("Idle");
            }
        }
    }

    //通常攻撃アニメーション実行処理
    private void StartAttackAnimation(string groundName, string airName)
    {
        attackName = pmController.isGround ? groundName : airName;
        animator.Play(attackName);
        attackAnimFlag = true;
        pmController.canMove = false;
    }

    //スキルアニメーション実行処理
    private void StartSkillAnimation(string groundName, string airName)
    {
        skillName = pmController.isGround ? groundName : airName;
        animator.Play(skillName);
        skillAnimFlag = true;
        pmController.canMove = false;
    }

    //アニメーション終了時に呼び出す関数 ***もしキャンセル時にスキルが消えるならこの処理を変える
    public void AnimFlagFalse()
    {
        attackAnimFlag = false;
        skillAnimFlag = false;
        skillChargeCount = 0.0f;
    }
}
