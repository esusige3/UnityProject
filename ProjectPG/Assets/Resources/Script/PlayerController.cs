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

	private bool facingRight = true;
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
	
	
	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}	

	void Start ()
	{
        shootTrans = shootPoint.GetComponent<Transform>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLogic>();
		Physics2D.IgnoreLayerCollision(9,10,false);
		health = GameManager.instance.p_HP;
		jumpForce = GameManager.instance.p_Jump;
		speed = GameManager.instance.p_Speed;
		weaponType = 1;

//		HPLABEL.text = health.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
		grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - gizmoX, transform.position.y - gizmoY),
			new Vector2(transform.position.x + gizmoX, transform.position.y -gizmoY), groundLayer);

		if (grounded&&Input.GetKeyDown(KeyCode.Z))
		{
			Jumping = true;
			jumpCounter = jumpTime;
			rb2d.velocity = Vector2.up*jumpForce;
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
		moveInput = Input.GetAxis("Horizontal");
		rb2d.velocity = new Vector2(moveInput*speed,rb2d.velocity.y);
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
	
	
	

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0,1,0,0.5f);
		Gizmos.DrawCube(new Vector2(transform.position.x,transform.position.y-gizmoY),new Vector2(gizmoWidth,gizmoX-gizmoHeight));
	}

	void Flip()
	{
        facingRight = !facingRight;
        shootTrans.Rotate(0f,180f,0f);
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}	
