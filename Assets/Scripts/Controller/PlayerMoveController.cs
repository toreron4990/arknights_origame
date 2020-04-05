using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
　  Rigidbody2D rb;
　　Animator animator;

    float jumpForce = 1000f;       // ジャンプ時に加える力
    float jumpThreshold = 1f;    // ジャンプ中か判定するための閾値
    float runForce = 40f;       // 走り始めに加える力
    float runThreshold = 20f;   // 速度切り替え判定のための閾値
    float runSpeed = 0.7f;       // 走っている間の速度
    bool isGround = true;        // 地面と接地しているか管理するフラグ
    int key = 0;                 // 左右の入力管理
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D> ();
        this.animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputKey();
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
		// 設置している時にXキー押下でジャンプ

		if (isGround) {
			if (Input.GetButton("Cross")) {
                isGround = false;
				this.rb.AddForce (transform.up * this.jumpForce);
                rb.velocity = new Vector3(rb.velocity.x,0,0);
                Debug.Log (rb.velocity);

			}
		}
		// 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
		float speedX = Mathf.Abs (this.rb.velocity.x);
        if (key == 0) {
            rb.velocity = new Vector3(rb.velocity.x * 0.95f,rb.velocity.y,0);
        }else{
		    if (speedX < this.runThreshold) {
            Debug.Log (transform.up);
                if (!isGround) {
                    //未入力の場合は key の値が0になるため移動しない
                    this.rb.AddForce (transform.right * key * this.runForce / 2);
                }else{
                    this.rb.AddForce (transform.right * key * this.runForce);
                }
		    } else {
                rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y,0);
                //this.transform.position += new Vector3 (runSpeed * Time.deltaTime * key , 0, 0);
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
