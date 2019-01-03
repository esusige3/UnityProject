using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy_Patrol : MonoBehaviour
{
	public LayerMask enemyMask;
	private float Speed = 1f;
	private Rigidbody2D rb2;
	private Transform _transform;
	private float myWidth;
	private bool isGround;
	private bool isBlocked;
	
	// Use this for initialization
	void Start ()
	{
		//damage = gameObject.GetComponent<UnitState>().power;
		
		_transform = gameObject.transform;
		rb2 = this.GetComponent<Rigidbody2D>();
		myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector2 lineCastPos = _transform.position - _transform.right * myWidth;
		//Debug.DrawLine(lineCastPos, lineCastPos+Vector2.down);
		isGround = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
		isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - _transform.right.toVector2()*.02f, enemyMask);
		if (!isGround||isBlocked)
		{
			Vector3 currRot = _transform.eulerAngles;
			currRot.y += 180;
			_transform.eulerAngles = currRot;
		}
		Vector2 vel = rb2.velocity;
		vel.x = -_transform.right.x * Speed;
		rb2.velocity = vel;
	}
}
