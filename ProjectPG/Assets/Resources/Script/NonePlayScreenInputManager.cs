using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonePlayScreenInputManager : MonoBehaviour {

	// Use this for initialization
	[SerializeField] private string LinkedStageName;
	[SerializeField]private bool hasCount;

	void Start ()
	{
		StartCoroutine(semiUpdate());
	}

	IEnumerator semiUpdate()
	{
		while (true)
		{if (Input.GetKeyDown(KeyCode.Space))
			{
					Debug.Log("Game Will Begin after 3s");
					for (int i = 0; i < 3; i++)
					{
						Debug.Log(i);
						yield return new WaitForSeconds(1f);
					}
					SceneManager.LoadScene(LinkedStageName);
				
		
			}
			yield return null;
		}
	}
	
}
