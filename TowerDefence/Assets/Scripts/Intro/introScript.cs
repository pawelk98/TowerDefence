using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class introScript : MonoBehaviour {

	public bool skipIntro = false;
	public bool gameIsAbleToStart = true;
	public int howLongVisibleIntroInSeconds = 5;


	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		StartCoroutine (LoadLevelAfterDelay ());
	}

	IEnumerator LoadLevelAfterDelay()
	{
		if(skipIntro)
			SceneManager.LoadScene ("Scenes/game");

		yield return new WaitForSeconds(howLongVisibleIntroInSeconds);

		if (gameIsAbleToStart)
			SceneManager.LoadScene ("Scenes/game");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
