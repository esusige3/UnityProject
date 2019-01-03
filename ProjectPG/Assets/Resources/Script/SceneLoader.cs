using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnEnable()
	{
		
		SceneManager.sceneLoaded += OnLevelLoadingFinished;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelLoadingFinished;
	}

	void OnLevelLoadingFinished(Scene scene, LoadSceneMode mode)
	{
		
	}

	void gmChecker()//이번 스테이지에 게임 매니저가 존재하는가?
	{
		
	}
}
