using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	[SerializeField]private float health;
	[SerializeField]private float speed;
	[SerializeField]private float jumpForce;
	private float moveInput;
	private WaitForSeconds hitGapTime = new WaitForSeconds(1f);
	[SerializeField] private float gizmoHeight;
	[SerializeField] private float gizmoWidth;
	[SerializeField]float gizmoX;
	[SerializeField]float gizmoY;
	private Rigidbody2D rb2d;

	[SerializeField]private bool facingRight = true;
	[SerializeField] private bool grounded = false;
	private bool Jumping;
	
	public LayerMask groundLayer;
	private SpriteRenderer _spriteRenderer;
	private float jumpCounter;	  
	[SerializeField]private float jumpTime;
	// Use this for initialization
    

	[SerializeField]private bool isFalling;
	private WaitForSeconds blinkGap = new WaitForSeconds(0.05f);//Blinking sprite gap
	[SerializeField] private Text HPLABEL;
	private CameraLogic sceneCamera;
	private int weaponType;//무기 종류를 선택한다.(1,2,3)

    public GameObject shootPoint;
    private Transform shootTrans;

    Animator animator;

    public Transform groundCheck;
    public float groundCheckRadius;
    private bool canMove;

    


    void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shootTrans = shootPoint.GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }	

	void Start ()
	{
		sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLogic>();
		Physics2D.IgnoreLayerCollision(9,10,false);
        Physics2D.IgnoreLayerCollision(10, 11,false);
        health = GameManager.instance.p_HP;
		jumpForce = GameManager.instance.p_Jump;
		speed = GameManager.instance.p_Speed;
		weaponType = 1;
	}

    // Update is called once per frame

    [SerializeField] private float offset_rayR;
    [SerializeField] private float offset_rayL;
    bool isGrounded()
    {
       
        Vector2 rayR = new Vector2(transform.position.x - offset_rayR,transform.position.y);
        Vector2 rayL = new Vector2(transform.position.x + offset_rayL, transform.position.y);
        Vector2 direction = Vector2.down;
        float distance = 0.8f;
        RaycastHit2D hitR = Physics2D.Raycast(rayR, direction, distance, groundLayer);
        RaycastHit2D hitL = Physics2D.Raycast(rayL, direction, distance, groundLayer);
        Debug.DrawRay(rayR, direction, Color.red);
        Debug.DrawRay(rayL, direction, Color.blue);
        if (hitR.collider != null||hitL.collider !=null)
        {
            return true;
        }

        return false;
    }

	void Update ()
    {
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        // grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - gizmoX, transform.position.y - gizmoY),new Vector2(transform.position.x + gizmoX, transform.position.y -gizmoY), groundLayer);
        grounded = isGrounded();
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("Fall", isFalling);

        animator.SetBool("Grounded", grounded);

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Event")) { canMove = false; }
        else { canMove = true;
           
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            UImanager._instanceUiManager.changeFallState(isFalling);
        }
		if (grounded&&Input.GetKeyDown(KeyCode.Z)&&canMove)//최초 점프
		{
			Jumping = true;
			jumpCounter = jumpTime;
			rb2d.velocity = Vector2.up*jumpForce;
            animator.SetTrigger("Jump");
		}
		if (Input.GetKey(KeyCode.Z)&&Jumping)
		{
			if (jumpCounter > 0)
			{
				rb2d.velocity = Vector2.up
			                 *jumpForce;
				jumpCounter -= Time.deltaTime;
			}
			else
			{
				Jumping = false;
			}
		}

		if (Input.GetKeyUp(KeyCode.Z))
		{
			Jumping = false;
		}


        if (Input.GetKeyDown(KeyCode.C))
        {
            switchWeapon();
        }
	}	
    void switchWeapon()
    {
        if(weaponType == 1)
        {
            weaponType = 2;
        }
        else
        {
            weaponType = 1;
        }
        Debug.Log("현재 무기>> " + weaponType);
    }
   

	private void FixedUpdate()
	{
        if (canMove)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
           
        }else if (!canMove)
        {
            moveInput = 0;
        }

        rb2d.velocity = new Vector2(moveInput * speed, rb2d.velocity.y);


        if (!facingRight && moveInput > 0)
		{
			Flip();
		}else if (facingRight && moveInput < 0)
		{
			Flip();
		}

		if (rb2d.velocity.y < -0.1)
		{
            
			isFalling = true;
           
		}
		else
		{
			isFalling = false;
           
		}
        

	}

	IEnumerator	TakeDamageEffect()
	{
		int cntTime = 0;
		Physics2D.IgnoreLayerCollision(9,10,true);//무적의 시작
		while (cntTime<10)
		{
			if (cntTime % 2 == 0)
			{
				_spriteRenderer.color = new Color32(255,255,255,90);
			}
			else
			{
				_spriteRenderer.color = new Color32(255,255,255,180);
			}

			yield return blinkGap;
			cntTime++;
		}
		_spriteRenderer.color = new Color32(255,255,255,255);
		Physics2D.IgnoreLayerCollision(9,10,false);//무적의 끝
		yield return null;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EndPoint"))
		{
			GameManager.instance.structPlayerState(health,speed,jumpForce);
			GameManager.instance.LoadNextLevel();
		}
	}


    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(TakeDamageEffect());
        sceneCamera.ShakeCamera(0.15f, 0.3f);//Camera
        UImanager._instanceUiManager.ChangeHpLabel((int)health);//UI에 전달
        animator.SetTrigger("Damage");
       
        if (health <= 0)
		{
			PlayerDead();
        }   
	}

	public void PlayerDead()
	{
		Debug.Log("PlayerDead function is Called");
        Physics2D.IgnoreLayerCollision(10, 9);
        Physics2D.IgnoreLayerCollision(10, 11);

        Debug.Log("사망으로 인한 콜리전 비활성화");
        Invoke("abc", 2);
	}
    void abc()
    {
        GameManager.instance.DecreaseExtraLife();
    }
	
	
	

	

	void Flip()
	{
        facingRight = !facingRight;
        transform.Rotate(0f,180f, 0f);
        if (!facingRight) { offset_rayL = offset_rayL * -1; offset_rayR = offset_rayR * -1; }
        else if (facingRight) { offset_rayL = Mathf.Abs(offset_rayL); offset_rayR = Mathf.Abs(offset_rayR); }
        //shootTrans.Rotate(0f,180f,0f);
        /*Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        transform.Rotate(0f, 180f, 0f);*/
    }
}	
