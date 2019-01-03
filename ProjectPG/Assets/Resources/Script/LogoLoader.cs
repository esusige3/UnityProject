using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoLoader : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(logoLoading());
	}

	IEnumerator logoLoading()
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("MainScreen");
	}
}
