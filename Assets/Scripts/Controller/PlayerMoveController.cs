using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
　  Rigidbody2D rb;
　　Animator animator;

    float jumpForce = 1000f;       // ジャンプ時に加える力
    float runForce = 100f;       // 走り始めに加える力
    float runThreshold = 20f;   // 速度切り替え判定のための閾値
    public bool isGround = true;        // 地面と接地しているか管理するフラグ
    public int key = 0;                 // 左右の入力管理
    private float deceleration = 0.9f;

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

        if (canMove)
        {
            GetInputKey();
        }
        Move();
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

        //スキル発動中で移動不可の場合
        if (!canMove)
        {
            //移動方向を0に
            key = 0;
            //減速係数を軽減する
            deceleration = 0.97f;
        }
        else
        {
            deceleration = 0.9f;
        }

        // 設置している時にXキー押下でジャンプ
        if (isGround) {
			if (Input.GetButton("Cross")) {
                isGround = false;
				this.rb.AddForce (transform.up * this.jumpForce);
                rb.velocity = new Vector3(rb.velocity.x,0,0);
			}
		}
		// 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
		float speedX = Mathf.Abs (this.rb.velocity.x);
        if (key == 0) {
            rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y,0);
        }else{
		    if (speedX < this.runThreshold) {
                if (!isGround) {
                    //未入力の場合は key の値が0になるため移動しない
                    this.rb.AddForce (transform.right * key * this.runForce / 2);
                }else{
                    this.rb.AddForce (transform.right * key * this.runForce);
                }
		    } else {
                rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y,0);
            }
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
