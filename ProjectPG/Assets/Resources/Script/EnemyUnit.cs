using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

	private float Hp=20;

	private float Sheild = 30;

	private float Damage=30;

	public void takeDmg(float dmg)
	{
		if (Sheild >0)
		{
			Sheild -= dmg;
			//Debug.Log("실드 잔량>> "+Sheild);
			//Debug.Log("받은 DMG>> "+dmg);
		}
		else
		{
			Hp -= dmg;
			//Debug.Log("체력 잔량>> "+Hp);
			if (Hp <= 0)
			{
				Dead();
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerController>().TakeDamage(Damage);
			//Debug.Log("플레이어의 접촉 발생!");
		}
	}

	void Dead()
	{
		//Debug.Log("Enemy 사망");
		gameObject.SetActive(false);
	}
}
