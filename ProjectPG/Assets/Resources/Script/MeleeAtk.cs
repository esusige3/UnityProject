using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Steps;

public class MeleeAtk : MonoBehaviour
{
	private float timeBtwAtk;
	private float startBtwAtk;
	
	[SerializeField]float attackDmg;
	WaitForSeconds atkGap = new WaitForSeconds(0.5f);
    WaitForSeconds atkModeGap = new WaitForSeconds(1f);
	[SerializeField] private Transform atkPos;
	[SerializeField]private LayerMask enemyLayer;
	[SerializeField]private float atkRange;

	[SerializeField]private int atkMode;//3개의 모션을 담당할 모드 변수

    

	private CameraLogic sceneCamera;

    IEnumerator gapCnt;



    // Use this for initialization
    void Start ()
	{
        gapCnt = AtkModeCountTimer();
        atkMode = 0;
		attackDmg = GameManager.instance.p_Power;
	}

    void OnEnable()
    {
        StartCoroutine(AttackLogic());
    }

    // Update is called once per frame
    IEnumerator AttackLogic()
	{
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				DoAttack();
				yield return atkGap;
			}
			yield return null;
		}
	}
    IEnumerator AtkModeCountTimer()
    {
        yield return atkModeGap;
        atkMode = 0;//모드 초기화
        Debug.Log("모드 초기화 완료");
    }



	private void DoAttack()
	{
		Collider2D[] enemiseToDmg = Physics2D.OverlapCircleAll(atkPos.position, atkRange, enemyLayer);
		
		for (int i = 0; i < enemiseToDmg.Length; i++)
		{
			if (i == 0)
			{
				
                if (atkMode == 3)
                {
                    atkMode = 0;
                    StopCoroutine(gapCnt);
                }
                else
                {
                    atkMode++;
                    StopCoroutine(gapCnt);
                    gapCnt = AtkModeCountTimer();
                    StartCoroutine(gapCnt);
                    
                }
                CameraLogic._instance_camera.zooming();
			}
			//Debug.Log("atkMode = "+atkMode);
			enemiseToDmg[i].GetComponent<EnemyUnit>().takeDmg(attackDmg);	
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(atkPos.position,atkRange);
	}
}
