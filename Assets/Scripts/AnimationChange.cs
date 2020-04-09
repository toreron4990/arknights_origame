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
    private float skillChargeCountMax = 1.0f;
    private string skillName;



    void Update()
    {
        //スキルチャージ時間を超えた場合
        if (skillChargeCount >= skillChargeCountMax || (skillChargeCount != 0 && Input.GetButtonUp("Circle")) )
        {
            //対応したスキルをプレイ
            animator.PlayInFixedTime(skillName, 0, 0f);
            //攻撃時のアクション
            OnlyOnceAction(attackName);
            //カウントリセット
            skillChargeCount = 0;
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
            if (Input.GetButtonDown("Square") && attackAnimFlag == false && skillAnimFlag == false)
            {
                //X軸入力がある時
                if (Input.GetAxis("X axis") == 1 || Input.GetAxis("X axis") == -1)
                {
                    StartAttackAnimation("Attack_Up", "Attack_Up");
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
                        StartAttackAnimation("Attack_N", "Attack_Up");
                    }
                    //いずれの入力もないニュートラルの時
                    else
                    {
                        StartAttackAnimation("Attack_N","Attack_N");
                    }
                }

            }

            //スキルボタンが押された時
            if (Input.GetButtonDown("Circle") && skillAnimFlag == false)
            {
                //時間を追加
                skillChargeCount += Time.deltaTime;
                //通常攻撃アニメのフラグを解除(スキルで通常攻撃をキャンセル可能)
                attackAnimFlag = false;
                attackName = "";

                //X軸入力がある時
                if (Input.GetAxis("X axis") == 1 || Input.GetAxis("X axis") == -1)
                {
                    SkillNameSet("Attack_N", "Attack_N");
                }
                //X軸入力がない時
                else
                {
                    //Y軸入力が上
                    if (Input.GetAxis("Y axis") == 1)
                    {
                        SkillNameSet("Attack_N", "Attack_N");
                    }
                    //Y軸入力が下
                    else if (Input.GetAxis("Y axis") == -1)
                    {
                        SkillNameSet("Attack_N", "Attack_N");
                    }
                    //いずれの入力もないニュートラルの時
                    else
                    {
                        SkillNameSet("Attack_N", "Attack_N");
                    }
                }
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
        animator.PlayInFixedTime(attackName, 0, 0f);
        //攻撃時のアクション
        OnlyOnceAction(attackName);
        //フラグ処理
        attackAnimFlag = true;
        pmController.canMove = false;
    }

    //スキルアニメーション名セット
    private void SkillNameSet(string groundName, string airName)
    {
        skillName = pmController.isGround ? groundName : airName;
        animator.PlayInFixedTime(skillName + "_Charge", 0, 0f);
        //フラグ処理
        skillAnimFlag = true;
        pmController.canMove = false;
    }

    //アニメーション終了時に呼び出す関数
    public void AnimFlagFalse()
    {
        attackAnimFlag = false;
        attackName = "";
        skillAnimFlag = false;
        skillName = "";
        skillChargeCount = 0.0f;
    }

    //
    private void OnlyOnceAction(string name)
    {
        switch (name)
        {
            case "Attack_Up":
                //ジャンプ直後の場合か否か
                if (!pmController.jumpNow)
                {
                    pmController.isGround = false;
                    pmController.rb.velocity = new Vector3(pmController.rb.velocity.x, 0, 0);
                    pmController.rb.AddForce(transform.up * 1500f);
                }
                else
                {
                    //Forceが二重にかかるのを防ぐ
                    pmController.rb.AddForce(transform.up * (1500f - pmController.jumpForce));
                }
                
                break;

            default:
                break;
        } 
    }
}
