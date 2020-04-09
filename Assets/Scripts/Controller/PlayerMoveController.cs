using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
　  public Rigidbody2D rb;
　　Animator animator;

    float dodgeForce = 1500f; //回避時に加える力
    float dodgeCoolTime = 0.0f; //回避のクールタイム
    float dodgeCoolTimeMax = 0.5f; //回避のクールタイム最大値
    public float jumpForce = 1000f;       // ジャンプ時に加える力
    public bool jumpNow = false;       // ジャンプした直後判定
    float runForce = 100f;       // 走り始めに加える力
    float runThreshold = 20f;   // 速度切り替え判定のための閾値
    public bool isGround = true;        // 地面と接地しているか管理するフラグ
    public int key = 0;                 // 左右の入力管理
    private float deceleration = 0.9f; //減速係数

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D> ();
        this.animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        CoolTimeDown();

        if (canMove)
        {
            GetInputKey();
        }

        Move();
    }
    
    //クールタイムの減少処理
    private void CoolTimeDown()
    {
        if(dodgeCoolTime > 0)
        {
            dodgeCoolTime -= Time.deltaTime;
        }
    }

    void GetInputKey(){
        key = 0;
        if (Input.GetAxisRaw("X axis") > 0){
            key = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxisRaw("X axis") < 0){
	        key = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetAxisRaw("X axis") == 0){
	        key = 0;
        }
	}

    void Move(){

        jumpNow = false;

        //スキル発動中で移動不可の場合
        if (!canMove)
        {
            //移動方向を0に
            key = 0;
        }
        //通常時
        else
        {
            // 設置している時
            if (isGround)
            {
                //R1,L1押下でダッシュ回避
                if ((Input.GetButtonDown("R1") || Input.GetButtonDown("L1")) && key != 0 && dodgeCoolTime <= 0)
                {
                    //ダッシュが逆方向なら一度速度を0にする
                    if ((key == -1 && rb.velocity.x > 0) || (key == 1 && rb.velocity.x < 0))
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                    this.rb.AddForce(transform.right * dodgeForce * key);

                    dodgeCoolTime = dodgeCoolTimeMax;
                }

                //Xキー押下でジャンプ
                if (Input.GetButtonDown("Cross"))
                {
                    isGround = false;
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                    rb.AddForce(transform.up * jumpForce);
                    jumpNow = true;
                }
            }


        }

        // 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
        float speedX = Mathf.Abs (this.rb.velocity.x);
        if (key == 0) {
            rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y,0);
        }else{
		    if (speedX < runThreshold) {
                //空中の場合 加速を抑える
                if (!isGround) {
                    //未入力の場合は key の値が0になるため移動しない
                    this.rb.AddForce (transform.right * key * runForce / 2);
                }else{
                    this.rb.AddForce (transform.right * key * runForce);
                }
		    } else {
                rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y,0);
            }
        }

        //移動方向が速度に対して逆入力されている時
        if ( (key == -1 && rb.velocity.x > 0) || (key == 1 && rb.velocity.x < 0) )
        {
            rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y, 0);
        }

	}

    // 着地判定
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Ground") {
			if (!isGround)
				isGround = true;
		}
	}
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Ground") {
			if(!isGround)
				isGround = true;
		}
	}
}
