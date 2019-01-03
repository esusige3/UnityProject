using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityScript.Steps;

public class GameManager : MonoBehaviour
{
	
	public static GameManager instance = null;
	[SerializeField] private GameObject PlayerPrefab;

	UniStateLoader defaltStateSet = new UniStateLoader();
	
	//플레이어 스탯 임시저장소
	public float p_HP;
	public float p_Speed;
	public float p_Jump; 
	public float p_Power;
	
	private int p_ExtraLife = 3;

	public void structPlayerState(float HP, float Speed, float Jump)
	{
		this.p_HP    = HP;
		this.p_Speed = Speed;
		this.p_Jump  = Jump;
	}

	void ResetPlayerState()
	{
		Debug.Log("플레이어 스탯 초기화 진행");
		p_HP    = defaltStateSet.Player_HP;
		p_Speed = defaltStateSet.Player_Speed;
		p_Jump  = defaltStateSet.Player_Jump;
		p_Power = defaltStateSet.PlayerDamage_m;
		
	}
	
	private void Awake()
	{
		ResetPlayerState();
		instance = this;
		DontDestroyOnLoad(gameObject);	
	}

	//Start와 Update는 없다.
	
	private void OnEnable()
	{
		Debug.Log("플레이어 목숨>> "+p_ExtraLife);
		Debug.Log("OnEnable is called");
		SceneManager.sceneLoaded += OnLevelLoadingFinished;
	}
	private void OnDisable()
	{
		Debug.Log("OnDisable is called");
		SceneManager.sceneLoaded -= OnLevelLoadingFinished;
	}

	void OnLevelLoadingFinished(Scene scene, LoadSceneMode mode)
	{
		gmChecker();
		
	}

	void GeneratePlayer(Transform genTrans)
	{
		Instantiate(PlayerPrefab, genTrans.position, Quaternion.identity);
		UImanager._instanceUiManager.ChangeHpLabel((int)p_HP);
		Debug.Log("Player Generated!");
		
	}

	public void LoadNextLevel()//재시작, 또는 게임오버 시 씬을 넘어가는데 있어서의 모든 처리를 담당할 예정이다,
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	void gmChecker()
	{
		if (GameObject.FindGameObjectWithTag("StartPoint") != null)//플레이용 씬이 아닐떄!
		{
			GeneratePlayer(GameObject.FindGameObjectWithTag("StartPoint").transform);
	
			
		}
		else
		{	
			ResetPlayerState();
		}
	}
	public void DecreaseExtraLife()
	{
		if (p_ExtraLife == 0)
		{
			SceneManager.LoadScene("GameOver");
			p_ExtraLife = 3;
		}
		else
		{
			p_ExtraLife -= 1;
			p_HP = defaltStateSet.Player_HP;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
       
    }

    

	private void OnApplicationQuit()
	{
		Destroy(this);
	}
}
